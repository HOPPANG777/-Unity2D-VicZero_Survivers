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

public abstract class Unit : MonoBehaviour, IUnitType, IUnitAnimationState
{
    [SerializeField] protected SpriteRenderer _unitSpriteRenderer;
    public SpriteRenderer UnitSpriteRenderer { get { return _unitSpriteRenderer; } set { _unitSpriteRenderer = value; } }

    [SerializeField] protected BoxCollider2D _unitBoxCollider2D;
    public BoxCollider2D UnitBoxCollider2D { get { return _unitBoxCollider2D; } set { _unitBoxCollider2D = value; } }

    [SerializeField] protected Animator _unitAnimator;
    public Animator UnitAnimator { get { return _unitAnimator; } set { _unitAnimator = value; } } 

    public bool isDead = false;
    public bool isAlive = false;
    public bool isEffect = false;
    public bool isHit = false;
    public bool isDeathAnim = false;

    public Unit_Stat_Sheet Origin_Stat;
    public Unit_Stat_Sheet Percent_Stat;

    [SerializeField] public UnitType UnitType { get; set; }
    [SerializeField] public UnitAnimationState UnitAnimationState { get; set; }

    public float Unit_Enemy_HP = 0f;
    public float Unit_Enemy_Speed = 0f;
    public float Unit_Enemy_Damage = 0f;

    public float Max_HP = 0f;
    public float Current_HP = 0f;

    public float Knock_Timer = 0f;
    //public float _spriteRenderer_GB = 0f;
    public float Hit_Time = 0.0f;

    public float Dead_Effect = 0.0f;

    protected bool isKnock = false;
    private Vector3 knockDirection; 
    private float knockStrength;    
    private float knockTimer;       
    private float knockDuration;

    void Awake()
    {
        // Awake를 생성자 대용으로 사용하셈. Start는 생성되자마자 호출되지 않음.
        _unitSpriteRenderer = this.AddComponent<SpriteRenderer>();

        _unitBoxCollider2D = this.AddComponent<BoxCollider2D>();
        _unitBoxCollider2D.isTrigger = true;

        _unitAnimator = this.AddComponent<Animator>();
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        // ������ ������ ���¶�� �Լ� ����
        if (All_Manager.instance.GameMode_M.Game_Stop) return;

        // SpriteRenderer ���� ������Ʈ
        Update_Sprite_Renderer();

        // �˹� ó��
        if (isKnock == true)
        {
            Perform_Knockback();
        }

        // �ǰ� ȿ�� ó��
        // Hit ���� �ð� ��� ó��
        if (!isHit)
        {
            Update_Hit_Timer();
        }
    }

    private void Update_Sprite_Renderer()
    {
        _unitSpriteRenderer.color = new Color(1, 1 - Hit_Time, 1 - Hit_Time, 1 - Dead_Effect);
        _unitSpriteRenderer.material.SetFloat("_Red_Value", 1 - Hit_Time);
        _unitSpriteRenderer.material.SetFloat("_Alpha", 1 - Dead_Effect);
    }

    private void Perform_Knockback()
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

    private void Update_Hit_Timer()
    {
        // �ǰ� ���� ȸ�� �ð��� ���
        Hit_Time -= Time.deltaTime;

        if (Hit_Time < 0.0f)
        {
            //Hit_Time = 1.0f;
            isHit = true;
        }
    }

    public virtual void Hitted(float p_Damage_Value)
    {
        if (isHit == true)
        {
            Hit_Time = 1.0f;

            var Temp_Defense = 100f / (100f + this.Origin_Stat.Unit_Defense) * p_Damage_Value;

            All_Manager.instance.Audio_M.Unit_Hitted_Effect_Audio_Play(this.UnitType);

            Current_HP -= (Temp_Defense);

            var Temp_Shader_Alpha = this._unitSpriteRenderer.material.GetFloat("_Alpha");
            var Temp_Color_Alpha = this._unitSpriteRenderer.color.a;

            isHit = false;
            
            //CheckDead();
            StartCoroutine(CheckDeadWithDelay());
        }

    }

    protected virtual IEnumerator CheckDeadWithDelay()
    {
        // Wait for hit effect to complete
        yield return new WaitForSeconds(Hit_Time);

        // Check if the unit is dead
        CheckDead();
    }

    protected virtual bool CheckDead()
    {
        if (Current_HP < 0)
        {
            Current_HP = 0;
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
        _unitAnimator.SetBool("Move", false);
        _unitAnimator.SetBool("Death", true);

        //_spriteRenderer_GB = 0f;
        Hit_Time = 0.0f;

        All_Manager.instance.Audio_M.Unit_Death_Effect_Audio_Play(UnitType);

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

    public void Knock_Back(Vector3 Knock_Dir, float Knock_Power, float Knock_Duration)
    {
        if (!isKnock)
        {
            this.knockDirection = Knock_Dir.normalized;
            this.knockStrength = Knock_Power;
            this.knockTimer = 0.0f;
            this.knockDuration = Knock_Duration;
            this.isKnock = true;
        }
    }

    public abstract void Unit_Stat_Setting(Unit p_Unit, STO_Unit Un_STO);

    public virtual void Unit_Animation(UnitAnimationState State)
    {

        switch (State)
        {
            case UnitAnimationState.Idle:
                {
                    Debug.Log("Idle �ٲ�");
                    _unitAnimator.SetBool("Idle", true);
                    _unitAnimator.SetBool("Move", false);
                    _unitAnimator.SetBool("Death", false);
                }
                break;
            case UnitAnimationState.Walk:
                {
                    Debug.Log("Walk �ٲ�");
                    _unitAnimator.SetBool("Idle", false);
                    _unitAnimator.SetBool("Move", true);
                }
                break;
            case UnitAnimationState.Death:
                {
                    Debug.Log("Death �ٲ�");
                    _unitAnimator.SetBool("Idle", false);
                    _unitAnimator.SetBool("Death", true);
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
//public void Knock_Back(Vector3 Knock_Dir, float Knock_Force, float Knock_Duration)
//{
//    if (!isKnock)
//    {
//        for (int i = 0; i < 100; i++)
//        {
//            this.transform.position = this.transform.position + ((Knock_Dir.normalized * Knock_Force) * (100 - i) * 0.01f * Time.deltaTime);
//        }

//        isKnock = true;
//        Knock_Timer = Knock_Duration;

//    }
//}

//if (!isKnock)
//{
//    for (int i = 0; i < Knock_Count; i++)
//    {
//        this.transform.position = this.transform.position + (Knock_Dir.normalized * Knock_Force * (Knock_Count - i) * 0.05f);

//    }

//    isKnock = true;
//    Knock_Timer = Knock_Duration;
//}

//for (int i = 0; i < Knock_Count; i++)
//{
//    this.transform.position = this.transform.position + (Knock_Dir.normalized * Knock_Force * (Knock_Count - i) * Knock_Duration);
//}

//    public virtual void Update()
//{
//    if (All_Manager.instance.GameMode_M.Game_Stop == true) return;

//    _spriteRenderer.color = new Color(1, 1 - _spriteRenderer_GB, 1 - _spriteRenderer_GB, 1 - Dead_Effect);

//    _spriteRenderer.material.SetFloat("_Red_Value", 1 - _spriteRenderer_GB);

//    _spriteRenderer.material.SetFloat("_Alpha", 1 - Dead_Effect);

//    //if (isKnock == true)
//    //{
//    //    Knock_Timer -= Time.deltaTime;

//    //    if (Knock_Timer < 0)
//    //    {
//    //        Knock_Timer = 0;

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

//    if (isHit_C == true)
//    {
//        _spriteRenderer_GB -= Time.deltaTime;

//        if (_spriteRenderer_GB < 0)
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