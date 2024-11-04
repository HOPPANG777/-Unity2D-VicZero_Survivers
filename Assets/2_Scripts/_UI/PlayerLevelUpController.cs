using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUpController : MonoBehaviour
{

    public void RenewStartWeaponPanel()
    {
        List<int> SelectedStartWeaponList = new List<int>();
        do
        {
            AllManager.instance.UIM.StartFirstWeaponNumber = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length - 1);

        } while (SelectedStartWeaponList.Contains(AllManager.instance.UIM.StartFirstWeaponNumber));

        SelectedStartWeaponList.Add(AllManager.instance.UIM.StartFirstWeaponNumber);

        do
        {
            AllManager.instance.UIM.StartSecondWeaponNumber = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length - 1);

        } while (SelectedStartWeaponList.Contains(AllManager.instance.UIM.StartSecondWeaponNumber));

        SelectedStartWeaponList.Add(AllManager.instance.UIM.StartSecondWeaponNumber);


        var TempweaponTypeFirst = (WeaponType)AllManager.instance.UIM.StartFirstWeaponNumber;

        WeaponData ConvertWeaponFirst = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(TempweaponTypeFirst);

        AllManager.instance.UIM.StartWeaponIconImage[0].sprite = ConvertWeaponFirst.ItemImage;
        AllManager.instance.UIM.StartWeaponNameText[0].text = ConvertWeaponFirst.WeaponName;
        AllManager.instance.UIM.StartWeaponExampleText[0].text = ConvertWeaponFirst.WeaponText;

        var TempweaponTypeSecond = (WeaponType)AllManager.instance.UIM.StartSecondWeaponNumber;

        WeaponSelectAssignInfo(1, TempweaponTypeSecond);

        WeaponData ConvertWeaponSecond = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(TempweaponTypeSecond);

        AllManager.instance.UIM.StartWeaponIconImage[1].sprite = ConvertWeaponSecond.ItemImage;
        AllManager.instance.UIM.StartWeaponNameText[1].text = ConvertWeaponSecond.WeaponName;
        AllManager.instance.UIM.StartWeaponExampleText[1].text = ConvertWeaponSecond.WeaponText;
    }

    public void ClickStartWeaponSelect(int pIndex)
    {

        if (pIndex == 0)
        {
            var TempweaponType = (WeaponType)AllManager.instance.UIM.StartFirstWeaponNumber;

            WeaponSelectAssignButton(TempweaponType);
        }

        else if (pIndex == 1)
        {
            var TempweaponType = (WeaponType)AllManager.instance.UIM.StartSecondWeaponNumber;

            WeaponSelectAssignButton(TempweaponType);
        }

        AllManager.instance.UIM.OffPanel();
    }
    public void RenewItemPanel()
    {
        RenewCurrentWeaponIcon();
        RenewCurrentAccessoryIcon();

        int Max = System.Enum.GetValues(typeof(WeaponType)).Length + System.Enum.GetValues(typeof(AccessoryType)).Length;
        List<int> SelectedItemList = new List<int>();
        do
        {   // 첫번째 패널에 나타날 아이템 룰렛 
            AllManager.instance.UIM.SelectFirstItemNumber = UnityEngine.Random.Range(0, Max - 1);

        } while (SelectedItemList.Contains(AllManager.instance.UIM.SelectFirstItemNumber));

        SelectedItemList.Add(AllManager.instance.UIM.SelectFirstItemNumber);


        do
        {   // 두번째 패널에 나타날 아이템 룰렛 
            AllManager.instance.UIM.SelectSecondItemNumber = UnityEngine.Random.Range(0, Max - 1);

        } while (SelectedItemList.Contains(AllManager.instance.UIM.SelectSecondItemNumber));

        SelectedItemList.Add(AllManager.instance.UIM.SelectSecondItemNumber);

        do
        {   // 세번째 패널에 나타날 아이템 룰렛 
            AllManager.instance.UIM.SelectThirdItemNumber = UnityEngine.Random.Range(0, Max - 1);

        } while (SelectedItemList.Contains(AllManager.instance.UIM.SelectThirdItemNumber));

        SelectedItemList.Add(AllManager.instance.UIM.SelectThirdItemNumber);


        //SelectFirstItemNumber
        if (AllManager.instance.UIM.SelectFirstItemNumber < System.Enum.GetValues(typeof(WeaponType)).Length)
        {
            //무기
            var TempweaponType = (WeaponType)AllManager.instance.UIM.SelectFirstItemNumber;
            WeaponSelectAssignInfo(0, TempweaponType);
        }

        else
        {
            //장비
            var TempET = (AccessoryType)(AllManager.instance.UIM.SelectFirstItemNumber - System.Enum.GetValues(typeof(WeaponType)).Length);
            AccessorySelectAssignInfo(0, TempET);
        }


        //SelectSecondItemNumber
        if (AllManager.instance.UIM.SelectSecondItemNumber < System.Enum.GetValues(typeof(WeaponType)).Length)
        {
            //무기
            var TempweaponType = (WeaponType)AllManager.instance.UIM.SelectSecondItemNumber;
            WeaponSelectAssignInfo(1, TempweaponType);
        }

        else
        {
            //장비
            var TempET = (AccessoryType)(AllManager.instance.UIM.SelectSecondItemNumber - System.Enum.GetValues(typeof(WeaponType)).Length);
            AccessorySelectAssignInfo(1, TempET);
        }

        //SelectThirdItemNumber
        if (AllManager.instance.UIM.SelectThirdItemNumber < System.Enum.GetValues(typeof(WeaponType)).Length)
        {
            //무기
            var TempweaponType = (WeaponType)AllManager.instance.UIM.SelectThirdItemNumber;
            WeaponSelectAssignInfo(2, TempweaponType);
        }

        else
        {
            //장비
            var TempET = (AccessoryType)(AllManager.instance.UIM.SelectThirdItemNumber - System.Enum.GetValues(typeof(WeaponType)).Length);
            AccessorySelectAssignInfo(2, TempET);
        }
    }

    public void ClickSelectItemButton(int pIndex)
    {
        //SelectFirstItemNumber 
        if (pIndex == 0)
        {
            if (AllManager.instance.UIM.SelectFirstItemNumber < System.Enum.GetValues(typeof(WeaponType)).Length)
            {
                var TempweaponType = (WeaponType)AllManager.instance.UIM.SelectFirstItemNumber;

                WeaponSelectAssignButton(TempweaponType);
            }

            else
            {
                var TempET = (AccessoryType)(AllManager.instance.UIM.SelectFirstItemNumber - System.Enum.GetValues(typeof(WeaponType)).Length);

                AccessorySelectAssignButton(TempET);
            }
        }

        //SelectSecondItemNumber
        else if (pIndex == 1)
        {
            if (AllManager.instance.UIM.SelectSecondItemNumber < System.Enum.GetValues(typeof(WeaponType)).Length)
            {
                var TempweaponType = (WeaponType)AllManager.instance.UIM.SelectSecondItemNumber;

                WeaponSelectAssignButton(TempweaponType);
            }

            else
            {
                var TempET = (AccessoryType)(AllManager.instance.UIM.SelectSecondItemNumber - System.Enum.GetValues(typeof(WeaponType)).Length);

                AccessorySelectAssignButton(TempET);
            }
        }

        //SelectThirdItemNumber
        else if (pIndex == 2)
        {
            if (AllManager.instance.UIM.SelectThirdItemNumber < System.Enum.GetValues(typeof(WeaponType)).Length)
            {
                var TempweaponType = (WeaponType)AllManager.instance.UIM.SelectThirdItemNumber;

                WeaponSelectAssignButton(TempweaponType);
            }

            else
            {
                var TempET = (AccessoryType)(AllManager.instance.UIM.SelectThirdItemNumber - System.Enum.GetValues(typeof(WeaponType)).Length);

                AccessorySelectAssignButton(TempET);
            }
        }

        AllManager.instance.UIM.OffPanel();
        AllManager.instance.UIM.UpdateGemCountImage(0);
    }

    public void WeaponSelectAssignButton(WeaponType pweaponType)
    {
        WeaponData TempweaponType = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        var PlayerWeaponLevel = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponType);

        if (PlayerWeaponLevel.Level >= 8)
        {
            AllManager.instance.GameModeM.CurrentPlayer.RecoveryCurrentHP();
        }

        else
        {
            AllManager.instance.GameModeM.CurrentPlayer.UpgradeWeapon(pweaponType);

            if (AllManager.instance.GameModeM.CurrentPlayer != null)
            {
                if (TempweaponType != null)
                {
                    bool SpriteCurrect = AllManager.instance.UIM.WeaponIconList.Contains(TempweaponType.ItemImage);

                    if (!SpriteCurrect)
                    {
                        AllManager.instance.UIM.WeaponIconList.Add(TempweaponType.ItemImage);
                    }

                    else
                    {
                        //Debug.Log("이미 무기 아이콘이 존재합니다");
                    }

                    for (int i = 0; i < AllManager.instance.UIM.WeaponIconList.Count; i++)
                    {
                        if (AllManager.instance.UIM.WeaponIconList[i] == TempweaponType.ItemImage)
                        {
                            int WeaponLevelTemp = 0;

                            for (int j = 0; j < PlayerWeaponLevel.Level; j++)
                            {
                                WeaponLevelTemp++;
                            }

                            AllManager.instance.UIM.BasicCurrentWeaponIconImage[i].GetComponentInChildren<Text>().text = "Lv. " + WeaponLevelTemp;
                        }
                    }

                    RenewCurrentWeaponIcon();
                }
            }
        }


    }

    public void AccessorySelectAssignButton(AccessoryType pET)
    {
        AccessoryData TempET = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(pET);

        var PlayerAccessoryLevel = AllManager.instance.GameModeM.CurrentPlayer.ConvertAccessoryLevelPlayer(pET);

        if (PlayerAccessoryLevel >= 5)
        {
            AllManager.instance.GameModeM.CurrentPlayer.RecoveryCurrentHP();
        }

        else
        {
            AllManager.instance.GameModeM.CurrentPlayer.UpgradeAccessory(pET);

            AllManager.instance.GameModeM.CurrentPlayer.OriginStatApplicationPlayer(pET);

            if (AllManager.instance.GameModeM.CurrentPlayer != null)
            {
                if (TempET != null)
                {

                    bool SpriteCurrect = AllManager.instance.UIM.AccessoryIconList.Contains(TempET.SpriteImage);

                    if (!SpriteCurrect)
                    {
                        AllManager.instance.UIM.AccessoryIconList.Add(TempET.SpriteImage);
                    }

                    else
                    {
                        //Debug.Log("이미 장신구 아이콘이 존재합니다.");
                    }

                    for (int i = 0; i < AllManager.instance.UIM.AccessoryIconList.Count; i++)
                    {
                        if (AllManager.instance.UIM.AccessoryIconList[i] == TempET.SpriteImage)
                        {
                            int AccessoryLevelTemp = 0;

                            var TempCurrentAccessoryLevel = AllManager.instance.GameModeM.CurrentPlayer.ConvertAccessoryLevelPlayer(pET);

                            for (int j = 0; j < TempCurrentAccessoryLevel; j++)
                            {
                                AccessoryLevelTemp++;
                            }

                            AllManager.instance.UIM.BasicCurrentAccessoryIconImage[i].GetComponentInChildren<Text>().text = "Lv. " + AccessoryLevelTemp;
                        }

                    }

                    RenewCurrentAccessoryIcon();
                }

            }
        }


    }

    public void WeaponSelectAssignInfo(int Index, WeaponType pweaponType)
    {
        WeaponData TempweaponType = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        ItemData TempFood = AllManager.instance.ResourceM.ConvertItemTypeToItemDataArr(ItemType.Food);

        if (AllManager.instance.GameModeM.CurrentPlayer != null)
        {
            var PlayerWeaponLevel = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponType);

            if (PlayerWeaponLevel.Level >= 8)
            {
                if (AllManager.instance.UIM.SelectItemIconImage[Index] == null) Debug.Log(2);
                AllManager.instance.UIM.SelectItemIconImage[Index].sprite = TempFood.SpriteImage;

                if (AllManager.instance.UIM.SelectItemNameText[Index] == null) Debug.Log(3);
                AllManager.instance.UIM.SelectItemNameText[Index].text = TempFood.ItemName;

                if (AllManager.instance.UIM.SelectItemExampleText[Index] == null) Debug.Log(4);
                AllManager.instance.UIM.SelectItemExampleText[Index].text = TempFood.ItemText;
            }

            else
            {
                if (AllManager.instance.UIM.SelectItemIconImage[Index] == null) Debug.Log(2);
                AllManager.instance.UIM.SelectItemIconImage[Index].sprite = TempweaponType.ItemImage;

                if (AllManager.instance.UIM.SelectItemNameText[Index] == null) Debug.Log(3);
                AllManager.instance.UIM.SelectItemNameText[Index].text = TempweaponType.WeaponName;

                if (AllManager.instance.UIM.SelectItemExampleText[Index] == null) Debug.Log(4);
                AllManager.instance.UIM.SelectItemExampleText[Index].text = TempweaponType.WeaponText;
            }
        }
    }

    public void AccessorySelectAssignInfo(int Index, AccessoryType pET)
    {
        AccessoryData TempET = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(pET);

        ItemData TempFood = AllManager.instance.ResourceM.ConvertItemTypeToItemDataArr(ItemType.Food);

        if (AllManager.instance.GameModeM.CurrentPlayer != null)
        {
            var PlayerAccessoryLevel = AllManager.instance.GameModeM.CurrentPlayer.ConvertAccessoryLevelPlayer(pET);

            if (PlayerAccessoryLevel >= 5)
            {
                if (AllManager.instance.UIM.SelectItemIconImage[Index] == null) Debug.Log(2);
                AllManager.instance.UIM.SelectItemIconImage[Index].sprite = TempFood.SpriteImage;

                if (AllManager.instance.UIM.SelectItemNameText[Index] == null) Debug.Log(3);
                AllManager.instance.UIM.SelectItemNameText[Index].text = TempFood.ItemName;

                if (AllManager.instance.UIM.SelectItemExampleText[Index] == null) Debug.Log(4);
                AllManager.instance.UIM.SelectItemExampleText[Index].text = TempFood.ItemText;
            }

            else
            {
                AllManager.instance.UIM.SelectItemIconImage[Index].sprite = TempET.SpriteImage;
                AllManager.instance.UIM.SelectItemNameText[Index].text = TempET.AccessoryName;
                AllManager.instance.UIM.SelectItemExampleText[Index].text = TempET.AccessoryText;
            }
        }


        //SelectItemButton[Index].onClick.AddListener(() => { AllManager.instance.GameModeM.CurrentPlayer.UpdateGetAccessory(pET); }); 
    }

    // 인게임 무기 아이콘 갱신
    public void RenewCurrentWeaponIcon()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer.CurrentHP <= 0)
        {
            for (int i = 0; i < AllManager.instance.UIM.BasicCurrentWeaponIconImage.Length; i++)
            {
                AllManager.instance.UIM.BasicCurrentWeaponIconImage[i].gameObject.SetActive(false);
                AllManager.instance.UIM.PlayerInfosCurrentWeaponIconImage[i].gameObject.SetActive(false);

                var TempweaponTypeO = AllManager.instance.ResourceM.WeaponDatas;

                for (int j = 0; j < TempweaponTypeO.Length; j++)
                {
                    var TempweaponType = TempweaponTypeO[j].weaponType;

                    var TempWeaponSprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(TempweaponType).ItemImage;

                    if (AllManager.instance.UIM.WeaponIconList.Contains(TempWeaponSprite))
                    {
                        AllManager.instance.UIM.WeaponIconList.Remove(TempWeaponSprite);
                    }

                }
            }
        }

        else
        {
            for (int i = 0; i < AllManager.instance.UIM.WeaponIconList.Count; i++)
            {
                AllManager.instance.UIM.BasicCurrentWeaponIconImage[i].sprite = AllManager.instance.UIM.WeaponIconList[i];
                AllManager.instance.UIM.BasicCurrentWeaponIconImage[i].gameObject.SetActive(true);

                AllManager.instance.UIM.PlayerInfosCurrentWeaponIconImage[i].sprite = AllManager.instance.UIM.WeaponIconList[i];
                AllManager.instance.UIM.PlayerInfosCurrentWeaponIconImage[i].gameObject.SetActive(true);
            }
        }

    }

    // 인게임 장신구 아이콘 갱신
    public void RenewCurrentAccessoryIcon()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer.CurrentHP <= 0)
        {
            for (int i = 0; i < AllManager.instance.UIM.BasicCurrentAccessoryIconImage.Length; i++)
            {
                AllManager.instance.UIM.BasicCurrentAccessoryIconImage[i].gameObject.SetActive(false);
                AllManager.instance.UIM.PlayerInfosCurrentAccessoryIconImage[i].gameObject.SetActive(false);

                var TempETO = AllManager.instance.ResourceM.AccessoryDatas;

                for (int j = 0; j < TempETO.Length; j++)
                {
                    var TempET = TempETO[j].ET;

                    var TempAccessorySprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(TempET).SpriteImage;

                    if (AllManager.instance.UIM.AccessoryIconList.Contains(TempAccessorySprite))
                    {
                        AllManager.instance.UIM.AccessoryIconList.Remove(TempAccessorySprite);

                    }

                }
            }

        }

        else
        {
            for (int i = 0; i < AllManager.instance.UIM.AccessoryIconList.Count; i++)
            {
                AllManager.instance.UIM.BasicCurrentAccessoryIconImage[i].sprite = AllManager.instance.UIM.AccessoryIconList[i];
                AllManager.instance.UIM.BasicCurrentAccessoryIconImage[i].gameObject.SetActive(true);

                AllManager.instance.UIM.PlayerInfosCurrentAccessoryIconImage[i].sprite = AllManager.instance.UIM.AccessoryIconList[i];
                AllManager.instance.UIM.PlayerInfosCurrentAccessoryIconImage[i].gameObject.SetActive(true);
            }

        }
    }

    public bool CanCombineWeapon(Weapon WeaponA, Weapon WeaponB)
    {
        var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;
        var TempweaponTypeOA = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponA.weaponType);
        var TempWLATA = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeOA);
        var TempweaponTypeOB = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponB.weaponType);
        var TempWLATB = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeOB);

        return TempWLATA.Level >= 8 && TempWLATB.Level >= 8;
    }

    public Weapon CombineWeapon(Weapon WeaponA, Weapon WeaponB)
    {
        Weapon NewWeapon = null;

        if (CanCombineWeapon(WeaponA, WeaponB))
        {
            
        }

        return NewWeapon;
    }

}
