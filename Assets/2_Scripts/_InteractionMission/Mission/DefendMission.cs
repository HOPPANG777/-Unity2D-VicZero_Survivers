using UnityEngine;

public class DefendMission : Mission
{
    // 발판에 서있는 동안 죽여야하는 적 수
    [SerializeField] private int missionClearKill = 100;
    public int MissionClearKill { get { return missionClearKill; } }

    [SerializeField] private int killCount = 0;

    protected override void Start()
    {
        base.Start();
        killCount = (AllManager.instance.GameModeM.KillCount + missionClearKill);
    }

    private void Update()
    {
        if (AllManager.instance.GameModeM.GameStop != false) return;

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
        if (isMissionPrograce != true) return;

        // 죽인 적의 수가 미션 클리어 조건수보다 많으면
        if (AllManager.instance.GameModeM.KillCount > killCount)
        {
            // 미션 클리어
            isMissionClear = true;
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
        pMission.MissionType = pItemTypeO.missionType;
        pMission.SpriteRenderer.sprite = pItemTypeO.Sprite;
        pMission.SpriteRenderer.material = pItemTypeO.Material;
        pMission.SpriteRenderer.sortingOrder = -1;
    }

}
