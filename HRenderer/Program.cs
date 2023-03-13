using System;
using HRenderer.Core;

namespace HRenderer {
    internal class Program {
        private static System.Timers.Timer aTimer;
        public static void Main(string[] args) {
            Console.WriteLine("hello world");
            
            var texture = new Texture("./Assets/001.jpg");
            Console.WriteLine(texture.width);
            Console.WriteLine(texture.height);
            
            texture.SaveImage();
            // 防止程序结束
            // Console.ReadLine();
        }
    }
}