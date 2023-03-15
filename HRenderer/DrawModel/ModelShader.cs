using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.DrawModel; 

public class ModelShader: Shader {
    public override Vector4 VertexShading(in Dictionary<string, Vector4> vector4s, in Dictionary<string, Vector2> vector2s) {
        var position = vector4s["position"];
        var r = Math.Sin(this.uniformFloats["time"]) * Math.PI;
        position = position.Transform(Matrix4.GetRotationY((float)r));
        var vpMat = this.projection.Mul(this.view);
        return position.TransformSelf(vpMat);
    }

    public override Vector4 FragShading() {
        var uv = this.varyVec2Dict["uv"];
        return this.Texture2D(this.uniformTextures["mainTexture"], uv);
    }
}