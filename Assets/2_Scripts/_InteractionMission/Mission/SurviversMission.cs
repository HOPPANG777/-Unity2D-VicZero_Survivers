using System.Threading;
using UnityEngine;

public class SurviversMission : Mission
{
    // 발판에서 생존해야하는 시간
    [SerializeField] private float missionClearTime = 30.0f;
    public float MissionClearTime { get { return missionClearTime; } }

    [SerializeField] private float timer = 0.0f;

    protected override void Start()
    {
        base.Start();
        timer = AllManager.instance.GameModeM.GameTime + missionClearTime;
    }

    private void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == true) return;

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

    private void SurviveOnMission()
    {
        // 발판을 밟고
        if (isMissionPrograce != true) return;

        

        // 게임시간이 미션클리어시간보다 더 흐르면
        if (AllManager.instance.GameModeM.GameTime > timer)
        {
            // 미션을 클리어 하고 발판을 없앤후 보물상자 드랍
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
        pMission.MissionType = pItemTypeO.MissionType;
        pMission.SpriteRenderer.sprite = pItemTypeO.Sprite;
        pMission.SpriteRenderer.material = pItemTypeO.Material;
        pMission.SpriteRenderer.sortingOrder = -1;
    }

}
