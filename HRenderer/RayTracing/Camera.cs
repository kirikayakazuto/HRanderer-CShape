namespace HRenderer.RayTracing; 
using HRenderer.Common;

public class Camera {

    // 位置, 上方向, 朝向 可以描述一个摄像机
    private readonly Vector4 _position;
    private readonly Vector4 _up;
    private readonly Vector4 _toward;

    // 宽高比
    public double aspect = 1;
    // fov 视角大小
    public double fovY = 40 * (double)Math.PI / 180;
    // 近平面
    public double near = -1;
    // 远平面
    public double far = -100;

    private Vector4 horizontal;
    private Vector4 vertical;
    private Vector4 bottomLeftCorner;

    private double aperture;

    private double focusDist;

    private readonly Vector4 right;
    private readonly Vector4 up;

    public Camera(double aspect) {
        // 初始化
        this._position = Vector4.Create(8f, 1.8f, 2.1f, 1);
        this._up = Vector4.Create(0, 1, 0, 1);
        this._toward = Vector4.Create(0, 0, 1, 1);
        this.LookAt(Vector4.Create(0, 0, 1));

        this.aspect = aspect;
        this.focusDist = 10;
        this.aperture = 0.02;

        var height = Math.Tan(this.fovY / 2) * this.near * 2;
        var width = height * this.aspect;

        // var right = this._up.Cross(this._toward).NormalizeSelf();
        var h = this.right = this._toward.Cross(this._up).NormalizeSelf();
        var v = this.up = h.Cross(this._toward).NormalizeSelf();
        this.horizontal = h.Mul(width * focusDist);
        this.vertical = v.Mul(height * focusDist);
        
        this.bottomLeftCorner = this._position.Sub(this.horizontal.Mul(0.5)).SubSelf(this.vertical.Mul(0.5)).SubSelf(this._toward.Mul(focusDist));
        
    }

    public void LookAt(Vector4 v) {
        this._toward.FromVec4(this._position.Sub(v));
        this._toward.NormalizeSelf();
    }

    public Vector4 GetPosition() {
        return this._position;
    }

    public Ray GetRay(double u, double v) {
        var rd = Utils.RandomInUnitDisk().MulSelf(this.aperture);
        var offset = this.right.Mul(rd.x).AddSelf(this.up.Mul(rd.y));

        var h = this.horizontal;
        var w = this.vertical;
        var n = this.near;

        var pos = this.bottomLeftCorner.Add(this.horizontal.Mul(u)).AddSelf(this.vertical.Mul(v)).SubSelf(this._position).SubSelf(offset);
        return new Ray(this._position.Add(offset), pos);
    }
}