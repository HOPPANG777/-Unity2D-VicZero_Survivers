using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public void Create_Item(STO_Item p_ItemTypeO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        StartCoroutine(C_Create_Item(p_ItemTypeO, p_TF, p_Number, p_Pool));
    }

    public IEnumerator C_Create_Item(STO_Item p_ItemTypeO, Transform p_TF, int p_Number, Transform p_Pool)
    {
        GameObject Item_Go = new GameObject(p_ItemTypeO.ItemType.ToString() + "-" + p_Number.ToString());

        Item_Go.AddComponent<Item>();

        Item_Go.transform.position = p_TF.transform.position;
        Item_Go.transform.rotation = Quaternion.identity;
        Item_Go.transform.localScale = Vector3.one;

        if (Item_Go.GetComponent<Item>())
        {
            yield return new WaitUntil(() => Item_Go.GetComponent<Item>().ItemSpriteRenderer);
            Item_Go.GetComponent<Item>().Item_Setting(Item_Go.GetComponent<Item>(), p_ItemTypeO);

            Item_Go.GetComponent<Item>().ItemType = p_ItemTypeO.ItemType;
        }

        Item_Go.transform.parent = p_Pool;

        Item_Go.SetActive(false);
    }

}
