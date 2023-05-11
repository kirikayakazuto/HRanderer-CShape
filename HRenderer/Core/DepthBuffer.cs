namespace HRenderer.Core; 

public class DepthBuffer {
	private readonly int width;
	private readonly int height;
	
	private readonly double[] _buffer;
	
	// msaa
	private readonly bool _useMsaa;
	private readonly double[,] _bufferMsaa;
	// 计算msaa覆盖率
	private readonly byte[] _pixelsMsaaCoverage;
	
	public DepthBuffer(int width, int height, bool msaa) {
		this.width = width;
		this.height = height;

		this._buffer = new double[width * height];
		for (var i = 0; i < this._buffer.Length; i++) {
			this._buffer[i] = 1;
		}
		
		this._useMsaa = msaa;
		if (!msaa) return;
		this._bufferMsaa = new double[4, width * height];
		for (var i = 0; i < this._bufferMsaa.GetLength(0); i++) {
			for (var j = 0; j < this._bufferMsaa.GetLength(1); j++) {
				this._bufferMsaa[i, j] = 1;
			}    
		}
		this._pixelsMsaaCoverage = new byte[width * height];
	}
	
	public byte[] GetCoverages() {
		return this._pixelsMsaaCoverage;
	}
	
	public double GetZ(int x, int y) {
		var idx = x + y * this.width;
		return this._buffer[idx];
	}
        
	public double GetZ(int x, int y, int level) {
		var idx = x + y * this.width;
		return this._bufferMsaa[level, idx];
	}
        
	public void SetZ(int x, int y, double z) {
		var idx = x + y * this.width;
		this._buffer[idx] = z;
	}
        
	public void SetZ(int x, int y, double z, int level) {
		var idx = x + y * this.width;
		this._bufferMsaa[level, idx] = z;
	}

	public bool ZTest(int x, int y, double z) {
		return z < this.GetZ(x, y);
	}

	public bool ZTest(int x, int y, double z, int level) {
		return z < this.GetZ(x, y, level);
	}

	public void Clear() {
		for (var i = 0; i < this._buffer.Length; i++) {
			this._buffer[i] = 1;
		}
            
		if (!this._useMsaa) return;
		for (var i = 0; i < this._bufferMsaa.GetLength(0); i++) {
			for (var j = 0; j < this._bufferMsaa.GetLength(1); j++) {
				this._bufferMsaa[i, j] = 1;
			}    
		}
		for (var i = 0; i < this._pixelsMsaaCoverage.Length; i++) {
			this._pixelsMsaaCoverage[i] = 0;
		}
	}
	
	public void AddMsaaCount(int x, int y) {
		var idx = x + y * this.width;
		this._pixelsMsaaCoverage[idx] ++;
	}
	
	public byte GetMsaaCount(int x, int y) {
		var idx = x + y * this.width;
		return this._pixelsMsaaCoverage[idx];
	}

	public bool CheckZ(int x, int y, double z) {
		if (!this.ZTest(x, y, z)) return false;
		this.SetZ(x, y, z);
		return true;
	}

	public bool CheckZ(int x, int y, double z, int level) {
		if (!this.ZTest(x, y, z, level)) return false;
		this.SetZ(x, y, z, level);
		return true;
	}
	
}