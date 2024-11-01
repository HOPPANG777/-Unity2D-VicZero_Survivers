using UnityEngine;

public class DefendMission : Mission
{
    // 발판에 서있는 동안 죽여야하는 적 수
    [SerializeField] private int _missionClearKill = 100;
    public int MissionClearKill { get { return _missionClearKill; } }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void CheckPlayerCollision()
    {
        base.CheckPlayerCollision();
    }

    protected override void MissionReward()
    {

    }

    protected override void RemoveMission()
    {

    }

    protected override void MissionSetting()
    {

    }

}
