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

	private readonly Scene _scene = new Scene1();
	private Game() {
		
	}

	public void MainLoop(double dt) {
		// 1, 场景更新
		this._scene.Update(dt);
		// 2, 开始渲染
		Renderer.instance.Render();
	}
}