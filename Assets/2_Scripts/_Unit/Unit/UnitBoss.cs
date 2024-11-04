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

public class UnitBoss : UnitEnemy
{
    public float PrepareTime = 1.0f; // ���� �غ� �ð�
    public float RushDuration = 2.0f; // ���� ���� �ð�
    public float CoolDownTime = 10.0f; // ���� �� ��ٿ� �ð�

    public BossState CurrentState = BossState.Idle;
    public float StateTimer = 0.0f;
    public Vector3 RushDirection;

    public float ChainRushCooldown = 30.0f; // ���� ���� ��ٿ� �ð�

    public bool RushOn = false;
    public bool ChainRushOn = false;

    public int ChainRushCount = 0; // ���� ���� Ƚ��
    public int MaxChainRushCount = 5; // ���� ���� �ִ� Ƚ��
    public Vector3 ChainRushDir; // ���� ���� ����

    public override void Start()
    {
        base.Start();
        CurrentState = BossState.Move;
        unitSpriteRenderer.material.SetFloat("Outline", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            if (AllManager.instance.GameModeM.CurrentPlayer != null)
            {
                if (isDead == false)
                {
                    CheckBossHP();

                    UpdateBossState();
                }
            }
        }
    }

    public override void Hitted(float pDamageValue)
    {
        base.Hitted(pDamageValue);

        CheckBossHP();
    }

    public override void Die()
    {
        base.Die();
        AllManager.instance.GameModeM.GameClear();
    }

    protected override void DeathAnimationEnd()
    {
        base.DeathAnimationEnd();
    }

    protected override IEnumerator CDeathAnimationEnd()
    {
        return base.CDeathAnimationEnd();
    }

    protected override void ReturnPool()
    {
        base.ReturnPool();
    }

    public override void UnitClear()
    {
        base.UnitClear();
    }

    public override void UnitStatSetting(Unit unit, UnitData UnSTO)
    {
        base.UnitStatSetting(unit, UnSTO);
    }

    // ���� ������
    public void UpdateBossState()
    {
        StateTimer -= Time.deltaTime;

        // ���� ����
        switch (CurrentState)
        {
            case BossState.Move:
                {
                    MoveTowardsPlayer();

                    if (StateTimer <= 0.0f)
                    {
                        if (ChainRushOn == true)
                        {
                            CurrentState = BossState.ChainRush;
                            StateTimer = ChainRushCooldown;
                            SetChainRushDir();
                        }
                        else
                        {
                            CurrentState = BossState.PrepareRush;
                            StateTimer = PrepareTime;
                        }
                    }
                }
                break;
            case BossState.PrepareRush:
                {
                    // ���� �غ� ���¿����� ������ �����.
                    if (StateTimer <= 0.0f)
                    {
                        RushDirection = (AllManager.instance.GameModeM.CurrentPlayer.transform.position - this.transform.position).normalized;
                        CurrentState = BossState.Rush;
                        StateTimer = RushDuration;
                    }
                }
                break;
            case BossState.Rush:
                {
                    RushTowardsPlayer(RushDirection);

                    if (StateTimer <= 0.0f)
                    {
                        CurrentState = BossState.Cooldown;
                        StateTimer = CoolDownTime;
                    }
                }
                break;
            case BossState.Cooldown:
                {
                    // ���� �� ��ٿ� ����
                    if (StateTimer <= 0.0f)
                    {
                        CurrentState = BossState.Move;
                    }
                }
                break;
            case BossState.ChainRush:
                {
                    RushTowardsPlayer(ChainRushDir);

                    if (StateTimer <= 0.0f)
                    {
                        ChainRushCount++;

                        if (ChainRushCount >= MaxChainRushCount)
                        {
                            CurrentState = BossState.Cooldown;
                            StateTimer = CoolDownTime;
                        }
                        else
                        {
                            SetChainRushDir(); // ���� ���� ���� ����
                        }
                    }
                }
                break;
        }
    }

    // �÷��̾� ����
    public void MoveTowardsPlayer()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            if (AllManager.instance.GameModeM.CurrentPlayer != null)
            {
                Vector3 dir = AllManager.instance.GameModeM.CurrentPlayer.transform.position - this.transform.position;

                this.transform.position = this.transform.position + (dir.normalized * (UnitEnemySpeed * Time.deltaTime));

                if (AllManager.instance.GameModeM.CurrentPlayer.transform.position.x < this.transform.position.x)
                {
                    unitSpriteRenderer.flipX = false;
                }

                else
                {
                    unitSpriteRenderer.flipX = true;
                }
            }

        }
    }

    // ���� ����
    public void RushTowardsPlayer(Vector3 Dir)
    {
        this.transform.position = this.transform.position + (Dir * (UnitEnemySpeed * 5.0f) * Time.deltaTime);

        // ���� �߿��� ���� ��ȯ�� ���� �ʴ´�.
        if (Dir.x < 0)
        {
            unitSpriteRenderer.flipX = false;
        }
        else
        {
            unitSpriteRenderer.flipX = true;
        }
    }

    // ���� ���� ���� ���� ���� (������� ����)
    public void SetChainRushDir()
    {
        // �÷��̾��� ��ġ�� ������� ���� ����
        Vector3 PlayerDir = (AllManager.instance.GameModeM.CurrentPlayer.transform.position - this.transform.position).normalized;

        // ���� ���� ������ �������� ������� ���� ����
        if (ChainRushCount % 2 == 0)
        {
            // ������ ���� ����
            ChainRushDir = new Vector3(PlayerDir.x + 1.0f, PlayerDir.y + 1.0f).normalized;
        }
        else
        {
            // ���� �Ʒ��� ����
            ChainRushDir = new Vector3(PlayerDir.x - 1.0f, PlayerDir.y - 1.0f).normalized;
        }

        StateTimer = RushDuration / MaxChainRushCount; // ª�� �Ÿ� ����
    }

    // ���� ü�� �Ǵ� �� ���� Ȱ��ȭ
    public void CheckBossHP()
    {
        var BossHP = (this.CurrentHP / this.MaxHP) * 100;

        if (BossHP <= 60 && ChainRushOn == false)
        {
            ChainRushOn = true;
        }
    }

    //public override void OnTriggerStay2D(Collider2D collision)
    //{
    //    base.OnTriggerStay2D(collision);
    //}

    public override void UnitAnimation(UnitAnimationState State)
    {
        base.UnitAnimation(State);
    }

    public override void EnemyToPlayerCollision()
    {
        base.EnemyToPlayerCollision();
    }

    public override void EnemyToEnemyPush()
    {
        base.EnemyToEnemyPush();
    }
}
