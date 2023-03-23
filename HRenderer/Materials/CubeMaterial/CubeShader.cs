using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 


public class CubeShader: Shader {
	public override Vector4 VertexShading(VectorDict attribsDict, VectorDict varyingDict) {
		var position = attribsDict.Vec4Dict["a_position"];
		var r = Math.Sin(this.uniformDoubles["time"]) * Math.PI;
		// var r = Math.PI / 4;
		// position = position.Transform(Matrix4.GetRotationX(Math.PI));
		position = position.Transform(Matrix4.GetRotationY((float)r));
		position =	position.Transform(Matrix4.GetScale(3, 3, 3));
		
		var normal = attribsDict.Vec4Dict["a_normal"].Transform(Matrix4.GetRotationY((float)r));
		varyingDict.Vec4Dict["v_normal"] = normal;
		var v_position = position.Clone();
		varyingDict.Vec4Dict["v_position"] = v_position;

		var vpMat = this.projection.Mul(this.view);
		return position.TransformSelf(vpMat);
	}

	public override Vector4 FragShading() {
		var lightDir = this.uniformVec4["Light.Position"].Sub(this.varyingVec4Dict["v_position"]).NormalizeSelf();
		var norm = this.varyingVec4Dict["v_normal"].Normalize();
		var diff = Math.Max(norm.Dot(lightDir), 0);
		var diffuse = this.uniformVec4["Light.Color"].Mul(diff);
		
		var ambientStrength = 0.1;
		var ambient = this.uniformVec4["Light.Color"].Mul(ambientStrength);
		
		var objectColor = Vector4.Create(1, 0.5, 0.31, 1);
		return ambient.AddSelf(diffuse).Clamp().Mul(objectColor);
	}
}