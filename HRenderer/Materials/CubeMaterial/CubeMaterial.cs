using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 

public class CubeMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/container2/container2_diffuse.png");
		var mesh = new CubeMesh();
		var shader = new CubeShader();
		
		var specular = new Texture("./Assets/container2/container2_specular.png");
		var m = new Material(texture, mesh, shader);
		m.uniformTextures["specularTexture"] = specular;
		return m;

	}
}