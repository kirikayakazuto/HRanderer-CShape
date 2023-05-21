using HRenderer.Core;
using HRenderer.Materials.SpriteMaterial;

namespace HRenderer.Scenes; 

public class Scene1: Scene {
    
    public Scene1(): base(600, 600) {
        this.AddMaterial(SpriteMaterial.GetMaterial());
    }

    protected override void OnUpdate(double dt) {
        
    }
}