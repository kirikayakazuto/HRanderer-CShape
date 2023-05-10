using HRenderer.Common;

namespace HRenderer.Core {
    public class FrameBuffer {
        public readonly int width;
        public readonly int height;
        
        // 像素buffer  r_g_b_a格式
        private readonly byte[] _pixelBuffer;
        
        public FrameBuffer(int width, int height) {
            this.width = width;
            this.height = height;

            this._pixelBuffer = new byte[width * height * 4];
        }

        /**
         * 读取像素信息
         */
        public byte[] GetPixels() {
            return this._pixelBuffer;
        }

        public void SetColor(int x, int y, Vector4 color) {
            if(x<0 || x>=this.width) return;
            if(y<0 || y>=this.height) return;
            var idx = (x + y * this.width) * 4;
            this._pixelBuffer[idx] = (byte)(color.x * 255);
            this._pixelBuffer[idx+1] = (byte)(color.y * 255);
            this._pixelBuffer[idx+2] = (byte)(color.z * 255);
            this._pixelBuffer[idx+3] = (byte)(color.w * 255);
        }
        
        public void SaveImageLocal(int frame) {
            Utils.SaveImage(this.width, this.height, this._pixelBuffer, frame);
        }

        public void Clear() {
            for (var i = 0; i < this._pixelBuffer.Length; i++) {
                this._pixelBuffer[i] = 0;
            }
        }
        
        public void DoMsaa(byte[] coverages) {
            for (var y = 0; y < this.height; y++) {
                for (var x = 0; x < this.width; x++) {
                    var idx = (x + y * this.width) * 4;
                    
                    var count = (double)coverages[x + y * this.width] / 4.0f;
                    count = Math.Min(1, count);
                    
                    this._pixelBuffer[idx]   = (byte)Math.Floor(this._pixelBuffer[idx] * count);
                    this._pixelBuffer[idx+1] = (byte)Math.Floor(this._pixelBuffer[idx+1] * count);
                    this._pixelBuffer[idx+2] = (byte)Math.Floor(this._pixelBuffer[idx+2] * count);
                    this._pixelBuffer[idx+3] = (byte)Math.Floor(this._pixelBuffer[idx+3] * count);
                }
            }
        }
        
    }
}