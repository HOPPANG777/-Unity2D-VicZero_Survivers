using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Weapon : MonoBehaviour
{
    public Weapon_Type WT;

    public SpriteRenderer SR;
    public BoxCollider2D BC;
    //public Rigidbody2D RB;
    public TrailRenderer TR;
    public Animator AM;

    public Unit Mine;

    public Unit_Enemy Weapon_Target_Weapon;

    public float Weapon_Damage = 0;
    public float Weapon_Speed = 0;
    public float Weapon_Range = 0;
    public float Weapon_Time = 0;
    public float Weapon_Pen = 0;
    public float Weapon_CoolDown = 0;
    public float Weapon_Duration = 0;
    public float W_Accumulate = 0;
    public float Weapon_Knock = 0;
    public int Weapon_Level = 0;
    public int Weapon_Count = 0;
    public float Weapon_Size = 0;

    public float Weapon_Basic_Damage = 0;
    public float Weapon_Basic_Speed = 0;
    public float Weapon_Basic_Range = 0;
    public float Weapon_Basic_Duration = 0;

    public int Collision_Count = 0;
    public float Closet_Distance = 0;

    public bool Forever = false;

    public virtual void Start()
    {
        SR = this.AddComponent<SpriteRenderer>();

        //RB = this.AddComponent<Rigidbody2D>();
        //RB.bodyType = RigidbodyType2D.Kinematic;

        BC = this.AddComponent<BoxCollider2D>();
        BC.isTrigger = true;

        SR.drawMode = SpriteDrawMode.Sliced;
        SR.size = new Vector2(1, 1);

        TR = this.AddComponent<TrailRenderer>();
        TR.enabled = false;

        AM = this.AddComponent<Animator>();
    }

    public void Weapon_Init(Unit p_User, STO_Weapon p_WP, int p_Level)
    {
        Mine = p_User;

        Weapon_Level = p_Level;

        Weapon_Stat(Weapon_Level, this, p_WP);
    }

    public virtual void Update()
    {
        if (All_Manager.instance.GameMode_M.Current_Player == null)
        {
            Destroy_Weapon();
        }
        else
        {
            Weapon_Stat_Appliaction();
            Weapon_To_Enemy_Collision();
        }

    }

    // 업그레이드
    public void Upgrade(int p_Level)
    {
        //Weapon_Application_Equipment();
    }
    
    // 무기 파괴 타이머
    public void Weapon_Timer()
    {
        if (Weapon_Time > Weapon_Duration)
        {
            Weapon_Time = 0;

            Destroy_Weapon();
        }

        Weapon_Time += Time.deltaTime;
    }

    // 베지어 곡선을 쓰는 무기들을 위한 타이머
    public void Weapon_Accumulate()
    {
        if (W_Accumulate < 1.0f)
        {
            W_Accumulate += (Weapon_Speed * Time.deltaTime);
        }
        else if (W_Accumulate > 1.0f)
        {
            Destroy_Weapon();
        }
    }

    // 무기가 적과 충돌했을때 // 피격처리
    //public virtual void OnTriggerStay2D(Collider2D collision)
    //{
    //    //충돌체가 이 컴포넌트를 가지고 있는가?
    //    if (collision.transform.GetComponent<Unit_Enemy>())
    //    {
    //        //collision.transform.GetComponent<Unit_Enemy>().Hitted(Weapon_Damage);

    //        Unit_Enemy Temp_Enemy = collision.transform.GetComponent<Unit_Enemy>();

    //        //Temp.Hitted(Weapon_Damage_Value);

    //        Temp_Enemy.Hitted(Weapon_Damage);

    //        if (Forever == false)
    //        {
    //            if (Collision_Count > Weapon_Pen)
    //            {
    //                Destroy_Weapon();
    //            }
    //        }
    //        else
    //        {

    //        }

    //        //Debug.Log("Enemey Hitted");

    //        Collision_Count++;

    //    }

    //}


    public virtual void Weapon_To_Enemy_Collision()
    {
        var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;
        var Weapon = this;

        for (int i = 0; i < Enemy_List.Count; i++)
        {
            Vector2 Dir = Weapon.transform.position - Enemy_List[i].transform.position;
            float Dis = Dir.magnitude;

            var Temp_WTO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(WT);
            var Temp_WALT = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO);

            float Min_Distance = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Range(WT, Temp_WALT);

            if (Dis < Min_Distance)
            {
                //collision.transform.GetComponent<Unit_Enemy>().Hitted(Weapon_Damage);

                Unit_Enemy Temp_Enemy = Enemy_List[i].transform.GetComponent<Unit_Enemy>();

                //Temp.Hitted(Weapon_Damage_Value);

                Temp_Enemy.Hitted(Weapon_Damage);

                if (Forever == false)
                {
                    if (Collision_Count > Weapon_Pen)
                    {
                        Destroy_Weapon();
                    }
                }
                else
                {

                }

                //Debug.Log("Enemey Hitted");

                Collision_Count++;
            }
        }


    }

    // 무기 넉백
    public void Weapon_KnockBack_Weapon()
    {
        Vector3 BC_Center = BC.transform.TransformPoint(BC.offset);
        Vector3 BC_Half_Extents = Vector3.Scale(BC.size, BC.transform.lossyScale) * 0.5f;
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(BC_Center, BC.size * 0.9f, 0);
        //Debug.Log(collider2Ds);

        float Dis = 10000;

        foreach (UnityEngine.Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.GetComponent<Unit_Enemy>())
            {
                Unit_Enemy Enemy_Temp = collider.gameObject.GetComponent<Unit_Enemy>();

                if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
                {
                    Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
                    Weapon_Target_Weapon = Enemy_Temp;
                }

                //Weapon_Target_Weapon = Enemy_Temp;
            }
        }

        if (Weapon_Target_Weapon != null)
        {
            Vector3 Dir = (Weapon_Target_Weapon.transform.position - this.transform.position).normalized;

            if (Weapon_Target_Weapon.UT == Unit_Type.Barrel)
            {
                
            }

            else
            {
                Weapon_Target_Weapon.Knock_Back(Dir, Weapon_Knock, Weapon_Duration);
            }
        }
    }

    // 무기 기본 스탯 조정
    public void Weapon_Stat(int p_Level, Weapon weapon, STO_Weapon weapon_STO)
    {
        weapon.SR.sprite = weapon_STO.Sprite_Image;
        weapon.WT = weapon_STO.WT;

        if (weapon_STO.Weapon_Animator_Controller != null)
        {
            weapon.AM.runtimeAnimatorController = weapon_STO.Weapon_Animator_Controller;
            weapon.AM.speed = 1.0f;
        }

        weapon.Weapon_Damage = weapon_STO.Info_s[p_Level].Weapon_Damage;
        weapon.Weapon_Speed = (weapon_STO.Info_s[p_Level].Weapon_Speed * 1.5f);
        weapon.Weapon_Range = weapon_STO.Info_s[p_Level].Weapon_Range;
        weapon.Weapon_Time = weapon_STO.Info_s[p_Level].Weapon_Time;
        weapon.Weapon_Count = weapon_STO.Info_s[p_Level].Weapon_Count;
        weapon.Weapon_Pen = weapon_STO.Info_s[p_Level].Weapon_Pen;
        weapon.Weapon_CoolDown = weapon_STO.Info_s[p_Level].Weapon_CoolDown;
        weapon.Weapon_Duration = weapon_STO.Info_s[p_Level].Weapon_Duration;
        weapon.Weapon_Knock = weapon_STO.Info_s[p_Level].Weapon_Knock;
        weapon.Weapon_Size = weapon_STO.Info_s[p_Level].Weapon_Size;
        weapon.Forever = weapon_STO.Forever;


        var Temp_WLAT = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(weapon_STO);

        var Temp_Weapon_Size = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Size(WT, Temp_WLAT);

        weapon.GetComponent<Weapon>().SR.drawMode = SpriteDrawMode.Sliced;

        weapon.GetComponent<Weapon>().SR.size = new Vector2((Temp_Weapon_Size), (Temp_Weapon_Size));
        weapon.BC.size = new Vector2((Temp_Weapon_Size * 0.5f), (Temp_Weapon_Size * 0.5f));
        
    }
    
    public void Weapon_Stat_Appliaction()
    {
        var Temp_WTO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(WT);

        var Temp_WLAT = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO);

        var Temp_Weapon_Damage = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Damage(WT, Temp_WLAT);
        var Temp_Weapon_Speed = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Speed(WT, Temp_WLAT);
        var Temp_Weapon_Range = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Range(WT, Temp_WLAT);
        var Temp_Weapon_Size = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Size(WT, Temp_WLAT);
        var Temp_Weapon_Duration = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Duration(WT, Temp_WLAT);

        Weapon_Damage = Temp_Weapon_Damage;
        Weapon_Speed = Temp_Weapon_Speed;
        Weapon_Range = Temp_Weapon_Range;
        Weapon_Duration = Temp_Weapon_Duration;
        Weapon_Size = Temp_Weapon_Size;
    }


    public void Weapon_Trail()
    {
        // Trail Renderer 설정
        TR.time = 1.0f; // 꼬리가 지속되는 시간
        TR.startWidth = 0.05f; // 꼬리의 시작 너비
        TR.endWidth = 0.01f; // 꼬리의 끝 너비

        // 재질 설정
        TR.material = new Material(Shader.Find("Sprites/Default"));
        TR.startColor = Color.white; // 꼬리의 시작 색상
        TR.endColor = Color.white; // 꼬리의 끝 색상

        // 추가 설정 (필요에 따라 변경 가능)
        TR.minVertexDistance = 0.1f; // 최소 꼭짓점 간 거리
        TR.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.2f),
            new Keyframe(0.5f, 0.0f)
        ); // 너비 변화 곡선
        TR.colorGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 0.5f),
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        }; // 색상 변화 그라디언트

    }

    // 무기 파괴 함수
    public void Destroy_Weapon()
    {
        // 만약 이 Mine으로 안된다면 다 Current_Player로 통합해야 한다.
        //if (All_Manager.instance.GameMode_M.Current_Player == null)
        //{
        //    Destroy(this.gameObject);
        //}

        //Debug.Log("Weapon Destroy");

        // 만약 Mine이 null 이라면
        if (All_Manager.instance.GameMode_M.Current_Player == null)
        {
            //Destroy(this.gameObject);

            All_Manager.instance.Pool_M.Weapon_Pool_In(this.gameObject.GetComponent<Weapon>());        
        }

        // Mine이 null이 아니라면
        else
        {
            if (All_Manager.instance.GameMode_M.Current_Player.All_Weapon.Contains(this))
            {
                All_Manager.instance.GameMode_M.Current_Player.All_Weapon.Remove(this);
                //Debug.Log("Weapon Destroy");
            }
        }

        //Destroy(this.gameObject);

        All_Manager.instance.Pool_M.Weapon_Pool_In(this.gameObject.GetComponent<Weapon>());
    }
}


#region
/*
도끼같은건 보통 관통력을 지니고 있다 룬트레이서도 마찬가지
따라서 Weapon_Pen을 추가했는데 
유닛이나 웨폰에 충돌할때마다 Pen을 감소시키거나 혹은 변수하나 만들어서 
하나씩 증가시키고 변수의 값이 Pen보다 크거나 같다면 
그때 Forever를 false로 만들면 된다.
*/

// 무기 스탯 가중치
//float Weapon_CoolDown_Value = 0;
//Weapon_CoolDown_Value = Mine.Origin_Stat.Unit_Weapon_CoolDown + (Mine.Origin_Stat.Unit_Weapon_CoolDown * Mine.Percent_Stat.Unit_Weapon_CoolDown * 0.01f);

//Weapon_CoolDown -= Weapon_CoolDown_Value;

//for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//{
//    var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//    var Temp_Count = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_Count;

//    All_Manager.instance.GameMode_M.Current_Player.Weapon_Public_Count = Temp_Count + Weapon_Count_Value;
//}
// 무기 오리진 스탯에 퍼센티지 스탯 적용
//public void Weapon_Application_Equipment()
//{
//    float Weapon_Damage_Value = 0;
//    Weapon_Damage_Value = Mine.Origin_Stat.Unit_Weapon_Damage + (Mine.Origin_Stat.Unit_Weapon_Damage * Mine.Percent_Stat.Unit_Weapon_Damage * 0.01f);

//    for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    {
//        var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//        var Temp_Damage = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_Damage;

//        Temp_Damage += Weapon_Damage_Value;
//    }

//    float Weapon_Speed_Value = 0;
//    Weapon_Speed_Value = Mine.Origin_Stat.Unit_Weapon_Proj_Speed + (Mine.Origin_Stat.Unit_Weapon_Proj_Speed * Mine.Percent_Stat.Unit_Weapon_Proj_Speed * 0.01f);

//    for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    {
//        var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//        var Temp_Speed = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_Speed;

//        Temp_Speed += Weapon_Speed_Value;
//    }

//    float Weapon_Range_Value = 0;
//    Weapon_Range_Value = Mine.Origin_Stat.Unit_Weapon_Range + (Mine.Origin_Stat.Unit_Weapon_Range * Mine.Percent_Stat.Unit_Weapon_Range * 0.01f);

//    for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    {
//        var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//        var Temp_Range = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_Range;

//        Temp_Range += Weapon_Range_Value;
//    }

//    float Weapon_CoolDown_Value = 0;
//    Weapon_CoolDown_Value = Mine.Origin_Stat.Unit_Weapon_CoolDown + (Mine.Origin_Stat.Unit_Weapon_CoolDown * Mine.Percent_Stat.Unit_Weapon_CoolDown * 0.01f);

//    for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    {
//        var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//        var Temp_CoolDown = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_CoolDown;

//        Temp_CoolDown -= Weapon_CoolDown_Value;
//    }

//    float Weapon_Duration_Value = 0;
//    Weapon_Duration_Value = Mine.Origin_Stat.Unit_Weapon_Duration + (Mine.Origin_Stat.Unit_Weapon_Duration * Mine.Percent_Stat.Unit_Weapon_Duration * 0.01f);

//    for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    {
//        var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//        var Temp_Duration = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_Duration;

//        Temp_Duration += Weapon_Duration_Value;
//    }

//    int Weapon_Count_Value = 0;
//    Weapon_Count_Value = Mine.Origin_Stat.Unit_Weapon_Proj_Count + Mine.Percent_Stat.Unit_Weapon_Proj_Count;

//    //Weapon_Count += Weapon_Count_Value;

//    //for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    //{
//    //    //All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[]
//    //    //for (int i = 0; i < )
//    //}

//    for (int i = 0; i < All_Manager.instance.Resource_M.STO_Weapons.Length; i++)
//    {
//        var WLAT_Temp = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(All_Manager.instance.Resource_M.STO_Weapons[i]).Level;

//        var Temp_Count = All_Manager.instance.Resource_M.STO_Weapons[i].Info_s[WLAT_Temp].Weapon_Count;

//        Temp_Count += Weapon_Count_Value;
//    }
//}

////
//Vector3 Knock = Dir * Weapon_Speed;
// F_target.Knock_Back(Dir, Weapon_Knock, Weapon_Duration, Weapon_Kncok_Temp);
////
#endregion