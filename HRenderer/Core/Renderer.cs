namespace HRenderer.Core {
	/**
     * 渲染器
     */
	public class Renderer {
		private static readonly Renderer _renderer = new Renderer(600, 600);
		public static Renderer instance => Renderer._renderer;

		// size
		private readonly int _width = 0;
		private readonly int _height = 0;
		
		// 渲染管线
		public readonly RenderPipeline pipeline;
		
		private Renderer(int width, int height) {
			this._width = width;
			this._height = height;
			this.pipeline = new RenderPipeline(width, height);
		}
		
		
		public void Render() {
			this.pipeline.ClearBuffer();
			var materials = Scene.materials;
			foreach (var material in materials) {
				this.pipeline.Draw(material);	
			}
		}
        

	}
}