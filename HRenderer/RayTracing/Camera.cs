namespace HRenderer.RayTracing; 
using HRenderer.Common;

public class Camera {

    // 位置, 上方向, 朝向 可以描述一个摄像机
    private readonly Vector4 _position;
    private readonly Vector4 _up;
    private readonly Vector4 _toward;

    public Vector4 up => this._up;
    public Vector4 right => this._up.Cross(this._toward);
    
    // 宽高比
    public double aspect = 2;
    // fov 视角大小
    public double fovY = 90 * (double)Math.PI / 180;
    // 近平面
    public double near = -2;
    // 远平面
    public double far = -100;

    private double width;
    private double height;

    public Camera() {
        // 初始化
        this._position = Vector4.Create(0f, 0f, 5f, 1);
        this._up = Vector4.Create(0, 1, 0, 1);
        this._toward = Vector4.Create(0, 0, 1, 1);

        this.height = Math.Tan(this.fovY / 2) * this.near * 2;
        this.width = this.height * this.aspect;
    }

    public Vector4 GetPosition() {
        return this._position;
    }

    public Ray GetRay(double u, double v) {
        var h = this.height;
        var w = this.width;
        var n = this.near;

        var screenPoint = Vector4.Create(u * w - w / 2, v * h - h / 2, n);
        return new Ray(this._position, screenPoint.SubSelf(this._position));
    }
}