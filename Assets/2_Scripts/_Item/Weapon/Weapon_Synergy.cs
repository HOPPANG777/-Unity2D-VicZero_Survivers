using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Weapon_Synergy : Weapon
{
    
}


#region
/*
    protected Weapon weapon;

    public Weapon_Synergy(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Weapon_Stat_Appliaction()
    {
        base.Weapon_Stat_Appliaction();
    }


 public class Weapon_Synergy : MonoBehaviour
{
    private Dictionary<List<WeaponKeyward>, string> synergyEffects;

    public Weapon_Synergy()
    {
        synergyEffects = new Dictionary<List<WeaponKeyward>, string>(new TypeListComparer())
        {
            { new List<WeaponKeyward> { WeaponKeyward.Bound, WeaponKeyward.Bound }, "Bound Axe"  },
            { new List<WeaponKeyward> { WeaponKeyward.Phoenix, WeaponKeyward.Phoenix }, "Phoenix" },
            // 다른 시너지 효과 추가 가능
        };
    }

    public string GetSynergyEffect(List<WeaponKeyward> weaponType)
    {
        foreach (var synergy in synergyEffects)
        {
            if (synergy.Key.TrueForAll(keyword => weaponType.Contains(keyword)))
            {
                return synergy.Value;
            }
        }
        return null; // 시너지 효과가 없는 경우
    }
}

// List 비교를 위한 커스텀 Comparer
public class TypeListComparer : IEqualityComparer<List<WeaponKeyward>>
{
    public bool Equals(List<WeaponKeyward> x, List<WeaponKeyward> y)
    {
        if (x == null || y == null)
            return false;

        if (x.Count != y.Count)
            return false;

        return new HashSet<WeaponKeyward>(x).SetEquals(y);
    }

    public int GetHashCode(List<WeaponKeyward> obj)
    {
        if (obj == null)
            return 0;

        int hash = 19;
        foreach (var keyword in obj)
        {
            hash = hash * 31 + keyword.GetHashCode();
        }
        return hash;
    }
}
 
 
*/
#endregion