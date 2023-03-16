using System;
using System.Collections.Generic;
using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.DrawTriangle {
    public class SpriteShader: Shader {
        public override Vector4 VertexShading() {
            var position = this.attribsVec4Dict["position"];
            var r = Math.PI / 2;
            position = position.Transform(Matrix4.GetRotationX((float)r));
            position.y += 1f;
            var vpMat = this.projection.Mul(this.view);
            
            return position.TransformSelf(vpMat);
        }

        public override Vector4 FragShading() {
            var uv = this.attribsVec2Dict["uv"];
            return this.Texture2D(this.uniformTextures["mainTexture"], uv);
        }
    }
}