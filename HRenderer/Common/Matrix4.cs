namespace HRenderer.Common {
    public class Matrix4 {
        private float[] _array = new float[16];

        public static Matrix4 GetIdentify() {
            var m = new Matrix4();
            return m;
        }
    }
}