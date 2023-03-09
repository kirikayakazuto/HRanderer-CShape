using System;

namespace HRenderer.Core {
    /**
     * 渲染器
     */
    public class Renderer {
        private int width = 0;
        private int height = 0;

        // 相机
        public Camera camera;
        // 画布
        public FrameBuffer frameBuffer;
        
        // 
        private System.Timers.Timer aTimer;

        public Renderer(int width, int height) {
            this.width = width;
            this.height = height;
            
            this.camera = new Camera();
            this.frameBuffer = new FrameBuffer();
            
            // 初始化定时器
            this.aTimer = new System.Timers.Timer(500);
            this.aTimer.Elapsed += this.Loop;
            this.aTimer.Start();
        }

        private void Loop(Object source, System.Timers.ElapsedEventArgs e) {
            this.Draw();
        }


        /**
         * 渲染
         */
        private void Draw() {
            
        }
        
        

    }
}