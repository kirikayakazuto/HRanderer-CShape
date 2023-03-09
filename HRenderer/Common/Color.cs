using System;

namespace HRenderer.Common {
    public class Color {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
        
        Color(byte r, byte g, byte b, byte a) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static Color Create(byte r, byte g, byte b, byte a) {
            return new Color(r, g ,b, a);
        }

        public static byte Clamp(byte min, byte max, byte value) {
            return Math.Min(max, Math.Max(min, value));
        }

        public Color Clamp(byte min = 0, byte max = 1) {
            this.r = Color.Clamp(min, max, this.r);
            this.g = Color.Clamp(min, max, this.g);
            this.b = Color.Clamp(min, max, this.b);
            this.a = Color.Clamp(min, max, this.a);
            return this;
        }
        
        public long ToInt() {
            return r << (byte) 24 + g << (byte) 16 + b << (byte) 8 + a;
        }

        public static Color IntToColor(long value) {
            var a = (byte)(value & 0x000000ff);
            var b = (byte)((value & 0x0000ff00) >> 8);
            var g = (byte)((value & 0x00ff0000) >> 16);
            var r = (byte)((value & 0xff000000) >> 24);
            return Color.Create(r, g, b, a);
        }
    }
}