using System;
using System.IO;
using System.Runtime.CompilerServices;
using HRenderer.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = HRenderer.Common.Color;

namespace HRenderer.Core {
    public enum FilterMode {
        Nearest,
        Linear
    }
    public enum WrapMode {
        Repeat,
        Clamp,
    }
    
    public class Texture {
        public int width;
        public int height;
        public byte[] pixel;
        public FilterMode filterMode = FilterMode.Linear;
        public WrapMode wrapMode = WrapMode.Clamp;
        
        public Texture(string imagePath) {
            this.InitFromLocalImage(imagePath);
        }

        public void SetFilterMode(FilterMode filter) {
            this.filterMode = filter;
        }

        public void SetWrapMode(WrapMode wrap) {
            this.wrapMode = wrap;
        }

        public Color GetColor(int x, int y) {
            var idx =x + y * this.width;
            return Color.Create(this.pixel[idx], this.pixel[idx+1], this.pixel[idx+2], this.pixel[idx+3]);
        }

        public Color Sample(float u, float v) {
            var x = u * (this.width - 1) + 0.5;
            var y = this.height - (v * (this.height - 1) + 0.5);
            // var y =  v * (this.height - 1) + 0.5;
            switch (this.filterMode) {
                case FilterMode.Nearest:
                    return this.Nearest(x, y);
                case FilterMode.Linear:
                    return this.Bilinear(x, y);
                default:
                    return Color.Create(0, 0, 0, 0);
            }
        }

        private Color Nearest(double x, double y) {
            var cx = (int)Math.Floor(x);
            var cy = (int)Math.Floor(y);
            var idx = this.GetIndex(cx, cy);
            return Color.Create(this.pixel[idx], this.pixel[idx + 1], this.pixel[idx + 2], this.pixel[idx + 3]);
        }

        private Color Bilinear(double x, double y) {
            var cx1 = (int) Math.Floor(x);
            var cy1 = (int) Math.Floor(y);
            var cx2 = (int) Math.Round(x);
            var cy2 = (int) Math.Round(y);
            
            if (cx1 == cx2) cx2 = cx2 >= 1 ? cx2 - 1 : cx2;
            if (cy1 == cy2) cy2 = cy2 >= 1 ? cy2 - 1 : cy2;
            
            var c1 = this.GetIndex(cx1, cy1);
            var c2 = this.GetIndex(cx2, cy1);
            var c3 = this.GetIndex(cx1, cy2);
            var c4 = this.GetIndex(cx2, cy2);
            
            var dx = Math.Abs(x - (cx1 + 0.5));
            var dy = Math.Abs(y - (cy1 + 0.5));

            var w1 = (1 - dx) * (1 - dy);
            var w2 = dx * (1 - dy);
            var w3 = dy * (1 - dx);
            var w4 = dx * dy;

            var r = (byte)Math.Round(this.pixel[c1] * w1 + this.pixel[c2] * w2 + this.pixel[c3] * w3 + this.pixel[c4] * w4);
            var g = (byte)Math.Round(this.pixel[c1+1] * w1 + this.pixel[c2+1] * w2 + this.pixel[c3+1] * w3 + this.pixel[c4+1] * w4);
            var b = (byte)Math.Round(this.pixel[c1+2] * w1 + this.pixel[c2+2] * w2 + this.pixel[c3+2] * w3 + this.pixel[c4+2] * w4);
            var a = (byte)Math.Round(this.pixel[c1+3] * w1 + this.pixel[c2+3] * w2 + this.pixel[c3+3] * w3 + this.pixel[c4+3] * w4);

            return Color.Create(r, g, b, a);
        }

        private int GetIndex(int x, int y) {
            switch (this.wrapMode) {
                case WrapMode.Clamp:
                    x = this.Clamp(0, this.width - 1, x);
                    y = this.Clamp(0, this.height - 1, y);
                    break;;
                case WrapMode.Repeat:
                    x = x % this.width;
                    y = y % this.height;
                    break;
                default:
                    break;
            }
            return (y * this.width + x) * 4;
        }
        
        
        private int Clamp(int min, int max, int value) {
            return Math.Min(max, Math.Max(min, value));
        }
        
        private void InitFromLocalImage(string imagePath) {
            var image = Image.Load<Rgba32>(imagePath);
            var num = image.Width * image.Height * 4;
            var metadata = new byte[num];
            for (var y = 0; y < image.Height; y++) {
                for (var x = 0; x < image.Width; x++) {
                    var idx = (y * image.Width + x) * 4;
                    var rgba = image[x, y];
                    metadata[idx] = rgba.R;
                    metadata[idx+1] = rgba.G;
                    metadata[idx+2] = rgba.B;
                    metadata[idx+3] = rgba.A;
                }
            }
            this.width = image.Width;
            this.height = image.Height;
            this.pixel = metadata;
        }
 
        public void SaveImage() {
            Utils.SaveImage(this.width, this.height, this.pixel, 1);
        }
        
        
    }
    
    
}