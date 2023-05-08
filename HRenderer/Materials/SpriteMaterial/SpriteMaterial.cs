using HRenderer.Core;

namespace HRenderer.Materials.SpriteMaterial; 

public class SpriteMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/001.jpg");
		var mesh = new SpriteMesh();
		var shader = new SpriteShader();
		var m = new Material(texture, mesh, shader);
		m.uniformData.Textures["noiseTexture"] = new Texture("./Assets/noises/noise.png");
		return m;
	}
}