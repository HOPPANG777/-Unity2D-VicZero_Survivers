//using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Garlic_Area : VFX
{
   
    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {

        
    }
    
    public void VFX_Garlic_Particle()
    {
        var PSR = PS.AddComponent<ParticleSystemRenderer>();

        var Garlic_Prewarm = PS.main.prewarm;
        Garlic_Prewarm = true;

        var Garlic_Start_LT = PS.main.startLifetime;
        Garlic_Start_LT = 2.0f;

        var Garlic_Start_SPD = PS.main.startSpeed;
        Garlic_Start_SPD = 0.0f;

        var Garlic_Start_Size = PS.main.startSize;
        Garlic_Start_Size = 1.6f;

        var Garlic_Start_Color = PS.main.startColor;
        Garlic_Start_Color = new Color(1, 1, 1, 0.5f);

        var Garlic_Emission = PS.emission;
        Garlic_Emission.rateOverTime = 0.5f;

        var Garlic_Shape = PS.shape;
        Garlic_Shape.enabled = false;

        var Garlic_Color_Time = PS.colorOverLifetime;
        Garlic_Color_Time.enabled = true;

        Gradient Garlic_Grad = new Gradient();
        Garlic_Grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f), new GradientAlphaKey(0.5f, 0.5f) });
       
        Garlic_Color_Time.color = Garlic_Grad;

        var Garlic_Rotation_Over_LT = PS.rotationOverLifetime;

        Garlic_Rotation_Over_LT.enabled = true;
        Garlic_Rotation_Over_LT.z = new ParticleSystem.MinMaxCurve(100.0f);

        var Garlic_Texture_SA = PS.textureSheetAnimation;

        Garlic_Texture_SA.enabled = true;
        Garlic_Texture_SA.mode = ParticleSystemAnimationMode.Sprites;
        Garlic_Texture_SA.AddSprite(All_Manager.instance.Resource_M.Convert_VFX_Type_To_STO_VFX_Arr(VFX_Type.Garlic_Area).Sprite_Image);
        Garlic_Texture_SA.timeMode = ParticleSystemAnimationTimeMode.Lifetime;

        var Garlic_Renderer_Material = PSR.material;

        Garlic_Renderer_Material = All_Manager.instance.Resource_M.Convert_VFX_Type_To_STO_VFX_Arr(VFX_Type.Garlic_Area).VFX_Material;

        PSR.material = Garlic_Renderer_Material;

        this.PS.Play();
    }

}
