using HRenderer.Common;

namespace HRenderer.RayTracing; 

public class Sphere: Geometry {

	public readonly Vector4 center;
	public readonly double radius;

	public Sphere(Vector4 center, double radius) {
		this.center = center;
		this.radius = radius;
	}

    public override bool HitTest(Ray ray, double tMin, double tMax, ref HitInfo hitInfo) {
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

		hitInfo.t = root;
		hitInfo.position = ray.PointTo(root);
		var outwardNormal = hitInfo.position.Sub(this.center).MulSelf(1 / radius).NormalizeSelf();
		hitInfo.SetFaceNormal(ray, outwardNormal);

		return true;
	}
}