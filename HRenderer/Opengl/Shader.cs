namespace HRenderer.Opengl; 

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class Shader {
    public readonly int Handle;

    private readonly Dictionary<string, int> _uniformLocations;
    
    public Shader(string vertPath, string fragPath) {
        // Load vertex shader and compile
        var shaderSource = File.ReadAllText(vertPath);

        // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);

        // Now, bind the GLSL source code
        GL.ShaderSource(vertexShader, shaderSource);

        // And then compile
        CompileShader(vertexShader);

        // We do the same for the fragment shader.
        shaderSource = File.ReadAllText(fragPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);
        CompileShader(fragmentShader);

        // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
        // To do this, create a program...
        Handle = GL.CreateProgram();

        // Attach both shaders...
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        // And then link them together.
        LinkProgram(Handle);

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
        // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
        // later.

        // First, we have to get the number of active uniforms in the shader.
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        // Next, allocate the dictionary to hold the locations.
        _uniformLocations = new Dictionary<string, int>();

        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(Handle, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(Handle, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }
    }

    private static void CompileShader(int shader)
    {
        // Try to compile the shader
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    }

    private static void LinkProgram(int program) {
        // We link the program
        GL.LinkProgram(program);

        // Check for linking errors
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({program})");
        }
    }

    // A wrapper function that enables the shader program.
    public void Use() {
        GL.UseProgram(Handle);
    }
    
    public int GetAttribLocation(string attribName) {
        return GL.GetAttribLocation(Handle, attribName);
    }
    
    public void SetInt(string name, int data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }
    
    public void SetFloat(string name, float data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }
    
    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(Handle);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }
    
    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(Handle);
        GL.Uniform3(_uniformLocations[name], data);
    }
}