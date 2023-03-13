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
    }
    
    public class Utils {
        public static float GetInterpValue3(float v1, float v2, float v3, float w1, float w2, float w3) {
            return v1 * w1 + v2 * w2 + v3 * w3;
        }
        
        public static Vector4 GetInterpVec4(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 w) {
            var vec = Vector4.Create();
            vec.x = Utils.GetInterpValue3(v1.x, v2.x, v3.x, w.x, w.y, w.z);
            vec.y = Utils.GetInterpValue3(v1.y, v2.y, v3.y, w.x, w.y, w.z);
            vec.z = Utils.GetInterpValue3(v1.z, v2.z, v3.z, w.x, w.y, w.z);
            vec.w = Utils.GetInterpValue3(v1.w, v2.w, v3.w, w.x, w.y, w.z);
            return vec;
        }
        
        public static Vector2 GetInterpVec2(Vector2 v1, Vector2 v2, Vector2 v3, Vector4 w) {
            var vec = Vector2.Create();
            vec.x = Utils.GetInterpValue3(v1.x, v2.x, v3.x, w.x, w.y, w.z);
            vec.y = Utils.GetInterpValue3(v1.y, v2.y, v3.y, w.x, w.y, w.z);
            return vec;
        }

        public static float GetDepth(float near, float far, float z) {
            return (1 / z - 1 / near) / (1 / far - 1 / near);
        }

        /**
         * 获取包围盒
         */
        public static Rect GetBoundingBox(Vector4 p1, Vector4 p2, Vector4 p3) {
            var minX = (int) Math.Floor(Math.Min(Math.Min(p1.x, p2.x), p3.x));
            var maxX = (int) Math.Ceiling(Math.Max(Math.Max(p1.x, p2.x), p3.x));
            
            var minY = (int) Math.Floor(Math.Min(Math.Min(p1.y, p2.y), p3.y));
            var maxY = (int) Math.Ceiling(Math.Max(Math.Max(p1.y, p2.y), p3.y));

            return new Rect() {minX = minX, maxX = maxX, minY = minY, maxY = maxY, width = maxX - minX, height = maxY - minY};
        }

        public static Vector4 GetBarycentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c) {
            var ac = a.Sub(c);
            var ab = a.Sub(b);
            var pa = p.Sub(a);
            var pb = p.Sub(b);
            var pc = p.Sub(c);

            var total = ab.Cross(ac);
            var alpha = pb.Cross(pc) / total;
            var beta = pc.Cross(pa) / total;
            var gamma = 1 - alpha - beta;

            return Vector4.Create(alpha, beta, gamma, 1);
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

        public static void AddDictionary() {
            
        }
    }
}