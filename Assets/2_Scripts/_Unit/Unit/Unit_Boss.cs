using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BossState
{
    Idle,
    Move,
    PrepareRush,
    Rush,
    Cooldown,
    ChainRush
}

public class Unit_Boss : Unit_Enemy
{
    public float Prepare_Time = 1.0f; // ���� �غ� �ð�
    public float Rush_Duration = 2.0f; // ���� ���� �ð�
    public float Cool_Down_Time = 10.0f; // ���� �� ��ٿ� �ð�

    public BossState Current_State = BossState.Idle;
    public float State_Timer = 0.0f;
    public Vector3 Rush_Direction;

    public float Chain_Rush_Cooldown = 30.0f; // ���� ���� ��ٿ� �ð�

    public bool Rush_On = false;
    public bool Chain_Rush_On = false;

    public int Chain_Rush_Count = 0; // ���� ���� Ƚ��
    public int Max_Chain_Rush_Count = 5; // ���� ���� �ִ� Ƚ��
    public Vector3 Chain_Rush_Dir; // ���� ���� ����

    public override void Start()
    {
        base.Start();
        Current_State = BossState.Move;
        _unitSpriteRenderer.material.SetFloat("_Outline", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                if (isDead == false)
                {
                    Check_Boss_HP();

                    Update_Boss_State();
                }
            }
        }
    }

    public override void Hitted(float p_Damage_Value)
    {
        base.Hitted(p_Damage_Value);

        Check_Boss_HP();
    }

    public override void Die()
    {
        base.Die();
        All_Manager.instance.GameMode_M.GameClear();
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
        base.Unit_Clear();
    }

    public override void Unit_Stat_Setting(Unit unit, STO_Unit Un_STO)
    {
        base.Unit_Stat_Setting(unit, Un_STO);
    }

    // ���� ������
    public void Update_Boss_State()
    {
        State_Timer -= Time.deltaTime;

        // ���� ����
        switch (Current_State)
        {
            case BossState.Move:
                {
                    Move_Towards_Player();

                    if (State_Timer <= 0.0f)
                    {
                        if (Chain_Rush_On == true)
                        {
                            Current_State = BossState.ChainRush;
                            State_Timer = Chain_Rush_Cooldown;
                            Set_Chain_Rush_Dir();
                        }
                        else
                        {
                            Current_State = BossState.PrepareRush;
                            State_Timer = Prepare_Time;
                        }
                    }
                }
                break;
            case BossState.PrepareRush:
                {
                    // ���� �غ� ���¿����� ������ �����.
                    if (State_Timer <= 0.0f)
                    {
                        Rush_Direction = (All_Manager.instance.GameMode_M.Current_Player.transform.position - this.transform.position).normalized;
                        Current_State = BossState.Rush;
                        State_Timer = Rush_Duration;
                    }
                }
                break;
            case BossState.Rush:
                {
                    Rush_Towards_Player(Rush_Direction);

                    if (State_Timer <= 0.0f)
                    {
                        Current_State = BossState.Cooldown;
                        State_Timer = Cool_Down_Time;
                    }
                }
                break;
            case BossState.Cooldown:
                {
                    // ���� �� ��ٿ� ����
                    if (State_Timer <= 0.0f)
                    {
                        Current_State = BossState.Move;
                    }
                }
                break;
            case BossState.ChainRush:
                {
                    Rush_Towards_Player(Chain_Rush_Dir);

                    if (State_Timer <= 0.0f)
                    {
                        Chain_Rush_Count++;

                        if (Chain_Rush_Count >= Max_Chain_Rush_Count)
                        {
                            Current_State = BossState.Cooldown;
                            State_Timer = Cool_Down_Time;
                        }
                        else
                        {
                            Set_Chain_Rush_Dir(); // ���� ���� ���� ����
                        }
                    }
                }
                break;
        }
    }

    // �÷��̾� ����
    public void Move_Towards_Player()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                Vector3 dir = All_Manager.instance.GameMode_M.Current_Player.transform.position - this.transform.position;

                this.transform.position = this.transform.position + (dir.normalized * (Unit_Enemy_Speed * Time.deltaTime));

                if (All_Manager.instance.GameMode_M.Current_Player.transform.position.x < this.transform.position.x)
                {
                    _unitSpriteRenderer.flipX = false;
                }

                else
                {
                    _unitSpriteRenderer.flipX = true;
                }
            }

        }
    }

    // ���� ����
    public void Rush_Towards_Player(Vector3 Dir)
    {
        this.transform.position = this.transform.position + (Dir * (Unit_Enemy_Speed * 5.0f) * Time.deltaTime);

        // ���� �߿��� ���� ��ȯ�� ���� �ʴ´�.
        if (Dir.x < 0)
        {
            _unitSpriteRenderer.flipX = false;
        }
        else
        {
            _unitSpriteRenderer.flipX = true;
        }
    }

    // ���� ���� ���� ���� ���� (������� ����)
    public void Set_Chain_Rush_Dir()
    {
        // �÷��̾��� ��ġ�� ������� ���� ����
        Vector3 Player_Dir = (All_Manager.instance.GameMode_M.Current_Player.transform.position - this.transform.position).normalized;

        // ���� ���� ������ �������� ������� ���� ����
        if (Chain_Rush_Count % 2 == 0)
        {
            // ������ ���� ����
            Chain_Rush_Dir = new Vector3(Player_Dir.x + 1.0f, Player_Dir.y + 1.0f).normalized;
        }
        else
        {
            // ���� �Ʒ��� ����
            Chain_Rush_Dir = new Vector3(Player_Dir.x - 1.0f, Player_Dir.y - 1.0f).normalized;
        }

        State_Timer = Rush_Duration / Max_Chain_Rush_Count; // ª�� �Ÿ� ����
    }

    // ���� ü�� �Ǵ� �� ���� Ȱ��ȭ
    public void Check_Boss_HP()
    {
        var Boss_HP = (this.Current_HP / this.Max_HP) * 100;

        if (Boss_HP <= 60 && Chain_Rush_On == false)
        {
            Chain_Rush_On = true;
        }
    }

    //public override void OnTriggerStay2D(Collider2D collision)
    //{
    //    base.OnTriggerStay2D(collision);
    //}

    public override void Unit_Animation(UnitAnimationState State)
    {
        base.Unit_Animation(State);
    }

    public override void Enemy_To_Player_Collision()
    {
        base.Enemy_To_Player_Collision();
    }

    public override void Enemy_To_Enemy_Push()
    {
        base.Enemy_To_Enemy_Push();
    }
}
