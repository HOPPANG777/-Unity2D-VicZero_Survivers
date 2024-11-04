using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitInformation
{
    public float HP;
    public float UnitSpeed;
    public int UnitDefense;
    public float UnitWeaponDamage;
    public float UnitWeaponProjSpeed;
    public float UnitWeaponDuration;
    public float UnitWeaponRange;
    public float UnitWeaponCoolDown;
    public int UnitWeaponProjNum;
    public float UnitGetGemRange;
}

[CreateAssetMenu(menuName = "STO/Unit")]

public class UnitData : ScriptableObject
{
    // 프리팹이 아니라 그냥 프리팹 없이 게임 오브젝트를 깡으로 생성할수 있다.
    
    // STO는 클립보드, 원한는 게임오브젝트에 STO에 입력된 데이터를 붙여넣기 하는거
    // STO는 붙여넣기만 해야한다.
    // 

    // 와 씨 난 STO없었으면 시체였을꺼야
    // 스크립트 하나에 스탯 다 박을꺼 생각했었으면 어우.

    public UnitType UnitType;

    public float SizeX;
    public float SizeY;

    public float OffsetX;
    public float OffsetY;

    public float spriteRendererSizeX;
    public float spriteRendererSizeY;

    public UnitInformation Playerinfos;

    public float UnitEnemyHP;
    public float UnitEnemySpeed;
    public float UnitEnemyDamage;

    public Sprite SpriteImage;
    public Material EliteMaterial;
    public AudioClip DeathAudioClip;
    public AudioClip HittedAudioClip;

    //public AnimationClip UnitAnimationClip;
    public RuntimeAnimatorController UnitAnimatorController;
}


