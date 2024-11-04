using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Manager : MonoBehaviour
{
    // 유닛
    public STO_Unit[] STO_Units;

    // 무기
    public STO_Weapon[] STO_Weapons;

    // 웨이브
    public STO_Wave[] STO_Waves;

    // 기타(경험치, 상자, 배럴, 푸드)
    public STO_Item[] STO_Item_s;

    // VFX(비주얼 이펙트)
    public STO_VFX[] STO_VFXs;

    // 장신구(패시브 스탯 증가)
    public STO_Accessory[] STO_Accessorys;            // STO 넣어놓는곳

    // 미션
    public STO_Mission[] STO_Missions;

    public GhostEffectController Ghost_Effect;

    // 외부에서 유닛 STO불러올때 쓰는거
    public STO_Unit Convert_UnitType_To_STO_Unit_Arr(UnitType p_UnitType)
    {
        STO_Unit Temp_UnitType_STO = null;

        for (int i = 0; i < STO_Units.Length; i++)
        {
            if (STO_Units[i].UnitType == p_UnitType) Temp_UnitType_STO = STO_Units[i];
        }

        return Temp_UnitType_STO;
    }

    // 외부에서 Item STO불러올때 쓰는거
    public STO_Item Convert_ItemType_To_STO_Item_Arr(ItemType p_ItemType)
    {
        STO_Item Temp_ItemType_STO = null;

        for (int i = 0; i < STO_Item_s.Length; i++)
        {
            if (STO_Item_s[i].ItemType == p_ItemType) Temp_ItemType_STO = STO_Item_s[i];
        }

        return Temp_ItemType_STO;
    }

    // 외부에서 VFX불러올때 쓰는거
    public STO_VFX Convert_VFX_Type_To_STO_VFX_Arr(VFX_Type p_VT)
    {
        STO_VFX Temp_VT_STO = null;

        for (int i = 0; i < STO_VFXs.Length; i++)
        {
            if (STO_VFXs[i].VT == p_VT) Temp_VT_STO = STO_VFXs[i];
        }

        return Temp_VT_STO;
    }

    // 외부에서 무기 STO불러올때 쓰는거
    public STO_Weapon Convert_WeaponType_To_STO_Weapon_Arr(WeaponType p_weaponType)
    {
        STO_Weapon Temp_weaponType_STO = null;

        for (int i = 0; i < STO_Weapons.Length; i++)
        {
            if (STO_Weapons[i].weaponType == p_weaponType) Temp_weaponType_STO = STO_Weapons[i];
        }

        return Temp_weaponType_STO;
    }

    // 외부에서 장신구 STO불러올때 쓰는거
    public STO_Accessory Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType p_EQ)
    {
        STO_Accessory Temp_EQ_STO = null;

        for (int i = 0; i < STO_Accessorys.Length; i++)
        {
            if (STO_Accessorys[i].ET == p_EQ) Temp_EQ_STO = STO_Accessorys[i];
        }

        return Temp_EQ_STO;
    }

    // 외부에서 미션 STO불러올때 쓰는거
    public STO_Mission Convert_Mission_Type_To_STO_Mission_Arr(MissionType p_Mission)
    {
        STO_Mission Temp_Mission_STO = null;

        for (int i = 0; i < STO_Missions.Length; i++)
        {
            if (STO_Missions[i].missionType == p_Mission) Temp_Mission_STO = STO_Missions[i];
        }

        return Temp_Mission_STO;
    }

}








