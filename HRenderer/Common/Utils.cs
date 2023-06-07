using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HRenderer.Common {

    public class Rect {
        public int minX;
        public int maxX;

        public int minY;
        public int maxY;

        public int width;
        public int height;

        public override string ToString() {
            return "minX: " + minX + " maxX: " + maxX + " minY: " + minY + " maxY: " + maxY;
        }
    }
    
    public class Utils {
        public static double GetInterpValue3(double v1, double v2, double v3, double w1, double w2, double w3) {
            return v1 * w1 + v2 * w2 + v3 * w3;
        }
        
        public static Vector4 GetInterpVec4(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 w, in Vector4 outVec) {
            var vec = outVec;
            vec.x = Utils.GetInterpValue3(v1.x, v2.x, v3.x, w.x, w.y, w.z);
            vec.y = Utils.GetInterpValue3(v1.y, v2.y, v3.y, w.x, w.y, w.z);
            vec.z = Utils.GetInterpValue3(v1.z, v2.z, v3.z, w.x, w.y, w.z);
            vec.w = Utils.GetInterpValue3(v1.w, v2.w, v3.w, w.x, w.y, w.z);
            return vec;
        }
        
        public static Vector2 GetInterpVec2(Vector2 v1, Vector2 v2, Vector2 v3, Vector4 w, in Vector2 outVec) {
            var vec = outVec;
            vec.x = Utils.GetInterpValue3(v1.x, v2.x, v3.x, w.x, w.y, w.z);
            vec.y = Utils.GetInterpValue3(v1.y, v2.y, v3.y, w.x, w.y, w.z);
            return vec;
        }

        public static double GetDepth(double near, double far, double z) {
            return (1 / z - 1 / near) / (1 / far - 1 / near);
        }

        /**
         * 获取包围盒
         */
        public static Rect GetBoundingBox(Vector4 p1, Vector4 p2, Vector4 p3, int width, int height) {
            var minX = (int) Math.Floor(Math.Min(Math.Min(p1.x, p2.x), p3.x));
            var maxX = (int) Math.Ceiling(Math.Max(Math.Max(p1.x, p2.x), p3.x));
            
            var minY = (int) Math.Floor(Math.Min(Math.Min(p1.y, p2.y), p3.y));
            var maxY = (int) Math.Ceiling(Math.Max(Math.Max(p1.y, p2.y), p3.y));

            minX = Math.Max(minX, 0);
            maxX = Math.Min(maxX, width);
            
            minY = Math.Max(minY, 0);
            maxY = Math.Min(maxY, height);
            
            return new Rect() {minX = minX, maxX = maxX, minY = minY, maxY = maxY, width = maxX - minX, height = maxY - minY};
        }

        public static double Cross(double x1, double y1, double x2, double y2) {
            return x1 * y2 - x2 * y1;
        }

        public static Vector4 Reflect(Vector4 i, Vector4 n) {
            return i.Sub(n.Mul(n.Dot(i)).Mul(2.0));
        }

        public static Vector4 Refract(Vector4 i, Vector4 n, double etaiOverEtat) {
            var cosTheta = Math.Min(i.Mul(-1).Dot(n), 1);
            var rOutPerp = i.Add(n.Mul(cosTheta)).MulSelf(etaiOverEtat);
            var rOutParallel = n.Mul(-Math.Sqrt(1 - rOutPerp.GetLengthSquared()));
            return rOutPerp.AddSelf(rOutParallel);
        }

        /**
         * 校正重心差值
         */
        public static Vector4 AdjustBarycentric(Vector4 barycentric, double z1, double z2, double z3) {
            var rz = 1 / (1 / z1 * barycentric.x + 1 / z2 * barycentric.y + 1 / z3 * barycentric.z);
            barycentric.x = barycentric.x / z1 * rz;
            barycentric.y = barycentric.y / z2 * rz;
            barycentric.z = barycentric.z / z3 * rz;
            return barycentric;
        }

        public static Vector4 GetBarycentric(Vector2 p, Vector4 position1, Vector4 position2, Vector4 position3, in Vector4 outVec) {
            var a = Vector2.Create(position1.x, position1.y);
            var b = Vector2.Create(position2.x, position2.y);
            var c = Vector2.Create(position3.x, position3.y);
            
            var ac = a.Sub(c);
            var ab = a.Sub(b);
            var pa = p.Sub(a);
            var pb = p.Sub(b);
            var pc = p.Sub(c);

            var total = ab.Cross(ac);
            var alpha = pb.Cross(pc) / total;
            var beta = pc.Cross(pa) / total;
            var gamma = 1 - alpha - beta;
            
            Vector2.Return(a);
            Vector2.Return(b);
            Vector2.Return(c);

            outVec.Set(alpha, beta, gamma, 1);
            return outVec;
        }

        public static void SaveImage(int width, int height, byte[] data, int frame) {
            var image = new Image<Rgba32>(width, height);
            for (var y = 0; y < image.Height; y++) {
                for (var x = 0; x < image.Width; x++) {
                    var idx = (y * image.Width + x) * 4;
                    image[x, y] = new Rgba32(data[idx], data[idx + 1], data[idx + 2], data[idx + 3]);
                }
            }
            if (!Directory.Exists("./output")) {
                Directory.CreateDirectory("./output");
            }
            image.SaveAsJpeg("./output/" + frame + ".jpg");
        }
        
        public static Matrix4 GetViewPortMatrix(int width, int height) {
            var m = Matrix4.GetIdentify();
            m.data[0] = width / 2.0f;
            m.data[5] = height / 2.0f;
            m.data[12] = width / 2.0f;
            m.data[13] = height / 2.0f;
            return m;
        }
        
        public static int Clamp(int min, int max, int value) {
            return Math.Min(max, Math.Max(min, value));
        }

        private static Random _random = new Random();
        public static double RandomZoreToOne() {
            return Utils._random.NextDouble();
        }

        public static double Random(double min, double max) {
            return min + (max - min) * Utils.RandomZoreToOne();
        }

        public static Vector4 RandomVec4() {
            return Vector4.Create(Utils.Random(-1, 1), Utils.Random(-1, 1), Utils.Random(-1, 1), 1);
        }

        public static Vector4 RandomInUnitSphere() {
            while(true) {
                var vec = Utils.RandomVec4();
                if(vec.GetLengthSquared() >= 1) continue;
                return vec;
            }
        }
    }
}