using HRenderer.Common;

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
        public FilterMode filterMode;
        public WrapMode wrapMode;

        public Texture(int width, int height, byte[] pixel) {
            this.width = width;
            this.height = height;
            this.pixel = pixel;
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
    }
}