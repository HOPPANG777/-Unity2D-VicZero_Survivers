using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "STO/Accessory")]
public class STO_Accessory : ScriptableObject
{
    public AccessoryType ET;

    public string Accessory_Name;

    [TextArea]
    public string Accessory_Text;

    public Sprite Sprite_Image;

    public Unit_Stat_Sheet Accessory_Stat_Sheet;
}
