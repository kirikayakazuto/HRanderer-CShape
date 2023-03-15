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
        // 定时器
        private System.Timers.Timer aTimer;
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
            this.frameBuffer.Clear();
            
            this._time += (float)dt;
            foreach (var material in this.materials) {
                this.DrawMaterial(material);
            }
        }
        
        public void DrawMaterial(Material material) {
            var mesh = material.mesh;
            var shader = material.shader;
            var texture = material.texture;

            shader.view = this.camera.viewMat;
            shader.projection = this.camera.OrthographicProjection;
            
            shader.uniformFloats["time"] = this._time;
            
            var indices = mesh.indiceBuffer;
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
            var texture = material.texture;
            
            mesh.GetVertexAttribs(v1, this._tmpVec4Attribs1, this._tmpVec2Attribs1);
            mesh.GetVertexAttribs(v2, this._tmpVec4Attribs2, this._tmpVec2Attribs2);
            mesh.GetVertexAttribs(v3, this._tmpVec4Attribs3, this._tmpVec2Attribs3);
            
            // vertex shading
            var position1 = shader.VertexShading(this._tmpVec4Attribs1, this._tmpVec2Attribs1);
            var position2 = shader.VertexShading(this._tmpVec4Attribs2, this._tmpVec2Attribs2);
            var position3 = shader.VertexShading(this._tmpVec4Attribs3, this._tmpVec2Attribs3);
            
            var near = this.camera.near;
            var far = this.camera.far;
            var z1 = Utils.GetDepth(near, far, position1.z);
            var z2 = Utils.GetDepth(near, far, position2.z);
            var z3 = Utils.GetDepth(near, far, position3.z);

            position1.Homogenenize();
            position2.Homogenenize();
            position3.Homogenenize();
            
            position1.TransformSelf(this.camera.viewPortMat);
            position2.TransformSelf(this.camera.viewPortMat);
            position3.TransformSelf(this.camera.viewPortMat);
            
            var pos1 = Vector2.Create(position1.x, position1.y); 
            var pos2 = Vector2.Create(position2.x, position2.y);
            var pos3 = Vector2.Create(position3.x, position3.y);
            
            // 设置uniform
            shader.texture = texture;
            
            var bound = Utils.GetBoundingBox(position1, position2, position3);
            var p = Vector2.Create();
            for (var y = Math.Max(bound.minY, 0); y < Math.Min(bound.maxY, this._height); y++) {
                p.y = y + 0.5f;
                for (var x = Math.Max(bound.minX, 0); x < Math.Min(bound.maxX, this._width); x++) {
                    p.x = x + 0.5f;
                    // 重心差值
                    var barycentric = Utils.GetBarycentric(p, pos1, pos2, pos3);
                    if(barycentric.x < 0 || barycentric.y < 0 || barycentric.z < 0) continue;
                    
                    // 计算attribInfo差值
                    this.ComputeVectorVarying(mesh.attribInfo, shader, barycentric);
                    // 计算z值
                    var z = Utils.GetInterpValue3(z1, z2, z3, barycentric.x, barycentric.y, barycentric.z);
                    
                    var color = shader.FragShading();
                    
                    this.frameBuffer.SetColor(x , y, color, z);
                }
            }
            
            // 清理attribs
            this.ClearAttribs();
        }

        public void Clear() {
            this.aTimer.Stop();
            this.aTimer.Close();
        }

        private void ComputeVectorVarying(IEnumerable<VertexFormat> attribInfo, Shader shader, Vector4 barycentric) {
            foreach (var vertexFormat in attribInfo) {
                var name = vertexFormat.name;
                switch (vertexFormat.num) {
                    case 4:
                        var vec4 = Utils.GetInterpVec4(this._tmpVec4Attribs1[name], this._tmpVec4Attribs2[name], this._tmpVec4Attribs3[name], barycentric);
                        shader.varyVec4Dict[name] = vec4;
                        break;
                    case 2:
                        var vec2 = Utils.GetInterpVec2(this._tmpVec2Attribs1[name], this._tmpVec2Attribs2[name], this._tmpVec2Attribs3[name], barycentric);
                        shader.varyVec2Dict[name] = vec2;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}