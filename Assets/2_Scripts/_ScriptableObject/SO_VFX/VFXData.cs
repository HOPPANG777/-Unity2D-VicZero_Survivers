using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STO/VFX")]

public class VFXData : ScriptableObject
{
    public VFXType VT;

    public Sprite SpriteImage;
    public Material VFXMaterial;
}
