namespace HRenderer.Common; 

public class Triangle {

	public uint v1;
	public uint v2;
	public uint v3;
	
	public Vector4 position1 = Vector4.Create();
	public Vector4 position2 = Vector4.Create();
	public Vector4 position3 = Vector4.Create();

	public double z1;
	public double z2;
	public double z3;

	public void Clear() {
		Vector4.Return(this.position1);
		Vector4.Return(this.position2);
		Vector4.Return(this.position3);
	}
}