using HRenderer.Common;
using HRenderer.Core.RenderingPipelineLibs;

namespace HRenderer.Core; 

/**
 * 渲染管线
 */
public class RenderingPipeline {
	// 顶点着色器
	private readonly VertexShader vertexShader;
	
	// 图元组装
	// ShapeAssembly
	
	// 几何着色器
	private readonly GeometryShader geometryShader;
	
	// 光栅化
	private readonly Rasterizer rasterizer;
	
	// 片元着色器
	private readonly FragmentShader fragmentShader;
	
	// 测试和混合
	// Tests And Blending
	
	
	// 缓存数据
	private readonly AttribsData _attribsData = new AttribsData();
	private readonly VaryingData _varyingData = new VaryingData();
	// gl缓存数据
	private readonly List<Vector4> _positions = new List<Vector4>();
	private readonly List<Vector4> gl_in = new List<Vector4>();
	private readonly List<Vector4> gl_out = new List<Vector4>();
	
	// 当前处理的三角形
	private readonly Triangle _triangle = new Triangle();

	private RenderMode _renderMode = RenderMode.Triangle;

	public RenderingPipeline(VertexShader vertexShader, GeometryShader geometryShader, Rasterizer rasterizer, FragmentShader fragmentShader) {
		this.vertexShader = vertexShader;
		this.geometryShader = geometryShader;
		this.rasterizer = rasterizer;
		this.fragmentShader = fragmentShader;
	}

	public void Run(Material material) {
		var mesh = material.mesh;
		var shader = material.shader;
		
		var indices = mesh.Ibo;
		
		var vectorDict = new VectorDict();
		for (uint i = 0; i < mesh.Vbo.Length; i+=mesh.stride) {
			mesh.GetVertexAttribs(i, vectorDict);
			var position = shader.VertexShading(vectorDict, this._varyingData.VectorDict1);
			this._positions.Add(position);
		}
	}
}