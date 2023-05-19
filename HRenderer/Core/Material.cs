using HRenderer.Common;

namespace HRenderer.Core {
    /**
     * 包含所有需要渲染的信息
     */
    public class Material {
        public readonly Mesh mesh;
        public readonly Shader shader;
        public readonly UniformData uniformData = new UniformData();
        
        public bool useFaceCulling = false;
        public bool writeStencil = false;

        public Material(Texture texture, Mesh mesh, Shader shader) {
            this.mesh = mesh;
            this.shader = shader;
            this.uniformData.Textures["mainTexture"] = texture;
        }
    }
}