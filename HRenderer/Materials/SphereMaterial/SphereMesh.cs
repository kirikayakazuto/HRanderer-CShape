using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SphereMaterial; 

public class SphereMesh: Mesh {

	private Vector4[] _vector4s = new Vector4[] {
		// 
		Vector4.Create(0, 1, 0, 0),
		Vector4.Create(0, 0, 1, 0),
		Vector4.Create(1, 0, 0, 0),
		//
		Vector4.Create(0, -1, 0, 0),
		Vector4.Create(1, 0, 0, 0),
		Vector4.Create(0, 0, 1, 0),
		//
		Vector4.Create(0, 1, 0, 0),
		Vector4.Create(-1, 0, 0, 0),
		Vector4.Create(0, 0, 1, 0),
		//
		Vector4.Create(0, -1, 0, 0),
		Vector4.Create(0, 0, 1, 0),
		Vector4.Create(-1, 0, 0, 0),
		//
		Vector4.Create(0, 1, 0, 0),
		Vector4.Create(1, 0, 0, 0),
		Vector4.Create(0, 0, -1, 0),
		//
		Vector4.Create(0, 1, 0, 0),
		Vector4.Create(0, 0, -1, 0),
		Vector4.Create(-1, 0, 0, 0),
		//
		Vector4.Create(0, -1, 0, 0),
		Vector4.Create(-1, 0, 0, 0),
		Vector4.Create(0, 0, -1, 0),
		//
		Vector4.Create(0, -1, 0, 0),
		Vector4.Create(0, 0, -1, 0),
		Vector4.Create(1, 0, 0, 0),
	};
	
	public SphereMesh(): base(new[] {
		new VertexFormat("a_position",  4),
		new VertexFormat("a_normal",  4),
		new VertexFormat("a_uv",  2),
	}) {
		var n = 10;
		int nn = n * 4;
		int vertexNum = (nn * nn / 16) * 24;
		var vertices = new Vector4[vertexNum];
		for (var i = 0; i < 24; i += 3) {
			for (var p = 0; p < n; p++) {
				
			}
		}
	}
}