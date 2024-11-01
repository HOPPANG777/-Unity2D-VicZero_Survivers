using UnityEngine;

public class SurviversMission : Mission
{
    // 발판에서 생존해야하는 시간
    [SerializeField] private float _missionClearTime = 30.0f;
    public float MissionClearTime { get { return _missionClearTime; } }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop != true) return;

        SurviveOnMission();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void CheckPlayerCollision()
    {
        base.CheckPlayerCollision();
    }

    private void SurviveOnMission()
    {
        // 발판을 밟고
        if (_isMissionPrograce != true) return;

        // 게임시간이 미션클리어시간보다 더 흐르면
        if (All_Manager.instance.GameMode_M.Game_Time > (All_Manager.instance.GameMode_M.Game_Time + _missionClearTime))
        {
            // 미션을 클리어 하고 발판을 없앤후 보물상자 드랍
            _isMissionClear = true;
        }
 
    }

    protected override void MissionReward()
    {
        // 미션이 클리어 되면
        if(_isMissionClear != true) return;

        // Reward(보물상자)를 떨군 후 미션(발판) 삭제 
        All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Chest, this.gameObject.transform);  // 보물상자 드랍
        RemoveMission();  // 발판 삭제
    }

    protected override void RemoveMission()
    {

    }

    protected override void MissionSetting()
    {
        
    }

}
