using HRenderer.Core;
using HRenderer.Materials.SpriteMaterial;
using HRenderer.Materials.ModelMaterial;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var program = new HRenderer.Program();

program.OpenWindow();

namespace HRenderer {
    public class Program {

        private const int _width = 400;
        private const int _height = 400;

        private readonly Renderer _renderer;
        public Program() {
            this._renderer = new Renderer(_width, _height);
            this.RunScene3();
        }

        public void RunScene1() {
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial());
        }

        public void RunScene2() {
            this._renderer.AddMaterial(ModelMaterial.GetMaterial());
        }

        public void RunScene3() {
            this._renderer.AddMaterial(SpriteMaterial.GetMaterial());
            this._renderer.AddMaterial(ModelMaterial.GetMaterial());
        }


        public void OpenWindow() {
            var nativeWindowSettings = new NativeWindowSettings() {
                Size = new Vector2i(_width, _height),
                Title = "HRenderer",
                Flags = ContextFlags.ForwardCompatible,
            };

            var window = new Window(GameWindowSettings.Default, nativeWindowSettings, this._renderer);
            window.Run();
        }
    }
}


