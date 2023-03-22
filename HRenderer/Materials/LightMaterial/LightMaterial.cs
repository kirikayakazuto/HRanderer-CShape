using HRenderer.Core;

namespace HRenderer.Materials.LightMaterial; 

public class LightMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/001.jpg");
		var mesh = new LightMesh();
		var shader = new LightShader();
		return new Material(texture, mesh, shader);
	}
}