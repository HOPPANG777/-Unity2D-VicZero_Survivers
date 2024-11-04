using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : Singleton<SynergyManager>
{
    // 무기 키워드 등록
    public void RegisterWeaponKeyword(WeaponKeyword keyword, Weapon weapon)
    {
        if (keyword == WeaponKeyword.NONE) return;

        if (!weapon.IsKeywordCounted)
        {
            AllManager.instance.GameModeM.CurrentPlayer.weaponKeywordCounts.TryGetValue(keyword, out int count);
            AllManager.instance.GameModeM.CurrentPlayer.weaponKeywordCounts[keyword] = count + 1;
            weapon.IsKeywordCounted = true;
        }
    }

    // 무기 키워드 등록 해제
    public void UnregisterWeaponKeyword(WeaponKeyword keyword, Weapon weapon)
    {
        if (keyword == WeaponKeyword.NONE) return;

        if (weapon.IsKeywordCounted)
        {
            AllManager.instance.GameModeM.CurrentPlayer.weaponKeywordCounts.TryGetValue(keyword, out int count);
            if (count > 1)
            {
                AllManager.instance.GameModeM.CurrentPlayer.weaponKeywordCounts[keyword] = count - 1;
            }
            else
            {
                AllManager.instance.GameModeM.CurrentPlayer.weaponKeywordCounts.Remove(keyword);
            }
            weapon.IsKeywordCounted = false;
        }
    }


    //// 무기 키워드를 카운트하는 딕셔너리
    //private Dictionary<WeaponKeyword, List<Weapon>> keywordWeapons = new Dictionary<WeaponKeyword, List<Weapon>>();
    //private HashSet<WeaponKeyword> appliedSynergies = new HashSet<WeaponKeyword>();

    //// 키워드 등록 함수
    //public void RegisterWeaponKeyword(WeaponKeyword keyword, Weapon weapon)
    //{
    //    if (keyword == WeaponKeyword.NONE)
    //        return;

    //    if (!keywordWeapons.ContainsKey(keyword))
    //    {
    //        keywordWeapons[keyword] = new List<Weapon>();
    //    }

    //    keywordWeapons[keyword].Add(weapon);

    //    // 키워드 등록 후 시너지 체크
    //    CheckForSynergy(keyword);
    //}

    //// 키워드 제거 함수 (무기가 파괴될 때 호출)
    //public void UnregisterWeaponKeyword(WeaponKeyword keyword, Weapon weapon)
    //{
    //    if (keyword == WeaponKeyword.NONE || !keywordWeapons.ContainsKey(keyword))
    //        return;

    //    keywordWeapons[keyword].Remove(weapon);

    //    if (keywordWeapons[keyword].Count == 0)
    //    {
    //        keywordWeapons.Remove(keyword);
    //        appliedSynergies.Remove(keyword); // 시너지 해제
    //    }
    //}

    //// 시너지 효과 체크 함수
    //private void CheckForSynergy(WeaponKeyword keyword)
    //{
    //    if (keywordWeapons.ContainsKey(keyword) && keywordWeapons[keyword].Count >= 2 && !appliedSynergies.Contains(keyword))
    //    {
    //        ApplySynergy(keyword);
    //    }
    //}

    //// 시너지 적용 함수
    //private void ApplySynergy(WeaponKeyword keyword)
    //{
    //    switch (keyword)
    //    {
    //        case WeaponKeyword.Bound:
    //            // 도끼와 RuneTracer 조합 시너지: Bound Axe 스탯 증가
    //            foreach (var weapon in keywordWeapons[keyword])
    //            {
    //                weapon.WeaponDamage *= 1.5f;
    //            }
    //            break;
    //        case WeaponKeyword.Phoenix:
    //            // FireWand와 Peachone 조합 시너지: 공격 속도 증가
    //            foreach (var weapon in keywordWeapons[keyword])
    //            {
    //                weapon.WeaponSpeed *= 1.5f;
    //            }
    //            break;
    //        case WeaponKeyword.Paladin:
    //            // ShieldChasing과 Banana 조합 시너지: 사거리 증가
    //            foreach (var weapon in keywordWeapons[keyword])
    //            {
    //                weapon.WeaponRange *= 1.5f;
    //            }
    //            break;
    //        case WeaponKeyword.Sonic:
    //            // LightningRing과 Nunchaku 조합 시너지: 공격 속도 증가
    //            foreach (var weapon in keywordWeapons[keyword])
    //            {
    //                weapon.WeaponCoolDown *= 0.5f;
    //            }
    //            break;
    //        case WeaponKeyword.Holy:
    //            // Force과 KingBible 조합 시너지: 크기 증가
    //            foreach (var weapon in keywordWeapons[keyword])
    //            {
    //                weapon.WeaponSize *= 1.5f;
    //            }
    //            break;
    //    }
    //    appliedSynergies.Add(keyword);
    //}
}
