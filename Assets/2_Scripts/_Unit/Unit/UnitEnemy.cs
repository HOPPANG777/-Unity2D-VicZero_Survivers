using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

public class UnitEnemy : Unit
{
    //public UnitType UnitType;

    Dictionary<Vector2Int, List<UnitEnemy>> gridCells = new Dictionary<Vector2Int, List<UnitEnemy>>();

    public float AttackTime = 0.0f;
    public float WatingAttack = 0.0f;

    public Vector2 MoveVec;

    public float TempTime = 1.0f;

    public bool Elite = false;

    private float PushStrength = 0.5f;

    private float CheckInterval = 0.25f;
    private float LastCheckTime = 0.0f;

    private bool isSpeedBoosted = false;

    public override void Start()
    {
        base.Start();

        WatingAttack = 3.0f;
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            if (AllManager.instance.GameModeM.CurrentPlayer != null)
            {
                base.Update();
                
                if (CurrentHP > 0)
                {
                    UpdateEnemyMove();
                }

                PlayerToFarEnemy();

                EnemyToPlayerCollision();

                TempTime -= Time.deltaTime;

                AttackTime += Time.deltaTime;

            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time >= LastCheckTime + CheckInterval)
        {
            // 0.25초가 경과한 경우 충돌 검사 수행
            EnemyToEnemyPush();

            // 현재 시간을 기준으로 마지막 체크 시간을 업데이트
            LastCheckTime = Time.time;
        }
    }

    public override void Hitted(float pDamageValue)
    {
        base.Hitted(pDamageValue);
        string DamageText = pDamageValue.ToString();
        AllManager.instance.PoolM.DamageEffect.ShowDamageEffect(transform.position, DamageText);
    }

    protected override IEnumerator CheckDeadWithDelay()
    {
        return base.CheckDeadWithDelay();
    }

    protected override bool CheckDead()
    {
        return base.CheckDead();
    }

    public override void Die()
    {
        base.Die();
        if (this.UnitType != UnitType.Boss || this.UnitType != UnitType.Barrel)
        {
            AllManager.instance.GameModeM.EnemyDown(this);
        }
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
        if (this.UnitType == UnitType.Boss) return;
        if (this.UnitType == UnitType.Barrel)
        {
            AllManager.instance.GameModeM.EnemyDown(this);
        }

        AllManager.instance.PoolM.EnemyPoolIn(this);

    }

    public override void UnitClear()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }


    public void UpdateEnemyMove()
    {
        if (isDead == true) return;

        Vector3 dir = AllManager.instance.GameModeM.CurrentPlayer.transform.position - this.transform.position;

        this.transform.position = this.transform.position + (dir.normalized * (UnityEngine.Random.Range(UnitEnemySpeed, UnitEnemySpeed) * Time.deltaTime));

        if (this.UnitType == UnitType.Barrel) return;

        if (AllManager.instance.GameModeM.EnemyList.Contains(this) || gameObject.GetComponent<Animator>())
        {
            unitAnimator.SetBool("Move", true);
        }
        else
        {
            unitAnimator.SetBool("Move", false);
        }

        if (AllManager.instance.GameModeM.CurrentPlayer.transform.position.x < this.transform.position.x && isDead == false)
        {
            unitSpriteRenderer.flipX = true;
        }

        else
        {
            unitSpriteRenderer.flipX = false;
        }


    }
    public virtual void EnemyToPlayerCollision()
    {
        var Player = AllManager.instance.GameModeM.CurrentPlayer;
        var EnemyList = AllManager.instance.GameModeM.EnemyList;

        // 플레이어의 위치를 미리 계산하여 변수에 저장하여 반복 사용
        Vector2 PlayerPosition = Player.transform.position;
        float MinDistance = 0.5f; // 충돌 반경 상수를 반복할 필요 없이 상수로 선언

        for (int i = 0; i < EnemyList.Count; i++)
        {
            // 플레이어와 적의 위치 차이 벡터 계산
            Vector2 Dir = PlayerPosition - (Vector2)EnemyList[i].transform.position;

            // 거리 계산을 제곱된 형태로 유지하여 성능 최적화 (루트 계산 방지)
            float DisSqr = Dir.sqrMagnitude;
            float MinDistanceSqr = MinDistance * MinDistance;

            if (DisSqr < MinDistanceSqr)
            {
                if (AttackTime > WatingAttack)
                {
                    UnitPlayer Temp = Player.GetComponent<UnitPlayer>();

                    if (this.UnitType != UnitType.Barrel) // 불필요한 빈 조건문 제거
                    {
                        Temp.Hitted(UnitEnemyDamage);
                    }

                    AttackTime = 0.0f;
                }
            }
        }
    }

    public virtual void EnemyToEnemyPush()
    {
        var EnemyList = AllManager.instance.GameModeM.EnemyList;
        var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;
        Camera TempCamera = AllManager.instance.GameModeM.CurrentCamera;

        if (this.UnitType == UnitType.Barrel)
        {
            return;
        }

        // 화면 내에 있는 유닛들만 따로 리스트로 관리 (적 및 플레이어 포함)
        List<Unit> InViewUnitList = new List<Unit>();

        // 적 추가
        for (int i = 0; i < EnemyList.Count; i++)
        {
            Vector3 ViewPos = TempCamera.WorldToViewportPoint(EnemyList[i].transform.position);
            if (ViewPos.x >= 0 && ViewPos.x <= 1 && ViewPos.y >= 0 && ViewPos.y <= 1 && ViewPos.z >= 0)
            {
                InViewUnitList.Add(EnemyList[i]);
            }
        }

        // 플레이어 추가 (플레이어가 화면 내에 있는 경우)
        //Vector3 PlayerViewPos = TempCamera.WorldToViewportPoint(TempPlayer.transform.position);
        //if (PlayerViewPos.x >= 0 && PlayerViewPos.x <= 1 && PlayerViewPos.y >= 0 && PlayerViewPos.y <= 1 && PlayerViewPos.z >= 0)
        //{
        //    InViewUnitList.Add(TempPlayer);
        //}

        // 그리드 크기 설정 (예: 5x5 유닛 영역)
        float GridSize = 3f;
        Dictionary<Vector2Int, List<Unit>> Grid = new Dictionary<Vector2Int, List<Unit>>();

        // 각 유닛을 그리드에 할당
        for (int i = 0; i < InViewUnitList.Count; i++)
        {
            Vector2Int GridPosition = new Vector2Int(
                Mathf.FloorToInt(InViewUnitList[i].transform.position.x / GridSize),
                Mathf.FloorToInt(InViewUnitList[i].transform.position.y / GridSize)
            );

            if (!Grid.ContainsKey(GridPosition))
            {
                Grid[GridPosition] = new List<Unit>();
            }

            Grid[GridPosition].Add(InViewUnitList[i]);
        }

        // 각 그리드 셀 내의 유닛들에 대해 밀쳐내기 처리
        foreach (var Cell in Grid)
        {
            List<Unit> ClusterUnitList = Cell.Value;

            for (int i = 0; i < ClusterUnitList.Count; i++)
            {
                for (int j = i + 1; j < ClusterUnitList.Count; j++)
                {
                    Vector2 Dir = ClusterUnitList[j].transform.position - ClusterUnitList[i].transform.position;

                    // 캐싱된 거리 제곱값을 사용
                    float sqrDis = Dir.sqrMagnitude;

                    // 충돌 반경 설정
                    float MinDistance = 0.5f;
                    float minDistanceSquared = MinDistance * MinDistance;

                    if (sqrDis < minDistanceSquared)
                    {
                        // 밀쳐내기 계산
                        Vector2 pushDirection = Dir.normalized * (MinDistance - Mathf.Sqrt(sqrDis)) * PushStrength;

                        // 충돌이 발생한 경우 위치 업데이트
                        ClusterUnitList[i].transform.position -= (Vector3)pushDirection * 0.5f;
                        ClusterUnitList[j].transform.position += (Vector3)pushDirection * 0.5f;
                    }
                }
            }
        }
    }


    public void EliteOutlineSet(UnitType pUnitType)
    {
        //float SetElite = 0.05f;
        float SetNotElite = 0.0f;

        if (Elite == false)
        {
            unitSpriteRenderer.material.SetFloat("Outline", SetNotElite);
        }

        else
        {
            unitSpriteRenderer.material.SetFloat("Outline", SetNotElite);
            EliteHPSetting(pUnitType);
        }

    }

    public void EliteHPSetting(UnitType pUnitType)
    {
        if (Elite == false)
        {

        }

        else
        {
            float EliteHP = this.MaxHP;

            EliteHP = EliteHP * 3.0f;

            this.UnitEnemyHP = this.CurrentHP = MaxHP = EliteHP;
        }
    }

    // 유닛 에너미 스탯 조정
    public override void UnitStatSetting(Unit pUnit, UnitData pUnSTO)
    {
        pUnit.UnitSpriteRenderer.sprite = pUnSTO.SpriteImage;
        pUnit.UnitSpriteRenderer.material = pUnSTO.EliteMaterial;

        pUnit.MaxHP = pUnit.CurrentHP = pUnSTO.UnitEnemyHP;
        pUnit.UnitEnemyHP = pUnSTO.UnitEnemyHP;
        pUnit.UnitEnemyDamage = pUnSTO.UnitEnemyDamage;
        pUnit.UnitEnemySpeed = (pUnSTO.UnitEnemySpeed * 1.5f);

        pUnit.UnitBoxCollider2D.size = new Vector2(pUnSTO.SizeX, pUnSTO.SizeY);
        pUnit.UnitBoxCollider2D.offset = new Vector2(pUnSTO.OffsetX, pUnSTO.OffsetY);

        if (pUnSTO.UnitAnimatorController != null)
        {
            pUnit.UnitAnimator.runtimeAnimatorController = pUnSTO.UnitAnimatorController;
            pUnit.UnitAnimator.speed = 1.0f; // 애니메이션 재생
            pUnit.UnitAnimator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }

    // 너무 멀리있는 적 오브젝트 풀로 반환.
    public void PlayerToFarEnemy()
    {
        var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;

        if (TempPlayer == null)
        {
            return;
        }

        var distanceThreshold = 100.0f;
        var direction = this.transform.position - TempPlayer.transform.position;
        float distance = direction.magnitude;

        if (!AllManager.instance.GameModeM.EnemyList.Contains(this))
        {
            return;
        }

        if (distance > distanceThreshold && !isSpeedBoosted)
        {
            if (this.UnitType != UnitType.Boss)
            {
                UnitEnemySpeed *= 10.0f;
                isSpeedBoosted = true; // 속도 부스터 상태 설정
            }
        }
        else if (distance <= distanceThreshold && isSpeedBoosted)
        {
            if (this.UnitType != UnitType.Boss)
            {
                UnitEnemySpeed = AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(this.UnitType).UnitEnemySpeed * 1.5f;
                isSpeedBoosted = false; // 속도 부스터 해제
            }
        }

    }

    public override void UnitAnimation(UnitAnimationState State)
    {
        base.UnitAnimation(State);
    }

}

#region
//var target = AllManager.instance.GameModeM.CurrentPlayer.transform;

//방향 바꾸고
//float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
//this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
//public virtual void OnTriggerStay2D(Collider2D collision)
//{
//    //충돌체가 이 컴포넌트를 가지고 있는가?
//    if (collision.transform.GetComponent<UnitPlayer>())
//    {
//        if (AttackTime > WatingAttack)
//        {
//            //collision.transform.GetComponent<UnitPlayer>().Hitted(UnitEnemyDamage);

//            UnitPlayer Temp = collision.transform.GetComponent<UnitPlayer>();

//            if (this.UnitType == UnitType.Barrel)
//            {

//            }

//            else
//            {
//                Temp.Hitted(UnitEnemyDamage);
//            }

//            AttackTime = 0.0f;
//        }
//    }
//}
#endregion