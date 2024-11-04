using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Example_Script : MonoBehaviour
{
    public int Number = 0;

    public void Start()
    {
        //this.GetComponent<Image>().color = new UnityEngine.Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

        for (int i = 0; i < this.transform.parent.childCount; i++) if (this.transform.parent.GetChild(i) == this.transform) Number = i;
    }
}
