namespace HRenderer.Core; 

/**
 * 模版buffer
 */
public class StencilBuffer {
	
	public readonly int width;
	public readonly int height;
	public readonly byte[] buffer;

	public StencilBuffer(int width, int height) {
		this.width = width;
		this.height = height;

		this.buffer = new byte[width * height];
	}

	public void Clear() {
		
	}
	
}