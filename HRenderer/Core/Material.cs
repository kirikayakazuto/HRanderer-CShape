using HRenderer.Common;

namespace HRenderer.Core {
    /**
     * 包含所有需要渲染的信息
     */
    public class Material {
        public readonly Mesh mesh;
        public readonly Shader shader;
        public readonly UniformData uniformData = new UniformData();
        
        // 开启深度写入
        public bool useDepthWrite = true;
        // 开启背面剔除
        public bool useFaceCulling = false;
        // 开启模版写入
        public bool useStencilWrite = false;

        public Material(Mesh mesh, Shader shader, Texture? texture = null) {
            this.mesh = mesh;
            this.shader = shader;
            if (texture != null) {
                this.uniformData.Textures["mainTexture"] = texture;    
            }
            
        }
    }
}