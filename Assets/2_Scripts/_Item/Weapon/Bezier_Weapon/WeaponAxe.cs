using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAxe : Weapon
{

    public Vector3[] WAxePoint = new Vector3[4];

    public float Dis;

    public Vector3 BoxSize;

    public float AxeTimer = 0.0f;

    public bool AxeThrowing = false;

    public float RotationSpeed = 360f;

    public override void Start()
    {
        base.Start();
    }   

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            AxeAttack();

            this.WeaponKnockBackWeapon();

            this.transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);

            this.WeaponTimer();
        }

        else { }
    }

    public void AxeInit(Vector3 pOneVector, Vector3 pTwoVector, Vector3 pThreeVector, Vector3 pFourVector)
    {
        WAxePoint[0] = pOneVector;
        WAxePoint[1] = pTwoVector;
        WAxePoint[2] = pThreeVector;
        WAxePoint[3] = pFourVector;
    }

    public void AxeAttack()
    {
        this.WeaponAccumulate();

        this.transform.position = AxeBezier(WAxePoint[0], WAxePoint[1], WAxePoint[2], WAxePoint[3], WAccumulate);
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

    // 베지어 곡선
    public Vector3 AxeBezier(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4, float Value)
    {
        Vector3 A = Vector3.Lerp(P1, P2, Value);
        Vector3 B = Vector3.Lerp(P2, P3, Value);
        Vector3 C = Vector3.Lerp(P3, P4, Value);

        Vector3 D = Vector3.Lerp(A, B, Value);
        Vector3 E = Vector3.Lerp(B, C, Value);

        Vector3 F = Vector3.Lerp(D, E, Value);

        return F;
    }


}
