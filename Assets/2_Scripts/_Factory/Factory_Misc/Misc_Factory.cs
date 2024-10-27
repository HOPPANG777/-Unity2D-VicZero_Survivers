using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc_Factory : MonoBehaviour
{
    public void Create_MISC(STO_MISC p_MTO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        StartCoroutine(C_Create_MISC(p_MTO, p_TF, p_Number, p_Pool));
    }

    public IEnumerator C_Create_MISC(STO_MISC p_MTO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        GameObject MISC_Go = new GameObject(p_MTO.MT.ToString() + "-" + p_Number.ToString());

        MISC_Go.AddComponent<MISC>();

        MISC_Go.transform.position = p_TF.transform.position;
        MISC_Go.transform.rotation = Quaternion.identity;
        MISC_Go.transform.localScale = Vector3.one;

        if (MISC_Go.GetComponent<MISC>())
        {
            yield return new WaitUntil(() => MISC_Go.GetComponent<MISC>().SR);
            MISC_Go.GetComponent<MISC>().MISC_Setting(MISC_Go.GetComponent<MISC>(), p_MTO);

            MISC_Go.GetComponent<MISC>().MT = p_MTO.MT;
        }

        MISC_Go.transform.parent = p_Pool;

        MISC_Go.SetActive(false);
    }

}
