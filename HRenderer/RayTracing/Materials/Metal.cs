using HRenderer.Common;

namespace HRenderer.RayTracing.Materials; 

public class Metal: Material {
	private readonly Vector4 albedo;
	private readonly double fuzz;

	public Metal(Vector4 albedo, double fuzz) {
		this.albedo = albedo;
		this.fuzz = fuzz < 1 ? fuzz : 1;
	}
	
	public override bool Scatter(Ray ray, ref HitInfo hitInfo, out Vector4 attenuation, out Ray rayOut) {
		var reflected = Utils.Reflect(ray.direction.Normalize(), hitInfo.normal);
		rayOut = new Ray(hitInfo.position, reflected.AddSelf(Utils.RandomInUnitSphere().MulSelf(this.fuzz)));
		attenuation = this.albedo.Clone();
		return rayOut.direction.Dot(hitInfo.normal) > 0;
	}
}