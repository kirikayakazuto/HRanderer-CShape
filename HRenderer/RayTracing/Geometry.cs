namespace HRenderer.RayTracing; 

public abstract class Geometry {
	public abstract bool HitTest(Ray ray, double min, double max);
	public abstract HitInfo GetHitInfo(Ray ray);
}