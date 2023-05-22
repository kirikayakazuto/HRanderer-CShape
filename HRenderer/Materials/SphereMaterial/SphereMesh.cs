using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SphereMaterial; 

public class SphereMesh: Mesh {
	
	public SphereMesh(): base(new[] {
		new VertexFormat("position",  4),
		new VertexFormat("uv", 2),
		new VertexFormat("normal", 4)
	}) {
		
		this.CreateIcosohedron(3);
		
		
		this._vbo = new float[this.vertices.Count * 10];
		this._ibo = new uint[this.indices.Count];
		
		for (var i = 0; i < this.vertices.Count; i++) {
			var idx = i * 10;
			this._vbo[idx] = (float)this.vertices[i].x;
			this._vbo[idx+1] = (float)this.vertices[i].y;
			this._vbo[idx+2] = (float)this.vertices[i].z;
			this._vbo[idx+3] = 1;
			
			this._vbo[idx + 4] = (float)this.uvs[i].x;
			this._vbo[idx + 5] = (float)this.uvs[i].y;
			
			this._vbo[idx+6] = (float)this.normals[i].x;
			this._vbo[idx+7] = (float)this.normals[i].y;
			this._vbo[idx+8] = (float)this.normals[i].z;
			this._vbo[idx+9] = 1;
		}
		
		for (var i = 0; i < this.indices.Count; i++) {
			this._ibo[i] = (uint)this.indices[i];
		}
	}


	private readonly List<Vector4> vertices = new List<Vector4>();
	private readonly List<Vector2> uvs = new List<Vector2>();
	private readonly List<Vector4> normals = new List<Vector4>();
	private readonly List<int> indices = new List<int>();

	public void CreateIcosohedron(int subdivisions) {
		const double t = 1.61803;
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

		var indexs = new int[] {
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
		};
		
		foreach (var vector4 in this.vertices) {
			vector4.NormalizeSelf();
		}
		
		for (var i = 0; i < subdivisions; i++) {
			var newIndices = new List<int>();
			for (var j = 0; j < indexs.Length; j += 3) {
				var v1 = indexs[j];
				var v2 = indexs[j+1];
				var v3 = indexs[j+2];

				var a = this.GetMiddlePoint(v1, v2);
				var b = this.GetMiddlePoint(v2, v3);
				var c = this.GetMiddlePoint(v3, v1);
				
				newIndices.AddRange(new int[] { v1, a, c });
				newIndices.AddRange(new int[] { v2, b, a });
				newIndices.AddRange(new int[] { v3, c, b });
				newIndices.AddRange(new int[] { a, b, c });
			}
			indexs = newIndices.ToArray();
		}
		this.indices.AddRange(indexs);
		
		// 为接缝处做额外处理
		for (var i = 0; i < this.indices.Count; i += 3) {
			var v1 = this.indices[i];
			var v2 = this.indices[i+1];
			var v3 = this.indices[i+2];

			if (this.vertices[v1].Sub(this.vertices[v2]).GetLength() > 0.5) {		// 接缝处
				
			}
		}
		
		foreach (var vector4 in this.vertices) {
			this.uvs.Add(Vector2.Create(Math.Atan2(vector4.x, vector4.z) / (2.0f * Math.PI) + 0.5f, Math.Asin(vector4.y) / Math.PI + 0.5f));
			// Console.WriteLine(this.uvs[this.uvs.Count-1].x);
		}
		
		foreach (var vector4 in this.vertices) {
			this.normals.Add(vector4.Normalize());
		}
	}

	private int GetMiddlePoint(int v1, int v2) {
		var p1 = this.vertices[v1];
		var p2 = this.vertices[v2];
		var mid = p1.Add(p2).MulSelf(0.5).NormalizeSelf();
		this.vertices.Add(mid);
		return this.vertices.Count - 1;
	}
}