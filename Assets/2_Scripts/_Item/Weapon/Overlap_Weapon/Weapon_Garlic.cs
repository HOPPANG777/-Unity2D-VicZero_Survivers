using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon_Garlic : Weapon
{
    public Unit_Enemy F_target;
    public CircleCollider2D CC;

    public override void Start()
    {
        CC = this.AddComponent<CircleCollider2D>();
        CC.isTrigger = true;
        base.Start();
        SR.sortingOrder = -1;
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            this.SR.size = new Vector2(Weapon_Range + 2.0f, Weapon_Range + 2.0f);

            Garlic_Attack();

            this.Weapon_Timer();
            
            SR.material = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Weapon_Type.Garlic).Sprite_Material;
        }

        else
        {

        }
    }

    public void Garlic_Attack()
    {
        BC.isTrigger = false;
        CC.radius = Weapon_Range;

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(this.transform.position, (Weapon_Range - 0.5f));

        float Dis = 10000;

        foreach (UnityEngine.Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.GetComponent<Unit_Enemy>())
            {
                Unit_Enemy Enemy_Temp = collider.gameObject.GetComponent<Unit_Enemy>();

                if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
                {
                    Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
                    Weapon_Target_Weapon = Enemy_Temp;
                }

                //Weapon_Target_Weapon = Enemy_Temp;
            }

        }

        if (Weapon_Target_Weapon != null)
        {
            Vector3 Dir = (Weapon_Target_Weapon.transform.position - this.transform.position).normalized;

            if (Weapon_Target_Weapon.UT == Unit_Type.Barrel)
            {

            }

            else
            {
                Weapon_Target_Weapon.Knock_Back(Dir, Weapon_Knock, Weapon_Duration);
            }
        }

        //BC.isTrigger = false;
        //CC.radius = Weapon_Range;

        //Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(this.transform.position, (Weapon_Range - 5.0f));

        //float Dis = 10000;

        //foreach (UnityEngine.Collider2D collider in collider2Ds)
        //{
        //    if (collider.gameObject.GetComponent<Unit_Enemy>())
        //    {
        //        Unit_Enemy Enemy_Temp = collider.gameObject.GetComponent<Unit_Enemy>();

        //        if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
        //        {
        //            Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
        //            Weapon_Target_Weapon = Enemy_Temp;
        //        }
        //    }
        //}

        //if (Weapon_Target_Weapon != null)
        //{
        //    Vector3 Dir = (Weapon_Target_Weapon.transform.position - this.transform.position).normalized;

        //    if (Weapon_Target_Weapon.UT != Unit_Type.Barrel)
        //    {
        //        // 넉백 메서드 호출
        //        Weapon_Target_Weapon.Knock_Back(Dir, Weapon_Knock, Weapon_Duration);
        //    }
        //}
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

}

#region
//foreach (var collider in collider2Ds)
//{
//    if (collider.gameObject.GetComponent<Unit_Enemy>())
//    {
//        var Enemy_Temp = collider.gameObject.GetComponent<Unit_Enemy>();

//        if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
//        {
//            Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
//            F_target = Enemy_Temp;
//        }
//    }
//}

//if (F_target != null)
//{
//    Vector3 Dir = (F_target.transform.position - this.transform.position).normalized;

//    //Vector3 Knock = Dir * Weapon_Speed;

//   // F_target.Knock_Back(Dir, Weapon_Knock, Weapon_Duration, Weapon_Kncok_Temp);
//   F_target.Knock_Back(Dir, Weapon_Knock, Weapon_Duration);
//}
//public class Garlic : Weapon
//{
//    public Enemy F_target;
//    public CircleCollider2D CC;

//    public override void Start()
//    {
//        CC = this.AddComponent<CircleCollider2D>();
//        CC.isTrigger = true;
//        CC.radius = 1f;
//        base.Start();
//        BC.size = new Vector2(0f, 0f);
//        BC.isTrigger = false;
//    }

//    public override void Update()
//    {
//        var collider2Ds = Physics2D.OverlapCircleAll(this.transform.position, Weapon_Range);

//        float Dis = 10000;

//        foreach (var collider in collider2Ds)
//        {
//            if (collider.gameObject.GetComponent<Enemy>())
//            {
//                var Enemy_Temp = collider.gameObject.GetComponent<Enemy>();

//                if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
//                {
//                    Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
//                    F_target = Enemy_Temp;
//                }
//            }
//        }

//        if (F_target != null)
//        {
//            Vector3 Dir = (F_target.transform.position - this.transform.position).normalized;

//            Vector3 Knock = Dir * Weapon_Speed;

//            F_target.Push(Knock);
//        }

//    }

//    public override void OnTriggerStay2D(Collider2D collision)
//    {
//        base.OnTriggerStay2D(collision);
//    }


//}

/*
자 이제 정리해보자 
지금 유닛에서 Push를 완성하고
여기에다가 푸쉬를 불러온다
그리고 플레이어에서 쿨타임을 만들고
쿨타임이 차면 포스를 사용하도록 한다.
아니면 여기다가 쿨타임을 만들수도 있다.
그게 더 쉬울수도 일단 먼저 푸쉬를 만들자

*/
#endregion