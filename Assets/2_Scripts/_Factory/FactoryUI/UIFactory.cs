using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : MonoBehaviour
{
    public void Creat_Damage_Effect(Transform p_TF, int p_Number, Transform p_Pool)
    {
        StartCoroutine(C_Create_Damage_Effect(p_TF, p_Number, p_Pool));
    }

    public IEnumerator C_Create_Damage_Effect(Transform p_TF, int p_Number, Transform p_Pool)
    {
        GameObject Damage_Effect = new GameObject("Damage_Effect" + "-" + p_Number.ToString());

        TextMesh Damage_Text = Damage_Effect.AddComponent<TextMesh>();

        Damage_Effect.transform.position = p_TF.transform.position;
        Damage_Effect.transform.rotation = Quaternion.identity;
        Damage_Effect.transform.localScale = Vector3.one;

        if (Damage_Effect.GetComponent<TextMesh>())
        {
            yield return null;

            Damage_Text.fontSize = 32;
            Damage_Text.color = Color.white;
        }

        Damage_Effect.transform.parent = p_Pool;

        Damage_Effect.SetActive(false);
    }



}
