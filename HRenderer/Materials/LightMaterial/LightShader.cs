using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.LightMaterial; 

public class LightShader: Shader {
	public override Vector4 VertexShading(GlData glData) {
		var position = glData.attributes.Vec4s["a_position"];
		var lightPos = this.uniformData.Vec4s["Light.Position"].Mul(1);
		position = position.Add(lightPos);
		
		var vpMat = this.projection.Mul(this.view);
		return position.Transform(vpMat);
	}

	public override Vector4 FragShading() {
		return Vector4.Create(1, 1, 1, 1);
	}
}