//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
// 
//using UnityEngine;

//public class Item_Gem : Item
//{
//    public ItemType ItemType;

//    public float Gem_Level = 1.0f;

//    //public float Gem_Timeer = 0.1f;
//    //public float Gem_Time_Count = 0f;

//    //public Unit_Player Player = null;

//    public override void Start()
//    {
//        base.Start();
//    }

//    public void Update()
//    {
//        Unit_Player Temp_Player = All_Manager.instance.GameMode_M.Current_Player;

//        if (Temp_Player == null)
//        {
//            //Debug.Log("Gem Target Player Null");
//            return;
//        }

//        else
//        {
//            if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) > Temp_Player.Origin_Stat.Unit_Get_Gem_Range)
//            {

//            }

//            else
//            {
//                if (Vector3.Distance(this.transform.position, Temp_Player.transform.position) < 0.1f)
//                {
//                    if (All_Manager.instance.GameMode_M.Game_Stop == false)
//                    {
//                        Temp_Player.Get_Gem(Gem_Level);
//                    }

//                    this.gameObject.SetActive(false);

//                    All_Manager.instance.GameMode_M.Gem_List.Remove(this);

//                    All_Manager.instance.Pool_M.Gem_Pool_In(this);
//                }

//                var Dir = (Temp_Player.transform.position - this.transform.position).normalized;

//                this.transform.position += (Dir * 0.05f);

//                if (this.transform.localScale.x > 0.01f)
//                {
//                    this.transform.localScale -= Vector3.one * 0.02f;
//                }
//            }
//        }


//    }

//    public void Gem_Clear()
//    {
//        if (this.gameObject != null)
//        {
//            All_Manager.instance.Pool_M.Gem_Pool_In(this);
//            //Destroy(this.gameObject);
//        }
//        else
//        {
//            All_Manager.instance.GameMode_M.Gem_List.Remove(this);
//        }

//    }

//}
