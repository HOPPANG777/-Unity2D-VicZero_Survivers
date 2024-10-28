using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

public class Unit_Enemy : Unit
{
    //public Unit_Type UT;

    public float Attack_Time = 0.0f;
    public float Wating_Attack = 0.0f;

    public Vector2 MoveVec;

    public float Temp_Time = 1.0f;

    public bool Elite = false;

    private float Push_Strength = 0.5f;

    public override void Start()
    {
        base.Start();

        Wating_Attack = 3.0f;
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                base.Update();

                Update_Enemy_Move();

                Player_To_Far_Enemy();

                Enemy_Knockback();

                Enemy_To_Player_Collision();

                Temp_Time -= Time.deltaTime;

                Attack_Time += Time.deltaTime;
            }
        }
    }

    public override void Hitted(float p_Damage_Value)
    {
        base.Hitted(p_Damage_Value);
    }

    public override void Die()
    {
        base.Die();
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

        if (this.UT != Unit_Type.Boss)
        {
            All_Manager.instance.GameMode_M.Enemy_Down(this);
            All_Manager.instance.Pool_M.Enemy_Pool_In(this);
        }
    }

    public override void Unit_Clear()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }


    public void Update_Enemy_Move()
    {
        if (isDead == true) return;

        Vector3 dir = All_Manager.instance.GameMode_M.Current_Player.transform.position - this.transform.position;

        this.transform.position = this.transform.position + (dir.normalized * (Random.Range(Unit_Enemy_Speed, Unit_Enemy_Speed) * Time.deltaTime));

        if (All_Manager.instance.GameMode_M.Enemy_List.Contains(this) || gameObject.GetComponent<Animator>())
        {   
            AM.SetBool("Move", true);
        }
        else
        {
            AM.SetBool("Move", false);
        }

        if (All_Manager.instance.GameMode_M.Current_Player.transform.position.x < this.transform.position.x && isDead == false)
        {
            SR.flipX = true;
        }

        else
        {
            SR.flipX = false;
        }


    }

    //public virtual void OnTriggerStay2D(Collider2D collision)
    //{
    //    //충돌체가 이 컴포넌트를 가지고 있는가?
    //    if (collision.transform.GetComponent<Unit_Player>())
    //    {
    //        if (Attack_Time > Wating_Attack)
    //        {
    //            //collision.transform.GetComponent<Unit_Player>().Hitted(Unit_Enemy_Damage);

    //            Unit_Player Temp = collision.transform.GetComponent<Unit_Player>();

    //            if (this.UT == Unit_Type.Barrel)
    //            {
 
    //            }

    //            else
    //            {
    //                Temp.Hitted(Unit_Enemy_Damage);
    //            }

    //            Attack_Time = 0.0f;
    //        }
    //    }
    //}

    public virtual void Enemy_To_Player_Collision()
    {
        var Player = All_Manager.instance.GameMode_M.Current_Player;
        var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;

        for (int i = 0; i < Enemy_List.Count; i++)
        {
            Vector2 Dir = Player.transform.position - Enemy_List[i].transform.position;
            float Dis = Dir.magnitude;

            float Min_Distance = 0.5f;

            if (Dis < Min_Distance)
            {
                if (Attack_Time > Wating_Attack)
                {
                    //collision.transform.GetComponent<Unit_Player>().Hitted(Unit_Enemy_Damage);

                    Unit_Player Temp = Player.transform.GetComponent<Unit_Player>();

                    if (this.UT == Unit_Type.Barrel)
                    {

                    }

                    else
                    {
                        Temp.Hitted(Unit_Enemy_Damage);
                    }

                    Attack_Time = 0.0f;
                }
            }
        
        }

    }

    public virtual void Enemy_Knockback()
    {
        var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;

        for (int i = 0; i < Enemy_List.Count; i++)
        {
            for (int j = i + 1; j < Enemy_List.Count; j++)
            {
                Vector2 Dir = Enemy_List[j].transform.position - Enemy_List[i].transform.position;
                float Dis = Dir.magnitude;

                // 충돌 반경 설정 (적의 크기와 비례하여 설정)
                float Min_Distance = 1.0f;

                if (Dis < Min_Distance)
                {
                    // 충돌이 발생한 경우 밀쳐내기
                    Vector2 Push_Direction = Dir.normalized * (Min_Distance - Dis) * Push_Strength;

                    Enemy_List[i].transform.position -= (Vector3)Push_Direction * 0.5f;
                    Enemy_List[j].transform.position += (Vector3)Push_Direction * 0.5f;
                }
            }
        }
    }


    public void Elite_Outline_Set(Unit_Type p_UT)
    {
        //float Set_Elite = 0.05f;
        float Set_Not_Elite = 0.0f;

        if (Elite == false)
        {
            SR.material.SetFloat("_Outline", Set_Not_Elite);
        }

        else
        {
            SR.material.SetFloat("_Outline", Set_Not_Elite);
            Elite_HP_Setting(p_UT);
        }

    }

    public void Elite_HP_Setting(Unit_Type p_UT)
    {
        if (Elite == false)
        {

        }

        else
        {
            float Elite_HP = this.Max_HP;

            Elite_HP = Elite_HP * 3.0f;

            this.Unit_Enemy_HP = this.Current_HP = Max_HP = Elite_HP;
        }
    }

    // 유닛 에너미 스탯 조정
    public override void Unit_Stat_Setting(Unit p_Unit, STO_Unit p_Un_STO)
    {
        p_Unit.SR.sprite = p_Un_STO.Sprite_Image;
        p_Unit.SR.material = p_Un_STO.Elite_Material;

        p_Unit.Max_HP = p_Unit.Current_HP = p_Un_STO.Unit_Enemy_HP;
        p_Unit.Unit_Enemy_HP = p_Un_STO.Unit_Enemy_HP;
        p_Unit.Unit_Enemy_Damage = p_Un_STO.Unit_Enemy_Damage;
        p_Unit.Unit_Enemy_Speed = (p_Un_STO.Unit_Enemy_Speed * 1.5f);

        p_Unit.BC.size = new Vector2(p_Un_STO.Size_X, p_Un_STO.Size_Y);
        p_Unit.BC.offset = new Vector2(p_Un_STO.Offset_X, p_Un_STO.Offset_Y);

        if (p_Un_STO.Unit_Animator_Controller != null)
        {
            p_Unit.AM.runtimeAnimatorController = p_Un_STO.Unit_Animator_Controller;
            p_Unit.AM.speed = 1.0f; // 애니메이션 재생
        }
    }

    // 너무 멀리있는 적 오브젝트 풀로 반환.
    public void Player_To_Far_Enemy()
    {
        var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
        var Dis = 100.0f;

        var Temp_Speed = Unit_Enemy_Speed;

        var Dir = this.transform.position - Temp_Player.transform.position;

        if (Temp_Player == null)
        {
            return;
        }

        else
        {
            if (All_Manager.instance.GameMode_M.Enemy_List.Contains(this))
            {
                if (Dir.magnitude > Dis)
                {
                    if (this.UT != Unit_Type.Boss)
                    {
                        //this.gameObject.SetActive(false);

                        //All_Manager.instance.Pool_M.Enemy_Pool_In(this);

                        var Temp = (this.Unit_Enemy_Speed * 10);

                        Unit_Enemy_Speed = Temp;
                    }
                }

                else if (Dir.magnitude <= Dis)
                {
                    if (this.UT != Unit_Type.Boss)
                    {
                         Unit_Enemy_Speed = (All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(this.UT).Unit_Enemy_Speed * 1.5f);
                    }
                }
            }

        }

    }

    public override void Unit_Animation(Unit_Anim_State State)
    {
        base.Unit_Animation(State);
    }

}

//var target = All_Manager.instance.GameMode_M.Current_Player.transform;

//방향 바꾸고
//float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
//this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);