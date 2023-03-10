using System;

namespace HRenderer.Common {
    public class Vector {
        public float[] data;
        public Vector(int length) {
            this.data = new float[length];
        }
    }
    
    public class Vector2: Vector {
        public float x {
            get => this.data[0];
            set => this.data[0] = value;
        }
        public float y {
            get => this.data[1];
            set => this.data[1] = value;
        }

        public static Vector2 Create(float x = 0, float y = 0) {
            return new Vector2(x, y);
        } 

        private Vector2(float x, float y): base(2) {
            this.x = x;
            this.y = y;
        }

        public Vector2 AddSelf(Vector2 a) {
            this.x += a.x;
            this.y += a.y;
            return this;
        }

        public Vector2 Add(Vector2 a) {
            return this.Clone().AddSelf(a);
        }

        public Vector2 SubSelf(Vector2 a) {
            this.x -= a.x;
            this.y -= a.y;
            return this;
        }

        public Vector2 Sub(Vector2 a) {
            return this.Clone().SubSelf(a);
        }

        public Vector2 MulSelf(Vector2 a) {
            this.x *= a.x;
            this.y *= a.y;
            return this;
        }

        public Vector2 Mul(Vector2 a) {
            return this.Clone().MulSelf(a);
        }

        public float Dot(Vector2 a) {
            return this.x * a.x + this.y * a.y;
        }
        
        public float Cross(Vector2 a) {
            return this.x * a.y - this.y * a.x;
        }
        
        public Vector2 Clone() {
            return new Vector2(this.x, this.y);
        }
    }

    public class Vector4: Vector {
        public float x {
            get => this.data[0];
            set => this.data[0] = value;
        }
        public float y {
            get => this.data[1];
            set => this.data[1] = value;
        }
        public float z {
            get => this.data[2];
            set => this.data[2] = value;
        }
        public float w {
            get => this.data[3];
            set => this.data[3] = value;
        }

        public Vector4(float x, float y, float z, float w): base(4) {
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

        public Vector4 CrossSelf(Vector4 v) {
            var a = this.y * v.z - this.z * v.y;
            var b = this.z * v.x - this.x * v.z;
            var c = this.x * v.y - this.y * v.x;
            this.x = a;
            this.y = b;
            this.z = c;
            return this;
        }

        public Vector4 Cross(Vector4 v) {
            return this.Clone().CrossSelf(v);
        }
    }
}