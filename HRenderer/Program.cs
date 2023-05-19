using HRenderer.Core;
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

        private readonly Renderer _renderer;
        public Program() {
            this._renderer = new Renderer(_width, _height);
            this.RunScene6();
        }

        public void RunScene6() {
            
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial());
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial3());
        }

        public void RunScene5() {
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial2());
            this._renderer.AddMaterial(TriangleMaterial.GetMaterial());
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial());
        }

        public void RunScene0() {
            this._renderer.AddMaterial(TriangleMaterial.GetMaterial());
        }

        public void RunScene4() {
            this._renderer.AddMaterial(LightMaterial.GetMaterial());
            this._renderer.AddMaterial(CubeMaterial.GetMaterial());
        }

        public void RunScene1() {
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial());
        }

        public void RunScene2() {
            // this._renderer.AddMaterial(LightMaterial.GetMaterial());
            this._renderer.AddMaterial(ModelMaterial.GetMaterial());
        }

        public void RunScene3() {
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial());
            this._renderer.AddMaterial(ModelMaterial.GetMaterial());
        }


        public void OpenWindow() {
            var nativeWindowSettings = new NativeWindowSettings() {
                Size = new Vector2i((int)(_width * _scale), (int)(_height * _scale)),
                Title = "HRenderer",
                Flags = ContextFlags.ForwardCompatible,
            };

            var window = new Window(GameWindowSettings.Default, nativeWindowSettings, this._renderer);
            window.Run();
        }
    }
}


