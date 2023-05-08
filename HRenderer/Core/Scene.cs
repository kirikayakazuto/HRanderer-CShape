using HRenderer.Common;
using HRenderer.Materials.CubeMaterial;
using HRenderer.Materials.LightMaterial;

namespace HRenderer.Core; 

/**
 * 简单的场景类
 */
public class Scene {
	private const int _width = 600;
	private const int _height = 600;
	// 灯光
	private readonly DirectionLight _directionLight;
	
	private List<Material> _materials = new List<Material>();
	public List<Material> Materials => this._materials;

	public Scene(int width, int height) {
		this._directionLight = new DirectionLight(Vector4.Create(1.5, 1.5, 1.5, 1), Vector4.Create(-1.5, -1.5, -1.5, 1).NormalizeSelf());
		this.InitMaterial();
	}

	private void InitMaterial() {
		this._materials.Add(LightMaterial.GetMaterial());
		this._materials.Add(CubeMaterial.GetMaterial());
		var materials = this._materials;
		foreach (var material in materials) {
			material.uniformData.Vec4s["Light.Position"] = this._directionLight.position;
			material.uniformData.Vec4s["Light.Direction"] = this._directionLight.direction;
			material.uniformData.Vec4s["Light.Color"] = this._directionLight.color;	
		}
	}
	
}