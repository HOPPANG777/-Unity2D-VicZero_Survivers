using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public void CreateProjectile(WeaponData pweaponTypeO, Transform pTF, int pNumber, Transform pPool)
    {
        StartCoroutine(CCreateProjectile(pweaponTypeO, pTF, pNumber, pPool));
    }

    public IEnumerator CCreateProjectile(WeaponData pweaponTypeO, Transform pTF, int pNumber, Transform pPool)
    {
        GameObject WeaponGo = new GameObject(pweaponTypeO.weaponType.ToString() + "-" + pNumber.ToString());

        switch (pweaponTypeO.weaponType)
        {
            case WeaponType.Axe:
                WeaponGo.AddComponent<WeaponAxe>();
                break;
            case WeaponType.KingBible:
                WeaponGo.AddComponent<WeaponKingBible>();
                break;
            case WeaponType.Peachone:
                WeaponGo.AddComponent<WeaponPeachone>();
                break;
            case WeaponType.FireWand:
                WeaponGo.AddComponent<WeaponFireWand>();
                break;
            case WeaponType.Banana:
                WeaponGo.AddComponent<WeaponBanana>();
                break;
            case WeaponType.Force:
                WeaponGo.AddComponent<WeaponForce>();
                break;
            case WeaponType.LightningRing:
                WeaponGo.AddComponent<WeaponLightningRing>();
                break;
            case WeaponType.ShieldChasing:
                WeaponGo.AddComponent<WeaponShieldChasing>();
                break;
            case WeaponType.RuneTracer:
                WeaponGo.AddComponent<WeaponRuneTracer>();
                break;
            case WeaponType.Nunchaku:
                WeaponGo.AddComponent<WeaponNunchaku>();
                break;
        }

        //WeaponGo.AddComponent<Weapon>();

        WeaponGo.transform.position = pTF.transform.position;
        WeaponGo.transform.rotation = Quaternion.identity;
        WeaponGo.transform.localScale = Vector3.one;

        yield return null;

        WeaponGo.transform.parent = pPool;

        WeaponGo.SetActive(false);
    }
}
