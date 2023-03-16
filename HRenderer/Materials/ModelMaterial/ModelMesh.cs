using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.ModelMaterial; 

public class ModelMesh: Mesh {
    public ModelMesh(): base(new[] {
        new VertexFormat("position",  4),
        new VertexFormat("uv",  2)
    }) {
        var objectModel = ObjectParser.ParseObj("./Assets/african/african_head.obj");
        
        this._vbo = new float[objectModel.vertexCount * this.stride];
        for (var i = 0; i < objectModel.vertexCount; i++) {
            var idx = (int)(i * this.stride);

            var vIdx = (int) (i * 3);
            this._vbo[idx] = objectModel.positions[vIdx];
            this._vbo[idx + 1] = objectModel.positions[vIdx+1];
            this._vbo[idx + 2] = objectModel.positions[vIdx+2];
            this._vbo[idx + 3] = 1;

            this._vbo[idx + 4] = objectModel.uvs[vIdx];
            this._vbo[idx + 5] = objectModel.uvs[vIdx+1];
        }

        this._ibo = new uint[objectModel.vertexCount];
        for (var i = 0; i < objectModel.indices.Count; i++) {
            this._ibo[i] = objectModel.indices[i];
        }
    }
}