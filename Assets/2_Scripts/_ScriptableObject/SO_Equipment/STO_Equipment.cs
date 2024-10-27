using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "STO/Equipment")]
public class STO_Equipment : ScriptableObject
{
    public Equipment_Type ET;

    public string Equipment_Name;

    [TextArea]
    public string Equipment_Text;

    public Sprite Sprite_Image;

    public Unit_Stat_Sheet Equipment_Stat_Sheet;
}
