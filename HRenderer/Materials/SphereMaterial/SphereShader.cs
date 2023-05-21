using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SphereMaterial; 

public class SphereShader: Shader {
        
    public override Vector4 VertexShading(GlData glData) {
        var position = glData.attributes.Vec4s["position"];;
        var r = Math.PI;
        position = position.Transform(Matrix4.GetRotationX(r).MulSelf(Matrix4.GetRotationY(r)));
        var vpMat = this.projection.Mul(this.view);
        
        return position.TransformSelf(vpMat);
    }

    public override Vector4 FragShading() {
        return Vector4.Create(1, 0, 0, 1);
    }
}