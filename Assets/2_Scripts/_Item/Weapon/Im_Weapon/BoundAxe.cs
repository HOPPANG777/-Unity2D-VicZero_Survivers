using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoundAxe : Weapon
{
    public Vector3[] WAxePoint = new Vector3[4];

    public float Dis;

    public Vector3 BoxSize;

    public float AxeTimer = 0.0f;

    public bool AxeThrowing = false;

    public float RotationSpeed = 360f;

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
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            AxeAttack();

            this.WeaponKnockBackWeapon();

            this.transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);

            this.WeaponTimer();
            WeaponDuration += Time.deltaTime;
        }

    }

    public void AxeInit(Vector3 pOneVector, Vector3 pTwoVector, Vector3 pThreeVector, Vector3 pFourVector)
    {
        WAxePoint[0] = pOneVector;
        WAxePoint[1] = pTwoVector;
        WAxePoint[2] = pThreeVector;
        WAxePoint[3] = pFourVector;
        WeaponDuration = 0;
        WeaponTime = 5;
    }

    public void AxeAttack()
    {
        this.WeaponAccumulate();

        this.transform.position = AxeBezier(WAxePoint[0], WAxePoint[1], WAxePoint[2], WAxePoint[3], WAccumulate);

        // Right
        if
            ((Camera.main.transform.position.x + screenHalfWidth) < this.transform.position.x)
        {
            WAccumulate = 0;
            WAxePoint[0] = this.transform.position;

            WAxePoint[1] = WAxePoint[0] + new Vector3(0, 5, 0);
            WAxePoint[2] = WAxePoint[1] + new Vector3(Random.Range(-5, -1), 0, 0);        // 오른쪽
            WAxePoint[3] = WAxePoint[0] + new Vector3(Random.Range(-5, -1), -10, 0);
        }
        // Left
        else if ((Camera.main.transform.position.x - screenHalfWidth) > this.transform.position.x)
        {
            WAccumulate = 0;
            WAxePoint[0] = this.transform.position;

            WAxePoint[1] = WAxePoint[0] + new Vector3(0, 5, 0);
            WAxePoint[2] = WAxePoint[1] + new Vector3(Random.Range(1, 5), 0, 0);        // 오른쪽
            WAxePoint[3] = WAxePoint[0] + new Vector3(Random.Range(1, 5), -10, 0);
        }
        // Top
        else if ((Camera.main.transform.position.y + screenHalfWidth) < this.transform.position.y)
        {
            // 일어날리가 없음
        }
        // Bottom
        else if ((Camera.main.transform.position.y - screenHalfWidth) > this.transform.position.y)
        {
            WAccumulate = 0;
            float height = WAxePoint[0].y / 2.0f;
            WAxePoint[0] = this.transform.position;

            WAxePoint[1] = WAxePoint[0] + new Vector3(0, height, 0);
            WAxePoint[2] = WAxePoint[1] + new Vector3(Random.Range(1, 5), height, 0);        // 오른쪽
            WAxePoint[3] = WAxePoint[0] + new Vector3(Random.Range(1, 5), -10, 0);
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

    void SwapVec3(ref Vector3 v1, ref Vector3 v2)
    {
        Vector3 temp = v1;
        v1 = v2;
        v2 = temp;
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
