using JetBrains.Annotations;
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
public class Weapon_Level_And_Timer
{
    public int Level = 0;
    public float Timer = 0;
}

public class Unit_Player : Unit, IWeaponType, IAccessoryType
{
    [SerializeField] Rigidbody2D _playerRigidBody;
    public Rigidbody2D PlayerRigidBody { get { return _playerRigidBody; } }

    [SerializeField] public WeaponType WeaponType { get; set; }
    [SerializeField] public AccessoryType AccessoryType { get; set; }

    //찾아보면서 써보기
    public List<Weapon> All_Weapon = new List<Weapon>();
    public List<STO_Accessory> All_Accessory = new List<STO_Accessory>();
    public Dictionary<WeaponKeyword, int> weaponKeywordCounts = new Dictionary<WeaponKeyword, int>();

    public Vector2 InputVec;

    public bool Player_Right = false;
    public bool Player_Left = false;

    public Weapon_Level_And_Timer Axe_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer KingBible_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Peachone_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer FireWand_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Banana_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Force_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer LightningRing_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer ShieldChasing_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer RuneTracer_LAT = new Weapon_Level_And_Timer();
    public Weapon_Level_And_Timer Nunchaku_LAT = new Weapon_Level_And_Timer();

    public int Bang_Level;
    public int Bracer_Level;
    public int Times_Level;
    public int EmptyTome_Level;
    public int Thunder_Level;
    public int TheFlags_Level;
    public int MagicHood_Level;
    public int FloppyDisk_Level;
    public int Attractorb_Level;
    public int RedAja_Level;

    public bool Creating_Weapon = false;
    public bool Creating_Accessory = false;
    public bool Weapon_Trigger = false;

    public bool Accessory_Level_Up = false;

    public override void Start()
    {
        base.Start();

        _playerRigidBody = this.AddComponent<Rigidbody2D>();
        _playerRigidBody.bodyType = RigidbodyType2D.Kinematic;

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

    public void Create_Weapon(WeaponType p_weaponType)
    {
        StartCoroutine(C_Create_Weapon(p_weaponType));
    }

    #region
    // ���� ����
    public IEnumerator C_Create_Weapon(WeaponType p_weaponType)
    {
        yield return new WaitWhile(() => Creating_Weapon == true);

        var Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        var Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

        Creating_Weapon = true;

        //GameObject Weapon_Go = null;
        GameObject Weapon_Go = null;

        switch (p_weaponType)
        {
            // ����
            case WeaponType.Axe:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);

                    //Weapon_Go.AddComponent<Weapon_Axe>();

                    // Weapon_Go�� �ش� ������ ������Ʈ�� �����ϰ� �ִٸ�.
                    if (Weapon_Go.GetComponent<WeaponAxe>())
                    {
                        // ��ġ �ʱ�ȭ
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        // �� ������ ��������Ʈ�� �ҷ��ö����� ��ٷ���.
                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponAxe>().WeaponSpriteRenderer);
                        Vector3 One = this.transform.position;
                        Vector3 Two, Three, Four;

                        // ���� ���⿡ ����
                        if (Weapon_Trigger)
                        {
                            Two = One + new Vector3(0, 5, 0);
                            Three = Two + new Vector3(Random.Range(1, 5), 0, 0);        // ������
                            Four = One + new Vector3(Random.Range(1, 5), -10, 0);       // ������ �Ʒ�
                        }
                        else
                        {
                            Two = One + new Vector3(0, 5, 0);
                            Three = Two + new Vector3(Random.Range(-5, -1), 0, 0);      // ����
                            Four = One + new Vector3(Random.Range(-5, -1), -10, 0);     // ���� �Ʒ�
                        }

                        // ���� �ʱ�ȭ
                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Axe_LAT.Level);
                        Weapon_Go.GetComponent<WeaponAxe>().Axe_Init(One, Two, Three, Four);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponAxe>().WeaponKeyword);

                        // ������ �����ɶ� ���� �ݴ�� ����
                        Weapon_Trigger = !Weapon_Trigger;

                        // ���� ������ ȿ����
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.Axe);
                    }
                }
                break;
            case WeaponType.KingBible:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);

                    //Weapon_Go.AddComponent<Weapon_KingBible>();

                    if (Weapon_Go.GetComponent<WeaponKingBible>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        //Start()
                        //Update()

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponKingBible>().WeaponSpriteRenderer);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, KingBible_LAT.Level);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponKingBible>().WeaponKeyword);

                        // ���� ���� ������ �ִ� ������ ����
                        int Max = 0;

                        // �÷��̾ ����ϰ� �ִ� ������ ����
                        for (int i = 0; i < All_Weapon.Count; i++)
                        {
                            if (All_Weapon[i].GetComponent<WeaponKingBible>())
                            {
                                Max++;
                            }
                        }

                        // 
                        Max++;

                        //
                        for (int i = 0; i < All_Weapon.Count; i++)
                        {
                            if (All_Weapon[i].GetComponent<WeaponKingBible>())
                            {
                                All_Weapon[i].GetComponent<WeaponKingBible>().Weapon_Max_KingBible_Count = Max;
                            }
                        }

                        Weapon_Go.GetComponent<WeaponKingBible>().Weapon_Max_KingBible_Count = Max;
                        Weapon_Go.GetComponent<WeaponKingBible>().Weapon_Mine_KingBible_Count = Max;

                        //var Temp_KingBible_Range = Current_Weapon_Range(WeaponType.KingBible, KingBible_LAT);

                        //Weapon_Go.GetComponent<Weapon_KingBible>().Weapon_Range = Temp_KingBible_Range;

                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.KingBible);
                    }
                }
                break;
            case WeaponType.Peachone:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);

                    //Weapon_Go.AddComponent<Weapon_Peachone>();

                    if (Weapon_Go.GetComponent<WeaponPeachone>())
                    {
                        Weapon_Go.transform.position = All_Manager.instance.UI_M.Peachone_Image.rectTransform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponPeachone>().WeaponSpriteRenderer);

                        var P_Peachone_UI_Range = Weapon_Go.GetComponent<WeaponPeachone>().Convert_RectTransform_To_Position(All_Manager.instance.UI_M.Peachone_Range_Image.rectTransform);
                        var P_Peachone_UI = Weapon_Go.GetComponent<WeaponPeachone>().Convert_RectTransform_To_Position(All_Manager.instance.UI_M.Peachone_Image.rectTransform);

                        Vector3 One = All_Manager.instance.UI_M.Peachone_Image.transform.position;
                        Vector3 Two = Weapon_Go.GetComponent<WeaponPeachone>().Peachone_Point_Setting(All_Manager.instance.UI_M.Peachone_Image.transform.position);
                        Vector3 Three = Weapon_Go.GetComponent<WeaponPeachone>().Peachone_Point_Setting(P_Peachone_UI_Range);
                        Vector3 Four = P_Peachone_UI_Range;

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Peachone_LAT.Level);
                        Weapon_Go.GetComponent<WeaponPeachone>().Peachone_Init(One, Two, Three, Four);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponPeachone>().WeaponKeyword);
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.Peachone);
                    }

                }
                break;
            case WeaponType.FireWand:
                {
                    var FireWand_Stat = Weapon_STO.Info_s[FireWand_LAT.Level];

                    for (int i = 0; i < Current_Weapon_Count(WeaponType.FireWand, FireWand_LAT); i++)
                    {
                        Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);

                        //Weapon_Go.AddComponent<Weapon_FireWand>();

                        if (Weapon_Go.GetComponent<WeaponFireWand>())
                        {
                            Weapon_Go.transform.position = this.transform.position + new Vector3(0, -0.3f, 0);
                            Weapon_Go.transform.rotation = Quaternion.identity;
                            Weapon_Go.transform.localScale = Vector3.one;

                            yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponFireWand>().WeaponSpriteRenderer);

                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, FireWand_LAT.Level);
                            CheckSynergyEffects(Weapon_Go.GetComponent<WeaponFireWand>().WeaponKeyword);

                            Vector3 Dir_Vec = new Vector3(Mathf.Cos((Mathf.PI * 2 * i / Current_Weapon_Count(WeaponType.FireWand, FireWand_LAT))), Mathf.Sin((Mathf.PI * i * 2 / Current_Weapon_Count(WeaponType.FireWand, FireWand_LAT)))); //���� �������� ������ ����ü���� ���͸� ����

                            if (All_Manager.instance.GameMode_M.Game_Stop == false)
                            {
                                Weapon_Go.transform.position = Weapon_Go.transform.position + (Dir_Vec.normalized * FireWand_Stat.Weapon_Speed * Time.deltaTime);
                            }
                            else { }

                            Weapon_Go.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (Current_Weapon_Count(WeaponType.FireWand, FireWand_LAT))) + 180); //����ü�� ������ �ڿ������� ȸ������ �־���.
                            All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.FireWand);
                        }
                    }
                }
                break;
            case WeaponType.Banana:
                {
                    var W_Banana_S = Weapon_STO.Info_s[Banana_LAT.Level];

                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);

                    //Weapon_Go.AddComponent<Weapon_Banana>();

                    if (Weapon_Go.GetComponent<WeaponBanana>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponBanana>().WeaponSpriteRenderer);

                        Vector3 One = this.transform.position;
                        Vector3 Target = One + new Vector3(Random.Range(-W_Banana_S.Weapon_Range, W_Banana_S.Weapon_Range), Random.Range(-W_Banana_S.Weapon_Range, W_Banana_S.Weapon_Range), 0);

                        Vector3 Two = One + (Target - One) / 3 + new Vector3(Random.Range(-W_Banana_S.Weapon_Range, W_Banana_S.Weapon_Range) / 2, Random.Range(-W_Banana_S.Weapon_Range, W_Banana_S.Weapon_Range) / 2, 0);
                        Vector3 Three = Target + (One - Target) / 3 + new Vector3(Random.Range(-W_Banana_S.Weapon_Range, W_Banana_S.Weapon_Range) / 2, Random.Range(-W_Banana_S.Weapon_Range, W_Banana_S.Weapon_Range) / 2, 0);
                        Vector3 Four = One + ((One - Target) * W_Banana_S.Weapon_Range);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Banana_LAT.Level);
                        Weapon_Go.GetComponent<WeaponBanana>().Banana_Init(One, Two, Three, Four);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponBanana>().WeaponKeyword);
                        //Weapon_Go.GetComponent<Weapon_Banana>().Weapon_Application_Accessory();
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.Banana);
                    }
                }
                break;
            case WeaponType.Force:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);
                    //Weapon_Go.AddComponent<Weapon_Force>();

                    if (Weapon_Go.GetComponent<WeaponForce>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        Weapon_Go.transform.parent = this.transform;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponForce>().WeaponSpriteRenderer);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Force_LAT.Level);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponForce>().WeaponKeyword);
                        //Weapon_Go.GetComponent<Weapon_Force>().Weapon_Application_Accessory();
                        //All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.Force);
                    }
                }
                break;
            case WeaponType.LightningRing:
                {
                    if (All_Manager.instance != null && All_Manager.instance.GameMode_M != null && All_Manager.instance.GameMode_M.Enemy_List != null && All_Manager.instance.GameMode_M.Enemy_List.Count > 0)
                    {
                        var Max_Count = Current_Weapon_Count(WeaponType.LightningRing, LightningRing_LAT);
                        int Count_To_Spawn = Mathf.Min(Max_Count, All_Manager.instance.GameMode_M.Enemy_List.Count); // �ִ� 10�������� �����ϵ��� ����

                        List<int> Spawned_Indices = new List<int>(); // �̹� ������ ���� �ε����� ������ ����Ʈ

                        for (int i = 0; i < Count_To_Spawn; i++)
                        {
                            int Random_Index = Random.Range(0, All_Manager.instance.GameMode_M.Enemy_List.Count);

                            // ��ø ����: �̹� ������ �� ��ġ�� LightningRing�� ������ �ǳʶ�
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

                            Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);
                            //Weapon_Go.AddComponent<Weapon_LightningRing>();


                            // LightningRing�� �������� �ִ��� Ȯ�� �� ó��
                            if (Weapon_Go.GetComponent<WeaponLightningRing>() != null)
                            {
                                Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[Random_Index].transform.position;
                                Weapon_Go.transform.rotation = Quaternion.identity;
                                Weapon_Go.transform.localScale = Vector3.one;

                                yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponLightningRing>().WeaponSpriteRenderer);

                                // Weapon_Init�� ���� �ʿ��� �ʱ�ȭ �۾� ����
                                Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, LightningRing_LAT.Level);
                                CheckSynergyEffects(Weapon_Go.GetComponent<WeaponLightningRing>().WeaponKeyword);
                                //Weapon_Go.GetComponent<Weapon_LightningRing>().Weapon_Application_Accessory();
                                All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.LightningRing);
                            }
                            else
                            {
                                //Debug.Log("LightningRing Gone!");
                            }

                        }
                    }



                }
                break;
            case WeaponType.ShieldChasing:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);
                    //Weapon_Go.AddComponent<Weapon_ShieldChasing>();

                    if (Weapon_Go.GetComponent<WeaponShieldChasing>())
                    {
                        Weapon_Go.transform.position = this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponShieldChasing>().WeaponSpriteRenderer);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, ShieldChasing_LAT.Level);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponShieldChasing>().WeaponKeyword);
                        //Weapon_Go.GetComponent<Weapon_ShieldChasing>().Weapon_Application_Accessory();
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.ShieldChasing);
                    }
                }
                break;
            case WeaponType.RuneTracer:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);
                    //Weapon_Go.AddComponent<Weapon_RuneTracer>();

                    if (Weapon_Go.GetComponent<WeaponRuneTracer>())
                    {
                        Weapon_Go.transform.position = this.transform.position;
                        Weapon_Go.transform.rotation = Quaternion.identity;
                        Weapon_Go.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponRuneTracer>().WeaponSpriteRenderer);

                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, RuneTracer_LAT.Level);
                        //Weapon_Go.GetComponent<Weapon_RuneTracer>().Weapon_Application_Accessory();
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponRuneTracer>().WeaponKeyword);
                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.RuneTracer);
                    }
                }
                break;
            case WeaponType.Nunchaku:
                {
                    Weapon_Go = All_Manager.instance.Pool_M.Weapon_Pool_Out(p_weaponType);
                    //Weapon_Go.AddComponent<Weapon_Nunchaku>();

                    //Weapon_Go.transform.parent = this.transform;

                    if (Weapon_Go.GetComponent<WeaponNunchaku>())
                    {
                        yield return new WaitUntil(() => Weapon_Go.GetComponent<WeaponNunchaku>().WeaponSpriteRenderer);

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


                        Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, Weapon_STO, Nunchaku_LAT.Level);
                        CheckSynergyEffects(Weapon_Go.GetComponent<WeaponNunchaku>().WeaponKeyword);
                        Weapon_Trigger = !Weapon_Trigger;

                        All_Manager.instance.Audio_M.Weapon_Attacking_Effect_Audio_Play(WeaponType.Nunchaku);
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

    void UseWeapon()
    {

    }

    // ���� ���׷��̵�
    public void Upgrade_Weapon(WeaponType p_weaponType)
    {
        if (p_weaponType == WeaponType.Axe)
        {
            Axe_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponAxe>())
                {
                    All_Weapon[i].Upgrade(Axe_LAT.Level);
                }
            }

            //Debug.Log(Axe_LAT.Level);
        }

        else if (p_weaponType == WeaponType.KingBible)
        {
            KingBible_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponKingBible>())
                {
                    All_Weapon[i].Upgrade(KingBible_LAT.Level);
                }
            }

            Create_Weapon(WeaponType.KingBible);
        }

        else if (p_weaponType == WeaponType.Peachone)
        {
            Peachone_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponPeachone>())
                {
                    All_Weapon[i].Upgrade(Peachone_LAT.Level);
                }
            }
        }

        else if (p_weaponType == WeaponType.FireWand)
        {
            FireWand_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponFireWand>())
                {
                    All_Weapon[i].Upgrade(FireWand_LAT.Level);
                }
            }

        }

        else if (p_weaponType == WeaponType.Banana)
        {
            Banana_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponBanana>())
                {
                    All_Weapon[i].Upgrade(Banana_LAT.Level);
                }
            }

        }

        else if (p_weaponType == WeaponType.Force)
        {
            Force_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponForce>())
                {
                    All_Weapon[i].Upgrade(Force_LAT.Level);
                }
            }


        }

        else if (p_weaponType == WeaponType.LightningRing)
        {
            LightningRing_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponLightningRing>())
                {
                    All_Weapon[i].Upgrade(LightningRing_LAT.Level);
                }
            }

        }

        else if (p_weaponType == WeaponType.ShieldChasing)
        {
            ShieldChasing_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponShieldChasing>())
                {
                    All_Weapon[i].Upgrade(ShieldChasing_LAT.Level);
                }
            }

        }

        else if (p_weaponType == WeaponType.RuneTracer)
        {
            RuneTracer_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponRuneTracer>())
                {
                    All_Weapon[i].Upgrade(RuneTracer_LAT.Level);
                }
            }

        }

        else if (p_weaponType == WeaponType.Nunchaku)
        {
            Nunchaku_LAT.Level++;

            for (int i = 0; i < All_Weapon.Count; i++)
            {
                if (All_Weapon[i].GetComponent<WeaponNunchaku>())
                {
                    All_Weapon[i].Upgrade(Nunchaku_LAT.Level);
                }
            }

        }


    }

    // ������ �ִ���
    public bool Check_Weapon(WeaponType p_PT)
    {
        return false;
    }

    // ���� ���� Ÿ�̸� ������Ʈ
    public void Spawn_Weapon_Timer_Update()
    {
        if (Axe_LAT.Level > 0)
        {
            if (Axe_LAT.Timer > Current_Weapon_CoolDown(WeaponType.Axe, Axe_LAT))
            {
                Axe_LAT.Timer = 0;

                //for (int i = 0; i < STO_Weapon_Temp.Info_s[Axe_LAT.Level].Weapon_Count; i++)
                //{
                //    Create_Weapon(WeaponType.Axe);
                //}

                for (int i = 0; i < Current_Weapon_Count(WeaponType.Axe, Axe_LAT); i++)
                {
                    Create_Weapon(WeaponType.Axe);
                }
            }

            Axe_LAT.Timer += Time.deltaTime;
        }

        if (KingBible_LAT.Level > 0)
        {
            if (KingBible_LAT.Timer > Current_Weapon_CoolDown(WeaponType.KingBible, KingBible_LAT))
            {
                KingBible_LAT.Timer = 0;

                //for (int i = 0; i < Current_Weapon_Count(WeaponType.KingBible, KingBible_LAT); i++)    // ������ �� ������ ���� ������ �ȳ������� ����, ���� �̰� STO�� ���� ������ ȸ�� ��Ų�� �� �̰� ���ľ� �Ѵ�.
                //{
                //    Create_Weapon(WeaponType.KingBible);
                //}
            }

            KingBible_LAT.Timer += Time.deltaTime;
        }

        if (Peachone_LAT.Level > 0)
        {
            if (Peachone_LAT.Timer > Current_Weapon_CoolDown(WeaponType.Peachone, Peachone_LAT))
            {
                Peachone_LAT.Timer = 0;


                for (int i = 0; i < Current_Weapon_Count(WeaponType.Peachone, Peachone_LAT); i++)
                {
                    Create_Weapon(WeaponType.Peachone);
                }
            }

            Peachone_LAT.Timer += Time.deltaTime;
        }

        if (FireWand_LAT.Level > 0)
        {
            if (FireWand_LAT.Timer > Current_Weapon_CoolDown(WeaponType.FireWand, FireWand_LAT))
            {
                FireWand_LAT.Timer = 0;

                Create_Weapon(WeaponType.FireWand);
                //Debug.Log("FireWand!");
            }

            FireWand_LAT.Timer += Time.deltaTime;
        }

        if (Banana_LAT.Level > 0)
        {
            if (Banana_LAT.Timer > Current_Weapon_CoolDown(WeaponType.Banana, Banana_LAT))
            {
                Banana_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(WeaponType.Banana, Banana_LAT); i++)
                {
                    Create_Weapon(WeaponType.Banana);
                }
            }

            Banana_LAT.Timer += Time.deltaTime;
        }

        if (Force_LAT.Level > 0)
        {
            var STO_Weapon_Temp = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(WeaponType.Force);

            if (Force_LAT.Timer > Current_Weapon_CoolDown(WeaponType.Force, Force_LAT))
            {
                Force_LAT.Timer = 0;

                Create_Weapon(WeaponType.Force);
            }

            Force_LAT.Timer += Time.deltaTime;
        }

        if (LightningRing_LAT.Level > 0)
        {
            if (LightningRing_LAT.Timer > Current_Weapon_CoolDown(WeaponType.LightningRing, LightningRing_LAT))
            {
                LightningRing_LAT.Timer = 0;

                Create_Weapon(WeaponType.LightningRing);
            }

            LightningRing_LAT.Timer += Time.deltaTime;
        }

        if (ShieldChasing_LAT.Level > 0)
        {
            if (ShieldChasing_LAT.Timer > Current_Weapon_CoolDown(WeaponType.ShieldChasing, ShieldChasing_LAT))
            {
                ShieldChasing_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(WeaponType.ShieldChasing, ShieldChasing_LAT); i++)
                {
                    Create_Weapon(WeaponType.ShieldChasing);
                }
            }

            ShieldChasing_LAT.Timer += Time.deltaTime;
        }

        if (RuneTracer_LAT.Level > 0)
        {
            if (RuneTracer_LAT.Timer > Current_Weapon_CoolDown(WeaponType.RuneTracer, RuneTracer_LAT))
            {
                RuneTracer_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(WeaponType.RuneTracer, RuneTracer_LAT); i++)
                {
                    Create_Weapon(WeaponType.RuneTracer);
                }
            }

            RuneTracer_LAT.Timer += Time.deltaTime;
        }

        if (Nunchaku_LAT.Level > 0)
        {
            if (Nunchaku_LAT.Timer > Current_Weapon_CoolDown(WeaponType.Nunchaku, Nunchaku_LAT))
            {
                Nunchaku_LAT.Timer = 0;

                for (int i = 0; i < Current_Weapon_Count(WeaponType.Nunchaku, Nunchaku_LAT); i++)
                {
                    Create_Weapon(WeaponType.Nunchaku);
                }
            }

            Nunchaku_LAT.Timer += Time.deltaTime;
        }

        //Debug.Log("Weapon_Spawn!");
    }

    // ��ű� ����
    public void Update_Get_Accessory(AccessoryType p_ET)
    {
        var Temp = All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(p_ET);

        if (p_ET == AccessoryType.RedAja)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.RedAja))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.Thunder)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.Thunder))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.MagicHood)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.MagicHood))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.Bang)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.Bang))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.Bracer)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.Bracer))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.FloppyDisk)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.FloppyDisk))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.TheFlags)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.TheFlags))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.EmptyTome)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.EmptyTome))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.Times)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.Times))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        else if (p_ET == AccessoryType.Attractorb)
        {

            if (Temp == All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(AccessoryType.Attractorb))
            {
                Get_Accessory_Player_Percentage_Stat(this, Temp);
            }

            //Get_Accessory_Player_Percentage_Stat(this, All_Manager.instance.Resource_M.STO_Accessory_RedAja);
        }

        //Debug.Log("��ű� ����");

        if (Temp != null)
        {
            All_Accessory.Add(Temp);
        }
    }

    // ��ű� ���׷��̵�
    public void Upgrade_Accessory(AccessoryType p_ET)
    {
        if (p_ET == AccessoryType.RedAja)
        {
            RedAja_Level += 1;

            Update_Get_Accessory(AccessoryType.RedAja);
        }

        else if (p_ET == AccessoryType.Thunder)
        {
            Thunder_Level += 1;

            Update_Get_Accessory(AccessoryType.Thunder);
        }

        else if (p_ET == AccessoryType.MagicHood)
        {
            MagicHood_Level += 1;

            Update_Get_Accessory(AccessoryType.MagicHood);
        }

        else if (p_ET == AccessoryType.Bang)
        {
            Bang_Level += 1;

            Update_Get_Accessory(AccessoryType.Bang);
        }

        else if (p_ET == AccessoryType.Bracer)
        {
            Bracer_Level += 1;

            Update_Get_Accessory(AccessoryType.Bracer);
        }

        else if (p_ET == AccessoryType.FloppyDisk)
        {
            FloppyDisk_Level += 1;

            Update_Get_Accessory(AccessoryType.FloppyDisk);
        }

        else if (p_ET == AccessoryType.TheFlags)
        {
            TheFlags_Level += 1;

            Update_Get_Accessory(AccessoryType.TheFlags);
        }

        else if (p_ET == AccessoryType.EmptyTome)
        {
            EmptyTome_Level += 1;

            Update_Get_Accessory(AccessoryType.EmptyTome);
        }

        else if (p_ET == AccessoryType.Times)
        {
            Times_Level += 1;

            Update_Get_Accessory(AccessoryType.Times);
        }

        else if (p_ET == AccessoryType.Attractorb)
        {
            Attractorb_Level += 1;

            Update_Get_Accessory(AccessoryType.Attractorb);
        }
    }

    // �÷��̾� ������ ���ȿ� �ۼ�Ƽ�� ���� ����
    public void Origin_Stat_Application_Player(AccessoryType p_ET)
    {
        //
        // this.Origin_Stat.Unit_HP 

        if (p_ET == AccessoryType.Bang)
        {
            if (Bang_Level > 0)
            {
                float Unit_Player_Damage = 0;
                Unit_Player_Damage = this.Percent_Stat.Unit_Weapon_Damage;

                this.Origin_Stat.Unit_Weapon_Damage += Unit_Player_Damage;
            }
        }

        else if (p_ET == AccessoryType.Bracer)
        {
            if (Bracer_Level > 0)
            {
                float Unit_Player_Proj_Spd = 0;
                Unit_Player_Proj_Spd = this.Percent_Stat.Unit_Weapon_Proj_Speed;

                this.Origin_Stat.Unit_Weapon_Proj_Speed += Unit_Player_Proj_Spd;
            }
        }

        else if (p_ET == AccessoryType.Times)
        {
            if (Times_Level > 0)
            {
                int Unit_Player_Proj_Count = 0;
                Unit_Player_Proj_Count = this.Percent_Stat.Unit_Weapon_Proj_Count;

                this.Origin_Stat.Unit_Weapon_Proj_Count += Unit_Player_Proj_Count;
            }
        }

        else if (p_ET == AccessoryType.EmptyTome)
        {
            if (EmptyTome_Level > 0)
            {
                float Unit_Player_Range = 0;
                Unit_Player_Range = this.Percent_Stat.Unit_Weapon_Range;

                this.Origin_Stat.Unit_Weapon_Range += Unit_Player_Range;
            }
        }

        else if (p_ET == AccessoryType.Thunder)
        {
            if (Thunder_Level > 0)
            {
                float Unit_Player_Speed = 0;
                Unit_Player_Speed = (this.Origin_Stat.Unit_Speed * this.Percent_Stat.Unit_Speed * 0.01f);

                this.Origin_Stat.Unit_Speed += Unit_Player_Speed;
            }
        }

        else if (p_ET == AccessoryType.TheFlags)
        {
            if (TheFlags_Level > 0)
            {
                float Unit_Player_Range = 0;
                float Unit_Player_Size = 0;
                Unit_Player_Range = this.Percent_Stat.Unit_Weapon_Range;
                Unit_Player_Size = this.Percent_Stat.Unit_Weapon_Size;

                this.Origin_Stat.Unit_Weapon_Range += Unit_Player_Range;
                this.Origin_Stat.Unit_Weapon_Size += Unit_Player_Size;
            }
        }

        else if (p_ET == AccessoryType.MagicHood)
        {
            if (MagicHood_Level > 0)
            {
                float Unit_Player_Defense = 0;
                Unit_Player_Defense = this.Percent_Stat.Unit_Defense;

                this.Origin_Stat.Unit_Defense += Unit_Player_Defense;
            }
        }

        else if (p_ET == AccessoryType.RedAja)
        {
            if (RedAja_Level > 0)
            {
                float Unit_Player_HP = 0;
                Unit_Player_HP = (this.Origin_Stat.Unit_HP * this.Percent_Stat.Unit_HP * 0.01f); // (this.Origin_Stat.Unit_HP * this.Percent_Stat.Unit_HP * 0.01f);

                var Temp_HP = this.Origin_Stat.Unit_HP + Unit_Player_HP;

                this.Origin_Stat.Unit_HP = Temp_HP;

                this.Max_HP = this.Origin_Stat.Unit_HP;
            }
        }

        else if (p_ET == AccessoryType.FloppyDisk)
        {
            if (FloppyDisk_Level > 0)
            {
                float Unit_Player_Duration = 0;
                Unit_Player_Duration = this.Percent_Stat.Unit_Weapon_Duration;

                this.Origin_Stat.Unit_Weapon_Duration += Unit_Player_Duration;
            }
        }

        else if (p_ET == AccessoryType.Attractorb)
        {
            if (Attractorb_Level > 0)
            {
                float Unit_Player_Get_Gem_Range = 0;
                Unit_Player_Get_Gem_Range = (this.Origin_Stat.Unit_Get_Gem_Range * this.Percent_Stat.Unit_Get_Gem_Range * 0.01f);

                this.Origin_Stat.Unit_Get_Gem_Range += Unit_Player_Get_Gem_Range;
            }
        }

    }


    // ��ű��� �нú� ���� ����
    public void Get_Accessory_Player_Percentage_Stat(Unit Player_Unit, STO_Accessory Eq_STO)
    {
        var Temp_Accessory_STO = All_Manager.instance.Resource_M.STO_Accessorys;

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

        if (Eq_STO.ET == Temp_Accessory_STO[0].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Damage += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_Damage;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[1].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Proj_Speed += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_Proj_Speed;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[2].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Proj_Count += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_Proj_Count;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[3].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_CoolDown += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_CoolDown;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[4].ET)
        {
            Player_Unit.Percent_Stat.Unit_Speed += Eq_STO.Accessory_Stat_Sheet.Unit_Speed;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[5].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Range += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_Range;
            Player_Unit.Percent_Stat.Unit_Weapon_Size += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_Size;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[6].ET)
        {
            Player_Unit.Percent_Stat.Unit_Defense += Eq_STO.Accessory_Stat_Sheet.Unit_Defense;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[7].ET)
        {
            Player_Unit.Percent_Stat.Unit_HP += Eq_STO.Accessory_Stat_Sheet.Unit_HP;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[8].ET)
        {
            Player_Unit.Percent_Stat.Unit_Weapon_Duration += Eq_STO.Accessory_Stat_Sheet.Unit_Weapon_Duration;
        }

        else if (Eq_STO.ET == Temp_Accessory_STO[9].ET)
        {
            Player_Unit.Percent_Stat.Unit_Get_Gem_Range += Eq_STO.Accessory_Stat_Sheet.Unit_Get_Gem_Range;
        }

    }

    // ���� ���� ã�ư��� �ְ�
    public Weapon_Level_And_Timer Convert_WLAT_Player(STO_Weapon p_weaponTypeO)
    {
        Weapon_Level_And_Timer WLAT = null;

        //var Temp = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr()

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[0])
        {
            WLAT = Axe_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[1])
        {
            WLAT = KingBible_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[2])
        {
            WLAT = Peachone_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[3])
        {
            WLAT = FireWand_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[4])
        {
            WLAT = Banana_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[5])
        {
            WLAT = Force_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[6])
        {
            WLAT = LightningRing_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[7])
        {
            WLAT = ShieldChasing_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[8])
        {
            WLAT = RuneTracer_LAT;
        }

        if (p_weaponTypeO == All_Manager.instance.Resource_M.STO_Weapons[9])
        {
            WLAT = Nunchaku_LAT;
        }

        return WLAT;

    }

    // ��ű� ���� ã�ư��� �ְ� // �ٵ� �̰� ���� �̳��̳� ��Ʈ��Ʈ�� ���� �����ϸ� ���ﲨ �����ѵ�
    public int Convert_Accessory_Level_Player(AccessoryType p_ET)
    {
        var Accessory_Level = 0;

        if (p_ET == AccessoryType.Bang)
        {
            Accessory_Level += Bang_Level;
        }

        if (p_ET == AccessoryType.Bracer)
        {
            Accessory_Level += Bracer_Level;
        }

        if (p_ET == AccessoryType.Times)
        {
            Accessory_Level += Times_Level;
        }

        if (p_ET == AccessoryType.EmptyTome)
        {
            Accessory_Level += EmptyTome_Level;
        }

        if (p_ET == AccessoryType.Thunder)
        {
            Accessory_Level += Thunder_Level;
        }

        if (p_ET == AccessoryType.TheFlags)
        {
            Accessory_Level += TheFlags_Level;
        }

        if (p_ET == AccessoryType.MagicHood)
        {
            Accessory_Level += MagicHood_Level;
        }

        if (p_ET == AccessoryType.RedAja)
        {
            Accessory_Level += RedAja_Level;
        }

        if (p_ET == AccessoryType.FloppyDisk)
        {
            Accessory_Level += FloppyDisk_Level;
        }

        if (p_ET == AccessoryType.Attractorb)
        {
            Accessory_Level += Attractorb_Level;
        }

        //Debug.Log("Accessory_Level : " + Accessory_Level);

        return Accessory_Level;
    }


    // ��ű��� �����ϴ� ���� ����ü ����
    public int Current_Weapon_Count(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        int Weapon_Count_Value = 0;
        Weapon_Count_Value = this.Origin_Stat.Unit_Weapon_Proj_Count;

        int p_Weapon_Count = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Count + Weapon_Count_Value;

        return p_Weapon_Count;
    }

    // ��ű��� ���ҵǴ� ������ ��Ÿ��
    public float Current_Weapon_CoolDown(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        float Weapon_CoolDown_Value = 0;
        Weapon_CoolDown_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_CoolDown * this.Origin_Stat.Unit_Weapon_CoolDown * 0.01f);

        float p_Weapon_CoolDown = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_CoolDown - Weapon_CoolDown_Value;

        return p_Weapon_CoolDown;
    }

    // ��ű��� �����ϴ� ���� ������ ��Ÿ�
    public float Current_Weapon_Range(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        float Weapon_Range_Value = 0;
        Weapon_Range_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Range * this.Origin_Stat.Unit_Weapon_Range * 0.01f);

        float p_Weapon_Range = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Range + Weapon_Range_Value;

        return p_Weapon_Range;
    }

    // ��ű��� �����ϴ� ���� ������ ũ��
    public float Current_Weapon_Size(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        float Weapon_Size_Value = 0;
        Weapon_Size_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Size * this.Origin_Stat.Unit_Weapon_Size * 0.01f);

        float p_Weapon_Size = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Size + Weapon_Size_Value;

        return p_Weapon_Size;
    }

    //���� ���� ������ ���ݼӵ�.
    public float Current_Weapon_Speed(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        float Weapon_Speed_Value = 0;
        Weapon_Speed_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Speed * this.Origin_Stat.Unit_Weapon_Proj_Speed * 0.01f);

        float p_Weapon_Speed = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Speed + Weapon_Speed_Value;

        return p_Weapon_Speed;
    }

    //���� ���� ������ ���ݷ�.
    public float Current_Weapon_Damage(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        float Weapon_Damage_Value = 0;
        Weapon_Damage_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Damage * this.Origin_Stat.Unit_Weapon_Damage * 0.01f);

        float p_Weapon_Damage = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Damage + Weapon_Damage_Value;

        return p_Weapon_Damage;
    }

    //���� ���� ������ ���ӽð�.
    public float Current_Weapon_Duration(WeaponType p_weaponType, Weapon_Level_And_Timer p_WLAT)
    {
        var STO_Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        float Weapon_Duration_Value = 0;
        Weapon_Duration_Value = (STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Duration * this.Origin_Stat.Unit_Weapon_Duration * 0.01f);

        float p_Weapon_Duration = STO_Weapon_STO.Info_s[p_WLAT.Level].Weapon_Duration + Weapon_Duration_Value;

        return p_Weapon_Duration;
    }
    #endregion

    // �ó��� ȿ�� ���� Ȯ�� �� ó��
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
            _unitAnimator.SetBool("Move", true);
        }
        else
        {
            _unitAnimator.SetBool("Move", false);
        }

        Vector3 Next_Vec = InputVec.normalized * (this.Origin_Stat.Unit_Speed * Time.deltaTime);
        //RB.MovePosition(RB.position + Next_Vec);

        this.transform.position = this.transform.position + Next_Vec;

        //this.transform.position = this.transform.position + Next_Vec;
        if (InputVec.x != 0)
        {
            _unitSpriteRenderer.flipX = InputVec.x >= 0;
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
        //All_Manager.instance.UI_M.Update_Gem_Count_Image(p_Get_Gem_Current_Count);
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
        p_Unit.UnitSpriteRenderer.sprite = p_Un_STO.Sprite_Image;
        p_Unit.UnitSpriteRenderer.material = p_Un_STO.Elite_Material;

        p_Unit.Origin_Stat.Unit_HP = p_Un_STO.Player_info_s.HP;
        p_Unit.Max_HP = p_Unit.Current_HP = p_Un_STO.Player_info_s.HP;
        p_Unit.Origin_Stat.Unit_Speed = (p_Un_STO.Player_info_s.Unit_Speed * 1.5f);
        p_Unit.Origin_Stat.Unit_Defense = p_Un_STO.Player_info_s.Unit_Defense;
        p_Unit.Origin_Stat.Unit_Get_Gem_Range = p_Un_STO.Player_info_s.Unit_Get_Gem_Range;

        p_Unit.UnitBoxCollider2D.size = new Vector2(p_Un_STO.Size_X, p_Un_STO.Size_Y);
        p_Unit.UnitBoxCollider2D.offset = new Vector2(p_Un_STO.Offset_X, p_Un_STO.Offset_Y);

        if (p_Un_STO.Unit_Animator_Controller != null)
        {
            p_Unit.UnitAnimator.runtimeAnimatorController = p_Un_STO.Unit_Animator_Controller;
            p_Unit.UnitAnimator.speed = 1.0f; // �ִϸ��̼� ���
            p_Unit.UnitAnimator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }

    public override void Unit_Animation(UnitAnimationState State)
    {
        base.Unit_Animation(State);
    }

}

#region
/*
         var Weapon_STO = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_Weapon.weaponType);

        var Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

        switch (p_Weapon.WeaponKeywords)
        {
            case WeaponKeyword.NONE:
                break;
            case WeaponKeyword.Bound:
                {
                    switch (p_Weapon.weaponType)
                    {
                        case WeaponType.Axe:
                            {
                                if (Axe_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                        case WeaponType.RuneTracer:
                            {
                                if (RuneTracer_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Phoenix:
                {
                    switch (p_Weapon.weaponType)
                    {
                        case WeaponType.Peachone:
                            {
                                if (Peachone_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                        case WeaponType.FireWand:
                            {
                                if (FireWand_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Paladin:
                {
                    switch (p_Weapon.weaponType)
                    {
                        case WeaponType.ShieldChasing:
                            {
                                if (ShieldChasing_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                        case WeaponType.Banana:
                            {
                                if (Banana_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Sonic:
                switch (p_Weapon.weaponType)
                {
                    case WeaponType.LightningRing:
                        {
                            if (LightningRing_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                    case WeaponType.Nunchaku:
                        {
                            if (Nunchaku_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                }
                break;
            case WeaponKeyword.Holy:
                switch (p_Weapon.weaponType)
                {
                    case WeaponType.Force:
                        {
                            if (Force_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                        }
                        break;
                    case WeaponType.KingBible:
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

//var Temp_Accessory = 

//if (p_weaponType == WeaponType.Axe)
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
//        if (All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count < Current_Weapon_Count(WeaponType.KingBible, KingBible_LAT))
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
//    Temp_Go = Instantiate(Force_Go, null);
//    Temp_Go.transform.parent = this.transform;
//    if (Temp_Go.GetComponent<Force>())
//    {
//        yield return new WaitUntil(() => Temp_Go.GetComponent<Force>()._spriteRenderer);
//        All_Manager.instance.Resource_M.Weapon_Stat(Temp_Go.GetComponent<Force>(), All_Manager.instance.Resource_M.STO_Force);
//    }
//}
//else
//{
//    Skill_Full = 0.0f;
//}

//switch (p_weaponType)
//{
//    case WeaponType.None:
//        {
//        }
//        break;
//    case WeaponType.KingBible:
//        {
//            //Temp_Go = Instantiate(Weapon_Go, null);
//            GameObject Weapon_Go = new GameObject("KingBible");
//            Weapon_Go.AddComponent<KingBible>();
//            Weapon_Go.transform.parent = this.transform;
//            if (Weapon_Go.GetComponent<KingBible>())
//            {
//                yield return new WaitUntil(() => Weapon_Go.GetComponent<KingBible>()._spriteRenderer);
//                All_Manager.instance.Resource_M.Weapon_Stat(Weapon_Go.GetComponent<KingBible>(), All_Manager.instance.Resource_M.STO_KingBible);
//            }
//            if (Weapon_Go.GetComponent<Weapon>())
//            {
//                Weapon_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case WeaponType.SpellBook:
//        {
//            //Temp_Go = Instantiate(SpellBook_Go, null);
//            GameObject SpellBook_Go = new GameObject("SpellBook");
//            SpellBook_Go.AddComponent<SpellBook>();
//            SpellBook_Go.transform.parent = this.transform;
//            if (SpellBook_Go.GetComponent<SpellBook>())
//            {
//                yield return new WaitUntil(() => SpellBook_Go.GetComponent<SpellBook>()._spriteRenderer);
//                All_Manager.instance.Resource_M.Weapon_Stat(SpellBook_Go.GetComponent<SpellBook>(), All_Manager.instance.Resource_M.STO_SpellBook);
//            }
//            if (SpellBook_Go.GetComponent<Weapon>())
//            {
//                SpellBook_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case WeaponType.FireWand_Launcher:
//        {
//            //Temp_Go = Instantiate(FireWand_Launcher_Go, null);
//            GameObject FireWand_Launcher_Go = new GameObject("FireWand_Launcher");
//            FireWand_Launcher_Go.AddComponent<FireWand_Launcher>();
//            FireWand_Launcher_Go.transform.parent = this.transform;
//            if (FireWand_Launcher_Go.GetComponent<FireWand_Launcher>())
//            {
//                yield return new WaitUntil(() => FireWand_Launcher_Go.GetComponent<FireWand_Launcher>()._spriteRenderer);
//                All_Manager.instance.Resource_M.Weapon_Stat(FireWand_Launcher_Go.GetComponent<FireWand_Launcher>(), All_Manager.instance.Resource_M.STO_FireWand_Launcher);
//            }
//            if (FireWand_Launcher_Go.GetComponent<Weapon>())
//            {
//                FireWand_Launcher_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case WeaponType.ShieldChasing_Launcher:
//        {
//            //Temp_Go = Instantiate(ShieldChasing_Launcher_Go, null);
//            GameObject ShieldChasing_Launcher_Go = new GameObject("ShieldChasing_Launcher");
//            ShieldChasing_Launcher_Go.AddComponent<ShieldChasing_Launcher>();
//            ShieldChasing_Launcher_Go.transform.parent = this.transform;
//            if (ShieldChasing_Launcher_Go.GetComponent<ShieldChasing_Launcher>())
//            {
//                yield return new WaitUntil(() => ShieldChasing_Launcher_Go.GetComponent<ShieldChasing_Launcher>()._spriteRenderer);
//                All_Manager.instance.Resource_M.Weapon_Stat(ShieldChasing_Launcher_Go.GetComponent<ShieldChasing_Launcher>(), All_Manager.instance.Resource_M.STO_ShieldChasing_Launcher);
//            }
//            if (ShieldChasing_Launcher_Go.GetComponent<Weapon>())
//            {
//                ShieldChasing_Launcher_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case WeaponType.Force:
//        {
//            //Temp_Go = Instantiate(Force_Go, null);
//            GameObject Force_Go = new GameObject("Force");
//            Force_Go.AddComponent<Force>();
//            Force_Go.transform.parent = this.transform;
//            if (Force_Go.GetComponent<Force>())
//            {
//                yield return new WaitUntil(() => Force_Go.GetComponent<Force>()._spriteRenderer);
//                All_Manager.instance.Resource_M.Weapon_Stat(Force_Go.GetComponent<Force>(), All_Manager.instance.Resource_M.STO_Force);
//            }
//            if (Force_Go.GetComponent<Weapon>())
//            {
//                Force_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    default:
//        break;
//}

//public IEnumerator C_Create_Weapon(WeaponType p_PT)
//{
//    yield return new WaitWhile(() => Creating_Weapon == true);

//    Creating_Weapon = true;

//    GameObject Weapon_Go = null;

//    switch (p_PT)
//    {
//        case WeaponType.None:
//            break;
//        case WeaponType.KingBible:
//            {
//                Weapon_Go = new GameObject("KingBible");

//                Weapon_Go.AddComponent<Weapon_KingBible>();

//                Weapon_Go.transform.parent = this.transform;
//                if (Weapon_Go.GetComponent<Weapon_KingBible>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_KingBible>()._spriteRenderer);
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
//        case WeaponType.Force:
//            {
//                Weapon_Go = new GameObject("Force");

//                Weapon_Go.AddComponent<Weapon_Force>();

//                if (Weapon_Go.GetComponent<Weapon_Force>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Force>()._spriteRenderer);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_Force, Force_LAT.Level);
//                }
//            }
//            break;
//        case WeaponType.LightningRing:
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
//                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>()._spriteRenderer);
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
//                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>()._spriteRenderer);
//                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                        }

//                        Destroy(Weapon_Go, 2.5f);
//                    }
//                }

//            }
//            break;
//        case WeaponType.FireWand:
//            {
//                Weapon_Go = new GameObject("FireWand");

//                Weapon_Go.AddComponent<Weapon_FireWand>();

//                if (Weapon_Go.GetComponent<Weapon_FireWand>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_FireWand>()._spriteRenderer);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_FireWand, FireWand_LAT.Level);
//                }
//            }
//            break;
//        case WeaponType.ShieldChasing:
//            {
//                Weapon_Go = new GameObject("ShieldChasing");

//                Weapon_Go.AddComponent<Weapon_ShieldChasing>();

//                if (Weapon_Go.GetComponent<Weapon_ShieldChasing>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_ShieldChasing>()._spriteRenderer);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_ShieldChasing, ShieldChasing_LAT.Level);
//                }
//            }
//            break;
//        case WeaponType.Axe:
//            {
//                Weapon_Go = new GameObject("Axe");

//                Weapon_Go.AddComponent<Axe>();



//                if (Weapon_Go.GetComponent<Axe>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Axe>()._spriteRenderer);
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
//            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>()._spriteRenderer);
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
//            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>()._spriteRenderer);
//            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//        }

//        Destroy(Weapon_Go, 2.5f);
//    }
//}

//public void Get_Accessory(AccessoryType p_ET)
//{
//    StartCoroutine(C_Get_Accessory(p_ET));
//}

//public IEnumerator C_Get_Accessory(AccessoryType p_ET)
//{
//    // ���� Temp�� ���̿��� �߻��ϴ� �����ΰ����� ���δ�.
//    // �ϴ� �� ���� ����
//    // ���� �ϴ� �ٽ� �ѹ�
//    yield return new WaitWhile(() => Creating_Accessory == true);

//    var Temp = All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory(p_ET);

//    Creating_Accessory = true;

//    if (p_ET == AccessoryType.None)
//    {

//    }

//    if (p_ET == AccessoryType.RedAja)
//    {
//        yield return new WaitUntil(() => (Temp = All_Manager.instance.Resource_M.STO_Accessory_RedAja));
//        if (Temp == All_Manager.instance.Resource_M.STO_Accessory_RedAja)
//        {
//            RedAja_Level++;

//            Get_Accessory_Player_Percentage_Stat(this, Temp);
//            Debug.Log(Temp);
//            Debug.Log("��ű� ����");
//        }
//    }

//    if (p_ET == AccessoryType.Thunder)
//    {
//        Thunder_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.MagicHood)
//    {
//        MagicHood_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.Bang)
//    {
//        Bang_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.Bracer)
//    {
//        Bracer_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.FloppyDisk)
//    {
//        FloppyDisk_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.TheFlags)
//    {
//        TheFlags_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.EmptyTome)
//    {
//        EmptyTome_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.Times)
//    {
//        Times_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == AccessoryType.Attractorb)
//    {
//        Attractorb_Level++;

//        Get_Accessory_Player_Percentage_Stat(this, Temp);
//    }

//    if (Temp != null)
//    {
//        All_Accessory.Add(Temp);
//    }

//    Creating_Accessory = false;
//}
#endregion
