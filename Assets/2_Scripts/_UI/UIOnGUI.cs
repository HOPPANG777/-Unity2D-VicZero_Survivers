using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnGUI : MonoBehaviour
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
        AllManager.instance.UIM.WPOptions = System.Enum.GetNames(typeof(WeaponType));
        AllManager.instance.UIM.EQOptions = System.Enum.GetNames(typeof(AccessoryType));
        AllManager.instance.UIM.PSOptions = System.Enum.GetNames(typeof(PanelStyle));

        if (showGUI != true) return;
        GUILayout.BeginHorizontal();

        // 패널 스타일 섹션
        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(AllManager.instance.UIM.PanelStyleOn ? "접기" : "펼치기"))
        {
            AllManager.instance.UIM.PanelStyleOn = !AllManager.instance.UIM.PanelStyleOn;
        }
        if (AllManager.instance.UIM.PanelStyleOn)
        {
            GUILayout.Label("패널 스타일 선택:");
            int selectedPanelIndex = GUILayout.SelectionGrid((int)AllManager.instance.TESTPS, AllManager.instance.UIM.PSOptions, 1);
            AllManager.instance.TESTPS = (PanelStyle)selectedPanelIndex;

            if (GUILayout.Button("패널 활성화"))
            {
                PanelCheck();
            }

            if (GUILayout.Button("패널 비활성화"))
            {
                PanelTestOff();
            }
        }
        GUILayout.EndVertical();

        // 무기 섹션
        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(AllManager.instance.UIM.WeaponOn ? "접기" : "펼치기"))
        {
            AllManager.instance.UIM.WeaponOn = !AllManager.instance.UIM.WeaponOn;
        }
        if (AllManager.instance.UIM.WeaponOn)
        {
            GUILayout.Label("무기 선택:");
            int selectedWeaponIndex = GUILayout.SelectionGrid((int)AllManager.instance.TESTWP, AllManager.instance.UIM.WPOptions, 1);
            AllManager.instance.TESTWP = (WeaponType)selectedWeaponIndex;

            if (GUILayout.Button("무기 테스트"))
            {
                WeaponTest();
            }

            if (GUILayout.Button("최대 레벨 무기 테스트"))
            {
                MaxWeaponTest();
            }
        }
        GUILayout.EndVertical();

        // 장비 섹션
        GUILayout.BeginVertical("Box");
        if (GUILayout.Button(AllManager.instance.UIM.AccessoryOn ? "접기" : "펼치기"))
        {
            AllManager.instance.UIM.AccessoryOn = !AllManager.instance.UIM.AccessoryOn;
        }
        if (AllManager.instance.UIM.AccessoryOn)
        {
            GUILayout.Label("장비 선택:");
            int selectedAccessoryIndex = GUILayout.SelectionGrid((int)AllManager.instance.TESTEQ, AllManager.instance.UIM.EQOptions, 1);
            AllManager.instance.TESTEQ = (AccessoryType)selectedAccessoryIndex;

            if (GUILayout.Button("장비 테스트"))
            {
                AccessoryTest();
            }

            if (GUILayout.Button("최대 레벨 장비 테스트"))
            {
                MaxAccessoryTest();
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }



    // 패널 테스트
    [ContextMenu("Panel Check")]
    public void PanelCheck()
    {
        AllManager.instance.UIM.OnPanel(AllManager.instance.TESTPS);
    }

    // 패널 끄기
    [ContextMenu("Panel Off")]
    public void PanelTestOff()
    {
        AllManager.instance.UIM.OffPanel();
    }

    // 무기 강화 테스트
    [ContextMenu("Weapoon Test")]
    public void WeaponTest()
    {
        AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(AllManager.instance.TESTWP);
    }

    // 장신구 강화 테스트
    [ContextMenu("Accessory Test")]
    public void AccessoryTest()
    {
        AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AllManager.instance.TESTEQ);
    }

    // 무기 만렙 강화 테스트
    [ContextMenu("Max Level Weapoon Test")]
    public void MaxWeaponTest()
    {
        var TempweaponTypeO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(AllManager.instance.TESTWP);
        var TempWLAT = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeO);

        for (int i = TempWLAT.Level; i < 8; i++)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(AllManager.instance.TESTWP);
        }
    }

    // 장신구 만렙 테스트
    [ContextMenu("Max Level Accessory Test")]
    public void MaxAccessoryTest()
    {
        var TempEQL = AllManager.instance.GameModeM.CurrentPlayer.ConvertAccessoryLevelPlayer(AllManager.instance.TESTEQ);

        for (int i = TempEQL; i < 5; i++)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AllManager.instance.TESTEQ);
        }
    }

}
