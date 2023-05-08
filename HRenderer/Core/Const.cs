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