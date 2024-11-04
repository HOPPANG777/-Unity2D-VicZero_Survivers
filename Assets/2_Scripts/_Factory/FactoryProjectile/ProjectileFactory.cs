using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public void Create_Projectile(STO_Weapon p_weaponTypeO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        StartCoroutine(C_Create_Projectile(p_weaponTypeO, p_TF, p_Number, p_Pool));
    }

    public IEnumerator C_Create_Projectile(STO_Weapon p_weaponTypeO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        GameObject Weapon_Go = new GameObject(p_weaponTypeO.weaponType.ToString() + "-" + p_Number.ToString());

        switch (p_weaponTypeO.weaponType)
        {
            case WeaponType.Axe:
                Weapon_Go.AddComponent<WeaponAxe>();
                break;
            case WeaponType.KingBible:
                Weapon_Go.AddComponent<WeaponKingBible>();
                break;
            case WeaponType.Peachone:
                Weapon_Go.AddComponent<WeaponPeachone>();
                break;
            case WeaponType.FireWand:
                Weapon_Go.AddComponent<WeaponFireWand>();
                break;
            case WeaponType.Banana:
                Weapon_Go.AddComponent<WeaponBanana>();
                break;
            case WeaponType.Force:
                Weapon_Go.AddComponent<WeaponForce>();
                break;
            case WeaponType.LightningRing:
                Weapon_Go.AddComponent<WeaponLightningRing>();
                break;
            case WeaponType.ShieldChasing:
                Weapon_Go.AddComponent<WeaponShieldChasing>();
                break;
            case WeaponType.RuneTracer:
                Weapon_Go.AddComponent<WeaponRuneTracer>();
                break;
            case WeaponType.Nunchaku:
                Weapon_Go.AddComponent<WeaponNunchaku>();
                break;
        }

        //Weapon_Go.AddComponent<Weapon>();

        Weapon_Go.transform.position = p_TF.transform.position;
        Weapon_Go.transform.rotation = Quaternion.identity;
        Weapon_Go.transform.localScale = Vector3.one;

        yield return null;

        Weapon_Go.transform.parent = p_Pool;

        Weapon_Go.SetActive(false);
    }
}
