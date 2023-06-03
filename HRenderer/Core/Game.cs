using HRenderer.Scenes;
using Timer = System.Timers.Timer;

namespace HRenderer.Core; 

/**
 * 控制整个游戏逻辑
 * 场景控制
 * 渲染状态
 */
public class Game {
	private static readonly Game _game = new Game();
	public static Game instance => Game._game;

	private readonly Scene scene;
	private ShadowTexture _shadowTexture;
	private Game() {
		this.scene = new Scene1();

		var width = Renderer.instance.width;
		var height = Renderer.instance.height;
		this._shadowTexture = new ShadowTexture(width, height);
	}

	private bool test1Runed = false;
	public void Test1() {
		Renderer.instance.Render();
		var depthBuffer = Renderer.instance.pipeline.depthBuffer.GetBuffer();
		this._shadowTexture.From(depthBuffer);
		var material = this.scene.GetMaterial("sprite");
		if (material != null) {
			material.uniformData.Textures["mainTexture"] = this._shadowTexture;    
		}
        
	}

	private double _passTime = 0;

	public void MainLoop(double dt) {
		this._passTime += dt;
		if (this._passTime >= 1) {
			this.Test1();
		}
		// 1, 场景更新
		this.scene.Update(dt);
		// 2, 开始渲染
		Renderer.instance.Render();
	}
}