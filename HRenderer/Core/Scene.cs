using HRenderer.Common;
using HRenderer.Materials.CubeMaterial;
using HRenderer.Materials.LightMaterial;

namespace HRenderer.Core; 

/**
 * 简单的场景类
 */
public abstract class Scene {
	// 场景大小
	private readonly int _width;
	private readonly int _height;
	// 灯光
	private readonly DirectionLight _directionLight;
	
	// 相机
	private readonly Camera _camera;
	// 材质
	private readonly List<Material> _materials = new List<Material>();
	
	// 游戏运行时间
	private double _passTime = 0;

	public Scene(int width, int height) {
		this._width = width;
		this._height = height;
		this._camera = new Camera(width, height);
		this._directionLight = new DirectionLight(Common.Vector4.Create(-2, -2, 2, 1), Common.Vector4.Create(2, 2, -2, 1).NormalizeSelf());
	}

	protected void AddMaterial(Material material) {
		this._materials.Add(material);
	}

	public List<Material> GetMaterials() {
		return this._materials;
	}

	public void Update(double dt) {
		this.OnUpdate(dt);
		this.OnLateUpdate(dt);
	}


	protected abstract void OnUpdate(double dt);

	private void OnLateUpdate(double dt) {
		this._passTime += dt;
		foreach (var material in this._materials) {
			var shader = material.shader;
			
			shader.view = this._camera.viewMat;
			shader.projection = this._camera.OrthographicProjection;
                
			// 添加材质自带的uniforms
			shader.AddUniforms(material.uniformData);
				
			shader.uniformData.Doubles["time"] = this._passTime;
				
			shader.uniformData.Doubles["Camera.Near"] = this._camera.near;
			shader.uniformData.Doubles["Camera.Far"] = this._camera.far;
				
			shader.uniformData.Matrix4s["Camera.View"] = this._camera.viewMat;
			shader.uniformData.Matrix4s["Camera.Projection"] = this._camera.OrthographicProjection;
				
			shader.uniformData.Vec4s["Camera.Position"] = this._camera.GetPosition();

			shader.uniformData.Vec4s["Light.Position"] = this._directionLight.position;
			shader.uniformData.Vec4s["Light.Direction"] = this._directionLight.direction;
			shader.uniformData.Vec4s["Light.Color"] = this._directionLight.color;
		}
	}
	
}