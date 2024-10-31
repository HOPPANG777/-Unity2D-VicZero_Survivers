using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponKeywards
{
    WeaponKeyward WeaponKeyward { get; set; }
}

[System.Serializable]
[Flags]
public enum WeaponKeyward
{
    NONE = 0,       // None
    Bound = 1 << 0,      // Bound Axe
    Phoenix = 1 << 1,    // Phoenix
    Paladin = 1 << 2,
    Sonic = 1 << 3,
    Holy = 1 << 4
}
