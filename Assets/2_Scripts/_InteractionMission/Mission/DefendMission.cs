using UnityEngine;

public class DefendMission : Mission
{
    // 발판에 서있는 동안 죽여야하는 적 수
    [SerializeField] private int _missionClearKill = 100;
    public int MissionClearKill { get { return _missionClearKill; } }

    [SerializeField] private int _killCount = 0;

    protected override void Start()
    {
        base.Start();
        _killCount = (All_Manager.instance.GameMode_M.Kill_Count + _missionClearKill);
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
        // 발판을 밟고
        if (_isMissionPrograce != true) return;

        // 죽인 적의 수가 미션 클리어 조건수보다 많으면
        if (All_Manager.instance.GameMode_M.Kill_Count > _killCount)
        {
            // 미션 클리어
            _isMissionClear = true;
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
