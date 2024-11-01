using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

//이거 이 타입 유니티에 연결시켜줘 나 하이라키에서 보게
//직렬화
[System.Serializable]
public enum GameMode_State
{
    Idle,
    Ingame,
    GameOver,
    GameClear
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

    // 킬 카운트
    public int Kill_Count = 0;

    // 젬 카운트
    public int Gem_Count = 0;
    public float Current_Gem;
    public float Max_Gem = 5.0f;
    public float Base_Max_Gem = 5.0f;

    // 플레이어 레벨
    public int Player_Level_Count = 0;

    //게임시간
    public float Game_Time = 0;

    // 사용할 웨이브 리스트
    public List<STO_Wave> Use_STO_Wave_List = new List<STO_Wave>();

    public bool Game_Stop = false;

    IEnumerator C_Wave_Temp;

    //시간차를 이용해서 만들기
    //이보크 함수 - X
    //Update()
    //코루틴

    //코루틴은 시간차를 이용한 기능이나.
    //일정시간 이후에 처리하는 기능을 원할때 사용하는 함수이다.
    //코루틴은 게임의 큰흐름을 강이라고 쳤을때
    //강 에서 빠져나와서 흐르다 다시 큰강으로 돌아오는
    //시냇물이라 볼수 있다.
    //하지만 코루틴을 지나치게 너무 많이 쓰게 되면
    //성능에 영향을 주어 게임을 망칠수 있으니 
    //적절한 사용이 필요하다.

    public float Follow_Speed = 4.0f;
    public float Fixed_Camera_Z = -10.0f;

    // 타일맵 관련
    public float Tilemap_Size = 40.0f;
    public float Tilemap_Boundary_Offset = 1.0f;
    public float Tilemap_Overlap_Offset = 0.1f;


    private void Update()
    {
        All_Manager.instance.UI_M.Update_Pool_Ready_Check_Text(All_Manager.instance.Pool_M.Ready.ToString());
        if (All_Manager.instance.Pool_M.Ready == false) return;
        if (Current_Player != null)
        {
            Current_Camera.transform.position = Vector2.Lerp
            (
                Current_Camera.transform.position, 
                Current_Player.transform.position, 
                Follow_Speed * Time.deltaTime
            );
            Current_Camera.transform.Translate(0, 0, Fixed_Camera_Z); //카메라를 원래 z축으로 이동
            Tilemap_Move();
        }
        
        if (GM_State == GameMode_State.Ingame)
        {
            //Time.timeScale = 1f;

            if (Current_Player == null) return;

            if (Game_Stop == false)
            {
                Game_Time += Time.deltaTime;
            }
            else 
            {
                
            }

            All_Manager.instance.UI_M.Update_Time_Count_Text(Game_Time);

            var Re = All_Manager.instance.Resource_M;

            // GM_State == GameMode_State.Ingame
            for (int i = 0; i < Re.STO_Waves.Length; i++)
            {
                //킬 조건이 맞다면
                if (Re.STO_Waves[i].Kill_Count <= Kill_Count)
                {
                    //시간 조건이 맞다면
                    if (Re.STO_Waves[i].Wave_Time <= Game_Time)
                    {
                        //사용중인 웨이브 리스트에 없다면
                        // Contains 이 리스트에 포함 합니까?
                        if (Use_STO_Wave_List.Contains(Re.STO_Waves[i]) == false)
                        {
                            Use_STO_Wave_List.Add(Re.STO_Waves[i]);

                            Start_Wave(Re.STO_Waves[i]);

                            //UnityEngine.Debug.Log(Re.STO_Waves[i].name + " Start!");
                            All_Manager.instance.UI_M.Update_Wave_Check_Text(Re.STO_Waves[i].name.ToString());
                        }
                    }
                }
            }

            All_Manager.instance.UI_M.Update_State_Check_Text(GameMode_State.Ingame.ToString());
        }

        if (GM_State == GameMode_State.GameOver)
        {
            StopCoroutine(C_Wave_Temp);

            Game_Time = 0;
            Use_STO_Wave_List.Clear();

            All_Manager.instance.UI_M.On_Panel(Panel_Style.GameOver);

            All_Manager.instance.UI_M.Update_State_Check_Text(GameMode_State.GameOver.ToString());
        }

        if (GM_State == GameMode_State.GameClear)
        {
            StopCoroutine(C_Wave_Temp);

            Game_Time = 0;
            Use_STO_Wave_List.Clear();

            All_Manager.instance.UI_M.On_Panel(Panel_Style.GameClear);

            All_Manager.instance.UI_M.Update_State_Check_Text(GameMode_State.GameClear.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Game_Stop == false)
            {
                All_Manager.instance.UI_M.On_Panel(Panel_Style.Pause);
                return;
            }

            else if (Game_Stop == true)
            {
                All_Manager.instance.UI_M.Off_Panel();
                return;
            }
        }

    }

    //게임 시작 함수
    public void Game_Start()
    {
        All_Manager.instance.Unit_M.Unit_F.Create_Player(All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(Unit_Type.Player));

        GM_State = GameMode_State.Ingame;

        All_Manager.instance.UI_M.On_Panel(Panel_Style.GameStart);
    }

    // 게임 재시작
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
        
        GM_State = GameMode_State.Ingame;

        All_Manager.instance.UI_M.On_Panel(Panel_Style.GameStart);
    }

    // 게임 나가기
    public void Game_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    // 게임 일시정지
    public void Button_Game_Pause()
    {
        All_Manager.instance.UI_M.On_Panel(Panel_Style.Pause);
    }

    // 게임 계속하기
    public void Button_Game_Continue()
    {
        All_Manager.instance.UI_M.Off_Panel();
    }

    // 타일맵 이동
    public void Tilemap_Move()
    {
        Vector3 Player_Pos = Current_Player.transform.position;

        for (int i = 0; i < Tilemap_List.Count; i++)
        {
            GameObject tilemap = Tilemap_List[i];
            Vector3 Map_Pos = tilemap.transform.position;

            // 타일맵 경계 계산
            float Left_Boundary = Map_Pos.x - (Tilemap_Size * 0.5f);
            float Right_Boundary = Map_Pos.x + (Tilemap_Size * 0.5f);
            float Bottom_Boundary = Map_Pos.y - (Tilemap_Size * 0.5f);
            float Top_Boundary = Map_Pos.y + (Tilemap_Size * 0.5f);
            
            // 플레이어가 경계를 넘었는지 확인
            var Move_Offset = Vector3.zero;

            if (Player_Pos.x < Left_Boundary)
            {
                // 왼쪽 경계를 넘은 경우
                Move_Offset.x = -Tilemap_Size + Tilemap_Overlap_Offset;
            }
            else if (Player_Pos.x > Right_Boundary)
            {
                // 오른쪽 경계를 넘은 경우
                Move_Offset.x = Tilemap_Size - Tilemap_Overlap_Offset;
            }

            if (Player_Pos.y < Bottom_Boundary)
            {
                // 아래 경계를 넘은 경우
                Move_Offset.y = -Tilemap_Size + Tilemap_Overlap_Offset;
            }
            else if (Player_Pos.y > Top_Boundary)
            {
                // 위쪽 경계를 넘은 경우
                Move_Offset.y = Tilemap_Size - Tilemap_Overlap_Offset;
            }

            // 타일맵 이동(대각선 포함)
            if (Move_Offset != Vector3.zero)
            {
                // 타일맵 이동
                tilemap.transform.position += Move_Offset;
            }
        }

    }

    // 적 리스트 초기화
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

    // 적 처치시 킬 카운트 & 젬 드랍
    public void Enemy_Down(Unit_Enemy p_UE)
    {
        Kill_Count++;

        if (p_UE.UT == Unit_Type.Barrel)
        {
            All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Food, p_UE);
        }

        else
        {
            if (p_UE.Elite == false)
            {
                All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Gem, p_UE);
            }

            else
            {
                All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Gem, p_UE);

                All_Manager.instance.Pool_M.MISC_Pool_Out(MISC_Type.Chest, p_UE);
            }
        }

        All_Manager.instance.UI_M.Update_Kill_Count_Text(Kill_Count);

        Enemy_List.Remove(p_UE);
    }




    // 경험치 획득
    public void Add_Gem(float p_Gem_Count)
    {
        float Gem_Prev = 0;

        Gem_Prev = Current_Gem;
        Current_Gem += p_Gem_Count;

        Update_Gem_Count_Bar_Fill(Gem_Prev);
    }

    // 레벨업 호출 및 경험치 현황
    public void Update_Gem_Count_Bar_Fill(float Gem_Prev)
    {

        Gem_Prev = Current_Gem;
        All_Manager.instance.UI_M.Update_Gem_Count_Image(Gem_Prev / Max_Gem);

        if (Gem_Prev / Max_Gem >= 1f)
        {
            Player_Level_Count++;

            Gem_Prev = 0f;
            Current_Gem = 0f;
            Max_Gem *= 1.2f;

            if (Current_Gem < 0)
            {
                Current_Gem = 0;
            }

            All_Manager.instance.UI_M.Update_Player_Level_Count_Text(Player_Level_Count);   // 레벨 UI 호출
            All_Manager.instance.UI_M.On_Panel(Panel_Style.Level); // 레벨업 호출

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

                        // STO 랑 Position 를 매개변수로 받을수 있게
                        // All_Manager.instance.Unit_M.Create_Unit(STO , Position);
                        // All_Manager.instance.Unit_M.Create_Unit(Unit_Name.Enemy);

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

                    float Rad = 6.0f;   // 플레이어 중심 기준 반지름이 6인 원을 구한다. 

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

                    // 플레이어 주변의 랜덤한 지점 계산
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

                        // STO 랑 Position 를 매개변수로 받을수 있게
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

                        // STO 랑 Position 를 매개변수로 받을수 있게
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

                    float Rad = 6.0f;   // 플레이어 중심 기준 반지름이 6인 원을 구한다. 

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

//    //    // 레벨업 호출
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

//            // 레벨업 호출
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

//        All_Manager.instance.UI_M.On_Panel(Panel_Style.Level);// 레벨업 호출
//    }

//}
#endregion