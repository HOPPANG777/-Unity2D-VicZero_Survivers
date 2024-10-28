using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class Weapon_Cross : Weapon
{
    public Vector3[] W_Cross_Point = new Vector3[4];

    public Unit_Enemy Cross_target;
    public Unit_Player Cross_Point;

    public float Rotation_Speed = 360f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            Cross_Attack();

            this.Weapon_KnockBack_Weapon();

            this.transform.Rotate(Vector3.forward, Rotation_Speed * Time.deltaTime);

            //this.Weapon_Timer();
        }

        else
        {

        }


    }

    // 부메랑 베지어 곡선 할당
    public void Cross_Init(Vector3 p_One_Vector, Vector3 p_Two_Vector, Vector3 p_Three_Vector, Vector3 p_Four_Vector)
    {
        W_Cross_Point[0] = p_One_Vector;
        W_Cross_Point[1] = p_Two_Vector;
        W_Cross_Point[2] = p_Three_Vector;
        W_Cross_Point[3] = p_Four_Vector;
    }

    // 부메랑 작동 로직
    public void Cross_Attack()
    {
        this.Weapon_Accumulate();

        this.transform.position = Cross_Bezier(W_Cross_Point[0], W_Cross_Point[1], W_Cross_Point[2], W_Cross_Point[3], W_Accumulate);
    }

    public Vector3 Cross_Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, Value);
        Vector3 B = Vector3.Lerp(P_2, P_3, Value);
        Vector3 C = Vector3.Lerp(P_3, P_4, Value);

        Vector3 D = Vector3.Lerp(A, B, Value);
        Vector3 E = Vector3.Lerp(B, C, Value);

        Vector3 F = Vector3.Lerp(D, E, Value);

        return F;
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

#region
/*
크로스는 이것도 엑스랑 똑같다 근데 이제 베지어 곡선을 
이상하게 해야 할것이다.
부메랑이기 때문에
베지어 곡선을 그리는 패턴이 P1과 P4가 플레이어의 위치와 동일하거나 비슷해야한다.
그리고 부메랑은 플레이어가 이동하는 위치를 따라서 정확하게 올 필요가 없기 때문에
플레이어가 부메랑을 발사한 위치에 다시 돌아와야 한다.

햇갈린다면 뱀파이어 서바이버의 십자가를 다시 보면 된다.

// 초기 위치 설정
Vector3 One = this.transform.position;
Vector3 Two = One + new Vector3(0, 5, 0);       // 위
Vector3 Three = Two + new Vector3(5, 0, 0);     // 오른쪽
Vector3 Four = One + new Vector3(5, -10, 0);    // 오른쪽 아래

// 코루틴 내에서 위치에 따라 반대편으로 설정
Vector3 oppositeThree = Three;
Vector3 oppositeFour = Four;

// 예시로 x 좌표가 0보다 작으면 반대편으로 설정
if (this.transform.position.x < 0)
{
    oppositeThree = Two + new Vector3(-5, 0, 0);     // 반대편 오른쪽
    oppositeFour = One + new Vector3(-5, -10, 0);    // 반대편 오른쪽 아래
} 

Weapon_Go.GetComponent<Weapon_Axe>().Axe_Init(One, Two, oppositeThree, oppositeFour);

해당 코드로 도끼를 생성했을때 플레이어 위로 솟아올랐다가
if문으로 인해 
P3와 P2가 반대쪽으로 변경되어 

부메랑 처럼 휘게 된다

이걸 좀더 격하게 하고
소환될때 마다 시계방향 혹은 적이 있는 방향으로 
소환을 하면 해결 될것이다.

베지어 곡선으로 하지 말고 다르게 한번 해보자
*/

/*
 using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public float speed = 5f; // 부메랑 이동 속도
    private Vector3 startPoint; // 발사 시작점
    private Vector3 endPoint; // 발사 끝점
    private Vector3 controlPoint1; // 제어점 1
    private Vector3 controlPoint2; // 제어점 2
    private bool returning = false; // 부메랑이 반대편으로 돌아가는 중인지 여부

    void Start()
    {
        startPoint = transform.position; // 시작점은 부메랑의 현재 위치
        Vector3 direction = (player.position - startPoint).normalized;
        endPoint = startPoint + direction * 10f; // 발사 끝점 설정 (임의의 거리)
        controlPoint1 = startPoint + direction * 3f; // 제어점 1 설정
        controlPoint2 = endPoint - direction * 3f; // 제어점 2 설정
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        if (!returning)
        {
            // 플레이어 쪽으로 발사 중
            transform.position = Vector3.Lerp(startPoint, endPoint, step);
            if (Vector3.Distance(transform.position, endPoint) < 0.1f)
            {
                returning = true;
            }
        }
        else
        {
            // 반대편으로 돌아가는 중
            transform.position = Bezier(startPoint, controlPoint1, controlPoint2, step);
            if (Vector3.Distance(transform.position, startPoint) < 0.1f)
            {
                Destroy(gameObject); // 돌아온 후 부메랑 파괴
            }
        }
    }

    Vector3 Bezier(Vector3 start, Vector3 control1, Vector3 control2, float t)
    {
        // 베지어 곡선 계산 함수
        return Mathf.Pow(1 - t, 3) * start +
               3 * Mathf.Pow(1 - t, 2) * t * control1 +
               3 * (1 - t) * Mathf.Pow(t, 2) * control2 +
               Mathf.Pow(t, 3) * endPoint;
    }
}

 */

/*
 using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public Transform target; // 플레이어나 목표 지점의 Transform

    public float flightSpeed = 5f; // 부메랑의 비행 속도
    public float returnSpeed = 10f; // 되돌아오는 속도

    private Vector3[] path; // 베지어 곡선을 따라갈 경로

    private bool isReturning = false; // 되돌아가는 중인지 여부

    void Start() {
        // 발사될 때의 경로 설정 (시계 방향으로 발사)
        path = new Vector3[] {
            transform.position, // 현재 위치
            transform.position + Vector3.up * 5f, // 위로 발사
            transform.position + Vector3.up * 5f + Vector3.right * 5f, // 오른쪽 위로 발사
            transform.position + Vector3.right * 5f - Vector3.up * 10f // 오른쪽 아래로 발사
        };

        // 만약 초기 위치가 x 좌표가 0보다 작다면, 반대편으로 설정
        if (transform.position.x < 0) {
            path[2] = transform.position + Vector3.up * 5f - Vector3.right * 5f; // 반대편 오른쪽 위로 발사
            path[3] = transform.position - Vector3.up * 10f - Vector3.right * 5f; // 반대편 오른쪽 아래로 발사
        }
    }

    void Update() {
        if (!isReturning) {
            // 비행 중인 경우
            MoveAlongBezierCurve();
        } else {
            // 되돌아가는 중인 경우
            MoveTowardsPlayer();
        }
    }

    void MoveAlongBezierCurve() {
        // 비행 속도에 따라 시간 경과를 계산하여 베지어 곡선을 따라 이동
        float flightTime = Time.deltaTime * flightSpeed;
        Vector3 newPosition = BezierUtils.CalculateBezierPoint(path, flightTime);

        transform.position = newPosition;

        // 만약 도착점에 도달했다면, 되돌아가는 상태로 전환
        if (flightTime >= 1f) {
            isReturning = true;
        }
    }

    void MoveTowardsPlayer() {
        // 플레이어 쪽으로 직진하며 되돌아가기
        Vector3 moveDirection = (target.position - transform.position).normalized;
        transform.position += moveDirection * returnSpeed * Time.deltaTime;

        // 플레이어를 통과한 후 베지어 곡선이 끝나면 객체를 파괴
        if (Vector3.Distance(transform.position, target.position) <= 0.1f) {
            Destroy(gameObject);
        }
    }
}

 */
#endregion