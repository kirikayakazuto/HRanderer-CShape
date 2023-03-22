using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 

public class CubeMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/001.jpg");
		var mesh = new CubeMesh();
		var shader = new CubeShader();
		return new Material(texture, mesh, shader);
	}
}