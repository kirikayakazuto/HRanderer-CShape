using HRenderer.Scenes;

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
	private Camera lightCamera;
	private Game() {
		this.scene = new Scene1();
	}
	
	private double _passTime = 0;

	public void MainLoop(double dt) {
		this._passTime += dt;
		// 1, 场景更新
		this.scene.Update(dt);
		// 2, 开始渲染
		Renderer.instance.Render();
		// RayTracing.Renderer.instance.Render();
	}
}