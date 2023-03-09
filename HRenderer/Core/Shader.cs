using HRenderer.Common;

namespace HRenderer.Core {
    
    public abstract class Shader {
        // 位置
        private Vector4 gl_Position;
        // 当前图块位置
        private Vector4 gl_FragCoord;
        // 输出颜色
        private Vector4 gl_FragColor;
        
        public abstract void VertexShading();
        
        public abstract void FragShading();
    }
}