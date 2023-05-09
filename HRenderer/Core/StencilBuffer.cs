namespace HRenderer.Core; 

public class StencilBuffer {
	public int width { get; }
	public int height { get; }

	private readonly byte[] _buffer;

	public StencilBuffer(int width, int height) {
		this.width = width;
		this.height = height;

		this._buffer = new byte[width * height];
	}
	
	
}