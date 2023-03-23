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
		
		private DirectionLight _directionLight = new DirectionLight(Common.Vector4.Create(-2, -2, 2, 1), Common.Vector4.Create(2, 2, -2, 1).NormalizeSelf());

		public List<Material> materials => this._materials;

		public Renderer(int width, int height) {
			this._width = width;
			this._height = height;
            
			this._camera = new Camera(width, height);
			this.pipeline = new RenderPipeline(width, height);
		}

		public void AddMaterial(Material material) {
			this._materials.Add(material);
		}

		private double _time = 0;
		public void Render(double dt) {
			this.pipeline.ClearFrameBuffer();
			
			this._time += dt;

			var r = Math.Sin(this._time * 0.5) * Math.PI;
			var lightPos = this._directionLight.position;
			// lightPos = lightPos.Transform(Matrix4.GetRotationX(r));
			// lightPos = lightPos.Transform(Matrix4.GetRotationY(r));
			
			foreach (var material in this._materials) {
				var shader = material.shader; 
				shader.view = this._camera.viewMat;
				shader.projection = this._camera.OrthographicProjection;
                
				shader.AddUniforms(material.uniformDoubles);
				shader.AddUniforms(material.uniformVec4);
				shader.AddUniforms(material.uniformTextures);
				shader.uniformDoubles["time"] = this._time;
				
				shader.uniformDoubles["Camera.Near"] = this._camera.near;
				shader.uniformDoubles["Camera.Far"] = this._camera.far;
				
				shader.uniformMatrix4s["Camera.View"] = this._camera.viewMat;
				shader.uniformMatrix4s["Camera.Projection"] = this._camera.OrthographicProjection;
				
				shader.uniformVec4["Camera.Position"] = this._camera.GetPosition();

				shader.uniformVec4["Light.Position"] = lightPos;
				shader.uniformVec4["Light.Direction"] = this._directionLight.direction;
				shader.uniformVec4["Light.Color"] = this._directionLight.color;
                
				this.pipeline.Draw(material);
			}
		}
        

	}
}