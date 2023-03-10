using System;

namespace HRenderer {
    internal class Program {
        private static System.Timers.Timer aTimer;
        public static void Main(string[] args) {
            Console.WriteLine("hello world");
            
            Program.aTimer = new System.Timers.Timer(500);
            Program.aTimer.Elapsed += Program.Loop;
            Program.aTimer.AutoReset = true;
            Program.aTimer.Enabled = true;
            Program.aTimer.Start();
            
            // 防止程序结束
            Console.ReadLine();
        }
        
        private static void Loop(Object source, System.Timers.ElapsedEventArgs e) {
            //this.Draw();
            Program.aTimer.Stop();
            Console.WriteLine("call loop");
            Program.aTimer.Start();
        }
    }
}