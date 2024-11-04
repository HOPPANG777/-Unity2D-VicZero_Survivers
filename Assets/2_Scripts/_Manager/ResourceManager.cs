using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // 유닛
    public UnitData[] UnitDatas;

    // 무기
    public WeaponData[] WeaponDatas;

    // 웨이브
    public WaveData[] WaveDatas;

    // 기타(경험치, 상자, 배럴, 푸드)
    public ItemData[] ItemDatas;

    // VFX(비주얼 이펙트)
    public VFXData[] VFXDatas;

    // 장신구(패시브 스탯 증가)
    public AccessoryData[] AccessoryDatas;            // STO 넣어놓는곳

    // 미션
    public MissionData[] MissionDatas;

    public GhostEffectController GhostEffect;

    // 외부에서 유닛 STO불러올때 쓰는거
    public UnitData ConvertUnitTypeToUnitDataArr(UnitType pUnitType)
    {
        UnitData TempUnitTypeSTO = null;

        for (int i = 0; i < UnitDatas.Length; i++)
        {
            if (UnitDatas[i].UnitType == pUnitType) TempUnitTypeSTO = UnitDatas[i];
        }

        return TempUnitTypeSTO;
    }

    // 외부에서 Item STO불러올때 쓰는거
    public ItemData ConvertItemTypeToItemDataArr(ItemType pItemType)
    {
        ItemData TempItemTypeSTO = null;

        for (int i = 0; i < ItemDatas.Length; i++)
        {
            if (ItemDatas[i].ItemType == pItemType) TempItemTypeSTO = ItemDatas[i];
        }

        return TempItemTypeSTO;
    }

    // 외부에서 VFX불러올때 쓰는거
    public VFXData ConvertVFXTypeToVFXDataArr(VFXType pVT)
    {
        VFXData TempVTSTO = null;

        for (int i = 0; i < VFXDatas.Length; i++)
        {
            if (VFXDatas[i].VT == pVT) TempVTSTO = VFXDatas[i];
        }

        return TempVTSTO;
    }

    // 외부에서 무기 STO불러올때 쓰는거
    public WeaponData ConvertWeaponTypeToWeaponDataArr(WeaponType pweaponType)
    {
        WeaponData TempweaponTypeSTO = null;

        for (int i = 0; i < WeaponDatas.Length; i++)
        {
            if (WeaponDatas[i].weaponType == pweaponType) TempweaponTypeSTO = WeaponDatas[i];
        }

        return TempweaponTypeSTO;
    }

    // 외부에서 장신구 STO불러올때 쓰는거
    public AccessoryData ConvertAccessoryTypeToAccessoryDataArr(AccessoryType pEQ)
    {
        AccessoryData TempEQSTO = null;

        for (int i = 0; i < AccessoryDatas.Length; i++)
        {
            if (AccessoryDatas[i].ET == pEQ) TempEQSTO = AccessoryDatas[i];
        }

        return TempEQSTO;
    }

    // 외부에서 미션 STO불러올때 쓰는거
    public MissionData ConvertMissionTypeToMissionDataArr(MissionType pMission)
    {
        MissionData TempMissionSTO = null;

        for (int i = 0; i < MissionDatas.Length; i++)
        {
            if (MissionDatas[i].missionType == pMission) TempMissionSTO = MissionDatas[i];
        }

        return TempMissionSTO;
    }

}








