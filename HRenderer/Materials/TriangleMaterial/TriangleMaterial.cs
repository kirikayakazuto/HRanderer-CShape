using HRenderer.Core;

namespace HRenderer.Materials.TriangleMaterial;

public class TriangleMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/001.jpg");
		var mesh = new TriangleMesh();
		var shader = new TriangleShader();
		var m = new Material(mesh, shader);
		m.useStencilWrite = true;
		return m;
	}
}