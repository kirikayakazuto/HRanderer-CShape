using System;
using HRenderer.Common;

namespace HRenderer.Core {
    public enum ProjectionMode {
        // 透视
        Perspective,
        // 正交
        Orthographic
    }
    public class Camera {
        
        private int width;
        private int height;
        
        // 位置, 上方向, 朝向 可以描述一个摄像机
        private readonly Vector4 _position;
        private readonly Vector4 _up;
        private readonly Vector4 _toward;
        
        public double aspect = 1;
        // fov 视角大小
        public double fovY = 120 * (double)Math.PI / 180;
        // 近平面
        public double near = -1;
        // 远平面
        public double far = -100;
        
        // 观察矩阵 / 摄像机矩阵  将摄像机放在0, 0, 0位置, 并看向-z方向
        public readonly Matrix4 viewMat = Matrix4.GetIdentify();
        
        // 视图矩阵 / 屏幕矩阵  将 -1 ~ 1 空间的点 变换到 0 ~ width 和 0 ~ height 空间中;
        private readonly Matrix4 viewPortMat = Matrix4.GetIdentify();
        
        // 透视矩阵
        private readonly Matrix4 perspectiveMat = Matrix4.GetIdentify();
        // 正交矩阵
        private readonly Matrix4 orthographicMat = Matrix4.GetIdentify();

        private ProjectionMode projectionMode;

        public Camera(int width, int height) {
            this.width = width;
            this.height = height;
            
            // 初始化
            this._position = Vector4.Create(0, 0, 5f, 1);
            this._up = Vector4.Create(0, 1, 0, 1);
            this._toward = Vector4.Create(0, 0, 1, 1);

            this.projectionMode = ProjectionMode.Perspective;
            
            this.ComputeViewPortMatrix();
            // view矩阵
            this.ComputeViewMatrix();
            
            // 计算透视矩阵
            this.ComputePerspective();
            // 计算正交矩阵
            this.ComputeOrthographic();
        }

        public void SetProjectionMode(ProjectionMode mode) {
            this.projectionMode = mode;
        }
        
        public void SetPosition(double x, double y, double z) {
            this._position.Set(x, y, z, 1);
            
            this.ComputeViewPortMatrix();
            this.ComputeViewMatrix();
            this.ComputePerspective();
            this.ComputeOrthographic();
        }

        public Vector4 GetPosition() {
            return this._position;
        }
        
        /**
         * 朝向某个点
         */
        public void LookAt(Vector4 v) {
            this._toward.FromVec4(this._position.Sub(v));;
            this._toward.NormalizeSelf();
            this.ComputeViewMatrix();
            this.ComputeOrthographic();
            this.ComputePerspective();
        }
        
        /**
         * 计算lookat矩阵
         */
        public Matrix4 ComputeViewMatrix() {
            var w = this._toward.NormalizeSelf();
            var u = this._up.Cross(w).NormalizeSelf();
            var v = w.Cross(u).NormalizeSelf();

            var pos = this._position.Mul(-1);
            
            var d = this.viewMat.data;
            d[0] = u.x;
            d[4] = u.y;
            d[8] = u.z;
            d[12] = u.Dot(pos);

            d[1] = v.x;
            d[5] = v.y;
            d[9] = v.z;
            d[13] = v.Dot(pos);

            d[2] = w.x;
            d[6] = w.y;
            d[10] = w.z;
            d[14] = w.Dot(pos);

            d[3] = 0;
            d[7] = 0;
            d[11] = 0;
            d[15] = 1;

            return this.viewMat;
        }

        public Matrix4 GetProjection() {
            return this.projectionMode switch {
                ProjectionMode.Orthographic => this.orthographicMat,
                ProjectionMode.Perspective => this.perspectiveMat,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /**
         * 透视投影矩阵
         */
        private Matrix4 ComputePerspective() {
            var n = near;
            var f = far;

            var h_n = Math.Tan(fovY / 2);
            var n_h = 1 / h_n;
            var n_w = n_h / aspect;

            this.perspectiveMat.data = new double[] {
                n_w, 0,   0,             0,
                0,   n_h, 0,             0,
                0,   0,   (n+f) / (n-f), (2*f*n)/(f-n),
                0,   0,   -1,            1,
            };
            return this.perspectiveMat;
        }

        /**
         * 计算正交矩阵
         * 希望实现正确的正交投影效果, 需要将near和far设置为大小相等, 方向相反的值
         */
        private Matrix4 ComputeOrthographic() {
            var t = Math.Tan(this.fovY/2) * this.near;
            var b = -t;

            var r = this.width / (double)this.height * t;
            var l = -r;

            var n = -this.near;
            var f = -this.far;
            
            this.orthographicMat.data = new double[] {
                2 / (r - l), 0,           0,           -(r + l) / (r - l),
                0,           2 / (t - b), 0,           -(t + b) / (t - b),
                0,           0,           2 / (n - f), -(n + f) / (n - f),
                0,           0,           0,            1,
            };
            return this.orthographicMat;
        }



        public Matrix4 ComputeViewPortMatrix() {
            var m = this.viewPortMat;
            m.data[0] = this.width / 2.0f;
            m.data[5] = this.height / 2.0f;
            m.data[12] = this.width / 2.0f;
            m.data[13] = this.height / 2.0f;
            return m;
        }
    }
}