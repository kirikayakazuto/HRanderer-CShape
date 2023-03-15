using HRenderer.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Shader = HRenderer.Opengl.Shader;
using Texture = HRenderer.Opengl.Texture;

public class Window : GameWindow {
    private readonly float[] _vertices = {
        // Position         Texture coordinates
         1f,  1f, 0.0f, 1.0f, 1.0f, // top right
         1f, -1f, 0.0f, 1.0f, 0.0f, // bottom right
        -1f, -1f, 0.0f, 0.0f, 0.0f, // bottom left
        -1f,  1f, 0.0f, 0.0f, 1.0f  // top left
    };

    private readonly uint[] _indices = {
        0, 1, 3,
        1, 2, 3
    };

    private int _elementBufferObject;

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    // For documentation on this, check Texture.cs.
    private Texture _texture;

    private Renderer _renderer;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, Renderer renderer): base(gameWindowSettings, nativeWindowSettings) {
        this._renderer = renderer;
    }
    
    protected override void OnLoad() {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        _shader = new Shader("Opengl/Shaders/shader.vert", "Opengl/Shaders/shader.frag");
        _shader.Use();
        
        var vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        
        var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _texture = new Texture();
        _texture.Use(TextureUnit.Texture0);
    }

    protected override void OnRenderFrame(FrameEventArgs e) {
        base.OnRenderFrame(e);
        
        this._renderer.Render(e.Time);
        var frame = this._renderer.frameBuffer;
        this._texture.UpdateData(frame.Width, frame.Height, frame.Pixels);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(_vertexArrayObject);

        _texture.Use(TextureUnit.Texture0);
        _shader.Use();

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e) {
        base.OnUpdateFrame(e);
        var input = KeyboardState;
        if (input.IsKeyDown(Keys.Escape)) {
            Close();
        }
    }
}