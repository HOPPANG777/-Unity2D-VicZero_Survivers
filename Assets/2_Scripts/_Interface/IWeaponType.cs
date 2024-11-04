using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponType
{
    WeaponType WeaponType { get; set; }
}

public enum WeaponType
{
    Axe,
    KingBible,
    Peachone,
    FireWand,
    Banana,
    Force,
    LightningRing,
    ShieldChasing,
    RuneTracer,
    Nunchaku
}
