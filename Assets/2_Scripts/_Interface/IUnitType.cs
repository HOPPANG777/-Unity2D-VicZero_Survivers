using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitType 
{
    UnitType UnitType { get; set; }
}

public enum UnitType
{
    Player,
    Boss,
    Barrel,
    Skeleton,
    Archer,
    Knight,
    OrcElite,
    Orc,
    Werebear,
    Slime,
    Werewolf
}