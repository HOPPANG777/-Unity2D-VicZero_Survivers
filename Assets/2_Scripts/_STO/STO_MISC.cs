using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STO/MISC")]

public class STO_MISC : ScriptableObject
{
    public MISC_Type MT;

    public string MISC_Name;

    [TextArea]
    public string MISC_Text;

    public Sprite Sprite_Image;
    public AudioClip Get_MISC_Audio_Clip;
}
