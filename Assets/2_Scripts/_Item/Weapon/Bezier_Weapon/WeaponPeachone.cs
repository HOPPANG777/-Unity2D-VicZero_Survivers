using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPeachone : Weapon
{
    Vector3[] WPeachonePoint = new Vector3[4];

    [SerializeField] public float PosA = 3f;
    [SerializeField] public float PosB = 2f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            PeachoneDrawTrajectory();

            //this.WeaponTimer();

            this.WeaponKnockBackWeapon();

            if (WAccumulate > 1)
            {
                this.DestroyWeapon();
            }
        }

        else
        {

        }
    }

    public void PeachoneInit(Vector3 One, Vector3 Two, Vector3 Three, Vector3 Four)
    {
        WPeachonePoint[0] = One; // P0
        WPeachonePoint[1] = Two; // P1
        WPeachonePoint[2] = Three; // P2
        WPeachonePoint[3] = Four; // P3
        weaponTrailRenderer.enabled = true;
    }

    public Vector3 PeachonePointSetting(Vector3 pVE)
    {
        float X, Y;

        X = PosA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + pVE.x;
        Y = PosB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + pVE.y;

        return new Vector3(X, Y, 0);
    }

    public void PeachoneDrawTrajectory()
    {
        this.WeaponAccumulate();

        this.transform.position = new Vector3(PeachoneBezier(WPeachonePoint[0].x, WPeachonePoint[1].x, WPeachonePoint[2].x, WPeachonePoint[3].x, WAccumulate),
           PeachoneBezier(WPeachonePoint[0].y, WPeachonePoint[1].y, WPeachonePoint[2].y, WPeachonePoint[3].y, WAccumulate));
    }

    public float PeachoneBezier(float P1, float P2, float P3, float P4, float Value)
    {
        //Vector3 A = Vector3.Lerp(P1, P2, Value);
        //Vector3 B = Vector3.Lerp(P2, P3, Value);
        //Vector3 C = Vector3.Lerp(P3, P4, Value);

        //Vector3 D = Vector3.Lerp(A, B, Value);
        //Vector3 E = Vector3.Lerp(B, C, Value);

        //Vector3 F = Vector3.Lerp(D, E, Value);

        //return F;

        return Mathf.Pow((1 - Value), 3) * P1
        + Mathf.Pow((1 - Value), 2) * 3 * Value * P2
        + Mathf.Pow(Value, 2) * 3 * (1 - Value) * P3
        + Mathf.Pow(Value, 3) * P4;
    }

    public Vector3 ConvertRectTransformToPosition(RectTransform pRT)
    {
        return new Vector3((pRT.position.x), (pRT.position.y), 0);
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

