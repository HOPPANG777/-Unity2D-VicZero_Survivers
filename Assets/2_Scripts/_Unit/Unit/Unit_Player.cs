using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEngine.UI.CanvasScaler;

[System.Serializable]
public enum Weapon_Type
{
    //None,             
    Axe,                
    KingBible,          
    Peachone,           
    FireWand,           
    Cross,              
    Garlic,             
    LightningRing,      
    ShadowPinion,       
    RuneTracer,         
    Whip                
}

[System.Serializable]
public enum Equipment_Type
{
    //None,
    Spinach,
    Bracer,
    Duplicator,
    EmptyTome,
    Wings,
    Candelabrador,
    Armour,
    SpellBinder,
    Attractorb,
    HollowHeart
}

[System.Serializable]
public class Weapon_Level_And_Timer
{
    public int Level = 0;
    public float Timer = 0;
}

public class Unit_Player : Unit
{
    public Rigidbody2D rigidBody;

    //ã�ƺ��鼭 �Ẹ��
    public List<Weapon> All_Weapon = new List<Weapon>();
    public List<STO_Equipment> All_Equipment = new List<STO_Equipment>();
    public Dictionary<WeaponKeyword, int> weaponKeywordCounts = new Dictionary<WeaponKeyword, int>();

    public Vector2 InputVec;

    public float W_Dist = 0.0f;

    public bool Player_Right = false;
    public bool Player_Left = false;

    public Weapon_Level_And_Timer Axe_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer KingBible_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Peachone_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer FireWand_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Cross_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Garlic_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer LightningRing_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer ShadowPinion_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer RuneTracer_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Whip_LAT = new Weapon_Level_And_Timer();

    public int Spinach_Level;
    public int Bracer_Level;
    public int Duplicator_Level;
    public int EmptyTome_Level;
    public int Wings_Level;
    public int Candelabrador_Level;
    public int Armour_Level;
    public int SpellBinder_Level;
    public int Attractorb_Level;
    public int HollowHeart_Level;

    private Dictionary<Equipment_Type, int> equipmentLevels = new Dictionary<Equipment_Type, int>
{
    { Equipment_Type.HollowHeart, 0 },
    { Equipment_Type.Wings, 0 },
    { Equipment_Type.Armour, 0 },
    { Equipment_Type.Spinach, 0 },
    { Equipment_Type.Bracer, 0 },
    { Equipment_Type.SpellBinder, 0 },
    { Equipment_Type.Candelabrador, 0 },
    { Equipment_Type.EmptyTome, 0 },
    { Equipment_Type.Duplicator, 0 },
    { Equipment_Type.Attractorb, 0 }
};

    public bool Creating_Weapon = false;
    public bool Creating_Equipment = false;
    public bool Weapon_Trigger = false;

    public bool Equipment_Level_Up = false;

    protected override void Awake()
    {
        base.Awake();
        rigidBody = this.AddComponent<Rigidbody2D>();
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
    public override void Start()
    {
        base.Start();
        /*
        Upgrade_Weapon(Weapon_Type.Axe);                // �ϼ�
        Upgrade_Weapon(Weapon_Type.KingBible);          // �ϼ� 
        Upgrade_Weapon(Weapon_Type.Peachone);           // �ϼ�
        Upgrade_Weapon(Weapon_Type.FireWand);           // �ϼ�
        Upgrade_Weapon(Weapon_Type.Cross);              // �ϼ�
        Upgrade_Weapon(Weapon_Type.Garlic);             // �ϼ�
        Upgrade_Weapon(Weapon_Type.LightningRing);      // �ϼ�
        Upgrade_Weapon(Weapon_Type.ShadowPinion);       // �ϼ�
        Upgrade_Weapon(Weapon_Type.RuneTracer);         // �ϼ�
        Upgrade_Weapon(Weapon_Type.Whip);               // �ϼ�
        */

        All_Manager.instance.Resource_M.Ghost_Effect.Add_Ghost_Target(this.transform);
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            Player_Move();

            Spawn_Weapon_Timer_Update();
        }
    }

    public override void Hitted(float p_Damage_Value)
    {
        base.Hitted(p_Damage_Value);

        All_Manager.instance.UI_M.Update_Player_HPBar_Image(Current_HP / Max_HP);
    }

    protected override IEnumerator CheckDeadWithDelay()
    {
        return base.CheckDeadWithDelay();
    }

    protected override bool CheckDead()
    {
        return base.CheckDead();
    }

    public override void Die()
    {
        base.Die();

        All_Manager.instance.GameMode_M.GameOver();
    }

    protected override void Death_Animation_End()
    {
        base.Death_Animation_End();
    }

    protected override IEnumerator C_Death_Animation_End()
    {
        return base.C_Death_Animation_End();
    }

    protected override void Return_Pool()
    {
        base.Return_Pool();
    }

    public override void Unit_Clear()
    {
        for (int i = 0; i < All_Weapon.Count; i++)
        {
            All_Weapon[i].Destroy_Weapon();
        }

        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
    Vector3 GetWeaponStartPosition(Weapon_Type weaponType)
    {
        Vector3 position = Vector3.zero;

        switch (weaponType)
        {
            case Weapon_Type.Axe:
            case Weapon_Type.KingBible:
            case Weapon_Type.Garlic:
            case Weapon_Type.ShadowPinion:
            case Weapon_Type.Cross:
            case Weapon_Type.RuneTracer:
            case Weapon_Type.Whip:
                return this.transform.position;
            case Weapon_Type.Peachone:
                return All_Manager.instance.UI_M.Peachone_Image.rectTransform.position;
            case Weapon_Type.LightningRing:
                return GetLightningRingStartPosition();
            case Weapon_Type.FireWand:
                return this.transform.position + new Vector3(0, -0.3f, 0);

            default:
                break;
        }
        return position;
    }
    Vector3 GetLightningRingStartPosition()
    {
        var enemyList = All_Manager.instance.GameMode_M?.Enemy_List;
        if (enemyList != null && enemyList.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyList.Count);
            var targetEnemy = enemyList[randomIndex];
            return targetEnemy?.transform.position ?? Vector3.zero;
        }
        return Vector3.zero;
    }

    public void Create_Weapon(Weapon_Type p_WT)
    {
        StartCoroutine(C_Create_Weapon(p_WT));
    }

    #region
    public IEnumerator C_Create_Weapon(Weapon_Type weaponType)
    {
        yield return new WaitWhile(() => Creating_Weapon == true);

        STO_Weapon Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(weaponType);

        Weapon_Level_And_Timer Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

        Creating_Weapon = true;

        GameObject weaponGo = All_Manager.instance.Pool_M.Weapon_Pool_Out(weaponType);

        if (weaponGo == null)
        {
            Debug.LogError("Weapon object is null.");
            yield break;
        }

        weaponGo.transform.position = GetWeaponStartPosition(weaponType);
        weaponGo.transform.rotation = Quaternion.identity;
        weaponGo.transform.localScale = Vector3.one;

        Weapon weapon = weaponGo.GetComponent<Weapon>();

        if (weapon == null)
            yield break;

        switch (weaponType)
        {
            case Weapon_Type.Axe:
                {
                    Vector3 One = this.transform.position;
                    Vector3 Two, Three, Four;

                    if (Weapon_Trigger)
                    {
                        Two = One + new Vector3(0, 5, 0);
                        Three = Two + new Vector3(Random.Range(1, 5), 0, 0);
                        Four = One + new Vector3(Random.Range(1, 5), -10, 0);
                    }
                    else
                    {
                        Two = One + new Vector3(0, 5, 0);
                        Three = Two + new Vector3(Random.Range(-5, -1), 0, 0);
                        Four = One + new Vector3(Random.Range(-5, -1), -10, 0);
                    }
                    Weapon_Axe waeponAxe = weaponGo.GetComponent<Weapon_Axe>();

                    waeponAxe.Weapon_Init(this, Weapon_STO, Axe_LAT.Level);
                    waeponAxe.Axe_Init(One, Two, Three, Four);

                    CheckSynergyEffects(waeponAxe.WeaponKeyword);

                    Weapon_Trigger = !Weapon_Trigger;

                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Axe);
                }
                break;
            case Weapon_Type.KingBible:
                {
                    weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, KingBible_LAT.Level);
                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_KingBible>().WeaponKeyword);

                    int Max = 0;

                    for (int i = 0; i < All_Weapon.Count; i++)
                    {
                        if (All_Weapon[i].GetComponent<Weapon_KingBible>())
                        {
                            Max++;
                        }
                    }

                    Max++;
                    for (int i = 0; i < All_Weapon.Count; i++)
                    {
                        if (All_Weapon[i].GetComponent<Weapon_KingBible>())
                        {
                            All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count = Max;
                        }
                    }

                    weaponGo.GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count = Max;
                    weaponGo.GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count = Max;

                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.KingBible);
                }
                break;
            case Weapon_Type.Peachone:
                {
                    var P_Peachone_UI_Range = weaponGo.GetComponent<Weapon_Peachone>().Convert_RectTransform_To_Position(All_Manager.instance.UI_M.Peachone_Range_Image.rectTransform);
                    var P_Peachone_UI = weaponGo.GetComponent<Weapon_Peachone>().Convert_RectTransform_To_Position(All_Manager.instance.UI_M.Peachone_Image.rectTransform);

                    Vector3 One = All_Manager.instance.UI_M.Peachone_Image.transform.position;
                    Vector3 Two = weaponGo.GetComponent<Weapon_Peachone>().Peachone_Point_Setting(All_Manager.instance.UI_M.Peachone_Image.transform.position);
                    Vector3 Three = weaponGo.GetComponent<Weapon_Peachone>().Peachone_Point_Setting(P_Peachone_UI_Range);
                    Vector3 Four = P_Peachone_UI_Range;

                    weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Peachone_LAT.Level);
                    weaponGo.GetComponent<Weapon_Peachone>().Peachone_Init(One, Two, Three, Four);
                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_Peachone>().WeaponKeyword);
                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Peachone);

                }
                break;
            case Weapon_Type.FireWand:
                {
                    var FireWand_Stat = Weapon_STO.Info_s[FireWand_LAT.Level];

                    for (int i = 0; i < Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT); i++)
                    {
                        weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, FireWand_LAT.Level);
                        CheckSynergyEffects(weaponGo.GetComponent<Weapon_FireWand>().WeaponKeyword);

                        Vector3 Dir_Vec = new Vector3(Mathf.Cos((Mathf.PI * 2 * i / Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT))), Mathf.Sin((Mathf.PI * i * 2 / Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT))));

                        if (All_Manager.instance.GameMode_M.Game_Stop == false)
                        {
                            weaponGo.transform.position = weaponGo.transform.position + (Dir_Vec.normalized * FireWand_Stat.Weapon_Speed * Time.deltaTime);
                        }

                        weaponGo.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT))) + 180);
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.FireWand);

                    }
                }
                break;
            case Weapon_Type.Cross:
                {
                    var W_Cross_S = Weapon_STO.Info_s[Cross_LAT.Level];

                    Vector3 One = this.transform.position;
                    Vector3 Target = One + new Vector3(Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range), Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range), 0);

                    Vector3 Two = One + (Target - One) / 3 + new Vector3(Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, 0);
                    Vector3 Three = Target + (One - Target) / 3 + new Vector3(Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, 0);
                    Vector3 Four = One + ((One - Target) * W_Cross_S.Weapon_Range);

                    weaponGo.GetComponent<Weapon_Cross>().Weapon_Init(this, Weapon_STO, Cross_LAT.Level);
                    weaponGo.GetComponent<Weapon_Cross>().Cross_Init(One, Two, Three, Four);
                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_Cross>().WeaponKeyword);
                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Cross);
                }
                break;
            case Weapon_Type.Garlic:
                {
                    weaponGo.transform.parent = this.transform;

                    weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Garlic_LAT.Level);
                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_Garlic>().WeaponKeyword);
                }
                break;
            case Weapon_Type.LightningRing:
                {
                    if (All_Manager.instance != null && All_Manager.instance.GameMode_M.Enemy_List != null && All_Manager.instance.GameMode_M.Enemy_List.Count > 0)
                    {
                        var Max_Count = Current_Weapon_Count(Weapon_Type.LightningRing, LightningRing_LAT);
                        int Count_To_Spawn = Mathf.Min(Max_Count, All_Manager.instance.GameMode_M.Enemy_List.Count);

                        List<int> Spawned_Indices = new List<int>();

                        for (int i = 0; i < Count_To_Spawn; i++)
                        {
                            int Random_Index = Random.Range(0, All_Manager.instance.GameMode_M.Enemy_List.Count);

                            if (Spawned_Indices.Contains(Random_Index))
                            {
                                continue;
                            }

                            Spawned_Indices.Add(Random_Index);

                            var Check_Enemy_Alive = All_Manager.instance.GameMode_M.Enemy_List[Random_Index];

                            if (Check_Enemy_Alive == null)
                            {
                                continue;
                            }

                            weaponGo = All_Manager.instance.Pool_M.Weapon_Pool_Out(weaponType);


                            if (weaponGo.GetComponent<Weapon_LightningRing>() != null)
                            {
                                weaponGo.transform.position = All_Manager.instance.GameMode_M.Enemy_List[Random_Index].transform.position;
                                weaponGo.transform.rotation = Quaternion.identity;
                                weaponGo.transform.localScale = Vector3.one;

                                yield return new WaitUntil(() => weaponGo.GetComponent<Weapon_LightningRing>().SR);

                                weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, LightningRing_LAT.Level);
                                CheckSynergyEffects(weaponGo.GetComponent<Weapon_LightningRing>().WeaponKeyword);

                                All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.LightningRing);
                            }
                        }
                    }
                }
                break;
            case Weapon_Type.ShadowPinion:
                {
                    weaponGo.transform.position = this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                    weaponGo.transform.rotation = Quaternion.identity;
                    weaponGo.transform.localScale = Vector3.one;

                    weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, ShadowPinion_LAT.Level);
                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_ShadowPinion>().WeaponKeyword);

                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.ShadowPinion);
                }
                break;
            case Weapon_Type.RuneTracer:
                {
                    weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, RuneTracer_LAT.Level);

                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_RuneTracer>().WeaponKeyword);
                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.RuneTracer);
                }
                break;
            case Weapon_Type.Whip:
                {
                    if (Weapon_Trigger)
                    {
                        weaponGo.transform.position = this.transform.position + new Vector3(-2f + Random.Range(-0.5f, 0.5f), -0.1f + Random.Range(-0.2f, 0.2f), 0);
                        weaponGo.transform.rotation = Quaternion.identity;
                        weaponGo.transform.localScale = new Vector3(-4, -1, -1);
                    }
                    else
                    {
                        weaponGo.transform.position = this.transform.position + new Vector3(2f + Random.Range(-0.5f, 0.5f), -0.1f + Random.Range(-0.2f, 0.2f), 0);
                        weaponGo.transform.rotation = Quaternion.identity;
                        weaponGo.transform.localScale = new Vector3(4, 1, 1);
                    }


                    weaponGo.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Whip_LAT.Level);
                    CheckSynergyEffects(weaponGo.GetComponent<Weapon_Whip>().WeaponKeyword);
                    Weapon_Trigger = !Weapon_Trigger;

                    All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Whip);

                }
                break;
            default:
                break;
        }

        if (weaponGo != null)
        {
            All_Weapon.Add(weapon);
        }
        Creating_Weapon = false;
    }

    public void Upgrade_Weapon(Weapon_Type weaponType)
    {
        // 무기 유형별로 WLAT과 컴포넌트 타입을 Dictionary에 저장
        var weaponLevels = new Dictionary<Weapon_Type, (Weapon_Level_And_Timer WLAT, System.Type Type)>
    {
        { Weapon_Type.Axe, (Axe_LAT, typeof(Weapon_Axe)) },
        { Weapon_Type.KingBible, (KingBible_LAT, typeof(Weapon_KingBible)) },
        { Weapon_Type.Peachone, (Peachone_LAT, typeof(Weapon_Peachone)) },
        { Weapon_Type.FireWand, (FireWand_LAT, typeof(Weapon_FireWand)) },
        { Weapon_Type.Cross, (Cross_LAT, typeof(Weapon_Cross)) },
        { Weapon_Type.Garlic, (Garlic_LAT, typeof(Weapon_Garlic)) },
        { Weapon_Type.LightningRing, (LightningRing_LAT, typeof(Weapon_LightningRing)) },
        { Weapon_Type.ShadowPinion, (ShadowPinion_LAT, typeof(Weapon_ShadowPinion)) },
        { Weapon_Type.RuneTracer, (RuneTracer_LAT, typeof(Weapon_RuneTracer)) },
        { Weapon_Type.Whip, (Whip_LAT, typeof(Weapon_Whip)) },
    };

        // 무기 유형이 Dictionary에 있는지 확인
        if (weaponLevels.TryGetValue(weaponType, out var weaponInfo))
        {
            // 해당 무기의 레벨 증가
            weaponInfo.WLAT.Level++;

            // 모든 무기 리스트에서 해당 무기 유형 찾기 및 업그레이드 호출
            foreach (var weapon in All_Weapon)
            {
                if (weapon.GetComponent(weaponInfo.Type) is Weapon matchedWeapon)
                {
                    matchedWeapon.Upgrade(weaponInfo.WLAT.Level);
                }
            }

            // 추가 로직이 필요한 경우 처리
            if (weaponType == Weapon_Type.KingBible)
            {
                Create_Weapon(Weapon_Type.KingBible);
            }
        }
    }

    // ������ �ִ���
    public bool Check_Weapon(Weapon_Type p_PT)
    {
        return false;
    }

    public void Spawn_Weapon_Timer_Update()
    {
        var weaponTimers = new Dictionary<Weapon_Type, Weapon_Level_And_Timer>
    {
        { Weapon_Type.Axe, Axe_LAT },
        { Weapon_Type.KingBible, KingBible_LAT },
        { Weapon_Type.Peachone, Peachone_LAT },
        { Weapon_Type.FireWand, FireWand_LAT },
        { Weapon_Type.Cross, Cross_LAT },
        { Weapon_Type.Garlic, Garlic_LAT },
        { Weapon_Type.LightningRing, LightningRing_LAT },
        { Weapon_Type.ShadowPinion, ShadowPinion_LAT },
        { Weapon_Type.RuneTracer, RuneTracer_LAT },
        { Weapon_Type.Whip, Whip_LAT }
    };

        foreach (var weapon in weaponTimers)
        {
            var weaponType = weapon.Key;
            var weaponData = weapon.Value;

            if (weaponData.Level <= 0) continue; // 레벨이 0보다 큰 경우에만 처리

            // 무기 쿨다운 체크
            if (weaponData.Timer > Current_Weapon_CoolDown(weaponType, weaponData))
            {
                weaponData.Timer = 0; // 타이머 초기화

                // 무기 스폰
                if (weaponType == Weapon_Type.FireWand || weaponType == Weapon_Type.LightningRing || weaponType == Weapon_Type.Garlic)
                {
                    // 단일 인스턴스만 생성하는 무기
                    Create_Weapon(weaponType);
                }
                else if (weaponType == Weapon_Type.KingBible)
                {
                    // 성경은 업그레이드에서 생성
                }
                else
                {
                    // Current_Weapon_Count에 따라 다수 생성하는 무기
                    int weaponCount = Current_Weapon_Count(weaponType, weaponData);
                    for (int i = 0; i < weaponCount; i++)
                    {
                        Create_Weapon(weaponType);
                    }
                }

            }

            // 타이머 증가
            weaponData.Timer += Time.deltaTime;
        }

        //Debug.Log("Weapon_Spawn!");
    }

    public void Update_Get_Equipment(Equipment_Type p_EquipmentType)
    {
        var equipmentSTO = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(p_EquipmentType);

        // p_EquipmentType과 equipmentSTO가 동일한 경우에만 호출
        if (equipmentSTO == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(p_EquipmentType))
        {
            Get_Equipment_Player_Percentage_Stat(this, equipmentSTO);
        }

        // equipmentSTO가 null이 아닌 경우 장비 추가
        if (equipmentSTO != null)
        {
            All_Equipment.Add(equipmentSTO);
        }
    }



    public void Upgrade_Equipment(Equipment_Type p_ET)
    {
        if (equipmentLevels.ContainsKey(p_ET))
        {
            equipmentLevels[p_ET] += 1;
            Update_Get_Equipment(p_ET);
        }
    }

    // 리팩토링된 Origin_Stat_Application_Player 메서드
    public void Origin_Stat_Application_Player(Equipment_Type p_EquipmentType)
    {
        if (!equipmentLevels.ContainsKey(p_EquipmentType) || equipmentLevels[p_EquipmentType] <= 0)
            return;

        switch (p_EquipmentType)
        {
            case Equipment_Type.Spinach:
                this.Origin_Stat.Unit_Weapon_Damage += this.Percent_Stat.Unit_Weapon_Damage;
                break;

            case Equipment_Type.Bracer:
                this.Origin_Stat.Unit_Weapon_Proj_Speed += this.Percent_Stat.Unit_Weapon_Proj_Speed;
                break;

            case Equipment_Type.Duplicator:
                this.Origin_Stat.Unit_Weapon_Proj_Count += this.Percent_Stat.Unit_Weapon_Proj_Count;
                break;

            case Equipment_Type.EmptyTome:
                this.Origin_Stat.Unit_Weapon_Range += this.Percent_Stat.Unit_Weapon_Range;
                break;

            case Equipment_Type.Wings:
                this.Origin_Stat.Unit_Speed += this.Origin_Stat.Unit_Speed * this.Percent_Stat.Unit_Speed * 0.01f;
                break;

            case Equipment_Type.Candelabrador:
                this.Origin_Stat.Unit_Weapon_Range += this.Percent_Stat.Unit_Weapon_Range;
                this.Origin_Stat.Unit_Weapon_Size += this.Percent_Stat.Unit_Weapon_Size;
                break;

            case Equipment_Type.Armour:
                this.Origin_Stat.Unit_Defense += this.Percent_Stat.Unit_Defense;
                break;

            case Equipment_Type.HollowHeart:
                float hpIncrease = this.Origin_Stat.Unit_HP * this.Percent_Stat.Unit_HP * 0.01f;
                this.Origin_Stat.Unit_HP += hpIncrease;
                this.Max_HP = this.Origin_Stat.Unit_HP;
                break;

            case Equipment_Type.SpellBinder:
                this.Origin_Stat.Unit_Weapon_Duration += this.Percent_Stat.Unit_Weapon_Duration;
                break;

            case Equipment_Type.Attractorb:
                this.Origin_Stat.Unit_Get_Gem_Range += this.Origin_Stat.Unit_Get_Gem_Range * this.Percent_Stat.Unit_Get_Gem_Range * 0.01f;
                break;
        }
    }


    //
    public void Get_Equipment_Player_Percentage_Stat(Unit Player_Unit, STO_Equipment Eq_STO)
    {
        var Temp_Equipment_STO = All_Manager.instance.Resource_M.STO_Equipments;

        if (Player_Unit == null)
        {
            Debug.Log("Unit NULL");
            return;
        }

        if (Eq_STO == null) //
        {
            return;
        }

        if (Eq_STO.ET == Temp_Equipment_STO[0].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Damage += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_Damage;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[1].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Proj_Speed += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_Proj_Speed;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[2].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Proj_Count += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_Proj_Count;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[3].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_CoolDown += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_CoolDown;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[4].ET)
        {
            Player_Unit.Percent_Stat.Unit_Speed += Eq_STO.Equipment_Stat_Sheet.Unit_Speed;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[5].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Range += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_Range;
            Player_Unit.Percent_Stat.Unit_Weapon_Size += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_Size;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[6].ET)
        {
            Player_Unit.Percent_Stat.Unit_Defense += Eq_STO.Equipment_Stat_Sheet.Unit_Defense;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[7].ET)
        {
            Player_Unit.Percent_Stat.Unit_HP += Eq_STO.Equipment_Stat_Sheet.Unit_HP;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[8].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Duration += Eq_STO.Equipment_Stat_Sheet.Unit_Weapon_Duration;
        }

        else if (Eq_STO.ET == Temp_Equipment_STO[9].ET)
        {
            Player_Unit.Percent_Stat.Unit_Get_Gem_Range += Eq_STO.Equipment_Stat_Sheet.Unit_Get_Gem_Range;
        }

    }

    //
    public Weapon_Level_And_Timer Convert_WLAT_Player(STO_Weapon p_WTO)
    {
        Weapon_Level_And_Timer WLAT = null;

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[0])
        {
            WLAT = Axe_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[1])
        {
            WLAT = KingBible_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[2])
        {
            WLAT = Peachone_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[3])
        {
            WLAT = FireWand_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[4])
        {
            WLAT = Cross_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[5])
        {
            WLAT = Garlic_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[6])
        {
            WLAT = LightningRing_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[7])
        {
            WLAT = ShadowPinion_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[8])
        {
            WLAT = RuneTracer_LAT;
        }

        if (p_WTO == All_Manager.instance.Resource_M.STO_Weapons[9])
        {
            WLAT = Whip_LAT;
        }

        return WLAT;
    }

    //
    public int Convert_Equipment_Level_Player(Equipment_Type p_ET)
    {
        // 요청된 장비 타입의 레벨을 반환, 없으면 0
        return equipmentLevels.TryGetValue(p_ET, out int level) ? level : 0;
    }

    private float GetCurrentWeaponStat(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT, Func<Weapon_Information, float> statSelector)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);
        var weaponInfo = STO_Weapon_STO.Info_s[p_WLAT.Level];

        // 선택한 통계 값을 가져온 후 Origin Stat과 비율을 적용하여 계산
        float weaponStatValue = statSelector(weaponInfo) * this.Origin_Stat.Unit_Weapon_Damage * 0.01f;

        // 최종 무기 통계 값 계산
        return statSelector(weaponInfo) + weaponStatValue;
    }

    public int Current_Weapon_Count(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);
        return this.Origin_Stat.Unit_Weapon_Proj_Count + STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Count;
    }

    public float Current_Weapon_CoolDown(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);
        var weaponInfo = STO_Weapon_STO.Info_s[p_WLAT.Level];

        // 무기 쿨다운을 감소값으로 계산
        float weaponCoolDownValue = (weaponInfo.Weapon_CoolDown * this.Origin_Stat.Unit_Weapon_CoolDown * 0.01f);
        return weaponInfo.Weapon_CoolDown - weaponCoolDownValue;
    }


    public float Current_Weapon_Range(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        return GetCurrentWeaponStat(p_WT, p_WLAT, weaponInfo => weaponInfo.Weapon_Range);
    }

    public float Current_Weapon_Size(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        return GetCurrentWeaponStat(p_WT, p_WLAT, weaponInfo => weaponInfo.Weapon_Size);
    }

    public float Current_Weapon_Speed(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        return GetCurrentWeaponStat(p_WT, p_WLAT, weaponInfo => weaponInfo.Weapon_Speed);
    }

    public float Current_Weapon_Damage(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        return GetCurrentWeaponStat(p_WT, p_WLAT, weaponInfo => weaponInfo.Weapon_Damage);
    }

    public float Current_Weapon_Duration(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        return GetCurrentWeaponStat(p_WT, p_WLAT, weaponInfo => weaponInfo.Weapon_Duration);
    }
    #endregion

    // 
    public void CheckSynergyEffects(WeaponKeyword keyword)
    {
        if (keyword == WeaponKeyword.NONE) return;

        int keywordCount = 0;
        foreach (Weapon weapon in All_Weapon)
        {
            if (weapon.WeaponKeyword == keyword)
            {
                keywordCount++;
            }
        }

        if (keywordCount >= 2)
        {
            ApplySynergyEffect(keyword);
        }
    }

    // �ó��� ȿ�� ���� �Լ�
    public void ApplySynergyEffect(WeaponKeyword keyword)
    {
        switch (keyword)
        {
            case WeaponKeyword.Bound:
                foreach (Weapon weapon in All_Weapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.Weapon_Speed *= 1.5f;
                        Debug.Log("바운드");
                    }
                }
                break;
            case WeaponKeyword.Phoenix:
                foreach (Weapon weapon in All_Weapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.Weapon_Speed *= 1.5f;
                    }
                }
                break;
            case WeaponKeyword.Paladin:
                foreach (Weapon weapon in All_Weapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.Weapon_Range *= 1.5f;
                    }
                }
                break;
            case WeaponKeyword.Sonic:
                foreach (Weapon weapon in All_Weapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.Weapon_CoolDown *= 0.5f;
                    }
                }
                break;
            case WeaponKeyword.Holy:
                foreach (Weapon weapon in All_Weapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.Weapon_Size *= 1.5f;
                    }
                }
                break;
                // �ٸ� Ű���忡 ���� �ó��� ȿ�� �߰� ����
        }
    }

    // �÷��̾� �̵�
    public void Player_Move()
    {
        if (isDead == true) return;

        InputVec.x = Input.GetAxisRaw("Horizontal");
        InputVec.y = Input.GetAxisRaw("Vertical");

        if (InputVec.x != 0 || InputVec.y != 0)
        {
            AM.SetBool("Move", true);
        }
        else
        {
            AM.SetBool("Move", false);
        }

        Vector3 Next_Vec = InputVec.normalized * (this.Origin_Stat.Unit_Speed * Time.deltaTime);

        this.transform.position = this.transform.position + Next_Vec;

        if (InputVec.x != 0)
        {
            SR.flipX = InputVec.x >= 0;
        }
    }


    // ü�� ȸ�� �Լ�
    public void Recovery_Current_HP()
    {
        int Recovery_HP = 25;

        if (this.Current_HP < this.Max_HP)
        {
            this.Current_HP += Recovery_HP;
        }

        else
        {
            Debug.Log("�̹� �ִ� ü�� �Դϴ�.");
        }

        All_Manager.instance.UI_M.Update_Player_HPBar_Image(Current_HP / Max_HP);
    }

    // �� ȹ�� �Լ�
    public void Get_Gem(float p_Get_Gem_Current_Count)
    {
        All_Manager.instance.GameMode_M.Add_Gem(p_Get_Gem_Current_Count);
    }

    // ���� �÷��̾� ���� ����
    public override void Unit_Stat_Setting(Unit p_Unit, STO_Unit p_Un_STO)
    {
        if (p_Unit == null)
        {
            Debug.Log("unit이 null임");
        }
        if (p_Un_STO == null)
        {
            Debug.Log("unit STO가 null임");
        }
        p_Unit.SR.sprite = p_Un_STO.Sprite_Image;
        p_Unit.SR.material = p_Un_STO.Elite_Material;

        p_Unit.Origin_Stat.Unit_HP = p_Un_STO.Player_info_s.HP;
        p_Unit.Max_HP = p_Unit.Current_HP = p_Un_STO.Player_info_s.HP;
        p_Unit.Origin_Stat.Unit_Speed = (p_Un_STO.Player_info_s.Unit_Speed * 1.5f);
        p_Unit.Origin_Stat.Unit_Defense = p_Un_STO.Player_info_s.Unit_Defense;
        p_Unit.Origin_Stat.Unit_Get_Gem_Range = p_Un_STO.Player_info_s.Unit_Get_Gem_Range;

        p_Unit.BC.size = new Vector2(p_Un_STO.Size_X, p_Un_STO.Size_Y);
        p_Unit.BC.offset = new Vector2(p_Un_STO.Offset_X, p_Un_STO.Offset_Y);

        if (p_Un_STO.Unit_Animator_Controller != null)
        {
            p_Unit.AM.runtimeAnimatorController = p_Un_STO.Unit_Animator_Controller;
            p_Unit.AM.speed = 1.0f; // �ִϸ��̼� ���
            p_Unit.AM.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }

    public override void Unit_Animation(Unit_Anim_State State)
    {
        base.Unit_Animation(State);
    }

}

#region
/*
         var Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_Weapon.WT);

        var Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

        switch (p_Weapon.WeaponKeywords)
        {
            case WeaponKeyword.NONE:
                break;
            case WeaponKeyword.Bound:
                {
                    switch (p_Weapon.WT)
                    {
                        case Weapon_Type.Axe:
                            {
                                if (Axe_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                        case Weapon_Type.RuneTracer:
                            {
                                if (RuneTracer_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Phoenix:
                {
                    switch (p_Weapon.WT)
                    {
                        case Weapon_Type.Peachone:
                            {
                                if (Peachone_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                        case Weapon_Type.FireWand:
                            {
                                if (FireWand_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Paladin:
                {
                    switch (p_Weapon.WT)
                    {
                        case Weapon_Type.ShadowPinion:
                            {
                                if (ShadowPinion_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                        case Weapon_Type.Cross:
                            {
                                if (Cross_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Sonic:
                switch (p_Weapon.WT)
                {
                    case Weapon_Type.LightningRing:
                        {
                            if (LightningRing_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                    case Weapon_Type.Whip:
                        {
                            if (Whip_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                }
                break;
            case WeaponKeyword.Holy:
                switch (p_Weapon.WT)
                {
                    case Weapon_Type.Garlic:
                        {
                            if (Garlic_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                    case Weapon_Type.KingBible:
                        {
                            if (KingBible_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                }
                break;
        }
 */
//if (Dis > Vector3.Distance(this.transform.position, Gem_Temp.transform.position))
//{
//    Dis = Vector3.Distance(this.transform.position, Gem_Temp.transform.position);
//    Target_Gem = Gem_Temp;
//}
//if (Gem_Temp == null) return;

//var Temp_Equipment = 

//if (p_WT == Weapon_Type.Axe)
//{
//    Axe_LAT.Level++;

//    for (int i = 0; i < All_Weapon.Count; i++)
//    {
//        if (All_Weapon[i].GetComponent<Weapon_Axe>())
//        {
//            All_Weapon[i].Upgrade(Axe_LAT.Level);
//        }
//    }

//}
//if (InputVec.x <= 0)
//{
//    Player_Right = false;
//}

//if (Player_Right == true)
//{
//    The_Scale.x *= -1;
//}

//Vector3 The_Scale = this.transform.localScale;
//The_Scale.x *= -1;
//this.transform.localScale = The_Scale;
//if (Target_Gem == null) return;

//if (Target_Gem != null)
//{
//    Debug.Log("Gem Not Null");

//    //���� �ٲٰ�
//    Vector2 Dir = (this.transform.position - Target_Gem.transform.position).normalized;

//    //float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
//    //Target_Gem.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

//    //Target_Gem.transform.position = Target_Gem.transform.position + Target_Gem.transform.right * (5.0f * Time.deltaTime);
//    var Gem_Dir = Dir * 0.1f;

//    Target_Gem.Gem_To_Player(Gem_Dir);
//}
//else
//{
//    Debug.Log("Gem Null");
//}

// �� ������ ���ؼ� ������

//if (All_Manager.instance != null && All_Manager.instance.GameMode_M != null && All_Manager.instance.GameMode_M.Gem_List != null && All_Manager.instance.GameMode_M.Gem_List.Count > 0)
//{
//    int Count_To_Gem_Spawn = All_Manager.instance.GameMode_M.Enemy_List.Count;

//    List<int> Spawned_Gem_Indices = new List<int>();

//    for (int i = 0; i < Count_To_Gem_Spawn; i++)
//    {

//        if (Spawned_Gem_Indices.Contains(i))
//        {

//        }
//        else
//        {
//            continue;
//        }

//        Spawned_Gem_Indices.Add(i);

//        var Check_Null_Gem = All_Manager.instance.GameMode_M.Gem_List[i];

//        if (Check_Null_Gem == null)
//        {
//            continue;
//        }

//        var Gem_trasform = All_Manager.instance.GameMode_M.Gem_List[i].transform;

//        if (Gem_trasform != null)
//        {
//            Debug.Log("Gem Not Null");
//            Vector2 Dir = this.transform.position - Gem_trasform.position;
//            float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
//            Gem_trasform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
//        }
//        else
//        {
//            Debug.Log("Gem NULL");
//        }

//        Gem_trasform.position = Gem_trasform.position + Gem_trasform.right * (5.0f * Time.deltaTime);

//      }


//}

//int Create_KingBible_Count = 0;

//for (int i = 0; i < All_Weapon.Count; i++)
//{
//    if (All_Weapon[i] == GetComponent<Weapon_KingBible>())
//    {
//        if (All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count < Current_Weapon_Count(Weapon_Type.KingBible, KingBible_LAT))
//        {
//            All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count++;
//        }

//    }
//}


// ���⼭ All_Weapon�� �ִ� ��� ������ �ٽ� ����
// All_Weapon.GetComponent<KingBible>().Max = 3;
// All_Weapon.GetComponent<KingBible>().Current = 3;
//
// 1 2 3

//var KingBibles = this.GetComponentsInChildren<Weapon_KingBible>();
//KingBibles.Length

//int Player_Weapon_KingBible_Count = 0;

//for (int i = 0; i < All_Weapon.Count; i++)
//{
//    if (All_Weapon[i].GetComponent<Weapon_KingBible>())
//    {
//        All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count++;

//        if (All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count > All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count)
//        {
//            All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count++;
//        }
//    }

//}

//Debug.Log("Player_Weapon_KingBible_Count : " + Player_Weapon_KingBible_Count);


//float xInput = Input.GetAxis("Horizontal");
//float yInput = Input.GetAxis("Vertical");

//float xSpeed = xInput * Xspeed * Time.deltaTime;
//float ySpeed = yInput * Yspeed * Time.deltaTime;

//�� �����ΰ���
// this.transform.position = this.transform.position + (this.transform.up * ySpeed);

//����
// this.transform.Rotate(new Vector3(0, 0, -xSpeed));

//if (Skill_Full < Skill_Cool)
//{

//    Skill_Full += Time.deltaTime;
//    All_Manager.instance.UI_M.Update_Player_Skill_Image(Skill_Full / Skill_Cool);
//    Temp_Go = Instantiate(Garlic_Go, null);
//    Temp_Go.transform.parent = this.transform;
//    if (Temp_Go.GetComponent<Garlic>())
//    {
//        yield return new WaitUntil(() => Temp_Go.GetComponent<Garlic>().SR);
//        All_Manager.instance.Resource_M.Weapon_Stat(Temp_Go.GetComponent<Garlic>(), All_Manager.instance.Resource_M.STO_Garlic);
//    }
//}
//else
//{
//    Skill_Full = 0.0f;
//}

//switch (p_WT)
//{
//    case Weapon_Type.None:
//        {
//        }
//        break;
//    case Weapon_Type.KingBible:
//        {
//            //Temp_Go = Instantiate(Weapon_Go, null);
//            GameObject Weapon_Go = new GameObject("KingBible");
//            Weapon_Go.AddComponent<KingBible>();
//            Weapon_Go.transform.parent = this.transform;
//            if (Weapon_Go.GetComponent<KingBible>())
//            {
//                yield return new WaitUntil(() => Weapon_Go.GetComponent<KingBible>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(Weapon_Go.GetComponent<KingBible>(), All_Manager.instance.Resource_M.STO_KingBible);
//            }
//            if (Weapon_Go.GetComponent<Weapon>())
//            {
//                Weapon_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.SpellBook:
//        {
//            //Temp_Go = Instantiate(SpellBook_Go, null);
//            GameObject SpellBook_Go = new GameObject("SpellBook");
//            SpellBook_Go.AddComponent<SpellBook>();
//            SpellBook_Go.transform.parent = this.transform;
//            if (SpellBook_Go.GetComponent<SpellBook>())
//            {
//                yield return new WaitUntil(() => SpellBook_Go.GetComponent<SpellBook>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(SpellBook_Go.GetComponent<SpellBook>(), All_Manager.instance.Resource_M.STO_SpellBook);
//            }
//            if (SpellBook_Go.GetComponent<Weapon>())
//            {
//                SpellBook_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.FireWand_Launcher:
//        {
//            //Temp_Go = Instantiate(FireWand_Launcher_Go, null);
//            GameObject FireWand_Launcher_Go = new GameObject("FireWand_Launcher");
//            FireWand_Launcher_Go.AddComponent<FireWand_Launcher>();
//            FireWand_Launcher_Go.transform.parent = this.transform;
//            if (FireWand_Launcher_Go.GetComponent<FireWand_Launcher>())
//            {
//                yield return new WaitUntil(() => FireWand_Launcher_Go.GetComponent<FireWand_Launcher>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(FireWand_Launcher_Go.GetComponent<FireWand_Launcher>(), All_Manager.instance.Resource_M.STO_FireWand_Launcher);
//            }
//            if (FireWand_Launcher_Go.GetComponent<Weapon>())
//            {
//                FireWand_Launcher_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.ShadowPinion_Launcher:
//        {
//            //Temp_Go = Instantiate(ShadowPinion_Launcher_Go, null);
//            GameObject ShadowPinion_Launcher_Go = new GameObject("ShadowPinion_Launcher");
//            ShadowPinion_Launcher_Go.AddComponent<ShadowPinion_Launcher>();
//            ShadowPinion_Launcher_Go.transform.parent = this.transform;
//            if (ShadowPinion_Launcher_Go.GetComponent<ShadowPinion_Launcher>())
//            {
//                yield return new WaitUntil(() => ShadowPinion_Launcher_Go.GetComponent<ShadowPinion_Launcher>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(ShadowPinion_Launcher_Go.GetComponent<ShadowPinion_Launcher>(), All_Manager.instance.Resource_M.STO_ShadowPinion_Launcher);
//            }
//            if (ShadowPinion_Launcher_Go.GetComponent<Weapon>())
//            {
//                ShadowPinion_Launcher_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.Garlic:
//        {
//            //Temp_Go = Instantiate(Garlic_Go, null);
//            GameObject Garlic_Go = new GameObject("Garlic");
//            Garlic_Go.AddComponent<Garlic>();
//            Garlic_Go.transform.parent = this.transform;
//            if (Garlic_Go.GetComponent<Garlic>())
//            {
//                yield return new WaitUntil(() => Garlic_Go.GetComponent<Garlic>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(Garlic_Go.GetComponent<Garlic>(), All_Manager.instance.Resource_M.STO_Garlic);
//            }
//            if (Garlic_Go.GetComponent<Weapon>())
//            {
//                Garlic_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    default:
//        break;
//}

//public IEnumerator C_Create_Weapon(Weapon_Type p_PT)
//{
//    yield return new WaitWhile(() => Creating_Weapon == true);

//    Creating_Weapon = true;

//    GameObject Weapon_Go = null;

//    switch (p_PT)
//    {
//        case Weapon_Type.None:
//            break;
//        case Weapon_Type.KingBible:
//            {
//                Weapon_Go = new GameObject("KingBible");

//                Weapon_Go.AddComponent<Weapon_KingBible>();

//                Weapon_Go.transform.parent = this.transform;
//                if (Weapon_Go.GetComponent<Weapon_KingBible>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_KingBible>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_KingBible, KingBible_LAT.Level);

//                    int Create_KingBible_Count = 1;

//                    for (int i = 0; i < All_Weapon.Count; i++)
//                    {
//                        if (All_Weapon[i].GetComponent<Weapon_KingBible>())
//                        {
//                            Create_KingBible_Count++;
//                        }
//                    }

//                    Weapon_Go.GetComponent<Weapon_KingBible>().Weapon_KingBible_Count = Create_KingBible_Count;
//                }
//            }
//            break;
//        case Weapon_Type.Garlic:
//            {
//                Weapon_Go = new GameObject("Garlic");

//                Weapon_Go.AddComponent<Weapon_Garlic>();

//                if (Weapon_Go.GetComponent<Weapon_Garlic>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Garlic>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_Garlic, Garlic_LAT.Level);
//                }
//            }
//            break;
//        case Weapon_Type.LightningRing:
//            {
//                if (All_Manager.instance.GameMode_M.Enemy_List.Count > 10)
//                {

//                    for (int i = 0; i < 10; i++)
//                    {
//                        GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//                        Weapon_Go = new GameObject("LightningRing");
//                        Weapon_Go.AddComponent<Weapon_LightningRing>();

//                        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//                        Weapon_Go.transform.rotation = Quaternion.identity;

//                        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//                        {
//                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                        }

//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < All_Manager.instance.GameMode_M.Enemy_List.Count; i++)
//                    {
//                        GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//                        Weapon_Go = new GameObject("LightningRing");
//                        Weapon_Go.AddComponent<Weapon_LightningRing>();

//                        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//                        Weapon_Go.transform.rotation = Quaternion.identity;

//                        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//                        {
//                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                        }

//                        Destroy(Weapon_Go, 2.5f);
//                    }
//                }

//            }
//            break;
//        case Weapon_Type.FireWand:
//            {
//                Weapon_Go = new GameObject("FireWand");

//                Weapon_Go.AddComponent<Weapon_FireWand>();

//                if (Weapon_Go.GetComponent<Weapon_FireWand>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_FireWand>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_FireWand, FireWand_LAT.Level);
//                }
//            }
//            break;
//        case Weapon_Type.ShadowPinion:
//            {
//                Weapon_Go = new GameObject("ShadowPinion");

//                Weapon_Go.AddComponent<Weapon_ShadowPinion>();

//                if (Weapon_Go.GetComponent<Weapon_ShadowPinion>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_ShadowPinion>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_ShadowPinion, ShadowPinion_LAT.Level);
//                }
//            }
//            break;
//        case Weapon_Type.Axe:
//            {
//                Weapon_Go = new GameObject("Axe");

//                Weapon_Go.AddComponent<Axe>();



//                if (Weapon_Go.GetComponent<Axe>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Axe>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_Axe, Axe_LAT.Level);
//                    Vector3 One = this.transform.position;
//                    Vector3 Two = One + new Vector3(0, 5, 0); // ��
//                    Vector3 Three = Two + new Vector3(5, 0, 0); //������
//                    Vector3 Four = One + new Vector3(5, -10, 0); // ������ �Ʒ�


//                    Weapon_Go.GetComponent<Axe>().Axe_Init(One, Two, Three, Four);
//                }

//            }
//            break;
//        default:
//            break;
//    }

//    All_Weapon.Add(Weapon_Go.GetComponent<Weapon>());

//    Creating_Weapon = false;
//}

//if (All_Manager.instance.GameMode_M.Enemy_List.Count > 10)
//{

//    for (int i = 0; i < 10; i++)
//    {
//        //GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//        Weapon_Go = new GameObject("LightningRing");
//        Weapon_Go.AddComponent<Weapon_LightningRing>();

//        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//        Weapon_Go.transform.rotation = Quaternion.identity;
//        Weapon_Go.transform.localScale = Vector3.one;

//        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//        {
//            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//        }

//    }
//}
//else
//{
//    for (int i = 0; i < All_Manager.instance.GameMode_M.Enemy_List.Count; i++)
//    {
//        //GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//        Weapon_Go = new GameObject("LightningRing");
//        Weapon_Go.AddComponent<Weapon_LightningRing>();

//        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//        Weapon_Go.transform.rotation = Quaternion.identity;
//        Weapon_Go.transform.localScale = Vector3.one;

//        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//        {
//            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//        }

//        Destroy(Weapon_Go, 2.5f);
//    }
//}

//public void Get_Equipment(Equipment_Type p_ET)
//{
//    StartCoroutine(C_Get_Equipment(p_ET));
//}

//public IEnumerator C_Get_Equipment(Equipment_Type p_ET)
//{
//    // ���� Temp�� ���̿��� �߻��ϴ� �����ΰ����� ���δ�.
//    // �ϴ� �� ���� ����
//    // ���� �ϴ� �ٽ� �ѹ�
//    yield return new WaitWhile(() => Creating_Equipment == true);

//    var Temp = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment(p_ET);

//    Creating_Equipment = true;

//    if (p_ET == Equipment_Type.None)
//    {

//    }

//    if (p_ET == Equipment_Type.HollowHeart)
//    {
//        yield return new WaitUntil(() => (Temp = All_Manager.instance.Resource_M.STO_Equipment_HollowHeart));
//        if (Temp == All_Manager.instance.Resource_M.STO_Equipment_HollowHeart)
//        {
//            HollowHeart_Level++;

//            Get_Equipment_Player_Percentage_Stat(this, Temp);
//            Debug.Log(Temp);
//            Debug.Log("��ű� ����");
//        }
//    }

//    if (p_ET == Equipment_Type.Wings)
//    {
//        Wings_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Armour)
//    {
//        Armour_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Spinach)
//    {
//        Spinach_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Bracer)
//    {
//        Bracer_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.SpellBinder)
//    {
//        SpellBinder_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Candelabrador)
//    {
//        Candelabrador_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.EmptyTome)
//    {
//        EmptyTome_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Duplicator)
//    {
//        Duplicator_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Attractorb)
//    {
//        Attractorb_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (Temp != null)
//    {
//        All_Equipment.Add(Temp);
//    }

//    Creating_Equipment = false;
//}
#endregion
