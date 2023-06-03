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

	private readonly Scene _scene;
	private Game() {
		this._scene = new Scene1();
		this.OnStart();
	}

	private void OnStart() {
		var timer = new Timer() { Interval = 1000, Enabled = true};
		timer.Elapsed += (state, e) => {
			Console.WriteLine("=====");
			timer.Stop();
		};
	}

	private double _passTime = 0;

	public void MainLoop(double dt) {
		this._passTime += dt;
		// 1, 场景更新
		this._scene.Update(dt);
		// 2, 开始渲染
		Renderer.instance.Render();
	}
}