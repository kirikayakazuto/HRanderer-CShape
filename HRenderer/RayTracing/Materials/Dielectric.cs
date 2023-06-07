using HRenderer.Common;

namespace HRenderer.RayTracing.Materials; 

public class Dielectric: Material {
	public double ir;

	public Dielectric(double ir) {
		this.ir = ir;
	}
	public override bool Scatter(Ray ray, ref HitInfo hitInfo, out Vector4 attenuation, out Ray rayOut) {
		attenuation = Vector4.Create(1, 1, 1, 1);
		var refractionRatio = hitInfo.isFrontFace ? (1f / ir) : ir;
		var dir = ray.direction.Normalize();

		var cosTheta = Math.Min(1, hitInfo.normal.Dot(dir.Mul(-1)));
		var sinTheta = Math.Sqrt(1 - cosTheta * cosTheta);
		var cannotRefract = refractionRatio * sinTheta > 1;

		var d = cannotRefract || this.Reflectance(cosTheta, refractionRatio) > Utils.RandomZoreToOne() ? Utils.Reflect(dir, hitInfo.normal) : Utils.Refract(dir, hitInfo.normal, refractionRatio);
		rayOut = new Ray(hitInfo.position, d);
		return true;
	}

	private double Reflectance(double cosine, double refIdx) {
		var r0 = (1-refIdx) / (1+refIdx);
		r0 *= r0;
		return r0 + (1 - r0) * Math.Pow(1-cosine, 5f);
	}
}