using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public enum MISC_Type
{
    Gem,
    Chest,
    Food
}

public class MISC : MonoBehaviour
{
    public MISC_Type MT;

    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public BoxCollider2D BC;

    public float Gem_Level = 5.0f;

    public virtual void Start()
    {
        SR = this.AddComponent<SpriteRenderer>();
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
            switch (MT)
            {
                case MISC_Type.Gem:
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
                                    All_Manager.instance.Audio_M.MISC_Get_Effect_Audio_Play(MISC_Type.Gem);
                                }

                                this.gameObject.SetActive(false);

                                All_Manager.instance.GameMode_M.MISC_List.Remove(this);

                                All_Manager.instance.Pool_M.MISC_Pool_In(this);
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
                case MISC_Type.Chest:
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
                                    All_Manager.instance.Audio_M.MISC_Get_Effect_Audio_Play(MISC_Type.Chest);
                                }

                                this.gameObject.SetActive(false);

                                All_Manager.instance.GameMode_M.MISC_List.Remove(this);

                                All_Manager.instance.Pool_M.MISC_Pool_In(this);
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
                case MISC_Type.Food:
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
                                    All_Manager.instance.Audio_M.MISC_Get_Effect_Audio_Play(MISC_Type.Food);
                                }

                                this.gameObject.SetActive(false);

                                All_Manager.instance.GameMode_M.MISC_List.Remove(this);

                                All_Manager.instance.Pool_M.MISC_Pool_In(this);
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

    public void MISC_Setting(MISC p_MISC, STO_MISC MISC_STO)
    {
        p_MISC.SR.sprite = MISC_STO.Sprite_Image;
    }

    public void MISC_Clear()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
            All_Manager.instance.Pool_M.MISC_Pool_In(this);
        }
        else
        {
            All_Manager.instance.GameMode_M.MISC_List.Remove(this);
        }

    }

}
// 