namespace HRenderer.Common; 

public class Triangle {
	public Vector4 position1 = Vector4.Create();
	public Vector4 position2 = Vector4.Create();
	public Vector4 position3 = Vector4.Create();

	public void Clear() {
		Vector4.Return(this.position1);
		Vector4.Return(this.position2);
		Vector4.Return(this.position3);
	}
}