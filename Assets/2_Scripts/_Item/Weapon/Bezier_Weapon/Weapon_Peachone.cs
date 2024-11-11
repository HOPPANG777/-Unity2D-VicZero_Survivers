using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon_Peachone : Weapon
{
    Vector3[] W_Peachone_Point = new Vector3[4];

    [SerializeField] public float Pos_A = 3f;
    [SerializeField] public float Pos_B = 2f;

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

            Peachone_Draw_Trajectory();

            //this.Weapon_Timer();

            this.Weapon_KnockBack_Weapon();

            this.Weapon_Trail();

            if (W_Accumulate > 1)
            {
                this.Destroy_Weapon();
            }
        }
    }
    protected override void AutoUse()
    {

    }
    protected override void Use()
    {

    }
    public void Peachone_Init(Vector3 One, Vector3 Two, Vector3 Three, Vector3 Four)
    {
        W_Peachone_Point[0] = One; // P0
        W_Peachone_Point[1] = Two; // P1
        W_Peachone_Point[2] = Three; // P2
        W_Peachone_Point[3] = Four; // P3
        TR.enabled = true;
    }

    public Vector3 Peachone_Point_Setting(Vector3 p_VE)
    {
        float X, Y;

        X = Pos_A * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + p_VE.x;
        Y = Pos_B * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + p_VE.y;

        return new Vector3(X, Y, 0);
    }

    public void Peachone_Draw_Trajectory()
    {
        this.Weapon_Accumulate();

        this.transform.position = new Vector3
            (
                Peachone_Bezier(W_Peachone_Point[0].x, W_Peachone_Point[1].x, W_Peachone_Point[2].x, W_Peachone_Point[3].x, W_Accumulate),
                Peachone_Bezier(W_Peachone_Point[0].y, W_Peachone_Point[1].y, W_Peachone_Point[2].y, W_Peachone_Point[3].y, W_Accumulate)
            );
    }

    public float Peachone_Bezier(float P_1, float P_2, float P_3, float P_4, float Value)
    {
        return Mathf.Pow((1 - Value), 3) * P_1
        + Mathf.Pow((1 - Value), 2) * 3 * Value * P_2
        + Mathf.Pow(Value, 2) * 3 * (1 - Value) * P_3
        + Mathf.Pow(Value, 3) * P_4;
    }

    public Vector3 Convert_RectTransform_To_Position(RectTransform p_RT)
    {
        return new Vector3((p_RT.position.x), (p_RT.position.y), 0);
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


}

