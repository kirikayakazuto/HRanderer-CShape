using HRenderer.Core;

namespace HRenderer.DrawTriangle {
    public class SpriteMesh: Mesh {
        public SpriteMesh(): base(new[] {
            new VertexFormat("position",  4),
            new VertexFormat("uv",  2)
        }) {
            this._vertexBuffer = new float[] {
                -1, -1, 1, 1, 0, 0,
                1, -1, 1, 1, 1, 0,
                1, 1, 1, 1f, 1, 1,
                -1, 1, 1, 1, 0, 1
            };
            this._indiceBuffer = new uint[] {0, 1, 2, 0, 2, 3};
        }
    }
}