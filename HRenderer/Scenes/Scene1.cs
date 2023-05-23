using HRenderer.Core;
using HRenderer.Materials.CubeMaterial;
using HRenderer.Materials.LightMaterial;
using HRenderer.Materials.SkyPanoramicMaterial;
using HRenderer.Materials.SphereMaterial;
using HRenderer.Materials.SpriteMaterial;

namespace HRenderer.Scenes; 

public class Scene1: Scene {
    
    public Scene1(): base(600, 600) {
        // this.AddMaterial(SkyPanoramicMaterial.GetMaterial());
        // this.AddMaterial(SpriteMaterial.GetMaterial());
        this.AddMaterial(SphereMaterial.GetMaterial());
        // this.AddMaterial(LightMaterial.GetMaterial());
        // this.AddMaterial(CubeMaterial.GetMaterial());
    }

    protected override void OnUpdate(double dt) {
        
    }
}