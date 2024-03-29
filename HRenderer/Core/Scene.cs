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
	public readonly DirectionLight directionLight;
	
	// 相机
	public readonly Camera camera;
	// 材质
	private static readonly List<Material> _materials = new List<Material>();
	public static List<Material> materials => Scene._materials;

	// 游戏运行时间
	private double _passTime = 0;

	public Scene(int width, int height) {
		this._width = width;
		this._height = height;
		this.camera = new Camera(width, height);
		this.directionLight = new DirectionLight(Common.Vector4.Create(-5, -3, 0, 1), Common.Vector4.Create(5, 3, 0, 1).NormalizeSelf());
	}

	protected void AddMaterial(Material material) {
		Scene._materials.Add(material);
	}

	public List<Material> GetMaterials() {
		return Scene._materials;
	}

	public Material? GetMaterial(string name) {
		foreach (var material in Scene._materials) {
			if (material.name == name) return material;
		}
		return null;
	}

	public void UpdateMaterialUniforms(Camera _camera) {
		foreach (var material in Scene._materials) {
			var shader = material.shader;

			var projectionMat = _camera.GetProjection();
			
			shader.view = _camera.viewMat;
			shader.projection = projectionMat;
                
			// 添加材质自带的uniforms
			shader.AddUniforms(material.uniformData);
				
			shader.uniformData.Doubles["time"] = this._passTime;
				
			shader.uniformData.Matrix4s["Camera.View"] = _camera.viewMat;
			shader.uniformData.Matrix4s["Camera.Projection"] = projectionMat;
				
			shader.uniformData.Vec4s["Camera.Position"] = _camera.GetPosition();

			shader.uniformData.Vec4s["Light.Position"] = this.directionLight.position;
			shader.uniformData.Vec4s["Light.Direction"] = this.directionLight.direction;
			shader.uniformData.Vec4s["Light.Color"] = this.directionLight.color;
		}
	}

	public void Update(double dt) {
		this.OnUpdate(dt);
		this._passTime += dt;
		this.UpdateMaterialUniforms(this.camera);
	}
	
	protected abstract void OnUpdate(double dt);
}