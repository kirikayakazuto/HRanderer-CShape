using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 在渲染管线中传递
 */

public class UniformData {
	public readonly Dictionary<string, Matrix4> Matrix4s = new Dictionary<string, Matrix4>();
	public readonly Dictionary<string, BaseTexture> Textures = new Dictionary<string, BaseTexture>();
	public readonly Dictionary<string, Vector4> Vec4s = new Dictionary<string, Vector4>();
	public readonly Dictionary<string, double> Doubles = new Dictionary<string, double>();
}

public class VectorDict {
	public readonly Dictionary<string, Vector4> Vec4s = new Dictionary<string, Vector4>();
	public readonly Dictionary<string, Vector2> Vec2s = new Dictionary<string, Vector2>();
}