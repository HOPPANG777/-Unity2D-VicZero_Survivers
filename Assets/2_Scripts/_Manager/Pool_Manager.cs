using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Pool_Manager : MonoBehaviour
{
    public bool Ready = false;
    public DamageEffectController Damage_Effect;

    private void Start()
    {
        StartCoroutine(C_Make_Pool());
    }

    public IEnumerator C_Make_Pool()
    {
        Ready = false;

        // i 가 2인 이유는 플레이어가 0, 보스가 1 이기 때문에
        for (int i = 2; i < System.Enum.GetValues(typeof(UnitType)).Length; i++)
        {
            var Temp_UnitType = (UnitType)i;

            GameObject Enemy_Pool = new GameObject(Temp_UnitType.ToString() + "-Pool");
            Enemy_Pool.transform.parent = this.transform;

            yield return new WaitWhile(() => Enemy_Pool == null);

            //50 은 좀 아슬아슬
            int Enemy_Number = 300;

            for (int k = 0; k < Enemy_Number; k++)
            {
                All_Manager.instance.Unit_M.Unit_F.Create_Enemy(All_Manager.instance.Resource_M.Convert_UnitType_To_STO_Unit_Arr(Temp_UnitType), 
                    Vector3.zero, (k + 1), Enemy_Pool.transform);
            }

            yield return new WaitWhile(() => Enemy_Pool.transform.childCount != Enemy_Number);
        }

        for (int j = 0; j < System.Enum.GetValues(typeof(ItemType)).Length; j++)
        {
            var Temp_ItemType = (ItemType)j;

            GameObject Item_Pool = new GameObject(Temp_ItemType.ToString() + "-Pool");
            Item_Pool.transform.parent = this.transform;

            yield return new WaitWhile(() => Item_Pool == null);

            int Item_Number = 1200;

            for (int k = 0; k < Item_Number; k++)
            {
                All_Manager.instance.Unit_M.Item_F.Create_Item(All_Manager.instance.Resource_M.Convert_ItemType_To_STO_Item_Arr(Temp_ItemType), this.transform, (k + 1), Item_Pool.transform);
            }

            yield return new WaitWhile(() => Item_Pool.transform.childCount != Item_Number);
        }

        for (int h = 0; h < System.Enum.GetValues(typeof(WeaponType)).Length; h++)
        {
            var Temp_weaponType = (WeaponType)h;

            GameObject Weapon_Pool = new GameObject(Temp_weaponType.ToString() + "-Pool");
            Weapon_Pool.transform.parent = this.transform;

            yield return new WaitWhile(() => Weapon_Pool == null);

            int Weapon_Number = 100;

            for (int k = 0; k < Weapon_Number; k++)
            {
                All_Manager.instance.Unit_M.Projectile_F.Create_Projectile(All_Manager.instance.Resource_M.Convert_WeaponType_To_STO_Weapon_Arr(Temp_weaponType), this.transform, (k + 1), Weapon_Pool.transform);
            }

            yield return new WaitWhile(() => Weapon_Pool.transform.childCount != Weapon_Number);
        }

        {
            Damage_Effect.DamageEffectPool();
        }

        Ready = true;
    }

    // 오브젝트 풀에 다시 집어넣기
    public void Enemy_Pool_In(Unit_Enemy p_UE)
    {
        var Target_Pool = this.transform.Find(p_UE.UnitType.ToString() + "-Pool");

        p_UE.transform.parent = Target_Pool;

        p_UE.Elite = false;

        p_UE.isDead = false;

        p_UE.isEffect = false;

        p_UE.isDeathAnim = false;

        p_UE.UnitAnimator.SetBool("Death", false);

        p_UE.UnitSpriteRenderer.material.SetFloat("_Outline", 0.0f);

        p_UE.UnitSpriteRenderer.material.SetFloat("_Red_Value", 1.0f);

        p_UE.UnitSpriteRenderer.material.SetFloat("_Alpha", 1.0f);

        p_UE.GetComponent<Unit_Enemy>().UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        p_UE.GetComponent<Unit_Enemy>().UnitSpriteRenderer.size = new Vector2(1, 1);

        p_UE.Dead_Effect = 0.0f;

        p_UE.Hit_Time = 0.0f;

        p_UE.Temp_Time = 1.0f;

        p_UE.Unit_Stat_Setting(p_UE, All_Manager.instance.Resource_M.Convert_UnitType_To_STO_Unit_Arr(p_UE.UnitType));
    }

    // 오브젝트 풀에서 꺼내기
    public void Enemy_Pool_Out(UnitType p_UnitType, Vector3 p_Position, bool p_Elite = false)
    {
        var Target_Pool = this.transform.Find(p_UnitType.ToString() + "-Pool");

        var Unit_Go = Target_Pool.GetChild(0).gameObject;

        Unit_Go.transform.parent = null;

        Unit_Go.transform.position = p_Position;

        Unit_Go.GetComponent<Unit_Enemy>().Elite = p_Elite;

        Unit_Go.SetActive(true);

        Unit_Go.GetComponent<Unit_Enemy>().UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        Unit_Go.GetComponent<Unit_Enemy>().UnitSpriteRenderer.size = new Vector2((Unit_Go.GetComponent<Unit_Enemy>().UnitSpriteRenderer.size.x * 4), (Unit_Go.GetComponent<Unit_Enemy>().UnitSpriteRenderer.size.y * 4));

        Unit_Go.GetComponent<Unit_Enemy>().Elite_Outline_Set(p_UnitType);
        
        Unit_Go.layer = 7;

        All_Manager.instance.GameMode_M.Enemy_List.Add(Unit_Go.GetComponent<Unit_Enemy>());
    }

    public void Item_Pool_In(Item p_Item)
    {
        var Target_Pool = this.transform.Find(p_Item.ItemType.ToString() + "-Pool");

        p_Item.transform.localScale = Vector3.one;

        p_Item.transform.parent = Target_Pool;
    }

    public void Item_Pool_Out(ItemType p_ItemType, Transform p_TF)
    {
        var Target_Pool = this.transform.Find(p_ItemType.ToString() + "-Pool");

        var Item_Go = Target_Pool.GetChild(0).gameObject;

        Item_Go.GetComponent<Item>().ItemSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        Item_Go.GetComponent<Item>().ItemSpriteRenderer.size = new Vector2((0.7f), (0.7f));

        Item_Go.transform.parent = null;

        Item_Go.transform.position = p_TF.transform.position;

        Item_Go.SetActive(true);

        Item_Go.layer = 8;

        All_Manager.instance.GameMode_M.Item_List.Add(Item_Go.GetComponent<Item>());
    }

    public void Weapon_Pool_In(Weapon p_Weapon)
    {
        var Target_Pool = this.transform.Find(p_Weapon.weaponType.ToString() + "-Pool");

        p_Weapon.transform.parent = Target_Pool;

        p_Weapon.transform.position = Target_Pool.position;

        p_Weapon.transform.rotation = Quaternion.identity;

        p_Weapon.transform.localScale = Vector3.one;

        p_Weapon.Collision_Count = 0;

        p_Weapon.Closet_Distance = 0;

        p_Weapon.W_Accumulate = 0;

        p_Weapon.Weapon_Time = 0;

        p_Weapon.gameObject.SetActive(false);
    }

    public GameObject Weapon_Pool_Out(WeaponType p_weaponType)
    {
        var Target_Pool = this.transform.Find(p_weaponType.ToString() + "-Pool");

        var Weapon_Go = Target_Pool.GetChild(0).gameObject;

        Weapon_Go.transform.parent = null;

        Weapon_Go.SetActive(true);

        Weapon_Go.layer = 10;

        return Weapon_Go;
    }


}

#region
// 필요시 새로운 몬스터 생성 (Optional)
//GameObject New_Enemy = Instantiate(Pools[i][0]);
//New_Enemy.SetActive(true);
//Pools[i].Add(New_Enemy);
//return New_Enemy;
//public GameObject[] Object_Go = null; 
//List<GameObject>[] Pools = null;
//private void Awake()
//{
//    Pools = new List<GameObject>[Object_Go.Length];

//    for (int i = 0; i < Pools.Length; i++)
//    {
//        Pools[i] = new List<GameObject>();
//    }
//}

//public GameObject GetPool(int Index)
//{
//    this.transform.Find(UnitType.Enemy.ToString()).GetChild(0); // = Enemy

//    this.transform.Find(UnitType.Golem.ToString()).GetChild(0);

//    this.transform.Find("Enemy1").GetChild(0);

//    GameObject Sel = null;

//    foreach (GameObject Item in Pools[Index])
//    {
//        if (!Item.activeSelf)
//        {
//            Sel = Item;
//            Sel.SetActive(true);
//            break;
//        }

//    }

//    if (!Sel)
//    {
//        Sel = new GameObject(Object_Go[Index].ToString());
//        Sel.transform.parent = this.transform;
//        //Sel = Instantiate(Object_Go[Index], this.transform);
//        Pools[Index].Add(Sel);
//    }

//    return Sel;
//}
#endregion