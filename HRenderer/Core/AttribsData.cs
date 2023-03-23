using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 在渲染管线中传递
 */
public class AttribsData {
	// 差值varying
	public readonly VectorDict VectorDict1 = new VectorDict();
	public readonly VectorDict VectorDict2 = new VectorDict();
	public readonly VectorDict VectorDict3 = new VectorDict();
}

public class VaryingData {
	// 差值varying
	public readonly VectorDict VectorDict1 = new VectorDict();
	public readonly VectorDict VectorDict2 = new VectorDict();
	public readonly VectorDict VectorDict3 = new VectorDict();
	
	public void DoVarying(in Shader shader, in Vector4 barycentric) {
		var vec4keys = this.VectorDict1.Vec4Dict.Keys;
		foreach (var key in vec4keys) {
			var vec4 = shader.varyingVec4Dict.ContainsKey(key) ? shader.varyingVec4Dict[key] : Vector4.Create(); 
			Utils.GetInterpVec4(this.VectorDict1.Vec4Dict[key], this.VectorDict2.Vec4Dict[key], this.VectorDict3.Vec4Dict[key], barycentric, vec4);
			shader.varyingVec4Dict[key] = vec4;
		}
		var vec2keys = this.VectorDict1.Vec2Dict.Keys;
		foreach (var key in vec2keys) {
			var vec2 = shader.varyingVec2Dict.ContainsKey(key) ? shader.varyingVec2Dict[key] : Vector2.Create(); 
			Utils.GetInterpVec2(this.VectorDict1.Vec2Dict[key], this.VectorDict2.Vec2Dict[key], this.VectorDict3.Vec2Dict[key], barycentric, vec2);
			shader.varyingVec2Dict[key] = vec2;
		}
	}
}

public class VectorDict {
	public readonly Dictionary<string, Vector4> Vec4Dict = new Dictionary<string, Vector4>();
	public readonly Dictionary<string, Vector2> Vec2Dict = new Dictionary<string, Vector2>();
}