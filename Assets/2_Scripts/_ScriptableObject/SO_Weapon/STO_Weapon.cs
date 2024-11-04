using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Weapon_Information
{
    public float Weapon_Damage;
    public float Weapon_Speed;
    public float Weapon_Range;
    public float Weapon_Time;
    public float Weapon_Pen;
    public float Weapon_CoolDown;
    public float Weapon_Duration;
    public float Weapon_Knock;
    public int Weapon_Count;
    public float Weapon_Size;
}
    
[CreateAssetMenu(menuName = "STO/Weapon")]

public class STO_Weapon : ScriptableObject
{
    public WeaponType weaponType;
    public WeaponKeyword WeaponKeywords;

    public string Weapon_Name;

    [TextArea]
    public string Weapon_Text;

    public Weapon_Information[] Info_s;

    public float Size_X;
    public float Size_Y;

    public float Offset_X;
    public float Offset_Y;

    public bool Forever = false;

    public Sprite Sprite_Image;
    public Sprite Item_Image;
    public Material Sprite_Material;
    public AudioClip Attacking_Audio_Clip;
    public RuntimeAnimatorController Weapon_Animator_Controller;
}
