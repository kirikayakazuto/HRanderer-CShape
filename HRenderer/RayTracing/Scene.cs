using HRenderer.RayTracing.Materials;

namespace HRenderer.RayTracing; 
using HRenderer.Common;


public class Scene {
    public readonly List<Geometry> geometryList = new List<Geometry>();

    public Scene() {

        var materialGround = new Lambertian(Vector4.Create(0.5, 0.5, 0.5, 1));
        this.geometryList.Add(new Sphere(Vector4.Create(0, -1000, 0, 0), 1000, materialGround));

        var material1 = new Dielectric(1.5);
        var material2 = new Lambertian(Vector4.Create(0.4, 0.2, 0.1));
        var material3 = new Metal(Vector4.Create(0.7, 0.6, 0.5), 0.0);

        this.geometryList.Add(new Sphere(Vector4.Create(0, 1, 0, 0), 1, material1));
        this.geometryList.Add(new Sphere(Vector4.Create(-4, 1, 0, 0), 1, material2));
        this.geometryList.Add(new Sphere(Vector4.Create(4, 1, 0, 0), 1, material3));

        var count = 5;

        for(var a=-count; a<count; a++) {
            for(var b=-count; b<count; b++) {
                var mat = Utils.RandomZoreToOne();
                var center = Vector4.Create(a + 0.9 * Utils.RandomZoreToOne(), 0.2, b + 0.9 * Utils.RandomZoreToOne());
                if(center.Sub(Vector4.Create(4, 0.2, 0)).GetLength() > 0.9) {
                    if(mat < 0.8) {
                        var albedo = Utils.RandomVec4(0, 1).MulSelf(Utils.RandomVec4(0, 1));
                        var material = new Lambertian(albedo);
                        this.geometryList.Add(new Sphere(center, 0.2, material));
                    }else if(mat < 0.95) {
                        var albedo = Utils.RandomVec4(0.5, 1);
                        var fuzz = Utils.Random(0, 0.5);
                        var material = new Metal(albedo, fuzz);
                        this.geometryList.Add(new Sphere(center, 0.2, material));
                    }else {
                        var material = new Dielectric(1.5);
                        this.geometryList.Add(new Sphere(center, 0.2, material));
                    }
                }
            }
        }
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