using HRenderer.Common;

namespace HRenderer.RayTracing; 

/**
 * 光追离线渲染器
 */
public class Renderer {

    public static readonly Renderer instance = new Renderer(800, 600);

    private readonly int width;
    private readonly int height;

    private readonly Camera camera;
    private readonly Scene scene;

    private readonly int sampleLines = 500;

    private readonly int depth = 20;

    private readonly byte[] data;

    Renderer(int width, int height) {
        this.width = width;
        this.height = height;
        this.scene = new Scene();
        this.camera = new Camera((double)width / height);
        this.data = new byte[this.width * this.height * 4];
    }

    private Vector4 GetRayColor(Ray ray, int depth) {
        var hitInfo = new HitInfo();

        if(depth <= 0) return Vector4.Create(0, 0, 0, 0);

        if(this.scene.HitTest(ray, 0.001, int.MaxValue, ref hitInfo)) {
            if (hitInfo.material.Scatter(ray, ref hitInfo, out var attenuation, out var rayOut)) {
                return attenuation.MulSelf(this.GetRayColor(rayOut, depth - 1));
            }
            return Vector4.Create(0, 0, 0, 0);
        }
        var dir = ray.direction.Normalize();
        var t = (dir.y + 1) * 0.5;
        return Vector4.Create(1, 1, 1, 1).MulSelf(1 - t).AddSelf(Vector4.Create(0.5, 0.7, 1.0, 1).MulSelf(t));
    }

    public void Render() {
        var origin = this.camera.GetPosition();
        Console.WriteLine("width: " + this.width + "height: " + this.height);
        for(var j=0; j<this.height; j++) {
            for(var i=0; i<this.width; i++) {
                var color = Vector4.Create(0, 0, 0, 0);
                for(var s=0; s<this.sampleLines; s++) {
                    var u = (i + Utils.RandomZoreToOne()) / (this.width - 1);
                    var v = (j + Utils.RandomZoreToOne()) / (this.height - 1);
                    var ray = this.camera.GetRay(u, v);
                    var tmpColor = this.GetRayColor(ray, this.depth);
                    color.AddSelf(tmpColor);
                    Vector4.Return(tmpColor);
                }
                
                color.MulSelf(1f / this.sampleLines).SqrtSelf();

                var idx = (this.width * j + i) * 4;
                this.data[idx] = (byte)(color.x * 255);
                this.data[idx+1] = (byte)(color.y * 255);
                this.data[idx+2] = (byte)(color.z * 255);
                this.data[idx+3] = (byte)(color.w * 255);
                Vector4.Return(color);
            }
            Console.WriteLine("lines: " + j);
        }
        Utils.SaveImage(this.width, this.height, this.data, 3);
    }
}