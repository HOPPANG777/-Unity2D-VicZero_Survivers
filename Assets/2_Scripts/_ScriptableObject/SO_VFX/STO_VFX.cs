using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STO/VFX")]

public class STO_VFX : ScriptableObject
{
    public VFX_Type VT;

    public Sprite Sprite_Image;
    public Material VFX_Material;
}
