using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Weapon_FireWand : Weapon
{
    private Camera mainCamera;

    public override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            this.transform.position = this.transform.position + (-this.transform.right * Weapon_Speed * Time.deltaTime);

            this.Weapon_KnockBack_Weapon();

            Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                this.Destroy_Weapon();
            }
        }

        else
        { 
        
        }
    }

    public override void Upgrade(int p_Level)
    {
        if (p_Level == 3)
        {
            All_Manager.instance.GameMode_M.Current_Player.AddWeaponKeyword(this.WeaponKeywords);
        }
    }

    public override void Weapon_Stat_Appliaction()
    {
        base.Weapon_Stat_Appliaction();
    }

    public override void Weapon_To_Enemy_Collision()
    {
        base.Weapon_To_Enemy_Collision();
    }
}

#region
//public void FireWand_Init()   // 안쓰는거
//{
//    if (All_Manager.instance.GameMode_M.Game_Stop == false)
//    {
//        int FireWand_Count = 0;

//        if (Mine == null) return;

//        for (int i = 0; i < All_Manager.instance.GameMode_M.Current_Player.All_Weapon.Count; i++)
//        {
//            if (All_Manager.instance.GameMode_M.Current_Player.All_Weapon[i].GetComponent<Weapon_FireWand>())
//            {
//                FireWand_Count++;
//            }
//        }

//        var Temp_Count = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Count(Weapon_Type.FireWand, All_Manager.instance.GameMode_M.Current_Player.FireWand_LAT);

//        for (int i = 0; i < Temp_Count; i++)
//        {
//            // GameObject boltObj = Instantiate(boltPrefebs, player.transform.position + new Vector3(0, 0.3f), Quaternion.Euler(0, 0, (360 * i / (FireWand_Count - 1)) + 180)); //투사체의 각도가 자연스럽게 회전값을 넣었다.
//            GameObject FireWand_Go = new GameObject("FireWand");

//            FireWand_Go.AddComponent<Weapon_FireWand>();

//            FireWand_Go.transform.position = Mine.transform.position; // + new Vector3(0, 0.3f)
//                                                                    //FireWand_Go.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (Weapon_Count - 1)) + 180);
//            FireWand_Go.transform.rotation = Quaternion.identity;
//            FireWand_Go.transform.localScale = Vector3.one;

//            Vector3 dirVec = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / (Temp_Count - 1)), Mathf.Sin((Mathf.PI) * i * 2 / (Temp_Count - 1))); //원을 기준으로 각각의 투사체마다 벡터를 정리
//            FireWand_Go.transform.position = FireWand_Go.transform.position + (dirVec.normalized * Weapon_Speed * Time.deltaTime);

//            //this.transform.position = this.transform.position + (-this.transform.right * Weapon_Speed * Time.deltaTime);

//            //Vector3 dirVec = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / (Weapon_Level - 1)), Mathf.Sin((Mathf.PI) * i * 2 / (Weapon_Level - 1)));
//        }
//    }

//    else
//    {

//    }


//}
//for (int i = 0; i < level; i++)
//{
//    GameObject clone = Instantiate(WeaponPrefab, this.transform.localPosition, Quaternion.identity);
//
//    if (clone.GetComponent<WeaponPrefab>())
//    {
//        yield return new WaitUntil(() => clone.GetComponent<WeaponPrefab>().SR);
//        clone.GetComponent<WeaponPrefab>().SR.sprite = All_Manager.instance.Resource_M.STO_Weapon.Sprite_Image;
//    }
//
//    time = IntervalAngle * i;
//    float MoveX = Mathf.Cos(time * Mathf.PI / 180.0f);
//    float MoveY = Mathf.Sin(time * Mathf.PI / 180.0f);
//    Vector2 MoveDir = new Vector2(MoveX, MoveY) * speed;
//
//    clone.GetComponent<Rigidbody2D>().AddGarlic(MoveDir * speed);
//    // clone.transform.position = clone.transform.position * (clone.transform.up * MoveDir);
//}

//for (int i = 0; i < Weapon_Level; i++)
//{
//    투사체의 각도가 자연스럽게 회전값을 넣었다.
//    GameObject clone = Instantiate(FireWand_Go, this.transform.position + new Vector3(0, 0.3f), Quaternion.Euler(0, 0, (360 * i / (Weapon_Level - 1)) + 180));

//    GameObject clone = new GameObject("FireWand");

//    clone.AddComponent<FireWand>();

//    clone.transform.position = this.transform.position + new Vector3(0, 0.3f);
//    Vector3 dirVec = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / (Weapon_Level - 1)), Mathf.Sin((Mathf.PI) * i * 2 / (Weapon_Level - 1)));
//    clone.transform.position = clone.transform.position + (dirVec.normalized * Weapon_Speed * Time.deltaTime);

//    clone.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (Weapon_Level - 1)) + 180);
//    clone.transform.localScale = Vector3.one;

//    if (clone.GetComponent<FireWand>())
//    {
//        yield return new WaitUntil(() => clone.GetComponent<FireWand>().SR);
//        All_Manager.instance.Resource_M.Weapon_Stat(clone.GetComponent<FireWand>(), All_Manager.instance.Resource_M.STO_FireWand);
//        Debug.Log("1");
//    }
//    원을 기준으로 각각의 투사체마다 벡터를 정리
//    clone.GetComponent<Rigidbody2D>().AddGarlic(dirVec * Weapon_Speed * Time.deltaTime);
//}

//this.transform.position = this.transform.position + (-this.transform.right * Weapon_Speed * Time.deltaTime);
#endregion