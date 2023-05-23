using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 

public class CubeMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/container2/container2_diffuse.png");
		var mesh = new CubeMesh();
		var shader = new CubeShader();
		
		var specular = new Texture("./Assets/container2/container2_specular.png");
		var m = new Material(mesh, shader, texture);
		m.uniformData.Textures["specularTexture"] = specular;
		return m;

	}
}