using HRenderer.Common;

namespace HRenderer.Core; 

public class ShadowTexture: BaseTexture {
    private readonly double[] buffer;
    public ShadowTexture(int width, int height): base() {
        this.width = width;
        this.height = height;

        this.buffer = new double[width * height];
    }

    public void From(double[] _buffer) {
        _buffer.CopyTo(this.buffer, 0);
    }
    
    public override Vector4 Nearest(double x, double y) {
        var cx = (int)Math.Floor(x);
        var cy = (int)Math.Floor(y);
        var idx = this.GetIndex(cx, cy);
        return Vector4.Create(this.buffer[idx], this.buffer[idx], this.buffer[idx], 1);
    }

    public override Vector4 Bilinear(double x, double y) {
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
        
        var res = this.buffer[c1] * w1 + this.buffer[c2] * w2 + this.buffer[c3] * w3 + this.buffer[c4] * w4;
        return Vector4.Create(res, res, res, 1);
    }

    public override int GetIndex(int x, int y) {
        switch (this.wrapMode) {
            case WrapMode.Clamp:
                x = Utils.Clamp(0, this.width - 1, x);
                y = Utils.Clamp(0, this.height - 1, y);
                break;;
            case WrapMode.Repeat:
                x = x % this.width;
                y = y % this.height;
                break;
            default:
                break;
        }
        return (y * this.width + x);
    }
}