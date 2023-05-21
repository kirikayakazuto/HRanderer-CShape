using HRenderer.Common;
using HRenderer.Scenes;

namespace HRenderer.Core {
	/**
     * 渲染器
     */
	public class Renderer {
		// size
		private readonly int _width = 0;
		private readonly int _height = 0;
		
		// 渲染管线
		public readonly RenderPipeline pipeline;

		public readonly Scene scene;
		
		public Renderer(int width, int height) {
			this._width = width;
			this._height = height;
			this.scene = new Scene1();
			this.pipeline = new RenderPipeline(width, height);
		}
		
		private double _time = 0;
		public void Render(double dt) {
			this.pipeline.ClearBuffer();

			this._time += dt;
			
			this.scene.Update(dt);
			
			var materials = this.scene.GetMaterials();
			foreach (var material in materials) {
				this.pipeline.Draw(material);	
			}
		}
        

	}
}