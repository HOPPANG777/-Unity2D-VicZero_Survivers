using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponForce : Weapon
{
    public UnitEnemy Ftarget;
    public CircleCollider2D CC;

    public override void Start()
    {
        CC = this.AddComponent<CircleCollider2D>();
        CC.isTrigger = true;
        base.Start();
        weaponSpriteRenderer.sortingOrder = -1;
        weaponSpriteRenderer.color = new Color(0, 0, 0, 0);
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            this.weaponSpriteRenderer.size = new Vector2(WeaponRange + 2.0f, WeaponRange + 2.0f);

            //ForceAttack();

            WeaponKnockBackWeapon();

            this.WeaponTimer();

            weaponSpriteRenderer.material = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Force).SpriteMaterial;
        }

        else
        {

        }
    }

    public void ForceAttack()
    {
        weaponBoxCollider2D.isTrigger = false;
        CC.radius = WeaponRange * 0.5f;

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(this.transform.position, (WeaponRange * 0.5f));

        float Dis = 10000;

        foreach (UnityEngine.Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.GetComponent<UnitEnemy>())
            {
                UnitEnemy EnemyTemp = collider.gameObject.GetComponent<UnitEnemy>();

                if (Dis > Vector3.Distance(this.transform.position, EnemyTemp.transform.position))
                {
                    Dis = Vector3.Distance(this.transform.position, EnemyTemp.transform.position);
                    WeaponTargetWeapon = EnemyTemp;
                }

                //WeaponTargetWeapon = EnemyTemp;
            }

        }

        if (WeaponTargetWeapon != null)
        {
            Vector3 Dir = (WeaponTargetWeapon.transform.position - this.transform.position).normalized;

            if (WeaponTargetWeapon.UnitType == UnitType.Barrel)
            {

            }

            else
            {
                WeaponTargetWeapon.KnockBack(Dir, WeaponKnock, WeaponDuration);
            }
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

//foreach (var collider in collider2Ds)
//{
//    if (collider.gameObject.GetComponent<UnitEnemy>())
//    {
//        var EnemyTemp = collider.gameObject.GetComponent<UnitEnemy>();

//        if (Dis > Vector3.Distance(this.transform.position, EnemyTemp.transform.position))
//        {
//            Dis = Vector3.Distance(this.transform.position, EnemyTemp.transform.position);
//            Ftarget = EnemyTemp;
//        }
//    }
//}

//if (Ftarget != null)
//{
//    Vector3 Dir = (Ftarget.transform.position - this.transform.position).normalized;

//    //Vector3 Knock = Dir * WeaponSpeed;

//   // Ftarget.KnockBack(Dir, WeaponKnock, WeaponDuration, WeaponKncokTemp);
//   Ftarget.KnockBack(Dir, WeaponKnock, WeaponDuration);
//}
//public class Force : Weapon
//{
//    public Enemy Ftarget;
//    public CircleCollider2D CC;

//    public override void Start()
//    {
//        CC = this.AddComponent<CircleCollider2D>();
//        CC.isTrigger = true;
//        CC.radius = 1f;
//        base.Start();
//        boxCollider2D.size = new Vector2(0f, 0f);
//        boxCollider2D.isTrigger = false;
//    }

//    public override void Update()
//    {
//        var collider2Ds = Physics2D.OverlapCircleAll(this.transform.position, WeaponRange);

//        float Dis = 10000;

//        foreach (var collider in collider2Ds)
//        {
//            if (collider.gameObject.GetComponent<Enemy>())
//            {
//                var EnemyTemp = collider.gameObject.GetComponent<Enemy>();

//                if (Dis > Vector3.Distance(this.transform.position, EnemyTemp.transform.position))
//                {
//                    Dis = Vector3.Distance(this.transform.position, EnemyTemp.transform.position);
//                    Ftarget = EnemyTemp;
//                }
//            }
//        }

//        if (Ftarget != null)
//        {
//            Vector3 Dir = (Ftarget.transform.position - this.transform.position).normalized;

//            Vector3 Knock = Dir * WeaponSpeed;

//            Ftarget.Push(Knock);
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