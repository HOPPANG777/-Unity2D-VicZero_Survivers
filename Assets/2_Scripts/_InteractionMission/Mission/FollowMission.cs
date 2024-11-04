using TMPro;
using UnityEngine;

public class FollowMission : Mission
{
    // 발판을 쫒아가는 동안 흐르는 시간
    [SerializeField] private float _missionFollowTimer = 0.0f;
    public float MissionFollowTimer { get { return _missionFollowTimer; } }

    // 발판을 쫒아가야하는 시간
    [SerializeField] private float _missionClearTime = 15.0f;
    public float MissionClearTime { get { return _missionClearTime; } }

    [SerializeField] private float _speed = 2f; // 발판의 이동 속도
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
        if (All_Manager.instance.GameMode_M.Game_Stop == true) return;

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
        if (_isMissionPrograce != true) return;

        if (_isMissionPrograce == true)
        {
            _missionFollowTimer += Time.deltaTime;
            // 움직이는 발판을 따라다니면
            if (_missionFollowTimer > _missionClearTime)
            {
                // 미션 클리어
                _isMissionClear = true;
            }
        }
        else
        {
            // 미션 도중 발판에서 벗어나면 미션 초기화
            _missionFollowTimer = 0f;
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
            transform.position = transform.position + (direction.normalized * (_speed * Time.deltaTime));
        }
    }

    protected override void MissionReward()
    {
        // 미션이 클리어 되면
        if (_isMissionClear == true)
        {
            // Reward(보물상자)를 떨군 후 미션(발판) 삭제 
            All_Manager.instance.Pool_M.Item_Pool_Out(ItemType.Chest, this.gameObject.transform);  // 보물상자 드랍
            RemoveMission();  // 발판 삭제
        }
    }

    protected override void RemoveMission()
    {
        Destroy(this.gameObject);
    }

    public override void MissionSetting(Mission p_Mission, STO_Mission p_ItemTypeO)
    {
        p_Mission.MissionType = p_ItemTypeO.missionType;
        p_Mission.SpriteRenderer.sprite = p_ItemTypeO.Sprite;
        p_Mission.SpriteRenderer.material = p_ItemTypeO.Material;
        p_Mission.SpriteRenderer.sortingOrder = -1;
    }

}
