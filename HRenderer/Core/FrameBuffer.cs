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

            this.pixelBuffer = new byte[width * height * 4];
            this.zBuffer = new float[width * height];
            for (var i = 0; i < this.zBuffer.Length; i++) {
                this.zBuffer[i] = 1;
            }
        }

        /**
         * 读取像素信息
         */
        public byte[] ReadPixel() {
            return this.pixelBuffer;
        }

        public void SetColor(int x, int y, Vector4 color, float z) {
            if(!this.ZTest(x, y, z)) return;
            var idx = (x + y * this._height) * 4;
            this.pixelBuffer[idx] = (byte)(color.x * 255);
            this.pixelBuffer[idx+1] = (byte)(color.y * 255);
            this.pixelBuffer[idx+2] = (byte)(color.z * 255);
            this.pixelBuffer[idx+3] = (byte)(color.w * 255);
            this.SetZ(x, y, z);
        }

        public void SetColor(int x, int y, Color color) {
            var idx = (x + y * this._height) * 4;
            this.pixelBuffer.CopyTo(color.data, idx);
        }

        public Color GetColor(int x, int y) {
            var idx = x + y * this._height;
            return Color.Create(this.pixelBuffer[idx], this.pixelBuffer[idx+1], this.pixelBuffer[idx+2], this.pixelBuffer[idx+3]);
        }

        public float GetZ(int x, int y) {
            var idx = x + y * this._height;
            return this.zBuffer[idx];
        }

        public void SetZ(int x, int y, float z) {
            var idx = x + y * this._height;
            this.zBuffer[idx] = z;
        }

        public bool ZTest(int x, int y, float z) {
            return z < this.GetZ(x, y);
        }
    }
}