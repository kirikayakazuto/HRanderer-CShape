using HRenderer.Core;

namespace HRenderer.DrawTriangle {
    public class TextureMesh: Mesh {
        public TextureMesh(VertexFormat[] attribInfo): base(attribInfo) {
            var attrib = new[] {
                new VertexFormat("position",  4),
                new VertexFormat("uv",  2)
            };
        }
    }
}