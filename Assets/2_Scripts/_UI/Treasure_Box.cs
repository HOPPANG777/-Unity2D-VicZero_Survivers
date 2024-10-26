using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Box : MonoBehaviour
{
    [Header("Treasure_Box")]
    public bool Treasure_Stop = false;
    public bool Treasure_Open_Chset = false;

    public Vector3[] Treasure_Gem_Vec_Pos = new Vector3[4];

    public float Treasure_Gem_Timer = 0;
    public int Treasure_Gem_Timer_Power = 1;

    public float Treasure_Timer = 0.0f;

    public int Treasure_Item_Number = 0;

    public float Mask_Roulette_Time = 0.0f;
    public float Mask_Roulette_Pos = 0.0f;

    public void Treasure_Chest_Roulette()
    {
        float Treasure_Reset_Pos_Y = 2300;
        float Treasure_Image_Move = 1000f;

        if (Treasure_Timer > All_Manager.instance.Audio_M.Treasure_Chest_AS.clip.length && !Treasure_Stop)
        {
            Treasure_Timer = 0;

            Treasure_Stop = true;

            float Dis = 10000;

            for (int i = 0; i < All_Manager.instance.UI_M.Treasure_Item_Image_01.GetComponent<RectTransform>().transform.childCount; i++)
            {
                if (Vector3.Distance(All_Manager.instance.UI_M.Treasure_Item_Image_01.GetComponent<RectTransform>().transform.GetChild(i).transform.position, All_Manager.instance.UI_M.Treasure_Reel_Image.rectTransform.position) < Dis)
                {
                    Dis = Vector3.Distance(All_Manager.instance.UI_M.Treasure_Item_Image_01.GetComponent<RectTransform>().transform.GetChild(i).transform.position, All_Manager.instance.UI_M.Treasure_Reel_Image.rectTransform.position);
                    Treasure_Item_Number = i;
                }
            }

            for (int j = 0; j < All_Manager.instance.UI_M.Treasure_Item_Image_02.GetComponent<RectTransform>().transform.childCount; j++)
            {
                if (Vector3.Distance(All_Manager.instance.UI_M.Treasure_Item_Image_02.GetComponent<RectTransform>().transform.GetChild(j).transform.position, All_Manager.instance.UI_M.Treasure_Reel_Image.rectTransform.position) < Dis)
                {
                    Dis = Vector3.Distance(All_Manager.instance.UI_M.Treasure_Item_Image_02.GetComponent<RectTransform>().transform.GetChild(j).transform.position, All_Manager.instance.UI_M.Treasure_Reel_Image.rectTransform.position);
                    Treasure_Item_Number = j;
                }
            }

            Treasure_Select_Item(Treasure_Item_Number);
            All_Manager.instance.UI_M.Treasure_Item_Mask.rectTransform.sizeDelta = new Vector2(100, 0);
            All_Manager.instance.UI_M.Treasure_Item_Mask.rectTransform.anchoredPosition = new Vector2(0, -50f);
            Mask_Roulette_Time = 0f;
            Mask_Roulette_Pos = 0f;
            All_Manager.instance.UI_M.Treasure_Gem_Image_List.SetActive(false);
        }

        else
        {
            Treasure_Timer += Time.deltaTime;
        }

        if (All_Manager.instance.UI_M.Treasure_Item_Image_01 != null && All_Manager.instance.UI_M.Treasure_Item_Image_02 != null && Treasure_Stop == false)
        {
            // 이미지를 위로 이동
            All_Manager.instance.UI_M.Treasure_Item_Image_01.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (Treasure_Image_Move * Time.deltaTime);
            All_Manager.instance.UI_M.Treasure_Item_Image_02.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (Treasure_Image_Move * Time.deltaTime);

            // 첫 번째 이미지가 기준점을 넘으면 재배치
            if (All_Manager.instance.UI_M.Treasure_Item_Image_01.GetComponent<RectTransform>().anchoredPosition.y >= Treasure_Reset_Pos_Y)
            {
                Reposition_Treasure_Image(All_Manager.instance.UI_M.Treasure_Item_Image_01.GetComponent<RectTransform>());
            }

            // 두 번째 이미지가 기준점을 넘으면 재배치
            if (All_Manager.instance.UI_M.Treasure_Item_Image_02.GetComponent<RectTransform>().anchoredPosition.y >= Treasure_Reset_Pos_Y)
            {
                Reposition_Treasure_Image(All_Manager.instance.UI_M.Treasure_Item_Image_02.GetComponent<RectTransform>());
            }
        }

        else if (Treasure_Stop == true)
        {
            All_Manager.instance.UI_M.Treasure_Close_Button.gameObject.SetActive(true);
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
                All_Manager.instance.UI_M.Treasure_Item_Mask.rectTransform.sizeDelta = new Vector2(100, Mask_Roulette_Time);
            }

            if (Mask_Roulette_Pos < 245)
            {
                All_Manager.instance.UI_M.Treasure_Item_Mask.rectTransform.anchoredPosition = new Vector2(0, Mask_Roulette_Pos);
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
        All_Manager.instance.UI_M.Treasure_Chest_Image.sprite = All_Manager.instance.UI_M.Open_Chest;
        Treasure_Open_Chset = true;
        All_Manager.instance.UI_M.Treasure_Reel_Image.gameObject.SetActive(true);
        All_Manager.instance.Audio_M.Treasure_Found_Audio_Play();
        All_Manager.instance.UI_M.Treasure_Gem_Image_List.SetActive(true);
    }

    public void Treasure_Close()
    {
        All_Manager.instance.UI_M.Treasure_Chest_Image.sprite = All_Manager.instance.UI_M.Close_Chest;
        All_Manager.instance.UI_M.Treasure_Close_Button.gameObject.SetActive(false);
        All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(false);
        Treasure_Open_Chset = false;
        Treasure_Item_Number = 0;
        All_Manager.instance.UI_M.Treasure_Reel_Image.gameObject.SetActive(false);
        All_Manager.instance.UI_M.Treasure_Item_Mask.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
        All_Manager.instance.UI_M.Treasure_Item_Mask.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50f);
        Mask_Roulette_Time = 0f;
        Mask_Roulette_Pos = 0f;
        Treasure_Timer = 0f;
        Treasure_Stop = false;
        All_Manager.instance.UI_M.Treasure_Found_Text.gameObject.SetActive(true);
        All_Manager.instance.UI_M.Treasure_Found_Text.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        All_Manager.instance.UI_M.Treasure_Found_Text.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
        All_Manager.instance.UI_M.Treasure_Open_Button.gameObject.SetActive(true);
        All_Manager.instance.UI_M.Treasure_Open_Button.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        All_Manager.instance.UI_M.Treasure_Open_Button.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
        All_Manager.instance.UI_M.Treasure_Close_Button.gameObject.SetActive(false);
        All_Manager.instance.UI_M.Off_Panel();
    }

    public void Treasure_Select_Item(int Item_Num)
    {
        if (Item_Num == 0)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.Axe);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Axe).Item_Image;
        }

        if (Item_Num == 1)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.KingBible);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.KingBible).Item_Image;
        }

        if (Item_Num == 2)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.Peachone);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Peachone).Item_Image;
        }

        if (Item_Num == 3)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.FireWand);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.FireWand).Item_Image;
        }

        if (Item_Num == 4)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.Cross);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Cross).Item_Image;
        }

        if (Item_Num == 5)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.Garlic);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Garlic).Item_Image;
        }

        if (Item_Num == 6)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.LightningRing);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.LightningRing).Item_Image;
        }

        if (Item_Num == 7)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.ShadowPinion);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.ShadowPinion).Item_Image;
        }

        if (Item_Num == 8)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.RuneTracer);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.RuneTracer).Item_Image;
        }

        if (Item_Num == 9)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Weapon_Select_Assign_Button(Weapon_Type.Whip);
           All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
           All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Whip).Item_Image;
        }

        if (Item_Num == 10)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Spinach);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Spinach).Sprite_Image;
        }

        if (Item_Num == 11)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Bracer);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Bracer).Sprite_Image;
        }

        if (Item_Num == 12)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Duplicator);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Duplicator).Sprite_Image;
        }

        if (Item_Num == 13)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.EmptyTome);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.EmptyTome).Sprite_Image;
        }

        if (Item_Num == 14)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Wings);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Wings).Sprite_Image;
        }

        if (Item_Num == 15)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Candelabrador);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Candelabrador).Sprite_Image;
        }

        if (Item_Num == 16)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Armour);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Armour).Sprite_Image;
        }

        if (Item_Num == 17)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.HollowHeart);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.HollowHeart).Sprite_Image;
        }

        if (Item_Num == 18)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.SpellBinder);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.SpellBinder).Sprite_Image;
        }

        if (Item_Num == 19)
        {
            All_Manager.instance.UI_M.Player_Level_Up.Equipment_Select_Assign_Button(Equipment_Type.Attractorb);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.gameObject.SetActive(true);
            All_Manager.instance.UI_M.Treasure_Select_Item_Image.sprite = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment_Arr(Equipment_Type.Attractorb).Sprite_Image;
        }
    }

    public void Treasure_Open_Button_And_Found_Text_Disabled()
    {
        if (Treasure_Open_Chset == true)
        {
            if (All_Manager.instance.UI_M.Treasure_Found_Text.GetComponent<RectTransform>().transform.localScale.x > 0.01f && All_Manager.instance.UI_M.Treasure_Open_Button.GetComponent<RectTransform>().transform.localScale.x > 0.01f)
            {
                All_Manager.instance.UI_M.Treasure_Found_Text.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                All_Manager.instance.UI_M.Treasure_Open_Button.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                All_Manager.instance.UI_M.Treasure_Open_Button.GetComponent<RectTransform>().transform.Rotate(0, 0, 100f * Time.deltaTime);
            }
            else
            {
                All_Manager.instance.UI_M.Treasure_Found_Text.gameObject.SetActive(false);
                All_Manager.instance.UI_M.Treasure_Open_Button.gameObject.SetActive(false);
            }
        }

        else
        {

        }

    }

}
