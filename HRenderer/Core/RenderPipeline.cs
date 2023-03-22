using HRenderer.Common;

namespace HRenderer.Core;

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


/**
 * 渲染管线
 */
public class RenderPipeline {
	// 屏幕大小
	private readonly int _width;
	private readonly int _height;
	private readonly Matrix4 _viewPortMat4;

	// 缓存数据
	private readonly CacheData _cacheData = new CacheData();

	// 抗锯齿
	private readonly bool _useMsaa = false;
	
	// 当前处理的三角形
	private readonly Triangle _triangle = new Triangle();
	
	private readonly Vector2[] _msaaOffsetVec2 = new Vector2[] {
		Vector2.Create(0.375, 0.125),
		Vector2.Create(0.125, -0.375),
		Vector2.Create(-0.375, -0.125),
		Vector2.Create(-0.125, -0.375),
		
	};
	public FrameBuffer frameBuffer { get; }
	
	public RenderPipeline(int width, int height, bool useMsaa = false) {
		this._width = width;
		this._height = height;
		this._viewPortMat4 = Utils.GetViewPortMatrix(width, height);
		this.frameBuffer = new FrameBuffer(width, height, useMsaa);
		this._useMsaa = useMsaa;
	}
	
	/**
	 * material 渲染所需数据
	 */
	public void Draw(Material material) {
		var mesh = material.mesh;
		var indices = mesh.Ibo;
		Vector4.newCount = 0;
		for (var i = 0; i < indices.Length; i += 3) {
			var v1 = mesh.stride * indices[i];
			var v2 = mesh.stride * indices[i+1];
			var v3 = mesh.stride * indices[i+2];
			this._drawTriangle(material, v1, v2, v3);
		}
		// Console.WriteLine(Vector4.newCount);
		if (!this._useMsaa) return;
		this.frameBuffer.DoMsaa();
	}

	public void ClearFrameBuffer() {
		this.frameBuffer.Clear();
	}

	private void _drawTriangle(Material material, uint v1, uint v2, uint v3) {
		var mesh = material.mesh;
		var shader = material.shader;
		
		var near = shader.uniformDoubles["CameraNear"];
		var far = shader.uniformDoubles["CameraFar"];
		
		// 顶点数据解析
		mesh.GetVertexAttribs(v1, this._cacheData.Vec4Attribs1, this._cacheData.Vec2Attribs1);
		mesh.GetVertexAttribs(v2, this._cacheData.Vec4Attribs2, this._cacheData.Vec2Attribs2);
		mesh.GetVertexAttribs(v3, this._cacheData.Vec4Attribs3, this._cacheData.Vec2Attribs3);
		
		// 第一个阶段, 顶点着色器
		shader.AddAttribs(this._cacheData.Vec4Attribs1);
		shader.AddAttribs(this._cacheData.Vec2Attribs1);
		var position1 = this._triangle.position1 = shader.VertexShading();
		shader.AddAttribs(this._cacheData.Vec4Attribs2);
		shader.AddAttribs(this._cacheData.Vec2Attribs2);
		var position2 = this._triangle.position2 = shader.VertexShading();
		shader.AddAttribs(this._cacheData.Vec4Attribs3);
		shader.AddAttribs(this._cacheData.Vec2Attribs3);
		var position3 = this._triangle.position3 = shader.VertexShading();
		
		// 转换到屏幕空间
		position1.TransformSelf(this._viewPortMat4);
		position2.TransformSelf(this._viewPortMat4);
		position3.TransformSelf(this._viewPortMat4);
		
		var z1 = position1.w;
		var z2 = position2.w;
		var z3 = position3.w;
		
		position1.Homogenenize();
		position2.Homogenenize();
		position3.Homogenenize();
		
		// 背面剔除
		if (material.useFaceCulling && this.IsBackFace()) return;

		// 光栅化
		var bound = Utils.GetBoundingBox(position1, position2, position3, this._width, this._height);
		var barycentric = Vector4.Create();
		var p = Vector2.Create();
		for (var y = bound.minY; y < bound.maxY; y++) {
			p.y = y + 0.5f;
			for (var x = bound.minX; x < bound.maxX; x++) {
				p.x = x + 0.5f;
				
				if(this._useMsaa) this.CheckMsaa(x, y, p, barycentric, near, far);
				
				if (!this.CheckInTriangle(p, barycentric)) continue;
				if(!this.CheckZ(x, y, barycentric, near, far)) continue;
				
				// 校正透视差值
				barycentric = Utils.AdjustBarycentric(barycentric, z1, z2, z3);
                    
				// 计算attribs差值
				this.ComputeShaderVectorVarying(mesh.attribInfo, shader, barycentric);

				// frag shading
				var color = shader.FragShading();
				
				// 输出颜色
				this.frameBuffer.SetColor(x , y, color);
				Vector4.Return(color);
			}
		}
		Vector2.Return(p);
		
		this._triangle.Clear();
	}
	
	private double GetInterpolationZ(in Vector4 barycentric, double near, double far) {
		var z1 = this._triangle.position1.z;
		var z2 = this._triangle.position2.z;
		var z3 = this._triangle.position3.z;
		var z = Utils.GetInterpValue3(z1, z2, z3, barycentric.x, barycentric.y, barycentric.z);
		return Utils.GetDepth(near, far, z);
	}
	
	private void ComputeShaderVectorVarying(IEnumerable<VertexFormat> attribInfo, Shader shader, in Vector4 barycentric) {
		var attribs1Vec4 = this._cacheData.Vec4Attribs1;
		var attribs2Vec4 = this._cacheData.Vec4Attribs2;
		var attribs3Vec4 = this._cacheData.Vec4Attribs3;

		var attribs1Vec2 = this._cacheData.Vec2Attribs1;
		var attribs2Vec2 = this._cacheData.Vec2Attribs2;
		var attribs3Vec2 = this._cacheData.Vec2Attribs3;
		
		foreach (var vertexFormat in attribInfo) {
			var name = vertexFormat.name;
			switch (vertexFormat.num) {
				case 4:
					var vec4 = shader.attribsVec4Dict.ContainsKey(name) ? shader.attribsVec4Dict[name] : Vector4.Create(); 
					Utils.GetInterpVec4(attribs1Vec4[name], attribs2Vec4[name], attribs3Vec4[name], barycentric, vec4);
					shader.attribsVec4Dict[name] = vec4;
					break;
				case 2:
					var vec2 = shader.attribsVec2Dict.ContainsKey(name) ? shader.attribsVec2Dict[name] : Vector2.Create();
					Utils.GetInterpVec2(attribs1Vec2[name], attribs2Vec2[name], attribs3Vec2[name], barycentric, vec2);
					shader.attribsVec2Dict[name] = vec2;
					break;
				default:
					break;
			}
		}
	}

	/**
	 * 背面剔除
	 */
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

	private bool CheckMsaa(int x, int y, in Vector2 p, in Vector4 barycentric, double near, double far) {
		var pMsaa = Vector2.Create();
		var msaa = 0;
		for (var i = 0; i < 4; i++) {
			p.Add(this._msaaOffsetVec2[i], pMsaa);
			// 是否在三角形内
			if(!this.CheckInTriangle(pMsaa, barycentric)) continue;
			// 深度通过
			if(!this.CheckZ(x, y, barycentric, near, far, i)) continue;;
			// 写入
			this.frameBuffer.AddMsaaCount(x, y);
			msaa++;
		}
		Vector2.Return(pMsaa);
		return msaa != 0;
	}
	
	private bool CheckInTriangle(in Vector2 p, in Vector4 barycentric) {
		var position1 = this._triangle.position1;
		var position2 = this._triangle.position2;
		var position3 = this._triangle.position3;
		Utils.GetBarycentric(p, position1, position2, position3, barycentric);
		return barycentric.x >= 0 && barycentric.y >= 0 && barycentric.z >= 0;
	}
	
	private bool CheckZ(int x, int y, in Vector4 barycentric, double near, double far) {
		var z = this.GetInterpolationZ(barycentric, near, far);
		if (!this.frameBuffer.ZTest(x, y, -z)) return false;
		this.frameBuffer.SetZ(x, y, -z);
		return true;
	}

	private bool CheckZ(int x, int y, in Vector4 barycentric, double near, double far, int level) {
		var z = this.GetInterpolationZ(barycentric, near, far);
		if (!this.frameBuffer.ZTest(x, y, -z, level)) return false;
		this.frameBuffer.SetZ(x, y, -z, level);
		return true;
	}
    
}