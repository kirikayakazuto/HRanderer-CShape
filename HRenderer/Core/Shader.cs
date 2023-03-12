using System.Collections.Generic;
using HRenderer.Common;

namespace HRenderer.Core {
    
    public abstract class Shader {

        public Texture texture;
        public abstract Vector4 VertexShading(Dictionary<string, Vector4> dictionary);

        public readonly Dictionary<string, Vector4> dictionary = new Dictionary<string, Vector4>();
        public abstract Vector4 FragShading();

        protected Color Texture2D(Texture t, Vector2 uv) {
            return t.Sample(uv.x, uv.y);
        }
    }
}