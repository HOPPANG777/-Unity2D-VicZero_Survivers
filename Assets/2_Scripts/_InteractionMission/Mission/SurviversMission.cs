using System.Threading;
using UnityEngine;

public class SurviversMission : Mission
{
    // 발판에서 생존하는 동안 흐르는 시간
    [SerializeField] private float _missionSurviveTimer = 0.0f;
    public float MissionSurviveTimer { get { return _missionSurviveTimer; } }

    // 발판에서 생존해야하는 시간
    [SerializeField] private float _missionClearTime = 30.0f;
    public float MissionClearTime { get { return _missionClearTime; } }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == true) return;

        SurviveOnMission();
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

    // Defend - 몬스터 100마리 토벌
    // Survive - 30초 생존
    // Follow - 움직이는 발판 15초 동안 추적
    private void SurviveOnMission()
    {
        // 발판을 밟고
        if (_isMissionPrograce == true)
        {
            //  발판에서 생존하면
            _missionSurviveTimer += Time.deltaTime;

            // 게임시간이 미션클리어 조건이 충족되면
            if (_missionSurviveTimer > _missionClearTime)
            {
                // 미션을 클리어 하고 발판을 없앤후 보물상자 드랍
                _isMissionClear = true;
            }
        }
        else
        {
            // 미션 도중 발판에서 벗어나면 미션 초기화
            _missionSurviveTimer = 0;
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
