using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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
    //None,               // = "없음"
    Axe,                  // 포물선을 그리며 떨어지는 도끼
    KingBible,            // 성경
    Peachone,             // 뱀서의 비둘기
    FireWand,             // 화염구 뿌리개
    Cross,                // 부메랑
    Garlic,               // 일정 쿨타임후 주변 밀어냄
    LightningRing,        // 플레이어 주변으로 떨어지는 번개
    ShadowPinion,         // 유도함정
    RuneTracer,           // 핑퐁처럼 화면에 닿을시 튕기는 룬 트레이서
    Whip                  // 플레이어 양쪽으로 번갈아가며 공격하는 채찍
}

[System.Serializable]
public enum Equipment_Type
{
    //None,                 // = "없음"
    Spinach,              // 레벨당 공격력 10%증가 (최대레벨 : 5)
    Bracer,               // 레벨당 투사체 속도 10%증가 (최대레벨 : 5)
    Duplicator,           // 레벨당 투사체 1개씩 증가 (최대레벨 : 5)
    EmptyTome,            // 레벨당 무기 쿨타임 8%씩 감소 (최대레벨 : 5)
    Wings,                // 레벨당 이동속도 10%증가 (최대레벨 : 5)
    Candelabrador,        // 레벨당 공격범위 10%증가   (최대레벨 : 5)
    Armour,               // 레벨당 방어력 5%증가 (최대레벨 : 5)
    SpellBinder,          // 레벨당 지속시간 10%증가 (최대레벨 5)
    Attractorb,           // 레벨당 아이템 획득 범위 30% (최대레벨 : 5)
    HollowHeart           // 레벨당 체력 20%증가 (최대레벨 : 5)
}

[System.Serializable]
public class Weapon_Level_And_Timer
{
    public int Level = 0;
    public float Timer = 0;
}

public class Unit_Player : Unit
{
    //찾아보면서 써보기
    public List<Weapon> All_Weapon = new List<Weapon>();
    public List<STO_Equipment> All_Equipment = new List<STO_Equipment>();

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

    public bool Creating_Weapon = false;
    public bool Creating_Equipment = false;
    public bool Weapon_Trigger = false;

    public bool Equipment_Level_Up = false;

    //public float Ghost_Spawn_Interval = 0.1f;
    //private float Ghost_Spawn_Timer = 0f;

    public override void Start()
    {
        base.Start();
        /*
        Upgrade_Weapon(Weapon_Type.Axe);              // 완성
        Upgrade_Weapon(Weapon_Type.KingBible);            // 완성 
        Upgrade_Weapon(Weapon_Type.Peachone);             // 완성
        Upgrade_Weapon(Weapon_Type.FireWand);           // 완성
        Upgrade_Weapon(Weapon_Type.Cross);            // 완성
        Upgrade_Weapon(Weapon_Type.Garlic);            // 완성
        Upgrade_Weapon(Weapon_Type.LightningRing);        // 완성
        Upgrade_Weapon(Weapon_Type.ShadowPinion);          // 완성
        Upgrade_Weapon(Weapon_Type.RuneTracer);       // 완성
        Upgrade_Weapon(Weapon_Type.Whip);             // 완성
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

        else
        {

        }
    }

    //public void Ghost_Effect_Cool_Time()
    //{
    //    Ghost_Spawn_Timer += Time.deltaTime;
    //    if (Ghost_Spawn_Timer >= Ghost_Spawn_Interval)
    //    {
    //        Create_Ghost_Effect();
    //        Ghost_Spawn_Timer = 0f;
    //    }
    //}

    public override void Hitted(float p_Damage_Value)
    {
        base.Hitted(p_Damage_Value);

        All_Manager.instance.UI_M.Update_Player_HPBar_Image(Current_HP / Max_HP);
    }

    public override void Die()
    {
        base.Die();

        All_Manager.instance.GameMode_M.GM_State = GameMode_State.GameOver;
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

    public void Create_Ghost_Effect()
    {
        //if (InputVec.x != 0 || InputVec.y != 0)
        //{
        //    // 현재 캐릭터의 스프라이트를 사용하여 잔상 생성
        //    SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //    All_Manager.instance.Resource_M.Ghost_Effect.Spawn_Ghost(sr.sprite, transform.position);
        //}
       
    }

    public void Create_Weapon(Weapon_Type p_WT)
    {
        StartCoroutine(C_Create_Weapon(p_WT));
    }

    #region
    // 무기 생성
    public IEnumerator C_Create_Weapon(Weapon_Type p_WT)
    {
        yield return new WaitWhile(() => Creating_Weapon == true);

        var Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        var Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

        Creating_Weapon = true;

        //GameObject Weapon_Go = null;
        GameObject Weapon_Go = null;

        switch (p_WT)
        {
            // 도끼
            case Weapon_Type.Axe:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);

                    //Weapon_Go.AddComponent<Weapon_Axe>();
                    
                    // Weapon_Go가 해당 무기의 컴포넌트를 포함하고 있다면.
                    if (Weapon_Go.GetComponent<Weapon_Axe>())
                    {
                        // 위치 초기화
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        // 이 무기의 스프라이트를 불러올때까지 기다려라.
                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Axe>().SR);
                        Vector3 One = this.transform.position;
                        Vector3 Two, Three, Four;

                        // 도끼 방향에 따라서
                        if (Weapon_Trigger)
                        {
                            Two = One + new Vector3(0, 5, 0);
                            Three = Two + new Vector3(Random.Range(1, 5), 0, 0);        // 오른쪽
                            Four = One + new Vector3(Random.Range(1, 5), -10, 0);       // 오른쪽 아래
                        }
                        else
                        {
                            Two = One + new Vector3(0, 5, 0);
                            Three = Two + new Vector3(Random.Range(-5, -1), 0, 0);      // 왼쪽
                            Four = One + new Vector3(Random.Range(-5, -1), -10, 0);     // 왼쪽 아래
                        }

                        // 도끼 초기화
                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Axe_LAT.Level);
                        Weapon_Go.GetComponent<Weapon_Axe>().Axe_Init(One, Two, Three, Four);

                        // 다음에 생성될때 방향 반대로 변경
                        Weapon_Trigger = !Weapon_Trigger;

                        // 무기 생성시 효과음
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Axe);
                    }
                }
                break;
            case Weapon_Type.KingBible:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);

                    //Weapon_Go.AddComponent<Weapon_KingBible>();

                    if (Weapon_Go.GetComponent<Weapon_KingBible>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        //Start()
                        //Update()

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_KingBible>().SR);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, KingBible_LAT.Level);

                        // 지금 현재 가지고 있는 성경의 갯수
                        int Max = 0;

                        // 플레이어가 사용하고 있는 성경의 갯수
                        for (int i = 0; i < All_Weapon.Count; i++)
                        {
                            if (All_Weapon[i].GetComponent<Weapon_KingBible>())
                            {
                                Max++;
                            }
                        }

                        // 
                        Max++;

                        //
                        for (int i = 0; i < All_Weapon.Count; i++)
                        {
                            if (All_Weapon[i].GetComponent<Weapon_KingBible>())
                            {
                                All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count = Max;
                            }
                        }

                        Weapon_Go.GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count = Max;
                        Weapon_Go.GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count = Max;

                        //var Temp_KingBible_Range = Current_Weapon_Range(Weapon_Type.KingBible, KingBible_LAT);

                        //Weapon_Go.GetComponent<Weapon_KingBible>().Weapon_Range = Temp_KingBible_Range;

                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.KingBible);
                    }
                }
                break;
            case Weapon_Type.Peachone:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);

                    //Weapon_Go.AddComponent<Weapon_Peachone>();

                    if (Weapon_Go.GetComponent<Weapon_Peachone>())
                    {
                        Weapon_Go.transform.position = All_Manager.instance.UI_M.Peachone_Image.rectTransform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Peachone>().SR);

                        var P_Peachone_UI_Range = Weapon_Go.GetComponent<Weapon_Peachone>().Convert_RectTransform_To_Position(All_Manager.instance.UI_M.Peachone_Range_Image.rectTransform);
                        var P_Peachone_UI = Weapon_Go.GetComponent<Weapon_Peachone>().Convert_RectTransform_To_Position(All_Manager.instance.UI_M.Peachone_Image.rectTransform);

                        Vector3 One = All_Manager.instance.UI_M.Peachone_Image.transform.position;
                        Vector3 Two = Weapon_Go.GetComponent<Weapon_Peachone>().Peachone_Point_Setting(All_Manager.instance.UI_M.Peachone_Image.transform.position);
                        Vector3 Three = Weapon_Go.GetComponent<Weapon_Peachone>().Peachone_Point_Setting(P_Peachone_UI_Range);
                        Vector3 Four = P_Peachone_UI_Range;

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Peachone_LAT.Level);
                        Weapon_Go.GetComponent<Weapon_Peachone>().Peachone_Init(One, Two, Three, Four);
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Peachone);
                    }

                }
                break;
            case Weapon_Type.FireWand:
                {
                    var FireWand_Stat = Weapon_STO.Info_s[FireWand_LAT.Level];

                    for (int i = 0; i < Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT); i++)
                    {
                        Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);

                        //Weapon_Go.AddComponent<Weapon_FireWand>();

                        if (Weapon_Go.GetComponent<Weapon_FireWand>())
                        {
                            Weapon_Go.transform.position = this.transform.position + new Vector3(0, -0.3f, 0);
                            Weapon_Go.transform.rotation = Quaternion.identity;
                            Weapon_Go.transform.localScale = Vector3.one;

                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_FireWand>().SR);

                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, FireWand_LAT.Level);

                            Vector3 Dir_Vec = new Vector3(Mathf.Cos((Mathf.PI * 2 * i / Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT))), Mathf.Sin((Mathf.PI * i * 2 / Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT)))); //원을 기준으로 각각의 투사체마다 벡터를 정리

                            if (All_Manager.instance.GameMode_M.Game_Stop == false)
                            {
                                Weapon_Go.transform.position = Weapon_Go.transform.position + (Dir_Vec.normalized * FireWand_Stat.Weapon_Speed * Time.deltaTime);
                            }
                            else { }

                            Weapon_Go.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (Current_Weapon_Count(Weapon_Type.FireWand, FireWand_LAT))) + 180); //투사체의 각도가 자연스럽게 회전값을 넣었다.
                            All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.FireWand);
                        }
                    }
                }
                break;
            case Weapon_Type.Cross:
                {
                    var W_Cross_S = Weapon_STO.Info_s[Cross_LAT.Level];

                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);

                    //Weapon_Go.AddComponent<Weapon_Cross>();

                    if (Weapon_Go.GetComponent<Weapon_Cross>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Cross>().SR);

                        Vector3 One = this.transform.position;
                        Vector3 Target = One + new Vector3(Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range), Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range), 0);

                        Vector3 Two = One + (Target - One) / 3 + new Vector3(Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, 0);
                        Vector3 Three = Target + (One - Target) / 3 + new Vector3(Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, Random.Range(-W_Cross_S.Weapon_Range, W_Cross_S.Weapon_Range) / 2, 0);
                        Vector3 Four = One + ((One - Target) * W_Cross_S.Weapon_Range);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Cross_LAT.Level);
                        Weapon_Go.GetComponent<Weapon_Cross>().Cross_Init(One, Two, Three, Four);
                        //Weapon_Go.GetComponent<Weapon_Cross>().Weapon_Application_Equipment();
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Cross);
                    }
                }
                break;
            case Weapon_Type.Garlic:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);
                    //Weapon_Go.AddComponent<Weapon_Garlic>();

                    if (Weapon_Go.GetComponent<Weapon_Garlic>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        Weapon_Go.transform.parent = this.transform;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Garlic>().SR);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Garlic_LAT.Level);

                        //Weapon_Go.GetComponent<Weapon_Garlic>().Weapon_Application_Equipment();
                        //All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Garlic);
                    }
                }
                break;
            case Weapon_Type.LightningRing:
                {
                    if (All_Manager.instance != null && All_Manager.instance.GameMode_M != null && All_Manager.instance.GameMode_M.Enemy_List != null && All_Manager.instance.GameMode_M.Enemy_List.Count > 0)
                    {
                        var Max_Count = Current_Weapon_Count(Weapon_Type.LightningRing, LightningRing_LAT);
                        int Count_To_Spawn = Mathf.Min(Max_Count, All_Manager.instance.GameMode_M.Enemy_List.Count); // 최대 10개까지만 생성하도록 제한

                        List<int> Spawned_Indices = new List<int>(); // 이미 생성된 적의 인덱스를 저장할 리스트

                        for (int i = 0; i < Count_To_Spawn; i++)
                        {
                            int Random_Index = Random.Range(0, All_Manager.instance.GameMode_M.Enemy_List.Count);

                            // 중첩 방지: 이미 생성된 적 위치에 LightningRing이 있으면 건너뜀
                            if (Spawned_Indices.Contains(Random_Index))
                            {
                                continue;
                            }

                            Spawned_Indices.Add(Random_Index);

                            var Check_Enemy_Alive = All_Manager.instance.GameMode_M.Enemy_List[Random_Index];

                            if (Check_Enemy_Alive == null)
                            {
                                //Debug.LogWarning("None Enemy! : " + Random_Index);
                                continue;
                            }

                            Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);
                            //Weapon_Go.AddComponent<Weapon_LightningRing>();


                            // LightningRing이 데미지를 주는지 확인 후 처리
                            if (Weapon_Go.GetComponent<Weapon_LightningRing>() != null)
                            {
                                Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[Random_Index].transform.position;
                                Weapon_Go.transform.rotation = Quaternion.identity;
                                Weapon_Go.transform.localScale = Vector3.one;

                                yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);

                                // Weapon_Init을 통해 필요한 초기화 작업 수행
                                Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, LightningRing_LAT.Level);
                                //Weapon_Go.GetComponent<Weapon_LightningRing>().Weapon_Application_Equipment();
                                All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.LightningRing);
                            }
                            else
                            {
                                //Debug.Log("LightningRing Gone!");
                            }

                        }
                    }



                }
                break;
            case Weapon_Type.ShadowPinion:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);
                    //Weapon_Go.AddComponent<Weapon_ShadowPinion>();

                    if (Weapon_Go.GetComponent<Weapon_ShadowPinion>())
                    {
                        Weapon_Go.transform.position = this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_ShadowPinion>().SR);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, ShadowPinion_LAT.Level);
                        //Weapon_Go.GetComponent<Weapon_ShadowPinion>().Weapon_Application_Equipment();
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.ShadowPinion);
                    }
                }
                break;
            case Weapon_Type.RuneTracer:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);
                    //Weapon_Go.AddComponent<Weapon_RuneTracer>();

                    if (Weapon_Go.GetComponent<Weapon_RuneTracer>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_RuneTracer>().SR);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, RuneTracer_LAT.Level);
                        //Weapon_Go.GetComponent<Weapon_RuneTracer>().Weapon_Application_Equipment();
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.RuneTracer);
                    }
                }
                break;
            case Weapon_Type.Whip:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_WT);
                    //Weapon_Go.AddComponent<Weapon_Whip>();

                    //Weapon_Go.transform.parent = this.transform;

                    if (Weapon_Go.GetComponent<Weapon_Whip>())
                    {
                        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Whip>().SR);

                        if (Weapon_Trigger)
                        {
                            Weapon_Go.transform.position = this.transform.position + new Vector3(-2f + Random.Range(-0.5f, 0.5f), -0.1f + Random.Range(-0.2f, 0.2f), 0);
                            Weapon_Go.transform.rotation = Quaternion.identity;
                            Weapon_Go.transform.localScale = new Vector3(-4, -1, -1);
                        }
                        else
                        {
                            Weapon_Go.transform.position = this.transform.position + new Vector3(2f + Random.Range(-0.5f, 0.5f), -0.1f + Random.Range(-0.2f, 0.2f), 0);
                            Weapon_Go.transform.rotation = Quaternion.identity;
                            Weapon_Go.transform.localScale = new Vector3(4, 1, 1);
                        }


                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Whip_LAT.Level);
                        Weapon_Trigger = !Weapon_Trigger;

                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(Weapon_Type.Whip);
                    }

                }
                break;
            default:
                break;
        }

        //Debug.Log(Weapon_Go);

        if (Weapon_Go != null)
        {
            All_Weapon.Add(Weapon_Go.GetComponent<Weapon>());
            //Weapon_Go.transform.parent = this.transform;
        }

        else
        {
            //Debug.Log("Weapon_Go Null");
        }

        Creating_Weapon = false;
    }

    // 무기 업그레이드
    public void Upgrade_Weapon(Weapon_Type p_WT)
    {
        if (p_WT == Weapon_Type.Axe)
        {
            Axe_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_Axe>())
                {
                    All_Weapon[i].Upgrade(Axe_LAT.Level);
                }
            }

            //Debug.Log(Axe_LAT.Level);
        }

        else if (p_WT == Weapon_Type.KingBible)
        {
            KingBible_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_KingBible>())
                {
                    All_Weapon[i].Upgrade(KingBible_LAT.Level);
                }
            }

            Create_Weapon(Weapon_Type.KingBible);
        }

        else if (p_WT == Weapon_Type.Peachone)
        {
            Peachone_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_Peachone>())
                {
                    All_Weapon[i].Upgrade(Peachone_LAT.Level);
                }
            }
        }

        else if (p_WT == Weapon_Type.FireWand)
        {
            FireWand_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_FireWand>())
                {
                    All_Weapon[i].Upgrade(FireWand_LAT.Level);
                }
            }

        }

        else if (p_WT == Weapon_Type.Cross)
        {
            Cross_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_Cross>())
                {
                    All_Weapon[i].Upgrade(Cross_LAT.Level);
                }
            }

        }

        else if (p_WT == Weapon_Type.Garlic)
        {
            Garlic_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_Garlic>())
                {
                    All_Weapon[i].Upgrade(Garlic_LAT.Level);
                }
            }


        }

        else if (p_WT == Weapon_Type.LightningRing)
        {
            LightningRing_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_LightningRing>())
                {
                    All_Weapon[i].Upgrade(LightningRing_LAT.Level);
                }
            }

        }

        else if (p_WT == Weapon_Type.ShadowPinion)
        {
            ShadowPinion_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_ShadowPinion>())
                {
                    All_Weapon[i].Upgrade(ShadowPinion_LAT.Level);
                }
            }

        }

        else if (p_WT == Weapon_Type.RuneTracer)
        {
            RuneTracer_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_RuneTracer>())
                {
                    All_Weapon[i].Upgrade(RuneTracer_LAT.Level);
                }
            }

        }

        else if (p_WT == Weapon_Type.Whip)
        {
            Whip_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<Weapon_Whip>())
                {
                    All_Weapon[i].Upgrade(Whip_LAT.Level);
                }
            }

        }


    }

    // 가지고 있는지
    public bool Check_Weapon(Weapon_Type p_PT)
    {
        return false;
    }

    // 무기 스폰 타이머 업데이트
    public void Spawn_Weapon_Timer_Update()
    {
        if (Axe_LAT.Level > 0)
        {
            if (Axe_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.Axe, Axe_LAT))
            {
                Axe_LAT.Timer = 0;

                //for (int i = 0; i < STO_Weapon_Temp.Info_s[Axe_LAT.Level].Weapon_Count; i++)
                //{
                //    Create_Weapon(Weapon_Type.Axe);
                //}

                for (int i = 0; i < Current_Weapon_Count(Weapon_Type.Axe, Axe_LAT); i++)
                {
                    Create_Weapon(Weapon_Type.Axe);
                }
            }

            Axe_LAT.Timer += Time.deltaTime;
        }

        if (KingBible_LAT.Level > 0)
        {
            if (KingBible_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.KingBible, KingBible_LAT))
            {
                KingBible_LAT.Timer = 0;

                //for (int i = 0; i < Current_Weapon_Count(Weapon_Type.KingBible, KingBible_LAT); i++)    // 성경이 왜 갯수에 따라 각도로 안나눠지는 이유, 지금 이건 STO에 의해 성경을 회전 시킨다 즉 이걸 고쳐야 한다.
                //{
                //    Create_Weapon(Weapon_Type.KingBible);
                //}
            }

            KingBible_LAT.Timer += Time.deltaTime;
        }

        if (Peachone_LAT.Level > 0)
        {
            if (Peachone_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.Peachone, Peachone_LAT))
            {
                Peachone_LAT.Timer = 0;


                for (int i = 0; i < Current_Weapon_Count(Weapon_Type.Peachone, Peachone_LAT); i++)
                {
                    Create_Weapon(Weapon_Type.Peachone);
                }
            }

            Peachone_LAT.Timer += Time.deltaTime;
        }

        if (FireWand_LAT.Level > 0)
        {
            if (FireWand_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.FireWand, FireWand_LAT))
            {
                FireWand_LAT.Timer = 0;

                Create_Weapon(Weapon_Type.FireWand);
                //Debug.Log("FireWand!");
            }

            FireWand_LAT.Timer += Time.deltaTime;
        }

        if (Cross_LAT.Level > 0)
        {
            if (Cross_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.Cross, Cross_LAT))
            {
                Cross_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(Weapon_Type.Cross, Cross_LAT); i++)
                {
                    Create_Weapon(Weapon_Type.Cross);
                }
            }

            Cross_LAT.Timer += Time.deltaTime;
        }

        if (Garlic_LAT.Level > 0)
        {
            var STO_Weapon_Temp = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Garlic);

            if (Garlic_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.Garlic, Garlic_LAT))
            {
                Garlic_LAT.Timer = 0;

                Create_Weapon(Weapon_Type.Garlic);
            }

            Garlic_LAT.Timer += Time.deltaTime;
        }

        if (LightningRing_LAT.Level > 0)
        {
            if (LightningRing_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.LightningRing, LightningRing_LAT))
            {
                LightningRing_LAT.Timer = 0;

                Create_Weapon(Weapon_Type.LightningRing);
            }

            LightningRing_LAT.Timer += Time.deltaTime;
        }

        if (ShadowPinion_LAT.Level > 0)
        {
            if (ShadowPinion_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.ShadowPinion, ShadowPinion_LAT))
            {
                ShadowPinion_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(Weapon_Type.ShadowPinion, ShadowPinion_LAT); i++)
                {
                    Create_Weapon(Weapon_Type.ShadowPinion);
                }
            }

            ShadowPinion_LAT.Timer += Time.deltaTime;
        }

        if (RuneTracer_LAT.Level > 0)
        {
            if (RuneTracer_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.RuneTracer, RuneTracer_LAT))
            {
                RuneTracer_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(Weapon_Type.RuneTracer, RuneTracer_LAT); i++)
                {
                    Create_Weapon(Weapon_Type.RuneTracer);
                }
            }

            RuneTracer_LAT.Timer += Time.deltaTime;
        }

        if (Whip_LAT.Level > 0)
        {
            if (Whip_LAT.Timer > Current_Weapon_CoolDown(Weapon_Type.Whip, Whip_LAT))
            {
                Whip_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(Weapon_Type.Whip, Whip_LAT); i++)
                {
                    Create_Weapon(Weapon_Type.Whip);
                }
            }

            Whip_LAT.Timer += Time.deltaTime;
        }

        //Debug.Log("Weapon_Spawn!");
    }

    // 장신구 장착
    public void Update_Get_Equipment(Equipment_Type p_ET)
    {
        var Temp = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(p_ET);

        if (p_ET == Equipment_Type.HollowHeart)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.HollowHeart))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Wings)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Wings))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Armour)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Armour))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Spinach)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Spinach))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Bracer)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Bracer))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.SpellBinder)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.SpellBinder))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Candelabrador)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Candelabrador))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.EmptyTome)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.EmptyTome))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Duplicator)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Duplicator))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        else if (p_ET == Equipment_Type.Attractorb)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Attractorb))
            {
                Get_Equipment_Player_Percentage_Stat(this, Temp);
            }

            //Get_Equipment_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Equipment_HollowHeart);
        }

        //Debug.Log("장신구 장착");

        if (Temp != null)
        {
            All_Equipment.Add(Temp);
        }
    }

    // 장신구 업그레이드
    public void Upgrade_Equipment(Equipment_Type p_ET)
    {
        if (p_ET == Equipment_Type.HollowHeart)
        {
            HollowHeart_Level += 1;

            Update_Get_Equipment(Equipment_Type.HollowHeart);
        }

        else if (p_ET == Equipment_Type.Wings)
        {
            Wings_Level += 1;

            Update_Get_Equipment(Equipment_Type.Wings);
        }

        else if(p_ET == Equipment_Type.Armour)
        {
            Armour_Level += 1;

            Update_Get_Equipment(Equipment_Type.Armour);
        }

        else if(p_ET == Equipment_Type.Spinach)
        {
            Spinach_Level += 1;

            Update_Get_Equipment(Equipment_Type.Spinach);
        }

        else if(p_ET == Equipment_Type.Bracer)
        {
            Bracer_Level += 1;

            Update_Get_Equipment(Equipment_Type.Bracer);
        }

        else if(p_ET == Equipment_Type.SpellBinder)
        {
            SpellBinder_Level += 1;

            Update_Get_Equipment(Equipment_Type.SpellBinder);
        }

        else if(p_ET == Equipment_Type.Candelabrador)
        {
            Candelabrador_Level += 1;
          
            Update_Get_Equipment(Equipment_Type.Candelabrador);
        }

        else if(p_ET == Equipment_Type.EmptyTome)
        {
            EmptyTome_Level += 1;

            Update_Get_Equipment(Equipment_Type.EmptyTome);
        }

        else if(p_ET == Equipment_Type.Duplicator)
        {
            Duplicator_Level += 1;

            Update_Get_Equipment(Equipment_Type.Duplicator);
        }

        else if(p_ET == Equipment_Type.Attractorb)
        {
            Attractorb_Level += 1;

            Update_Get_Equipment(Equipment_Type.Attractorb);
        }
    }

    // 플레이어 오리진 스탯에 퍼센티지 스탯 적용
    public void Origin_Stat_Application_Player(Equipment_Type p_ET)
    {
        //
        // this.Origin_Stat.Unit_HP 

        if (p_ET == Equipment_Type.Spinach)
        {
            if (Spinach_Level > 0)
            {
                float Unit_Player_Damage = 0;
                Unit_Player_Damage = this.Percent_Stat.Unit_Weapon_Damage;

                this.Origin_Stat.Unit_Weapon_Damage += Unit_Player_Damage;
            }
        }

        else if (p_ET == Equipment_Type.Bracer)
        {
            if (Bracer_Level > 0)
            {
                float Unit_Player_Proj_Spd = 0;
                Unit_Player_Proj_Spd = this.Percent_Stat.Unit_Weapon_Proj_Speed;

                this.Origin_Stat.Unit_Weapon_Proj_Speed += Unit_Player_Proj_Spd;
            }
        }

        else if (p_ET == Equipment_Type.Duplicator)
        {
            if (Duplicator_Level > 0)
            {
                int Unit_Player_Proj_Count = 0;
                Unit_Player_Proj_Count = this.Percent_Stat.Unit_Weapon_Proj_Count;

                this.Origin_Stat.Unit_Weapon_Proj_Count += Unit_Player_Proj_Count;
            }
        }

        else if (p_ET == Equipment_Type.EmptyTome)
        {
            if (EmptyTome_Level > 0)
            {
                float Unit_Player_Range = 0;
                Unit_Player_Range = this.Percent_Stat.Unit_Weapon_Range;

                this.Origin_Stat.Unit_Weapon_Range += Unit_Player_Range;
            }
        }

        else if (p_ET == Equipment_Type.Wings)
        {
            if (Wings_Level > 0)
            {
                float Unit_Player_Speed = 0;
                Unit_Player_Speed = (this.Origin_Stat.Unit_Speed * this.Percent_Stat.Unit_Speed * 0.01f);

                this.Origin_Stat.Unit_Speed += Unit_Player_Speed;
            }
        }

        else if (p_ET == Equipment_Type.Candelabrador)
        {
            if (Candelabrador_Level > 0)
            {
                float Unit_Player_Range = 0;
                float Unit_Player_Size = 0;
                Unit_Player_Range = this.Percent_Stat.Unit_Weapon_Range;
                Unit_Player_Size = this.Percent_Stat.Unit_Weapon_Size;

                this.Origin_Stat.Unit_Weapon_Range += Unit_Player_Range;
                this.Origin_Stat.Unit_Weapon_Size += Unit_Player_Size;
            }
        }

        else if (p_ET == Equipment_Type.Armour)
        {
            if (Armour_Level > 0)
            {
                float Unit_Player_Defense = 0;
                Unit_Player_Defense = this.Percent_Stat.Unit_Defense;

                this.Origin_Stat.Unit_Defense += Unit_Player_Defense;
            }
        }

        else if (p_ET == Equipment_Type.HollowHeart)
        {
            if (HollowHeart_Level > 0)
            {
                float Unit_Player_HP = 0;
                Unit_Player_HP = (this.Origin_Stat.Unit_HP * this.Percent_Stat.Unit_HP * 0.01f); // (this.Origin_Stat.Unit_HP * this.Percent_Stat.Unit_HP * 0.01f);

                var Temp_HP = this.Origin_Stat.Unit_HP + Unit_Player_HP;

                this.Origin_Stat.Unit_HP = Temp_HP;

                this.Max_HP = this.Origin_Stat.Unit_HP;
            }
        }

        else if (p_ET == Equipment_Type.SpellBinder)
        {
            if (SpellBinder_Level > 0)
            {
                float Unit_Player_Duration = 0;
                Unit_Player_Duration = this.Percent_Stat.Unit_Weapon_Duration;

                this.Origin_Stat.Unit_Weapon_Duration += Unit_Player_Duration;
            }
        }

        else if (p_ET == Equipment_Type.Attractorb)
        {
            if (Attractorb_Level > 0)
            {
                float Unit_Player_Get_Gem_Range = 0;
                Unit_Player_Get_Gem_Range = (this.Origin_Stat.Unit_Get_Gem_Range * this.Percent_Stat.Unit_Get_Gem_Range * 0.01f);

                this.Origin_Stat.Unit_Get_Gem_Range += Unit_Player_Get_Gem_Range;
            }
        }
        
    }


    // 장신구로 패시브 스탯 조정
    public void Get_Equipment_Player_Percentage_Stat(Unit Player_Unit, STO_Equipment Eq_STO)
    {
        var Temp_Equipment_STO = All_Manager.instance.Resource_M.STO_Equipments;

        // Percent_Damage = (1 + Eq_STO.stat / 100f);
        if (Player_Unit == null)
        {
            Debug.Log("Unit NULL");
            return;
        }

        if (Eq_STO == null) //
        {
            //Debug.Log("EQ_STO NULL");
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

    // 무기 레벨 찾아갈수 있게
    public Weapon_Level_And_Timer Convert_WLAT_Player(STO_Weapon p_WTO)
    {
        Weapon_Level_And_Timer WLAT = null;

        //var Temp = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr()

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

    // 장신구 레벨 찾아갈수 있게 // 근데 이걸 위에 이넘이나 스트럭트로 만들어서 저장하면 쉬울꺼 같긴한데
    public int Convert_Equipment_Level_Player(Equipment_Type p_ET)
    {
        var Equipment_Level = 0;

        if (p_ET == Equipment_Type.Spinach)
        {
            Equipment_Level += Spinach_Level;
        }

        if (p_ET == Equipment_Type.Bracer)
        {
            Equipment_Level += Bracer_Level;
        }

        if (p_ET == Equipment_Type.Duplicator)
        {
            Equipment_Level += Duplicator_Level;
        }

        if (p_ET == Equipment_Type.EmptyTome)
        {
            Equipment_Level += EmptyTome_Level;
        }

        if (p_ET == Equipment_Type.Wings)
        {
            Equipment_Level += Wings_Level;
        }

        if (p_ET == Equipment_Type.Candelabrador)
        {
            Equipment_Level += Candelabrador_Level;
        }

        if (p_ET == Equipment_Type.Armour)
        {
            Equipment_Level += Armour_Level;
        }

        if (p_ET == Equipment_Type.HollowHeart)
        {
            Equipment_Level += HollowHeart_Level;
        }

        if (p_ET == Equipment_Type.SpellBinder)
        {
            Equipment_Level += SpellBinder_Level;
        }

        if (p_ET == Equipment_Type.Attractorb)
        {
            Equipment_Level += Attractorb_Level;
        }

        //Debug.Log("Equipment_Level : " + Equipment_Level);

        return Equipment_Level;
    }


    // 장신구로 증가하는 무기 투사체 갯수
    public int Current_Weapon_Count(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        int Weapon_Count_Value = 0;
        Weapon_Count_Value = this.Origin_Stat.Unit_Weapon_Proj_Count;

        int p_Weapon_Count = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Count + Weapon_Count_Value;

        return p_Weapon_Count;
    }

    // 장신구로 감소되는 무기의 쿨타임
    public float Current_Weapon_CoolDown(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        float Weapon_CoolDown_Value = 0;
        Weapon_CoolDown_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_CoolDown * this.Origin_Stat.Unit_Weapon_CoolDown * 0.01f);

        float p_Weapon_CoolDown = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_CoolDown - Weapon_CoolDown_Value;

        return p_Weapon_CoolDown;
    }
    
    // 장신구로 증가하는 현재 무기의 사거리
    public float Current_Weapon_Range(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        float Weapon_Range_Value = 0;
        Weapon_Range_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Range * this.Origin_Stat.Unit_Weapon_Range * 0.01f);

        float p_Weapon_Range = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Range + Weapon_Range_Value;

        return p_Weapon_Range;
    }

    // 장신구로 증가하는 현재 무기의 크기
    public float Current_Weapon_Size(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        float Weapon_Size_Value = 0;
        Weapon_Size_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Size * this.Origin_Stat.Unit_Weapon_Size * 0.01f);

        float p_Weapon_Size = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Size + Weapon_Size_Value;

        return p_Weapon_Size;
    }

    //지금 현재 무기의 공격속도.
    public float Current_Weapon_Speed(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        float Weapon_Speed_Value = 0;
        Weapon_Speed_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Speed * this.Origin_Stat.Unit_Weapon_Proj_Speed * 0.01f);

        float p_Weapon_Speed = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Speed + Weapon_Speed_Value;

        return p_Weapon_Speed;
    }

    //지금 현재 무기의 공격력.
    public float Current_Weapon_Damage(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        float Weapon_Damage_Value = 0;
        Weapon_Damage_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Damage * this.Origin_Stat.Unit_Weapon_Damage * 0.01f);

        float p_Weapon_Damage = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Damage + Weapon_Damage_Value;

        return p_Weapon_Damage;
    }

    //지금 현재 무기의 지속시간.
    public float Current_Weapon_Duration(Weapon_Type p_WT, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        float Weapon_Duration_Value = 0;
        Weapon_Duration_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Duration * this.Origin_Stat.Unit_Weapon_Duration * 0.01f);

        float p_Weapon_Duration = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Duration + Weapon_Duration_Value;

        return p_Weapon_Duration;
    }
    #endregion

    // 플레이어 이동
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
        //RB.MovePosition(RB.position + Next_Vec);

        this.transform.position = this.transform.position + Next_Vec;

        //this.transform.position = this.transform.position + Next_Vec;
        if (InputVec.x != 0)
        {
            SR.flipX = InputVec.x >= 0;
        }
    }


    // 체력 회복 함수
    public void Recovery_Current_HP()
    {
        int Recovery_HP = 25;

        if (this.Current_HP < this.Max_HP)
        {
            this.Current_HP += Recovery_HP;
        }

        else
        {
            Debug.Log("이미 최대 체력 입니다.");
        }

        All_Manager.instance.UI_M.Update_Player_HPBar_Image(Current_HP / Max_HP);
    }

    // 젬 획득 함수
    public void Get_Gem(float p_Get_Gem_Current_Count)
    {
        //All_Manager.instance.UI_M.Update_Gem_Count_Image(p_Get_Gem_Current_Count);
        All_Manager.instance.GameMode_M.Add_Gem(p_Get_Gem_Current_Count);
    }

    // 유닛 플레이어 스탯 조정
    public override void Unit_Stat_Setting(Unit p_Unit, STO_Unit p_Un_STO)
    {
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
            p_Unit.AM.speed = 1.0f; // 애니메이션 재생
        }
    }

    public override void Unit_Animation(Unit_Anim_State State)
    {
        base.Unit_Animation(State);
    }

}

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

//    //방향 바꾸고
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

// 그 방향을 향해서 앞으로

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


// 여기서 All_Weapon에 있는 모든 성경을 다시 갱신
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

//앞 뒤으로가기
// this.transform.position = this.transform.position + (this.transform.up * ySpeed);

//돌기
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
//                    Vector3 Two = One + new Vector3(0, 5, 0); // 위
//                    Vector3 Three = Two + new Vector3(5, 0, 0); //오른쪽
//                    Vector3 Four = One + new Vector3(5, -10, 0); // 오른쪽 아래


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
//    // 지금 Temp가 널이여서 발생하는 문제인것으로 보인다.
//    // 일단 왜 널이 나냐
//    // 지금 일단 다시 한번
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
//            Debug.Log("장신구 장착");
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

