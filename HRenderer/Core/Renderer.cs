using System;
using System.Collections.Generic;
using HRenderer.Common;

namespace HRenderer.Core {
    /**
     * 渲染器
     */
    public class Renderer {
        // size
        private int _width = 0;
        private int _height = 0;

        // 相机
        public Camera camera;
        // 画布
        public FrameBuffer frameBuffer;
        // 定时器
        private System.Timers.Timer aTimer;

        public Material[] materials;

        public Renderer(int width, int height) {
            this._width = width;
            this._height = height;
            
            this.camera = new Camera(width, height);
            this.frameBuffer = new FrameBuffer(width, height);
            
            // 初始化定时器
            this.InitTimer();
        }

        private void InitTimer() {
            this.aTimer = new System.Timers.Timer(500);
            this.aTimer.Elapsed += this.Loop;
            this.aTimer.AutoReset = true;
            this.aTimer.Enabled = true;
            this.aTimer.Start();
        }

        private void Loop(Object source, System.Timers.ElapsedEventArgs e) {
            this.aTimer.Stop();
            this.Draw();
            this.aTimer.Start();
        }
        
        /**
         * 渲染
         */
        private void Draw() {
            foreach (var material in this.materials) {
                this.DrawMaterial(material);
            }
        }

        public void DrawMaterial(Material material) {
            var mesh = material.mesh;
            var shader = material.shader;
            var texture = material.texture;
            var indices = mesh.indiceBuffer;
            for (var i = 0; i < indices.Length; i += 3) {
                var v1 = mesh.stride * indices[i];
                var v2 = mesh.stride * indices[i+1];
                var v3 = mesh.stride * indices[i+2];
                
                this.DrawTriangle(material, v1, v2, v3);
            }
        }

        private readonly Dictionary<string, Vector4> _tmpAttribs1 = new Dictionary<string, Vector4>();
        private readonly Dictionary<string, Vector4> _tmpAttribs2 = new Dictionary<string, Vector4>();
        private readonly Dictionary<string, Vector4> _tmpAttribs3 = new Dictionary<string, Vector4>();
        private void DrawTriangle(Material material, uint v1, uint v2, uint v3) {
            var mesh = material.mesh;
            var shader = material.shader;
            var texture = material.texture;
            
            this._tmpAttribs1.Clear();
            this._tmpAttribs2.Clear();
            this._tmpAttribs3.Clear();

            var attribNames = mesh.GetAttribNames(); 
            mesh.GetVertexAttribs(v1, this._tmpAttribs1);
            mesh.GetVertexAttribs(v2, this._tmpAttribs1);
            mesh.GetVertexAttribs(v3, this._tmpAttribs1);
            
            // vertex shading
            var position1 = shader.VertexShading(this._tmpAttribs1);
            var position2 = shader.VertexShading(this._tmpAttribs1);
            var position3 = shader.VertexShading(this._tmpAttribs1);

            var near = this.camera.near;
            var far = this.camera.far;
            var z1 = Utils.GetDepth(near, far, position1.z);
            var z2 = Utils.GetDepth(near, far, position2.z);
            var z3 = Utils.GetDepth(near, far, position3.z);

            position1.Homogenenize();
            position2.Homogenenize();
            position3.Homogenenize();

            position1.Transform(this.camera.viewPort);
            position2.Transform(this.camera.viewPort);
            position3.Transform(this.camera.viewPort);

            var pos1 = Vector2.Create(position1.x, position1.y); 
            var pos2 = Vector2.Create(position2.x, position2.y);
            var pos3 = Vector2.Create(position3.x, position3.y);

            
            var bound = Utils.GetBoundingBox(position1, position2, position3);
            var p = Vector2.Create();
            for (var y = Math.Max(bound.minY, 0); y < Math.Min(bound.maxY, this._height); y++) {
                p.y = y + 0.5f;
                for (var x = Math.Max(bound.minX, 0); x < Math.Min(bound.maxX, this._width); x++) {
                    p.x = x + 0.5f;
                    
                    var barycentric = Utils.GetBarycentric(p, pos1, pos2, pos3);
                    if(barycentric.x < 0 || barycentric.y < 0 || barycentric.z < 0) continue;

                    // 计算差值
                    foreach (var name in attribNames) {
                        var v = Utils.GetInterpVec4(this._tmpAttribs1[name], this._tmpAttribs2[name], this._tmpAttribs3[name], barycentric);
                        if (shader.dictionary.ContainsKey(name)) {
                            shader.dictionary[name] = v;
                        } else {
                            shader.dictionary.Add(name, v);    
                        }
                    }
                    var z = Utils.GetInterpValue3(z1, z2, z3, barycentric.x, barycentric.y, barycentric.z);

                    shader.texture = texture;
                    var color = shader.FragShading();
                    this.frameBuffer.SetColor(x, y, color, z);
                }
            }
        }

        public void Clear() {
            this.aTimer.Stop();
            this.aTimer.Close();
        }

    }
}