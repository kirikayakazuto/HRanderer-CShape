using System;
using HRenderer.Common;

namespace HRenderer {
    public class Camera {
        // 位置
        public Vector4 position;
        // 向上
        public Vector4 up;
        // 
        public Vector4 lookat;

        public float aspect = 1;

        public float fovY = (float)Math.PI / 2;

        public float near = 1;

        public float far = 500;
        
        
    }
}