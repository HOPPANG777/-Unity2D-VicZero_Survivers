using UnityEngine;

[CreateAssetMenu(menuName = "STO/Mission")]
public class STO_Mission : ScriptableObject
{
    [Header("MissionData")]
    // 미션 타입
    [SerializeField] MissionType _missionType = MissionType.SURVIVE;
    public MissionType missionType { get { return _missionType; } set { _missionType = value; } }
    
    // 발판
    [SerializeField] Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }

    // 발판
    [SerializeField] Material _material;
    public Material Material { get { return _material; } set { _material = value; } }


    // 미션 소환 시간
    [Header("나오는 조건")]
    public int Mission_Time = 0;
}
