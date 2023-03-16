using System;
using System.Collections.Generic;
using HRenderer.Common;

namespace HRenderer.Core {
    /**
     * 包含所以需要渲染的信息
     */
    public class Material {
        public Mesh mesh;
        public Shader shader;
        
        public Dictionary<string, Texture> uniformTextures = new Dictionary<string, Texture>();
        public Dictionary<string, Vector4> uniformVec4 = new Dictionary<string, Vector4>();
        public Dictionary<string, float> uniformFloats = new Dictionary<string, float>();

        public bool useFaceCulling = false;

        public Material(Texture texture, Mesh mesh, Shader shader) {
            this.mesh = mesh;
            this.shader = shader;
            this.uniformTextures["mainTexture"] = texture;
        }

    }
}