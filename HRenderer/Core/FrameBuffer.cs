using HRenderer.Common;

namespace HRenderer.Core {
    public class FrameBuffer {
        private int _width;
        private int _height;
        
        // z buffer
        private float[] zBuffer;
        // 像素buffer  r_g_b_a格式
        private byte[] pixelBuffer;

        public FrameBuffer(int width, int height) {
            this._width = width;
            this._height = height;
        }

        /**
         * 读取像素信息
         */
        public byte[] ReadPixel() {
            return this.pixelBuffer;
        }

        public void SetColor(int x, int y, Color color) {
            var idx = x + y * this._height;
            this.pixelBuffer[idx] = color.r;
            this.pixelBuffer[idx+1] = color.g;
            this.pixelBuffer[idx+2] = color.b;
            this.pixelBuffer[idx+3] = color.a;
        }

        public Color GetColor(int x, int y) {
            var idx = x + y * this._height;
            return Color.Create(this.pixelBuffer[idx], this.pixelBuffer[idx+1], this.pixelBuffer[idx+2], this.pixelBuffer[idx+3]);
        }
    }
}