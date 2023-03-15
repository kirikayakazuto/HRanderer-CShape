using System.Collections.Generic;
using HRenderer.Common;

namespace HRenderer.Core {
    
    public abstract class Shader {

        // 摄像机相关
        public Matrix4 view;
        public Matrix4 projection;

        public Dictionary<string, float> uniformFloats = new Dictionary<string, float>();
        public Dictionary<string, Texture> uniformTextures = new Dictionary<string, Texture>();
        public Dictionary<string, Vector4> uniformVec4 = new Dictionary<string, Vector4>();

        public void AddUniforms(Dictionary<string, float> dictionary) {
            foreach (var keyValuePair in dictionary) {
                this.uniformFloats[keyValuePair.Key] = keyValuePair.Value;
            }
        }
        public void AddUniforms(Dictionary<string, Vector4> dictionary) {
            foreach (var keyValuePair in dictionary) {
                this.uniformVec4[keyValuePair.Key] = keyValuePair.Value;
            }
        }
        public void AddUniforms(Dictionary<string, Texture> dictionary) {
            foreach (var keyValuePair in dictionary) {
                this.uniformTextures[keyValuePair.Key] = keyValuePair.Value;
            }
        }
        
        // 差值数据
        public readonly Dictionary<string, Vector4> varyVec4Dict = new Dictionary<string, Vector4>();
        public readonly Dictionary<string, Vector2> varyVec2Dict = new Dictionary<string, Vector2>();
        
        /**
         * 顶点着色器
         */
        public abstract Vector4 VertexShading(in Dictionary<string, Vector4> vector4s, in Dictionary<string, Vector2> vector2s);
        
        /**
         * 片元着色器
         */
        public abstract Vector4 FragShading();
        
        protected Vector4 Texture2D(Texture t, Vector2 uv) {
            var color = t.Sample(uv.x, uv.y);
            var v = Vector4.Create(color.r / 256f, color.g / 256f, color.b / 256f, color.a / 256f);
            // 回收
            Color.Return(color);
            return v;
        }
    }
}