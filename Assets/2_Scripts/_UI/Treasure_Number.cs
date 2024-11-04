using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Treasure_Number : MonoBehaviour
{
    public int Treasure_Num = 0;

    void Start()
    {
        for (int i = 0; i < this.transform.parent.childCount; i++) if (this.transform.parent.GetChild(i) == this.transform) Treasure_Num = i;
    }
}