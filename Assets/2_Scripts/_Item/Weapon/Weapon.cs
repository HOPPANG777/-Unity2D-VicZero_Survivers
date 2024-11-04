using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public abstract class Weapon : MonoBehaviour, IWeaponKeywords
{

    public WeaponKeyword WeaponKeyword { get; set; }
    public WeaponType weaponType;

    [SerializeField] protected SpriteRenderer weaponSpriteRenderer;
    public SpriteRenderer WeaponSpriteRenderer { get {  return weaponSpriteRenderer; } set { weaponSpriteRenderer = value; } }

    [SerializeField] protected BoxCollider2D weaponBoxCollider2D;
    public BoxCollider2D WeaponBoxCollider2D { get { return weaponBoxCollider2D; } set { weaponBoxCollider2D = value; } }

    [SerializeField] protected TrailRenderer weaponTrailRenderer;
    public TrailRenderer WeaponTrailRenderer { get { return weaponTrailRenderer; } set { weaponTrailRenderer = value; } }

    [SerializeField] protected Animator weaponAnimator;
    public Animator WeaponAnimator { get { return weaponAnimator; } set { weaponAnimator = value; } }

    public Unit Mine;

    public UnitEnemy WeaponTargetWeapon;

    public float WeaponDamage = 0;
    public float WeaponSpeed = 0;
    public float WeaponRange = 0;
    public float WeaponTime = 0;
    public float WeaponPen = 0;
    public float WeaponCoolDown = 0;
    public float WeaponDuration = 0;
    public float WAccumulate = 0;
    public float WeaponKnock = 0;
    public float WeaponSize = 0;
    public int WeaponLevel = 0;
    public int WeaponCount = 0;

    public float WeaponBasicDamage = 0;
    public float WeaponBasicSpeed = 0;
    public float WeaponBasicRange = 0;
    public float WeaponBasicDuration = 0;

    public int CollisionCount = 0;
    public float ClosetDistance = 0;

    private float CheckInterval = 0.5f;
    private float NextCheckTime = 0.0f;

    public bool Forever = false;

    public bool IsKeywordCounted { get; set; } = false;

    public virtual void Start()
    {
        weaponSpriteRenderer = this.AddComponent<SpriteRenderer>();

        weaponBoxCollider2D = this.AddComponent<BoxCollider2D>();
        weaponBoxCollider2D.isTrigger = true;

        weaponTrailRenderer = this.AddComponent<TrailRenderer>();
        weaponTrailRenderer.enabled = false;

        weaponAnimator = this.AddComponent<Animator>();
    }

    public void WeaponInit(Unit pUser, WeaponData pWP, int pLevel)
    {
        Mine = pUser;

        WeaponLevel = pLevel;

        WeaponStat(WeaponLevel, this, pWP);
    }

    public virtual void Update()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer == null)
        {
            DestroyWeapon();
        }
        else
        {
            WeaponStatAppliaction();

            WeaponToEnemyCollision();

            NextCheckTime = Time.time + CheckInterval;
        }

    }

    // 업그레이드
    public abstract void Upgrade(int pLevel);

    // 무기 파괴 타이머
    public void WeaponTimer()
    {
        if (WeaponTime > WeaponDuration)
        {
            WeaponTime = 0;

            DestroyWeapon();
        }

        WeaponTime += Time.deltaTime;
    }

    // 베지어 곡선을 쓰는 무기들을 위한 타이머
    public void WeaponAccumulate()
    {
        if (WAccumulate < 1.0f)
        {
            WAccumulate += (WeaponSpeed * Time.deltaTime);
        }
        else if (WAccumulate > 1.0f)
        {
            DestroyWeapon();
        }
    }

    public virtual void WeaponToEnemyCollision()
    {
        var EnemyList = AllManager.instance.GameModeM.EnemyList;
        var Weapon = this;

        // 무기 위치를 변수로 저장하여 반복 사용
        Vector2 WeaponPosition = Weapon.transform.position;

        // 무기 타입에 따른 STO 및 무기 범위 관련 변수 미리 계산
        var TempweaponTypeO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(weaponType);
        var TempWALT = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeO);
        float MinDistance = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponSize(weaponType, TempWALT) * 0.5f;

        // MinDistance를 제곱하여 루트 계산 방지 (성능 최적화)
        float MinDistanceSqr = (MinDistance * MinDistance);

        // 무기에 의한 적의 타격 처리 여부를 저장할 리스트
        List<UnitEnemy> enemiesHit = new List<UnitEnemy>();

        for (int i = 0; i < EnemyList.Count; i++)
        {
            // 무기와 적 간의 거리 계산
            Vector2 Dir = WeaponPosition - (Vector2)EnemyList[i].transform.position;

            // 거리의 제곱을 계산하여 비교 (루트 계산 방지)
            float DisSqr = Dir.sqrMagnitude;

            if (DisSqr < MinDistanceSqr)
            {
                UnitEnemy TempEnemy = EnemyList[i].transform.GetComponent<UnitEnemy>();

                if (!enemiesHit.Contains(TempEnemy))
                {
                    // 적에게 피해 입히기
                    CollisionCount++;
                    TempEnemy.Hitted(WeaponDamage);
                    enemiesHit.Add(TempEnemy); // 해당 적이 이미 타격 처리되었음을 기록

                    // 무기가 영구적인 것이 아닌 경우만 충돌 횟수 및 무기 파괴 로직 처리
                    if (Forever == false)
                    {
                        if (CollisionCount >= WeaponPen)
                        {
                            DestroyWeapon();
                            return; // 무기를 파괴하고 함수 종료
                        }
                    }
                }
            }
        }
    }

    // 무기 넉백
    public void WeaponKnockBackWeapon()
    {
        // 모든 적 유닛을 가져와 충돌 검사
        var EnemyList = AllManager.instance.GameModeM.EnemyList;
        var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;
        var TempweaponTypeO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(weaponType);
        var TempWLAT = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeO);

        // 넉백 범위 설정
        float KnockBackRange = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponSize(weaponType, TempWLAT) * 0.5f; // 넉백 범위 (필요에 따라 조정 가능)
        float KnockBackRangeSqr = (KnockBackRange * KnockBackRange); // 거리 비교 최적화 (루트 계산 방지)

        for (int i = 0; i < EnemyList.Count; i++)
        {
            UnitEnemy Enemy = EnemyList[i];

            if (Enemy == this || Enemy.UnitType == UnitType.Barrel)
            {
                continue; // 자기 자신이거나 특정 타입은 건너뜀
            }

            // 적과 무기 간의 거리 계산
            Vector3 Dir = this.transform.position - Enemy.transform.position;
            float DisSqr = Dir.sqrMagnitude;

            // 넉백 범위 내의 적에 대해서만 넉백 적용
            if (DisSqr <= KnockBackRangeSqr)
            {
                Vector3 direction = (Enemy.transform.position - this.transform.position).normalized;
                Enemy.KnockBack(direction, WeaponKnock, WeaponDuration);
            }
        }
    }

    // 무기 기본 스탯 조정
    public void WeaponStat(int pLevel, Weapon weapon, WeaponData weaponSTO)
    {
        //Debug.Log("스프라이트 삽입");

        weapon.weaponSpriteRenderer.sprite = weaponSTO.SpriteImage;
        weapon.weaponType = weaponSTO.weaponType;
        weapon.WeaponKeyword = weaponSTO.WeaponKeywords;

        if (weaponSTO.WeaponAnimatorController != null)
        {
            weapon.weaponAnimator.runtimeAnimatorController = weaponSTO.WeaponAnimatorController;
            weapon.weaponAnimator.speed = 1.0f;
            weapon.weaponAnimator.cullingMode = AnimatorCullingMode.CullCompletely;
        }

        weapon.WeaponDamage = weaponSTO.Infos[pLevel].WeaponDamage;
        weapon.WeaponSpeed = (weaponSTO.Infos[pLevel].WeaponSpeed * 1.5f);
        weapon.WeaponRange = weaponSTO.Infos[pLevel].WeaponRange;
        weapon.WeaponTime = weaponSTO.Infos[pLevel].WeaponTime;
        weapon.WeaponCount = weaponSTO.Infos[pLevel].WeaponCount;
        weapon.WeaponPen = weaponSTO.Infos[pLevel].WeaponPen;
        weapon.WeaponCoolDown = weaponSTO.Infos[pLevel].WeaponCoolDown;
        weapon.WeaponDuration = weaponSTO.Infos[pLevel].WeaponDuration;
        weapon.WeaponKnock = weaponSTO.Infos[pLevel].WeaponKnock;
        weapon.WeaponSize = weaponSTO.Infos[pLevel].WeaponSize;
        weapon.Forever = weaponSTO.Forever;

        var TempWLAT = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(weaponSTO);

        var TempWeaponSize = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponSize(weaponType, TempWLAT);

        weapon.GetComponent<Weapon>().weaponSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        weapon.GetComponent<Weapon>().weaponSpriteRenderer.size = new Vector2((TempWeaponSize), (TempWeaponSize));
        weapon.weaponBoxCollider2D.size = new Vector2((weaponSpriteRenderer.size.x * 0.5f), (weaponSpriteRenderer.size.y * 0.5f));

        // 설정된 키워드를 SynergyManager에 등록한다.
        SynergyManager.instance.RegisterWeaponKeyword(weapon.WeaponKeyword, weapon);
    }


    public virtual void WeaponStatAppliaction()
    {
        var TempweaponTypeO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(weaponType);

        var TempWLAT = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeO);

        var TempWeaponDamage = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponDamage(weaponType, TempWLAT);
        var TempWeaponSpeed = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponSpeed(weaponType, TempWLAT);
        var TempWeaponRange = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponRange(weaponType, TempWLAT);
        var TempWeaponSize = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponSize(weaponType, TempWLAT);
        var TempWeaponDuration = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponDuration(weaponType, TempWLAT);

        WeaponDamage = TempWeaponDamage;
        WeaponSpeed = TempWeaponSpeed;
        WeaponRange = TempWeaponRange;
        WeaponDuration = TempWeaponDuration;
        WeaponSize = TempWeaponSize;
    }


    public void WeaponTrail()
    {
        // Trail Renderer 설정
        weaponTrailRenderer.time = 1.0f; // 꼬리가 지속되는 시간
        weaponTrailRenderer.startWidth = 0.05f; // 꼬리의 시작 너비
        weaponTrailRenderer.endWidth = 0.01f; // 꼬리의 끝 너비

        // 재질 설정
        weaponTrailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        weaponTrailRenderer.startColor = Color.white; // 꼬리의 시작 색상
        weaponTrailRenderer.endColor = Color.white; // 꼬리의 끝 색상

        // 추가 설정 (필요에 따라 변경 가능)
        weaponTrailRenderer.minVertexDistance = 0.1f; // 최소 꼭짓점 간 거리
        weaponTrailRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.2f),
            new Keyframe(0.5f, 0.0f)
        ); // 너비 변화 곡선
        weaponTrailRenderer.colorGradient = new Gradient()
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
    public void DestroyWeapon()
    {
        // 만약 이 Mine으로 안된다면 다 CurrentPlayer로 통합해야 한다.
        //if (AllManager.instance.GameModeM.CurrentPlayer == null)
        //{
        //    Destroy(this.gameObject);
        //}

        //Debug.Log("Weapon Destroy");

        // 만약 Mine이 null 이라면
        if (AllManager.instance.GameModeM.CurrentPlayer == null)
        {
            //Destroy(this.gameObject);

            AllManager.instance.PoolM.WeaponPoolIn(this.gameObject.GetComponent<Weapon>());
            SynergyManager.instance.UnregisterWeaponKeyword(WeaponKeyword, this);
        }

        // Mine이 null이 아니라면
        else
        {
            if (AllManager.instance.GameModeM.CurrentPlayer.AllWeapon.Contains(this))
            {
                AllManager.instance.GameModeM.CurrentPlayer.AllWeapon.Remove(this);
                SynergyManager.instance.UnregisterWeaponKeyword(WeaponKeyword, this);
                //Debug.Log("Weapon Destroy");
            }
        }

        //Destroy(this.gameObject);

        AllManager.instance.PoolM.WeaponPoolIn(this.gameObject.GetComponent<Weapon>());
    }



}


#region
// 무기가 적과 충돌했을때 // 피격처리
//public virtual void OnTriggerStay2D(Collider2D collision)
//{
//    //충돌체가 이 컴포넌트를 가지고 있는가?
//    if (collision.transform.GetComponent<UnitEnemy>())
//    {
//        //collision.transform.GetComponent<UnitEnemy>().Hitted(WeaponDamage);

//        UnitEnemy TempEnemy = collision.transform.GetComponent<UnitEnemy>();

//        //Temp.Hitted(WeaponDamageValue);

//        TempEnemy.Hitted(WeaponDamage);

//        if (Forever == false)
//        {
//            if (CollisionCount > WeaponPen)
//            {
//                DestroyWeapon();
//            }
//        }
//        else
//        {

//        }

//        //Debug.Log("Enemey Hitted");

//        CollisionCount++;

//    }

//}
//    public void WeaponKnockBackWeapon()
//{
//    Vector3 boxCollider2DCenter = boxCollider2D.transform.TransformPoint(boxCollider2D.offset);
//    Vector3 boxCollider2DHalfExtents = Vector3.Scale(boxCollider2D.size, boxCollider2D.transform.lossyScale) * 0.5f;
//    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxCollider2DCenter, boxCollider2D.size * 0.9f, 0);
//    //Debug.Log(collider2Ds);

//    float Dis = 10000;

//    foreach (UnityEngine.Collider2D collider in collider2Ds)
//    {
//        if (collider.gameObject.GetComponent<UnitEnemy>())
//        {
//            UnitEnemy EnemyTemp = collider.gameObject.GetComponent<UnitEnemy>();

//            if (Dis > Vector3.Distance(this.transform.position, EnemyTemp.transform.position))
//            {
//                Dis = Vector3.Distance(this.transform.position, EnemyTemp.transform.position);
//                WeaponTargetWeapon = EnemyTemp;
//            }

//            //WeaponTargetWeapon = EnemyTemp;
//        }
//    }

//    if (WeaponTargetWeapon != null)
//    {
//        Vector3 Dir = (WeaponTargetWeapon.transform.position - this.transform.position).normalized;

//        if (WeaponTargetWeapon.UnitType == UnitType.Barrel)
//        {

//        }

//        else
//        {
//            WeaponTargetWeapon.KnockBack(Dir, WeaponKnock, WeaponDuration);
//        }
//    }
//}
/*
도끼같은건 보통 관통력을 지니고 있다 룬트레이서도 마찬가지
따라서 WeaponPen을 추가했는데 
유닛이나 웨폰에 충돌할때마다 Pen을 감소시키거나 혹은 변수하나 만들어서 
하나씩 증가시키고 변수의 값이 Pen보다 크거나 같다면 
그때 Forever를 false로 만들면 된다.
*/

// 무기 스탯 가중치
//float WeaponCoolDownValue = 0;
//WeaponCoolDownValue = Mine.OriginStat.UnitWeaponCoolDown + (Mine.OriginStat.UnitWeaponCoolDown * Mine.PercentStat.UnitWeaponCoolDown * 0.01f);

//WeaponCoolDown -= WeaponCoolDownValue;

//for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//{
//    var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//    var TempCount = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponCount;

//    AllManager.instance.GameModeM.CurrentPlayer.WeaponPublicCount = TempCount + WeaponCountValue;
//}
// 무기 오리진 스탯에 퍼센티지 스탯 적용
//public void WeaponApplicationAccessory()
//{
//    float WeaponDamageValue = 0;
//    WeaponDamageValue = Mine.OriginStat.UnitWeaponDamage + (Mine.OriginStat.UnitWeaponDamage * Mine.PercentStat.UnitWeaponDamage * 0.01f);

//    for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    {
//        var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//        var TempDamage = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponDamage;

//        TempDamage += WeaponDamageValue;
//    }

//    float WeaponSpeedValue = 0;
//    WeaponSpeedValue = Mine.OriginStat.UnitWeaponProjSpeed + (Mine.OriginStat.UnitWeaponProjSpeed * Mine.PercentStat.UnitWeaponProjSpeed * 0.01f);

//    for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    {
//        var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//        var TempSpeed = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponSpeed;

//        TempSpeed += WeaponSpeedValue;
//    }

//    float WeaponRangeValue = 0;
//    WeaponRangeValue = Mine.OriginStat.UnitWeaponRange + (Mine.OriginStat.UnitWeaponRange * Mine.PercentStat.UnitWeaponRange * 0.01f);

//    for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    {
//        var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//        var TempRange = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponRange;

//        TempRange += WeaponRangeValue;
//    }

//    float WeaponCoolDownValue = 0;
//    WeaponCoolDownValue = Mine.OriginStat.UnitWeaponCoolDown + (Mine.OriginStat.UnitWeaponCoolDown * Mine.PercentStat.UnitWeaponCoolDown * 0.01f);

//    for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    {
//        var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//        var TempCoolDown = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponCoolDown;

//        TempCoolDown -= WeaponCoolDownValue;
//    }

//    float WeaponDurationValue = 0;
//    WeaponDurationValue = Mine.OriginStat.UnitWeaponDuration + (Mine.OriginStat.UnitWeaponDuration * Mine.PercentStat.UnitWeaponDuration * 0.01f);

//    for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    {
//        var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//        var TempDuration = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponDuration;

//        TempDuration += WeaponDurationValue;
//    }

//    int WeaponCountValue = 0;
//    WeaponCountValue = Mine.OriginStat.UnitWeaponProjCount + Mine.PercentStat.UnitWeaponProjCount;

//    //WeaponCount += WeaponCountValue;

//    //for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    //{
//    //    //AllManager.instance.ResourceM.WeaponDatas[i].Infos[]
//    //    //for (int i = 0; i < )
//    //}

//    for (int i = 0; i < AllManager.instance.ResourceM.WeaponDatas.Length; i++)
//    {
//        var WLATTemp = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(AllManager.instance.ResourceM.WeaponDatas[i]).Level;

//        var TempCount = AllManager.instance.ResourceM.WeaponDatas[i].Infos[WLATTemp].WeaponCount;

//        TempCount += WeaponCountValue;
//    }
//}

////
//Vector3 Knock = Dir * WeaponSpeed;
// Ftarget.KnockBack(Dir, WeaponKnock, WeaponDuration, WeaponKncokTemp);
////
#endregion