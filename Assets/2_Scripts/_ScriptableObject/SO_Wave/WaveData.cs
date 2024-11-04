using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WaveSpawnType
{
    //랜덤
    Random,

    //원형으로 둘러싸서 나오기
    Surround,

    //한점에 뭉쳐서 오기
    Dot,

    //마지막 보스
    BossWave
}

[CreateAssetMenu(menuName = "STO/Wave")]

public class WaveData : ScriptableObject
{
    [Header("어떻게 스폰될까?")]
    public WaveSpawnType WST = WaveSpawnType.Random;

    //나오는 조건
    [Header("나오는 조건")]
    public int WaveTime = 0;
    public int KillCount = 0;

    [Header("나올 시간 0 일경우 무한으로 나옴")]

    //나올 시간 0 일경우 무한으로 나옴
    public int SpawnTime = 0;

    [Header("얼마나 나올지")]

    //얼마나 나올지
    public int SpawnCount = 0;

    [Header("나올 애")]

    //나올 애
    public UnitData STOEnemy;
    public bool Elite = false;
}
