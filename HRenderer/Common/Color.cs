using System;

namespace HRenderer.Common {
    public class Color {
        public int r;
        public int g;
        public int b;
        public int a;
        
        Color(int r, int g, int b, int a) {
            this.r = r;
        }

        int Clamp(int min, int max, int value) {
            return Math.Min(max, Math.Max(min, value));
        }

        public Color Clamp(int min = 0, int max = 1) {
            this.r = this.Clamp(min, max, this.r);
            this.g = this.Clamp(min, max, this.g);
            this.b = this.Clamp(min, max, this.b);
            this.a = this.Clamp(min, max, this.a);
            return this;
        }

        public new Color Normalize() {
            this.r /= 255;
            this.g /= 255;
            this.b /= 255;
            this.a /= 255;
            return this;
        }
        
    }
}