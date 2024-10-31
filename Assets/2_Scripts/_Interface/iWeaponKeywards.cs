using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponKeywards
{
    WeaponKeyward WeaponKeyward { get; set; }
}


public enum WeaponKeyward
{
    Bound,      // Bound Axe
    Phoenix,    // Phoenix

}
