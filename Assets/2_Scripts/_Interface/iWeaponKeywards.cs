using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponKeywards
{
    WeaponKeyward WeaponKeyward { get; set; }
}

[System.Serializable]
public enum WeaponKeyward
{
    NONE,       // None
    Bound,      // Bound Axe
    Phoenix,    // Phoenix
}
