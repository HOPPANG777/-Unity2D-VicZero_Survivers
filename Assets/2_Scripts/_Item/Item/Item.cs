using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;


public class Item : MonoBehaviour, IItemType
{
    [SerializeField] public ItemType ItemType { get; set; }

    [SerializeField] protected SpriteRenderer _itemSpriteRenderer;
    public SpriteRenderer ItemSpriteRenderer { get {  return _itemSpriteRenderer; } set { _itemSpriteRenderer = value; } }

    public float Gem_Level = 4.0f;

    public virtual void Start()
    {
        _itemSpriteRenderer = this.AddComponent<SpriteRenderer>();
    }

    public void Update()
    {
        Unit_Player Temp_Player = All_Manager.instance.GameMode_M.Current_Player;

        if (Temp_Player == null)
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
                        if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) > Temp_Player.Origin_Stat.Unit_Get_Gem_Range)
                        {

                        }

                        else
                        {
                            if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) < 0.1f)
                            {
                                if (All_Manager.instance.GameMode_M.Game_Stop == false)
                                {
                                    Temp_Player.Get_Gem(Gem_Level);
                                    All_Manager.instance.Audio_M.Item_Get_Effect_Audio_Play(ItemType.Gem);
                                }

                                this.gameObject.SetActive(false);

                                All_Manager.instance.GameMode_M.Item_List.Remove(this);

                                All_Manager.instance.Pool_M.Item_Pool_In(this);
                            }

                            var Dir = (Temp_Player.transform.position - this.transform.position).normalized;

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
                        if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) > 0.5f)
                        {

                        }

                        else
                        {
                            if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) < 0.1f)
                            {
                                if (All_Manager.instance.GameMode_M.Game_Stop == false)
                                { 
                                    All_Manager.instance.UI_M.On_Panel(Panel_Style.TreasureFound);
                                    All_Manager.instance.Audio_M.Item_Get_Effect_Audio_Play(ItemType.Chest);
                                }

                                this.gameObject.SetActive(false);

                                All_Manager.instance.GameMode_M.Item_List.Remove(this);

                                All_Manager.instance.Pool_M.Item_Pool_In(this);
                            }

                            var Dir = (Temp_Player.transform.position - this.transform.position).normalized;

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
                        if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) > 0.5f)
                        {

                        }

                        else
                        {
                            if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) < 0.1f)
                            {
                                if (All_Manager.instance.GameMode_M.Game_Stop == false)
                                {
                                    Temp_Player.Recovery_Current_HP();
                                    All_Manager.instance.Audio_M.Item_Get_Effect_Audio_Play(ItemType.Food);
                                }

                                this.gameObject.SetActive(false);

                                All_Manager.instance.GameMode_M.Item_List.Remove(this);

                                All_Manager.instance.Pool_M.Item_Pool_In(this);
                            }

                            var Dir = (Temp_Player.transform.position - this.transform.position).normalized;

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

    public void Item_Setting(Item p_Item, STO_Item Item_STO)
    {
        p_Item._itemSpriteRenderer.sprite = Item_STO.Sprite_Image;
    }

    public void Item_Clear()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            All_Manager.instance.GameMode_M.Item_List.Remove(this);
        }

    }

}
// 