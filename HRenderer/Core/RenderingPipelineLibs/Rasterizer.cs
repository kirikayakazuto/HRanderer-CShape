namespace HRenderer.Core.RenderingPipelineLibs; 

/**
 * 光栅化
 */
public abstract class Rasterizer {
    public abstract void Run(Material material, uint v1, uint v2, uint v3);
}