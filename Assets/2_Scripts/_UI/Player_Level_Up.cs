using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Level_Up : MonoBehaviour
{
    private List<Weapon_Type> availableWeapons;

    void Start()
    {
        InitializeAvailableWeapons();
    }

    private void InitializeAvailableWeapons()
    {
        availableWeapons = new List<Weapon_Type>
        {
            Weapon_Type.Axe,
            Weapon_Type.KingBible,
            Weapon_Type.Peachone,
            Weapon_Type.FireWand,
            Weapon_Type.Cross,
            Weapon_Type.Garlic,
            Weapon_Type.LightningRing,
            Weapon_Type.ShadowPinion,
            Weapon_Type.RuneTracer,
            Weapon_Type.Whip
        };
    }

    public void Renew_Start_Weapon_Panel()
    {
        List<int> selectedWeapons = SelectUniqueRandomNumbers(2, availableWeapons.Count);

        AssignWeaponInfoToUI(0, selectedWeapons[0]);
        AssignWeaponInfoToUI(1, selectedWeapons[1]);
    }

    private List<int> SelectUniqueRandomNumbers(int count, int max)
    {
        List<int> selectedNumbers = new List<int>();
        while (selectedNumbers.Count < count)
        {
            int randomNumber = UnityEngine.Random.Range(0, max);
            if (!selectedNumbers.Contains(randomNumber))
            {
                selectedNumbers.Add(randomNumber);
            }
        }
        return selectedNumbers;
    }

    private void AssignWeaponInfoToUI(int index, int weaponIndex)
    {
        var weaponType = availableWeapons[weaponIndex];
        var weaponData = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(weaponType);

        All_Manager.instance.UI_M.Start_Weapon_Icon_Image[index].sprite = weaponData.Item_Image;
        All_Manager.instance.UI_M.Start_Weapon_Name_Text[index].text = weaponData.Weapon_Name;
        All_Manager.instance.UI_M.Start_Weapon_Example_Text[index].text = weaponData.Weapon_Text;
    }

    public void Click_Start_Weapon_Select(int index)
    {
        var selectedWeaponIndex = (index == 0)
            ? All_Manager.instance.UI_M.Start_First_Weapon_Number
            : All_Manager.instance.UI_M.Start_Second_Weapon_Number;

        var weaponType = availableWeapons[selectedWeaponIndex];
        Weapon_Select_Assign_Button(weaponType);

        All_Manager.instance.UI_M.Off_Panel();
    }

    public void Renew_Item_Panel()
    {
        Renew_Current_Weapon_Icon();
        Renew_Current_Equipment_Icon();

        List<int> selectedItems = SelectUniqueRandomNumbers(3, availableWeapons.Count + System.Enum.GetValues(typeof(Equipment_Type)).Length);

        AssignItemInfoToUI(0, selectedItems[0]);
        AssignItemInfoToUI(1, selectedItems[1]);
        AssignItemInfoToUI(2, selectedItems[2]);
    }

    private void AssignItemInfoToUI(int index, int itemIndex)
    {
        if (itemIndex < availableWeapons.Count)
        {
            Weapon_Select_Assign_Info(index, availableWeapons[itemIndex]);
        }
        else
        {
            Equipment_Select_Assign_Info(index, (Equipment_Type)(itemIndex - availableWeapons.Count));
        }
    }

    public void Click_Select_Item_Button(int index)
    {
        int selectedItemNumber = index switch
        {
            0 => All_Manager.instance.UI_M.Select_First_Item_Number,
            1 => All_Manager.instance.UI_M.Select_Second_Item_Number,
            _ => All_Manager.instance.UI_M.Select_Third_Item_Number,
        };

        if (selectedItemNumber < availableWeapons.Count)
        {
            Weapon_Select_Assign_Button(availableWeapons[selectedItemNumber]);
        }
        else
        {
            Equipment_Select_Assign_Button((Equipment_Type)(selectedItemNumber - availableWeapons.Count));
        }

        All_Manager.instance.UI_M.Off_Panel();
        All_Manager.instance.UI_M.Update_Gem_Count_Image(0);
    }

    public void Weapon_Select_Assign_Button(Weapon_Type weaponType)
    {
        var weaponData = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(weaponType);
        var weaponLevel = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(weaponData);

        if (weaponLevel.Level >= 8)
        {
            All_Manager.instance.GameMode_M.Current_Player.Recovery_Current_HP();
        }
        else
        {
            All_Manager.instance.GameMode_M.Current_Player.Upgrade_Weapon(weaponType);
            UpdateWeaponIcon(weaponData, weaponLevel.Level);
        }
    }

    private void UpdateWeaponIcon(STO_Weapon weaponData, int weaponLevel)
    {
        bool spriteExists = All_Manager.instance.UI_M.Weapon_Icon_List.Contains(weaponData.Item_Image);
        if (!spriteExists)
        {
            All_Manager.instance.UI_M.Weapon_Icon_List.Add(weaponData.Item_Image);
        }

        int index = All_Manager.instance.UI_M.Weapon_Icon_List.IndexOf(weaponData.Item_Image);
        All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image[index].GetComponentInChildren<Text>().text = "Lv. " + weaponLevel;
        Renew_Current_Weapon_Icon();
    }

    public void Equipment_Select_Assign_Button(Equipment_Type equipmentType)
    {
        var equipmentData = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(equipmentType);
        var equipmentLevel = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(equipmentType);

        if (equipmentLevel >= 5)
        {
            All_Manager.instance.GameMode_M.Current_Player.Recovery_Current_HP();
        }
        else
        {
            All_Manager.instance.GameMode_M.Current_Player.Upgrade_Equipment(equipmentType);
            All_Manager.instance.GameMode_M.Current_Player.Origin_Stat_Application_Player(equipmentType);
            UpdateEquipmentIcon(equipmentData, equipmentLevel);
        }
    }

    private void UpdateEquipmentIcon(STO_Equipment equipmentData, int equipmentLevel)
    {
        bool spriteExists = All_Manager.instance.UI_M.Equipment_Icon_List.Contains(equipmentData.Sprite_Image);
        if (!spriteExists)
        {
            All_Manager.instance.UI_M.Equipment_Icon_List.Add(equipmentData.Sprite_Image);
        }

        int index = All_Manager.instance.UI_M.Equipment_Icon_List.IndexOf(equipmentData.Sprite_Image);
        All_Manager.instance.UI_M.Basic_Current_Equipment_Icon_Image[index].GetComponentInChildren<Text>().text = "Lv. " + equipmentLevel;
        Renew_Current_Equipment_Icon();
    }

    public void Weapon_Select_Assign_Info(int index, Weapon_Type weaponType)
    {
        var weaponData = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(weaponType);
        AssignWeaponInfoToUI(index, weaponData);
    }

    public void Equipment_Select_Assign_Info(int index, Equipment_Type equipmentType)
    {
        var equipmentData = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(equipmentType);
        AssignEquipmentInfoToUI(index, equipmentData);
    }

    private void AssignWeaponInfoToUI(int index, STO_Weapon weaponData)
    {
        var playerWeaponLevel = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(weaponData);
        bool isMaxLevel = playerWeaponLevel.Level >= 8;

        if (isMaxLevel)
        {
            // 레벨이 8 이상일 때, MISC 정보를 할당
            var miscData = All_Manager.instance.Resource_M.Convert_MISC_Type_To_STO_MISC_Arr(MISC_Type.Food);
            All_Manager.instance.UI_M.Select_Item_Icon_Image[index].sprite = miscData.Sprite_Image;
            All_Manager.instance.UI_M.Select_Item_Name_Text[index].text = miscData.MISC_Name;
            All_Manager.instance.UI_M.Select_Item_Example_Text[index].text = miscData.MISC_Text;
        }
        else
        {
            // 그렇지 않으면 무기 정보를 할당
            All_Manager.instance.UI_M.Select_Item_Icon_Image[index].sprite = weaponData.Item_Image;
            All_Manager.instance.UI_M.Select_Item_Name_Text[index].text = weaponData.Weapon_Name;
            All_Manager.instance.UI_M.Select_Item_Example_Text[index].text = weaponData.Weapon_Text;
        }
    }

    private void AssignEquipmentInfoToUI(int index, STO_Equipment equipmentData)
    {
        var playerEquipmentLevel = All_Manager.instance.GameMode_M.Current_Player.Convert_Equipment_Level_Player(equipmentData.ET);
        bool isMaxLevel = playerEquipmentLevel >= 5;

        if (isMaxLevel)
        {
            // 레벨이 5 이상일 경우, MISC 정보를 UI에 할당
            var miscData = All_Manager.instance.Resource_M.Convert_MISC_Type_To_STO_MISC_Arr(MISC_Type.Food);
            All_Manager.instance.UI_M.Select_Item_Icon_Image[index].sprite = miscData.Sprite_Image;
            All_Manager.instance.UI_M.Select_Item_Name_Text[index].text = miscData.MISC_Name;
            All_Manager.instance.UI_M.Select_Item_Example_Text[index].text = miscData.MISC_Text;
        }
        else
        {
            // 레벨이 5 미만일 경우, 장비 정보를 UI에 할당
            All_Manager.instance.UI_M.Select_Item_Icon_Image[index].sprite = equipmentData.Sprite_Image;
            All_Manager.instance.UI_M.Select_Item_Name_Text[index].text = equipmentData.Equipment_Name;
            All_Manager.instance.UI_M.Select_Item_Example_Text[index].text = equipmentData.Equipment_Text;
        }
    }

    public void Renew_Current_Weapon_Icon()
    {
        UpdateIconUI(All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image, All_Manager.instance.UI_M.Weapon_Icon_List, "weapon");
    }

    public void Renew_Current_Equipment_Icon()
    {
        UpdateIconUI(All_Manager.instance.UI_M.Basic_Current_Equipment_Icon_Image, All_Manager.instance.UI_M.Equipment_Icon_List, "equipment");
    }

    private void UpdateIconUI(Image[] iconImages, List<Sprite> iconList, string type)
    {
        bool isPlayerAlive = All_Manager.instance.GameMode_M.Current_Player.Current_HP > 0;
        for (int i = 0; i < iconImages.Length; i++)
        {
            iconImages[i].gameObject.SetActive(isPlayerAlive && i < iconList.Count);
            if (isPlayerAlive && i < iconList.Count)
            {
                iconImages[i].sprite = iconList[i];
            }
        }
    }

    public bool Can_Combine_Weapon(Weapon Weapon_A, Weapon Weapon_B)
    {
        var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
        var Temp_WTO_A = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_A.WT);
        var Temp_WLAT_A = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO_A);
        var Temp_WTO_B = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_B.WT);
        var Temp_WLAT_B = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO_B);

        return Temp_WLAT_A.Level >= 8 && Temp_WLAT_B.Level >= 8;
    }

    public Weapon Combine_Weapon(Weapon Weapon_A, Weapon Weapon_B)
    {
        Weapon New_Weapon = null;

        if (Can_Combine_Weapon(Weapon_A, Weapon_B))
        {
            
        }

        return New_Weapon;
    }

}

