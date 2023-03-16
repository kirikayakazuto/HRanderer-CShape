using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 在渲染管线中传递
 */
public class ShaderData {
    // 摄像机相关
    public Matrix4 view;
    public Matrix4 projection;

    // uniforms
    public Dictionary<string, float> uniformFloats = new Dictionary<string, float>();
    public Dictionary<string, Texture> uniformTextures = new Dictionary<string, Texture>();
    public Dictionary<string, Vector4> uniformVec4 = new Dictionary<string, Vector4>();
    
    // attribs
    public readonly Dictionary<string, Vector4> attribsVec4 = new Dictionary<string, Vector4>();
    public readonly Dictionary<string, Vector2> attribsVec2 = new Dictionary<string, Vector2>();
}