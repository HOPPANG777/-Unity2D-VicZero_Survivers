using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : MonoBehaviour
{
    public void CreatDamageEffect(Transform pTF, int pNumber, Transform pPool)
    {
        StartCoroutine(CCreateDamageEffect(pTF, pNumber, pPool));
    }

    public IEnumerator CCreateDamageEffect(Transform pTF, int pNumber, Transform pPool)
    {
        GameObject DamageEffect = new GameObject("DamageEffect" + "-" + pNumber.ToString());

        TextMesh DamageText = DamageEffect.AddComponent<TextMesh>();

        DamageEffect.transform.position = pTF.transform.position;
        DamageEffect.transform.rotation = Quaternion.identity;
        DamageEffect.transform.localScale = Vector3.one;

        if (DamageEffect.GetComponent<TextMesh>())
        {
            yield return null;

            DamageText.fontSize = 32;
            DamageText.color = Color.white;
        }

        DamageEffect.transform.parent = pPool;

        DamageEffect.SetActive(false);
    }



}
