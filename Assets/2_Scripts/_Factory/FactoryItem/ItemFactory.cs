using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public void CreateItem(ItemData pItemTypeO, Transform pTF, int pNumber, Transform pPool)
    {
        StartCoroutine(CCreateItem(pItemTypeO, pTF, pNumber, pPool));
    }

    public IEnumerator CCreateItem(ItemData pItemTypeO, Transform pTF, int pNumber, Transform pPool)
    {
        GameObject ItemGo = new GameObject(pItemTypeO.ItemType.ToString() + "-" + pNumber.ToString());

        ItemGo.AddComponent<Item>();

        ItemGo.transform.position = pTF.transform.position;
        ItemGo.transform.rotation = Quaternion.identity;
        ItemGo.transform.localScale = Vector3.one;

        if (ItemGo.GetComponent<Item>())
        {
            yield return new WaitUntil(() => ItemGo.GetComponent<Item>().ItemSpriteRenderer);
            ItemGo.GetComponent<Item>().ItemSetting(ItemGo.GetComponent<Item>(), pItemTypeO);

            ItemGo.GetComponent<Item>().ItemType = pItemTypeO.ItemType;
        }

        ItemGo.transform.parent = pPool;

        ItemGo.SetActive(false);
    }

}
