using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public enum Panel_Style
{
    Pause,
    Level,
    GameOver,
    GameStart,
    TreasureFound,
    GameClear
}

public class UI_Manager : MonoBehaviour
{
    [Header("기본 UI")]
    public Text Kill_Count_Text;
    public Text Time_Count_Text;
    public Text Player_Level_Count_Text;
    public Image Gem_Count_Image;
    public Image Player_HPbar_Image;

    [Header("인게임 싱크 UI")]
    public Image Peachone_Range_Image;
    public Image Peachone_Image;
    public Text Wave_Text;
    public Text Game_State_Text;
    public Text Panel_Style_Text;
    public Text Game_Pool_Ready_Text;

    [Header("UI캔버스")]
    public GameObject Basic_Panel;
    public GameObject Ingame_Sink_Panel;
    public GameObject Player_Information_Panel;
    public GameObject Pause_Panel;
    public GameObject Select_Item_Panel;
    public GameObject Game_Over_Panel;
    public GameObject Game_Start_Panel;
    public GameObject Game_Title_Panel;
    public GameObject Game_Clear_Panel;
    public GameObject Game_End_Panel;
    public GameObject Treasure_Chest_Panel;
    public GameObject Ingame_Debug_Log;

    [Header("타이틀 화면")]
    public Button[] Start_Weapon_Button;        // 시작 버튼 누르고 나오는 무기 선택 버튼
    
    public Image[] Start_Weapon_Icon_Image;     // 시작 무기 아이콘
    public Text[] Start_Weapon_Name_Text;       // 이름
    public Text[] Start_Weapon_Example_Text;    // 설명

    public Text Start_Loding_Text;  // 시작 할때 로딩 중임을 알리는 텍스트

    [Header("시작 무기 이넘")]
    public int Start_First_Weapon_Number;   // 시작 무기 버튼 1에서 나올 무기
    public int Start_Second_Weapon_Number;  // 시작 무기 버튼 2에서 나올 무기

    [Header("아이템 아이콘")]
    public List<Sprite> Weapon_Icon_List = new List<Sprite>();      // 획득한 무기의 아이콘 저장
    public List<Sprite> Equipment_Icon_List = new List<Sprite>();   // 획득한 장신구의 아이콘 저장

    [Header("아이템 이넘")]
    public int Select_First_Item_Number;    // 첫 번째 버튼에서 나올 아이템 
    public int Select_Second_Item_Number;   // 두 번째 버튼에서 나올 아이템
    public int Select_Third_Item_Number;    // 세 번째 버튼에서 나올 아이템


    [Header("아이템 선택창")]
    public Button[] Select_Item_Button;     // 아이템 선택 버튼

    public Image[] Select_Item_Icon_Image;  // 선택할 아이템의 이미지
    public Text[] Select_Item_Name_Text;    // 선택할 아이템의 이름
    public Text[] Select_Item_Example_Text; // 선택할 아이템의 설명

    [Header("플레이어 정보창")]
    // 인게임 플레이 화면의 현재 무기와 장신구 아이콘
    public Image[] Basic_Current_Weapon_Icon_Image;
    public Image[] Basic_Current_Equipment_Icon_Image;

    // 인게임 UI로 띄워주는 무기 레벨
    public Text[] Weapon_Level_Text;
    public Text[] Equipment_Level_Text;

    // 플레이어 정보창 현재 무기와 장신구 아이콘
    public Image[] Player_Info_s_Current_Weapon_Icon_Image;
    public Image[] Player_Info_s_Current_Equipment_Icon_Image;

    // 플레이어 정보창 스탯
    public Text[] Player_Info_s_Pase_One_Text;
    public Text[] Player_Info_s_Pase_Two_Text;

    [Header("인게임 싱크 UI용")]
    public float W_Peachone_Deg = 0;    // Peachone의 각도(플레이어 주변을 돈다).
    public float W_Peachone_Range_Deg = 0;  // Peachone의 공격 범위(플레이어 주변을 돈다).

    public bool isLevelUp = false;  // 레벨업 상태인지 확인용

    public float Speed = 1.0f;  // 레벨업시 경험치바 점멸 속도.
    public float Current_Blue = 0.0f;   // 현재 RGBA중 B색깔
    public bool Blinking = false;   // 점멸 할꺼냐.

    [Header("보물상자 UI용")]
    public GameObject Treasure_Item_Image_01;
    public GameObject Treasure_Item_Image_02;
    public GameObject Treasure_Gem_Image_List;
    public Image Treasure_Reel_Image;
    public Image Treasure_Item_Mask;
    public Image Treasure_Select_Item_Image;
    public Text Treasure_Found_Text;
    public Text Treasure_Got_Gem;

    public GameObject Treasure_Gem_Parent;

    public Image Treasure_Chest_Image;

    public Sprite Close_Chest;
    public Sprite Open_Chest;

    public Button Treasure_Open_Button;
    public Button Treasure_Close_Button;

    public bool Treasure_Stop = false;
    public bool Treasure_Open_Chset = false;

    public Vector3[] Treasure_Gem_Vec_Pos = new Vector3[4];

    public float Treasure_Gem_Timer = 0;
    public int Treasure_Gem_Timer_Power = 1;

    public float Treasure_Timer = 0.0f;

    public int Treasure_Item_Number = 0;

    //public float Treasure_Start_Pos_Y = 0;
    //public float Treasure_Reset_Pos_Y = 2300;

    public float Mask_Roulette_Time = 0.0f;
    public float Mask_Roulette_Pos = 0.0f;

    //public float Mask_Roulette_Speed = 1500.0f;
    //public float Mask_Roulette_Move_Spd = 400.0f;

    //public int Treasure_Got_Gem_Number = 0;


    [Header("OnGUI용")]
    public string[] WP_Options;
    public string[] EQ_Options;
    public string[] PS_Options;

    public bool Weapon_On = false;
    public bool Equipment_On = false;
    public bool PanelStyle_On = false;

    private void Update()
    {
        if (All_Manager.instance.Pool_M.Ready == false) return;

        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            Update_Peachone_Range_Image();
            Update_Peachone_Image();
        }

        else
        {

        }

        if (isLevelUp == true)
        {
            Gem_Bar_Blinking();
        }

        else
        {
            Gem_Count_Image.color = new Color(0, 1, 1, 1);
        }

        Loding_Text();

        Update_MaxHPBar_Position_Image();

        if (Treasure_Open_Chset == true)
        {
            Treasure_Chest_Roulette();
        }
    }


    public void Gem_Bar_Blinking()
    {
        Color color = Gem_Count_Image.color;

        if (Blinking)
        {
            float Max_B = 1.0f;

            Current_Blue += Speed * Time.deltaTime;
            if (Current_Blue >= Max_B)
            {
                Current_Blue = Max_B;
                Blinking = false;
            }
        }
        else
        {
            float Min_B = 0.0f;

            Current_Blue -= Speed * Time.deltaTime;
            if (Current_Blue <= Min_B)
            {
                Current_Blue = Min_B;
                Blinking = true;
            }
        }

        color.b = Current_Blue;
        Gem_Count_Image.color = color;
    }


    public void Update_Wave_Check_Text(string p_Text)
    {
        Wave_Text.text = p_Text;
    }

    public void Update_State_Check_Text(string p_Text)
    {
        Game_State_Text.text = p_Text;
    }

    public void Update_Panel_Check_Text(string p_Text)
    {
        Panel_Style_Text.text = p_Text;
    }

    public void Update_Pool_Ready_Check_Text(string p_Text)
    {
        Game_Pool_Ready_Text.text = p_Text;
    }

    // 킬 카운트 갱신
    public void Update_Kill_Count_Text(int p_Count)
    {
        Kill_Count_Text.text = "\t : " + p_Count;
    }

    // 플레이어 현재 체력 갱신
    public void Update_Player_HPBar_Image(float p_FillAmount_Value)
    {
        Player_HPbar_Image.fillAmount = p_FillAmount_Value;
    }

    // 플레이어 레벨 카운트 갱신
    public void Update_Player_Level_Count_Text(int p_Level_Count)
    {
        Player_Level_Count_Text.text = "Lv . " + p_Level_Count;
    }

    // 무기 레벨 카운트 갱신
    public void Update_Weapon_Level_Count_Text(int Index, int p_Weapon_Level_Count)
    {
        Weapon_Level_Text[Index].text = "Lv. " + p_Weapon_Level_Count;
    }

    // 장신구 레벨 카운트 갱신
    public void Update_Equipment_Level_Count_Text(int Index, int p_Equipment_Level_Count)
    {
        Equipment_Level_Text[Index].text = "Lv. " + p_Equipment_Level_Count;
    }

    // 게임 시간 경과 갱신
    public void Update_Time_Count_Text(float p_Time_Count)
    {
        Time_Count_Text.text = ((int)p_Time_Count / 60 % 60).ToString("D2") + " : " + ((int)p_Time_Count % 60).ToString("D2");
    }

    // 경험치 바 갱신
    public void Update_Gem_Count_Image(float p_Fill_Amount_Gem_Count)
    {
        Gem_Count_Image.fillAmount = p_Fill_Amount_Gem_Count;
    }

    // 클릭 테스트
    public void Click_Test()
    {
        Debug.Log("fd");
    }

    // Peachone무기 공격범위
    public void Update_Peachone_Range_Image()
    {
        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
            var W_UI_Peachone_Range_S = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Peachone).Info_s[Temp_Player.Peachone_LAT.Level];

            if (All_Manager.instance.GameMode_M.Current_Player.Peachone_LAT.Level == 0)
            {
                Peachone_Range_Image.gameObject.SetActive(false);
            }

            else
            {
                Peachone_Range_Image.gameObject.SetActive(true);

                Peachone_Range_Image.rectTransform.sizeDelta = new Vector2(W_UI_Peachone_Range_S.Weapon_Range, W_UI_Peachone_Range_S.Weapon_Range);

                W_Peachone_Range_Deg += Time.deltaTime * W_UI_Peachone_Range_S.Weapon_Speed * 20.0f;
                if (W_Peachone_Range_Deg < 360)
                {
                    var rad = Mathf.Deg2Rad * (W_Peachone_Range_Deg);
                    var x = W_UI_Peachone_Range_S.Weapon_Range * Mathf.Sin(rad);
                    var y = W_UI_Peachone_Range_S.Weapon_Range * Mathf.Cos(rad);
                    Peachone_Range_Image.rectTransform.position = Temp_Player.transform.position + new Vector3(x, y);
                }
                else
                {
                    W_Peachone_Range_Deg = 0;
                }
            }

        }

        else
        {
            Peachone_Range_Image.gameObject.SetActive(false);
        }
    }

    // Peachone무기 이미지
    public void Update_Peachone_Image()
    {
        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;

            if (All_Manager.instance.GameMode_M.Current_Player.Peachone_LAT.Level == 0)
            {
                Peachone_Image.gameObject.SetActive(false);
            }

            else
            {
                Peachone_Image.gameObject.SetActive(true);

                W_Peachone_Deg += Time.deltaTime * 10f;
                if (W_Peachone_Deg < 360)
                {
                    var rad = Mathf.Deg2Rad * (W_Peachone_Deg);
                    var x = 0.7f * Mathf.Sin(rad);
                    var y = 0.7f * Mathf.Cos(rad);
                    Peachone_Image.rectTransform.position = Temp_Player.transform.position + new Vector3(x, y);
                }
                else
                {
                    W_Peachone_Deg = 0;
                }
            }

        }

        else
        {
            Peachone_Image.gameObject.SetActive(false);
        }

    }

    // 플레이어를 따라오는 체력
    public void Update_MaxHPBar_Position_Image()
    {
        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Player_Position = All_Manager.instance.GameMode_M.Current_Player;

            Player_HPbar_Image.rectTransform.position = Player_Position.transform.position + new Vector3(0, -0.6f, 0);
        }
    }

    // 패널 킴
    public void On_Panel(Panel_Style p_PS)
    {

        All_Manager.instance.GameMode_M.Game_Stop = true;

        if (p_PS == Panel_Style.Pause)
        {
            Update_Player_Information_Panel();

            Player_Information_Panel.SetActive(true);
            Pause_Panel.SetActive(true);

        }

        else if (p_PS == Panel_Style.Level)
        {
            Update_Player_Information_Panel();
            Renew_Item_Panel();

            All_Manager.instance.Audio_M.Level_Up_Audio_Play();
            Player_Information_Panel.SetActive(true);
            Select_Item_Panel.SetActive(true);
        }

        else if (p_PS == Panel_Style.GameOver)
        {
            Game_Over_Panel.SetActive(true);
            Game_End_Panel.SetActive(true);
        }

        else if (p_PS == Panel_Style.GameStart)
        {
           Game_Title_Panel.SetActive(true);
        }

        else if (p_PS == Panel_Style.TreasureFound)
        {
            Treasure_Chest_Panel.SetActive(true);
        }

        else if (p_PS == Panel_Style.GameClear)
        {
            Game_Clear_Panel.SetActive(true);
            Game_End_Panel.SetActive(true);
        }


        All_Manager.instance.Audio_M.Background_Audio_Play(p_PS);

        Update_Panel_Check_Text(p_PS.ToString());
    }

    // 패널 끔
    public void Off_Panel()
    {
        All_Manager.instance.GameMode_M.Game_Stop = false;

        Update_Player_Information_Panel();

        Player_Information_Panel.SetActive(false);
        Select_Item_Panel.SetActive(false);
        Pause_Panel.SetActive(false);
        Game_Over_Panel.SetActive(false);
        Game_Start_Panel.SetActive(false);
        Game_Title_Panel.SetActive(false);
        Game_End_Panel.SetActive(false);
        Game_Clear_Panel.SetActive(false);
        Treasure_Chest_Panel.SetActive(false);

        isLevelUp = false;

        All_Manager.instance.Audio_M.Background_AS.UnPause();
    }

    public void Loding_Text()
    {
        if (All_Manager.instance.Pool_M.Ready == true)
        {
            Start_Loding_Text.gameObject.SetActive(false);
        }
    }

    public void Start_Select()
    {
        if (All_Manager.instance.Pool_M.Ready == true)
        {
            Game_Title_Panel.SetActive(false);

            Renew_Start_Weapon_Panel();

            Game_Start_Panel.SetActive(true);
        }

        else
        {
            return;
        }
    }

    public void Update_Player_Information_Panel()
    {
        Renew_Player_Info_Stat();
        Renew_Current_Weapon_Icon();
        Renew_Current_Equipment_Icon();
    }

    public void Renew_Start_Weapon_Panel()
    {
        List<int> Selected_Start_Weapon_List = new List<int>();
        do
        {
            Start_First_Weapon_Number = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(Weapon_Type)).Length - 1);

        } while (Selected_Start_Weapon_List.Contains(Start_First_Weapon_Number));

        Selected_Start_Weapon_List.Add(Start_First_Weapon_Number);

        do
        {
            Start_Second_Weapon_Number = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(Weapon_Type)).Length - 1);

        } while (Selected_Start_Weapon_List.Contains(Start_Second_Weapon_Number));

        Selected_Start_Weapon_List.Add(Start_Second_Weapon_Number);


        var Temp_WT_First = (Weapon_Type)Start_First_Weapon_Number;

        STO_Weapon Convert_Weapon_First = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Temp_WT_First);

        Start_Weapon_Icon_Image[0].sprite = Convert_Weapon_First.Item_Image;
        Start_Weapon_Name_Text[0].text = Convert_Weapon_First.Weapon_Name;
        Start_Weapon_Example_Text[0].text = Convert_Weapon_First.Weapon_Text;

        var Temp_WT_Second = (Weapon_Type)Start_Second_Weapon_Number;

        Weapon_Select_Assign_Info(1, Temp_WT_Second);

        STO_Weapon Convert_Weapon_Second = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Temp_WT_Second);

        Start_Weapon_Icon_Image[1].sprite = Convert_Weapon_Second.Item_Image;
        Start_Weapon_Name_Text[1].text = Convert_Weapon_Second.Weapon_Name;
        Start_Weapon_Example_Text[1].text = Convert_Weapon_Second.Weapon_Text;
    }

    public void Click_Start_Weapon_Select(int p_Index)
    {

        if (p_Index == 0)
        {
            var Temp_WT = (Weapon_Type)Start_First_Weapon_Number;

            Weapon_Select_Assign_Button(Temp_WT);
        }

        else if (p_Index == 1)
        {
            var Temp_WT = (Weapon_Type)Start_Second_Weapon_Number;

            Weapon_Select_Assign_Button(Temp_WT);
        }

        Off_Panel();
    }

    public void Renew_Player_Info_Stat()
    {
        // 페이지 1
        Player_Info_s_Pase_One_Text[0].text = "" + All_Manager.instance.GameMode_M.Current_Player.Origin_Stat.Unit_HP;
        Player_Info_s_Pase_One_Text[1].text = "" + All_Manager.instance.GameMode_M.Current_Player.Origin_Stat.Unit_Speed;
        Player_Info_s_Pase_One_Text[2].text = "" + All_Manager.instance.GameMode_M.Current_Player.Origin_Stat.Unit_Defense + "%";
        Player_Info_s_Pase_One_Text[3].text = "" + All_Manager.instance.GameMode_M.Current_Player.Percent_Stat.Unit_Weapon_Damage + "%";
        Player_Info_s_Pase_One_Text[4].text = "" + All_Manager.instance.GameMode_M.Current_Player.Origin_Stat.Unit_Weapon_Proj_Speed + "%";

        // 페이지 2
        Player_Info_s_Pase_Two_Text[0].text = "" + All_Manager.instance.GameMode_M.Current_Player.Percent_Stat.Unit_Weapon_Duration + "%";
        Player_Info_s_Pase_Two_Text[1].text = "" + All_Manager.instance.GameMode_M.Current_Player.Percent_Stat.Unit_Weapon_Range + "%";
        Player_Info_s_Pase_Two_Text[2].text = "-" + All_Manager.instance.GameMode_M.Current_Player.Percent_Stat.Unit_Weapon_CoolDown + "%";
        Player_Info_s_Pase_Two_Text[3].text = "" + All_Manager.instance.GameMode_M.Current_Player.Percent_Stat.Unit_Weapon_Proj_Count;
        Player_Info_s_Pase_Two_Text[4].text = "" + All_Manager.instance.GameMode_M.Current_Player.Percent_Stat.Unit_Get_Gem_Range;
    }

    public void Renew_Item_Panel()
    {
        Renew_Current_Weapon_Icon();
        Renew_Current_Equipment_Icon();

        int Max = System.Enum.GetValues(typeof(Weapon_Type)).Length + System.Enum.GetValues(typeof(Equipment_Type)).Length;
        List<int> Selected_Item_List = new List<int>();
        do
        {   // 첫번째 패널에 나타날 아이템 룰렛 
            Select_First_Item_Number = UnityEngine.Random.Range(0, Max - 1);

        } while (Selected_Item_List.Contains(Select_First_Item_Number));

        Selected_Item_List.Add(Select_First_Item_Number);


        do
        {   // 두번째 패널에 나타날 아이템 룰렛 
            Select_Second_Item_Number = UnityEngine.Random.Range(0, Max - 1);

        } while (Selected_Item_List.Contains(Select_Second_Item_Number));

        Selected_Item_List.Add(Select_Second_Item_Number);

        do
        {   // 세번째 패널에 나타날 아이템 룰렛 
            Select_Third_Item_Number = UnityEngine.Random.Range(0, Max - 1);

        } while (Selected_Item_List.Contains(Select_Third_Item_Number));

        Selected_Item_List.Add(Select_Third_Item_Number);


        //Select_First_Item_Number
        if (Select_First_Item_Number < System.Enum.GetValues(typeof(Weapon_Type)).Length)
        {
            //무기
            var Temp_WT = (Weapon_Type)Select_First_Item_Number;
            Weapon_Select_Assign_Info(0, Temp_WT);
        }

        else
        {
            //장비
            var Temp_ET = (Equipment_Type)(Select_First_Item_Number - System.Enum.GetValues(typeof(Weapon_Type)).Length);
            Equipment_Select_Assign_Info(0, Temp_ET);
        }


        //Select_Second_Item_Number
        if (Select_Second_Item_Number < System.Enum.GetValues(typeof(Weapon_Type)).Length)
        {
            //무기
            var Temp_WT = (Weapon_Type)Select_Second_Item_Number;
            Weapon_Select_Assign_Info(1, Temp_WT);
        }

        else
        {
            //장비
            var Temp_ET = (Equipment_Type)(Select_Second_Item_Number - System.Enum.GetValues(typeof(Weapon_Type)).Length);
            Equipment_Select_Assign_Info(1, Temp_ET);
        }

        //Select_Third_Item_Number
        if (Select_Third_Item_Number < System.Enum.GetValues(typeof(Weapon_Type)).Length)
        {
            //무기
            var Temp_WT = (Weapon_Type)Select_Third_Item_Number;
            Weapon_Select_Assign_Info(2, Temp_WT);
        }

        else
        {
            //장비
            var Temp_ET = (Equipment_Type)(Select_Third_Item_Number - System.Enum.GetValues(typeof(Weapon_Type)).Length);
            Equipment_Select_Assign_Info(2, Temp_ET);
        }
    }

    public void Click_Select_Item_Button(int p_Index)
    {
        //Select_First_Item_Number 
        if (p_Index == 0)
        {
            if (Select_First_Item_Number < System.Enum.GetValues(typeof(Weapon_Type)).Length)
            {
                var Temp_WT = (Weapon_Type)Select_First_Item_Number;

                Weapon_Select_Assign_Button(Temp_WT);
            }

            else
            {
                var Temp_ET = (Equipment_Type)(Select_First_Item_Number - System.Enum.GetValues(typeof(Weapon_Type)).Length);

                Equipment_Select_Assign_Button(Temp_ET);
            }
        }

        //Select_Second_Item_Number
        else if (p_Index == 1)
        {
            if (Select_Second_Item_Number < System.Enum.GetValues(typeof(Weapon_Type)).Length)
            {
                var Temp_WT = (Weapon_Type)Select_Second_Item_Number;

                Weapon_Select_Assign_Button(Temp_WT);
            }

            else
            {
                var Temp_ET = (Equipment_Type)(Select_Second_Item_Number - System.Enum.GetValues(typeof(Weapon_Type)).Length);

                Equipment_Select_Assign_Button(Temp_ET);
            }
        }

        //Select_Third_Item_Number
        else if (p_Index == 2)
        {
            if (Select_Third_Item_Number < System.Enum.GetValues(typeof(Weapon_Type)).Length)
            {
                var Temp_WT = (Weapon_Type)Select_Third_Item_Number;

                Weapon_Select_Assign_Button(Temp_WT);
            }

            else
            {
                var Temp_ET = (Equipment_Type)(Select_Third_Item_Number - System.Enum.GetValues(typeof(Weapon_Type)).Length);

                Equipment_Select_Assign_Button(Temp_ET);
            }
        }

        Off_Panel();
        Update_Gem_Count_Image(0);
    }

    public void Weapon_Select_Assign_Button(Weapon_Type p_WT)
    {
        STO_Weapon Temp_WT = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        var Player_Weapon_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WT);

        if (Player_Weapon_Level.Level >= 8)
        {
            All_Manager.instance.GameMode_M.Current_Player.Recovery_Current_HP();
        }

        else
        {
            All_Manager.instance.GameMode_M.Current_Player.Upgrade_Weapon(p_WT);

            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                if (Temp_WT != null)
                {
                    bool Sprite_Currect = Weapon_Icon_List.Contains(Temp_WT.Item_Image);

                    if (!Sprite_Currect)
                    {
                        Weapon_Icon_List.Add(Temp_WT.Item_Image);
                    }

                    else
                    {
                        //Debug.Log("이미 무기 아이콘이 존재합니다");
                    }

                    for (int i = 0; i < Weapon_Icon_List.Count; i++)
                    {
                        if (Weapon_Icon_List[i] == Temp_WT.Item_Image)
                        {
                            int Weapon_Level_Temp = 0;

                            for (int j = 0; j < Player_Weapon_Level.Level; j++)
                            {
                                Weapon_Level_Temp++;
                            }

                            Basic_Current_Weapon_Icon_Image[i].GetComponentInChildren<Text>().text = "Lv. " + Weapon_Level_Temp;
                        }
                    }

                    Renew_Current_Weapon_Icon();
                }
            }
        }


    }

    public void Equipment_Select_Assign_Button(Equipment_Type p_ET)
    {
        STO_Equipment Temp_ET = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(p_ET);

        var Player_Equipment_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(p_ET);

        if (Player_Equipment_Level >= 5)
        {
            All_Manager.instance.GameMode_M.Current_Player.Recovery_Current_HP();
        }

        else
        {
            All_Manager.instance.GameMode_M.Current_Player.Upgrade_Equipment(p_ET);

            All_Manager.instance.GameMode_M.Current_Player.Origin_Stat_Application_Player(p_ET);

            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                if (Temp_ET != null)
                {

                    bool Sprite_Currect = Equipment_Icon_List.Contains(Temp_ET.Sprite_Image);

                    if (!Sprite_Currect)
                    {
                        Equipment_Icon_List.Add(Temp_ET.Sprite_Image);
                    }

                    else
                    {
                        //Debug.Log("이미 장신구 아이콘이 존재합니다.");
                    }

                    for (int i = 0; i < Equipment_Icon_List.Count; i++)
                    {
                        if (Equipment_Icon_List[i] == Temp_ET.Sprite_Image)
                        {
                            int Equipment_Level_Temp = 0;

                            var Temp_Current_Equipment_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(p_ET);

                            for (int j = 0; j < Temp_Current_Equipment_Level; j++)
                            {
                                Equipment_Level_Temp++;
                            }

                            Basic_Current_Equipment_Icon_Image[i].GetComponentInChildren<Text>().text = "Lv. " + Equipment_Level_Temp;
                        }

                    }

                    Renew_Current_Equipment_Icon();
                }

            }
        }


    }

    public void Weapon_Select_Assign_Info(int Index, Weapon_Type p_WT)
    {
        STO_Weapon Temp_WT = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);

        STO_MISC Temp_Food = All_Manager.instance.Resource_M.Convert_MISC_Type_To_STO_MISC_Arr(MISC_Type.Food);

        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Player_Weapon_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WT);

            if (Player_Weapon_Level.Level >= 8)
            {
                if (Select_Item_Icon_Image[Index] == null) Debug.Log(2);
                Select_Item_Icon_Image[Index].sprite = Temp_Food.Sprite_Image;

                if (Select_Item_Name_Text[Index] == null) Debug.Log(3);
                Select_Item_Name_Text[Index].text = Temp_Food.MISC_Name;

                if (Select_Item_Example_Text[Index] == null) Debug.Log(4);
                Select_Item_Example_Text[Index].text = Temp_Food.MISC_Text;
            }

            else
            {
                if (Select_Item_Icon_Image[Index] == null) Debug.Log(2);
                Select_Item_Icon_Image[Index].sprite = Temp_WT.Item_Image;

                if (Select_Item_Name_Text[Index] == null) Debug.Log(3);
                Select_Item_Name_Text[Index].text = Temp_WT.Weapon_Name;

                if (Select_Item_Example_Text[Index] == null) Debug.Log(4);
                Select_Item_Example_Text[Index].text = Temp_WT.Weapon_Text;
            }
        }
    }

    public void Equipment_Select_Assign_Info(int Index, Equipment_Type p_ET)
    {
        STO_Equipment Temp_ET = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(p_ET);

        STO_MISC Temp_Food = All_Manager.instance.Resource_M.Convert_MISC_Type_To_STO_MISC_Arr(MISC_Type.Food);

        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Player_Equipment_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(p_ET);

            if (Player_Equipment_Level >= 5)
            {
                if (Select_Item_Icon_Image[Index] == null) Debug.Log(2);
                Select_Item_Icon_Image[Index].sprite = Temp_Food.Sprite_Image;

                if (Select_Item_Name_Text[Index] == null) Debug.Log(3);
                Select_Item_Name_Text[Index].text = Temp_Food.MISC_Name;

                if (Select_Item_Example_Text[Index] == null) Debug.Log(4);
                Select_Item_Example_Text[Index].text = Temp_Food.MISC_Text;
            }

            else
            {
                Select_Item_Icon_Image[Index].sprite = Temp_ET.Sprite_Image;
                Select_Item_Name_Text[Index].text = Temp_ET.Equipment_Name;
                Select_Item_Example_Text[Index].text = Temp_ET.Equipment_Text;
            }
        }


        //Select_Item_Button[Index].onClick.AddListener(() => { All_Manager.instance.GameMode_M.Current_Player.Update_Get_Equipment(p_ET); }); 
    }

    // 인게임 무기 아이콘 갱신
    public void Renew_Current_Weapon_Icon()
    {
        if (All_Manager.instance.GameMode_M.Current_Player.Current_HP <= 0)
        {
            for (int i = 0; i < Basic_Current_Weapon_Icon_Image.Length; i++)
            {
                Basic_Current_Weapon_Icon_Image[i].gameObject.SetActive(false);
                Player_Info_s_Current_Weapon_Icon_Image[i].gameObject.SetActive(false);

                var Temp_WTO = All_Manager.instance.Resource_M.STO_Weapons;

                for (int j = 0; j < Temp_WTO.Length; j++)
                {
                    var Temp_WT = Temp_WTO[j].WT;

                    var Temp_Weapon_Sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Temp_WT).Item_Image;

                    if (Weapon_Icon_List.Contains(Temp_Weapon_Sprite))
                    {
                        Weapon_Icon_List.Remove(Temp_Weapon_Sprite);
                    }

                }
            }
        }

        else
        {
            for (int i = 0; i < Weapon_Icon_List.Count; i++)
            {
                Basic_Current_Weapon_Icon_Image[i].sprite = Weapon_Icon_List[i];
                Basic_Current_Weapon_Icon_Image[i].gameObject.SetActive(true);

                Player_Info_s_Current_Weapon_Icon_Image[i].sprite = Weapon_Icon_List[i];
                Player_Info_s_Current_Weapon_Icon_Image[i].gameObject.SetActive(true);
            }
        }

    }

    // 인게임 장신구 아이콘 갱신
    public void Renew_Current_Equipment_Icon()
    {
        if (All_Manager.instance.GameMode_M.Current_Player.Current_HP <= 0)
        {
            for (int i = 0; i < Basic_Current_Equipment_Icon_Image.Length; i++)
            {
                Basic_Current_Equipment_Icon_Image[i].gameObject.SetActive(false);
                Player_Info_s_Current_Equipment_Icon_Image[i].gameObject.SetActive(false);

                var Temp_ETO = All_Manager.instance.Resource_M.STO_Equipments;

                for (int j = 0; j < Temp_ETO.Length; j++)
                {
                    var Temp_ET = Temp_ETO[j].ET;

                    var Temp_Equipment_Sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Temp_ET).Sprite_Image;

                    if (Equipment_Icon_List.Contains(Temp_Equipment_Sprite))
                    {
                        Equipment_Icon_List.Remove(Temp_Equipment_Sprite);

                    }

                }
            }

        }

        else
        {
            for (int i = 0; i < Equipment_Icon_List.Count; i++)
            {
                Basic_Current_Equipment_Icon_Image[i].sprite = Equipment_Icon_List[i];
                Basic_Current_Equipment_Icon_Image[i].gameObject.SetActive(true);

                Player_Info_s_Current_Equipment_Icon_Image[i].sprite = Equipment_Icon_List[i];
                Player_Info_s_Current_Equipment_Icon_Image[i].gameObject.SetActive(true);
            }

        }
    }

    public void Treasure_Chest_Roulette()
    {
        float Treasure_Reset_Pos_Y = 2300;
        float Treasure_Image_Move = 1000f;

        if (Treasure_Timer > All_Manager.instance.Audio_M.Treasure_Chest_AS.clip.length && !Treasure_Stop)
        {
            Treasure_Timer = 0;

            Treasure_Stop = true;

            float Dis = 10000;

            for (int i = 0; i < Treasure_Item_Image_01.GetComponent<RectTransform>().transform.childCount; i++)
            {
                if (Vector3.Distance(Treasure_Item_Image_01.GetComponent<RectTransform>().transform.GetChild(i).transform.position, Treasure_Reel_Image.rectTransform.position) < Dis)
                {
                    Dis = Vector3.Distance(Treasure_Item_Image_01.GetComponent<RectTransform>().transform.GetChild(i).transform.position, Treasure_Reel_Image.rectTransform.position);
                    Treasure_Item_Number = i;
                }
            }

            for (int j = 0; j < Treasure_Item_Image_02.GetComponent<RectTransform>().transform.childCount; j++)
            {
                if (Vector3.Distance(Treasure_Item_Image_02.GetComponent<RectTransform>().transform.GetChild(j).transform.position, Treasure_Reel_Image.rectTransform.position) < Dis)
                {
                    Dis = Vector3.Distance(Treasure_Item_Image_02.GetComponent<RectTransform>().transform.GetChild(j).transform.position, Treasure_Reel_Image.rectTransform.position);
                    Treasure_Item_Number = j;
                }
            }

            Treasure_Select_Item(Treasure_Item_Number);
            Treasure_Item_Mask.rectTransform.sizeDelta = new Vector2(100, 0);
            Treasure_Item_Mask.rectTransform.anchoredPosition = new Vector2(0, -50f);
            Mask_Roulette_Time = 0f;
            Mask_Roulette_Pos = 0f;
            Treasure_Gem_Image_List.SetActive(false);
        }

        else
        {
            Treasure_Timer += Time.deltaTime;
        }

        if (Treasure_Item_Image_01 != null && Treasure_Item_Image_02 != null && Treasure_Stop == false)
        {
            // 이미지를 위로 이동
            Treasure_Item_Image_01.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (Treasure_Image_Move * Time.deltaTime);
            Treasure_Item_Image_02.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (Treasure_Image_Move * Time.deltaTime);

            // 첫 번째 이미지가 기준점을 넘으면 재배치
            if (Treasure_Item_Image_01.GetComponent<RectTransform>().anchoredPosition.y >= Treasure_Reset_Pos_Y)
            {
                Reposition_Treasure_Image(Treasure_Item_Image_01.GetComponent<RectTransform>());
            }

            // 두 번째 이미지가 기준점을 넘으면 재배치
            if (Treasure_Item_Image_02.GetComponent<RectTransform>().anchoredPosition.y >= Treasure_Reset_Pos_Y)
            {
                Reposition_Treasure_Image(Treasure_Item_Image_02.GetComponent<RectTransform>());
            }
        }

        else if (Treasure_Stop == true)
        {
            Treasure_Close_Button.gameObject.SetActive(true);
        }

        Vertical_Treasure_Image();
        Treasure_Open_Button_And_Found_Text_Disabled();
    }

    public void Reposition_Treasure_Image(RectTransform Image)
    {
        // 이미지를 아래로 재배치
        float New_Y = Image.anchoredPosition.y - 2 * 1900f;
        Image.anchoredPosition = new Vector2(Image.anchoredPosition.x, New_Y);
    }

    public void Vertical_Treasure_Image()
    {
        if (Treasure_Stop == false)
        {
            Mask_Roulette_Time += 450f * Time.deltaTime;
            Mask_Roulette_Pos += 185f * Time.deltaTime;

            if (Mask_Roulette_Time < 590)
            {
                Treasure_Item_Mask.rectTransform.sizeDelta = new Vector2(100, Mask_Roulette_Time);
            }

            if (Mask_Roulette_Pos < 245)
            {
                Treasure_Item_Mask.rectTransform.anchoredPosition = new Vector2(0, Mask_Roulette_Pos);
            }
        }
    }

    public void Treaure_Gem_Activate()
    {
        var Target_Parent = this.transform.Find("Treasure_Gem_List");

        var Gem_Go = Target_Parent.GetChild(0).gameObject;

        Gem_Go.transform.parent = null;

        Gem_Go.SetActive(true);
    }

    public void Treasure_Open()
    {
        Treasure_Chest_Image.sprite = Open_Chest;
        Treasure_Open_Chset = true;
        Treasure_Reel_Image.gameObject.SetActive(true);
        All_Manager.instance.Audio_M.Treasure_Found_Audio_Play();
        Treasure_Gem_Image_List.SetActive(true);
    }

    public void Treasure_Close()
    {
        Treasure_Chest_Image.sprite = Close_Chest;
        Treasure_Close_Button.gameObject.SetActive(false);
        Treasure_Select_Item_Image.gameObject.SetActive(false);
        Treasure_Open_Chset = false;
        Treasure_Item_Number = 0;
        Treasure_Reel_Image.gameObject.SetActive(false);
        Treasure_Item_Mask.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
        Treasure_Item_Mask.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50f);
        Mask_Roulette_Time = 0f;
        Mask_Roulette_Pos = 0f;
        Treasure_Timer = 0f;
        Treasure_Stop = false;
        Treasure_Found_Text.gameObject.SetActive(true);
        Treasure_Found_Text.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        Treasure_Found_Text.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
        Treasure_Open_Button.gameObject.SetActive(true);
        Treasure_Open_Button.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        Treasure_Open_Button.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
        Treasure_Close_Button.gameObject.SetActive(false);
        Off_Panel();
    }

    public void Treasure_Select_Item(int Item_Num)
    {
        if (Item_Num == 0)
        {
            Weapon_Select_Assign_Button(Weapon_Type.Axe); 
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Axe).Item_Image;
        }

        if (Item_Num == 1)
        {
            Weapon_Select_Assign_Button(Weapon_Type.KingBible);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.KingBible).Item_Image;
        }

        if (Item_Num == 2)
        {
            Weapon_Select_Assign_Button(Weapon_Type.Peachone);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Peachone).Item_Image;
        }

        if (Item_Num == 3)
        {
            Weapon_Select_Assign_Button(Weapon_Type.FireWand);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.FireWand).Item_Image;
        }

        if (Item_Num == 4)
        {
            Weapon_Select_Assign_Button(Weapon_Type.Cross);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Cross).Item_Image;
        }

        if (Item_Num == 5)
        {
            Weapon_Select_Assign_Button(Weapon_Type.Garlic);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Garlic).Item_Image;
        }

        if (Item_Num == 6)
        {
            Weapon_Select_Assign_Button(Weapon_Type.LightningRing);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.LightningRing).Item_Image;
        }

        if (Item_Num == 7)
        {
            Weapon_Select_Assign_Button(Weapon_Type.ShadowPinion);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.ShadowPinion).Item_Image;
        }

        if (Item_Num == 8)
        {
            Weapon_Select_Assign_Button(Weapon_Type.RuneTracer);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.RuneTracer).Item_Image;
        }

        if (Item_Num == 9)
        {
            Weapon_Select_Assign_Button(Weapon_Type.Whip);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Whip).Item_Image;
        }

        if (Item_Num == 10)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Spinach);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Spinach).Sprite_Image;
        }

        if (Item_Num == 11)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Bracer);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Bracer).Sprite_Image;
        }

        if (Item_Num == 12)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Duplicator);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Duplicator).Sprite_Image;
        }

        if (Item_Num == 13)
        {
            Equipment_Select_Assign_Button(Equipment_Type.EmptyTome);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.EmptyTome).Sprite_Image;
        }

        if (Item_Num == 14)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Wings);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Wings).Sprite_Image;
        }

        if (Item_Num == 15)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Candelabrador);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Candelabrador).Sprite_Image;
        }

        if (Item_Num == 16)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Armour);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Armour).Sprite_Image;
        }

        if (Item_Num == 17)
        {
            Equipment_Select_Assign_Button(Equipment_Type.HollowHeart);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.HollowHeart).Sprite_Image;
        }

        if (Item_Num == 18)
        {
            Equipment_Select_Assign_Button(Equipment_Type.SpellBinder);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.SpellBinder).Sprite_Image;
        }

        if (Item_Num == 19)
        {
            Equipment_Select_Assign_Button(Equipment_Type.Attractorb);
            Treasure_Select_Item_Image.gameObject.SetActive(true);
            Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Attractorb).Sprite_Image;
        }
    }

    public void Treasure_Open_Button_And_Found_Text_Disabled()
    {
        if (Treasure_Open_Chset == true)
        {
            if (Treasure_Found_Text.GetComponent<RectTransform>().transform.localScale.x > 0.01f && Treasure_Open_Button.GetComponent<RectTransform>().transform.localScale.x > 0.01f)
            {
                Treasure_Found_Text.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                Treasure_Open_Button.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                Treasure_Open_Button.GetComponent<RectTransform>().transform.Rotate(0, 0, 100f * Time.deltaTime);
            }
            else
            {
                Treasure_Found_Text.gameObject.SetActive(false);
                Treasure_Open_Button.gameObject.SetActive(false);
            }
        }

        else
        {

        }
        
    }

    public void Clear_UI()
    {
        Update_Player_HPBar_Image(1);
        Update_Player_Level_Count_Text(0);
        Update_Kill_Count_Text(0);
    }


    public void OnGUI()
    {
        WP_Options = System.Enum.GetNames(typeof(Weapon_Type));
        EQ_Options = System.Enum.GetNames(typeof(Equipment_Type));
        PS_Options = System.Enum.GetNames(typeof(Panel_Style));

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(PanelStyle_On ? "접기" : "펼치기"))
        {
            PanelStyle_On = !PanelStyle_On;
        }
        if (PanelStyle_On)
        {
            // 패널 스타일 선택
            GUILayout.Label("Select Panel Style:");
            int Sel_Panel = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_PS, PS_Options, 1);
            All_Manager.instance.TEST_PS = (Panel_Style)Sel_Panel;

            if (GUILayout.Button("Activate Panel"))
            {
                Panel_Check();
            }

            if (GUILayout.Button("Deactivate Panel"))
            {
                Panel_Test_Off();
            }
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(Weapon_On ? "접기" : "펼치기"))
        {
            Weapon_On = !Weapon_On;
        }
        if (Weapon_On)
        {
            // 무기 선택
            GUILayout.Label("Select Weapon:");
            int selectedWeaponIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_WP, WP_Options, 1);
            All_Manager.instance.TEST_WP = (Weapon_Type)selectedWeaponIndex;

            if (GUILayout.Button("Test Weapon"))
            {
                Weapon_Test();
            }

            if (GUILayout.Button("Max Level Weapon Test"))
            {
                Max_Weapon_Test();
            }
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(Equipment_On ? "접기" : "펼치기"))
        {
            Equipment_On = !Equipment_On;
        }
        if (Equipment_On)
        {
            // 장신구 선택
            GUILayout.Label("Select Equipment:");
            int selectedEquipmentIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_EQ, EQ_Options, 1);
            All_Manager.instance.TEST_EQ = (Equipment_Type)selectedEquipmentIndex;

            if (GUILayout.Button("Test Equipment"))
            {
                Equipment_Test();
            }

            if (GUILayout.Button("Max Level Equipment Test"))
            {
                Max_Equipment_Test();
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    // 패널 테스트
    [ContextMenu("Panel Check")]
    public void Panel_Check()
    {
        On_Panel(All_Manager.instance.TEST_PS);
    }

    // 패널 끄기
    [ContextMenu("Panel Off")]
    public void Panel_Test_Off()
    {
        Off_Panel();
    }

    // 무기 강화 테스트
    [ContextMenu("Weapoon Test")]
    public void Weapon_Test()
    {
        Weapon_Select_Assign_Button(All_Manager.instance.TEST_WP);
    }

    // 장신구 강화 테스트
    [ContextMenu("Equipment Test")]
    public void Equipment_Test()
    {
        Equipment_Select_Assign_Button(All_Manager.instance.TEST_EQ);
    }

    // 무기 만렙 강화 테스트
    [ContextMenu("Max Level Weapoon Test")]
    public void Max_Weapon_Test()
    {
        var Temp_WTO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(All_Manager.instance.TEST_WP);
        var Temp_WLAT = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO);

        for (int i = Temp_WLAT.Level; i < 8; i++)
        {
            Weapon_Select_Assign_Button(All_Manager.instance.TEST_WP);
        }
    }

    // 장신구 만렙 테스트
    [ContextMenu("Max Level Equipment Test")]
    public void Max_Equipment_Test()
    {
        var Temp_EQL = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(All_Manager.instance.TEST_EQ);

        for (int i = Temp_EQL; i < 5; i++)
        {
            Equipment_Select_Assign_Button(All_Manager.instance.TEST_EQ);
        }
    }
}

#region
//public void Update_Peachone_Image()
//{
//    if (All_Manager.instance.GameMode_M.Current_Player != null)
//    {
//        var P_Peachone_Stat = All_Manager.instance.GameMode_M.Current_Player;
//        var W_Peachone_Stat = All_Manager.instance.Resource_M.STO_Peachone.Info_s[P_Peachone_Stat.Peachone_LAT.Level];


//        if (All_Manager.instance.GameMode_M.Current_Player.Peachone_LAT.Level == 0)
//        {
//            Peachone_Image.gameObject.SetActive(false);
//        }

//        else
//        {
//            Peachone_Image.gameObject.SetActive(true);

//            // Peachone_Image.rectTransform
//            //돌리고 나서

//            W_Peachone_Stat.Weapon_Time += Time.deltaTime * W_Peachone_Stat.Weapon_Speed;
//            if (W_Peachone_Stat.Weapon_Time < 360)
//            {
//                float x = W_Peachone_Stat.Weapon_Range * Mathf.Cos(W_Peachone_Stat.Weapon_Time);
//                float y = W_Peachone_Stat.Weapon_Range * Mathf.Sin(W_Peachone_Stat.Weapon_Time);
//                Peachone_Image.rectTransform.position = P_Peachone_Stat.transform.position + new Vector3(x, y, 0);
//                // 오류 이유 Mine이 생성되기 전에 불러와서
//                // 해결책 내가 아는건 코루틴 
//            }
//            else
//            {
//                W_Peachone_Stat.Weapon_Time = 0;
//            }

//            // Peachone_Image.rectTransform.position += new Vector3(Random.Range(0, 1), Random.Range(0, 1));
//        }
//    }
//}


//public void Update_Peachone_Range_Image()
//{
//    if (All_Manager.instance.GameMode_M.Current_Player != null)
//    {
//        var P_Peachone_Stat = All_Manager.instance.GameMode_M.Current_Player;
//        var W_Peachone_Stat = All_Manager.instance.Resource_M.STO_Peachone.Info_s[P_Peachone_Stat.Peachone_LAT.Level];

//        if (All_Manager.instance.GameMode_M.Current_Player.Peachone_LAT.Level == 0)
//        {
//            Peachone_Range_Image.gameObject.SetActive(false);
//        }

//        else
//        {
//            Peachone_Range_Image.gameObject.SetActive(true);

//            W_Peachone_Stat.Weapon_Time += Time.deltaTime * W_Peachone_Stat.Weapon_Speed;
//            if (W_Peachone_Stat.Weapon_Time < 360)
//            {
//                float x = W_Peachone_Stat.Weapon_Range * Mathf.Cos(W_Peachone_Stat.Weapon_Time);
//                float y = W_Peachone_Stat.Weapon_Range * Mathf.Sin(W_Peachone_Stat.Weapon_Time);
//                Peachone_Range_Image.rectTransform.position = P_Peachone_Stat.transform.position + new Vector3(x, y, 0);
//                // 오류 이유 Mine이 생성되기 전에 불러와서
//                // 해결책 내가 아는건 코루틴 
//            }
//            else
//            {
//                W_Peachone_Stat.Weapon_Time = 0;
//            }
//        }
//    }
//}

//using UnityEngine;
//using UnityEngine.UI;

//public class SelectWeaponPanel : MonoBehaviour
//{
//    public Button[] weaponButtons;  // 무기 선택을 위한 버튼 배열
//    public Image weaponIcon;        // 무기 아이콘을 표시할 이미지 UI
//    public Text weaponNameText;     // 무기 이름을 표시할 텍스트 UI
//    public Text weaponDescriptionText;  // 무기 설명을 표시할 텍스트 UI

//    public WeaponData[] weapons;    // 미리 설정한 무기 데이터 배열

//    private void Start()
//    {
//        // 각 버튼에 클릭 이벤트를 추가
//        for (int i = 0; i < weaponButtons.Length; i++)
//        {
//            int index = i;  // 클로저 변수로 사용하기 위해 인덱스 저장
//            weaponButtons[i].onClick.AddListener(() => SelectWeapon(index));
//        }

//        // 초기 무기 정보 표시
//        SelectWeapon(0);  // 예를 들어, 첫 번째 무기를 초기 선택으로 설정
//    }

//    void SelectWeapon(int index)
//    {
//        // 선택한 무기의 정보를 UI에 표시
//        weaponIcon.sprite = weapons[index].icon;
//        weaponNameText.text = weapons[index].name;
//        weaponDescriptionText.text = weapons[index].description;
//    }
//}
#endregion