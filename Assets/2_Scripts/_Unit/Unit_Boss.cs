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
    public float Prepare_Time = 1.0f; // 돌진 준비 시간
    public float Rush_Duration = 2.0f; // 돌진 지속 시간
    public float Cool_Down_Time = 10.0f; // 돌진 후 쿨다운 시간

    public BossState Current_State = BossState.Idle;
    public float State_Timer = 0.0f;
    public Vector3 Rush_Direction;

    public float Chain_Rush_Cooldown = 30.0f; // 연속 돌진 쿨다운 시간

    public bool Rush_On = false;
    public bool Chain_Rush_On = false;
    
    public int Chain_Rush_Count = 0; // 연속 돌진 횟수
    public int Max_Chain_Rush_Count = 5; // 연속 돌진 최대 횟수
    public Vector3 Chain_Rush_Dir; // 연속 돌진 방향

    public override void Start()
    {
        base.Start();
        Current_State = BossState.Move;
        SR.material.SetFloat("_Outline", 0.0f);
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
        All_Manager.instance.GameMode_M.GM_State = GameMode_State.GameClear;
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

    // 보스 움직임
    public void Update_Boss_State()
    {
        State_Timer -= Time.deltaTime;

        // 보스 패턴
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
                    // 돌진 준비 상태에서는 보스가 멈춘다.
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
                    // 돌진 후 쿨다운 상태
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
                            Set_Chain_Rush_Dir(); // 다음 돌진 방향 설정
                        }
                    }
                }
                break;
        }
    }

    // 플레이어 추적
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
                    SR.flipX = false;
                }

                else
                {
                    SR.flipX = true;
                }
            }

        }
    }

    // 보스 돌진
    public void Rush_Towards_Player(Vector3 Dir)
    {
        this.transform.position = this.transform.position + (Dir * (Unit_Enemy_Speed * 5.0f) * Time.deltaTime);

        // 돌진 중에는 방향 전환을 하지 않는다.
        if (Dir.x < 0)
        {
            SR.flipX = false;
        }
        else
        {
            SR.flipX = true;
        }
    }

    // 다음 연속 돌진 방향 설정 (지그재그 패턴)
    public void Set_Chain_Rush_Dir()
    {
        // 플레이어의 위치를 기반으로 방향 설정
        Vector3 Player_Dir = (All_Manager.instance.GameMode_M.Current_Player.transform.position - this.transform.position).normalized;

        // 현재 돌진 방향을 기준으로 지그재그 방향 설정
        if (Chain_Rush_Count % 2 == 0)
        {
            // 오른쪽 위로 돌진
            Chain_Rush_Dir = new Vector3(Player_Dir.x + 1.0f, Player_Dir.y + 1.0f).normalized;
        }
        else
        {
            // 왼쪽 아래로 돌진
            Chain_Rush_Dir = new Vector3(Player_Dir.x - 1.0f, Player_Dir.y - 1.0f).normalized;
        }

        State_Timer = Rush_Duration / Max_Chain_Rush_Count; // 짧은 거리 돌진
    }

    // 보스 체력 판단 후 패턴 활성화
    public void Check_Boss_HP()
    {
        var Boss_HP = (this.Current_HP / this.Max_HP) * 100;

        if (Boss_HP <= 60 && Chain_Rush_On == false)
        {
            Chain_Rush_On = true;
        }
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

    public override void Unit_Animation(Unit_Anim_State State)
    {
        base.Unit_Animation(State);
    }

}
