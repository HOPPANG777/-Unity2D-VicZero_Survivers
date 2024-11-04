using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    public bool Ready = false;
    public DamageEffectController DamageEffect;

    private void Start()
    {
        StartCoroutine(CMakePool());
    }

    public IEnumerator CMakePool()
    {
        Ready = false;

        // i 가 2인 이유는 플레이어가 0, 보스가 1 이기 때문에
        for (int i = 2; i < System.Enum.GetValues(typeof(UnitType)).Length; i++)
        {
            var TempUnitType = (UnitType)i;

            GameObject EnemyPool = new GameObject(TempUnitType.ToString() + "-Pool");
            EnemyPool.transform.parent = this.transform;

            yield return new WaitWhile(() => EnemyPool == null);

            //50 은 좀 아슬아슬
            int EnemyNumber = 300;

            for (int k = 0; k < EnemyNumber; k++)
            {
                AllManager.instance.UnitM.UnitF.CreateEnemy(AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(TempUnitType), 
                    Vector3.zero, (k + 1), EnemyPool.transform);
            }

            yield return new WaitWhile(() => EnemyPool.transform.childCount != EnemyNumber);
        }

        for (int j = 0; j < System.Enum.GetValues(typeof(ItemType)).Length; j++)
        {
            var TempItemType = (ItemType)j;

            GameObject ItemPool = new GameObject(TempItemType.ToString() + "-Pool");
            ItemPool.transform.parent = this.transform;

            yield return new WaitWhile(() => ItemPool == null);

            int ItemNumber = 1200;

            for (int k = 0; k < ItemNumber; k++)
            {
                AllManager.instance.UnitM.ItemF.CreateItem(AllManager.instance.ResourceM.ConvertItemTypeToItemDataArr(TempItemType), this.transform, (k + 1), ItemPool.transform);
            }

            yield return new WaitWhile(() => ItemPool.transform.childCount != ItemNumber);
        }

        for (int h = 0; h < System.Enum.GetValues(typeof(WeaponType)).Length; h++)
        {
            var TempweaponType = (WeaponType)h;

            GameObject WeaponPool = new GameObject(TempweaponType.ToString() + "-Pool");
            WeaponPool.transform.parent = this.transform;

            yield return new WaitWhile(() => WeaponPool == null);

            int WeaponNumber = 100;

            for (int k = 0; k < WeaponNumber; k++)
            {
                AllManager.instance.UnitM.ProjectileF.CreateProjectile(AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(TempweaponType), this.transform, (k + 1), WeaponPool.transform);
            }

            yield return new WaitWhile(() => WeaponPool.transform.childCount != WeaponNumber);
        }

        {
            DamageEffect.DamageEffectPool();
        }

        Ready = true;
    }

    // 오브젝트 풀에 다시 집어넣기
    public void EnemyPoolIn(UnitEnemy pUE)
    {
        var TargetPool = this.transform.Find(pUE.UnitType.ToString() + "-Pool");

        pUE.transform.parent = TargetPool;

        pUE.Elite = false;

        pUE.isDead = false;

        pUE.isEffect = false;

        pUE.isDeathAnim = false;

        pUE.UnitAnimator.SetBool("Death", false);

        pUE.UnitSpriteRenderer.material.SetFloat("Outline", 0.0f);

        pUE.UnitSpriteRenderer.material.SetFloat("RedValue", 1.0f);

        pUE.UnitSpriteRenderer.material.SetFloat("Alpha", 1.0f);

        pUE.GetComponent<UnitEnemy>().UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        pUE.GetComponent<UnitEnemy>().UnitSpriteRenderer.size = new Vector2(1, 1);

        pUE.DeadEffect = 0.0f;

        pUE.HitTime = 0.0f;

        pUE.TempTime = 1.0f;

        pUE.UnitStatSetting(pUE, AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(pUE.UnitType));
    }

    // 오브젝트 풀에서 꺼내기
    public void EnemyPoolOut(UnitType pUnitType, Vector3 pPosition, bool pElite = false)
    {
        var TargetPool = this.transform.Find(pUnitType.ToString() + "-Pool");

        var UnitGo = TargetPool.GetChild(0).gameObject;

        UnitGo.transform.parent = null;

        UnitGo.transform.position = pPosition;

        UnitGo.GetComponent<UnitEnemy>().Elite = pElite;

        UnitGo.SetActive(true);

        UnitGo.GetComponent<UnitEnemy>().UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        UnitGo.GetComponent<UnitEnemy>().UnitSpriteRenderer.size = new Vector2((UnitGo.GetComponent<UnitEnemy>().UnitSpriteRenderer.size.x * 4), (UnitGo.GetComponent<UnitEnemy>().UnitSpriteRenderer.size.y * 4));

        UnitGo.GetComponent<UnitEnemy>().EliteOutlineSet(pUnitType);
        
        UnitGo.layer = 7;

        AllManager.instance.GameModeM.EnemyList.Add(UnitGo.GetComponent<UnitEnemy>());
    }

    public void ItemPoolIn(Item pItem)
    {
        var TargetPool = this.transform.Find(pItem.ItemType.ToString() + "-Pool");

        pItem.transform.localScale = Vector3.one;

        pItem.transform.parent = TargetPool;
    }

    public void ItemPoolOut(ItemType pItemType, Transform pTF)
    {
        var TargetPool = this.transform.Find(pItemType.ToString() + "-Pool");

        var ItemGo = TargetPool.GetChild(0).gameObject;

        ItemGo.GetComponent<Item>().ItemSpriteRenderer.drawMode = SpriteDrawMode.Sliced;

        ItemGo.GetComponent<Item>().ItemSpriteRenderer.size = new Vector2((0.7f), (0.7f));

        ItemGo.transform.parent = null;

        ItemGo.transform.position = pTF.transform.position;

        ItemGo.SetActive(true);

        ItemGo.layer = 8;

        AllManager.instance.GameModeM.ItemList.Add(ItemGo.GetComponent<Item>());
    }

    public void WeaponPoolIn(Weapon pWeapon)
    {
        var TargetPool = this.transform.Find(pWeapon.weaponType.ToString() + "-Pool");

        pWeapon.transform.parent = TargetPool;

        pWeapon.transform.position = TargetPool.position;

        pWeapon.transform.rotation = Quaternion.identity;

        pWeapon.transform.localScale = Vector3.one;

        pWeapon.CollisionCount = 0;

        pWeapon.ClosetDistance = 0;

        pWeapon.WAccumulate = 0;

        pWeapon.WeaponTime = 0;

        pWeapon.gameObject.SetActive(false);
    }

    public GameObject WeaponPoolOut(WeaponType pweaponType)
    {
        var TargetPool = this.transform.Find(pweaponType.ToString() + "-Pool");

        var WeaponGo = TargetPool.GetChild(0).gameObject;

        WeaponGo.transform.parent = null;

        WeaponGo.SetActive(true);

        WeaponGo.layer = 10;

        return WeaponGo;
    }


}

#region
// 필요시 새로운 몬스터 생성 (Optional)
//GameObject NewEnemy = Instantiate(Pools[i][0]);
//NewEnemy.SetActive(true);
//Pools[i].Add(NewEnemy);
//return NewEnemy;
//public GameObject[] ObjectGo = null; 
//List<GameObject>[] Pools = null;
//private void Awake()
//{
//    Pools = new List<GameObject>[ObjectGo.Length];

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
//        Sel = new GameObject(ObjectGo[Index].ToString());
//        Sel.transform.parent = this.transform;
//        //Sel = Instantiate(ObjectGo[Index], this.transform);
//        Pools[Index].Add(Sel);
//    }

//    return Sel;
//}
#endregion