using HRenderer.Core;
using HRenderer.Materials.SkyPanoramicMaterial;
using HRenderer.Materials.SphereMaterial;
using HRenderer.Materials.SpriteMaterial;

namespace HRenderer.Scenes; 

public class Scene1: Scene {
    
    public Scene1(): base(600, 600) {
        // this.AddMaterial(SkyPanoramicMaterial.GetMaterial());
        // this.AddMaterial(SpriteMaterial.GetMaterial());
        this.AddMaterial(SphereMaterial.GetMaterial());
    }

    protected override void OnUpdate(double dt) {
        
    }
}