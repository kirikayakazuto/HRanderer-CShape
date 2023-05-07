using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 


public class CubeShader: Shader {
	public override Vector4 VertexShading(VectorDict attribsDict, VectorDict varyingDict) {
		var position = attribsDict.Vec4s["a_position"];
		var r = Math.Sin(this.uniformDoubles["time"]) * Math.PI;
		// var r = Math.PI / 4;
		// position = position.Transform(Matrix4.GetRotationX(Math.PI));
		position = position.Transform(Matrix4.GetRotationY((float)r));
		position =	position.Transform(Matrix4.GetScale(3, 3, 3));
		
		var normal = attribsDict.Vec4s["a_normal"].Transform(Matrix4.GetRotationY((float)r));
		varyingDict.Vec4s["v_normal"] = normal;
		var v_position = position.Clone();
		varyingDict.Vec4s["v_position"] = v_position;

		var v_uv = attribsDict.Vec2s["a_uv"];
		varyingDict.Vec2s["v_uv"] = v_uv;

		var vpMat = this.projection.Mul(this.view);
		return position.TransformSelf(vpMat);
	}

	public override Vector4 FragShading() {
		var lightColor = this.uniformVec4["Light.Color"];
		
		var uv = this.varyingVec2Dict["v_uv"];
		var lightDir = this.uniformVec4["Light.Position"].Sub(this.varyingVec4Dict["v_position"]).NormalizeSelf();
		var norm = this.varyingVec4Dict["v_normal"].Normalize();
		var cameraPos = this.uniformVec4["Camera.Position"];
		var viewDir = cameraPos.Sub(this.varyingVec4Dict["v_position"]).NormalizeSelf();
		var reflectDir = Utils.Reflect(lightDir.Mul(-1), norm);
		
		// 环境光
		const double ambientStrength = 0.2;
		// var ambient = this.uniformVec4["Light.Color"].Mul(ambientStrength);
		var ambient = lightColor.Mul(this.Texture2D(this.uniformTextures["mainTexture"], uv)).Mul(ambientStrength);
		
		// 漫反射
		var diff = Math.Max(norm.Dot(lightDir), 0);
		// var diffuse = this.uniformVec4["Light.Color"].Mul(diff);
		var diffuse = this.Texture2D(this.uniformTextures["mainTexture"], uv).Mul(lightColor).Mul(diff);
		
		// 镜面反射
		var spec = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0.0), 32);
		const double specularStrength = 0.9;
		// var specular = lightColor.Mul(specularStrength * spec);
		var specular = lightColor.Mul(this.Texture2D(this.uniformTextures["specularTexture"], uv)).Mul(spec * specularStrength);
		
		// var objectColor = Vector4.Create(1, 0.5, 0.31, 1);

		return ambient.AddSelf(diffuse).AddSelf(specular).Clamp();
	}
}