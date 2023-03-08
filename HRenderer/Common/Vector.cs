using System;

namespace HRenderer.Common {
    public class Vector {
    }

    public class Vector2 {
        public float x;
        public float y;

        public static Vector2 Create(float x = 0, float y = 0) {
            return new Vector2(x, y);
        } 

        private Vector2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        Vector2 AddSelf(Vector2 a) {
            this.x += a.x;
            this.y += a.y;
            return this;
        }

        Vector2 Add(Vector2 a) {
            return this.Clone().AddSelf(a);
        }

        Vector2 SubSelf(Vector2 a) {
            this.x -= a.x;
            this.y -= a.y;
            return this;
        }

        Vector2 Sub(Vector2 a) {
            return this.Clone().SubSelf(a);
        }

        Vector2 MulSelf(Vector2 a) {
            this.x *= a.x;
            this.y *= a.y;
            return this;
        }

        Vector2 Mul(Vector2 a) {
            return this.Clone().MulSelf(a);
        }

        float Dot(Vector2 a) {
            return this.x * a.x + this.y * a.y;
        }
        
        float Corss(Vector2 a) {
            return this.x * a.y - this.y * a.x;
        }
        
        public Vector2 Clone() {
            return new Vector2(this.x, this.y);
        }
    }

    public class Vector4 {
        public float x;
        public float y;
        public float z;
        public float w;

        private Vector4(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector4 Create(float x = 0, float y = 0, float z = 0, float w = 0) {
            return new Vector4(x, y, z, w);
        }

        public Vector4 AddSelf(Vector4 v) {
            this.x += v.x;
            this.y += v.y;
            this.z += v.z;
            this.w = 1;
            return this;
        }

        public Vector4 Add(Vector4 v) {
            return this.Clone().AddSelf(v);
        }

        public Vector4 SubSelf(Vector4 v) {
            this.x *= v.x;
            this.y *= v.y;
            this.z *= v.z;
            return this;
        }

        public Vector4 Sub(Vector4 v) {
            return this.Clone().SubSelf(v);
        }

        public Vector4 MulSelf(Vector4 v) {
            this.x *= v.x;
            this.y *= v.y;
            this.z *= v.z;
            return this;
        }

        public Vector4 Mul(Vector4 v) {
            return this.Clone().MulSelf(v);
        }

        public Vector4 Homogenenize() {
            if (this.w == 0) return this;
            this.x /= this.w;
            this.y /= this.w;
            this.z /= this.w;
            return this;
        }

        public Vector4 NormalizeSelf() {
            var lenght =this.GetLength();
            if (lenght <= 0) return this;
            this.x /= lenght;
            this.y /= lenght;
            this.z /= lenght;
            this.w = 0;
            return this;
        }

        public Vector4 Normalize() {
            return this.Clone().NormalizeSelf();
        }

        public float GetLength() {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector4 Clone() {
            return new Vector4(this.x, this.y, this.z, this.w);
        }
    }
}