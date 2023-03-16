using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.DrawModel; 

public class ModelShader: Shader {
    public override Vector4 VertexShading() {
        var position = this.attribsVec4Dict["position"];
        var r = Math.Sin(this.uniformFloats["time"]) * Math.PI;
        position = position.Transform(Matrix4.GetRotationX((float)Math.PI));
        position.TransformSelf(Matrix4.GetRotationY((float)r));
        var vpMat = this.projection.Mul(this.view);
        return position.TransformSelf(vpMat);
    }

    public override Vector4 FragShading() {
        var uv = this.attribsVec2Dict["uv"];
        return this.Texture2D(this.uniformTextures["mainTexture"], uv);
        // return Vector4.Create(1, 0, 0, 1);
    }
}