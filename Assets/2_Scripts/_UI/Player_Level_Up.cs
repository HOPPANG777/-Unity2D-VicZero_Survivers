using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Level_Up : MonoBehaviour
{

    public void Renew_Start_Weapon_Panel()
    {
        List<int> Selected_Start_Weapon_List = new List<int>();
        do
        {
            All_Manager.instance.UI_M.Start_First_Weapon_Number = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length - 1);

        } while (Selected_Start_Weapon_List.Contains(All_Manager.instance.UI_M.Start_First_Weapon_Number));

        Selected_Start_Weapon_List.Add(All_Manager.instance.UI_M.Start_First_Weapon_Number);

        do
        {
            All_Manager.instance.UI_M.Start_Second_Weapon_Number = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length - 1);

        } while (Selected_Start_Weapon_List.Contains(All_Manager.instance.UI_M.Start_Second_Weapon_Number));

        Selected_Start_Weapon_List.Add(All_Manager.instance.UI_M.Start_Second_Weapon_Number);


        var Temp_weaponType_First = (WeaponType)All_Manager.instance.UI_M.Start_First_Weapon_Number;

        STO_Weapon Convert_Weapon_First = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(Temp_weaponType_First);

        All_Manager.instance.UI_M.Start_Weapon_Icon_Image[0].sprite = Convert_Weapon_First.Item_Image;
        All_Manager.instance.UI_M.Start_Weapon_Name_Text[0].text = Convert_Weapon_First.Weapon_Name;
        All_Manager.instance.UI_M.Start_Weapon_Example_Text[0].text = Convert_Weapon_First.Weapon_Text;

        var Temp_weaponType_Second = (WeaponType)All_Manager.instance.UI_M.Start_Second_Weapon_Number;

        Weapon_Select_Assign_Info(1, Temp_weaponType_Second);

        STO_Weapon Convert_Weapon_Second = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(Temp_weaponType_Second);

        All_Manager.instance.UI_M.Start_Weapon_Icon_Image[1].sprite = Convert_Weapon_Second.Item_Image;
        All_Manager.instance.UI_M.Start_Weapon_Name_Text[1].text = Convert_Weapon_Second.Weapon_Name;
        All_Manager.instance.UI_M.Start_Weapon_Example_Text[1].text = Convert_Weapon_Second.Weapon_Text;
    }

    public void Click_Start_Weapon_Select(int p_Index)
    {

        if (p_Index == 0)
        {
            var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Start_First_Weapon_Number;

            Weapon_Select_Assign_Button(Temp_weaponType);
        }

        else if (p_Index == 1)
        {
            var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Start_Second_Weapon_Number;

            Weapon_Select_Assign_Button(Temp_weaponType);
        }

        All_Manager.instance.UI_M.Off_Panel();
    }
    public void Renew_Item_Panel()
    {
        Renew_Current_Weapon_Icon();
        Renew_Current_Accessory_Icon();

        int Max = System.Enum.GetValues(typeof(WeaponType)).Length + System.Enum.GetValues(typeof(AccessoryType)).Length;
        List<int> Selected_Item_List = new List<int>();
        do
        {   // 첫번째 패널에 나타날 아이템 룰렛 
            All_Manager.instance.UI_M.Select_First_Item_Number = UnityEngine.Random.Range(0, Max - 1);

        } while (Selected_Item_List.Contains(All_Manager.instance.UI_M.Select_First_Item_Number));

        Selected_Item_List.Add(All_Manager.instance.UI_M.Select_First_Item_Number);


        do
        {   // 두번째 패널에 나타날 아이템 룰렛 
            All_Manager.instance.UI_M.Select_Second_Item_Number = UnityEngine.Random.Range(0, Max - 1);

        } while (Selected_Item_List.Contains(All_Manager.instance.UI_M.Select_Second_Item_Number));

        Selected_Item_List.Add(All_Manager.instance.UI_M.Select_Second_Item_Number);

        do
        {   // 세번째 패널에 나타날 아이템 룰렛 
            All_Manager.instance.UI_M.Select_Third_Item_Number = UnityEngine.Random.Range(0, Max - 1);

        } while (Selected_Item_List.Contains(All_Manager.instance.UI_M.Select_Third_Item_Number));

        Selected_Item_List.Add(All_Manager.instance.UI_M.Select_Third_Item_Number);


        //Select_First_Item_Number
        if (All_Manager.instance.UI_M.Select_First_Item_Number < System.Enum.GetValues(typeof(WeaponType)).Length)
        {
            //무기
            var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Select_First_Item_Number;
            Weapon_Select_Assign_Info(0, Temp_weaponType);
        }

        else
        {
            //장비
            var Temp_ET = (AccessoryType)(All_Manager.instance.UI_M.Select_First_Item_Number - System.Enum.GetValues(typeof(WeaponType)).Length);
            Accessory_Select_Assign_Info(0, Temp_ET);
        }


        //Select_Second_Item_Number
        if (All_Manager.instance.UI_M.Select_Second_Item_Number < System.Enum.GetValues(typeof(WeaponType)).Length)
        {
            //무기
            var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Select_Second_Item_Number;
            Weapon_Select_Assign_Info(1, Temp_weaponType);
        }

        else
        {
            //장비
            var Temp_ET = (AccessoryType)(All_Manager.instance.UI_M.Select_Second_Item_Number - System.Enum.GetValues(typeof(WeaponType)).Length);
            Accessory_Select_Assign_Info(1, Temp_ET);
        }

        //Select_Third_Item_Number
        if (All_Manager.instance.UI_M.Select_Third_Item_Number < System.Enum.GetValues(typeof(WeaponType)).Length)
        {
            //무기
            var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Select_Third_Item_Number;
            Weapon_Select_Assign_Info(2, Temp_weaponType);
        }

        else
        {
            //장비
            var Temp_ET = (AccessoryType)(All_Manager.instance.UI_M.Select_Third_Item_Number - System.Enum.GetValues(typeof(WeaponType)).Length);
            Accessory_Select_Assign_Info(2, Temp_ET);
        }
    }

    public void Click_Select_Item_Button(int p_Index)
    {
        //Select_First_Item_Number 
        if (p_Index == 0)
        {
            if (All_Manager.instance.UI_M.Select_First_Item_Number < System.Enum.GetValues(typeof(WeaponType)).Length)
            {
                var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Select_First_Item_Number;

                Weapon_Select_Assign_Button(Temp_weaponType);
            }

            else
            {
                var Temp_ET = (AccessoryType)(All_Manager.instance.UI_M.Select_First_Item_Number - System.Enum.GetValues(typeof(WeaponType)).Length);

                Accessory_Select_Assign_Button(Temp_ET);
            }
        }

        //Select_Second_Item_Number
        else if (p_Index == 1)
        {
            if (All_Manager.instance.UI_M.Select_Second_Item_Number < System.Enum.GetValues(typeof(WeaponType)).Length)
            {
                var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Select_Second_Item_Number;

                Weapon_Select_Assign_Button(Temp_weaponType);
            }

            else
            {
                var Temp_ET = (AccessoryType)(All_Manager.instance.UI_M.Select_Second_Item_Number - System.Enum.GetValues(typeof(WeaponType)).Length);

                Accessory_Select_Assign_Button(Temp_ET);
            }
        }

        //Select_Third_Item_Number
        else if (p_Index == 2)
        {
            if (All_Manager.instance.UI_M.Select_Third_Item_Number < System.Enum.GetValues(typeof(WeaponType)).Length)
            {
                var Temp_weaponType = (WeaponType)All_Manager.instance.UI_M.Select_Third_Item_Number;

                Weapon_Select_Assign_Button(Temp_weaponType);
            }

            else
            {
                var Temp_ET = (AccessoryType)(All_Manager.instance.UI_M.Select_Third_Item_Number - System.Enum.GetValues(typeof(WeaponType)).Length);

                Accessory_Select_Assign_Button(Temp_ET);
            }
        }

        All_Manager.instance.UI_M.Off_Panel();
        All_Manager.instance.UI_M.Update_Gem_Count_Image(0);
    }

    public void Weapon_Select_Assign_Button(WeaponType p_weaponType)
    {
        STO_Weapon Temp_weaponType = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        var Player_Weapon_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_weaponType);

        if (Player_Weapon_Level.Level >= 8)
        {
            All_Manager.instance.GameMode_M.Current_Player.Recovery_Current_HP();
        }

        else
        {
            All_Manager.instance.GameMode_M.Current_Player.Upgrade_Weapon(p_weaponType);

            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                if (Temp_weaponType != null)
                {
                    bool Sprite_Currect = All_Manager.instance.UI_M.Weapon_Icon_List.Contains(Temp_weaponType.Item_Image);

                    if (!Sprite_Currect)
                    {
                        All_Manager.instance.UI_M.Weapon_Icon_List.Add(Temp_weaponType.Item_Image);
                    }

                    else
                    {
                        //Debug.Log("이미 무기 아이콘이 존재합니다");
                    }

                    for (int i = 0; i < All_Manager.instance.UI_M.Weapon_Icon_List.Count; i++)
                    {
                        if (All_Manager.instance.UI_M.Weapon_Icon_List[i] == Temp_weaponType.Item_Image)
                        {
                            int Weapon_Level_Temp = 0;

                            for (int j = 0; j < Player_Weapon_Level.Level; j++)
                            {
                                Weapon_Level_Temp++;
                            }

                            All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image[i].GetComponentInChildren<Text>().text = "Lv. " + Weapon_Level_Temp;
                        }
                    }

                    Renew_Current_Weapon_Icon();
                }
            }
        }


    }

    public void Accessory_Select_Assign_Button(AccessoryType p_ET)
    {
        STO_Accessory Temp_ET = All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(p_ET);

        var Player_Accessory_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_Accessory_Level_Player(p_ET);

        if (Player_Accessory_Level >= 5)
        {
            All_Manager.instance.GameMode_M.Current_Player.Recovery_Current_HP();
        }

        else
        {
            All_Manager.instance.GameMode_M.Current_Player.Upgrade_Accessory(p_ET);

            All_Manager.instance.GameMode_M.Current_Player.Origin_Stat_Application_Player(p_ET);

            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                if (Temp_ET != null)
                {

                    bool Sprite_Currect = All_Manager.instance.UI_M.Accessory_Icon_List.Contains(Temp_ET.Sprite_Image);

                    if (!Sprite_Currect)
                    {
                        All_Manager.instance.UI_M.Accessory_Icon_List.Add(Temp_ET.Sprite_Image);
                    }

                    else
                    {
                        //Debug.Log("이미 장신구 아이콘이 존재합니다.");
                    }

                    for (int i = 0; i < All_Manager.instance.UI_M.Accessory_Icon_List.Count; i++)
                    {
                        if (All_Manager.instance.UI_M.Accessory_Icon_List[i] == Temp_ET.Sprite_Image)
                        {
                            int Accessory_Level_Temp = 0;

                            var Temp_Current_Accessory_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_Accessory_Level_Player(p_ET);

                            for (int j = 0; j < Temp_Current_Accessory_Level; j++)
                            {
                                Accessory_Level_Temp++;
                            }

                            All_Manager.instance.UI_M.Basic_Current_Accessory_Icon_Image[i].GetComponentInChildren<Text>().text = "Lv. " + Accessory_Level_Temp;
                        }

                    }

                    Renew_Current_Accessory_Icon();
                }

            }
        }


    }

    public void Weapon_Select_Assign_Info(int Index, WeaponType p_weaponType)
    {
        STO_Weapon Temp_weaponType = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(p_weaponType);

        STO_Item Temp_Food = All_Manager.instance.Resource_M.Convert_ItemType_To_STO_Item_Arr(ItemType.Food);

        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Player_Weapon_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_weaponType);

            if (Player_Weapon_Level.Level >= 8)
            {
                if (All_Manager.instance.UI_M.Select_Item_Icon_Image[Index] == null) Debug.Log(2);
                All_Manager.instance.UI_M.Select_Item_Icon_Image[Index].sprite = Temp_Food.Sprite_Image;

                if (All_Manager.instance.UI_M.Select_Item_Name_Text[Index] == null) Debug.Log(3);
                All_Manager.instance.UI_M.Select_Item_Name_Text[Index].text = Temp_Food.Item_Name;

                if (All_Manager.instance.UI_M.Select_Item_Example_Text[Index] == null) Debug.Log(4);
                All_Manager.instance.UI_M.Select_Item_Example_Text[Index].text = Temp_Food.Item_Text;
            }

            else
            {
                if (All_Manager.instance.UI_M.Select_Item_Icon_Image[Index] == null) Debug.Log(2);
                All_Manager.instance.UI_M.Select_Item_Icon_Image[Index].sprite = Temp_weaponType.Item_Image;

                if (All_Manager.instance.UI_M.Select_Item_Name_Text[Index] == null) Debug.Log(3);
                All_Manager.instance.UI_M.Select_Item_Name_Text[Index].text = Temp_weaponType.Weapon_Name;

                if (All_Manager.instance.UI_M.Select_Item_Example_Text[Index] == null) Debug.Log(4);
                All_Manager.instance.UI_M.Select_Item_Example_Text[Index].text = Temp_weaponType.Weapon_Text;
            }
        }
    }

    public void Accessory_Select_Assign_Info(int Index, AccessoryType p_ET)
    {
        STO_Accessory Temp_ET = All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(p_ET);

        STO_Item Temp_Food = All_Manager.instance.Resource_M.Convert_ItemType_To_STO_Item_Arr(ItemType.Food);

        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Player_Accessory_Level = All_Manager.instance.GameMode_M.Current_Player.Convert_Accessory_Level_Player(p_ET);

            if (Player_Accessory_Level >= 5)
            {
                if (All_Manager.instance.UI_M.Select_Item_Icon_Image[Index] == null) Debug.Log(2);
                All_Manager.instance.UI_M.Select_Item_Icon_Image[Index].sprite = Temp_Food.Sprite_Image;

                if (All_Manager.instance.UI_M.Select_Item_Name_Text[Index] == null) Debug.Log(3);
                All_Manager.instance.UI_M.Select_Item_Name_Text[Index].text = Temp_Food.Item_Name;

                if (All_Manager.instance.UI_M.Select_Item_Example_Text[Index] == null) Debug.Log(4);
                All_Manager.instance.UI_M.Select_Item_Example_Text[Index].text = Temp_Food.Item_Text;
            }

            else
            {
                All_Manager.instance.UI_M.Select_Item_Icon_Image[Index].sprite = Temp_ET.Sprite_Image;
                All_Manager.instance.UI_M.Select_Item_Name_Text[Index].text = Temp_ET.Accessory_Name;
                All_Manager.instance.UI_M.Select_Item_Example_Text[Index].text = Temp_ET.Accessory_Text;
            }
        }


        //Select_Item_Button[Index].onClick.AddListener(() => { All_Manager.instance.GameMode_M.Current_Player.Update_Get_Accessory(p_ET); }); 
    }

    // 인게임 무기 아이콘 갱신
    public void Renew_Current_Weapon_Icon()
    {
        if (All_Manager.instance.GameMode_M.Current_Player.Current_HP <= 0)
        {
            for (int i = 0; i < All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image.Length; i++)
            {
                All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image[i].gameObject.SetActive(false);
                All_Manager.instance.UI_M.Player_Info_s_Current_Weapon_Icon_Image[i].gameObject.SetActive(false);

                var Temp_weaponTypeO = All_Manager.instance.Resource_M.STO_Weapons;

                for (int j = 0; j < Temp_weaponTypeO.Length; j++)
                {
                    var Temp_weaponType = Temp_weaponTypeO[j].weaponType;

                    var Temp_Weapon_Sprite = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(Temp_weaponType).Item_Image;

                    if (All_Manager.instance.UI_M.Weapon_Icon_List.Contains(Temp_Weapon_Sprite))
                    {
                        All_Manager.instance.UI_M.Weapon_Icon_List.Remove(Temp_Weapon_Sprite);
                    }

                }
            }
        }

        else
        {
            for (int i = 0; i < All_Manager.instance.UI_M.Weapon_Icon_List.Count; i++)
            {
                All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image[i].sprite = All_Manager.instance.UI_M.Weapon_Icon_List[i];
                All_Manager.instance.UI_M.Basic_Current_Weapon_Icon_Image[i].gameObject.SetActive(true);

                All_Manager.instance.UI_M.Player_Info_s_Current_Weapon_Icon_Image[i].sprite = All_Manager.instance.UI_M.Weapon_Icon_List[i];
                All_Manager.instance.UI_M.Player_Info_s_Current_Weapon_Icon_Image[i].gameObject.SetActive(true);
            }
        }

    }

    // 인게임 장신구 아이콘 갱신
    public void Renew_Current_Accessory_Icon()
    {
        if (All_Manager.instance.GameMode_M.Current_Player.Current_HP <= 0)
        {
            for (int i = 0; i < All_Manager.instance.UI_M.Basic_Current_Accessory_Icon_Image.Length; i++)
            {
                All_Manager.instance.UI_M.Basic_Current_Accessory_Icon_Image[i].gameObject.SetActive(false);
                All_Manager.instance.UI_M.Player_Info_s_Current_Accessory_Icon_Image[i].gameObject.SetActive(false);

                var Temp_ETO = All_Manager.instance.Resource_M.STO_Accessorys;

                for (int j = 0; j < Temp_ETO.Length; j++)
                {
                    var Temp_ET = Temp_ETO[j].ET;

                    var Temp_Accessory_Sprite = All_Manager.instance.Resource_M.Convert_AccessoryType_To_STO_Accessory_Arr(Temp_ET).Sprite_Image;

                    if (All_Manager.instance.UI_M.Accessory_Icon_List.Contains(Temp_Accessory_Sprite))
                    {
                        All_Manager.instance.UI_M.Accessory_Icon_List.Remove(Temp_Accessory_Sprite);

                    }

                }
            }

        }

        else
        {
            for (int i = 0; i < All_Manager.instance.UI_M.Accessory_Icon_List.Count; i++)
            {
                All_Manager.instance.UI_M.Basic_Current_Accessory_Icon_Image[i].sprite = All_Manager.instance.UI_M.Accessory_Icon_List[i];
                All_Manager.instance.UI_M.Basic_Current_Accessory_Icon_Image[i].gameObject.SetActive(true);

                All_Manager.instance.UI_M.Player_Info_s_Current_Accessory_Icon_Image[i].sprite = All_Manager.instance.UI_M.Accessory_Icon_List[i];
                All_Manager.instance.UI_M.Player_Info_s_Current_Accessory_Icon_Image[i].gameObject.SetActive(true);
            }

        }
    }

    public bool Can_Combine_Weapon(Weapon Weapon_A, Weapon Weapon_B)
    {
        var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
        var Temp_weaponTypeO_A = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(Weapon_A.weaponType);
        var Temp_WLAT_A = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_weaponTypeO_A);
        var Temp_weaponTypeO_B = All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(Weapon_B.weaponType);
        var Temp_WLAT_B = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_weaponTypeO_B);

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
