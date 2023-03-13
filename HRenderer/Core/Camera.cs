using System;
using HRenderer.Common;

namespace HRenderer.Core {
    public class Camera {
        
        private int width;
        private int height;
        
        // 位置
        public Vector4 position = Vector4.Create();
        public Vector4 up = Vector4.Create();
        public Vector4 lookat = Vector4.Create();
        
        public float aspect = 1;
        // fov
        public float fovY = (float)Math.PI / 2;
        // 近平面
        public float near = 1;
        // 远平面
        public float far = 500;
        // 视图矩阵
        public Matrix4 view = Matrix4.GetIdentify();
        // 投影矩阵
        public Matrix4 projection = Matrix4.GetIdentify();
        
        public Matrix4 viewPort = Matrix4.GetIdentify();

        public Camera(int width, int height) {
            this.width = width;
            this.height = height;
            
            this.ComputeViewPort();
        }
        
        public Matrix4 ComputeLookAt() {
            var m = this.view;
            var w = this.lookat.Normalize();
            var u = this.up.Cross(w).NormalizeSelf();
            var v = w.Cross(u);

            var pos = this.position.Mul(-1);
            
            var d = m.data;
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

            return m;
        }

        public Matrix4 ComputeProjection() {
            var m = this.projection;
            var n = near;
            var f = far;

            var h_n = (float)Math.Tan(fovY / 2);
            var n_h = 1 / h_n;
            var n_w = n_h / aspect;

            m.data[0] = n_w;
            m.data[5] = n_h;
            m.data[10] = (n+f) / (n-f);
            m.data[11] = 1;
            m.data[14] = (2*f*n)/(f-n);
            return m;
        }

        public Matrix4 ComputeViewPort() {
            var m = this.viewPort;
            m.data[0] = this.width / 2.0f;
            m.data[5] = this.height / 2.0f;
            m.data[12] = this.width / 2.0f;
            m.data[13] = this.height / 2.0f;
            return m;
        }
    }
}