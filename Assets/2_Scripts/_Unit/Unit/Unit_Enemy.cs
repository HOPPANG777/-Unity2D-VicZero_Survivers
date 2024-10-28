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

        if (All_Manager.instance.GameMode_M.Enemy_List.Contains(this))
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

    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        //충돌체가 이 컴포넌트를 가지고 있는가?
        if (collision.transform.GetComponent<Unit_Player>())
        {
            if (Attack_Time > Wating_Attack)
            {
                //collision.transform.GetComponent<Unit_Player>().Hitted(Unit_Enemy_Damage);

                Unit_Player Temp = collision.transform.GetComponent<Unit_Player>();

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

    public void On_Collider(GameObject Temp, GameObject Collider)
    {
        var Collision = Vector2.Distance(Temp.transform.position, Collider.transform.position);

        float radius1 = 0.5f;
        float radius2 = 0.5f;

        if (Collision < (radius1 + radius2))
        {

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

        if (Temp_Player == null)
        {
            return;
        }

        else
        {
            if (All_Manager.instance.GameMode_M.Enemy_List.Contains(this))
            {
                if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) < Dis)
                {

                }

                else
                {
                    if (this.UT != Unit_Type.Boss)
                    {
                        this.gameObject.SetActive(false);

                        All_Manager.instance.Pool_M.Enemy_Pool_In(this);
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