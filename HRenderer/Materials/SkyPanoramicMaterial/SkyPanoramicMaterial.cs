using HRenderer.Core;

namespace HRenderer.Materials.SkyPanoramicMaterial; 

public class SkyPanoramicMaterial {
	public static Material GetMaterial() {
		var texture = new Texture("./Assets/sky.png");
		var mesh = new SkyPanoramicMesh();
		var shader = new SkyPanoramicShader();
		var m = new Material(mesh, shader, texture);
		m.useFaceCulling = false;
		m.useDepthWrite = false;
		
		return m;
	}
}