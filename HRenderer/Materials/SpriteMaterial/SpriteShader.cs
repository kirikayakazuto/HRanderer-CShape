using System;
using System.Collections.Generic;
using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.Materials.SpriteMaterial {
    public class SpriteShader: Shader {
        
        public override Vector4 VertexShading() {
            var position = this.attribsVec4Dict["position"];
            var r = Math.PI / 3f;
            position = position.Transform(Matrix4.GetRotationZ(r));
            // position.y += 1f;
            var vpMat = this.projection.Mul(this.view);
            
            return position.TransformSelf(vpMat);
        }

        public override Vector4 FragShading() {
            var uv = this.attribsVec2Dict["uv"];
            // var noiseColor = this.Texture2D(this.uniformTextures["noiseTexture"], uv);
            var color = this.Texture2D(this.uniformTextures["mainTexture"], uv);
            // if (noiseColor.x < (Math.Sin(this.uniformFloats["time"]) + 1) / 2) {
            //     color.Set(0, 0, 0, 0);
            // }
            return color;
        }
    }
}