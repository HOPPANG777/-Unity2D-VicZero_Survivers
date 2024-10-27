using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct Unit_Stat_Sheet
{
    public float Unit_HP;
    public float Unit_Speed;
    public float Unit_Defense;
    public float Unit_Weapon_Damage;
    public float Unit_Weapon_Proj_Speed;
    public float Unit_Weapon_Duration;
    public float Unit_Weapon_Range;
    public float Unit_Weapon_CoolDown;
    public int Unit_Weapon_Proj_Count;
    public float Unit_Get_Gem_Range;
}

[System.Serializable]
public enum Unit_Type
{
    Player,
    Boss,
    Barrel,
    Skeleton,
    Archer,
    Knight,
    OrcElite,
    Orc,
    Werebear,
    Slime,
    Werewolf
}

[System.Serializable]
public enum Unit_Anim_State
{ 
    Idle,
    Walk,
    Death
}


public abstract class Unit : MonoBehaviour
{
    public SpriteRenderer SR;
    public BoxCollider2D BC;
    public Rigidbody2D RB;
    public Animator AM;

    public bool isDead = false;
    public bool isAlive = false;
    protected bool isHit_C = false;
    protected bool isKnock = false;
    public bool isEffect = false;
    public bool isHit = false;
    public bool isDeathAnim = false;

    public Unit_Stat_Sheet Origin_Stat;
    public Unit_Stat_Sheet Percent_Stat;

    //퍼센테이지 스탯.Unit_Get_Gem_Range = 10.0f;
    //void PLUS_장신구(매개변수 장신구 이름, 스탯 이넘);
    //그리고 퍼센테이즈 스탯을 가진 장신구가 오리진 스탯에 곱하기

    public Unit_Type UT;

    public float Unit_Enemy_HP = 0f;
    public float Unit_Enemy_Speed = 0f;
    public float Unit_Enemy_Damage = 0f;

    public float Max_HP = 0f;
    public float Current_HP = 0f;

    public float Knock_Timer = 0f;
    public float SR_GB = 0f;
    public float Hit_Time = 0.0f;

    public float Dead_Effect = 0.0f;

    //private Vector3 knockbackDirection;     // 넉백의 방향과 크기
    //private float knockbackTime;            // 넉백 지속 시간
    //private float knockbackElapsed = 0f;    // 넉백 경과 시간
    //private bool isKnockback = false;       // 넉백 상태 여부


    public virtual void Start()
    {
        SR = this.AddComponent<SpriteRenderer>();

        RB = this.AddComponent<Rigidbody2D>();
        RB.bodyType = RigidbodyType2D.Kinematic;

        BC = this.AddComponent<BoxCollider2D>();
        BC.isTrigger = true;

        AM = this.AddComponent<Animator>();
    }

    public virtual void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == true) return;

        SR.color = new Color(1, 1 - SR_GB, 1 - SR_GB, 1 - Dead_Effect);

        SR.material.SetFloat("_Red_Value", 1 - SR_GB);

        SR.material.SetFloat("_Alpha", 1 - Dead_Effect);

        if (isKnock == true)
        {
            Knock_Timer -= Time.deltaTime;

            if (Knock_Timer < 0)
            {
                Knock_Timer = 0;

                isKnock = false;
            }
        }

        if (isHit_C == true)
        {
            SR_GB -= Time.deltaTime;

            if (SR_GB < 0)
            {
                isHit_C = false;
            }
        }

        if (isHit == false)
        {
            if (Hit_Time > 1.0f)
            {
                Hit_Time = 0;

                isHit = true;
            }

            Hit_Time += Time.deltaTime;
        }

        //// 넉백 중일 때 처리
        //if (isKnockback)
        //{
        //    if (knockbackElapsed < knockbackTime)
        //    {
        //        // 넉백 방향으로 이동
        //        transform.position += knockbackDirection * Time.deltaTime;
        //        knockbackElapsed += Time.deltaTime;
        //    }
        //    else
        //    {
        //        // 넉백 종료
        //        isKnockback = false;
        //    }
        //}

    }

    public virtual void Hitted(float p_Damage_Value)
    {
        if (isHit == true)
        {
            var Temp_Defense = 100f / (100f + this.Origin_Stat.Unit_Defense) * p_Damage_Value;

            All_Manager.instance.Audio_M.Unit_Hitted_Effect_Audio_Play(this.UT);

            Current_HP -= (Temp_Defense);

            var Temp_Shader_Alpha = this.SR.material.GetFloat("_Alpha");
            var Temp_Color_Alpha = this.SR.color.a;

            if (!isHit_C)
            {
                isHit_C = true;
                SR_GB = 1f;
            }
            if (Current_HP <= 0)
            {
                AM.SetBool("Move", false);
                Current_HP = 0;
                Die();
            }

            isHit = false;
        }

    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        AM.SetBool("Death", true);

        SR_GB = 0f;
        Hit_Time = 0.0f;

        All_Manager.instance.Audio_M.Unit_Death_Effect_Audio_Play(UT);

        Death_Animation_End();
    }

    protected virtual void Death_Animation_End()
    {
        StartCoroutine(C_Death_Animation_End());
    }

    protected virtual IEnumerator C_Death_Animation_End()
    {
        yield return new WaitForSeconds(1.5f);

        Return_Pool();
    }

    protected virtual void Return_Pool()
    {
        this.gameObject.SetActive(false);
    }

    public abstract void Unit_Clear();

    public void Knock_Back(Vector3 Knock_Dir, float Knock_Garlic, float Knock_Duration)
    {
        if (!isKnock)
        {
            for (int i = 0; i < 100; i++)
            {
                this.transform.position = this.transform.position + ((Knock_Dir.normalized * Knock_Garlic) * (100 - i) * 0.01f * Time.deltaTime);
            }

            isKnock = true;
            Knock_Timer = Knock_Duration;
        }
    }


    //// 넉백 시작 메서드
    //public void Knock_Back(Vector3 direction, float force, float duration)
    //{
    //    knockbackDirection = direction.normalized * force; // 방향 정규화 후 힘 적용
    //    knockbackTime = duration;  // 넉백 지속 시간 설정
    //    knockbackElapsed = 0f;     // 경과 시간 초기화
    //    isKnockback = true;        // 넉백 상태로 설정
    //}



    public abstract void Unit_Stat_Setting(Unit p_Unit, STO_Unit Un_STO);

    public virtual void Unit_Animation(Unit_Anim_State State)
    {

        switch (State)
        {
            case Unit_Anim_State.Idle:
                {
                    Debug.Log("Idle 바뀜");
                    AM.SetBool("Idle", true);
                    AM.SetBool("Move", false);
                    AM.SetBool("Death", false);
                }
                break;
            case Unit_Anim_State.Walk:
                {
                    Debug.Log("Walk 바뀜");
                    AM.SetBool("Idle", false);
                    AM.SetBool("Move", true);
                }
                break;
            case Unit_Anim_State.Death:
                {
                    Debug.Log("Death 바뀜");
                    AM.SetBool("Idle", false);
                    AM.SetBool("Death", true);
                }
                break;
        }
    }
}

//if (!isKnock)
//{
//    for (int i = 0; i < Knock_Count; i++)
//    {
//        this.transform.position = this.transform.position + (Knock_Dir.normalized * Knock_Garlic * (Knock_Count - i) * 0.05f);

//    }

//    isKnock = true;
//    Knock_Timer = Knock_Duration;
//}

//for (int i = 0; i < Knock_Count; i++)
//{
//    this.transform.position = this.transform.position + (Knock_Dir.normalized * Knock_Garlic * (Knock_Count - i) * Knock_Duration);
//}