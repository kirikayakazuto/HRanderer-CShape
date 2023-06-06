namespace HRenderer.RayTracing; 
using HRenderer.Common;


public class Scene {
    public readonly List<Geometry> geometryList = new List<Geometry>();

    public Scene() {
        this.geometryList.Add(new Sphere(Vector4.Create(0, 0, -1, 0), 0.5));
    }

    public bool HitTest(Ray ray, double tMin, double tMax) {
        var isHit = false;
        foreach (var geometry in this.geometryList) {
			if(geometry.HitTest(ray, tMin, tMax)) {
                isHit = true;
                var hitInfo = geometry.GetHitInfo(ray);
                tMax = hitInfo.t;
            }
		}

        return isHit;
    }
}