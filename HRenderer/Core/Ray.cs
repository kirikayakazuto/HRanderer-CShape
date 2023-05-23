using HRenderer.Common;

namespace HRenderer.Core; 

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
	
    /**
     * 前进
     */
    public Vector4 SteppingTo(double len) {
	    return this.position.Add(this.direction.Mul(len));
    } 
}