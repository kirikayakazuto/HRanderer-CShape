using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.ModelMaterial; 

public class ModelShader: Shader {
    public override Vector4 VertexShading(VectorDict attribsDict, VectorDict varyingDict) {
        var position = attribsDict.Vec4Dict["position"];
        var r = Math.Sin(this.uniformDoubles["time"]) * Math.PI;
        // var r = Math.PI;
        position = position.Transform(Matrix4.GetRotationX(Math.PI));
        position.TransformSelf(Matrix4.GetRotationY(Math.PI));
        position.TransformSelf(Matrix4.GetScale(2, 2, 2));
        
        varyingDict.Vec4Dict["v_position"] = position.Clone();
        varyingDict.Vec2Dict["v_uv"] = attribsDict.Vec2Dict["uv"];
        
        var vpMat = this.projection.Mul(this.view);
        return position.TransformSelf(vpMat);
    }

    public override Vector4 FragShading() {
        var lightColor = this.uniformVec4["Light.Color"];
        var uv = this.varyingVec2Dict["v_uv"];
        var lightDir = this.uniformVec4["Light.Position"].Sub(this.varyingVec4Dict["v_position"]).NormalizeSelf();
        
        var normalTexture = this.uniformTextures["normalTexture"];
        var norm = this.Texture2D(normalTexture, uv).Normalize();
        norm.x *= -1;
        norm.y *= -1;
        
        // 环境光
        const double ambientStrength = 0.2;
        // var ambient = this.uniformVec4["Light.Color"].Mul(ambientStrength);
        var ambient = lightColor.Mul(this.Texture2D(this.uniformTextures["mainTexture"], uv)).Mul(ambientStrength);
        
        var diff = Math.Max(norm.Dot(lightDir), 0);
        var diffuse = this.Texture2D(this.uniformTextures["mainTexture"], uv).Mul(lightColor).Mul(diff);

        return ambient.AddSelf(diffuse);
    }
}