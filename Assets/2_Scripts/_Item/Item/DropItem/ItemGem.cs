//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
// 
//using UnityEngine;

//public class ItemGem : Item
//{
//    public ItemType ItemType;

//    public float GemLevel = 1.0f;

//    //public float GemTimeer = 0.1f;
//    //public float GemTimeCount = 0f;

//    //public UnitPlayer Player = null;

//    public override void Start()
//    {
//        base.Start();
//    }

//    public void Update()
//    {
//        UnitPlayer TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;

//        if (TempPlayer == null)
//        {
//            //Debug.Log("Gem Target Player Null");
//            return;
//        }

//        else
//        {
//            if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) > TempPlayer.OriginStat.UnitGetGemRange)
//            {

//            }

//            else
//            {
//                if (Vector3.Distance(this.transform.position, TempPlayer.transform.position) < 0.1f)
//                {
//                    if (AllManager.instance.GameModeM.GameStop == false)
//                    {
//                        TempPlayer.GetGem(GemLevel);
//                    }

//                    this.gameObject.SetActive(false);

//                    AllManager.instance.GameModeM.GemList.Remove(this);

//                    AllManager.instance.PoolM.GemPoolIn(this);
//                }

//                var Dir = (TempPlayer.transform.position - this.transform.position).normalized;

//                this.transform.position += (Dir * 0.05f);

//                if (this.transform.localScale.x > 0.01f)
//                {
//                    this.transform.localScale -= Vector3.one * 0.02f;
//                }
//            }
//        }


//    }

//    public void GemClear()
//    {
//        if (this.gameObject != null)
//        {
//            AllManager.instance.PoolM.GemPoolIn(this);
//            //Destroy(this.gameObject);
//        }
//        else
//        {
//            AllManager.instance.GameModeM.GemList.Remove(this);
//        }

//    }

//}
