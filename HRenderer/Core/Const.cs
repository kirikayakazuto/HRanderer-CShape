using HRenderer.Common;

namespace HRenderer.Core; 

public static class Const {
    public static readonly Vector2[] MsaaOffsetVec2 = new Vector2[] {
        Vector2.Create(0.375, 0.125),
        Vector2.Create(0.125, -0.375),
        Vector2.Create(-0.375, -0.125),
        Vector2.Create(-0.125, -0.375),
		
    };
}

public enum RenderMode {
    /**
	 * 正常三角形光栅化
	 */
    Triangle,
    /**
	 * 线段模式
	 */
    Line,
    /**
	 * 点模式
	 */
    Point
}