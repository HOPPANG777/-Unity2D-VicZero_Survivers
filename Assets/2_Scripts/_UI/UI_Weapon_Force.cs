using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Weapon_Force : MonoBehaviour
{
    private void Update()
    {
        Update_Force_Image();
    }


    public void Update_Force_Image()
    {
        if (All_Manager.instance.GameMode_M.Current_Player != null)
        {
            var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
            var Weapon_Force_Size = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Garlic).Info_s[Temp_Player.Garlic_LAT.Level];


            if (All_Manager.instance.GameMode_M.Current_Player.Garlic_LAT.Level == 0)
            {
                All_Manager.instance.UI_M.Force_Image.gameObject.SetActive(false);
            }

            else
            {
                All_Manager.instance.UI_M.Force_Image.gameObject.SetActive(true);

                var Temp_Module = All_Manager.instance.UI_M.Force_Image.GetComponent<ParticleSystem>().main;
                Temp_Module.startSize = Temp_Player.Current_Weapon_Size(Weapon_Type.Garlic, Temp_Player.Garlic_LAT);
           
                All_Manager.instance.UI_M.Force_Image.rectTransform.position = Temp_Player.transform.position;
            }

        }

        else
        {
            All_Manager.instance.UI_M.Force_Image.gameObject.SetActive(false);
        }
    }
}
