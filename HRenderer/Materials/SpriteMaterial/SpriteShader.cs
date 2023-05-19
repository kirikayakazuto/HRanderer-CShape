using System;
using System.Collections.Generic;
using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SpriteMaterial {
    public class SpriteShader: Shader {
        
        public override Vector4 VertexShading(GlData glData) {
            var position = glData.attributes.Vec4s["position"];;
            var r = Math.PI;
            position = position.Transform(Matrix4.GetRotationX(r).MulSelf(Matrix4.GetRotationY(r)));
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
        }
    }
}