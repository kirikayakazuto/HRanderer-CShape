using System;
using HRenderer.Core;
using HRenderer.DrawTriangle;

namespace HRenderer {
    internal class Program {
        public static void Main(string[] args) {
            Console.WriteLine("hello world");
            
            Program.DoRender();
        }

        /**
         * 渲染测试
         */
        public static void DoRender() {
            var renderer = new Renderer(400, 400);
            renderer.materials.Add(Program.GetMaterial());
            
            renderer.Render();
        }

        public static Material GetMaterial() {
            var texture = new Texture("./Assets/001.jpg");
            var mesh = new SpriteMesh();
            var shader = new SpriteShader();
            return new Material(texture, mesh, shader);
        }
    }
}