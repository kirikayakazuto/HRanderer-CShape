using System;
using HRenderer.Common;

namespace HRenderer.Core {
    public class Camera {
        // 位置
        public Vector4 position;
        public Vector4 up;
        public Vector4 lookat;
        
        public float aspect = 1;
        // fov
        public float fovY = (float)Math.PI / 2;
        // 近平面
        public float near = 1;
        // 远平面
        public float far = 500;
        // 视图矩阵
        public Matrix4 view;
        // 投影矩阵
        public Matrix4 projection;
    }
}