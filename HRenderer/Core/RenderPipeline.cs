using HRenderer.Common;

namespace HRenderer.Core; 

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

	public FrameBuffer frameBuffer { get; }
	
	public RenderPipeline(int width, int height) {
		this._width = width;
		this._height = height;
		this._viewPortMat4 = Utils.GetViewPortMatrix(width, height);
		this.frameBuffer = new FrameBuffer(width, height);
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
	}

	public void ClearFrameBuffer() {
		this.frameBuffer.Clear();
	}

	private void _drawTriangle(Material material, uint v1, uint v2, uint v3) {
		var mesh = material.mesh;
		var shader = material.shader;
		
		var near = shader.uniformFloats["CameraNear"];
		var far = shader.uniformFloats["CameraFar"];
		
		// 顶点数据解析
		mesh.GetVertexAttribs(v1, this._cacheData.Vec4Attribs1, this._cacheData.Vec2Attribs1);
		mesh.GetVertexAttribs(v2, this._cacheData.Vec4Attribs2, this._cacheData.Vec2Attribs2);
		mesh.GetVertexAttribs(v3, this._cacheData.Vec4Attribs3, this._cacheData.Vec2Attribs3);

		// 第一个阶段, 顶点着色器
		shader.AddAttribs(this._cacheData.Vec4Attribs1);
		shader.AddAttribs(this._cacheData.Vec2Attribs1);
		var position1 = shader.VertexShading();
		shader.AddAttribs(this._cacheData.Vec4Attribs2);
		shader.AddAttribs(this._cacheData.Vec2Attribs2);
		var position2 = shader.VertexShading();
		shader.AddAttribs(this._cacheData.Vec4Attribs3);
		shader.AddAttribs(this._cacheData.Vec2Attribs3);
		var position3 = shader.VertexShading();
		
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
		if (material.useFaceCulling && this.IsBackFace(position1, position2, position3)) return;

		// 光栅化
		var bound = Utils.GetBoundingBox(position1, position2, position3);
		var barycentric = Vector4.Create();
		var p = Vector2.Create();
		for (var y = Math.Max(bound.minY, 0); y < Math.Min(bound.maxY, this._height); y++) {
			p.y = y + 0.5f;
			for (var x = Math.Max(bound.minX, 0); x < Math.Min(bound.maxX, this._width); x++) {
				p.x = x + 0.5f;
				// 重心差值
				Utils.GetBarycentric(p, position1, position2, position3, barycentric);
				if(barycentric.x < 0 || barycentric.y < 0 || barycentric.z < 0) continue;
				
				// 计算z值 线性的z
				var z = Utils.GetInterpValue3(position1.z, position2.z, position3.z, barycentric.x, barycentric.y, barycentric.z);
				
				// 校正透视差值
				barycentric = Utils.AdjustBarycentric(barycentric, z1, z2, z3);
                    
				// 计算attribs差值
				this.ComputeShaderVectorVarying(mesh.attribInfo, shader, barycentric);
				
				// 计算非线性的z值
				z = Utils.GetDepth(near, far, z);
				
				// frag shading
				var color = shader.FragShading();

				// 输出颜色
				this.frameBuffer.SetColor(x , y, color, z);
				Vector4.Return(color);
			}
		}
		Vector4.Return(position1);
		Vector4.Return(position2);
		Vector4.Return(position3);
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
	private bool IsBackFace(Vector4 p1, Vector4 p2, Vector4 p3) {
		var a = p2.x - p1.x;
		var b = p2.y - p1.y;
		var c = p3.x - p1.x;
		var d = p3.y - p1.y;
		return a * d - b * c < 0;
	}
	
    
}