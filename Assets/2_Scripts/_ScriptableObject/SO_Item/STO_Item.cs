using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STO/Item")]

public class STO_Item : ScriptableObject
{
    public ItemType ItemType;

    public string Item_Name;

    [TextArea]
    public string Item_Text;

    public Sprite Sprite_Image;
    public AudioClip Get_Item_Audio_Clip;
}
