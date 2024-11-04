using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

//�̰� �� Ÿ�� ����Ƽ�� ��������� �� ���̶�Ű���� ����
//����ȭ
[System.Serializable]
public enum GameModeState
{
    Idle,
}

public class GameModeManager : MonoBehaviour
{
    public GameModeState GMState = GameModeState.Idle;

    public UnitPlayer CurrentPlayer = null;
    public List<UnitEnemy> EnemyList = new List<UnitEnemy>();
    public List<Mission> missionList = new List<Mission>();
    public List<Item> ItemList = new List<Item>();
    public Camera CurrentCamera = null;
    public List<GameObject> TilemapList = new List<GameObject>();
    private List<float> missionSpawnTimes = new List<float> { 120f, 180f, 240f }; // 2��, 3��, 4��(�� ����)
    private HashSet<float> spawnedMissions = new HashSet<float>();

    // ų ī��Ʈ
    public int KillCount = 0;

    // �� ī��Ʈ
    public int GemCount = 0;
    public float CurrentGem;
    public float MaxGem = 5.0f;
    public float BaseMaxGem = 5.0f;

    // �÷��̾� ����
    public int PlayerLevelCount = 0;

    //���ӽð�
    public float GameTime = 0;

    // ����� ���̺� ����Ʈ
    public List<WaveData> UseWaveDataList = new List<WaveData>();

    public bool GameStop = false;

    IEnumerator CWaveTemp;

    //�ð����� �̿��ؼ� �����
    //�̺�ũ �Լ� - X
    //Update()
    //�ڷ�ƾ

    //�ڷ�ƾ�� �ð����� �̿��� ����̳�.
    //�����ð� ���Ŀ� ó���ϴ� ����� ���Ҷ� ����ϴ� �Լ��̴�.
    //�ڷ�ƾ�� ������ ū�帧�� ���̶�� ������
    //�� ���� �������ͼ� �帣�� �ٽ� ū������ ���ƿ���
    //�ó����̶� ���� �ִ�.
    //������ �ڷ�ƾ�� ����ġ�� �ʹ� ���� ���� �Ǹ�
    //���ɿ� ������ �־� ������ ��ĥ�� ������ 
    //������ ����� �ʿ��ϴ�.

    public float FollowSpeed = 4.0f;
    public float FixedCameraZ = -10.0f;

    // Ÿ�ϸ� ����
    public float TilemapSize = 40.0f;
    public float TilemapBoundaryOffset = 1.0f;
    public float TilemapOverlapOffset = 0.1f;

    public delegate void OnGameState();
    public OnGameState onGameOver;
    public OnGameState onGameClear;
    Coroutine executeCoroutine;


    private void Update()
    {
        AllManager.instance.UIM.UpdatePoolReadyCheckText(AllManager.instance.PoolM.Ready.ToString());
        if (AllManager.instance.PoolM.Ready == false) return;

        // 카메라가 따라가는 로직
        if (CurrentPlayer != null)
        {
            CurrentCamera.transform.position = Vector2.Lerp
            (
                CurrentCamera.transform.position,
                CurrentPlayer.transform.position,
                FollowSpeed * Time.deltaTime
            );
            CurrentCamera.transform.Translate(0, 0, FixedCameraZ); //ī�޶� ���� z������ �̵�
            TilemapMove();
        }

    }

    IEnumerator CoExecuteGame()
    {
        while (true)
        {
            //Time.timeScale = 1f;

            if (CurrentPlayer == null)
                yield return null;

            if (GameStop == false)
            {
                GameTime += Time.deltaTime;
            }

            AllManager.instance.UIM.UpdateTimeCountText(GameTime);

            ResourceManager resourceManager = AllManager.instance.ResourceM;

            for (int i = 0; i < resourceManager.WaveDatas.Length; i++)
            {
                //ų ������ �´ٸ�
                if (resourceManager.WaveDatas[i].KillCount <= KillCount)
                {
                    //�ð� ������ �´ٸ�
                    if (resourceManager.WaveDatas[i].WaveTime <= GameTime)
                    {
                        // 특정 시간과 킬 개수를 만족하면 해당 코드를 실행
                        //������� ���̺� ����Ʈ�� ���ٸ�
                        // Contains �� ����Ʈ�� ���� �մϱ�?
                        if (UseWaveDataList.Contains(resourceManager.WaveDatas[i]) == false)
                        {
                            UseWaveDataList.Add(resourceManager.WaveDatas[i]);

                            StartWave(resourceManager.WaveDatas[i]);

                            //UnityEngine.Debug.Log(Re.WaveDatas[i].name + " Start!");
                            AllManager.instance.UIM.UpdateWaveCheckText(resourceManager.WaveDatas[i].name.ToString());
                        }
                    }
                }
            }

            CheckMissionSpawn();

            AllManager.instance.UIM.UpdateStateCheckText("InGame");

            yield return null;
        }
    }

    public void GameOver()
    {
        StopCoroutine(CWaveTemp);

        GameTime = 0;
        UseWaveDataList.Clear();
        AllManager.instance.GameModeM.GameStop = true;
        StopCoroutine(executeCoroutine);

        onGameOver();
    }
    // 게임 클리어 상태일 경우
    public void GameClear()
    {
        StopCoroutine(CWaveTemp);

        GameTime = 0;
        UseWaveDataList.Clear();
        AllManager.instance.GameModeM.GameStop = true;
        StopCoroutine(executeCoroutine);

        onGameClear();
    }

    //���� ���� �Լ�
    public void GameStart()
    {
        AllManager.instance.UnitM.UnitF.CreatePlayer(AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(UnitType.Player));
        AllManager.instance.UIM.OnPanel(PanelStyle.GameStart);

        executeCoroutine = StartCoroutine(CoExecuteGame());
    }

    // ���� �����
    public void GameRestart()
    {
        EnemyListRemove();
        ItemListRemove();
        CurrentPlayer.UnitClear();
        AllManager.instance.UIM.ClearUI();
        AllManager.instance.UIM.OffPanel();
        GameTime = 0.0f;
        KillCount = 0;

        AllManager.instance.UnitM.UnitF.CreatePlayer(AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(UnitType.Player));

        GameStart();

        AllManager.instance.UIM.OnPanel(PanelStyle.GameStart);
    }

    // ���� ������
    public void GameExit()
    {
#if UNITYEDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    // ���� �Ͻ�����
    public void ButtonGamePause()
    {
        AllManager.instance.UIM.OnPanel(PanelStyle.Pause);
    }

    // ���� ����ϱ�
    public void ButtonGameContinue()
    {
        AllManager.instance.UIM.OffPanel();
    }

    // Ÿ�ϸ� �̵�
    public void TilemapMove()
    {
        Vector3 PlayerPos = CurrentPlayer.transform.position;

        for (int i = 0; i < TilemapList.Count; i++)
        {
            GameObject tilemap = TilemapList[i];
            Vector3 MapPos = tilemap.transform.position;

            // Ÿ�ϸ� ��� ���
            float LeftBoundary = MapPos.x - (TilemapSize * 0.5f);
            float RightBoundary = MapPos.x + (TilemapSize * 0.5f);
            float BottomBoundary = MapPos.y - (TilemapSize * 0.5f);
            float TopBoundary = MapPos.y + (TilemapSize * 0.5f);

            // �÷��̾ ��踦 �Ѿ����� Ȯ��
            var MoveOffset = Vector3.zero;

            if (PlayerPos.x < LeftBoundary)
            {
                // ���� ��踦 ���� ���
                MoveOffset.x = -TilemapSize + TilemapOverlapOffset;
            }
            else if (PlayerPos.x > RightBoundary)
            {
                // ������ ��踦 ���� ���
                MoveOffset.x = TilemapSize - TilemapOverlapOffset;
            }

            if (PlayerPos.y < BottomBoundary)
            {
                // �Ʒ� ��踦 ���� ���
                MoveOffset.y = -TilemapSize + TilemapOverlapOffset;
            }
            else if (PlayerPos.y > TopBoundary)
            {
                // ���� ��踦 ���� ���
                MoveOffset.y = TilemapSize - TilemapOverlapOffset;
            }

            // Ÿ�ϸ� �̵�(�밢�� ����)
            if (MoveOffset != Vector3.zero)
            {
                // Ÿ�ϸ� �̵�
                tilemap.transform.position += MoveOffset;
            }
        }

    }

    // �� ����Ʈ �ʱ�ȭ
    public void EnemyListRemove()
    {
        foreach (UnitEnemy enemy in EnemyList)
        {
            enemy.UnitClear();
        }

        EnemyList.Clear();
    }

    public void ItemListRemove()
    {
        foreach (Item misc in ItemList)
        {
            misc.ItemClear();
        }

        ItemList.Clear();

        AllManager.instance.UIM.UpdateGemCountImage(0);
    }

    // �� óġ�� ų ī��Ʈ & �� ���
    public void EnemyDown(UnitEnemy pUE)
    {
        KillCount++;

        if (pUE.UnitType == UnitType.Barrel)
        {
            AllManager.instance.PoolM.ItemPoolOut(ItemType.Food, pUE.transform);
        }

        else
        {
            if (pUE.Elite == false)
            {
                AllManager.instance.PoolM.ItemPoolOut(ItemType.Gem, pUE.transform);
            }

            else
            {
                AllManager.instance.PoolM.ItemPoolOut(ItemType.Gem, pUE.transform);

                AllManager.instance.PoolM.ItemPoolOut(ItemType.Chest, pUE.transform);
            }
        }

        AllManager.instance.UIM.UpdateKillCountText(KillCount);

        EnemyList.Remove(pUE);
    }




    // ����ġ ȹ��
    public void AddGem(float pGemCount)
    {
        float GemPrev = 0;

        GemPrev = CurrentGem;
        CurrentGem += pGemCount;

        UpdateGemCountBarFill(GemPrev);
    }

    // ������ ȣ�� �� ����ġ ��Ȳ
    public void UpdateGemCountBarFill(float GemPrev)
    {

        GemPrev = CurrentGem;
        AllManager.instance.UIM.UpdateGemCountImage(GemPrev / MaxGem);

        if (GemPrev / MaxGem >= 1f)
        {
            PlayerLevelCount++;

            GemPrev = 0f;
            CurrentGem = 0f;
            MaxGem *= 1.2f;

            if (CurrentGem < 0)
            {
                CurrentGem = 0;
            }

            AllManager.instance.UIM.UpdatePlayerLevelCountText(PlayerLevelCount);   // ���� UI ȣ��
            AllManager.instance.UIM.OnPanel(PanelStyle.Level); // ������ ȣ��

            AllManager.instance.UIM.isLevelUp = true;
            //UnityEngine.Debug.Log(CurrentGem);
        }


    }

    public void StartWave(WaveData pWaveData)
    {
        CWaveTemp = CStartWave(pWaveData);

        StartCoroutine(CWaveTemp);
    }

    public IEnumerator CStartWave(WaveData pWaveData)
    {
        if (pWaveData.SpawnTime == 0)
        {
            while (true)
            {
                if (pWaveData.WST == WaveSpawnType.Random)
                {
                    for (int i = 0; i < pWaveData.SpawnCount; i++)
                    {
                        int TempX = Random.Range(5, 10);
                        TempX = ((TempX % 2) == 1) ? -TempX : TempX;

                        int TempY = Random.Range(5, 10);
                        TempY = ((TempY % 2) == 1) ? -TempY : TempY;

                        Vector3 PlayerPos = CurrentPlayer.transform.position;
                        PlayerPos += new Vector3(TempX, TempY, 0);

                        // STO �� Position �� �Ű������� ������ �ְ�
                        // AllManager.instance.UnitM.CreateUnit(STO , Position);
                        // AllManager.instance.UnitM.CreateUnit(UnitName.Enemy);

                        if (GameStop == false)
                        {
                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, PlayerPos);
                            }

                            else
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, PlayerPos, true);
                            }
                        }

                        else { }
                    }
                }

                else if (pWaveData.WST == WaveSpawnType.Surround)
                {

                    float Rad = 6.0f;   // �÷��̾� �߽� ���� �������� 6�� ���� ���Ѵ�. 

                    Vector3 PlayerPos = CurrentPlayer.transform.position;

                    float AngleStep = 360.0f / pWaveData.SpawnCount;
                    float Angle = 0.0f;

                    for (int i = 0; i < pWaveData.SpawnCount; i++)
                    {
                        if (GameStop == false)
                        {
                            float EnemyPosX = PlayerPos.x + Mathf.Sin(Angle * Mathf.Deg2Rad) * Rad;
                            float EnemyPosY = PlayerPos.y + Mathf.Cos(Angle * Mathf.Deg2Rad) * Rad;

                            Vector3 EnemyPos = new Vector3(EnemyPosX, EnemyPosY, PlayerPos.z);


                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, EnemyPos);
                            }

                            else
                            {

                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, EnemyPos, true);

                            }

                            Angle += AngleStep;
                        }

                        else
                        {

                        }
                    }
                }



                else if (pWaveData.WST == WaveSpawnType.Dot)
                {
                    int TempX = Random.Range(5, 10);
                    TempX = ((TempX % 2) == 1) ? -TempX : TempX;

                    int TempY = Random.Range(5, 10);
                    TempY = ((TempY % 2) == 1) ? -TempY : TempY;

                    Vector3 PlayerPos = CurrentPlayer.transform.position;
                    PlayerPos += new Vector3(TempX, TempY, 0);

                    // �÷��̾� �ֺ��� ������ ���� ���
                    //Vector3 DotSpawn = PlayerPos + (Vector3)(Random.insideUnitCircle * spawnRadius);

                    if (GameStop == false)
                    {
                        for (int i = 0; i < pWaveData.SpawnCount; i++)
                        {
                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, (PlayerPos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)));
                            }

                            else
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, (PlayerPos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)), true);
                            }
                        }
                    }

                    else { }
                }

                else if (pWaveData.WST == WaveSpawnType.BossWave)
                {
                    for (int i = 0; i < pWaveData.SpawnCount; i++)
                    {
                        int TempX = Random.Range(5, 10);
                        TempX = ((TempX % 2) == 1) ? -TempX : TempX;

                        int TempY = Random.Range(5, 10);
                        TempY = ((TempY % 2) == 1) ? -TempY : TempY;

                        Vector3 PlayerPos = CurrentPlayer.transform.position;
                        PlayerPos += new Vector3(TempX, TempY, 0);

                        // STO �� Position �� �Ű������� ������ �ְ�
                        // AllManager.instance.UnitM.CreateUnit(STO , Position);
                        // AllManager.instance.UnitM.CreateUnit(UnitName.Enemy);

                        if (GameStop == false)
                        {
                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.UnitM.UnitF.CreateBoss(AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(UnitType.Boss), PlayerPos);
                            }
                        }

                        else { }
                    }
                }

                yield return new WaitForSeconds(1f);
                //UnityEngine.Debug.Log(111);
            }
        }

        else
        {
            for (int i = 0; i < pWaveData.SpawnTime; i++)
            {
                if (pWaveData.WST == WaveSpawnType.Random)
                {
                    for (int j = 0; j < pWaveData.SpawnCount; j++)
                    {
                        int TempX = Random.Range(5, 10);
                        TempX = ((TempX % 2) == 1) ? -TempX : TempX;

                        int TempY = Random.Range(5, 10);
                        TempY = ((TempY % 2) == 1) ? -TempY : TempY;

                        Vector3 PlayerPos = CurrentPlayer.transform.position;
                        PlayerPos += new Vector3(TempX, TempY, 0);

                        // STO �� Position �� �Ű������� ������ �ְ�
                        //AllManager.instance.UnitM.CreateUnit(STO , Position);

                        if (GameStop == false)
                        {
                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, PlayerPos);
                            }

                            else
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, PlayerPos, true);
                            }
                        }

                        else { }
                    }
                }

                else if (pWaveData.WST == WaveSpawnType.Surround)
                {

                    float Rad = 6.0f;   // �÷��̾� �߽� ���� �������� 6�� ���� ���Ѵ�. 

                    Vector3 PlayerPos = CurrentPlayer.transform.position;

                    float AngleStep = 360.0f / pWaveData.SpawnCount;
                    float Angle = 0.0f;

                    for (int j = 0; j < pWaveData.SpawnCount; j++)
                    {
                        if (GameStop == false)
                        {
                            float EnemyPosX = PlayerPos.x + Mathf.Sin(Angle * Mathf.Deg2Rad) * Rad;
                            float EnemyPosY = PlayerPos.y + Mathf.Cos(Angle * Mathf.Deg2Rad) * Rad;

                            Vector3 EnemyPos = new Vector3(EnemyPosX, EnemyPosY, PlayerPos.z);

                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, EnemyPos);
                            }

                            else
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, EnemyPos, true);
                            }

                            Angle += AngleStep;
                        }

                        else
                        {

                        }
                    }
                }



                else if (pWaveData.WST == WaveSpawnType.Dot)
                {
                    int TempX = Random.Range(5, 10);
                    TempX = ((TempX % 2) == 1) ? -TempX : TempX;

                    int TempY = Random.Range(5, 10);
                    TempY = ((TempY % 2) == 1) ? -TempY : TempY;

                    Vector3 PlayerPos = CurrentPlayer.transform.position;
                    PlayerPos += new Vector3(TempX, TempY, 0);

                    if (GameStop == false)
                    {
                        for (int j = 0; j < pWaveData.SpawnCount; j++)
                        {

                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, (PlayerPos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)));
                            }

                            else
                            {
                                AllManager.instance.PoolM.EnemyPoolOut(pWaveData.STOEnemy.UnitType, (PlayerPos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)), true);
                            }

                        }
                    }

                    else { }
                }

                else if (pWaveData.WST == WaveSpawnType.BossWave)
                {
                    for (int j = 0; j < pWaveData.SpawnCount; j++)
                    {
                        int TempX = Random.Range(5, 10);
                        TempX = ((TempX % 2) == 1) ? -TempX : TempX;

                        int TempY = Random.Range(5, 10);
                        TempY = ((TempY % 2) == 1) ? -TempY : TempY;

                        Vector3 PlayerPos = CurrentPlayer.transform.position;
                        PlayerPos += new Vector3(TempX, TempY, 0);

                        if (GameStop == false)
                        {
                            if (pWaveData.Elite == false)
                            {
                                AllManager.instance.UnitM.UnitF.CreateBoss(AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(UnitType.Boss), PlayerPos);
                            }
                        }

                        else { }
                    }
                }

                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void CheckMissionSpawn()
    {
        foreach (var spawnTime in missionSpawnTimes)
        {
            if (GameTime >= spawnTime && !spawnedMissions.Contains(spawnTime))
            {
                SpawnRandomMission();
                spawnedMissions.Add(spawnTime);
            }
        }
    }

    private void SpawnRandomMission()
    {
        // ���� �̼� Ÿ�� ����
        MissionType randomMissionType = (MissionType)Random.Range(0, System.Enum.GetValues(typeof(MissionType)).Length);

        // ConvertMissionTypeToMissionDataArr�� ����� MissionData ��������
        MissionData newMission = AllManager.instance.ResourceM.ConvertMissionTypeToMissionDataArr(randomMissionType);

        if (newMission != null)
        {
            // �÷��̾� �ֺ��� ������ ��ġ ����
            Vector3 randomPosition = CurrentPlayer.transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);

            // ������ �̼��� ����Ͽ� �̼� ����
            AllManager.instance.UnitM.MissionF.CreateMission(newMission, randomPosition);
            AllManager.instance.UIM.UpdateMissionCheckText(randomMissionType.ToString());
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Mission of type '{randomMissionType}' could not be found.");
        }
    }

}

#region
//public void StopGemCountBarFill(float GemPrev)
//{
//    StopCoroutine(CUpdateGemCountBarFill(GemPrev));
//}

//public void UpdateGemCountBarFill(float GemPrev)
//{
//    StartCoroutine(CUpdateGemCountBarFill(GemPrev));
//}

//public IEnumerator CUpdateGemCountBarFill(float GemPrev)
//{
//    var GemCurrent = GemList[GemList.Count - 1].CurrentGem;
//    var GemMax = GemList[GemList.Count - 1].MaxGem;

//    //AllManager.instance.UIM.UpdateGemCountImage(GemPrev / GemMax);

//    //if (GemPrev / GemMax > 1f)
//    //{
//    //    GemPrev = 0f;
//    //    ExpCurrent -= ExpMax;
//    //    ExpMax *= 2.0f;

//    //    // ������ ȣ��
//    //}

//    float process = 0f;

//    while (process < 1f)
//    {
//        process += Time.deltaTime;

//        GemPrev = Mathf.Lerp(GemPrev, GemCurrent, process);
//        AllManager.instance.UIM.UpdateGemCountImage(GemPrev / GemMax);

//        if (GemPrev / GemMax > 1f)
//        {
//            GemPrev = 0f;
//            process = 0f;
//            GemCurrent -= GemMax;
//            GemMax *= 1.3f;

//            // ������ ȣ��
//        }

//        yield return null;
//    }

//}

//float GemCurrent = 0; //= GemList[GemList.Count - 1].CurrentGem;
//float GemMax = 0; //= GemList[GemList.Count - 1].MaxGem;

//for (int i = 0; i < GemList.Count; i++)
//{
//    GemCurrent = GemList[i].CurrentGem;
//    GemMax = GemList[i].MaxGem;
//}

//float process = 0f;

//while (process < 1f)
//{
//    process += Time.deltaTime;

//    GemPrev = Mathf.Lerp(GemPrev, GemCurrent, process);
//    AllManager.instance.UIM.UpdateGemCountImage(GemPrev / GemMax);

//    if (GemPrev / GemMax > 1f)
//    {
//        GemPrev = 0f;
//        process = 0f;
//        GemCurrent -= GemMax;
//        GemMax *= 1.3f;

//        AllManager.instance.UIM.OnPanel(PanelStyle.Level);// ������ ȣ��
//    }

//}
#endregion