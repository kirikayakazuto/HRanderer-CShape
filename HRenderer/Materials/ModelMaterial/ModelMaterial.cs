using HRenderer.Core;

namespace HRenderer.Materials.ModelMaterial; 

public class ModelMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/african/african_head_diffuse.png");
		var mesh = new ModelMesh();
		var shader = new ModelShader();
		return new Material(texture, mesh, shader) {useFaceCulling = true};
	}
}