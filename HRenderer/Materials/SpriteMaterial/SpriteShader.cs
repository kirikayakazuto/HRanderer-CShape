using System;
using System.Collections.Generic;
using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SpriteMaterial {
    public class SpriteShader: Shader {
        
        public override Vector4 VertexShading(GlData glData) {
            var position = glData.attributes.Vec4s["position"].Clone();
            
            // var r = Math.PI * this.uniformData.Doubles["time"] * 0.5;
            // position = position.Transform(Matrix4.GetRotationX(r).MulSelf(Matrix4.GetRotationY(r)));
            // position.TransformSelf(Matrix4.GetScale(2.5, 1, 1));
            // position.TransformSelf(Matrix4.GetRotationY(r));
            position.TransformSelf(Matrix4.GetScale(2, 2, 2));
            position.TransformSelf(Matrix4.GetTranslation(7, 0, 0));

            var vpMat = this.projection.Mul(this.view);            
            glData.varyingDict.Vec2s["uv"] = glData.attributes.Vec2s["uv"];
            
            return position.TransformSelf(vpMat);
        }

        public override Vector4 FragShading() {
            var uv = this.varyingDict.Vec2s["uv"];
            var noiseColor = this.Texture2D(this.uniformData.Textures["noiseTexture"], uv);
            var color = this.Texture2D(this.uniformData.Textures["mainTexture"], uv);
            // if (noiseColor.x < (Math.Sin(this.uniformData.Doubles["time"]) + 1) / 2) {
            //     color.Set(0, 0, 0, 0);
            // }
            return color;

            // var vpos = this.varyingDict.Vec4s["gl_FragCoord"];
            // var z = vpos.z;
            // z = (z + 1) * 0.5;
            // return Vector4.Create(z, z, z, 1);
        }
    }
}