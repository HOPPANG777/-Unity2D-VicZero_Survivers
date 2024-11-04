using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureNumber : MonoBehaviour
{
    public int TreasureNum = 0;

    void Start()
    {
        for (int i = 0; i < this.transform.parent.childCount; i++) if (this.transform.parent.GetChild(i) == this.transform) TreasureNum = i;
    }
}