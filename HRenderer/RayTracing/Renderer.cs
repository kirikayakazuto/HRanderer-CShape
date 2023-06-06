using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.RayTracing; 

/**
 * 光追离线渲染器
 */
public class Renderer {

    public static readonly Renderer instance = new Renderer(300, 300);

    private readonly int width;
    private readonly int height;

    private readonly Camera camera;
    private readonly Scene scene;

    Renderer(int width, int height) {
        this.width = width;
        this.height = height;
        this.scene = new Scene();
        this.camera = new Camera(width, height);
    }

    Vector4 GetRayColor(Ray ray) {
        var dir = ray.direction;
        var t = (dir.y + 1) * 0.5;
        return Vector4.Create(1, 1, 1, 1).MulSelf(1 - t).AddSelf(Vector4.Create(0.5, 0.7, 1.0, 1).MulSelf(t));
    }

    public void Render() {
        var data = new byte[this.width * this.height * 4];
        var origin = this.camera.GetPosition();
        for(var j=0; j<this.height; j++) {
            for(var i=0; i<this.width; i++) {
                var u = (double)i / (this.width - 1);
                var v = (double)j / (this.height - 1);
                var ray = this.camera.GetRay(u, v);
                var color = this.GetRayColor(ray);
                var idx = (this.width * j + i) * 4;
                data[idx] = (byte)(color.x * 255);
                data[idx+1] = (byte)(color.y * 255);
                data[idx+2] = (byte)(color.z * 255);
                data[idx+3] = (byte)(color.w * 255);
            }
        }
        Utils.SaveImage(this.width, this.height, data, 2);
    }
}