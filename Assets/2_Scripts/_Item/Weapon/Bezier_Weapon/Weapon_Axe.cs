using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Axe : Weapon
{

    public Vector3[] W_Axe_Point = new Vector3[4];

    public float Dis;

    public Vector3 BoxSize;

    public float Axe_Timer = 0.0f;

    public bool Axe_Throwing = false;

    public float Rotation_Speed = 360f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
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
        }

        else { }
    }

    public void Axe_Init(Vector3 p_One_Vector, Vector3 p_Two_Vector, Vector3 p_Three_Vector, Vector3 p_Four_Vector)
    {
        W_Axe_Point[0] = p_One_Vector;
        W_Axe_Point[1] = p_Two_Vector;
        W_Axe_Point[2] = p_Three_Vector;
        W_Axe_Point[3] = p_Four_Vector;
    }

    public void Axe_Attack()
    {
        this.Weapon_Accumulate();

        this.transform.position = Axe_Bezier(W_Axe_Point[0], W_Axe_Point[1], W_Axe_Point[2], W_Axe_Point[3], W_Accumulate);
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
