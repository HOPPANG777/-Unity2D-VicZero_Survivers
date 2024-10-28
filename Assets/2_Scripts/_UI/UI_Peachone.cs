using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Peachone : MonoBehaviour
{
    // Peachone무기 공격범위
    public void Update_Peachone_Range_Image()
    {
        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
            var W_UI_Peachone_Range_S = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Peachone).Info_s[Temp_Player.Peachone_LAT.Level];

            if (All_Manager.instance.GameMode_M.Current_Player.Peachone_LAT.Level == 0)
            {
                All_Manager.instance.UI_M.Peachone_Range_Image.gameObject.SetActive(false);
            }

            else
            {
                All_Manager.instance.UI_M.Peachone_Range_Image.gameObject.SetActive(true);

                All_Manager.instance.UI_M.Peachone_Range_Image.rectTransform.sizeDelta = new Vector2(Temp_Player.Current_Weapon_Range(Weapon_Type.Peachone, Temp_Player.Peachone_LAT), Temp_Player.Current_Weapon_Range(Weapon_Type.Peachone, Temp_Player.Peachone_LAT)) * 0.5f;

                All_Manager.instance.UI_M.W_Peachone_Range_Deg += Time.deltaTime * W_UI_Peachone_Range_S.Weapon_Speed * 20.0f;
                if (All_Manager.instance.UI_M.W_Peachone_Range_Deg < 360)
                {
                    var rad = Mathf.Deg2Rad * (All_Manager.instance.UI_M.W_Peachone_Range_Deg);
                    var x = (Temp_Player.Current_Weapon_Range(Weapon_Type.Peachone, Temp_Player.Peachone_LAT) * 0.4f) * Mathf.Sin(rad);
                    var y = (Temp_Player.Current_Weapon_Range(Weapon_Type.Peachone, Temp_Player.Peachone_LAT) * 0.4f) * Mathf.Cos(rad);
                    All_Manager.instance.UI_M.Peachone_Range_Image.rectTransform.position = Temp_Player.transform.position + new Vector3(x, y);
                }
                else
                {
                    All_Manager.instance.UI_M.W_Peachone_Range_Deg = 0;
                }
            }

        }

        else
        {
            All_Manager.instance.UI_M.Peachone_Range_Image.gameObject.SetActive(false);
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
                All_Manager.instance.UI_M.Peachone_Image.gameObject.SetActive(false);
            }

            else
            {
                All_Manager.instance.UI_M.Peachone_Image.gameObject.SetActive(true);

                All_Manager.instance.UI_M.W_Peachone_Deg += Time.deltaTime * 10f;
                if (All_Manager.instance.UI_M.W_Peachone_Deg < 360)
                {
                    var rad = Mathf.Deg2Rad * (All_Manager.instance.UI_M.W_Peachone_Deg);
                    var x = 0.7f * Mathf.Sin(rad);
                    var y = 0.7f * Mathf.Cos(rad);
                    All_Manager.instance.UI_M.Peachone_Image.rectTransform.position = Temp_Player.transform.position + new Vector3(x, y);

                }
                else
                {
                    All_Manager.instance.UI_M.W_Peachone_Deg = 0;
                }
            }

        }

        else
        {
            All_Manager.instance.UI_M.Peachone_Image.gameObject.SetActive(false);
        }

    }
}
