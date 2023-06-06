namespace HRenderer.RayTracing; 
using HRenderer.Common;


public class Scene {
    public readonly List<Geometry> geometryList = new List<Geometry>();

    public Scene() {
        this.geometryList.Add(new Sphere(Vector4.Create(0, 0, 1, 0), 0.5));
        this.geometryList.Add(new Sphere(Vector4.Create(0, -50.5, 0, 0), 50));
    }

    public bool HitTest(Ray ray, double tMin, double tMax, ref HitInfo hitInfo) {
        var isHit = false;
        foreach (var geometry in this.geometryList) {
			if(geometry.HitTest(ray, tMin, tMax, ref hitInfo)) {
                isHit = true;
                tMax = hitInfo.t;
            }
		}

        return isHit;
    }
}