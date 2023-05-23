using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SphereMaterial; 

public class SphereShader: Shader {
        
    public override Vector4 VertexShading(GlData glData) {
        var position = glData.attributes.Vec4s["position"];;
        var r = Math.PI * this.uniformData.Doubles["time"] * 0.1;

        // position = position.Transform(Matrix4.GetRotationX(Math.PI));
        position = position.Transform(Matrix4.GetRotationY(r));
        position.TransformSelf(Matrix4.GetScale(4, 4, 4));
        
        var normal = glData.attributes.Vec4s["normal"].Transform(Matrix4.GetRotationY(r));
        glData.varyingDict.Vec4s["v_normal"] = normal;
        
        glData.varyingDict.Vec2s["v_uv"] = glData.attributes.Vec2s["uv"].Clone();
        glData.varyingDict.Vec4s["v_position"] = glData.attributes.Vec4s["position"].Clone();
        
        
        // position = position.Transform(Matrix4.GetRotationX(Math.PI));
        
        var vpMat = this.projection.Mul(this.view);
        return position.TransformSelf(vpMat);
    }

    public override Vector4 FragShading() {
        
        var lightColor = this.uniformData.Vec4s["Light.Color"];
		
        // var uv = this.varyingDict.Vec2s["v_uv"];

        var pos = this.varyingDict.Vec4s["v_position"];
        var uv = Vector2.Create(Math.Atan2(pos.x, pos.z) / (2.0f * Math.PI) + 0.5f, Math.Asin(pos.y) / Math.PI + 0.5f);
            
        var lightDir = this.uniformData.Vec4s["Light.Position"].Sub(this.varyingDict.Vec4s["v_position"]).NormalizeSelf();
        var norm = this.varyingDict.Vec4s["v_normal"].Normalize();
        var cameraPos = this.uniformData.Vec4s["Camera.Position"];
        var viewDir = cameraPos.Sub(this.varyingDict.Vec4s["v_position"]).NormalizeSelf();
        var reflectDir = Utils.Reflect(lightDir.Mul(-1), norm);
		
        // 环境光
        const double ambientStrength = 0.4;
        var ambient = lightColor.Mul(this.Texture2D(this.uniformData.Textures["mainTexture"], uv)).Mul(ambientStrength);
        
        // 漫反射
        var diff = Math.Max(norm.Dot(lightDir), 0);
        var diffuse = this.Texture2D(this.uniformData.Textures["mainTexture"], uv).Mul(lightColor).Mul(diff);
        
        // 镜面反射
        var spec = Math.Pow(Math.Max(viewDir.Dot(reflectDir), 0.0), 32);
        const double specularStrength = 0.4;
        var specular = lightColor.Mul(spec * specularStrength);
        
        return ambient.AddSelf(diffuse).AddSelf(specular).Clamp();
    }
}