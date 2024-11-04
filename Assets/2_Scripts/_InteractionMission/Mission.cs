using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Mission : MonoBehaviour, IMissionType
{
    [SerializeField] protected BoxCollider2D boxCollider2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }

    // 미션 시작
    [SerializeField] protected bool isMissionPrograce = false;
    public bool IsMissionPrograce { get { return isMissionPrograce; } }

    // 미션 클리어
    [SerializeField] protected bool isMissionClear = false;
    public bool IsMissionCleare { get { return isMissionClear; } }

    // 1초당 4번 충돌 처리
    [SerializeField] protected float checkInterval = 0.25f;
    [SerializeField] protected float lastCheckTime = 0.0f;

    // 미션 타입
    [SerializeField] protected MissionType missionType;
    public MissionType MissionType { get { return missionType; } set { missionType = value; } }

    protected virtual void Start()
    {
        spriteRenderer = this.AddComponent<SpriteRenderer>();

        boxCollider2D = this.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time >= lastCheckTime + checkInterval)
        {
            // 0.25초가 경과한 경우 충돌 검사 수행
            CheckPlayerCollision();

            // 현재 시간을 기준으로 마지막 체크 시간을 업데이트
            lastCheckTime = Time.time;
        }
    }

    // 플레이어가 발판에 올라왔을때 충돌처리
    protected virtual void CheckPlayerCollision()
    {
        var player = AllManager.instance.GameModeM.CurrentPlayer;
        var mission = this.gameObject;

        // 플레이어 위치와 최소 거리 설정
        Vector2 playerPosition = player.transform.position;
        float minDistance = 1.0f;

        // 플레이어와 미션간의 위치 차이 계산
        Vector2 direction = playerPosition - (Vector2)mission.transform.position;

        // 거리 계산을 제곱된 형태로 유지하여 성능 최적화 (루트 계산 방지)
        float directionSqr = direction.sqrMagnitude;
        float minDistanceSqr = minDistance * minDistance;

        if (directionSqr < minDistanceSqr)
        {
            isMissionPrograce = true;
        }
        else
        {
            isMissionPrograce = false;
        }
    }

    // 미션 클리어후 보상
    protected abstract void MissionReward();

    // 미션 보상을 떨군후 미션 오브젝트 풀 or 삭제
    protected abstract void RemoveMission();

    // 미션 생성시 Scriptable Object로 초기화
    public abstract void MissionSetting(Mission pMission, MissionData pItemTypeO);

}
