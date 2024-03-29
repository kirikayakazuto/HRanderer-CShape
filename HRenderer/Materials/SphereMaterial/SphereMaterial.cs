using HRenderer.Core;

namespace HRenderer.Materials.SphereMaterial; 

public class SphereMaterial {
    public static Material GetMaterial() {
        var texture = new Texture("./Assets/earth.jpg");
        var mesh = new SphereMesh();
        var shader = new SphereShader();
        var m = new Material("Sphere", mesh, shader, texture);
        m.useFaceCulling = true;
        return m;
    }
}