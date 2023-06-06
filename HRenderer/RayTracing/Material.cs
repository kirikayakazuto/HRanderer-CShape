
namespace HRenderer.RayTracing; 

/**
 * 射线类
 */
public abstract class Material {
    public abstract bool Scatter(Ray ray);
}