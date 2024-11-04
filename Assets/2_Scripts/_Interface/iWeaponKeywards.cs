using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponKeywords
{
    WeaponKeyword WeaponKeyword { get; set; }
}

[System.Serializable]
public enum WeaponKeyword
{
    NONE,       // None
    Bound,      // Axe RuneTracer       -> Bound Axe(조합무기)
    Phoenix,    // FireWand Peachone    -> Phoenix(조합무기)
    Paladin,    // ShieldChasing Banana   -> 사거리 1.5배
    Sonic,      // LightningRing Nunchaku   -> 공격속도 1.5배
    Holy        // Force KingBible     -> 사이즈 1.5배
}
