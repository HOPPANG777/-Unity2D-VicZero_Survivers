using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO
- bullets 초기화해서 사용할 FireBullet넣어주기
 */

public class Phoenix : Weapon
{
    public FireBullet bullet;

    List<FireBullet> bullets;
    int bulletCount;
    float shootGapValue;
    public const float shootGap = 0.1f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        shootGapValue += Time.deltaTime;
        if (shootGapValue > shootGap)
        {
            shootGapValue -= shootGap;
            bullets[bulletCount].Shoot(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            bulletCount++;
            if (bulletCount >= bullets.Count) bulletCount = 0;
        }
    }

    public override void Upgrade(int p_Level)
    {
        
    }

    public override void Weapon_Stat_Appliaction()
    {
        base.Weapon_Stat_Appliaction();
    }

}
