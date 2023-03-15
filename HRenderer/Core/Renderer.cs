using System;
using System.Collections.Generic;
using HRenderer.Common;

namespace HRenderer.Core {
    /**
     * 渲染器
     */
    public class Renderer {
        // size
        private readonly int _width = 0;
        private readonly int _height = 0;
        // 相机
        public Camera camera;
        // 画布
        public FrameBuffer frameBuffer;
        // 待渲染的材质
        public List<Material> materials = new List<Material>();
        
        
        public Renderer(int width, int height) {
            this._width = width;
            this._height = height;
            
            this.camera = new Camera(width, height);
            this.frameBuffer = new FrameBuffer(width, height);
        }

        public void AddMaterial(Material material) {
            this.materials.Add(material);
        }

        private float _time = 0;
        public void Render(double dt) {
            Vector4.newCount = 0;
            this.frameBuffer.Clear();
            
            this._time += (float)dt;
            foreach (var material in this.materials) {
                this.DrawMaterial(material);
            }
        }
        
        public void DrawMaterial(Material material) {
            var mesh = material.mesh;
            var shader = material.shader;

            shader.view = this.camera.viewMat;
            shader.projection = this.camera.OrthographicProjection;
            
            // 设置uniform
            shader.AddUniforms(material.uniformFloats);
            shader.AddUniforms(material.uniformVec4);
            shader.AddUniforms(material.uniformTextures);
            shader.uniformFloats["time"] = this._time;
            
            var indices = mesh.Ibo;
            for (var i = 0; i < indices.Length; i += 3) {
                var v1 = mesh.stride * indices[i];
                var v2 = mesh.stride * indices[i+1];
                var v3 = mesh.stride * indices[i+2];
                
                this.DrawTriangle(material, v1, v2, v3);
            }
        }

        // 缓存 vec4
        private readonly Dictionary<string, Vector4> _tmpVec4Attribs1 = new Dictionary<string, Vector4>();
        private readonly Dictionary<string, Vector4> _tmpVec4Attribs2 = new Dictionary<string, Vector4>();
        private readonly Dictionary<string, Vector4> _tmpVec4Attribs3 = new Dictionary<string, Vector4>();
        // 缓存 vec2
        private readonly Dictionary<string, Vector2> _tmpVec2Attribs1 = new Dictionary<string, Vector2>();
        private readonly Dictionary<string, Vector2> _tmpVec2Attribs2 = new Dictionary<string, Vector2>();
        private readonly Dictionary<string, Vector2> _tmpVec2Attribs3 = new Dictionary<string, Vector2>();

        private void ClearAttribs() {
            this._tmpVec4Attribs1.Clear();
            this._tmpVec4Attribs2.Clear();
            this._tmpVec4Attribs3.Clear();
            
            this._tmpVec2Attribs1.Clear();
            this._tmpVec2Attribs2.Clear();
            this._tmpVec2Attribs3.Clear();
        }
        
        private void DrawTriangle(Material material, uint v1, uint v2, uint v3) {
            var mesh = material.mesh;
            var shader = material.shader;
            
            // 获取顶点信息
            mesh.GetVertexAttribs(v1, this._tmpVec4Attribs1, this._tmpVec2Attribs1);
            mesh.GetVertexAttribs(v2, this._tmpVec4Attribs2, this._tmpVec2Attribs2);
            mesh.GetVertexAttribs(v3, this._tmpVec4Attribs3, this._tmpVec2Attribs3);
            
            // vertex shading
            var position1 = shader.VertexShading(this._tmpVec4Attribs1, this._tmpVec2Attribs1);
            var position2 = shader.VertexShading(this._tmpVec4Attribs2, this._tmpVec2Attribs2);
            var position3 = shader.VertexShading(this._tmpVec4Attribs3, this._tmpVec2Attribs3);
            
            position1.TransformSelf(this.camera.viewPortMat);
            position2.TransformSelf(this.camera.viewPortMat);
            position3.TransformSelf(this.camera.viewPortMat);
            
            var z1 = position1.w;
            var z2 = position2.w;
            var z3 = position3.w;

            position1.Homogenenize();
            position2.Homogenenize();
            position3.Homogenenize();
            
            var bound = Utils.GetBoundingBox(position1, position2, position3);
            var barycentric = Vector4.Create();
            var p = Vector2.Create();
            for (var y = Math.Max(bound.minY, 0); y < Math.Min(bound.maxY, this._height); y++) {
                p.y = y + 0.5f;
                for (var x = Math.Max(bound.minX, 0); x < Math.Min(bound.maxX, this._width); x++) {
                    p.x = x + 0.5f;
                    // 重心差值
                    Utils.GetBarycentric(p, position1, position2, position3, barycentric);
                    if(barycentric.x < 0 || barycentric.y < 0 || barycentric.z < 0) continue;
                    
                    // 计算z值 线性的z
                    var z = Utils.GetInterpValue3(position1.z, position2.z, position3.z, barycentric.x, barycentric.y, barycentric.z);
                    
                    // 校正透视差值
                    barycentric = Utils.AdjustBarycentric(barycentric, z1, z2, z3);
                    
                    // 计算attribInfo差值
                    this.ComputeShaderVectorVarying(mesh.attribInfo, shader, barycentric);
                    
                    // 计算非线性的z值
                    z = Utils.GetDepth(this.camera.near, this.camera.far, z);
                    
                    // frag shading
                    var color = shader.FragShading();
                    
                    // 输出颜色
                    this.frameBuffer.SetColor(x , y, color, z);
                    Vector4.Return(color);
                }
            }
            
            // 清理attribs
            this.ClearAttribs();
            
            Vector4.Return(position1);
            Vector4.Return(position2);
            Vector4.Return(position3);
        }
        
        private void ComputeShaderVectorVarying(IEnumerable<VertexFormat> attribInfo, Shader shader, in Vector4 barycentric) {
            foreach (var vertexFormat in attribInfo) {
                var name = vertexFormat.name;
                switch (vertexFormat.num) {
                    case 4:
                        var vec4 = shader.varyVec4Dict.ContainsKey(name) ? shader.varyVec4Dict[name] : Vector4.Create(); 
                        Utils.GetInterpVec4(this._tmpVec4Attribs1[name], this._tmpVec4Attribs2[name], this._tmpVec4Attribs3[name], barycentric, vec4);
                        shader.varyVec4Dict[name] = vec4;
                        break;
                    case 2:
                        var vec2 = shader.varyVec2Dict.ContainsKey(name) ? shader.varyVec2Dict[name] : Vector2.Create();
                        Utils.GetInterpVec2(this._tmpVec2Attribs1[name], this._tmpVec2Attribs2[name], this._tmpVec2Attribs3[name], barycentric, vec2);
                        shader.varyVec2Dict[name] = vec2;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}