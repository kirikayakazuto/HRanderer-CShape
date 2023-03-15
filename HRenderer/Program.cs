using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

Console.WriteLine("Hello, World!");

// var texture = new Texture("./Assets/001.jpg");
// var mesh = new SpriteMesh();
// var shader = new SpriteShader();
// var material = new Material(texture, mesh, shader);
//
// var renderer = new Renderer(500, 500);
// renderer.materials.Add(material);
// renderer.Render();

var nativeWindowSettings = new NativeWindowSettings() {
    Size = new Vector2i(800, 600),
    Title = "LearnOpenTK - Textures",
    // This is needed to run on macos
    Flags = ContextFlags.ForwardCompatible,
};

using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings)) {
    window.Run();
}