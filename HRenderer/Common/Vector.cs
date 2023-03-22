using System;

namespace HRenderer.Common {
    public class Vector {
        public double[] data;
        public Vector(int length) {
            this.data = new double[length];
        }
    }
    
    public class Vector2: Vector {
        private static Stack<Vector2> vector2s = new Stack<Vector2>();
        public double x {
            get => this.data[0];
            set => this.data[0] = value;
        }
        public double y {
            get => this.data[1];
            set => this.data[1] = value;
        }

        public static Vector2 Create(double x = 0, double y = 0) {
            var v = Vector2.vector2s.Count > 0 ? Vector2.vector2s.Pop() : new Vector2(x, y);
            v.Set(x, y);
            return v;
        }

        public static void Return(Vector2 v) {
            Vector2.vector2s.Push(v);
        }

        private Vector2(double x, double y): base(2) {
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
        
        public Vector2 Add(Vector2 a, in Vector2 outVector2) {
            outVector2.x = this.x;
            outVector2.y = this.y;
            return outVector2.AddSelf(a);
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

        public double Dot(Vector2 a) {
            return this.x * a.x + this.y * a.y;
        }
        
        public double Cross(Vector2 a) {
            return this.x * a.y - this.y * a.x;
        }
        
        public Vector2 Clone() {
            return new Vector2(this.x, this.y);
        }
        
        public double Dot(Vector4 v) {
            return this.x * v.x + this.y * v.y;
        }
        
        public void Set(double x, double y) {
            this.x = x;
            this.y = y;
        }
        
        public override string ToString() {
            return "x: " + this.x + " y: " + this.y;
        }
    }

    public class Vector4: Vector {
        public static int newCount = 0;
        private static Stack<Vector4> vector4s = new Stack<Vector4>();
        public double x {
            get => this.data[0];
            set => this.data[0] = value;
        }
        public double y {
            get => this.data[1];
            set => this.data[1] = value;
        }
        public double z {
            get => this.data[2];
            set => this.data[2] = value;
        }
        public double w {
            get => this.data[3];
            set => this.data[3] = value;
        }

        public void FromVec4(Vector4 v) {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            // this.w = v.w;
        }

        public void Set(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public void Set(double x, double y, double z, double w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(double x, double y, double z, double w): base(4) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector4 Create(double x = 0, double y = 0, double z = 0, double w = 0) {
            if (Vector4.vector4s.Count <= 0) {
                Vector4.newCount++;
            }
            var v = Vector4.vector4s.Count > 0 ? Vector4.vector4s.Pop() : new Vector4(x, y, z, w);
            v.Set(x, y, z, w);
            return v;
        }

        public static void Return(Vector4 v) {
            Vector4.vector4s.Push(v);
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

        public Vector4 Add(Vector2 v) {
            return this.Clone().AddSelf(v);
        }

        public Vector4 AddSelf(Vector2 v) {
            this.x += v.x;
            this.y += v.y;
            return this;
        }

        public Vector4 SubSelf(Vector4 v) {
            this.x -= v.x;
            this.y -= v.y;
            this.z -= v.z;
            return this;
        }

        public Vector4 Sub(Vector4 v) {
            return this.Clone().SubSelf(v);
        }

        public Vector4 MulSelf(double v) {
            this.x *= v;
            this.y *= v;
            this.z *= v;
            this.w *= v;
            return this;
        }

        public Vector4 Mul(double v) {
            return this.Clone().MulSelf(v);
        }

        public Vector4 Mul(Vector4 v) {
            return this.Clone().MulSelf(v);
        }

        public Vector4 MulSelf(Vector4 v) {
            this.x *= v.x;
            this.y *= v.y;
            this.z *= v.z;
            this.w = 1;
            return this;
        }

        public Vector4 Homogenenize() {
            if (this.w == 0) return this;
            this.x /= this.w;
            this.y /= this.w;
            this.z /= this.w;
            this.w = 1;
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

        public double GetLength() {
            return (double)Math.Sqrt(x * x + y * y + z * z);
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

        public double Dot(Vector4 v) {
            return this.x * v.x + this.y * v.y + this.z * v.z;
        }

        public Vector4 TransformSelf(Matrix4 matrix4) {
            var _x = this.x;
            var _y = this.y;
            var _z = this.z;
            var _w = this.w;
            var m = matrix4.data;
            this.x = m[0] * _x + m[4] * _y + m[8]  * _z + m[12] * _w;
            this.y = m[1] * _x + m[5] * _y + m[9]  * _z + m[13] * _w;
            this.z = m[2] * _x + m[6] * _y + m[10] * _z + m[14] * _w;
            this.w = m[3] * _x + m[7] * _y + m[11] * _z + m[15] * _w;
            return this;
        }

        public Vector4 Transform(Matrix4 matrix4) {
            return this.Clone().TransformSelf(matrix4);
        }

        public override string ToString() {
            return "x: " + this.x + " y: " + this.y + " z: " + this.z + " w: " + this.w;
        }

        public Vector4 Clamp() {
            this.x = Math.Min(this.x, 1);
            this.y = Math.Min(this.y, 1);
            this.z = Math.Min(this.z, 1);
            this.w = Math.Min(this.w, 1);
            return this;
        }
    }
}