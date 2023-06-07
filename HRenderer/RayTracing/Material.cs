using HRenderer.Common;
namespace HRenderer.RayTracing; 

/**
 * 射线类
 */
public abstract class Material {
    public abstract bool Scatter(Ray ray, ref HitInfo hitInfo, out Vector4 attenuation, out Ray rayOut);
}