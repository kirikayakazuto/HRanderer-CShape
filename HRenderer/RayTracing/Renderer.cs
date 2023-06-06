using HRenderer.Common;

namespace HRenderer.RayTracing; 

/**
 * 光追离线渲染器
 */
public class Renderer {

    public static readonly Renderer instance = new Renderer(500, 500);

    private readonly int width;
    private readonly int height;

    private readonly Camera camera;
    private readonly Scene scene;

    private readonly int sampleLines = 20;

    private readonly byte[] data;

    Renderer(int width, int height) {
        this.width = width;
        this.height = height;
        this.scene = new Scene();
        this.camera = new Camera();
        this.data = new byte[this.width * this.height * 4];
    }

    Vector4 GetRayColor(Ray ray, int depth) {
        var hitInfo = new HitInfo();

        if(depth <= 0) return Vector4.Create(0, 0, 0, 1);

        if(this.scene.HitTest(ray, 0.001, 100, ref hitInfo)) {
            var target = hitInfo.position.Add(hitInfo.normal).AddSelf(this.RandomVec4()).NormalizeSelf();
            // return hitInfo.normal.Add(Vector4.Create(1, 1, 1, 1)).MulSelf(0.5);
            return this.GetRayColor(new Ray(hitInfo.position, target.SubSelf(hitInfo.position)), depth-1);
        }
        var dir = ray.direction;
        var t = (dir.y + 2) * 0.5;
        return Vector4.Create(1, 1, 1, 1).MulSelf(1 - t).AddSelf(Vector4.Create(0.5, 0.7, 1.0, 1).MulSelf(t));
    }

    public void Render() {
        var origin = this.camera.GetPosition();
        Console.WriteLine("width: " + this.width + "height: " + this.height);
        for(var j=0; j<this.height; j++) {
            for(var i=0; i<this.width; i++) {
                var color = Vector4.Create(0, 0, 0, 0);
                for(int s=0; s<this.sampleLines; s++) {
                    var u = (i + Utils.RandomZoreToOne()) / (this.width - 1);
                    var v = (j + Utils.RandomZoreToOne()) / (this.height - 1);
                    var ray = this.camera.GetRay(u, v);
                    var tmpColor = this.GetRayColor(ray, 10);
                    color.AddSelf(tmpColor);
                }
                
                color.MulSelf(1f / this.sampleLines);
                color.SqrtSelf();
                
                var idx = (this.width * j + i) * 4;
                this.data[idx] = (byte)(color.x * 255);
                this.data[idx+1] = (byte)(color.y * 255);
                this.data[idx+2] = (byte)(color.z * 255);
                this.data[idx+3] = (byte)(color.w * 255);
            }
        }
        Utils.SaveImage(this.width, this.height, this.data, 3);
    }

    public Vector4 RandomVec4() {
        while(true) {
            var vec = Utils.RandomVec4();
            if(vec.GetLengthSquared() >= 1) continue;
            return vec;
        }
    }
}