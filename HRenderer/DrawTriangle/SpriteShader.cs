using System;
using System.Collections.Generic;
using HRenderer.Common;
using HRenderer.Core;

namespace HRenderer.DrawTriangle {
    public class SpriteShader: Shader {
        public override Vector4 VertexShading(in Dictionary<string, Vector4> vector4s, in Dictionary<string, Vector2> vector2s) {
            var position = vector4s["position"];
            var r = Math.Sin(this.uniformFloats["time"]) * Math.PI;
            position = position.Transform(Matrix4.GetRotationX((float)r));
            var mvp = this.view.Mul(this.projection);
            return position.Transform(mvp);
        }

        public override Vector4 FragShading() {
            var uv = this.varyVec2Dict["uv"];
            return this.Texture2D(this.texture, uv);
        }
    }
}