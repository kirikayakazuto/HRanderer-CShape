using HRenderer.Common;

namespace HRenderer.Core {
    public class FrameBuffer {
        
        private readonly int _width;
        private readonly int _height;
        // z buffer
        private readonly float[] _zBuffer;
        // 像素buffer  r_g_b_a格式
        private readonly byte[] _pixelBuffer;

        public int Width => this._width;
        public int Height => this._height;
        public byte[] Pixels => this._pixelBuffer;

        public FrameBuffer(int width, int height) {
            this._width = width;
            this._height = height;

            this._pixelBuffer = new byte[width * height * 4];
            this._zBuffer = new float[width * height];
            for (var i = 0; i < this._zBuffer.Length; i++) {
                this._zBuffer[i] = 1;
            }
        }

        /**
         * 读取像素信息
         */
        public byte[] ReadPixel() {
            return this._pixelBuffer;
        }

        public void SetColor(int x, int y, Vector4 color, float z) {
            if(!this.ZTest(x, y, -z)) return;
            var idx = (x + y * this._width) * 4;
            this._pixelBuffer[idx] = (byte)(color.x * 255);
            this._pixelBuffer[idx+1] = (byte)(color.y * 255);
            this._pixelBuffer[idx+2] = (byte)(color.z * 255);
            this._pixelBuffer[idx+3] = (byte)(color.w * 255);
            this.SetZ(x, y, -z);
        }

        public void SetColor(int x, int y, Color color) {
            var idx = (x + y * this._height) * 4;
            this._pixelBuffer.CopyTo(color.data, idx);
        }

        public Color GetColor(int x, int y) {
            var idx = x + y * this._height;
            return Color.Create(this._pixelBuffer[idx], this._pixelBuffer[idx+1], this._pixelBuffer[idx+2], this._pixelBuffer[idx+3]);
        }

        public float GetZ(int x, int y) {
            var idx = x + y * this._height;
            return this._zBuffer[idx];
        }

        public void SetZ(int x, int y, float z) {
            var idx = x + y * this._height;
            this._zBuffer[idx] = z;
        }

        public bool ZTest(int x, int y, float z) {
            return z < this.GetZ(x, y);
        }

        public void SaveImageLocal(int frame) {
            Utils.SaveImage(this._width, this._height, this._pixelBuffer, frame);
        }

        public void Clear() {
            for (var i = 0; i < this._pixelBuffer.Length; i++) {
                this._pixelBuffer[i] = 0;
            }

            for (var i = 0; i < this._zBuffer.Length; i++) {
                this._zBuffer[i] = 1;
            }
        }
    }
}