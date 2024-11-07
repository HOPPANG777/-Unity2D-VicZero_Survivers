using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

//�̰� �� Ÿ�� ����Ƽ�� ��������� �� ���̶�Ű���� ����
//����ȭ
[System.Serializable]
public enum GameMode_State
{
    Idle,
}

public class GameMode_Manager : MonoBehaviour
{
    public GameMode_State GM_State = GameMode_State.Idle;

    public Unit_Player Current_Player = null;
    public List<Unit_Enemy> Enemy_List = new List<Unit_Enemy>();
    public List<Mission> missionList = new List<Mission>();
    public List<MISC> MISC_List = new List<MISC>();
    public Camera Current_Camera = null;
    public List<GameObject> Tilemap_List = new List<GameObject>();
    private List<float> missionSpawnTimes = new List<float> { 120f, 180f, 240f }; // 2��, 3��, 4��(�� ����)
    private HashSet<float> spawnedMissions = new HashSet<float>();

    // ų ī��Ʈ
    public int Kill_Count = 0;

    // �� ī��Ʈ
    public int Gem_Count = 0;
    public float Current_Gem;
    public float Max_Gem = 5.0f;
    public float Base_Max_Gem = 5.0f;

    // �÷��̾� ����
    public int Player_Level_Count = 0;

    //���ӽð�
    public float Game_Time = 0;

    // ����� ���̺� ����Ʈ
    public List<STO_Wave> Use_STO_Wave_List = new List<STO_Wave>();

    public bool Game_Stop = false;

    IEnumerator C_Wave_Temp;

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

    public float Follow_Speed = 4.0f;
    public float Fixed_Camera_Z = -10.0f;

    // Ÿ�ϸ� ����
    public float Tilemap_Size = 40.0f;
    public float Tilemap_Boundary_Offset = 1.0f;
    public float Tilemap_Overlap_Offset = 0.1f;

    public delegate void OnGameState();
    public OnGameState onGameOver;
    public OnGameState onGameClear;
    Coroutine executeCoroutine;


    private void Update()
    {
        All_Manager.instance.UI_M.Update_Pool_Ready_Check_Text(All_Manager.instance.Pool_M.Ready.ToString());
        if (All_Manager.instance.Pool_M.Ready == false) return;

        // 카메라가 따라가는 로직
        if (Current_Player != null)
        {
            Current_Camera.transform.position = Vector2.Lerp
            (
                Current_Camera.transform.position,
                Current_Player.transform.position,
                Follow_Speed * Time.deltaTime
            );
            Current_Camera.transform.Translate(0, 0, Fixed_Camera_Z); //ī�޶� ���� z������ �̵�
            Tilemap_Move();
        }

    }

    IEnumerator CoExecuteGame()
    {
        while (true)
        {
            //Time.timeScale = 1f;

            if (Current_Player == null)
                yield return null;

            if (Game_Stop == false)
            {
                Game_Time += Time.deltaTime;
            }

            All_Manager.instance.UI_M.Update_Time_Count_Text(Game_Time);

            Resource_Manager resourceManager = All_Manager.instance.Resource_M;

            for (int i = 0; i < resourceManager.STO_Waves.Length; i++)
            {
                //킬 카운트가 조건이 충족되면
                if (resourceManager.STO_Waves[i].Kill_Count <= Kill_Count)
                {
                    //시간 조건이 충족되면
                    if (resourceManager.STO_Waves[i].Wave_Time <= Game_Time)
                    {
                        // 특정 시간과 킬 개수를 만족하면 해당 코드를 실행
                        if (Use_STO_Wave_List.Contains(resourceManager.STO_Waves[i]) == false)
                        {
                            Use_STO_Wave_List.Add(resourceManager.STO_Waves[i]);

                            Start_Wave(resourceManager.STO_Waves[i]);

                            //UnityEngine.Debug.Log(Re.STO_Waves[i].name + " Start!");
                            All_Manager.instance.UI_M.Update_Wave_Check_Text(resourceManager.STO_Waves[i].name.ToString());
                        }
                    }
                }
            }

            CheckMissionSpawn();

            All_Manager.instance.UI_M.Update_State_Check_Text("InGame");

            yield return null;
        }
    }

    public void GameOver()
    {
        StopCoroutine(C_Wave_Temp);

        Game_Time = 0;
        Use_STO_Wave_List.Clear();
        All_Manager.instance.GameMode_M.Game_Stop = true;
        StopCoroutine(executeCoroutine);

        onGameOver();
    }
    // 게임 클리어 상태일 경우
    public void GameClear()
    {
        StopCoroutine(C_Wave_Temp);

        Game_Time = 0;
        Use_STO_Wave_List.Clear();
        All_Manager.instance.GameMode_M.Game_Stop = true;
        StopCoroutine(executeCoroutine);

        onGameClear();
    }

    //���� ���� �Լ�
    public void GameStart()
    {
        All_Manager.instance.Unit_M.Unit_F.Create_Player(All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(Unit_Type.Player));
        All_Manager.instance.UI_M.On_Panel(Panel_Style.GameStart);

        executeCoroutine = StartCoroutine(CoExecuteGame());
    }

    // ���� �����
    public void Game_Restart()
    {
        Enemy_List_Remove();
        MISC_List_Remove();
        Current_Player.Unit_Clear();
        All_Manager.instance.UI_M.Clear_UI();
        All_Manager.instance.UI_M.Off_Panel();
        Game_Time = 0.0f;
        Kill_Count = 0;

        All_Manager.instance.Unit_M.Unit_F.Create_Player(All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(Unit_Type.Player));

        GameStart();

        All_Manager.instance.UI_M.On_Panel(Panel_Style.GameStart);
    }

    // ���� ������
    public void Game_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    // ���� �Ͻ�����
    public void Button_Game_Pause()
    {
        All_Manager.instance.UI_M.On_Panel(Panel_Style.Pause);
    }

    // ���� ����ϱ�
    public void Button_Game_Continue()
    {
        All_Manager.instance.UI_M.Off_Panel();
    }

    public void Tilemap_Move()
    {
        // 현재 플레이어의 위치 가져오기
        Vector3 Player_Pos = Current_Player.transform.position;

        // 모든 타일맵에 대해 반복
        for (int i = 0; i < Tilemap_List.Count; i++)
        {
            GameObject tilemap = Tilemap_List[i];
            Vector3 Map_Pos = tilemap.transform.position;

            // 타일맵의 경계 설정
            float Left_Boundary = Map_Pos.x - (Tilemap_Size * 0.5f);  // 왼쪽 경계
            float Right_Boundary = Map_Pos.x + (Tilemap_Size * 0.5f); // 오른쪽 경계
            float Bottom_Boundary = Map_Pos.y - (Tilemap_Size * 0.5f); // 아래쪽 경계
            float Top_Boundary = Map_Pos.y + (Tilemap_Size * 0.5f);   // 위쪽 경계

            // 플레이어가 경계를 벗어났을 때 타일맵을 이동시키기 위한 오프셋 초기화
            var Move_Offset = Vector3.zero;

            // 플레이어가 왼쪽 경계를 벗어났을 경우
            if (Player_Pos.x < Left_Boundary)
            {
                // 타일맵을 왼쪽으로 이동
                Move_Offset.x = -Tilemap_Size + Tilemap_Overlap_Offset;
            }
            // 플레이어가 오른쪽 경계를 벗어났을 경우
            else if (Player_Pos.x > Right_Boundary)
            {
                // 타일맵을 오른쪽으로 이동
                Move_Offset.x = Tilemap_Size - Tilemap_Overlap_Offset;
            }

            // 플레이어가 아래쪽 경계를 벗어났을 경우
            if (Player_Pos.y < Bottom_Boundary)
            {
                // 타일맵을 아래쪽으로 이동
                Move_Offset.y = -Tilemap_Size + Tilemap_Overlap_Offset;
            }
            // 플레이어가 위쪽 경계를 벗어났을 경우
            else if (Player_Pos.y > Top_Boundary)
            {
                // 타일맵을 위쪽으로 이동
                Move_Offset.y = Tilemap_Size - Tilemap_Overlap_Offset;
            }

            // 플레이어가 경계를 벗어나 타일맵 이동이 필요한 경우
            if (Move_Offset != Vector3.zero)
            {
                // 타일맵의 위치를 오프셋만큼 이동
                tilemap.transform.position += Move_Offset;
            }
        }
    }

    // �� ����Ʈ �ʱ�ȭ
    public void Enemy_List_Remove()
    {
        foreach (Unit_Enemy enemy in Enemy_List)
        {
            enemy.Unit_Clear();
        }

        Enemy_List.Clear();
    }

    public void MISC_List_Remove()
    {
        foreach (MISC misc in MISC_List)
        {
            misc.MISC_Clear();
        }

        MISC_List.Clear();

        All_Manager.instance.UI_M.Update_Gem_Count_Image(0);
    }

    // �� óġ�� ų ī��Ʈ & �� ���
    public void Enemy_Down(Unit_Enemy p_UE)
    {
        Kill_Count++;

        if (p_UE.UT == Unit_Type.Barrel)
        {
            All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Food, p_UE.transform);
        }

        else
        {
            if (p_UE.Elite == false)
            {
                All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Gem, p_UE.transform);
            }

            else
            {
                All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Gem, p_UE.transform);

                All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Chest, p_UE.transform);
            }
        }

        All_Manager.instance.UI_M.Update_Kill_Count_Text(Kill_Count);

        Enemy_List.Remove(p_UE);
    }




    // ����ġ ȹ��
    public void Add_Gem(float p_Gem_Count)
    {
        float Gem_Prev = 0;

        Gem_Prev = Current_Gem;
        Current_Gem += p_Gem_Count;

        Update_Gem_Count_Bar_Fill(Gem_Prev);
    }

    // ������ ȣ�� �� ����ġ ��Ȳ
    public void Update_Gem_Count_Bar_Fill(float Gem_Prev)
    {
        Gem_Prev = Current_Gem;
        All_Manager.instance.UI_M.Update_Gem_Count_Image(Gem_Prev / Max_Gem);

        if (Gem_Prev / Max_Gem >= 1.0f)
        {
            Player_Level_Count++;

            Gem_Prev = 0f;
            Current_Gem = 0f;
            Max_Gem *= 1.3f;

            if (Current_Gem < 0)
            {
                Current_Gem = 0;
            }

            All_Manager.instance.UI_M.Update_Player_Level_Count_Text(Player_Level_Count);   // ���� UI ȣ��
            All_Manager.instance.UI_M.On_Panel(Panel_Style.Level); // ������ ȣ��

            All_Manager.instance.UI_M.isLevelUp = true;
            //UnityEngine.Debug.Log(Current_Gem);
        }


    }

    public void Start_Wave(STO_Wave p_STO_Wave)
    {
        C_Wave_Temp = C_Start_Wave(p_STO_Wave);

        StartCoroutine(C_Wave_Temp);
    }

    public IEnumerator C_Start_Wave(STO_Wave p_STO_Wave)
    {
        if (p_STO_Wave.Spawn_Time == 0)
        {
            while (true)
            {
                if (p_STO_Wave.WST == Wave_Spawn_Type.Random)
                {
                    for (int i = 0; i < p_STO_Wave.Spawn_Count; i++)
                    {
                        int Temp_X = Random.Range(5, 10);
                        Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

                        int Temp_Y = Random.Range(5, 10);
                        Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

                        Vector3 Player_Pos = Current_Player.transform.position;
                        Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

                        if (Game_Stop == false)
                        {
                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Player_Pos);
                            }

                            else
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Player_Pos, true);
                            }
                        }

                        else { }
                    }
                }

                else if (p_STO_Wave.WST == Wave_Spawn_Type.Surround)
                {

                    float Rad = 6.0f;   // �÷��̾� �߽� ���� �������� 6�� ���� ���Ѵ�. 

                    Vector3 Player_Pos = Current_Player.transform.position;

                    float Angle_Step = 360.0f / p_STO_Wave.Spawn_Count;
                    float Angle = 0.0f;

                    for (int i = 0; i < p_STO_Wave.Spawn_Count; i++)
                    {
                        if (Game_Stop == false)
                        {
                            float Enemy_PosX = Player_Pos.x + Mathf.Sin(Angle * Mathf.Deg2Rad) * Rad;
                            float Enemy_PosY = Player_Pos.y + Mathf.Cos(Angle * Mathf.Deg2Rad) * Rad;

                            Vector3 Enemy_Pos = new Vector3(Enemy_PosX, Enemy_PosY, Player_Pos.z);


                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Enemy_Pos);
                            }

                            else
                            {

                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Enemy_Pos, true);

                            }

                            Angle += Angle_Step;
                        }

                        else
                        {

                        }
                    }
                }



                else if (p_STO_Wave.WST == Wave_Spawn_Type.Dot)
                {
                    int Temp_X = Random.Range(5, 10);
                    Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

                    int Temp_Y = Random.Range(5, 10);
                    Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

                    Vector3 Player_Pos = Current_Player.transform.position;
                    Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

                    // �÷��̾� �ֺ��� ������ ���� ���
                    //Vector3 Dot_Spawn = Player_Pos + (Vector3)(Random.insideUnitCircle * spawnRadius);

                    if (Game_Stop == false)
                    {
                        for (int i = 0; i < p_STO_Wave.Spawn_Count; i++)
                        {
                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, (Player_Pos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)));
                            }

                            else
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, (Player_Pos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)), true);
                            }
                        }
                    }

                    else { }
                }

                else if (p_STO_Wave.WST == Wave_Spawn_Type.BossWave)
                {
                    for (int i = 0; i < p_STO_Wave.Spawn_Count; i++)
                    {
                        int Temp_X = Random.Range(5, 10);
                        Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

                        int Temp_Y = Random.Range(5, 10);
                        Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

                        Vector3 Player_Pos = Current_Player.transform.position;
                        Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

                        // STO �� Position �� �Ű������� ������ �ְ�
                        // All_Manager.instance.Unit_M.Create_Unit(STO , Position);
                        // All_Manager.instance.Unit_M.Create_Unit(Unit_Name.Enemy);

                        if (Game_Stop == false)
                        {
                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Unit_M.Unit_F.Create_Boss(All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(Unit_Type.Boss), Player_Pos);
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
            for (int i = 0; i < p_STO_Wave.Spawn_Time; i++)
            {
                if (p_STO_Wave.WST == Wave_Spawn_Type.Random)
                {
                    for (int j = 0; j < p_STO_Wave.Spawn_Count; j++)
                    {
                        int Temp_X = Random.Range(5, 10);
                        Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

                        int Temp_Y = Random.Range(5, 10);
                        Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

                        Vector3 Player_Pos = Current_Player.transform.position;
                        Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

                        // STO �� Position �� �Ű������� ������ �ְ�
                        //All_Manager.instance.Unit_M.Create_Unit(STO , Position);

                        if (Game_Stop == false)
                        {
                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Player_Pos);
                            }

                            else
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Player_Pos, true);
                            }
                        }

                        else { }
                    }
                }

                else if (p_STO_Wave.WST == Wave_Spawn_Type.Surround)
                {

                    float Rad = 6.0f;   // �÷��̾� �߽� ���� �������� 6�� ���� ���Ѵ�. 

                    Vector3 Player_Pos = Current_Player.transform.position;

                    float Angle_Step = 360.0f / p_STO_Wave.Spawn_Count;
                    float Angle = 0.0f;

                    for (int j = 0; j < p_STO_Wave.Spawn_Count; j++)
                    {
                        if (Game_Stop == false)
                        {
                            float Enemy_PosX = Player_Pos.x + Mathf.Sin(Angle * Mathf.Deg2Rad) * Rad;
                            float Enemy_PosY = Player_Pos.y + Mathf.Cos(Angle * Mathf.Deg2Rad) * Rad;

                            Vector3 Enemy_Pos = new Vector3(Enemy_PosX, Enemy_PosY, Player_Pos.z);

                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Enemy_Pos);
                            }

                            else
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, Enemy_Pos, true);
                            }

                            Angle += Angle_Step;
                        }

                        else
                        {

                        }
                    }
                }



                else if (p_STO_Wave.WST == Wave_Spawn_Type.Dot)
                {
                    int Temp_X = Random.Range(5, 10);
                    Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

                    int Temp_Y = Random.Range(5, 10);
                    Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

                    Vector3 Player_Pos = Current_Player.transform.position;
                    Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

                    if (Game_Stop == false)
                    {
                        for (int j = 0; j < p_STO_Wave.Spawn_Count; j++)
                        {

                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, (Player_Pos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)));
                            }

                            else
                            {
                                All_Manager.instance.Pool_M.Enemy_Pool_Out(p_STO_Wave.STO_Enemy.UT, (Player_Pos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)), true);
                            }

                        }
                    }

                    else { }
                }

                else if (p_STO_Wave.WST == Wave_Spawn_Type.BossWave)
                {
                    for (int j = 0; j < p_STO_Wave.Spawn_Count; j++)
                    {
                        int Temp_X = Random.Range(5, 10);
                        Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

                        int Temp_Y = Random.Range(5, 10);
                        Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

                        Vector3 Player_Pos = Current_Player.transform.position;
                        Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

                        if (Game_Stop == false)
                        {
                            if (p_STO_Wave.Elite == false)
                            {
                                All_Manager.instance.Unit_M.Unit_F.Create_Boss(All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(Unit_Type.Boss), Player_Pos);
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
            if (Game_Time >= spawnTime && !spawnedMissions.Contains(spawnTime))
            {
                SpawnRandomMission();
                spawnedMissions.Add(spawnTime);
            }
        }
    }

    private void SpawnRandomMission()
    {
        MissionType randomMissionType = (MissionType)Random.Range(0, System.Enum.GetValues(typeof(MissionType)).Length);

        STO_Mission newMission = All_Manager.instance.Resource_M.Convert_Mission_Type_To_STO_Mission_Arr(randomMissionType);

        if (newMission != null)
        {
            Vector3 randomPosition = Current_Player.transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);

            All_Manager.instance.Unit_M.Mission_F.Create_Mission(newMission, randomPosition);
            All_Manager.instance.UI_M.Update_Mission_Check_Text(randomMissionType.ToString());
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Mission of type '{randomMissionType}' could not be found.");
        }
    }

}

#region
//public void Stop_Gem_Count_Bar_Fill(float Gem_Prev)
//{
//    StopCoroutine(C_Update_Gem_Count_Bar_Fill(Gem_Prev));
//}

//public void Update_Gem_Count_Bar_Fill(float Gem_Prev)
//{
//    StartCoroutine(C_Update_Gem_Count_Bar_Fill(Gem_Prev));
//}

//public IEnumerator C_Update_Gem_Count_Bar_Fill(float Gem_Prev)
//{
//    var Gem_Current = Gem_List[Gem_List.Count - 1].Current_Gem;
//    var Gem_Max = Gem_List[Gem_List.Count - 1].Max_Gem;

//    //All_Manager.instance.UI_M.Update_Gem_Count_Image(Gem_Prev / Gem_Max);

//    //if (Gem_Prev / Gem_Max > 1f)
//    //{
//    //    Gem_Prev = 0f;
//    //    ExpCurrent -= ExpMax;
//    //    ExpMax *= 2.0f;

//    //    // ������ ȣ��
//    //}

//    float process = 0f;

//    while (process < 1f)
//    {
//        process += Time.deltaTime;

//        Gem_Prev = Mathf.Lerp(Gem_Prev, Gem_Current, process);
//        All_Manager.instance.UI_M.Update_Gem_Count_Image(Gem_Prev / Gem_Max);

//        if (Gem_Prev / Gem_Max > 1f)
//        {
//            Gem_Prev = 0f;
//            process = 0f;
//            Gem_Current -= Gem_Max;
//            Gem_Max *= 1.3f;

//            // ������ ȣ��
//        }

//        yield return null;
//    }

//}

//float Gem_Current = 0; //= Gem_List[Gem_List.Count - 1].Current_Gem;
//float Gem_Max = 0; //= Gem_List[Gem_List.Count - 1].Max_Gem;

//for (int i = 0; i < Gem_List.Count; i++)
//{
//    Gem_Current = Gem_List[i].Current_Gem;
//    Gem_Max = Gem_List[i].Max_Gem;
//}

//float process = 0f;

//while (process < 1f)
//{
//    process += Time.deltaTime;

//    Gem_Prev = Mathf.Lerp(Gem_Prev, Gem_Current, process);
//    All_Manager.instance.UI_M.Update_Gem_Count_Image(Gem_Prev / Gem_Max);

//    if (Gem_Prev / Gem_Max > 1f)
//    {
//        Gem_Prev = 0f;
//        process = 0f;
//        Gem_Current -= Gem_Max;
//        Gem_Max *= 1.3f;

//        All_Manager.instance.UI_M.On_Panel(Panel_Style.Level);// ������ ȣ��
//    }

//}
#endregion