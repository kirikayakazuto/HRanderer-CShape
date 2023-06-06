using HRenderer.Common;

namespace HRenderer.RayTracing; 

public class Sphere: Geometry {

	public readonly Vector4 center;
	public readonly double radius;

	private double tmpT = 0;

	public Sphere(Vector4 center, double radius) {
		this.center = center;
		this.radius = radius;
	}

    public override HitInfo GetHitInfo(Ray ray) {
        var hitInfo = new HitInfo();
		hitInfo.t = this.tmpT;
		hitInfo.position = ray.PointTo(this.tmpT);
		var outwardNormal = hitInfo.position.Sub(this.center).MulSelf(1 / radius).NormalizeSelf();
		hitInfo.SetFaceNormal(ray, outwardNormal);
		
		return hitInfo;
    }

    public override bool HitTest(Ray ray, double tMin, double tMax) {
		var oc = ray.position.Sub(this.center);
        var a = ray.direction.GetLengthSquared();
		var halfB = oc.Dot(ray.direction);
		var c = oc.GetLengthSquared() - this.radius * this.radius;
		var dis = halfB * halfB - a * c;
		if(dis < 0) return false;

		var sqrtDis = Math.Sqrt(dis);
		var root = (-halfB - sqrtDis) / a;
		if(root < tMin || root > tMax) {
			root = (-halfB + sqrtDis) / a;
			if(root < tMin || root > tMax) return false;
		}

		this.tmpT = root;

		return true;
	}
}