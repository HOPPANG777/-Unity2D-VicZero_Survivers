using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OnGUI : MonoBehaviour
{
    private bool showGUI = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            showGUI = !showGUI;
        }
    }

    public void OnGUI()
    {
        // 무기, 장비, 패널 스타일에 대한 옵션을 Enum에서 가져옵니다.
        All_Manager.instance.UI_M.WP_Options = System.Enum.GetNames(typeof(Weapon_Type));
        All_Manager.instance.UI_M.EQ_Options = System.Enum.GetNames(typeof(Equipment_Type));
        All_Manager.instance.UI_M.PS_Options = System.Enum.GetNames(typeof(Panel_Style));

        if (showGUI != true) return;
        GUILayout.BeginHorizontal();

        // 패널 스타일 섹션
        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(All_Manager.instance.UI_M.PanelStyle_On ? "접기" : "펼치기"))
        {
            All_Manager.instance.UI_M.PanelStyle_On = !All_Manager.instance.UI_M.PanelStyle_On;
        }
        if (All_Manager.instance.UI_M.PanelStyle_On)
        {
            GUILayout.Label("패널 스타일 선택:");
            int selectedPanelIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_PS, All_Manager.instance.UI_M.PS_Options, 1);
            All_Manager.instance.TEST_PS = (Panel_Style)selectedPanelIndex;

            if (GUILayout.Button("패널 활성화"))
            {
                Panel_Check();
            }

            if (GUILayout.Button("패널 비활성화"))
            {
                Panel_Test_Off();
            }
        }
        GUILayout.EndVertical();

        // 무기 섹션
        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(All_Manager.instance.UI_M.Weapon_On ? "접기" : "펼치기"))
        {
            All_Manager.instance.UI_M.Weapon_On = !All_Manager.instance.UI_M.Weapon_On;
        }
        if (All_Manager.instance.UI_M.Weapon_On)
        {
            GUILayout.Label("무기 선택:");
            int selectedWeaponIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_WP, All_Manager.instance.UI_M.WP_Options, 1);
            All_Manager.instance.TEST_WP = (Weapon_Type)selectedWeaponIndex;

            if (GUILayout.Button("무기 테스트"))
            {
                Weapon_Test();
            }

            if (GUILayout.Button("최대 레벨 무기 테스트"))
            {
                Max_Weapon_Test();
            }
        }
        GUILayout.EndVertical();

        // 장비 섹션
        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(All_Manager.instance.UI_M.Equipment_On ? "접기" : "펼치기"))
        {
            All_Manager.instance.UI_M.Equipment_On = !All_Manager.instance.UI_M.Equipment_On;
        }
        if (All_Manager.instance.UI_M.Equipment_On)
        {
            GUILayout.Label("장비 선택:");
            int selectedEquipmentIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_EQ, All_Manager.instance.UI_M.EQ_Options, 1);
            All_Manager.instance.TEST_EQ = (Equipment_Type)selectedEquipmentIndex;

            if (GUILayout.Button("장비 테스트"))
            {
                Equipment_Test();
            }

            if (GUILayout.Button("최대 레벨 장비 테스트"))
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
        All_Manager.instance.UI_M.On_Panel(All_Manager.instance.TEST_PS);
    }

    // 패널 끄기
    [ContextMenu("Panel Off")]
    public void Panel_Test_Off()
    {
        All_Manager.instance.UI_M.Off_Panel();
    }

    // 무기 강화 테스트
    [ContextMenu("Weapoon Test")]
    public void Weapon_Test()
    {
        All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(All_Manager.instance.TEST_WP);
    }

    // 장신구 강화 테스트
    [ContextMenu("Equipment Test")]
    public void Equipment_Test()
    {
        All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(All_Manager.instance.TEST_EQ);
    }

    // 무기 만렙 강화 테스트
    [ContextMenu("Max Level Weapoon Test")]
    public void Max_Weapon_Test()
    {
        var Temp_WTO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(All_Manager.instance.TEST_WP);
        var Temp_WLAT = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO);

        for (int i = Temp_WLAT.Level; i < 8; i++)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(All_Manager.instance.TEST_WP);
        }
    }

    // 장신구 만렙 테스트
    [ContextMenu("Max Level Equipment Test")]
    public void Max_Equipment_Test()
    {
        var Temp_EQL = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(All_Manager.instance.TEST_EQ);

        for (int i = Temp_EQL; i < 5; i++)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(All_Manager.instance.TEST_EQ);
        }
    }

}
