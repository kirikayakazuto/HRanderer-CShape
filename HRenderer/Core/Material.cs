namespace HRenderer.Core {
    public class Material {
        public Texture texture;
        public Mesh mesh;
        public Shader shader;

        public Material(Texture texture, Mesh mesh, Shader shader) {
            this.texture = texture;
            this.mesh = mesh;
            this.shader = shader;
        }
    }
}