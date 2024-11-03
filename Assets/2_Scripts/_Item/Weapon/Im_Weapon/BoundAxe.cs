using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoundAxe : Weapon
{
    public Vector3[] W_Axe_Point = new Vector3[4];

    public float Dis;

    public Vector3 BoxSize;

    public float Axe_Timer = 0.0f;

    public bool Axe_Throwing = false;

    public float Rotation_Speed = 360f;

    float screenHalfHeight;
    float screenHalfWidth;

    public override void Start()
    {
        base.Start();
        screenHalfHeight = Camera.main.orthographicSize;
        screenHalfWidth = screenHalfHeight * Camera.main.aspect;
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            Axe_Attack();

            this.Weapon_KnockBack_Weapon();

            this.transform.Rotate(Vector3.forward, Rotation_Speed * Time.deltaTime);

            this.Weapon_Timer();
            Weapon_Duration += Time.deltaTime;
        }

    }

    public void Axe_Init(Vector3 p_One_Vector, Vector3 p_Two_Vector, Vector3 p_Three_Vector, Vector3 p_Four_Vector)
    {
        W_Axe_Point[0] = p_One_Vector;
        W_Axe_Point[1] = p_Two_Vector;
        W_Axe_Point[2] = p_Three_Vector;
        W_Axe_Point[3] = p_Four_Vector;
        Weapon_Duration = 0;
        Weapon_Time = 5;
    }

    public void Axe_Attack()
    {
        this.Weapon_Accumulate();

        this.transform.position = Axe_Bezier(W_Axe_Point[0], W_Axe_Point[1], W_Axe_Point[2], W_Axe_Point[3], W_Accumulate);

        // Right
        if
            ((Camera.main.transform.position.x + screenHalfWidth) < this.transform.position.x)
        {
            W_Accumulate = 0;
            W_Axe_Point[0] = this.transform.position;

            W_Axe_Point[1] = W_Axe_Point[0] + new Vector3(0, 5, 0);
            W_Axe_Point[2] = W_Axe_Point[1] + new Vector3(Random.Range(-5, -1), 0, 0);        // 오른쪽
            W_Axe_Point[3] = W_Axe_Point[0] + new Vector3(Random.Range(-5, -1), -10, 0);
        }
        // Left
        else if ((Camera.main.transform.position.x - screenHalfWidth) > this.transform.position.x)
        {
            W_Accumulate = 0;
            W_Axe_Point[0] = this.transform.position;

            W_Axe_Point[1] = W_Axe_Point[0] + new Vector3(0, 5, 0);
            W_Axe_Point[2] = W_Axe_Point[1] + new Vector3(Random.Range(1, 5), 0, 0);        // 오른쪽
            W_Axe_Point[3] = W_Axe_Point[0] + new Vector3(Random.Range(1, 5), -10, 0);
        }
        // Top
        else if ((Camera.main.transform.position.y + screenHalfWidth) < this.transform.position.y)
        {
            // 일어날리가 없음
        }
        // Bottom
        else if ((Camera.main.transform.position.y - screenHalfWidth) > this.transform.position.y)
        {
            W_Accumulate = 0;
            float height = W_Axe_Point[0].y / 2.0f;
            W_Axe_Point[0] = this.transform.position;

            W_Axe_Point[1] = W_Axe_Point[0] + new Vector3(0, height, 0);
            W_Axe_Point[2] = W_Axe_Point[1] + new Vector3(Random.Range(1, 5), height, 0);        // 오른쪽
            W_Axe_Point[3] = W_Axe_Point[0] + new Vector3(Random.Range(1, 5), -10, 0);
        }
    }

    public override void Upgrade(int p_Level)
    {

    }

    public override void Weapon_Stat_Appliaction()
    {
        base.Weapon_Stat_Appliaction();
    }

    public override void Weapon_To_Enemy_Collision()
    {
        base.Weapon_To_Enemy_Collision();
    }

    void SwapVec3(ref Vector3 v1, ref Vector3 v2)
    {
        Vector3 temp = v1;
        v1 = v2;
        v2 = temp;
    }

    // 베지어 곡선
    public Vector3 Axe_Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, Value);
        Vector3 B = Vector3.Lerp(P_2, P_3, Value);
        Vector3 C = Vector3.Lerp(P_3, P_4, Value);

        Vector3 D = Vector3.Lerp(A, B, Value);
        Vector3 E = Vector3.Lerp(B, C, Value);

        Vector3 F = Vector3.Lerp(D, E, Value);

        return F;
    }


}
