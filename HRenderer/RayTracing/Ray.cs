using HRenderer.Common;

namespace HRenderer.RayTracing; 

/**
 * 射线类
 */
public class Ray {
    private readonly Vector4 position;
    private readonly Vector4 direction;

    public Ray(Vector4 position, Vector4 direction) {
        this.position = position;
        this.direction = direction;
    }
    
    public Vector4 PointTo(double t) {
	    // o + td
	    return this.position.Add(this.direction.Mul(t));
    } 
}