using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 渲染管线
 */
public class RenderPipeline {
	// 窗口相关数据
	private readonly int _width;
	private readonly int _height;
	private readonly Matrix4 _viewPortMat4;
	
	// gl缓存数据
	private readonly List<GlData> _glDatas = new List<GlData>();
	private readonly List<Vector4> _positions = new List<Vector4>();
	private readonly List<double> _depths = new List<double>();

	// 当前处理的三角形
	private readonly Triangle _triangle = new Triangle();
	
	// 输出
	public readonly FrameBuffer frameBuffer;
	public readonly DepthBuffer depthBuffer;
	public readonly StencilBuffer stencilBuffer;

	// 抗锯齿
	private readonly bool _useMsaa = false;

	// 深度测试
	private bool _useZTest = true;
	// 模版测试
	private bool _useStencil = true;
	// 模版写入
	private bool _writeStencil = false;

	// 渲染模式
	public RenderMode renderMode = RenderMode.Triangle;

	public RenderPipeline(int width, int height, bool useMsaa = false) {
		this._width = width;
		this._height = height;
		this._viewPortMat4 = Utils.GetViewPortMatrix(width, height);
		
		this._useMsaa = useMsaa;
		
		this.frameBuffer = new FrameBuffer(width, height);
		this.depthBuffer = new DepthBuffer(width, height, useMsaa);
		this.stencilBuffer = new StencilBuffer(width, height);
	}

	public void Draw(Material material) {
		var mesh = material.mesh;
		var shader = material.shader;
		
		var indices = mesh.Ibo;
		
		this._glDatas.Clear();
		this._depths.Clear();
		this._positions.Clear();
		
		// 顶点着色器工作
		for (uint i = 0; i < mesh.Vbo.Length; i+=mesh.stride) {
			var glData = new GlData();
			
			// 处理attribute
			mesh.GetVertexAttribs(i, glData.attributes);
			
			// 顶点着色器
			var position = shader.VertexShading(glData);
			// 转换到屏幕坐标
			position.TransformSelf(this._viewPortMat4);
			
			var w = position.w;
			glData.varyingDict.Vec4s["gl_FragCoord"] = position.Homogenenize();
			
			this._glDatas.Add(glData);
			this._positions.Add(position);
			this._depths.Add(w);
		}
		
		// 片元着色器工作
		for (var i = 0; i < indices.Length; i += 3) {
			
			var v1 = this._triangle.v1 = indices[i];
			var v2 = this._triangle.v2 = indices[i+1];
			var v3 = this._triangle.v3 = indices[i+2];

			this._triangle.position1 = this._positions[(int)v1];
			this._triangle.position2 = this._positions[(int)v2];
			this._triangle.position3 = this._positions[(int)v3];

			this._triangle.z1 = this._depths[(int)v1];
			this._triangle.z2 = this._depths[(int)v2];
			this._triangle.z3 = this._depths[(int)v3];
			
			// 三角形在屏幕外
			if(!this.CheckTriangleInScreen()) continue;
			
			if(material.useFaceCulling && this.IsBackFace()) continue;

			switch (this.renderMode) {
				case RenderMode.Triangle:
					this._rasterByTriangle(material); 
					break;
				case RenderMode.Line:
					this._rasterByLine(material);
					break;
				case RenderMode.Point:
					this._rasterByPoint(material);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	
		if(!this._useMsaa) return;
		this.frameBuffer.DoMsaa(this.depthBuffer.GetCoverages());
	}
	
	public void ClearBuffer() {
		this.frameBuffer.Clear();
		this.depthBuffer.Clear();
		this.stencilBuffer.Clear();
	}

	/**
	 * 三角形光栅化
	 */
	private void _rasterByTriangle(Material material) {
		var shader = material.shader;
		
		var v1 = this._triangle.v1;
		var v2 = this._triangle.v2;
		var v3 = this._triangle.v3;
		
		var position1 = this._triangle.position1;
		var position2 = this._triangle.position2;
		var position3 = this._triangle.position3;

		var z1 = this._triangle.z1;
		var z2 = this._triangle.z2;
		var z3 = this._triangle.z3;
				
		// 光栅化
		var bound = Utils.GetBoundingBox(position1, position2, position3, this._width, this._height);
		var barycentric = Vector4.Create();
		var p = Vector2.Create();
		for (var y = bound.minY; y < bound.maxY; y++) {
			p.y = y + 0.5f;
			for (var x = bound.minX; x < bound.maxX; x++) {
				p.x = x + 0.5f;
				
				// msaa
				if(this._useMsaa) this.CheckMsaa(x, y, p, barycentric);
				if (!this.CheckInTriangle(p, barycentric)) continue;
				
				// 模版测试
				if(this._useStencil && !this.CheckStencli(material.useStencilWrite, x, y)) continue;;

				// 深度测试
				var z = this.GetInterpolationZ(barycentric);
				if(this._useZTest && material.useDepthWrite && !this.depthBuffer.CheckZ(x, y, z)) continue;

				// 校正透视差值
				barycentric = Utils.AdjustBarycentric(barycentric, z1, z2, z3);
				
				// 计算attribs差值
				this.ComputeShaderVectorVarying(shader, barycentric, v1, v2, v3);

				// frag shading
				var color = shader.FragShading();
				
				// 输出颜色
				this.frameBuffer.SetColor(x , y, color);
				Vector4.Return(color);
			}
		}
	}

	private void _rasterByLine(Material material) {
		var shader = material.shader;

		var v1 = this._triangle.v1;
		var v2 = this._triangle.v2;
		var v3 = this._triangle.v3;
		
		var position1 = this._triangle.position1;
		var position2 = this._triangle.position2;
		var position3 = this._triangle.position3;

		var z1 = this._triangle.z1;
		var z2 = this._triangle.z2;
		var z3 = this._triangle.z3;
		
		this._drawLine(position1, position2);
		this._drawLine(position1, position3);
		this._drawLine(position2, position3);
	}

	/**
	 * Bresenham算法
	 */
	private void _drawLine(Vector4 p1, Vector4 p2) {
		var x1 = (int)Math.Round(p1.x);
		var y1 = (int)Math.Round(p1.y);
		
		var x2 = (int)Math.Round(p2.x);
		var y2 = (int)Math.Round(p2.y);
		
		var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
		if (steep) {
			(x1, y1) = (y1, x1);
			(x2, y2) = (y2, x2);
		}
		if (x1 > x2) {
			(x1, x2) = (x2, x1);
			(y1, y2) = (y2, y1);
		}
		var deltaX = x2 - x1;
		var offset = deltaX / 2;
		var deltaY = Math.Abs(y2 - y1);
		var yStep = y1 < y2 ? 1 : -1;

		var y = y1;
		for (var x = x1; x < x2; x++) {
			this.frameBuffer.SetColor(steep ? y : x , steep ? x : y, Vector4.Create(1, 1, 1, 1));
			offset -= deltaY;
			if (offset >= 0) continue;
			y += yStep;
			offset += deltaX;
		}
	}

	private void _rasterByPoint(Material material) {
		var shader = material.shader;

		var v1 = this._triangle.v1;
		var v2 = this._triangle.v2;
		var v3 = this._triangle.v3;
		
		var position1 = this._triangle.position1;
		var position2 = this._triangle.position2;
		var position3 = this._triangle.position3;
		
		this._drawPoint(position1);
		this._drawPoint(position2);
		this._drawPoint(position3);
	}

	private void _drawPoint(Vector4 p) {
		this.frameBuffer.SetColor((int)Math.Round(p.x), (int)Math.Round(p.y), Vector4.Create(1, 1, 1, 1));
	}
	
	private bool CheckInTriangle(in Vector2 p, in Vector4 barycentric) {
		var position1 = this._triangle.position1;
		var position2 = this._triangle.position2;
		var position3 = this._triangle.position3;
		Utils.GetBarycentric(p, position1, position2, position3, barycentric);
		return barycentric.x >= 0 && barycentric.y >= 0 && barycentric.z >= 0;
	}
	
	private double GetInterpolationZ(in Vector4 barycentric) {
		var z1 = this._triangle.position1.z;
		var z2 = this._triangle.position2.z;
		var z3 = this._triangle.position3.z;
		var z = Utils.GetInterpValue3(z1, z2, z3, barycentric.x, barycentric.y, barycentric.z);
		return (z + 1) * 0.5;
	}
	
	/**
	 * 计算差值
	 */
	private void ComputeShaderVectorVarying(Shader shader, in Vector4 barycentric, uint v1, uint v2, uint v3) {
		// gl data
		var glData1 = this._glDatas[(int) v1];
		var glData2 = this._glDatas[(int) v2];
		var glData3 = this._glDatas[(int) v3];
		var vec4keys = glData1.varyingDict.Vec4s.Keys;
		foreach (var key in vec4keys) {
			var vec4 = shader.varyingDict.Vec4s.ContainsKey(key) ? shader.varyingDict.Vec4s[key] : Vector4.Create(); 
			Utils.GetInterpVec4(glData1.varyingDict.Vec4s[key], glData2.varyingDict.Vec4s[key], glData3.varyingDict.Vec4s[key], barycentric, vec4);
			shader.varyingDict.Vec4s[key] = vec4;
		}
		var vec2keys = glData1.varyingDict.Vec2s.Keys;
		foreach (var key in vec2keys) {
			var vec2 = shader.varyingDict.Vec2s.ContainsKey(key) ? shader.varyingDict.Vec2s[key] : Vector2.Create(); 
			Utils.GetInterpVec2(glData1.varyingDict.Vec2s[key], glData2.varyingDict.Vec2s[key], glData3.varyingDict.Vec2s[key], barycentric, vec2);
			shader.varyingDict.Vec2s[key] = vec2;
		}
	}
	
	private bool CheckMsaa(int x, int y, in Vector2 p, in Vector4 barycentric) {
		var pMsaa = Vector2.Create();
		var msaa = 0;
		for (var i = 0; i < 4; i++) {
			p.Add(Const.MsaaOffsetVec2[i], pMsaa);
			// 是否在三角形内
			if(!this.CheckInTriangle(pMsaa, barycentric)) continue;
			// 深度通过
			var z = this.GetInterpolationZ(barycentric);
			if(!this.depthBuffer.CheckZ(x, y, z, i)) continue;;
			// 写入
			this.depthBuffer.AddMsaaCount(x, y);
			msaa++;
		}
		Vector2.Return(pMsaa);
		return msaa != 0;
	}

	private bool IsBackFace() {
		var p1 = this._triangle.position1;
		var p2 = this._triangle.position2;
		var p3 = this._triangle.position3;
		var a = p2.x - p1.x;
		var b = p2.y - p1.y;
		var c = p3.x - p1.x;
		var d = p3.y - p1.y;
		
		return a * d - b * c < 0;
	}

	private bool CheckTriangleInScreen() {
		var p1 = this._triangle.position1;
		var p2 = this._triangle.position2;
		var p3 = this._triangle.position3;
		var p1Hide = p1.x < 0 || p1.x >= this._width && p1.y < 0 || p1.y >= this._height;
		var p2Hide = p2.x < 0 || p2.x >= this._width && p2.y < 0 || p2.y >= this._height;
		var p3Hide = p3.x < 0 || p3.x >= this._width && p3.y < 0 || p3.y >= this._height;
		return !(p1Hide && p2Hide && p3Hide);
	}

	/**
	 * 模版测试
	 */
	private bool CheckStencli(bool write, int x, int y) {
		if (!write) return !this.stencilBuffer.Check(x, y);
		this.stencilBuffer.Set(x, y); 
		return false;
	}

	
	/**
	 * 光线追踪
	 */
	private void DoRayTracing() {
		for (var x = 0; x < this._width; x++) {
			for (var y = 0; y < this._height; y++) {
				var u = x / this._width;
				var v = y / this._height;
				
				
			}
		}
	}

}