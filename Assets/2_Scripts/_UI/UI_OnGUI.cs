using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OnGUI : MonoBehaviour
{
    public void OnGUI()
    {
        All_Manager.instance.UI_M.WP_Options = System.Enum.GetNames(typeof(Weapon_Type));
        All_Manager.instance.UI_M.EQ_Options = System.Enum.GetNames(typeof(Equipment_Type));
        All_Manager.instance.UI_M.PS_Options = System.Enum.GetNames(typeof(Panel_Style));

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(All_Manager.instance.UI_M.PanelStyle_On ? "접기" : "펼치기"))
        {
            All_Manager.instance.UI_M.PanelStyle_On = !All_Manager.instance.UI_M.PanelStyle_On;
        }
        if (All_Manager.instance.UI_M.PanelStyle_On)
        {
            // 패널 스타일 선택
            GUILayout.Label("Select Panel Style:");
            int Sel_Panel = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_PS, All_Manager.instance.UI_M.PS_Options, 1);
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
        if (GUILayout.Button(All_Manager.instance.UI_M.Weapon_On ? "접기" : "펼치기"))
        {
            All_Manager.instance.UI_M.Weapon_On = !All_Manager.instance.UI_M.Weapon_On;
        }
        if (All_Manager.instance.UI_M.Weapon_On)
        {
            // 무기 선택
            GUILayout.Label("Select Weapon:");
            int selectedWeaponIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_WP, All_Manager.instance.UI_M.WP_Options, 1);
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
        if (GUILayout.Button(All_Manager.instance.UI_M.Equipment_On ? "접기" : "펼치기"))
        {
            All_Manager.instance.UI_M.Equipment_On = !All_Manager.instance.UI_M.Equipment_On;
        }
        if (All_Manager.instance.UI_M.Equipment_On)
        {
            // 장신구 선택
            GUILayout.Label("Select Equipment:");
            int selectedEquipmentIndex = GUILayout.SelectionGrid((int)All_Manager.instance.TEST_EQ, All_Manager.instance.UI_M.EQ_Options, 1);
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
