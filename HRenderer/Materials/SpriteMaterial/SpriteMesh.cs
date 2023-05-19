using HRenderer.Core;

namespace HRenderer.Materials.SpriteMaterial {
    public class SpriteMesh: Mesh {
        public SpriteMesh(): base(new[] {
            new VertexFormat("position",  4),
            new VertexFormat("uv",  2)
        }) {
            this._vbo = new float[] {
                -2, -2, 0, 1, 0, 0,
                2, -2, 0, 1, 1, 0,
                2, 2, 0, 1, 1, 1,
                -2, 2, 0, 1, 0, 1
            };
            this._ibo = new uint[] {0, 1, 2, 0, 2, 3};
        }
    }
    public class SpriteMesh2: Mesh {
        public SpriteMesh2(): base(new[] {
            new VertexFormat("position",  4),
            new VertexFormat("uv",  2)
        }) {
            this._vbo = new float[] {
                -2, -2, -1, 1, 0, 0,
                2, -2, -1, 1, 1, 0,
                2, 2, -1, 1, 1, 1,
                -2, 2, -1, 1, 0, 1
            };
            this._ibo = new uint[] {0, 1, 2, 0, 2, 3};
        }
    }
    
    public class SpriteMesh3: Mesh {
        public SpriteMesh3(): base(new[] {
            new VertexFormat("position",  4),
            new VertexFormat("uv",  2)
        }) {
            this._vbo = new float[] {
                -1, -1, 1, 1, 0, 0,
                1, -1, 1, 1, 1, 0,
                1, 1, 1, 1, 1, 1,
                -1, 1, 1, 1, 0, 1
            };
            this._ibo = new uint[] {0, 1, 2, 0, 2, 3};
        }
    }
}