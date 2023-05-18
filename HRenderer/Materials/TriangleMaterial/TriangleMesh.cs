using HRenderer.Core;

namespace HRenderer.Materials.TriangleMaterial; 

public class TriangleMesh: Mesh {
	public TriangleMesh(): base(new[] {
		new VertexFormat("position",  4),
		new VertexFormat("color", 4)
	}) {
		this._vbo = new float[] {
			-1, -1, 0, 1, 1, 0, 0, 1,
			1, -1, 0, 1, 0, 1, 0, 1,
			0, 1, 0, 1, 0, 0, 1, 1
		};
		this._ibo = new uint[] {0, 1, 2};
	}	
}