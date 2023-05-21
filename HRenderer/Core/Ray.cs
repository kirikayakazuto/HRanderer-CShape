using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 射线类
 */
public class Ray {
    private Vector4 position;
    private Vector4 direction;

    public Ray(Vector4 position, Vector4 direction) {
        this.position = position;
        this.direction = direction;
    }
}