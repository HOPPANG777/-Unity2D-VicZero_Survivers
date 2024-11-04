using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;


public class Item : MonoBehaviour, IItemType
{
    [SerializeField] public ItemType ItemType { get; set; }

    [SerializeField] protected SpriteRenderer itemSpriteRenderer;
    public SpriteRenderer ItemSpriteRenderer { get {  return itemSpriteRenderer; } set { itemSpriteRenderer = value; } }

    public float GemLevel = 4.0f;

    public virtual void Start()
    {
        itemSpriteRenderer = this.AddComponent<SpriteRenderer>();
    }

    public void Update()
    {
        UnitPlayer TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;

        if (TempPlayer == null)
        {
            //Debug.Log("Gem Target Player Null");
            return;
        }

        else
        {
            switch (ItemType)
            {
                case ItemType.Gem:
                    {
                        if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) > TempPlayer.OriginStat.UnitGetGemRange)
                        {

                        }

                        else
                        {
                            if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) < 0.1f)
                            {
                                if (AllManager.instance.GameModeM.GameStop == false)
                                {
                                    TempPlayer.GetGem(GemLevel);
                                    AllManager.instance.AudioM.ItemGetEffectAudioPlay(ItemType.Gem);
                                }

                                this.gameObject.SetActive(false);

                                AllManager.instance.GameModeM.ItemList.Remove(this);

                                AllManager.instance.PoolM.ItemPoolIn(this);
                            }

                            var Dir = (TempPlayer.transform.position - this.transform.position).normalized;

                            this.transform.position += (Dir * 0.05f);

                            if (this.transform.localScale.x > 0.01f)
                            {
                                this.transform.localScale -= Vector3.one * 0.02f;
                            }
                        }
                    }
                    break;
                case ItemType.Chest:
                    {
                        if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) > 0.5f)
                        {

                        }

                        else
                        {
                            if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) < 0.1f)
                            {
                                if (AllManager.instance.GameModeM.GameStop == false)
                                { 
                                    AllManager.instance.UIM.OnPanel(PanelStyle.TreasureFound);
                                    AllManager.instance.AudioM.ItemGetEffectAudioPlay(ItemType.Chest);
                                }

                                this.gameObject.SetActive(false);

                                AllManager.instance.GameModeM.ItemList.Remove(this);

                                AllManager.instance.PoolM.ItemPoolIn(this);
                            }

                            var Dir = (TempPlayer.transform.position - this.transform.position).normalized;

                            this.transform.position += (Dir * 0.05f);

                            if (this.transform.localScale.x > 0.01f)
                            {
                                this.transform.localScale -= Vector3.one * 0.02f;
                            }
                        }
                    }
                    break;
                case ItemType.Food:
                    {
                        if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) > 0.5f)
                        {

                        }

                        else
                        {
                            if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) < 0.1f)
                            {
                                if (AllManager.instance.GameModeM.GameStop == false)
                                {
                                    TempPlayer.RecoveryCurrentHP();
                                    AllManager.instance.AudioM.ItemGetEffectAudioPlay(ItemType.Food);
                                }

                                this.gameObject.SetActive(false);

                                AllManager.instance.GameModeM.ItemList.Remove(this);

                                AllManager.instance.PoolM.ItemPoolIn(this);
                            }

                            var Dir = (TempPlayer.transform.position - this.transform.position).normalized;

                            this.transform.position += (Dir * 0.05f);

                            if (this.transform.localScale.x > 0.01f)
                            {
                                this.transform.localScale -= Vector3.one * 0.02f;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

        }
    }

    public void ItemSetting(Item pItem, ItemData ItemSTO)
    {
        pItem.itemSpriteRenderer.sprite = ItemSTO.SpriteImage;
    }

    public void ItemClear()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            AllManager.instance.GameModeM.ItemList.Remove(this);
        }

    }

}
// 