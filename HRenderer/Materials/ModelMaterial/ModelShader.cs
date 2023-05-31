using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.ModelMaterial; 

public class ModelShader: Shader {
    public override Vector4 VertexShading(GlData glData) {
        var position = glData.attributes.Vec4s["position"];
        var r = Math.Sin(this.uniformData.Doubles["time"]) * Math.PI;
        // var r = Math.PI;
        // position = position.Transform(Matrix4.GetRotationX(Math.PI));
        position.TransformSelf(Matrix4.GetRotationY(r));
        position.TransformSelf(Matrix4.GetScale(2, 2, 2));
        
        glData.varyingDict.Vec4s["v_position"] = position.Clone();
        glData.varyingDict.Vec2s["v_uv"] = glData.attributes.Vec2s["uv"];
        
        var vpMat = this.projection.Mul(this.view);
        return position.TransformSelf(vpMat);
    }

    public override Vector4 FragShading() {
        var lightColor = this.uniformData.Vec4s["Light.Color"];
        var uv = this.varyingDict.Vec2s["v_uv"];
        var lightDir = this.uniformData.Vec4s["Light.Position"].Sub(this.varyingDict.Vec4s["v_position"]).NormalizeSelf();
        
        var normalTexture = this.uniformData.Textures["normalTexture"];
        var norm = this.Texture2D(normalTexture, uv).Normalize();
        norm.x *= -1;
        norm.y *= -1;
        
        // 环境光
        const double ambientStrength = 0.2;
        // var ambient = this.uniformVec4["Light.Color"].Mul(ambientStrength);
        var ambient = lightColor.Mul(this.Texture2D(this.uniformData.Textures["mainTexture"], uv)).Mul(ambientStrength);
        
        var diff = Math.Max(norm.Dot(lightDir), 0);
        var diffuse = this.Texture2D(this.uniformData.Textures["mainTexture"], uv).Mul(lightColor).Mul(diff);

        return ambient.AddSelf(diffuse);
        // var z = this.varyingDict.Vec4s["v_position"].z;
        // return Vector4.Create(z, z, z, 1);
    }
}