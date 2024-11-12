using UnityEngine;

public class DefendMission : Mission
{
    // 발판에 서있는 동안 죽인 몬스터
    [SerializeField] private int _missionKillCount = 0;
    public int MissionKillCount { get { return _missionKillCount; } }

    // 발판에 서있는 동안 죽여야하는 적 수
    [SerializeField] private int _missionClearKill = 100;
    public int MissionClearKill { get { return _missionClearKill; } }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop != false) return;

        DefendOnMission();
        MissionReward();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void CheckPlayerCollision()
    {
        base.CheckPlayerCollision();
    }

    // 수비 -> 몬스터 100마리 죽이기
    private void DefendOnMission()
    {
        int GameMissionKillCount = All_Manager.instance.GameMode_M.MissionKillCount;
        
        // 발판을 밟고
        if (_isMissionPrograce == true)
        {
            // 죽인 몬스터수가 클리어 몬스터 수보다 많으면
            if (GameMissionKillCount > _missionClearKill)
            {
                // 미션을 클리어 하고 발판을 없앤후 보물상자 드랍
                _isMissionClear = true;
            }
        }
        else
        {
            // 미션 도중 발판에서 벗어나면 미션 초기화
            GameMissionKillCount = _missionKillCount;
        }
    }


    protected override void MissionReward()
    {
        // 미션이 클리어 되면
        if (_isMissionClear == true)
        {
            // Reward(보물상자)를 떨군 후 미션(발판) 삭제 
            All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Chest, this.gameObject.transform);  // 보물상자 드랍
            RemoveMission();  // 발판 삭제
        }
    }

    protected override void RemoveMission()
    {
        Destroy(this.gameObject);
        All_Manager.instance.Resource_M.SYGizmo.GizmoTargets.Remove(this.gameObject);
    }

    public override void MissionSetting(Mission p_Mission, STO_Mission p_MTO)
    {
        p_Mission.MissionType = p_MTO.missionType;
        p_Mission.SpriteRenderer.sprite = p_MTO.Sprite;
        p_Mission.SpriteRenderer.material = p_MTO.Material;
        p_Mission.SpriteRenderer.sortingOrder = -1;
    }

}
