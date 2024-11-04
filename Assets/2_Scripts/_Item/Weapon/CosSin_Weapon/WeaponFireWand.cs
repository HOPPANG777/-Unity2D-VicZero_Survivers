using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class WeaponFireWand : Weapon
{
    private Camera mainCamera;

    public override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            this.transform.position = this.transform.position + (-this.transform.right * WeaponSpeed * Time.deltaTime);

            this.WeaponKnockBackWeapon();

            Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                this.DestroyWeapon();
            }
        }

        else
        { 
        
        }
    }

    public override void Upgrade(int pLevel)
    {

    }

    public override void WeaponStatAppliaction()
    {
        base.WeaponStatAppliaction();
    }

    public override void WeaponToEnemyCollision()
    {
        base.WeaponToEnemyCollision();
    }


}

#region
//public void FireWandInit()   // 안쓰는거
//{
//    if (AllManager.instance.GameModeM.GameStop == false)
//    {
//        int FireWandCount = 0;

//        if (Mine == null) return;

//        for (int i = 0; i < AllManager.instance.GameModeM.CurrentPlayer.AllWeapon.Count; i++)
//        {
//            if (AllManager.instance.GameModeM.CurrentPlayer.AllWeapon[i].GetComponent<WeaponFireWand>())
//            {
//                FireWandCount++;
//            }
//        }

//        var TempCount = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponCount(WeaponType.FireWand, AllManager.instance.GameModeM.CurrentPlayer.FireWandLAT);

//        for (int i = 0; i < TempCount; i++)
//        {
//            // GameObject boltObj = Instantiate(boltPrefebs, player.transform.position + new Vector3(0, 0.3f), Quaternion.Euler(0, 0, (360 * i / (FireWandCount - 1)) + 180)); //투사체의 각도가 자연스럽게 회전값을 넣었다.
//            GameObject FireWandGo = new GameObject("FireWand");

//            FireWandGo.AddComponent<WeaponFireWand>();

//            FireWandGo.transform.position = Mine.transform.position; // + new Vector3(0, 0.3f)
//                                                                    //FireWandGo.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (WeaponCount - 1)) + 180);
//            FireWandGo.transform.rotation = Quaternion.identity;
//            FireWandGo.transform.localScale = Vector3.one;

//            Vector3 dirVec = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / (TempCount - 1)), Mathf.Sin((Mathf.PI) * i * 2 / (TempCount - 1))); //원을 기준으로 각각의 투사체마다 벡터를 정리
//            FireWandGo.transform.position = FireWandGo.transform.position + (dirVec.normalized * WeaponSpeed * Time.deltaTime);

//            //this.transform.position = this.transform.position + (-this.transform.right * WeaponSpeed * Time.deltaTime);

//            //Vector3 dirVec = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / (WeaponLevel - 1)), Mathf.Sin((Mathf.PI) * i * 2 / (WeaponLevel - 1)));
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
//        yield return new WaitUntil(() => clone.GetComponent<WeaponPrefab>().spriteRenderer);
//        clone.GetComponent<WeaponPrefab>().spriteRenderer.sprite = AllManager.instance.ResourceM.WeaponData.SpriteImage;
//    }
//
//    time = IntervalAngle * i;
//    float MoveX = Mathf.Cos(time * Mathf.PI / 180.0f);
//    float MoveY = Mathf.Sin(time * Mathf.PI / 180.0f);
//    Vector2 MoveDir = new Vector2(MoveX, MoveY) * speed;
//
//    clone.GetComponent<Rigidbody2D>().AddForce(MoveDir * speed);
//    // clone.transform.position = clone.transform.position * (clone.transform.up * MoveDir);
//}

//for (int i = 0; i < WeaponLevel; i++)
//{
//    투사체의 각도가 자연스럽게 회전값을 넣었다.
//    GameObject clone = Instantiate(FireWandGo, this.transform.position + new Vector3(0, 0.3f), Quaternion.Euler(0, 0, (360 * i / (WeaponLevel - 1)) + 180));

//    GameObject clone = new GameObject("FireWand");

//    clone.AddComponent<FireWand>();

//    clone.transform.position = this.transform.position + new Vector3(0, 0.3f);
//    Vector3 dirVec = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / (WeaponLevel - 1)), Mathf.Sin((Mathf.PI) * i * 2 / (WeaponLevel - 1)));
//    clone.transform.position = clone.transform.position + (dirVec.normalized * WeaponSpeed * Time.deltaTime);

//    clone.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (WeaponLevel - 1)) + 180);
//    clone.transform.localScale = Vector3.one;

//    if (clone.GetComponent<FireWand>())
//    {
//        yield return new WaitUntil(() => clone.GetComponent<FireWand>().spriteRenderer);
//        AllManager.instance.ResourceM.WeaponStat(clone.GetComponent<FireWand>(), AllManager.instance.ResourceM.STOFireWand);
//        Debug.Log("1");
//    }
//    원을 기준으로 각각의 투사체마다 벡터를 정리
//    clone.GetComponent<Rigidbody2D>().AddForce(dirVec * WeaponSpeed * Time.deltaTime);
//}

//this.transform.position = this.transform.position + (-this.transform.right * WeaponSpeed * Time.deltaTime);
#endregion