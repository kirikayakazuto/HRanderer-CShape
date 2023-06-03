namespace HRenderer.Core {
	/**
     * 渲染器
     */
	public class Renderer {
		private static readonly Renderer _renderer = new Renderer(600, 600);
		public static Renderer instance => Renderer._renderer;

		// size
		public readonly int width = 0;
		public readonly int height = 0;
		
		// 渲染管线
		public readonly RenderPipeline pipeline;
		
		private Renderer(int width, int height) {
			this.width = width;
			this.height = height;
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