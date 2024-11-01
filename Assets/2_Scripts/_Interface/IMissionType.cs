using UnityEngine;

public interface IMissionType
{
    MissionType MissionType { get; set; }
}

public enum MissionType
{
    SURVIVE,
    DEFEND,
    FOLLOW
}