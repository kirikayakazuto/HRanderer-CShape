using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 在渲染管线中传递
 */
public class CacheData {
	// 缓存 vec4
	public readonly Dictionary<string, Vector4> Vec4Attribs1 = new Dictionary<string, Vector4>();
	public readonly Dictionary<string, Vector4> Vec4Attribs2 = new Dictionary<string, Vector4>();
	public readonly Dictionary<string, Vector4> Vec4Attribs3 = new Dictionary<string, Vector4>();
	// 缓存 vec2
	public readonly Dictionary<string, Vector2> Vec2Attribs1 = new Dictionary<string, Vector2>();
	public readonly Dictionary<string, Vector2> Vec2Attribs2 = new Dictionary<string, Vector2>();
	public readonly Dictionary<string, Vector2> Vec2Attribs3 = new Dictionary<string, Vector2>();
}