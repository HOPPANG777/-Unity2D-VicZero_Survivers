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

public enum PanelStyle
{
    Pause,
    Level,
    GameOver,
    GameStart,
    TreasureFound,
    GameClear
}

public class UIManager : MonoBehaviour
{
    [Header("기본 UI")]
    public Text KillCountText;
    public Text TimeCountText;
    public Text PlayerLevelCountText;
    public UnityEngine.UI.Image GemCountImage;
    public UnityEngine.UI.Image PlayerHPbarImage;

    [Header("인게임 싱크 UI")]
    public UnityEngine.UI.Image PeachoneRangeImage;
    public UnityEngine.UI.Image PeachoneImage;
    public UnityEngine.UI.Image ForceImage;
    public Text WaveText;
    public Text GameStateText;
    public Text PanelStyleText;
    public Text GamePoolReadyText;
    public Text MissionText;

    [Header("UI캔버스")]
    public GameObject BasicPanel;
    public GameObject IngameSinkPanel;
    public GameObject PlayerInformationPanel;
    public GameObject PausePanel;
    public GameObject SelectItemPanel;
    public GameObject GameOverPanel;
    public GameObject GameStartPanel;
    public GameObject GameTitlePanel;
    public GameObject GameClearPanel;
    public GameObject GameEndPanel;
    public GameObject TreasureChestPanel;
    public GameObject IngameDebugLog;

    [Header("타이틀 화면")]
    public UnityEngine.UI.Button[] StartWeaponButton;        // 시작 버튼 누르고 나오는 무기 선택 버튼

    public UnityEngine.UI.Image[] StartWeaponIconImage;     // 시작 무기 아이콘
    public Text[] StartWeaponNameText;       // 이름
    public Text[] StartWeaponExampleText;    // 설명

    public Text StartLodingText;  // 시작 할때 로딩 중임을 알리는 텍스트

    [Header("시작 무기 이넘")]
    public int StartFirstWeaponNumber;   // 시작 무기 버튼 1에서 나올 무기
    public int StartSecondWeaponNumber;  // 시작 무기 버튼 2에서 나올 무기

    [Header("아이템 아이콘")]
    public List<Sprite> WeaponIconList = new List<Sprite>();      // 획득한 무기의 아이콘 저장
    public List<Sprite> AccessoryIconList = new List<Sprite>();   // 획득한 장신구의 아이콘 저장

    [Header("아이템 이넘")]
    public int SelectFirstItemNumber;    // 첫 번째 버튼에서 나올 아이템 
    public int SelectSecondItemNumber;   // 두 번째 버튼에서 나올 아이템
    public int SelectThirdItemNumber;    // 세 번째 버튼에서 나올 아이템


    [Header("아이템 선택창")]
    public UnityEngine.UI.Button[] SelectItemButton;     // 아이템 선택 버튼

    public UnityEngine.UI.Image[] SelectItemIconImage;  // 선택할 아이템의 이미지
    public Text[] SelectItemNameText;    // 선택할 아이템의 이름
    public Text[] SelectItemExampleText; // 선택할 아이템의 설명

    [Header("플레이어 정보창")]
    // 인게임 플레이 화면의 현재 무기와 장신구 아이콘
    public UnityEngine.UI.Image[] BasicCurrentWeaponIconImage;
    public UnityEngine.UI.Image[] BasicCurrentAccessoryIconImage;

    // 인게임 UI로 띄워주는 무기 레벨
    public Text[] WeaponLevelText;
    public Text[] AccessoryLevelText;

    // 플레이어 정보창 현재 무기와 장신구 아이콘
    public UnityEngine.UI.Image[] PlayerInfosCurrentWeaponIconImage;
    public UnityEngine.UI.Image[] PlayerInfosCurrentAccessoryIconImage;

    // 플레이어 정보창 스탯
    public Text[] PlayerInfosPaseOneText;
    public Text[] PlayerInfosPaseTwoText;

    [Header("인게임 싱크 UI용")]
    public float WPeachoneDeg = 0;    // Peachone의 각도(플레이어 주변을 돈다).
    public float WPeachoneRangeDeg = 0;  // Peachone의 공격 범위(플레이어 주변을 돈다).

    public bool isLevelUp = false;  // 레벨업 상태인지 확인용

    public float Speed = 1.0f;  // 레벨업시 경험치바 점멸 속도.
    public float CurrentBlue = 0.0f;   // 현재 RGBA중 B색깔
    public bool Blinking = false;   // 점멸 할꺼냐.

    [Header("보물상자 UI용")]
    public GameObject TreasureItemImage01;
    public GameObject TreasureItemImage02;
    public GameObject TreasureGemImageList;
    public UnityEngine.UI.Image TreasureReelImage;
    public UnityEngine.UI.Image TreasureItemMask;
    public UnityEngine.UI.Image TreasureSelectItemImage;
    public Text TreasureFoundText;
    public Text TreasureGotGem;

    public GameObject TreasureGemParent;

    public UnityEngine.UI.Image TreasureChestImage;

    public Sprite CloseChest;
    public Sprite OpenChest;

    public UnityEngine.UI.Button TreasureOpenButton;
    public UnityEngine.UI.Button TreasureCloseButton;

    [Header("OnGUI용")]
    public string[] WPOptions;
    public string[] EQOptions;
    public string[] PSOptions;

    public bool WeaponOn = false;
    public bool AccessoryOn = false;
    public bool PanelStyleOn = false;

    // 패널 상태 변수들
    public bool HelpOn = false;
    public bool ConfigurationOn = false;
    public bool WindowOptionsOn = false;
    public bool WidgetsOn = false;
    public bool LayoutScrollingOn = false;
    public bool PopupsModalOn = false;
    public bool TablesColumnsOn = false;
    public bool InputsFocusOn = false;

    // 디버그 정보 변수들
    public bool WantCaptureMouse = true;
    public bool WantCaptureKeyboard = false;
    public bool NavActive = false;

    [Header("UISystem")]
    public PlayerLevelUpController PlayerLevelUp;
    public TreasureBoxController TreasureBox;
    public UIOnGUI UIOnGUI;
    public UIPeachone UIPeachone;
    public UIWeaponForce UIForce;

    void Start()
    {
        AllManager.instance.GameModeM.onGameOver += () =>
        {
            Debug.Log("GameOver UI OnPanel is called");
            GameOverPanel.SetActive(true);
            GameEndPanel.SetActive(true);
            GameStateText.text = "GameOver";
        };

        AllManager.instance.GameModeM.onGameClear += () =>
        {
            GameClearPanel.SetActive(true);
            GameEndPanel.SetActive(true);
            AllManager.instance.UIM.UpdateStateCheckText("GameClear");
        };
    }


    private void Update()
    {
        if (AllManager.instance.PoolM.Ready == false) return;

        if (AllManager.instance.GameModeM.GameStop == false)
        {
            UIPeachone.UpdatePeachoneRangeImage();
            UIPeachone.UpdatePeachoneImage();
        }

        if (isLevelUp == true)
        {
            GemBarBlinking();
        }

        else
        {
            GemCountImage.color = new Color(0, 1, 1, 1);
        }

        LodingText();

        UpdateMaxHPBarPositionImage();

        if (TreasureBox.TreasureOpenChset == true)
        {
            TreasureBox.TreasureChestRoulette();
        }
    }


    public void GemBarBlinking()
    {
        Color color = GemCountImage.color;

        if (Blinking)
        {
            float MaxB = 1.0f;

            CurrentBlue += Speed * Time.deltaTime;
            if (CurrentBlue >= MaxB)
            {
                CurrentBlue = MaxB;
                Blinking = false;
            }
        }
        else
        {
            float MinB = 0.0f;

            CurrentBlue -= Speed * Time.deltaTime;
            if (CurrentBlue <= MinB)
            {
                CurrentBlue = MinB;
                Blinking = true;
            }
        }

        color.b = CurrentBlue;
        GemCountImage.color = color;
    }


    public void UpdateWaveCheckText(string pText)
    {
        WaveText.text = pText;
    }

    public void UpdateStateCheckText(string pText)
    {
        GameStateText.text = pText;
    }

    public void UpdatePanelCheckText(string pText)
    {
        PanelStyleText.text = pText;
    }

    public void UpdatePoolReadyCheckText(string pText)
    {
        GamePoolReadyText.text = pText;
    }

    public void UpdateMissionCheckText(string pText)
    {
        MissionText.text = pText;
    }

    // 킬 카운트 갱신
    public void UpdateKillCountText(int pCount)
    {
        KillCountText.text = "\t : " + pCount;
    }

    // 플레이어 현재 체력 갱신
    public void UpdatePlayerHPBarImage(float pFillAmountValue)
    {
        PlayerHPbarImage.fillAmount = pFillAmountValue;
    }

    // 플레이어 레벨 카운트 갱신
    public void UpdatePlayerLevelCountText(int pLevelCount)
    {
        PlayerLevelCountText.text = "Lv . " + pLevelCount;
    }

    // 무기 레벨 카운트 갱신
    public void UpdateWeaponLevelCountText(int Index, int pWeaponLevelCount)
    {
        WeaponLevelText[Index].text = "Lv. " + pWeaponLevelCount;
    }

    // 장신구 레벨 카운트 갱신
    public void UpdateAccessoryLevelCountText(int Index, int pAccessoryLevelCount)
    {
        AccessoryLevelText[Index].text = "Lv. " + pAccessoryLevelCount;
    }

    // 게임 시간 경과 갱신
    public void UpdateTimeCountText(float pTimeCount)
    {
        TimeCountText.text = ((int)pTimeCount / 60 % 60).ToString("D2") + " : " + ((int)pTimeCount % 60).ToString("D2");
    }

    // 경험치 바 갱신
    public void UpdateGemCountImage(float pFillAmountGemCount)
    {
        GemCountImage.fillAmount = pFillAmountGemCount;
    }

    // 클릭 테스트
    public void ClickTest()
    {
        Debug.Log("fd");
    }



    // 플레이어를 따라오는 체력
    public void UpdateMaxHPBarPositionImage()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer != null)
        {
            var PlayerPosition = AllManager.instance.GameModeM.CurrentPlayer;

            PlayerHPbarImage.rectTransform.position = PlayerPosition.transform.position + new Vector3(0, -0.6f, 0);
        }
    }

    // 로딩 창
    public void LodingText()
    {
        if (AllManager.instance.PoolM.Ready == true)
        {
            StartLodingText.gameObject.SetActive(false);
        }
    }

    // 패널 킴
    public void OnPanel(PanelStyle pPS)
    {
        AllManager.instance.GameModeM.GameStop = true;

        if (pPS == PanelStyle.Pause)
        {
            UpdatePlayerInformationPanel();

            PlayerInformationPanel.SetActive(true);
            PausePanel.SetActive(true);

        }

        else if (pPS == PanelStyle.Level)
        {
            UpdatePlayerInformationPanel();
            PlayerLevelUp.RenewItemPanel();

            AllManager.instance.AudioM.LevelUpAudioPlay();
            PlayerInformationPanel.SetActive(true);
            SelectItemPanel.SetActive(true);
        }

        else if (pPS == PanelStyle.GameOver)
        {
        }

        else if (pPS == PanelStyle.GameStart)
        {
            GameTitlePanel.SetActive(true);
        }

        else if (pPS == PanelStyle.TreasureFound)
        {
            TreasureChestPanel.SetActive(true);
        }


        AllManager.instance.AudioM.BackgroundAudioPlay(pPS);

        UpdatePanelCheckText(pPS.ToString());
    }

    // 패널 끔
    public void OffPanel()
    {
        AllManager.instance.GameModeM.GameStop = false;

        UpdatePlayerInformationPanel();

        PlayerInformationPanel.SetActive(false);
        SelectItemPanel.SetActive(false);
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        GameStartPanel.SetActive(false);
        GameTitlePanel.SetActive(false);
        GameEndPanel.SetActive(false);
        GameClearPanel.SetActive(false);
        TreasureChestPanel.SetActive(false);

        isLevelUp = false;

        //AllManager.instance.AudioM.BackgroundAS.UnPause();
        AllManager.instance.AudioM.BackgroundAS.volume = 0.3f;
    }

    public void UpdatePlayerInformationPanel()
    {
        RenewPlayerInfoStat();
        PlayerLevelUp.RenewCurrentWeaponIcon();
        PlayerLevelUp.RenewCurrentAccessoryIcon();
    }

    public void RenewPlayerInfoStat()
    {
        // 페이지 1
        PlayerInfosPaseOneText[0].text = "" + AllManager.instance.GameModeM.CurrentPlayer.OriginStat.UnitHP;
        PlayerInfosPaseOneText[1].text = "" + AllManager.instance.GameModeM.CurrentPlayer.OriginStat.UnitSpeed;
        PlayerInfosPaseOneText[2].text = "" + AllManager.instance.GameModeM.CurrentPlayer.OriginStat.UnitDefense + "%";
        PlayerInfosPaseOneText[3].text = "" + AllManager.instance.GameModeM.CurrentPlayer.PercentStat.UnitWeaponDamage + "%";
        PlayerInfosPaseOneText[4].text = "" + AllManager.instance.GameModeM.CurrentPlayer.OriginStat.UnitWeaponProjSpeed + "%";

        // 페이지 2
        PlayerInfosPaseTwoText[0].text = "" + AllManager.instance.GameModeM.CurrentPlayer.PercentStat.UnitWeaponDuration + "%";
        PlayerInfosPaseTwoText[1].text = "" + AllManager.instance.GameModeM.CurrentPlayer.PercentStat.UnitWeaponRange + "%";
        PlayerInfosPaseTwoText[2].text = "-" + AllManager.instance.GameModeM.CurrentPlayer.PercentStat.UnitWeaponCoolDown + "%";
        PlayerInfosPaseTwoText[3].text = "" + AllManager.instance.GameModeM.CurrentPlayer.PercentStat.UnitWeaponProjCount;
        PlayerInfosPaseTwoText[4].text = "" + AllManager.instance.GameModeM.CurrentPlayer.PercentStat.UnitGetGemRange;
    }


    public void ClearUI()
    {
        UpdatePlayerHPBarImage(1);
        UpdatePlayerLevelCountText(0);
        UpdateKillCountText(0);
    }

    public void StartSelect()
    {
        if (AllManager.instance.PoolM.Ready == true)
        {
            GameTitlePanel.SetActive(false);

            PlayerLevelUp.RenewStartWeaponPanel();

            GameStartPanel.SetActive(true);
        }

        else
        {
            return;
        }
    }

}

#region
//public void UpdatePeachoneImage()
//{
//    if (AllManager.instance.GameModeM.CurrentPlayer != null)
//    {
//        var PPeachoneStat = AllManager.instance.GameModeM.CurrentPlayer;
//        var WPeachoneStat = AllManager.instance.ResourceM.STOPeachone.Infos[PPeachoneStat.PeachoneLAT.Level];


//        if (AllManager.instance.GameModeM.CurrentPlayer.PeachoneLAT.Level == 0)
//        {
//            PeachoneImage.gameObject.SetActive(false);
//        }

//        else
//        {
//            PeachoneImage.gameObject.SetActive(true);

//            // PeachoneImage.rectTransform
//            //돌리고 나서

//            WPeachoneStat.WeaponTime += Time.deltaTime * WPeachoneStat.WeaponSpeed;
//            if (WPeachoneStat.WeaponTime < 360)
//            {
//                float x = WPeachoneStat.WeaponRange * Mathf.Cos(WPeachoneStat.WeaponTime);
//                float y = WPeachoneStat.WeaponRange * Mathf.Sin(WPeachoneStat.WeaponTime);
//                PeachoneImage.rectTransform.position = PPeachoneStat.transform.position + new Vector3(x, y, 0);
//                // 오류 이유 Mine이 생성되기 전에 불러와서
//                // 해결책 내가 아는건 코루틴 
//            }
//            else
//            {
//                WPeachoneStat.WeaponTime = 0;
//            }

//            // PeachoneImage.rectTransform.position += new Vector3(Random.Range(0, 1), Random.Range(0, 1));
//        }
//    }
//}


//public void UpdatePeachoneRangeImage()
//{
//    if (AllManager.instance.GameModeM.CurrentPlayer != null)
//    {
//        var PPeachoneStat = AllManager.instance.GameModeM.CurrentPlayer;
//        var WPeachoneStat = AllManager.instance.ResourceM.STOPeachone.Infos[PPeachoneStat.PeachoneLAT.Level];

//        if (AllManager.instance.GameModeM.CurrentPlayer.PeachoneLAT.Level == 0)
//        {
//            PeachoneRangeImage.gameObject.SetActive(false);
//        }

//        else
//        {
//            PeachoneRangeImage.gameObject.SetActive(true);

//            WPeachoneStat.WeaponTime += Time.deltaTime * WPeachoneStat.WeaponSpeed;
//            if (WPeachoneStat.WeaponTime < 360)
//            {
//                float x = WPeachoneStat.WeaponRange * Mathf.Cos(WPeachoneStat.WeaponTime);
//                float y = WPeachoneStat.WeaponRange * Mathf.Sin(WPeachoneStat.WeaponTime);
//                PeachoneRangeImage.rectTransform.position = PPeachoneStat.transform.position + new Vector3(x, y, 0);
//                // 오류 이유 Mine이 생성되기 전에 불러와서
//                // 해결책 내가 아는건 코루틴 
//            }
//            else
//            {
//                WPeachoneStat.WeaponTime = 0;
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