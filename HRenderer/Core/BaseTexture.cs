using HRenderer.Common;

namespace HRenderer.Core; 

public abstract class BaseTexture {
    public int width;
    public int height;
    private FilterMode filterMode = FilterMode.Linear;
    protected WrapMode wrapMode = WrapMode.Clamp;

    public Vector4 Sample(double u, double v) {
        var x = u * (this.width - 1) + 0.5;
        var y = this.height - (v * (this.height - 1) + 0.5);
        // var y =  v * (this.height - 1) + 0.5;
        switch (this.filterMode) {
            case FilterMode.Nearest:
                return this.Nearest(x, y);
            case FilterMode.Linear:
                return this.Bilinear(x, y);
            default:
                return Vector4.Create(0, 0, 0, 0);
        }
    }

    public abstract Vector4 Nearest(double x, double y);

    public abstract Vector4 Bilinear(double x, double y);

    public abstract int GetIndex(int x, int y);
    
    public void SetFilterMode(FilterMode filter) {
        this.filterMode = filter;
    }

    public void SetWrapMode(WrapMode wrap) {
        this.wrapMode = wrap;
    }
}