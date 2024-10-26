using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;



public class Treasure_Gem_Jackpot : MonoBehaviour
{
    public Vector3[] Gem_Bezier_Vec = new Vector3[4];

    public float Gem_Timer = 0.0f;
    public float Gem_Timer_Power = 1.0f;

    public bool Gem_Trigger = false;

    public float Gem_Spd = 0.1f;

    public void Start()
    {
        Vector3 One = All_Manager.instance.UI_M.Treasure_Gem_Image_List.GetComponent<RectTransform>().transform.position;
        Vector3 Two, Three, Four;

        Two = One + new Vector3(0, 1000, 0);
        Three = Two + new Vector3(Random.Range(-1000, 1000), 0, 0);
        Four = One + new Vector3(Random.Range(-1000, 1000), -1000, 0);

        Treasure_Gem_Init(One, Two, Three, Four);
    }

    public void Update()
    {
        if (All_Manager.instance.UI_M.Treasure_Open_Chset == true)
        {
            Treasure_Gem_Activate();
        }

        else
        {
            Treasure_Gem_Disabled();
        }
    }

    public void Treasure_Gem_Init(Vector3 Point_00, Vector3 Point_01, Vector3 Point_02, Vector3 Point_03)
    {
        Gem_Bezier_Vec[0] = Point_00;
        Gem_Bezier_Vec[1] = Point_01;
        Gem_Bezier_Vec[2] = Point_02;
        Gem_Bezier_Vec[3] = Point_03;
    }

    public void Treasure_Gem_Activate()
    {
        if (Gem_Timer >= Gem_Timer_Power)
        {
            Gem_Timer = 0;
            Gem_Trigger = !Gem_Trigger;
        }
        else
        {
            Gem_Timer += (Time.deltaTime * Random.Range(0.01f, 0.9f));
        }

        // 다음에 생성될때 방향 반대로 변경

        this.GetComponent<RectTransform>().transform.position = Treasure_Gem_Bezier(Gem_Bezier_Vec[0], Gem_Bezier_Vec[1], Gem_Bezier_Vec[2], Gem_Bezier_Vec[3], Gem_Timer); 
    }

    public void Treasure_Gem_Disabled()
    {
        Gem_Timer = 0;
        Vector3 One = All_Manager.instance.UI_M.Treasure_Gem_Image_List.GetComponent<RectTransform>().transform.position;
        this.GetComponent<RectTransform>().transform.position = One; 
    }

    public Vector3 Treasure_Gem_Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)
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

#region
//public Rigidbody2D RB;
//public BoxCollider2D BC;

//public Vector3[] Gem_Vector = new Vector3[4];

//public float Accumulate = 0.0f;

//public bool is_Active = false;

//public void Update()
//{
//    if (is_Active == true)
//    {
//        if (Accumulate > 1.0f)
//        {
//            Accumulate = 0;
//        }

//        Treasure_Bezier();
//    }
//}

//public void Treasure_Gem_Init(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4)
//{
//    Gem_Vector[0] = P_1;
//    Gem_Vector[1] = P_2;
//    Gem_Vector[2] = P_3;
//    Gem_Vector[3] = P_4;
//}

//public void Treasure_Bezier()
//{
//    Gem_Timer();

//    Treasure_Gem_Bezier(Gem_Vector[0],Gem_Vector[1], Gem_Vector[2],  Gem_Vector[3], Accumulate);
//}

//public void Gem_Timer()
//{
//    Accumulate += (1f * Time.deltaTime);
//}

//public void Gem_Disabled()
//{
//    this.transform.localScale = Vector3.one;

//    Accumulate = 0f;

//    is_Active = false;

//    this.gameObject.SetActive(false);

//    this.GetComponent<RectTransform>().anchoredPosition = Gem_Vector[0];
//}

//public void Gem_Activate(Vector3 Start, Vector3 Ctrl_1, Vector3 Ctrl_2, Vector3 End)
//{
//    Treasure_Gem_Init(Start, Ctrl_1, Ctrl_2, End);

//    is_Active = true;

//    this.gameObject.SetActive(true);
//}

//private void Check_Bounds(Vector3 Pos)
//{
//    Vector3 Screen_Position = RectTransformUtility.WorldToScreenPoint(Camera.main, Pos);
//    if (Screen_Position.x < 0 || Screen_Position.x > Screen.width)
//    {
//        // X 방향으로 튕기기
//        Gem_Vector[1].x = -Gem_Vector[1].x;
//        Gem_Vector[2].x = -Gem_Vector[2].x;
//    }
//    if (Screen_Position.y < 0)
//    {
//        // 화면 아래로 사라지면 비활성화
//       Gem_Disabled();
//    }
//}

//public Vector3 Treasure_Gem_Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)
//{
//    Vector3 A = Vector3.Lerp(P_1, P_2, Value);
//    Vector3 B = Vector3.Lerp(P_2, P_3, Value);
//    Vector3 C = Vector3.Lerp(P_3, P_4, Value);

//    Vector3 D = Vector3.Lerp(A, B, Value);
//    Vector3 E = Vector3.Lerp(B, C, Value);

//    Vector3 F = Vector3.Lerp(D, E, Value);

//    return F;
//}
#endregion