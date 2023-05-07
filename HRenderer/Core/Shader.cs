using HRenderer.Common;

namespace HRenderer.Core; 

public abstract class Shader {
	
	// 摄像机相关
	public Matrix4 view;
	public Matrix4 projection;
	
    // uniforms
    public readonly UniformData uniformData = new UniformData();

    public readonly VectorDict varyingDict = new VectorDict();
    /**
     * 顶点着色器
     */
    public abstract Vector4 VertexShading(GlData glData);

    /**
     * 几何着色器
     */
    public void GeometryShader(GlData glData) {
	    
    }
    
    /**
     * 片元着色器
     */
    public abstract Vector4 FragShading();
    
    /**
     * 纹理采样
     */
    protected Vector4 Texture2D(Texture t, Vector2 uv) {
        var color = t.Sample(uv.x, uv.y);
        var v = Vector4.Create(color.r / 256f, color.g / 256f, color.b / 256f, color.a / 256f);
        Color.Return(color);
        return v;
    }
    
    public void AddUniforms(Dictionary<string, double> dictionary) {
	    foreach (var keyValuePair in dictionary) {
		    this.uniformData.Doubles[keyValuePair.Key] = keyValuePair.Value;
	    }
	}
	public void AddUniforms(Dictionary<string, Vector4> dictionary) {
		foreach (var keyValuePair in dictionary) {
			this.uniformData.Vec4s[keyValuePair.Key] = keyValuePair.Value;
		}
	}
	public void AddUniforms(Dictionary<string, Texture> dictionary) {
		foreach (var keyValuePair in dictionary) {
			this.uniformData.Textures[keyValuePair.Key] = keyValuePair.Value;
		}
	}
    
}