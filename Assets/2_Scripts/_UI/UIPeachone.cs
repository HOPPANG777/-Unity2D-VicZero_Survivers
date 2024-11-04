using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPeachone : MonoBehaviour
{
    // Peachone무기 공격범위
    public void UpdatePeachoneRangeImage()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer != null)
        {
            var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;
            var WUIPeachoneRangeS = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Peachone).Infos[TempPlayer.PeachoneLAT.Level];

            if (AllManager.instance.GameModeM.CurrentPlayer.PeachoneLAT.Level == 0)
            {
                AllManager.instance.UIM.PeachoneRangeImage.gameObject.SetActive(false);
            }

            else
            {
                AllManager.instance.UIM.PeachoneRangeImage.gameObject.SetActive(true);

                AllManager.instance.UIM.PeachoneRangeImage.rectTransform.sizeDelta = new Vector2(TempPlayer.CurrentWeaponRange(WeaponType.Peachone, TempPlayer.PeachoneLAT), TempPlayer.CurrentWeaponRange(WeaponType.Peachone, TempPlayer.PeachoneLAT)) * 0.5f;

                AllManager.instance.UIM.WPeachoneRangeDeg += Time.deltaTime * WUIPeachoneRangeS.WeaponSpeed * 20.0f;
                if (AllManager.instance.UIM.WPeachoneRangeDeg < 360)
                {
                    var rad = Mathf.Deg2Rad * (AllManager.instance.UIM.WPeachoneRangeDeg);
                    var x = (TempPlayer.CurrentWeaponRange(WeaponType.Peachone, TempPlayer.PeachoneLAT) * 0.4f) * Mathf.Sin(rad);
                    var y = (TempPlayer.CurrentWeaponRange(WeaponType.Peachone, TempPlayer.PeachoneLAT) * 0.4f) * Mathf.Cos(rad);
                    AllManager.instance.UIM.PeachoneRangeImage.rectTransform.position = TempPlayer.transform.position + new Vector3(x, y);
                }
                else
                {
                    AllManager.instance.UIM.WPeachoneRangeDeg = 0;
                }
            }

        }

        else
        {
            AllManager.instance.UIM.PeachoneRangeImage.gameObject.SetActive(false);
        }
    }

    // Peachone무기 이미지
    public void UpdatePeachoneImage()
    {
        if (AllManager.instance.GameModeM.CurrentPlayer != null)
        {
            var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;

            if (AllManager.instance.GameModeM.CurrentPlayer.PeachoneLAT.Level == 0)
            {
                AllManager.instance.UIM.PeachoneImage.gameObject.SetActive(false);
            }

            else
            {
                AllManager.instance.UIM.PeachoneImage.gameObject.SetActive(true);

                AllManager.instance.UIM.WPeachoneDeg += Time.deltaTime * 10f;
                if (AllManager.instance.UIM.WPeachoneDeg < 360)
                {
                    var rad = Mathf.Deg2Rad * (AllManager.instance.UIM.WPeachoneDeg);
                    var x = 0.7f * Mathf.Sin(rad);
                    var y = 0.7f * Mathf.Cos(rad);
                    AllManager.instance.UIM.PeachoneImage.rectTransform.position = TempPlayer.transform.position + new Vector3(x, y);

                }
                else
                {
                    AllManager.instance.UIM.WPeachoneDeg = 0;
                }
            }

        }

        else
        {
            AllManager.instance.UIM.PeachoneImage.gameObject.SetActive(false);
        }

    }
}
