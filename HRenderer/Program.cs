﻿using HRenderer.Core;
using HRenderer.Materials.CubeMaterial;
using HRenderer.Materials.LightMaterial;
using HRenderer.Materials.SpriteMaterial;
using HRenderer.Materials.ModelMaterial;
using HRenderer.Materials.TriangleMaterial;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var program = new HRenderer.Program();

program.OpenWindow();

namespace HRenderer {
    public class Program {

        private const int _width = 600;
        private const int _height = 600;
        private const float _scale = 1f;
        
        public Program() {
        }
        
        public void OpenWindow() {
            var nativeWindowSettings = new NativeWindowSettings() {
                Size = new Vector2i((int)(_width * _scale), (int)(_height * _scale)),
                Title = "HRenderer",
                Flags = ContextFlags.ForwardCompatible,
            };
            
            var window = new Window(GameWindowSettings.Default, nativeWindowSettings, Game.instance);
            window.Run();
        }
    }
}


