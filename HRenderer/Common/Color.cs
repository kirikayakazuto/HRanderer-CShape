using System;
using System.Collections.Generic;

namespace HRenderer.Common {
    public class Color {
        private static Stack<Color> colors = new Stack<Color>();
        public byte[] data;

        public byte r {
            get => this.data[0];
            set => this.data[0] = value;
        }
        public byte g {
            get => this.data[1];
            set => this.data[1] = value;
        }
        public byte b {
            get => this.data[2];
            set => this.data[2] = value;
        }
        public byte a {
            get => this.data[3];
            set => this.data[3] = value;
        }
        
        private Color(byte r, byte g, byte b, byte a) {
            this.data = new byte[4];
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        
        public static Color Create(byte r, byte g, byte b, byte a) {
            Color c;
            if (Color.colors.Count > 0) {
                c = Color.colors.Pop();
                c.SetColor(r, g, b, a);
            }
            else {
                c = new Color(r, g ,b, a);
            }
            return c;
        }

        public static void Return(Color color) {
            Color.colors.Push(color);
        }

        public void SetColor(byte r, byte g, byte b, byte a) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
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