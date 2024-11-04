using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public struct UnitStatSheet
{
    public float UnitHP;
    public float UnitSpeed;
    public float UnitDefense;
    public float UnitWeaponDamage;
    public float UnitWeaponProjSpeed;
    public float UnitWeaponDuration;
    public float UnitWeaponRange;
    public float UnitWeaponCoolDown;
    public int UnitWeaponProjCount;
    public float UnitGetGemRange;
    public float UnitWeaponSize;
}

public abstract class Unit : MonoBehaviour, IUnitType, IUnitAnimationState
{
    [SerializeField] protected SpriteRenderer unitSpriteRenderer;
    public SpriteRenderer UnitSpriteRenderer { get { return unitSpriteRenderer; } set { unitSpriteRenderer = value; } }

    [SerializeField] protected BoxCollider2D unitBoxCollider2D;
    public BoxCollider2D UnitBoxCollider2D { get { return unitBoxCollider2D; } set { unitBoxCollider2D = value; } }

    [SerializeField] protected Animator unitAnimator;
    public Animator UnitAnimator { get { return unitAnimator; } set { unitAnimator = value; } } 

    public bool isDead = false;
    public bool isAlive = false;
    public bool isEffect = false;
    public bool isHit = false;
    public bool isDeathAnim = false;

    public UnitStatSheet OriginStat;
    public UnitStatSheet PercentStat;

    [SerializeField] public UnitType UnitType { get; set; }
    [SerializeField] public UnitAnimationState UnitAnimationState { get; set; }

    public float UnitEnemyHP = 0f;
    public float UnitEnemySpeed = 0f;
    public float UnitEnemyDamage = 0f;

    public float MaxHP = 0f;
    public float CurrentHP = 0f;

    public float KnockTimer = 0f;
    //public float spriteRendererGB = 0f;
    public float HitTime = 0.0f;

    public float DeadEffect = 0.0f;

    protected bool isKnock = false;
    private Vector3 knockDirection; 
    private float knockStrength;    
    private float knockTimer;       
    private float knockDuration;

    void Awake()
    {
        // Awake를 생성자 대용으로 사용하셈. Start는 생성되자마자 호출되지 않음.
        unitSpriteRenderer = this.AddComponent<SpriteRenderer>();

        unitBoxCollider2D = this.AddComponent<BoxCollider2D>();
        unitBoxCollider2D.isTrigger = true;

        unitAnimator = this.AddComponent<Animator>();
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        // ������ ������ ���¶�� �Լ� ����
        if (AllManager.instance.GameModeM.GameStop) return;

        // SpriteRenderer ���� ������Ʈ
        UpdateSpriteRenderer();

        // �˹� ó��
        if (isKnock == true)
        {
            PerformKnockback();
        }

        // �ǰ� ȿ�� ó��
        // Hit ���� �ð� ��� ó��
        if (!isHit)
        {
            UpdateHitTimer();
        }
    }

    private void UpdateSpriteRenderer()
    {
        unitSpriteRenderer.color = new Color(1, 1 - HitTime, 1 - HitTime, 1 - DeadEffect);
        unitSpriteRenderer.material.SetFloat("RedValue", 1 - HitTime);
        unitSpriteRenderer.material.SetFloat("Alpha", 1 - DeadEffect);
    }

    private void PerformKnockback()
    {
        if (isKnock)
        {
            if (knockTimer < knockDuration)
            {
                float knockProgress = knockTimer / knockDuration;
                float currentPower = knockStrength * (1f - Mathf.Pow(knockProgress, 2));

                transform.position += knockDirection * currentPower * Time.deltaTime;

                knockTimer += Time.deltaTime;
            }
            else
            {
                isKnock = false;
            }
        }
    }

    private void UpdateHitTimer()
    {
        // �ǰ� ���� ȸ�� �ð��� ���
        HitTime -= Time.deltaTime;

        if (HitTime < 0.0f)
        {
            //HitTime = 1.0f;
            isHit = true;
        }
    }

    public virtual void Hitted(float pDamageValue)
    {
        if (isHit == true)
        {
            HitTime = 1.0f;

            var TempDefense = 100f / (100f + this.OriginStat.UnitDefense) * pDamageValue;

            AllManager.instance.AudioM.UnitHittedEffectAudioPlay(this.UnitType);

            CurrentHP -= (TempDefense);

            var TempShaderAlpha = this.unitSpriteRenderer.material.GetFloat("Alpha");
            var TempColorAlpha = this.unitSpriteRenderer.color.a;

            isHit = false;
            
            //CheckDead();
            StartCoroutine(CheckDeadWithDelay());
        }

    }

    protected virtual IEnumerator CheckDeadWithDelay()
    {
        // Wait for hit effect to complete
        yield return new WaitForSeconds(HitTime);

        // Check if the unit is dead
        CheckDead();
    }

    protected virtual bool CheckDead()
    {
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
            Die();
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        unitAnimator.SetBool("Move", false);
        unitAnimator.SetBool("Death", true);

        //spriteRendererGB = 0f;
        HitTime = 0.0f;

        AllManager.instance.AudioM.UnitDeathEffectAudioPlay(UnitType);

        DeathAnimationEnd();
    }

    protected virtual void DeathAnimationEnd()
    {
        StartCoroutine(CDeathAnimationEnd());
    }

    protected virtual IEnumerator CDeathAnimationEnd()
    {
        yield return new WaitForSeconds(1.6f);

        ReturnPool();
    }

    protected virtual void ReturnPool()
    {
        this.gameObject.SetActive(false);
    }

    public abstract void UnitClear();

    public void KnockBack(Vector3 KnockDir, float KnockPower, float KnockDuration)
    {
        if (!isKnock)
        {
            this.knockDirection = KnockDir.normalized;
            this.knockStrength = KnockPower;
            this.knockTimer = 0.0f;
            this.knockDuration = KnockDuration;
            this.isKnock = true;
        }
    }

    public abstract void UnitStatSetting(Unit pUnit, UnitData UnSTO);

    public virtual void UnitAnimation(UnitAnimationState State)
    {

        switch (State)
        {
            case UnitAnimationState.Idle:
                {
                    Debug.Log("Idle �ٲ�");
                    unitAnimator.SetBool("Idle", true);
                    unitAnimator.SetBool("Move", false);
                    unitAnimator.SetBool("Death", false);
                }
                break;
            case UnitAnimationState.Walk:
                {
                    Debug.Log("Walk �ٲ�");
                    unitAnimator.SetBool("Idle", false);
                    unitAnimator.SetBool("Move", true);
                }
                break;
            case UnitAnimationState.Death:
                {
                    Debug.Log("Death �ٲ�");
                    unitAnimator.SetBool("Idle", false);
                    unitAnimator.SetBool("Death", true);
                }
                break;
        }
    }
}


#region
// �˹� ���� ��, Ÿ�̸Ӹ� ���ҽ�Ű�鼭 �˹� ó��
//if (knockTimer > 0)
//{
//    float currentKnockStrength = Mathf.Lerp(knockStrength, 0, 1 - (knockTimer / knockStrength));
//    transform.position += knockDirection * currentKnockStrength * Time.deltaTime;

//    knockTimer -= Time.deltaTime;
//}
//else
//{
//    // �˹� ����
//    isKnock = false;
//}

//public void KnockBack(Vector3 knockDir, float knockStrength, float knockDuration)
//{
//    if (isKnock == false)
//    {
//        this.knockDirection = knockDir.normalized;
//        this.knockStrength = knockStrength;
//        this.knockTimer = knockDuration;
//        this.isKnock = true;
//    }
//}
//public void KnockBack(Vector3 KnockDir, float KnockForce, float KnockDuration)
//{
//    if (!isKnock)
//    {
//        for (int i = 0; i < 100; i++)
//        {
//            this.transform.position = this.transform.position + ((KnockDir.normalized * KnockForce) * (100 - i) * 0.01f * Time.deltaTime);
//        }

//        isKnock = true;
//        KnockTimer = KnockDuration;

//    }
//}

//if (!isKnock)
//{
//    for (int i = 0; i < KnockCount; i++)
//    {
//        this.transform.position = this.transform.position + (KnockDir.normalized * KnockForce * (KnockCount - i) * 0.05f);

//    }

//    isKnock = true;
//    KnockTimer = KnockDuration;
//}

//for (int i = 0; i < KnockCount; i++)
//{
//    this.transform.position = this.transform.position + (KnockDir.normalized * KnockForce * (KnockCount - i) * KnockDuration);
//}

//    public virtual void Update()
//{
//    if (AllManager.instance.GameModeM.GameStop == true) return;

//    spriteRenderer.color = new Color(1, 1 - spriteRendererGB, 1 - spriteRendererGB, 1 - DeadEffect);

//    spriteRenderer.material.SetFloat("RedValue", 1 - spriteRendererGB);

//    spriteRenderer.material.SetFloat("Alpha", 1 - DeadEffect);

//    //if (isKnock == true)
//    //{
//    //    KnockTimer -= Time.deltaTime;

//    //    if (KnockTimer < 0)
//    //    {
//    //        KnockTimer = 0;

//    //        isKnock = false;
//    //    }
//    //}

//    // �˹� ���� ��, �� �����Ӹ��� �˹� ó��
//    if (isKnock)
//    {
//        if (knockTimer > 0)
//        {
//            // �˹� ó��: ���� �پ��� ������ �з���
//            float currentKnockStrength = Mathf.Lerp(knockStrength, 0, 1 - (knockTimer / knockStrength));
//            transform.position += knockDirection * currentKnockStrength * Time.deltaTime;

//            // Ÿ�̸� ����
//            knockTimer -= Time.deltaTime;
//        }
//        else
//        {
//            // �˹� ����
//            isKnock = false;
//        }
//    }

//    if (isHitC == true)
//    {
//        spriteRendererGB -= Time.deltaTime;

//        if (spriteRendererGB < 0)
//        {
//            isHitC = false;
//        }
//    }

//    if (isHit == false)
//    {
//        if (HitTime > 1.0f)
//        {
//            HitTime = 0;

//            isHit = true;
//        }

//        HitTime += Time.deltaTime;
//    }

//    //// �˹� ���� �� ó��
//    //if (isKnockback)
//    //{
//    //    if (knockbackElapsed < knockbackTime)
//    //    {
//    //        // �˹� �������� �̵�
//    //        transform.position += knockbackDirection * Time.deltaTime;
//    //        knockbackElapsed += Time.deltaTime;
//    //    }
//    //    else
//    //    {
//    //        // �˹� ����
//    //        isKnockback = false;
//    //    }
//    //}

//}
#endregion