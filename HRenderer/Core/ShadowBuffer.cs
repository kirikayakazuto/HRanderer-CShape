namespace HRenderer.Core; 

public class ShadowBuffer {
    public readonly int width;
    public readonly int height;
    
    // 像素buffer  r_g_b_a格式
    private readonly double[] _buffer;
    
    public ShadowBuffer(int width, int height) {
        this.width = width;
        this.height = height;

        this._buffer = new double[width * height];
    }

    public void From(double[] buffer) {
        this._buffer.CopyTo(buffer, 0);
    }


    public void Set(int x, int y, double z) {
        if(x<0 || x>=this.width) return;
        if(y<0 || y>=this.height) return;
        
        var idx = (x + y * this.width);
        this._buffer[idx] = z;
    }

    public double Get(int x, int y) {
        var idx = (x + y * this.width);
        return this._buffer[idx];
    }
    
    public void Clear() {
        for (var i = 0; i < this._buffer.Length; i++) {
            this._buffer[i] = 1;
        }
    }
            
}