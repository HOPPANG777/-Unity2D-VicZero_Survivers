using UnityEngine;

[CreateAssetMenu(menuName = "STO/Mission")]
public class STO_Mission : ScriptableObject
{
    [Header("MissionData")]
    // 미션 타입
    [SerializeField] MissionType _missionType;
    public MissionType missionType { get { return _missionType; } set { _missionType = value; } }
}
