using HRenderer.Common;

namespace HRenderer.Core; 

public class Light {}

/**
 * 方向光
 */
public class DirectionLight: Light {
	// 光源位置
	public Vector4 position;
	// 光源方向
	public Vector4 direction;
	// 光颜色
	public Vector4 color = Vector4.Create(1, 1, 1, 1);
	// 环境光强度
	public double ambientStrength = 0.1;
	// 
	// 光强度
	public double intersity = 1;

	public DirectionLight(Vector4 position, Vector4 direction) {
		this.position = position;
		this.direction = direction;
	}
	
}

/**
 * 点光源(灯泡)
 */
public class PointLight: Light {
	// 光源位置
	public Vector4 position = Vector4.Create();
	// 
	public double range = 1;
	// 衰减
	public double attenuation = 1;
}