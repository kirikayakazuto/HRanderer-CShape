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
     * 细分曲面着色器
     */
    public void TessellationShader(GlData glData) {
	    
    }

    /**
     * 几何着色器
     */
    public  void GeometryShader(GlData glData) {
	    
    }
    
    /**
     * 片元着色器
     */
    public abstract Vector4 FragShading();
    
    /**
     * 纹理采样
     */
    protected Vector4 Texture2D(BaseTexture t, Vector2 uv) {
        return t.Sample(uv.x, uv.y);
    }


    public void AddUniforms(UniformData uniformDatas) {
	    foreach (var keyValuePair in uniformDatas.Doubles) {
		    this.uniformData.Doubles[keyValuePair.Key] = keyValuePair.Value;
	    }
	    foreach (var keyValuePair in uniformDatas.Vec4s) {
		    this.uniformData.Vec4s[keyValuePair.Key] = keyValuePair.Value;
	    }
	    foreach (var keyValuePair in uniformDatas.Matrix4s) {
		    this.uniformData.Matrix4s[keyValuePair.Key] = keyValuePair.Value;
	    }
	    foreach (var keyValuePair in uniformDatas.Textures) {
		    this.uniformData.Textures[keyValuePair.Key] = keyValuePair.Value;
	    }
    }
}
