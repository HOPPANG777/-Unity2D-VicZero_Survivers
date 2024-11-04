using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIWeaponForce : MonoBehaviour
{
    private void Update()
    {
        UpdateForceImage();
    }


    public void UpdateForceImage()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer != null)
        {
            var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;
            var WeaponForceSize = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Force).Infos[TempPlayer.ForceLAT.Level];


            if (AllManager.instance.GameModeM.CurrentPlayer.ForceLAT.Level == 0)
            {
                AllManager.instance.UIM.ForceImage.gameObject.SetActive(false);
            }

            else
            {
                AllManager.instance.UIM.ForceImage.gameObject.SetActive(true);

                var TempModule = AllManager.instance.UIM.ForceImage.GetComponent<ParticleSystem>().main;
                TempModule.startSize = TempPlayer.CurrentWeaponSize(WeaponType.Force, TempPlayer.ForceLAT);
           
                AllManager.instance.UIM.ForceImage.rectTransform.position = TempPlayer.transform.position;
            }

        }

        else
        {
            AllManager.instance.UIM.ForceImage.gameObject.SetActive(false);
        }
    }
}
