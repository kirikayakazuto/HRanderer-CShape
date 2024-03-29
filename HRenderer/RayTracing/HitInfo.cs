using HRenderer.Common;

namespace HRenderer.RayTracing; 

public struct HitInfo {
    public Vector4 position;
    public Vector4 normal;
    public double t;
    public Material material;

    // 是否是外表面
    public bool isFrontFace;

    public void SetFaceNormal(Ray ray, Vector4 outwardNormal) {
        this.isFrontFace = ray.direction.Dot(outwardNormal) < 0;
        this.normal = this.isFrontFace ? outwardNormal : outwardNormal.MulSelf(-1);
    }

    public void From(HitInfo hitInfo) {
        this.position = hitInfo.position;
        this.normal = hitInfo.normal;
        this.t = hitInfo.t;
        this.isFrontFace = hitInfo.isFrontFace;
    }
}