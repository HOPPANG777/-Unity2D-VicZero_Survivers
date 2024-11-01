using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    [SerializeField] BoxCollider2D _boxCollider2D;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [SerializeField] private bool _isMissionPrograce = false;
    public bool IsMissionPrograce { get { return _isMissionPrograce; } }

    [SerializeField] private float _checkInterval = 0.25f;
    [SerializeField] private float _lastCheckTime = 0.0f;

    protected virtual void Start()
    {
        _spriteRenderer = this.AddComponent<SpriteRenderer>();

        _boxCollider2D = this.AddComponent<BoxCollider2D>();
        _boxCollider2D.isTrigger = true;
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time >= _lastCheckTime + _checkInterval)
        {
            // 0.25초가 경과한 경우 충돌 검사 수행
            CheckPlayerCollision();

            // 현재 시간을 기준으로 마지막 체크 시간을 업데이트
            _lastCheckTime = Time.time;
        }
    }

    protected virtual void CheckPlayerCollision()
    {
        var player = All_Manager.instance.GameMode_M.Current_Player;
        var mission = All_Manager.instance.GameMode_M.missionList;

        // 플레이어 위치와 최소 거리 설정
        Vector2 playerPosition = player.transform.position;
        float minDistance = 1.0f;

        for (int i = 0; i < mission.Count; i++)
        {
            // 플레이어와 미션간의 위치 차이 계산
            Vector2 direction = playerPosition - (Vector2)mission[i].transform.position;

            // 거리 계산을 제곱된 형태로 유지하여 성능 최적화 (루트 계산 방지)
            float directionSqr = direction.sqrMagnitude;
            float minDistanceSqr = minDistance * minDistance;

            if (directionSqr < minDistanceSqr)
            {
                _isMissionPrograce = true;
            }
            else
            {
                _isMissionPrograce = false;
            }
        }
    }
}
