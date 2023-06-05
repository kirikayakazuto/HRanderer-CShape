using HRenderer.Core;
using HRenderer.Materials.CubeMaterial;
using HRenderer.Materials.LightMaterial;
using HRenderer.Materials.SpriteMaterial;

namespace HRenderer.Scenes; 

public class Scene2: Scene {
    public Scene2() : base(600, 600) {
        this.AddMaterial(SpriteMaterial.GetMaterial());
        this.AddMaterial(CubeMaterial.GetMaterial1());
        this.AddMaterial(SpriteMaterial.GetMaterial4());
    }

    protected override void OnUpdate(double dt) {
        
    }
}