using HRenderer.Common;

namespace HRenderer.Core; 

/**
 * 方向光
 */
public struct DirectionLight {
	// 光源位置
	public Vector4 position = Vector4.Create();
	// 光源方向
	public Vector4 direction = Vector4.Create();
	// 光颜色
	public Vector4 color = Vector4.Create();

	public double ambientStrength = 0.1;
	// 光强度
	public double intersity = 1;
	
	
	// // 环境颜色
	// public Vector4 ambient = Vector4.Create(1, 1, 1, 1);
	// // 漫反射颜色
	// public Vector4 diffuse = Vector4.Create();
	// // 镜面反射颜色
	// public Vector4 specular = Vector4.Create();

	DirectionLight(Vector4 position) {
		this.position = position;
	}
	
	
}

/**
 * 点光源(灯泡)
 */
public struct PointLight {
	// 光源位置
	public Vector4 position = Vector4.Create();
	// 
	public double range = 1;
	// 衰减
	public double attenuation = 1;

	PointLight(Vector4 position) {
		this.position = position;
	}
}