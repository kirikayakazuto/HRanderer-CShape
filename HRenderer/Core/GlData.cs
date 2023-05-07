using HRenderer.Common;

namespace HRenderer.Core; 

public class GlData {
	// Attribute
	public readonly VectorDict attribsDict = new VectorDict();
	// varying
	public readonly VectorDict varyingDict = new VectorDict();
	// uniforms
	public readonly UniformData uniformData = new UniformData();
}