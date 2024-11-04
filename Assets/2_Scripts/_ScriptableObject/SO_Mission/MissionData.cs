using UnityEngine;

[CreateAssetMenu(menuName = "STO/Mission")]
public class MissionData : ScriptableObject
{
    [Header("MissionData")]
    // 미션 타입
    [SerializeField] MissionType missionType = MissionType.SURVIVE;
    public MissionType missionType { get { return missionType; } set { missionType = value; } }
    
    // 발판
    [SerializeField] Sprite sprite;
    public Sprite Sprite { get { return sprite; } set { sprite = value; } }

    // 발판
    [SerializeField] Material material;
    public Material Material { get { return material; } set { material = value; } }


    // 미션 소환 시간
    [Header("나오는 조건")]
    public int MissionTime = 0;
}
