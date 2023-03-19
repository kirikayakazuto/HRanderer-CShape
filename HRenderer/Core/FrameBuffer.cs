using HRenderer.Common;

namespace HRenderer.Core {
    public class FrameBuffer {
        public int width { get; }
        public int height { get; }
        
        // z buffer
        private readonly double[] _zBuffer;
        // 像素buffer  r_g_b_a格式
        private readonly byte[] _pixelBuffer;
        
        // msaa
        private readonly bool _useMsaa;
        private readonly byte[] _pixelsMsaa;
        private readonly double[,] _zBufferMsaa;

        public FrameBuffer(int width, int height, bool msaa = false) {
            this.width = width;
            this.height = height;
            this._useMsaa = msaa;

            this._pixelBuffer = new byte[width * height * 4];
            this._zBuffer = new double[width * height];
            for (var i = 0; i < this._zBuffer.Length; i++) {
                this._zBuffer[i] = 1;
            }
            
            if (!msaa) return;
            this._zBufferMsaa = new double[4, width * height];
            for (var i = 0; i < this._zBufferMsaa.GetLength(0); i++) {
                for (var j = 0; j < this._zBufferMsaa.GetLength(1); j++) {
                    this._zBufferMsaa[i, j] = 1;
                }    
            }
            this._pixelsMsaa = new byte[width * height];
        }

        /**
         * 读取像素信息
         */
        public byte[] GetPixels() {
            return this._pixelBuffer;
        }

        public void SetColor(int x, int y, Vector4 color) {
            var idx = (x + y * this.width) * 4;
            this._pixelBuffer[idx] = (byte)(color.x * 255);
            this._pixelBuffer[idx+1] = (byte)(color.y * 255);
            this._pixelBuffer[idx+2] = (byte)(color.z * 255);
            this._pixelBuffer[idx+3] = (byte)(color.w * 255);
        }

        public void SetColor(int x, int y, Color color) {
            var idx = (x + y * this.width) * 4;
            this._pixelBuffer.CopyTo(color.data, idx);
        }

        public Color GetColor(int x, int y) {
            var idx = x + y * this.width;
            return Color.Create(this._pixelBuffer[idx], this._pixelBuffer[idx+1], this._pixelBuffer[idx+2], this._pixelBuffer[idx+3]);
        }

        public double GetZ(int x, int y) {
            var idx = x + y * this.width;
            return this._zBuffer[idx];
        }
        
        public double GetZ(int x, int y, int level) {
            var idx = x + y * this.width;
            return this._zBufferMsaa[level, idx];
        }
        
        public void SetZ(int x, int y, double z) {
            var idx = x + y * this.width;
            this._zBuffer[idx] = z;
        }
        
        public void SetZ(int x, int y, double z, int level) {
            var idx = x + y * this.width;
            this._zBufferMsaa[level, idx] = z;
        }

        public bool ZTest(int x, int y, double z) {
            return z < this.GetZ(x, y);
        }

        public bool ZTest(int x, int y, double z, int level) {
            return z < this.GetZ(x, y, level);
        }

        public void SaveImageLocal(int frame) {
            Utils.SaveImage(this.width, this.height, this._pixelBuffer, frame);
        }

        public void Clear() {
            for (var i = 0; i < this._pixelBuffer.Length; i++) {
                this._pixelBuffer[i] = 0;
            }
            
            for (var i = 0; i < this._zBuffer.Length; i++) {
                this._zBuffer[i] = 1;
            }
            
            if (!this._useMsaa) return;
            for (var i = 0; i < this._zBufferMsaa.GetLength(0); i++) {
                for (var j = 0; j < this._zBufferMsaa.GetLength(1); j++) {
                    this._zBufferMsaa[i, j] = 1;
                }    
            }
            for (var i = 0; i < this._pixelsMsaa.Length; i++) {
                this._pixelsMsaa[i] = 0;
            }
        }

        public void AddMsaaCount(int x, int y) {
            var idx = x + y * this.width;
            this._pixelsMsaa[idx] ++;
        }

        public byte GetMsaaCount(int x, int y) {
            var idx = x + y * this.width;
            return this._pixelsMsaa[idx];
        }

        public void DoMsaa() {
            for (var y = 0; y < this.height; y++) {
                for (var x = 0; x < this.width; x++) {
                    var idx = (x + y * this.width) * 4;
                    
                    var msaa = this._pixelsMsaa[x + y * this.width];
                    var count = (double)msaa / 4.0f;
                    count = Math.Min(1, count);
                    
                    this._pixelBuffer[idx]   = (byte)Math.Floor(this._pixelBuffer[idx] * count);
                    this._pixelBuffer[idx+1] = (byte)Math.Floor(this._pixelBuffer[idx+1] * count);
                    this._pixelBuffer[idx+2] = (byte)Math.Floor(this._pixelBuffer[idx+2] * count);
                    this._pixelBuffer[idx+3] = (byte)Math.Floor(this._pixelBuffer[idx+3] * count);
                }
            }
            // for (var y = 0; y < this.height; y++) {
            //     for (var x = 0; x < this.width; x++) {
            //         var msaa = this._pixelMsaas[x + y * this.width];
            //         Console.Write(msaa);
            //     }
            //     Console.WriteLine();
            // }
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine();
        }
        
        
    }
}