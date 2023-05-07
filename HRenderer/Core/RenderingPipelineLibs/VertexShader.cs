using HRenderer.Common;

namespace HRenderer.Core.RenderingPipelineLibs; 

/**
 * 顶点着色器
 */
public abstract class VertexShader {
	public abstract Vector4 Run(GlData glData);
}