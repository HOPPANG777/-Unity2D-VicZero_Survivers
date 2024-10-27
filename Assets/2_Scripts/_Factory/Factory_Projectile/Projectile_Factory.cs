using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Factory : MonoBehaviour
{
    //public void Create_Projectile(STO_Weapon p_WTO, Transform p_TF, int p_Number, Transform p_Pool)
    //{
    //    StartCoroutine(C_Create_Projectile(p_WTO, p_TF, p_Number, p_Pool));
    //}

    //public IEnumerator C_Create_Projectile(STO_Weapon p_WTO, Transform p_TF, int p_Number, Transform p_Pool)
    //{
    //    GameObject Weapon_Go = new GameObject(p_WTO.WT.ToString() + "-" + p_Number.ToString());

    //    Weapon_Go.AddComponent<Weapon>();

    //    Weapon_Go.transform.position = p_TF.transform.position;
    //    Weapon_Go.transform.rotation = Quaternion.identity;
    //    Weapon_Go.transform.localScale = Vector3.one;

    //    if (Weapon_Go.GetComponent<Weapon>())
    //    {
    //        yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon>().SR);
    //        Weapon_Go.GetComponent<Weapon>().Weapon_Stat(Weapon_Go.GetComponent<Weapon>(), p_WTO);

    //        Weapon_Go.GetComponent<Weapon>().SR.drawMode = SpriteDrawMode.Sliced;
    //        Weapon_Go.GetComponent<Weapon>().SR.size = new Vector2((Weapon_Go.GetComponent<Weapon>().SR.size.x * 2), (Weapon_Go.GetComponent<Weapon>().SR.size.y * 2));

    //        Weapon_Go.GetComponent<Weapon>().WT = p_WTO.WT;
    //    }

    //    Weapon_Go.transform.parent = p_Pool;

    //    Weapon_Go.SetActive(false);
    //}

    //public IEnumerator C_Create_Weapon(Weapon_Type p_WT)
    //{
    //    yield return new WaitWhile(() => Creating_Weapon == true);

    //    var Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);
    //    var Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

    //    Creating_Weapon = true;

    //    // 무기 풀에서 무기를 가져옵니다.
    //    GameObject Weapon_Go = WeaponPool.instance.GetWeapon(p_WT);

    //    // 만약 풀에서 무기를 가져오지 못한 경우 새로 생성하고 풀에 추가
    //    if (Weapon_Go == null)
    //    {
    //        Weapon_Go = new GameObject(p_WT.ToString());
    //        switch (p_WT)
    //        {
    //            case Weapon_Type.Axe:
    //                Weapon_Go.AddComponent<Weapon_Axe>();
    //                break;
    //            case Weapon_Type.KingBible:
    //                Weapon_Go.AddComponent<Weapon_KingBible>();
    //                break;
    //                // 각 무기 타입에 맞는 컴포넌트 추가
    //        }
    //        WeaponPool.instance.InitializePool(p_WT, Weapon_Go, 10);
    //    }

    //    if (Weapon_Go != null)
    //    {
    //        // 무기 초기화
    //        Weapon_Go.transform.position = this.transform.position;
    //        Weapon_Go.transform.rotation = Quaternion.identity;
    //        Weapon_Go.transform.localScale = Vector3.one;

    //        // 무기별 초기화 로직
    //        if (Weapon_Go.TryGetComponent<Weapon>(out var weaponComponent))
    //        {
    //            weaponComponent.Weapon_Init(this, Weapon_STO, Weapon_WLAT.Level);
    //        }

    //        // 특정 무기 타입에 대한 추가 초기화
    //        if (Weapon_Go.TryGetComponent<Weapon_Axe>(out var axe))
    //        {
    //            // 도끼 초기화 코드 추가
    //        }

    //        All_Weapon.Add(Weapon_Go.GetComponent<Weapon>());
    //    }

    //    Creating_Weapon = false;
    //}
}
