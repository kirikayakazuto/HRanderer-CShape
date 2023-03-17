using HRenderer.Common;

namespace HRenderer.Core {
	/**
     * 渲染器
     */
	public class Renderer {
		// size
		private readonly int _width = 0;
		private readonly int _height = 0;
		// 相机
		private readonly Camera _camera;
		// 待渲染的材质
		private readonly List<Material> _materials = new List<Material>();
		// 渲染管线
		public readonly RenderPipeline pipeline;
        
		public Renderer(int width, int height) {
			this._width = width;
			this._height = height;
            
			this._camera = new Camera(width, height);
			this.pipeline = new RenderPipeline(width, height);
		}

		public void AddMaterial(Material material) {
			this._materials.Add(material);
		}

		private float _time = 0;
		public void Render(double dt) {
			this.pipeline.ClearFrameBuffer();
			
			this._time += (float)dt;
			foreach (var material in this._materials) {
				var shader = material.shader; 
				shader.view = this._camera.viewMat;
				shader.projection = this._camera.OrthographicProjection;
                
				shader.AddUniforms(material.uniformFloats);
				shader.AddUniforms(material.uniformVec4);
				shader.AddUniforms(material.uniformTextures);
				shader.uniformFloats["time"] = this._time;
				
				shader.uniformMatrix4s["CameraView"] = this._camera.viewMat;
				shader.uniformMatrix4s["CameraProjection"] = this._camera.OrthographicProjection;
				shader.uniformVec4["CameraPosition"] = this._camera.GetPosition();
				shader.uniformFloats["CameraNear"] = this._camera.near;
				shader.uniformFloats["CameraFar"] = this._camera.far;
                
				this.pipeline.Draw(material);
			}
		}
        

	}
}