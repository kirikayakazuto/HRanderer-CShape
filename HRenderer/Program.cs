using HRenderer.Core;
using HRenderer.DrawModel;
using HRenderer.DrawTriangle;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var program = new HRenderer.Program();

program.OpenWindow();

namespace HRenderer {
    public class Program {

        private readonly Renderer _renderer;
        public Program() {
            this._renderer = new Renderer(500, 500);
            this.RunScene2();
        }

        public void RunScene1() {
            var texture = new Texture("./Assets/001.jpg");
            var mesh = new SpriteMesh();
            var shader = new SpriteShader();
            var material = new Material(texture, mesh, shader);
            this._renderer.materials.Add(material);
        }

        public void RunScene2() {
            var texture = new Texture("./Assets/african/african_head_diffuse.png");
            var mesh = new ModelMesh();
            var shader = new ModelShader();
            var material = new Material(texture, mesh, shader);
            this._renderer.materials.Add(material);
        }

        public void OpenWindow() {
            var nativeWindowSettings = new NativeWindowSettings() {
                Size = new Vector2i(500, 500),
                Title = "HRenderer",
                Flags = ContextFlags.ForwardCompatible,
            };

            var window = new Window(GameWindowSettings.Default, nativeWindowSettings, this._renderer);
            window.Run();
        }
    }
}


