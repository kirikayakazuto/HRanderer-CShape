using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.DrawModel; 

public class ModelMesh: Mesh {
    public ModelMesh(): base(new[] {
        new VertexFormat("position",  4),
        new VertexFormat("uv",  2)
    }) {
        // this._vbo
        ObjectParser.ParseObj("./Assets/african/african_head.obj");
        
        this._vbo = new float[ObjectParser.VertexCount * this.stride];
        for (var i = 0; i < ObjectParser.VertexCount; i++) {
            var idx = (int)(i * this.stride);

            var vIdx = (int) (i * 3);
            this._vbo[idx] = ObjectParser.v[vIdx];
            this._vbo[idx + 1] = ObjectParser.v[vIdx+1];
            this._vbo[idx + 2] = ObjectParser.v[vIdx+2];
            this._vbo[idx + 3] = 1;

            this._vbo[idx + 4] = ObjectParser.vt[vIdx];
            this._vbo[idx + 5] = ObjectParser.vt[vIdx+1];
        }

        this._ibo = new uint[ObjectParser.f.Count];
        for (var i = 0; i < this._ibo.Length; i++) {
            this._ibo[i] = ObjectParser.f[i] - 1;
        }
    }
}