using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ShadowPinion : Weapon
{
    public float Dir;
    public float RotateSpeed = 200f;

    public Unit_Enemy ShadowPinion_target;

    public Vector3 Start_Dir;

    public Vector2 BoxSize;

    public override void Start()
    {
        base.Start();
        Start_Dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            ShadowPinion_Attack();

            this.Weapon_KnockBack_Weapon();
        }

        else
        {

        }


    }

    // 유도함정 작동 로직
    public void ShadowPinion_Attack()
    {
        BoxSize = new Vector2(Weapon_Range, Weapon_Range);

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(this.transform.position, BoxSize, 0); // 이게 그 유도함정 주변에서 박스 사이즈 만큼 탐지

        float Dis = 10000; //상대와 나의 거리

        //Debug.Log(collider2Ds.Length);

        Vector3 Dir = new Vector3();

        // C#때 나온 for문 업그레이드 버전
        // 배열에 맞춰 요소마다 접근 가능
        // 근데 for문에 비해 상당히 불안정한 기능
        // 그래서 나올때도 저거 쓰면 안됨! 이라며 반발했다.
        // 아직까진 멀쩡함!
        foreach (UnityEngine.Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.GetComponent<Unit_Enemy>())
            {
                Unit_Enemy Enemy_Temp = collider.gameObject.GetComponent<Unit_Enemy>();

                if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
                {
                    Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
                    ShadowPinion_target = Enemy_Temp;
                }
            }
        }

        if (ShadowPinion_target != null)
        {
            //방향 바꾸고
            Dir = (ShadowPinion_target.transform.position - this.transform.position).normalized; // 방향
            float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;    // 각도
            this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);    // 회전
        }

        else
        {
            //Debug.Log("Null");

            //this.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(Random.Range(0, 360), Random.Range(0, 360), 0));
            //this.transform.rotation = Quaternion.Euler(Random.Range(0,))
        }

        // 그 방향을 향해서 앞으로
        this.transform.position = this.transform.position + (Dir * Weapon_Speed * Time.deltaTime);
    }

    //public override void OnTriggerStay2D(Collider2D collision)
    //{
    //    base.OnTriggerStay2D(collision);
    //}

    public override void Weapon_To_Enemy_Collision()
    {
        base.Weapon_To_Enemy_Collision();
    }
}

//Temp_Go.transform.position = Mine.transform.position;
//Temp_Go.transform.rotation = Quaternion.identity;
//Temp_Go.transform.localScale = Vector3.one;
