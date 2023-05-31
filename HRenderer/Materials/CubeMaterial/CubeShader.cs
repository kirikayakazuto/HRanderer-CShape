using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.CubeMaterial; 


public class CubeShader: Shader {
	public override Vector4 VertexShading(GlData glData) {
		var position = glData.attributes.Vec4s["a_position"];
		var r = Math.Sin(this.uniformData.Doubles["time"] * 0.1) * Math.PI;
		// var r = Math.PI / 4;
		// position = position.Transform(Matrix4.GetRotationX(Math.PI));
		position = position.Transform(Matrix4.GetRotationY(r));
		position =	position.Transform(Matrix4.GetScale(3, 3, 3));
		
		var normal = glData.attributes.Vec4s["a_normal"].Transform(Matrix4.GetRotationY(r));
		glData.varyingDict.Vec4s["v_normal"] = normal;
		var v_position = position.Clone();
		glData.varyingDict.Vec4s["v_position"] = v_position;

		var v_uv = glData.attributes.Vec2s["a_uv"];
		glData.varyingDict.Vec2s["v_uv"] = v_uv;

		var vpMat = this.projection.Mul(this.view);
		return position.TransformSelf(vpMat);
	}

	public override Vector4 FragShading() {
		var lightColor = this.uniformData.Vec4s["Light.Color"];
		
		var uv = this.varyingDict.Vec2s["v_uv"];
		var lightDir = this.uniformData.Vec4s["Light.Position"].Sub(this.varyingDict.Vec4s["v_position"]).NormalizeSelf();
		var norm = this.varyingDict.Vec4s["v_normal"].Normalize();
		var cameraPos = this.uniformData.Vec4s["Camera.Position"];
		var viewDir = cameraPos.Sub(this.varyingDict.Vec4s["v_position"]).NormalizeSelf();
		var reflectDir = Utils.Reflect(lightDir.Mul(-1), norm);
		
		// 环境光
		const double ambientStrength = 0.6;
		// var ambient = this.uniformVec4["Light.Color"].Mul(ambientStrength);
		var ambient = lightColor.Mul(this.Texture2D(this.uniformData.Textures["mainTexture"], uv)).Mul(ambientStrength);
		
		// 漫反射
		var diff = Math.Max(norm.Dot(lightDir), 0);
		// var diffuse = this.uniformVec4["Light.Color"].Mul(diff);
		var diffuse = this.Texture2D(this.uniformData.Textures["mainTexture"], uv).Mul(lightColor).Mul(diff);
		
		// 镜面反射
		var spec = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0.0), 32);
		const double specularStrength = 0.9;
		// var specular = lightColor.Mul(specularStrength * spec);
		var specular = lightColor.Mul(this.Texture2D(this.uniformData.Textures["specularTexture"], uv)).Mul(spec * specularStrength);
		
		// var objectColor = Vector4.Create(1, 0.5, 0.31, 1);

		return ambient.AddSelf(diffuse).AddSelf(specular).Clamp();

		// var p = this.varyingDict.Vec4s["v_position"].Clone();
		// p.HomogenenizeInvertion();
		// return Vector4.Create(p.z, p.z, p.z, 1);
	}
}