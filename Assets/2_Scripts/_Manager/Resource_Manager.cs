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
    public STO_MISC[] STO_MISC_s;

    // VFX(비주얼 이펙트)
    public STO_VFX[] STO_VFXs;

    // 장신구(패시브 스탯 증가)
    public STO_Equipment[] STO_Equipments;            // STO 넣어놓는곳

    public Ghost_Effect_Controller Ghost_Effect;

    // 외부에서 유닛 STO불러올때 쓰는거
    public STO_Unit Convert_Unit_Type_To_STO_Unit_Arr(Unit_Type p_UT)
    {
        STO_Unit Temp_UT_STO = null;

        for (int i = 0; i < STO_Units.Length; i++)
        {
            if (STO_Units[i].UT == p_UT) Temp_UT_STO = STO_Units[i];
        }

        return Temp_UT_STO;
    }

    // 외부에서 MISC STO불러올때 쓰는거
    public STO_MISC Convert_MISC_Type_To_STO_MISC_Arr(MISC_Type p_MT)
    {
        STO_MISC Temp_MT_STO = null;

        for (int i = 0; i < STO_MISC_s.Length; i++)
        {
            if (STO_MISC_s[i].MT == p_MT) Temp_MT_STO = STO_MISC_s[i];
        }

        return Temp_MT_STO;
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
    public STO_Weapon Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type p_WT)
    {
        STO_Weapon Temp_WT_STO = null;

        for (int i = 0; i < STO_Weapons.Length; i++)
        {
            if (STO_Weapons[i].WT == p_WT) Temp_WT_STO = STO_Weapons[i];
        }

        return Temp_WT_STO;
    }

    // 외부에서 장신구 STO불러올때 쓰는거
    public STO_Equipment Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type p_EQ)
    {
        STO_Equipment Temp_EQ_STO = null;

        for (int i = 0; i < STO_Equipments.Length; i++)
        {
            if (STO_Equipments[i].ET == p_EQ) Temp_EQ_STO = STO_Equipments[i];
        }

        return Temp_EQ_STO;
    }

}








