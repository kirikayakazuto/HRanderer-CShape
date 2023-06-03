using HRenderer.Core;

namespace HRenderer.Materials.ModelMaterial; 

public class ModelMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/african/african_head_diffuse.png");
		var mesh = new ModelMesh();
		var shader = new ModelShader();
		var normalTexture = new Texture("./Assets/african/african_head_nm.png");
		var m = new Material("model", mesh, shader, texture) {useFaceCulling = true};
		m.uniformData.Textures["normalTexture"] = normalTexture;
		return m;
	}
}