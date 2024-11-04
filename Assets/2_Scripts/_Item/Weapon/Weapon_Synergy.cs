using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponSynergy 
{

}


#region
/*
    protected Weapon weapon;

    public WeaponSynergy(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void WeaponStatAppliaction()
    {
        base.WeaponStatAppliaction();
    }


 public class WeaponSynergy : MonoBehaviour
{
    private Dictionary<List<WeaponKeyword>, string> synergyEffects;

    public WeaponSynergy()
    {
        synergyEffects = new Dictionary<List<WeaponKeyword>, string>(new TypeListComparer())
        {
            { new List<WeaponKeyword> { WeaponKeyword.Bound, WeaponKeyword.Bound }, "Bound Axe"  },
            { new List<WeaponKeyword> { WeaponKeyword.Phoenix, WeaponKeyword.Phoenix }, "Phoenix" },
            // 다른 시너지 효과 추가 가능
        };
    }

    public string GetSynergyEffect(List<WeaponKeyword> weaponType)
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
public class TypeListComparer : IEqualityComparer<List<WeaponKeyword>>
{
    public bool Equals(List<WeaponKeyword> x, List<WeaponKeyword> y)
    {
        if (x == null || y == null)
            return false;

        if (x.Count != y.Count)
            return false;

        return new HashSet<WeaponKeyword>(x).SetEquals(y);
    }

    public int GetHashCode(List<WeaponKeyword> obj)
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