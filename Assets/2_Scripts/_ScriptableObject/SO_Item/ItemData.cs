using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STO/Item")]

public class ItemData : ScriptableObject
{
    public ItemType ItemType;

    public string ItemName;

    [TextArea]
    public string ItemText;

    public Sprite SpriteImage;
    public AudioClip GetItemAudioClip;
}
