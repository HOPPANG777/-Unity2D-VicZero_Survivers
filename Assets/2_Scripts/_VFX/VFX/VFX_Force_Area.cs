//using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Force_Area : VFX
{
   
    public override void Start()
    {
        base.Start();
    }

    public void VFX_Force_Particle()
    {
        if (PS == null)
        {
            PS = CreateForceParticleSystem();
        }

        // ParticleSystemRenderer 설정
        var PSR = PS.GetComponent<ParticleSystemRenderer>();
        if (PSR == null)
        {
            PSR = PS.gameObject.AddComponent<ParticleSystemRenderer>();
        }

        // 메인 모듈 설정
        var main = PS.main;
        main.prewarm = true;
        main.startLifetime = 2.0f;
        main.startSpeed = 0.0f;
        main.startSize = 1.6f;
        main.startColor = new Color(1, 1, 1, 1.0f);

        // Emission 모듈 설정
        var emission = PS.emission;
        emission.rateOverTime = 0.5f;

        // Shape 모듈 설정
        var shape = PS.shape;
        shape.enabled = false;

        // Color over Lifetime 설정
        var colorOverLifetime = PS.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient Garlic_Grad = new Gradient();
        Garlic_Grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f), new GradientAlphaKey(0.5f, 0.5f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(Garlic_Grad);

        // Rotation over Lifetime 설정
        var rotationOverLifetime = PS.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(100.0f);

        // Texture Sheet Animation 설정
        var textureSheetAnimation = PS.textureSheetAnimation;
        textureSheetAnimation.enabled = true;
        textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;
        textureSheetAnimation.AddSprite(All_Manager.instance.Resource_M.Convert_VFX_Type_To_STO_VFX_Arr(VFX_Type.Garlic_Area).Sprite_Image);
        textureSheetAnimation.timeMode = ParticleSystemAnimationTimeMode.Lifetime;

        // Material 설정
        if (PSR != null)
        {
            PSR.material = All_Manager.instance.Resource_M.Convert_VFX_Type_To_STO_VFX_Arr(VFX_Type.Garlic_Area).VFX_Material;
        }

        // 파티클 시스템 재생
        PS.Play();
    }

    private ParticleSystem CreateForceParticleSystem()
    {
        GameObject particleObject = new GameObject("ForceParticleSystem");
        particleObject.transform.SetParent(transform);
        particleObject.transform.localPosition = Vector3.zero;
        ParticleSystem particleSystem = particleObject.AddComponent<ParticleSystem>();

        // 기본 설정 추가
        var main = particleSystem.main;
        main.loop = false;
        main.duration = 2.0f;

        return particleSystem;
    }
}
