using HRenderer.Common;

namespace HRenderer.Core {
    public class FrameBuffer {
        public readonly int width;
        public readonly int height;
        
        // 像素buffer  r_g_b_a格式
        private readonly byte[] _pixelBuffer;

        private readonly DepthBuffer _depthBuffer;
        private readonly StencilBuffer _stencilBuffer;

        public FrameBuffer(int width, int height, bool msaa) {
            this.width = width;
            this.height = height;

            this._pixelBuffer = new byte[width * height * 4];

            this._depthBuffer = new DepthBuffer(width, height, msaa);
            this._stencilBuffer = new StencilBuffer(width, height);
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
            
            this._depthBuffer.Clear();
            this._stencilBuffer.Clear();
        }
        
        public void DoMsaa() {
            for (var y = 0; y < this.height; y++) {
                for (var x = 0; x < this.width; x++) {
                    var idx = (x + y * this.width) * 4;
                    
                    var count = (double)this._depthBuffer.GetMsaaCount(x, y) / 4.0f;
                    count = Math.Min(1, count);
                    
                    this._pixelBuffer[idx]   = (byte)Math.Floor(this._pixelBuffer[idx] * count);
                    this._pixelBuffer[idx+1] = (byte)Math.Floor(this._pixelBuffer[idx+1] * count);
                    this._pixelBuffer[idx+2] = (byte)Math.Floor(this._pixelBuffer[idx+2] * count);
                    this._pixelBuffer[idx+3] = (byte)Math.Floor(this._pixelBuffer[idx+3] * count);
                }
            }
        }

        public bool CheckZ(int x, int y, double z) {
            if (!this._depthBuffer.ZTest(x, y, z)) return false;
            this._depthBuffer.SetZ(x, y, z);
            return true;
        }
        
        public bool CheckZ(int x, int y, double z, int level) {
            if (!this._depthBuffer.ZTest(x, y, z, level)) return false;
            this._depthBuffer.SetZ(x, y, z, level);
            return true;
        }

        public void AddMsaaCount(int x, int y) {
            this._depthBuffer.AddMsaaCount(x, y);
        }
        
    }
}