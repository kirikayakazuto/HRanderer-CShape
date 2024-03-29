using System;
using System.Runtime.CompilerServices;

namespace HRenderer.Common {
    /**
     * 行矩阵
     * 0, 1, 2, 3,
     * 4, 5, 6, 7,
     * 8, 9, 10, 11,
     * 12, 13, 14, 15
     * 
     * 计算方式, 前行乘后列
     */
    public class Matrix4 {
        public double[] data;

        Matrix4() {
            this.data = new double[16];
        }

        public static Matrix4 GetIdentify() {
            var m = new Matrix4();
            m.data[0] = 1;
            m.data[5] = 1;
            m.data[10] = 1;
            m.data[15] = 1;
            return m;
        }

        public static Matrix4 GetRotationX(double angle) {
            var m = Matrix4.GetIdentify();
            var cos = (double)Math.Cos(angle);
            var sin = (double)Math.Sin(angle);
            m.data[5] = cos;
            m.data[6] = sin;
            m.data[9] = -sin;
            m.data[10] = cos;
            return m;
        }

        public static Matrix4 GetRotationY(double angle) {
            var m = Matrix4.GetIdentify();
            var cos = (double)Math.Cos(angle);
            var sin = (double)Math.Sin(angle);
            m.data[0] = cos;
            m.data[2] = -sin;
            m.data[8] = sin;
            m.data[10] = cos;
            return m;
        }

        public static Matrix4 GetRotationZ(double angle) {
            var m = Matrix4.GetIdentify();
            var cos = (double)Math.Cos(angle);
            var sin = (double)Math.Sin(angle);
            m.data[0] = cos;
            m.data[1] = sin;
            m.data[4] = -sin;
            m.data[5] = cos;
            return m;
        }

        public static Matrix4 GetTranslation(double x = 0, double y = 0, double z = 0) {
            var m = Matrix4.GetIdentify();
            m.data[12] = x;
            m.data[13] = y;
            m.data[14] = z;
            return m;
        }

        public static Matrix4 GetScale(double sacleX = 1, double scaleY = 1, double scaleZ = 1) {
            var m = Matrix4.GetIdentify();
            m.data[0] = sacleX;
            m.data[5] = scaleY;
            m.data[10] = scaleZ;
            return m;
        }

        public Matrix4 MulSelf(Matrix4 m) {
            var a = this.data;
            var b = m.data;
            var _a00 = a[0]; var _a01 = a[1]; var _a02 = a[2]; var _a03 = a[3];
            var _a10 = a[4]; var _a11 = a[5]; var _a12 = a[6]; var _a13 = a[7];
            var _a20 = a[8]; var _a21 = a[9]; var _a22 = a[10]; var _a23 = a[11];
            var _a30 = a[12]; var _a31 = a[13]; var _a32 = a[14]; var _a33 = a[15];
            
            var b0 = b[0];
            var b1 = b[1];
            var b2 = b[2];
            var b3 = b[3];
            a[0] = b0 * _a00 + b1 * _a10 + b2 * _a20 + b3 * _a30;
            a[1] = b0 * _a01 + b1 * _a11 + b2 * _a21 + b3 * _a31;
            a[2] = b0 * _a02 + b1 * _a12 + b2 * _a22 + b3 * _a32;
            a[3] = b0 * _a03 + b1 * _a13 + b2 * _a23 + b3 * _a33;

            b0 = b[4]; b1 = b[5]; b2 = b[6]; b3 = b[7];
            a[4] = b0 * _a00 + b1 * _a10 + b2 * _a20 + b3 * _a30;
            a[5] = b0 * _a01 + b1 * _a11 + b2 * _a21 + b3 * _a31;
            a[6] = b0 * _a02 + b1 * _a12 + b2 * _a22 + b3 * _a32;
            a[7] = b0 * _a03 + b1 * _a13 + b2 * _a23 + b3 * _a33;

            b0 = b[8]; b1 = b[9]; b2 = b[10]; b3 = b[11];
            a[8] = b0 * _a00 + b1 * _a10 + b2 * _a20 + b3 * _a30;
            a[9] = b0 * _a01 + b1 * _a11 + b2 * _a21 + b3 * _a31;
            a[10] = b0 * _a02 + b1 * _a12 + b2 * _a22 + b3 * _a32;
            a[11] = b0 * _a03 + b1 * _a13 + b2 * _a23 + b3 * _a33;

            b0 = b[12]; b1 = b[13]; b2 = b[14]; b3 = b[15];
            a[12] = b0 * _a00 + b1 * _a10 + b2 * _a20 + b3 * _a30;
            a[13] = b0 * _a01 + b1 * _a11 + b2 * _a21 + b3 * _a31;
            a[14] = b0 * _a02 + b1 * _a12 + b2 * _a22 + b3 * _a32;
            a[15] = b0 * _a03 + b1 * _a13 + b2 * _a23 + b3 * _a33;
            
            return this;
        }

        public Matrix4 Mul(Matrix4 m) {
            return this.Clone().MulSelf(m);
        }
        
        public Vector4 Mul(Vector4 v) {
            var a = this.data;
            var _a00 = a[0]; var _a01 = a[1]; var _a02 = a[2]; var _a03 = a[3];
            var _a10 = a[4]; var _a11 = a[5]; var _a12 = a[6]; var _a13 = a[7];
            var _a20 = a[8]; var _a21 = a[9]; var _a22 = a[10]; var _a23 = a[11];
            var _a30 = a[12]; var _a31 = a[13]; var _a32 = a[14]; var _a33 = a[15];
            
            var x = v.x * _a00 + v.y * _a01 + v.z * _a02 + v.w * _a03;
            var y = v.x * _a10 + v.y * _a11 + v.z * _a12 + v.w * _a13;
            var z = v.x * _a20 + v.y * _a21 + v.z * _a22 + v.w * _a23;
            var w = v.x * _a30 + v.y * _a31 + v.z * _a32 + v.w * _a33;

            return Vector4.Create(x, y, z, w);
        }

        public Matrix4 Clone() {
            var m = Matrix4.Create();
            Array.Copy(this.data, m.data, this.data.Length);
            return m;
        }

        public static Matrix4 Create() {
            return new Matrix4();
        }

        public Matrix4 Inverse() {
            // 伴生矩阵除法
            return Matrix4.GetIdentify();
        }
    }
}