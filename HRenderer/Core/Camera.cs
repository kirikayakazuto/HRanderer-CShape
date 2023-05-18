using System;
using HRenderer.Common;

namespace HRenderer.Core {
    public class Camera {
        
        private int width;
        private int height;
        
        // 位置, 上方向, 朝向 可以描述一个摄像机
        private readonly Vector4 _position;
        private readonly Vector4 _up;
        private readonly Vector4 _toward;
        
        public double aspect = 1;
        // fov 视角大小
        public double fovY = 90 * (double)Math.PI / 180;
        // 近平面
        public double near = 1f;
        // 远平面
        public double far = 500;
        
        // 观察矩阵 / 摄像机矩阵  将摄像机放在0, 0, 0位置, 并看向-z方向
        public readonly Matrix4 viewMat = Matrix4.GetIdentify();
        
        // 视图矩阵 / 屏幕矩阵  将 -1 ~ 1 空间的点 变换到 0 ~ width 和 0 ~ height 空间中;
        public readonly Matrix4 viewPortMat = Matrix4.GetIdentify();
        
        // 透视投影矩阵 
        public readonly Matrix4 OrthographicProjection = Matrix4.GetIdentify();

        public readonly Matrix4 projectionMat = Matrix4.GetIdentify();
        public readonly Matrix4 orthographicMat = Matrix4.GetIdentify();

        public Camera(int width, int height) {
            this.width = width;
            this.height = height;
            
            // 初始化
            this._position = Vector4.Create(0, 0, 6f, 1);
            this._up = Vector4.Create(0, 1, 0, 1);
            this._toward = Vector4.Create(0, 0, 1, 1);
            
            // 初始化矩阵
            this.ComputeViewPortMatrix();
            this.ComputeViewMatrix();
            this.ComputeOrthographicProjection();
        }
        
        public void SetPosition(double x, double y, double z) {
            this._position.Set(x, y, z, 1);
            this.ComputeViewMatrix();
            this.ComputeOrthographicProjection();
        }

        public Vector4 GetPosition() {
            return this._position.Clone();
        }
        
        /**
         * 朝向某个点
         */
        public void LookAt(Vector4 v) {
            this._toward.FromVec4(this._position.Sub(v));;
            this._toward.NormalizeSelf();
            this.ComputeViewMatrix();
            this.ComputeOrthographicProjection();
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

        /**
         * 透视投影矩阵
         */
        public Matrix4 ComputeOrthographicProjection() {
            var m = this.OrthographicProjection;
            var n = near;
            var f = far;

            var h_n = (double)Math.Tan(fovY / 2);
            var n_h = 1 / h_n;
            var n_w = n_h / aspect;

            m.data[0] = n_w;
            m.data[5] = n_h;
            m.data[10] = (n+f) / (n-f);
            m.data[11] = (2*f*n)/(f-n);
            m.data[14] = -1;
            return m;
        }

        /**
         * 计算正交矩阵
         * 将right-left, top-bottom, near-far 空间的点, 变换到 -1 ~ 1 的标准矩阵中
         */
        public Matrix4 ComputeOrthographic() {
            var t = (double)Math.Tan(this.fovY/2) * this.near;
            var b = -t;

            var r = (double)this.width / (double)this.height * t;
            var l = -r;

            var n = this.near;
            var f = this.far;
            
            this.orthographicMat.data = new double[] {
                2 / (r - l), 0,           0,           -(r + l) / (r - l),
                0,           2 / (t - b), 0,           -(t + b) / (t - b),
                0,           0,           2 / (n - f), -(n + f) / (n - f),
                0,           0,           0,            1,
            };
            return this.orthographicMat;
        }

        public Matrix4 ComputeProjection() {
            var n = this.near;
            var f = this.far;
            this.projectionMat.data = new double[] {
                n, 0, 0, 0,
                0, n, 0, 0,
                0, 0, n + f, -f * n,
                0, 0, 1, 0,
            };
            return this.projectionMat;
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