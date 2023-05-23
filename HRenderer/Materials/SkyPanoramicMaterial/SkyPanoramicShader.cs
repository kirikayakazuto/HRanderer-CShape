using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SkyPanoramicMaterial; 

public class SkyPanoramicShader: Shader {

	public override Vector4 VertexShading(GlData glData) {
		var position = glData.attributes.Vec4s["position"];
		
		glData.varyingDict.Vec2s["v_uv"] = glData.attributes.Vec2s["uv"];
		glData.varyingDict.Vec4s["v_position"] = glData.attributes.Vec4s["position"];
		
		position = position.Transform(Matrix4.GetRotationX(Math.PI).MulSelf(Matrix4.GetScale(100, 100, 100)));
		var vpMat = this.projection.Mul(this.view);
        
		return position.TransformSelf(vpMat);
	}
	public override Vector4 FragShading() {
		var pos = this.varyingDict.Vec4s["v_position"];
		var uv = Vector2.Create(Math.Atan2(pos.x, pos.z) / (2.0f * Math.PI) + 0.5f, Math.Asin(pos.y) / Math.PI + 0.5f);
		var color = this.Texture2D(this.uniformData.Textures["mainTexture"], uv);
		return color;
	}
}