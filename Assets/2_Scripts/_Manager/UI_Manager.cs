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
using UnityEngine.UIElements;
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
    public UnityEngine.UI.Image Gem_Count_Image;
    public UnityEngine.UI.Image Player_HPbar_Image;

    [Header("인게임 싱크 UI")]
    public UnityEngine.UI.Image Peachone_Range_Image;
    public UnityEngine.UI.Image Peachone_Image;
    public UnityEngine.UI.Image Force_Image;
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
    public UnityEngine.UI.Button[] Start_Weapon_Button;        // 시작 버튼 누르고 나오는 무기 선택 버튼

    public UnityEngine.UI.Image[] Start_Weapon_Icon_Image;     // 시작 무기 아이콘
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
    public UnityEngine.UI.Button[] Select_Item_Button;     // 아이템 선택 버튼

    public UnityEngine.UI.Image[] Select_Item_Icon_Image;  // 선택할 아이템의 이미지
    public Text[] Select_Item_Name_Text;    // 선택할 아이템의 이름
    public Text[] Select_Item_Example_Text; // 선택할 아이템의 설명

    [Header("플레이어 정보창")]
    // 인게임 플레이 화면의 현재 무기와 장신구 아이콘
    public UnityEngine.UI.Image[] Basic_Current_Weapon_Icon_Image;
    public UnityEngine.UI.Image[] Basic_Current_Equipment_Icon_Image;

    // 인게임 UI로 띄워주는 무기 레벨
    public Text[] Weapon_Level_Text;
    public Text[] Equipment_Level_Text;

    // 플레이어 정보창 현재 무기와 장신구 아이콘
    public UnityEngine.UI.Image[] Player_Info_s_Current_Weapon_Icon_Image;
    public UnityEngine.UI.Image[] Player_Info_s_Current_Equipment_Icon_Image;

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
    public UnityEngine.UI.Image Treasure_Reel_Image;
    public UnityEngine.UI.Image Treasure_Item_Mask;
    public UnityEngine.UI.Image Treasure_Select_Item_Image;
    public Text Treasure_Found_Text;
    public Text Treasure_Got_Gem;

    public GameObject Treasure_Gem_Parent;

    public UnityEngine.UI.Image Treasure_Chest_Image;

    public Sprite Close_Chest;
    public Sprite Open_Chest;

    public UnityEngine.UI.Button Treasure_Open_Button;
    public UnityEngine.UI.Button Treasure_Close_Button;

    //public float Treasure_Start_Pos_Y = 0;
    //public float Treasure_Reset_Pos_Y = 2300;

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

    // 패널 상태 변수들
    public bool Help_On = false;
    public bool Configuration_On = false;
    public bool WindowOptions_On = false;
    public bool Widgets_On = false;
    public bool LayoutScrolling_On = false;
    public bool PopupsModal_On = false;
    public bool TablesColumns_On = false;
    public bool InputsFocus_On = false;

    // 디버그 정보 변수들
    public bool WantCaptureMouse = true;
    public bool WantCaptureKeyboard = false;
    public bool NavActive = false;

    [Header("UI_System")]
    public Player_Level_Up Player_Level_Up;
    public Treasure_Box Treasure_Box;
    public UI_OnGUI UI_OnGUI;
    public UI_Peachone UI_Peachone;
    public UI_Weapon_Force UI_Force;

    private void Update()
    {
        if (All_Manager.instance.Pool_M.Ready == false) return;

        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            UI_Peachone.Update_Peachone_Range_Image();
            UI_Peachone.Update_Peachone_Image();
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

        if (Treasure_Box.Treasure_Open_Chset == true)
        {
            Treasure_Box.Treasure_Chest_Roulette();
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



    // 플레이어를 따라오는 체력
    public void Update_MaxHPBar_Position_Image()
    {
        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Player_Position = All_Manager.instance.GameMode_M.Current_Player;

            Player_HPbar_Image.rectTransform.position = Player_Position.transform.position + new Vector3(0, -0.6f, 0);
        }
    }

    // 로딩 창
    public void Loding_Text()
    {
        if (All_Manager.instance.Pool_M.Ready == true)
        {
            Start_Loding_Text.gameObject.SetActive(false);
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
            Player_Level_Up.Renew_Item_Panel();

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

    public void Update_Player_Information_Panel()
    {
        Renew_Player_Info_Stat();
        Player_Level_Up.Renew_Current_Weapon_Icon();
        Player_Level_Up.Renew_Current_Equipment_Icon();
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


    public void Clear_UI()
    {
        Update_Player_HPBar_Image(1);
        Update_Player_Level_Count_Text(0);
        Update_Kill_Count_Text(0);
    }

    public void Start_Select()
    {
        if (All_Manager.instance.Pool_M.Ready == true)
        {
            Game_Title_Panel.SetActive(false);

            Player_Level_Up.Renew_Start_Weapon_Panel();

            Game_Start_Panel.SetActive(true);
        }

        else
        {
            return;
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