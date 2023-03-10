using System;

namespace HRenderer.Core {
    /**
     * 渲染器
     */
    public class Renderer {
        // size
        private int _width = 0;
        private int _height = 0;

        // 相机
        public Camera camera;
        // 画布
        public FrameBuffer frameBuffer;
        // 定时器
        private System.Timers.Timer aTimer;

        public Renderer(int width, int height) {
            this._width = width;
            this._height = height;
            
            this.camera = new Camera();
            this.frameBuffer = new FrameBuffer(width, height);
            
            // 初始化定时器
            this.InitTimer();
        }

        private void InitTimer() {
            this.aTimer = new System.Timers.Timer(500);
            this.aTimer.Elapsed += this.Loop;
            this.aTimer.AutoReset = true;
            this.aTimer.Enabled = true;
            this.aTimer.Start();
        }

        private void Loop(Object source, System.Timers.ElapsedEventArgs e) {
            this.aTimer.Stop();
            this.Draw();
            this.aTimer.Start();
        }


        /**
         * 绑定shader
         */
        public void BindShader() {
            
        }

        /**
         * 绑定材质
         */
        public void BindMaterial() {
            
        }
        
        /**
         * 渲染
         */
        private void Draw() {
            
        }

        private void DrawElementArray() {
            
        }
        
        
        public void Clear() {
            this.aTimer.Stop();
            this.aTimer.Close();
        }

    }
}