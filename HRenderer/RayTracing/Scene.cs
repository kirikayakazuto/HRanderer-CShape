using HRenderer.RayTracing.Materials;

namespace HRenderer.RayTracing; 
using HRenderer.Common;


public class Scene {
    public readonly List<Geometry> geometryList = new List<Geometry>();

    public Scene() {

        var materialGround = new Lambertian(Vector4.Create(0.8, 0.8, 0, 1));
        var materialCenter = new Lambertian(Vector4.Create(0.7, 0.3, 0.3, 1));
        // var materialLeft = new Metal(Vector4.Create(0.8, 0.8, 0.8, 1), 0.3);
        var materialRight = new Metal(Vector4.Create(0.8, 0.6, 0.2, 1), 0);
        var materialLeft = new Dielectric(1.5);
        // var materialRight = new Dielectric(1.5);
        
        
        this.geometryList.Add(new Sphere(Vector4.Create(0, -100.5, 0, 0), 100, materialGround));
        this.geometryList.Add(new Sphere(Vector4.Create(0, 0, 1, 0), 0.5, materialCenter));
        this.geometryList.Add(new Sphere(Vector4.Create(-1, 0, 1, 0), 0.5, materialLeft));
        this.geometryList.Add(new Sphere(Vector4.Create(-1, 0, 1, 0), -0.45, materialLeft));
        this.geometryList.Add(new Sphere(Vector4.Create(1, 0, 1, 0), 0.5, materialRight));
    }

    public bool HitTest(Ray ray, double tMin, double tMax, ref HitInfo hitInfo) {
        var isHit = false;
        foreach (var geometry in this.geometryList) {
            if (!geometry.HitTest(ray, tMin, tMax, ref hitInfo)) continue;
            isHit = true;
            tMax = hitInfo.t;
        }

        return isHit;
    }
}