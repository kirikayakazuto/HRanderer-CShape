using HRenderer.Common;

namespace HRenderer.RayTracing.Materials; 

public class Lambertian: Material {
    private readonly Vector4 albedo;

    public Lambertian(Vector4 albedo) {
        this.albedo = albedo;
    }

    public override bool Scatter(Ray ray, ref HitInfo hitInfo, out Vector4 attenuation, out Ray rayOut) {
        var scatterDir = hitInfo.normal.Add(Utils.RandomInUnitSphere());
        if (scatterDir.EqualsZero()) {
            scatterDir = hitInfo.normal;
        }
        rayOut = new Ray(hitInfo.position, scatterDir);
        attenuation = this.albedo.Clone();
        return true;
    }
}