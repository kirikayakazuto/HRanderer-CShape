using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SphereMaterial; 

public class SphereMesh: Mesh {
	
	public SphereMesh(): base(new[] {
		new VertexFormat("position",  4)
	}) {
		this.CreateIcosohedron(1);
		this._vbo = new float[this.vertices.Count * 4];
		this._ibo = new uint[this.indices.Count];
		
		for (var i = 0; i < this.vertices.Count; i++) {
			var idx = i * 4;
			this._vbo[idx] = (float)this.vertices[i].x;
			this._vbo[idx+1] = (float)this.vertices[i].y;
			this._vbo[idx+2] = (float)this.vertices[i].z;
			this._vbo[idx+3] = (float)this.vertices[i].w;
		}
		
		for (var i = 0; i < this.indices.Count; i++) {
			this._ibo[i] = (uint)this.indices[i];
		}
	}


	private List<Vector4> vertices = new List<Vector4>();
	private List<int> indices = new List<int>();
	public void SubdivideTriangle(Vector4 a, Vector4 b, Vector4 c, int depth) {
		if (depth == 0) {
			this.indices.Add(this.vertices.Count);
			this.vertices.Add(a);
			
			this.indices.Add(this.vertices.Count);
			this.vertices.Add(b);
			
			this.indices.Add(this.vertices.Count);
			this.vertices.Add(c);
			return;
		}

		var ab = a.Add(b).MulSelf(1 / 2.0);
		var ac = a.Add(c).MulSelf(1 / 2.0);
		var bc = b.Add(c).MulSelf(1 / 2.0);

		this.SubdivideTriangle(a, ab, ac, depth - 1);
		this.SubdivideTriangle(b, ab, bc, depth - 1);
		this.SubdivideTriangle(c, ac, bc, depth - 1);
		this.SubdivideTriangle(ab, bc, ac, depth - 1);
	}

	public void CreateIcosohedron(int subdivisions) {
		var t = (1 + Math.Sqrt(5)) / 2.0;
		this.vertices.Add(Vector4.Create(-1, t, 0, 1));
		this.vertices.Add(Vector4.Create(1, t, 0, 1));
		
		this.vertices.Add(Vector4.Create(-1, -t, 0, 1));
		this.vertices.Add(Vector4.Create(1, -t, 0, 1));
		
		this.vertices.Add(Vector4.Create(0, -1, t, 1));
		this.vertices.Add(Vector4.Create(0, 1, t, 1));
		
		this.vertices.Add(Vector4.Create(0, -1, -t, 1));
		this.vertices.Add(Vector4.Create(0, 1, -t, 1));
		
		this.vertices.Add(Vector4.Create(t, 0, -1, 1));
		this.vertices.Add(Vector4.Create(t, 0, 1, 1));
		
		this.vertices.Add(Vector4.Create(-t, 0, -1, 1));
		this.vertices.Add(Vector4.Create(-t, 0, 1, 1));
		
		this.indices.AddRange(new int[] {
			0, 11, 5,
			0, 5, 1,
			0, 1, 7,
			0, 7, 10,
			0, 10, 11,
			1, 5, 9,
			5, 11, 4,
			11, 10, 2,
			10, 7, 6,
			7, 1, 8,
			3, 9, 4,
			3, 4, 2,
			3, 2, 6,
			3, 6, 8,
			3, 8, 9,
			4, 9, 5,
			2, 4, 11,
			6, 2, 10,
			8, 6, 7,
			9, 8, 1
		});

		for (var i = 0; i < subdivisions; i++) {
			var count = this.indices.Count;
			for (var j = 0; j < count; j += 3) {
				var a = this.vertices[this.indices[j]];
				var b = this.vertices[this.indices[j+1]];
				var c = this.vertices[this.indices[j+2]];
				SubdivideTriangle(a, b, c, 1);
				this.indices[j] = vertices.Count - 3;
				this.indices[j + 1] = vertices.Count - 2;
				this.indices[j + 2] = vertices.Count - 1;
			}
		}
	}
}