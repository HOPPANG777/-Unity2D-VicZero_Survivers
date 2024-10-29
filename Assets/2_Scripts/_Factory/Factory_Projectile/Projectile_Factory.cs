using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Factory : MonoBehaviour
{
    public void Create_Projectile(STO_Weapon p_WTO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        StartCoroutine(C_Create_Projectile(p_WTO, p_TF, p_Number, p_Pool));
    }

    public IEnumerator C_Create_Projectile(STO_Weapon p_WTO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        GameObject Weapon_Go = new GameObject(p_WTO.WT.ToString() + "-" + p_Number.ToString());

        switch (p_WTO.WT)
        {
            case Weapon_Type.Axe:
                Weapon_Go.AddComponent<Weapon_Axe>();
                break;
            case Weapon_Type.KingBible:
                Weapon_Go.AddComponent<Weapon_KingBible>();
                break;
            case Weapon_Type.Peachone:
                Weapon_Go.AddComponent<Weapon_Peachone>();
                break;
            case Weapon_Type.FireWand:
                Weapon_Go.AddComponent<Weapon_FireWand>();
                break;
            case Weapon_Type.Cross:
                Weapon_Go.AddComponent<Weapon_Cross>();
                break;
            case Weapon_Type.Garlic:
                Weapon_Go.AddComponent<Weapon_Garlic>();
                break;
            case Weapon_Type.LightningRing:
                Weapon_Go.AddComponent<Weapon_LightningRing>();
                break;
            case Weapon_Type.ShadowPinion:
                Weapon_Go.AddComponent<Weapon_ShadowPinion>();
                break;
            case Weapon_Type.RuneTracer:
                Weapon_Go.AddComponent<Weapon_RuneTracer>();
                break;
            case Weapon_Type.Whip:
                Weapon_Go.AddComponent<Weapon_Whip>();
                break;
        }

        //Weapon_Go.AddComponent<Weapon>();

        Weapon_Go.transform.position = p_TF.transform.position;
        Weapon_Go.transform.rotation = Quaternion.identity;
        Weapon_Go.transform.localScale = Vector3.one;

        yield return null;

        //if (Weapon_Go.GetComponent<Weapon>())
        //{
        //    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon>().SR);

        //    //Weapon_Go.GetComponent<Weapon>().SR.drawMode = SpriteDrawMode.Sliced;
        //    //Weapon_Go.GetComponent<Weapon>().SR.size = new Vector2((Weapon_Go.GetComponent<Weapon>().SR.size.x * 2), (Weapon_Go.GetComponent<Weapon>().SR.size.y * 2));

        //    Weapon_Go.GetComponent<Weapon>().WT = p_WTO.WT;
        //}

        Weapon_Go.transform.parent = p_Pool;

        Weapon_Go.SetActive(false);
    }
}
