namespace HRenderer.Core {
    public class FrameBuffer {
        private int width;
        private int height;
        
        // z buffer
        private float[] zBuffer;
        // 像素buffer  r_g_b_a格式
        private int[] pixelBuffer;

        /**
         * 读取像素信息
         */
        public int[] ReadPixel() {
            return this.pixelBuffer;
        }

        public void SetPixel() {
            
        }
    }
}