using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "STO/Accessory")]
public class AccessoryData : ScriptableObject
{
    public AccessoryType ET;

    public string AccessoryName;

    [TextArea]
    public string AccessoryText;

    public Sprite SpriteImage;

    public UnitStatSheet AccessoryStatSheet;
}
