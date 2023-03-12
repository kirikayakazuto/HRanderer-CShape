using System;
using System.Collections.Generic;
using HRenderer.Common;

namespace HRenderer.Core {
    /**
     * 包含所以需要渲染的信息
     */
    public class Material {
        public Texture texture;
        public Mesh mesh;
        public Shader shader;
        // public Dictionary<>

        public Material(Texture texture, Mesh mesh, Shader shader) {
            this.texture = texture;
            this.mesh = mesh;
            this.shader = shader;
        }
        

   
    }
}