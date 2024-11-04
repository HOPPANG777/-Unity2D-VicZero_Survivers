using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxController : MonoBehaviour
{
    [Header("TreasureBox")]
    public bool TreasureStop = false;
    public bool TreasureOpenChset = false;

    public Vector3[] TreasureGemVecPos = new Vector3[4];

    public float TreasureGemTimer = 0;
    public int TreasureGemTimerPower = 1;

    public float TreasureTimer = 0.0f;

    public int TreasureItemNumber = 0;

    public float MaskRouletteTime = 0.0f;
    public float MaskRoulettePos = 0.0f;

    public void TreasureChestRoulette()
    {
        float TreasureResetPosY = 2300;
        float TreasureImageMove = 1000f;

        if (TreasureTimer > (AllManager.instance.AudioM.TreasureChestAS.clip.length - 2.0f) && !TreasureStop)
        {
            TreasureTimer = 0;

            TreasureStop = true;

            float Dis = 10000;

            for (int i = 0; i < AllManager.instance.UIM.TreasureItemImage01.GetComponent<RectTransform>().transform.childCount; i++)
            {
                if (Vector3.Distance(AllManager.instance.UIM.TreasureItemImage01.GetComponent<RectTransform>().transform.GetChild(i).transform.position, AllManager.instance.UIM.TreasureReelImage.rectTransform.position) < Dis)
                {
                    Dis = Vector3.Distance(AllManager.instance.UIM.TreasureItemImage01.GetComponent<RectTransform>().transform.GetChild(i).transform.position, AllManager.instance.UIM.TreasureReelImage.rectTransform.position);
                    TreasureItemNumber = i;
                }
            }

            for (int j = 0; j < AllManager.instance.UIM.TreasureItemImage02.GetComponent<RectTransform>().transform.childCount; j++)
            {
                if (Vector3.Distance(AllManager.instance.UIM.TreasureItemImage02.GetComponent<RectTransform>().transform.GetChild(j).transform.position, AllManager.instance.UIM.TreasureReelImage.rectTransform.position) < Dis)
                {
                    Dis = Vector3.Distance(AllManager.instance.UIM.TreasureItemImage02.GetComponent<RectTransform>().transform.GetChild(j).transform.position, AllManager.instance.UIM.TreasureReelImage.rectTransform.position);
                    TreasureItemNumber = j;
                }
            }

            TreasureSelectItem(TreasureItemNumber);
            AllManager.instance.UIM.TreasureItemMask.rectTransform.sizeDelta = new Vector2(100, 0);
            AllManager.instance.UIM.TreasureItemMask.rectTransform.anchoredPosition = new Vector2(0, -50f);
            MaskRouletteTime = 0f;
            MaskRoulettePos = 0f;
            AllManager.instance.UIM.TreasureGemImageList.SetActive(false);
        }

        else
        {
            TreasureTimer += Time.deltaTime;
        }

        if (AllManager.instance.UIM.TreasureItemImage01 != null && AllManager.instance.UIM.TreasureItemImage02 != null && TreasureStop == false)
        {
            // 이미지를 위로 이동
            AllManager.instance.UIM.TreasureItemImage01.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (TreasureImageMove * Time.deltaTime);
            AllManager.instance.UIM.TreasureItemImage02.GetComponent<RectTransform>().anchoredPosition += Vector2.up * (TreasureImageMove * Time.deltaTime);

            // 첫 번째 이미지가 기준점을 넘으면 재배치
            if (AllManager.instance.UIM.TreasureItemImage01.GetComponent<RectTransform>().anchoredPosition.y >= TreasureResetPosY)
            {
                RepositionTreasureImage(AllManager.instance.UIM.TreasureItemImage01.GetComponent<RectTransform>());
            }

            // 두 번째 이미지가 기준점을 넘으면 재배치
            if (AllManager.instance.UIM.TreasureItemImage02.GetComponent<RectTransform>().anchoredPosition.y >= TreasureResetPosY)
            {
                RepositionTreasureImage(AllManager.instance.UIM.TreasureItemImage02.GetComponent<RectTransform>());
            }
        }

        else if (TreasureStop == true)
        {
            AllManager.instance.UIM.TreasureCloseButton.gameObject.SetActive(true);
        }

        VerticalTreasureImage();
        TreasureOpenButtonAndFoundTextDisabled();
    }

    public void RepositionTreasureImage(RectTransform Image)
    {
        // 이미지를 아래로 재배치
        float NewY = Image.anchoredPosition.y - 2 * 1900f;
        Image.anchoredPosition = new Vector2(Image.anchoredPosition.x, NewY);
    }

    public void VerticalTreasureImage()
    {
        if (TreasureStop == false)
        {
            MaskRouletteTime += 450f * Time.deltaTime;
            MaskRoulettePos += 185f * Time.deltaTime;

            if (MaskRouletteTime < 590)
            {
                AllManager.instance.UIM.TreasureItemMask.rectTransform.sizeDelta = new Vector2(100, MaskRouletteTime);
            }

            if (MaskRoulettePos < 245)
            {
                AllManager.instance.UIM.TreasureItemMask.rectTransform.anchoredPosition = new Vector2(0, MaskRoulettePos);
            }
        }
    }

    public void TreaureGemActivate()
    {
        var TargetParent = this.transform.Find("TreasureGemList");

        var GemGo = TargetParent.GetChild(0).gameObject;

        GemGo.transform.parent = null;

        GemGo.SetActive(true);
    }

    public void TreasureOpen()
    {
        AllManager.instance.UIM.TreasureChestImage.sprite = AllManager.instance.UIM.OpenChest;
        TreasureOpenChset = true;
        AllManager.instance.UIM.TreasureReelImage.gameObject.SetActive(true);
        AllManager.instance.AudioM.TreasureFoundAudioPlay();
        AllManager.instance.UIM.TreasureGemImageList.SetActive(true);
    }

    public void TreasureClose()
    {
        AllManager.instance.UIM.TreasureChestImage.sprite = AllManager.instance.UIM.CloseChest;
        AllManager.instance.UIM.TreasureCloseButton.gameObject.SetActive(false);
        AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(false);
        TreasureOpenChset = false;
        TreasureItemNumber = 0;
        AllManager.instance.UIM.TreasureReelImage.gameObject.SetActive(false);
        AllManager.instance.UIM.TreasureItemMask.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
        AllManager.instance.UIM.TreasureItemMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50f);
        MaskRouletteTime = 0f;
        MaskRoulettePos = 0f;
        TreasureTimer = 0f;
        TreasureStop = false;
        AllManager.instance.UIM.TreasureFoundText.gameObject.SetActive(true);
        AllManager.instance.UIM.TreasureFoundText.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        AllManager.instance.UIM.TreasureFoundText.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
        AllManager.instance.UIM.TreasureOpenButton.gameObject.SetActive(true);
        AllManager.instance.UIM.TreasureOpenButton.GetComponent<RectTransform>().transform.localScale = Vector3.one;
        AllManager.instance.UIM.TreasureOpenButton.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
        AllManager.instance.UIM.TreasureCloseButton.gameObject.SetActive(false);
        AllManager.instance.UIM.OffPanel();
    }

    public void TreasureSelectItem(int ItemNum)
    {
        if (ItemNum == 0)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.Axe);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Axe).ItemImage;
        }

        if (ItemNum == 1)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.KingBible);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.KingBible).ItemImage;
        }

        if (ItemNum == 2)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.Peachone);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Peachone).ItemImage;
        }

        if (ItemNum == 3)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.FireWand);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.FireWand).ItemImage;
        }

        if (ItemNum == 4)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.Banana);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Banana).ItemImage;
        }

        if (ItemNum == 5)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.Force);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Force).ItemImage;
        }

        if (ItemNum == 6)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.LightningRing);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.LightningRing).ItemImage;
        }

        if (ItemNum == 7)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.ShieldChasing);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.ShieldChasing).ItemImage;
        }

        if (ItemNum == 8)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.RuneTracer);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.RuneTracer).ItemImage;
        }

        if (ItemNum == 9)
        {
            AllManager.instance.UIM.PlayerLevelUp.WeaponSelectAssignButton(WeaponType.Nunchaku);
           AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
           AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Nunchaku).ItemImage;
        }

        if (ItemNum == 10)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.Bang);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Bang).SpriteImage;
        }

        if (ItemNum == 11)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.Bracer);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Bracer).SpriteImage;
        }

        if (ItemNum == 12)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.Times);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Times).SpriteImage;
        }

        if (ItemNum == 13)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.EmptyTome);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.EmptyTome).SpriteImage;
        }

        if (ItemNum == 14)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.Thunder);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Thunder).SpriteImage;
        }

        if (ItemNum == 15)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.TheFlags);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.TheFlags).SpriteImage;
        }

        if (ItemNum == 16)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.MagicHood);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.MagicHood).SpriteImage;
        }

        if (ItemNum == 17)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.RedAja);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.RedAja).SpriteImage;
        }

        if (ItemNum == 18)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.FloppyDisk);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.FloppyDisk).SpriteImage;
        }

        if (ItemNum == 19)
        {
            AllManager.instance.UIM.PlayerLevelUp.AccessorySelectAssignButton(AccessoryType.Attractorb);
            AllManager.instance.UIM.TreasureSelectItemImage.gameObject.SetActive(true);
            AllManager.instance.UIM.TreasureSelectItemImage.sprite = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Attractorb).SpriteImage;
        }
    }

    public void TreasureOpenButtonAndFoundTextDisabled()
    {
        if (TreasureOpenChset == true)
        {
            if (AllManager.instance.UIM.TreasureFoundText.GetComponent<RectTransform>().transform.localScale.x > 0.01f && AllManager.instance.UIM.TreasureOpenButton.GetComponent<RectTransform>().transform.localScale.x > 0.01f)
            {
                AllManager.instance.UIM.TreasureFoundText.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                AllManager.instance.UIM.TreasureOpenButton.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                AllManager.instance.UIM.TreasureOpenButton.GetComponent<RectTransform>().transform.Rotate(0, 0, 100f * Time.deltaTime);
            }
            else
            {
                AllManager.instance.UIM.TreasureFoundText.gameObject.SetActive(false);
                AllManager.instance.UIM.TreasureOpenButton.gameObject.SetActive(false);
            }
        }

        else
        {

        }

    }

}
