using TMPro;
using UnityEngine;

public class FollowMission : Mission
{
    // 발판을 쫒아가는 동안 흐르는 시간
    [SerializeField] private float missionFollowTimer = 0.0f;
    public float MissionFollowTimer { get { return missionFollowTimer; } }

    // 발판을 쫒아가야하는 시간
    [SerializeField] private float missionClearTime = 15.0f;
    public float MissionClearTime { get { return missionClearTime; } }

    [SerializeField] private float speed = 2f; // 발판의 이동 속도
    [SerializeField] private Vector2 minBounds; // 랜덤 목표 위치의 최소 경계 (예: 왼쪽 아래)
    [SerializeField] private Vector2 maxBounds; // 랜덤 목표 위치의 최대 경계 (예: 오른쪽 위)

    private Vector3 targetPosition;

    protected override void Start()
    {
        base.Start();
        SetNewTargetPosition();
    }

    private void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == true) return;

        FollowOnMission();
        MissionReward();
        MoveTowardsTarget();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void CheckPlayerCollision()
    {
        base.CheckPlayerCollision();
    }

    // 발판을 따라다니는 미션이다
    private void FollowOnMission()
    {
        // 발판을 밟고
        if (isMissionPrograce != true) return;

        if (isMissionPrograce == true)
        {
            missionFollowTimer += Time.deltaTime;
            // 움직이는 발판을 따라다니면
            if (missionFollowTimer > missionClearTime)
            {
                // 미션 클리어
                isMissionClear = true;
            }
        }
        else
        {
            // 미션 도중 발판에서 벗어나면 미션 초기화
            missionFollowTimer = 0f;
        }

        
    }

    private void SetNewTargetPosition()
    {
        float randomX = UnityEngine.Random.Range(minBounds.x + 1, maxBounds.x + 5);
        float randomY = UnityEngine.Random.Range(minBounds.y + 1, maxBounds.y + 5);
        targetPosition = new Vector3(randomX, randomY, transform.position.z);
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;

        if (direction.magnitude < 0.1f) // 목표 위치에 거의 도달하면 새로운 목표 설정
        {
            SetNewTargetPosition();
        }
        else
        {
            transform.position = transform.position + (direction.normalized * (speed * Time.deltaTime));
        }
    }

    protected override void MissionReward()
    {
        // 미션이 클리어 되면
        if (isMissionClear == true)
        {
            // Reward(보물상자)를 떨군 후 미션(발판) 삭제 
            AllManager.instance.PoolM.ItemPoolOut(ItemType.Chest, this.gameObject.transform);  // 보물상자 드랍
            RemoveMission();  // 발판 삭제
        }
    }

    protected override void RemoveMission()
    {
        Destroy(this.gameObject);
    }

    public override void MissionSetting(Mission pMission, MissionData pItemTypeO)
    {
        pMission.MissionType = pItemTypeO.MissionType;
        pMission.SpriteRenderer.sprite = pItemTypeO.Sprite;
        pMission.SpriteRenderer.material = pItemTypeO.Material;
        pMission.SpriteRenderer.sortingOrder = -1;
    }

}
