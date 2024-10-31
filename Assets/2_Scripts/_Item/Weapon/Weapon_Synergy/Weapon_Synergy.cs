using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Weapon_Synergy : Weapon
{
    public Weapon_Type Weapon1;
    public int RequiredLevel1;
    public Weapon_Type Weapon2;
    public int RequiredLevel2;

    // 강화할 스탯 정보: 키워드와 강화 비율의 딕셔너리
    public Dictionary<WeaponKeyward, float> StatBoosts;

    // 이미 적용되었는지 여부를 저장하는 플래그
    public bool IsApplied = false;

    public Weapon_Synergy(Weapon_Type weapon1, int level1, Weapon_Type weapon2, int level2, Dictionary<WeaponKeyward, float> statBoosts)
    {
        Weapon1 = weapon1;
        RequiredLevel1 = level1;
        Weapon2 = weapon2;
        RequiredLevel2 = level2;
        StatBoosts = statBoosts;
    }
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