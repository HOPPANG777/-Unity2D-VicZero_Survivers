using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
    public float Unit_Weapon_Size;
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
    //public Rigidbody2D RB;
    public Animator AM;

    public bool isDead = false;
    public bool isAlive = false;
    protected bool isHit_C = false;
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

    protected bool isKnock = false;
    private Vector3 knockDirection; // 넉백 방향
    private float knockStrength; // 넉백 강도
    private float knockTimer; // 넉백 지속 시간 타이머

    public virtual void Start()
    {
        SR = this.AddComponent<SpriteRenderer>();

        //RB = this.AddComponent<Rigidbody2D>();
        //RB.bodyType = RigidbodyType2D.Kinematic;

        BC = this.AddComponent<BoxCollider2D>();
        BC.isTrigger = true;

        AM = this.AddComponent<Animator>();
    }

    public virtual void Update()
    {
        // 게임이 정지된 상태라면 함수 종료
        if (All_Manager.instance.GameMode_M.Game_Stop) return;

        // SpriteRenderer 색상 업데이트
        Update_Sprite_Renderer();

        // 넉백 처리
        if (isKnock == true)
        {
            Perform_Knockback();
        }

        // 피격 효과 처리
        if (isHit_C)
        {
            Update_Hit_Effect();
        }

        // Hit 상태 시간 경과 처리
        if (!isHit)
        {
            Update_Hit_Timer();
        }
    }

    private void Update_Sprite_Renderer()
    {
        // SR_GB와 Dead_Effect를 기반으로 색상 및 재질 값 설정
        SR.color = new Color(1, 1 - SR_GB, 1 - SR_GB, 1 - Dead_Effect);
        SR.material.SetFloat("_Red_Value", 1 - SR_GB);
        SR.material.SetFloat("_Alpha", 1 - Dead_Effect);
    }

    private void Perform_Knockback()
    {
        Knock_Timer -= Time.deltaTime;

        if (Knock_Timer < 0)
        {
            Knock_Timer = 0;

            isKnock = false;
        }
    }

    private void Update_Hit_Effect()
    {
        // 피격 효과 시간 경과
        SR_GB -= Time.deltaTime;

        if (SR_GB < 0)
        {
            isHit_C = false;
        }
    }

    private void Update_Hit_Timer()
    {
        // 피격 이후 회복 시간을 계산
        Hit_Time += Time.deltaTime;

        if (Hit_Time > 1.0f)
        {
            Hit_Time = 0;
            isHit = true;
        }
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
        yield return new WaitForSeconds(1.6f);

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


#region
// 넉백 중일 때, 타이머를 감소시키면서 넉백 처리
//if (knockTimer > 0)
//{
//    float currentKnockStrength = Mathf.Lerp(knockStrength, 0, 1 - (knockTimer / knockStrength));
//    transform.position += knockDirection * currentKnockStrength * Time.deltaTime;

//    knockTimer -= Time.deltaTime;
//}
//else
//{
//    // 넉백 종료
//    isKnock = false;
//}

//public void Knock_Back(Vector3 knockDir, float knockStrength, float knockDuration)
//{
//    if (isKnock == false)
//    {
//        this.knockDirection = knockDir.normalized;
//        this.knockStrength = knockStrength;
//        this.knockTimer = knockDuration;
//        this.isKnock = true;
//    }
//}
//public void Knock_Back(Vector3 Knock_Dir, float Knock_Garlic, float Knock_Duration)
//{
//    if (!isKnock)
//    {
//        for (int i = 0; i < 100; i++)
//        {
//            this.transform.position = this.transform.position + ((Knock_Dir.normalized * Knock_Garlic) * (100 - i) * 0.01f * Time.deltaTime);
//        }

//        isKnock = true;
//        Knock_Timer = Knock_Duration;

//    }
//}

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

//    public virtual void Update()
//{
//    if (All_Manager.instance.GameMode_M.Game_Stop == true) return;

//    SR.color = new Color(1, 1 - SR_GB, 1 - SR_GB, 1 - Dead_Effect);

//    SR.material.SetFloat("_Red_Value", 1 - SR_GB);

//    SR.material.SetFloat("_Alpha", 1 - Dead_Effect);

//    //if (isKnock == true)
//    //{
//    //    Knock_Timer -= Time.deltaTime;

//    //    if (Knock_Timer < 0)
//    //    {
//    //        Knock_Timer = 0;

//    //        isKnock = false;
//    //    }
//    //}

//    // 넉백 중일 때, 매 프레임마다 넉백 처리
//    if (isKnock)
//    {
//        if (knockTimer > 0)
//        {
//            // 넉백 처리: 점점 줄어드는 강도로 밀려남
//            float currentKnockStrength = Mathf.Lerp(knockStrength, 0, 1 - (knockTimer / knockStrength));
//            transform.position += knockDirection * currentKnockStrength * Time.deltaTime;

//            // 타이머 감소
//            knockTimer -= Time.deltaTime;
//        }
//        else
//        {
//            // 넉백 종료
//            isKnock = false;
//        }
//    }

//    if (isHit_C == true)
//    {
//        SR_GB -= Time.deltaTime;

//        if (SR_GB < 0)
//        {
//            isHit_C = false;
//        }
//    }

//    if (isHit == false)
//    {
//        if (Hit_Time > 1.0f)
//        {
//            Hit_Time = 0;

//            isHit = true;
//        }

//        Hit_Time += Time.deltaTime;
//    }

//    //// 넉백 중일 때 처리
//    //if (isKnockback)
//    //{
//    //    if (knockbackElapsed < knockbackTime)
//    //    {
//    //        // 넉백 방향으로 이동
//    //        transform.position += knockbackDirection * Time.deltaTime;
//    //        knockbackElapsed += Time.deltaTime;
//    //    }
//    //    else
//    //    {
//    //        // 넉백 종료
//    //        isKnockback = false;
//    //    }
//    //}

//}
#endregion