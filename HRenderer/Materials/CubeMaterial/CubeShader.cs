using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 


public class CubeShader: Shader {
	public override Vector4 VertexShading() {
		var position = this.attribsVec4Dict["a_position"];
		var r = Math.Sin(this.uniformDoubles["time"]) * Math.PI;
		// var r = Math.PI / 4;
		// position = position.Transform(Matrix4.GetRotationX(Math.PI));
		position = position.Transform(Matrix4.GetRotationY((float)r));
		position =	position.Transform(Matrix4.GetScale(1.5, 1.5, 1.5));
		var vpMat = this.projection.Mul(this.view);
		return position.TransformSelf(vpMat);
	}

	public override Vector4 FragShading() {
		// var uv = this.attribsVec2Dict["uv"];
		// return this.Texture2D(this.uniformTextures["mainTexture"], uv);
		return Vector4.Create(0.2, 0, 0, 1);
	}
}