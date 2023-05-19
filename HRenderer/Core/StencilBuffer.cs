namespace HRenderer.Core; 

/**
 * 模版buffer
 */
public class StencilBuffer {
	
	private readonly int width;
	private readonly int height;
	private readonly byte[] _buffer;

	private byte _mask = 0xff;
	public StencilBuffer(int width, int height) {
		this.width = width;
		this.height = height;

		this._buffer = new byte[width * height];
		this.Clear();
	}

	public void Set(int x, int y) {
		var idx = x + y * this.width;
		this._buffer[idx] = 1;
	}

	public byte Get(int x, int y) {
		var idx = x + y * this.width;
		return this._buffer[idx];
	}

	public bool Check(int x, int y) {
		return this.Get(x, y) == 1;
	}

	public void Clear() {
		for (var i = 0; i < this._buffer.Length; i++) {
			this._buffer[i] = 0;
		}
	}
	
}