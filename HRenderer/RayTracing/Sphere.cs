using HRenderer.Common;

namespace HRenderer.RayTracing; 

public class Sphere: Geometry {

	private readonly Vector4 center;
	private readonly double radius;

	public Sphere(Vector4 center, double radius) {
		this.center = center;
		this.radius = radius;
	}

	public override bool HitTest(Ray ray) {
		
		return false;
	}
}