using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponInformation
{
    public float WeaponDamage;
    public float WeaponSpeed;
    public float WeaponRange;
    public float WeaponTime;
    public float WeaponPen;
    public float WeaponCoolDown;
    public float WeaponDuration;
    public float WeaponKnock;
    public int WeaponCount;
    public float WeaponSize;
}
    
[CreateAssetMenu(menuName = "STO/Weapon")]

public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public WeaponKeyword WeaponKeywords;

    public string WeaponName;

    [TextArea]
    public string WeaponText;

    public WeaponInformation[] Infos;

    public float SizeX;
    public float SizeY;

    public float OffsetX;
    public float OffsetY;

    public bool Forever = false;

    public Sprite SpriteImage;
    public Sprite ItemImage;
    public Material SpriteMaterial;
    public AudioClip AttackingAudioClip;
    public RuntimeAnimatorController WeaponAnimatorController;
}
