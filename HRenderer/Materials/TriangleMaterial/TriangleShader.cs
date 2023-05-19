using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.TriangleMaterial; 

public class TriangleShader: Shader {
        
	public override Vector4 VertexShading(GlData glData) {
		var position = glData.attributes.Vec4s["position"];;
		
		var r = Math.PI * Math.Sin(this.uniformData.Doubles["time"]);
		position = position.Transform(Matrix4.GetRotationX(r).MulSelf(Matrix4.GetScale(0.5, 0.5, 0.5)));
		var vpMat = this.projection.Mul(this.view);
		
		
		glData.varyingDict.Vec4s["color"] = glData.attributes.Vec4s["color"];
            
		return position.TransformSelf(vpMat);
	}

	public override Vector4 FragShading() {
		
		var color = this.varyingDict.Vec4s["color"].Clone();
		
		return color;
	}
}