using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemType
{
    ItemType ItemType { get; set; }
}

public enum ItemType
{
    Gem,
    Chest,
    Food
}
