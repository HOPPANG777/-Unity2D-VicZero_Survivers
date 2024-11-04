using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Unit_Information
{
    public float HP;
    public float Unit_Speed;
    public int Unit_Defense;
    public float Unit_Weapon_Damage;
    public float Unit_Weapon_Proj_Speed;
    public float Unit_Weapon_Duration;
    public float Unit_Weapon_Range;
    public float Unit_Weapon_CoolDown;
    public int Unit_Weapon_Proj_Num;
    public float Unit_Get_Gem_Range;
}

[CreateAssetMenu(menuName = "STO/Unit")]

public class STO_Unit : ScriptableObject
{
    // 프리팹이 아니라 그냥 프리팹 없이 게임 오브젝트를 깡으로 생성할수 있다.
    
    // STO는 클립보드, 원한는 게임오브젝트에 STO에 입력된 데이터를 붙여넣기 하는거
    // STO는 붙여넣기만 해야한다.
    // 

    // 와 씨 난 STO없었으면 시체였을꺼야
    // 스크립트 하나에 스탯 다 박을꺼 생각했었으면 어우.

    public UnitType UnitType;

    public float Size_X;
    public float Size_Y;

    public float Offset_X;
    public float Offset_Y;

    public float _spriteRenderer_Size_X;
    public float _spriteRenderer_Size_Y;

    public Unit_Information Player_info_s;

    public float Unit_Enemy_HP;
    public float Unit_Enemy_Speed;
    public float Unit_Enemy_Damage;

    public Sprite Sprite_Image;
    public Material Elite_Material;
    public AudioClip Death_Audio_Clip;
    public AudioClip Hitted_Audio_Clip;

    //public AnimationClip Unit_Animation_Clip;
    public RuntimeAnimatorController Unit_Animator_Controller;
}


