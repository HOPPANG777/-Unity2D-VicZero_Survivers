using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;



public class TreasureGemJackpot : MonoBehaviour
{
    public Vector3[] GemBezierVec = new Vector3[4];

    public float GemTimer = 0.0f;
    public float GemTimerPower = 1.0f;

    public bool GemTrigger = false;

    public float GemSpd = 0.1f;

    public void Start()
    {
        Vector3 One = AllManager.instance.UIM.TreasureGemImageList.GetComponent<RectTransform>().transform.position;
        Vector3 Two, Three, Four;

        Two = One + new Vector3(0, 1000, 0);
        Three = Two + new Vector3(Random.Range(-1000, 1000), 0, 0);
        Four = One + new Vector3(Random.Range(-1000, 1000), -1000, 0);

        TreasureGemInit(One, Two, Three, Four);
    }

    public void Update()
    {
        if (AllManager.instance.UIM.TreasureBox.TreasureOpenChset == true)
        {
            TreasureGemActivate();
        }

        else
        {
            TreasureGemDisabled();
        }
    }

    public void TreasureGemInit(Vector3 Point00, Vector3 Point01, Vector3 Point02, Vector3 Point03)
    {
        GemBezierVec[0] = Point00;
        GemBezierVec[1] = Point01;
        GemBezierVec[2] = Point02;
        GemBezierVec[3] = Point03;
    }

    public void TreasureGemActivate()
    {
        if (GemTimer >= GemTimerPower)
        {
            GemTimer = 0;
            GemTrigger = !GemTrigger;
        }
        else
        {
            GemTimer += (Time.deltaTime * Random.Range(0.01f, 0.9f));
        }

        // 다음에 생성될때 방향 반대로 변경

        this.GetComponent<RectTransform>().transform.position = TreasureGemBezier(GemBezierVec[0], GemBezierVec[1], GemBezierVec[2], GemBezierVec[3], GemTimer); 
    }

    public void TreasureGemDisabled()
    {
        GemTimer = 0;
        Vector3 One = AllManager.instance.UIM.TreasureGemImageList.GetComponent<RectTransform>().transform.position;
        this.GetComponent<RectTransform>().transform.position = One; 
    }

    public Vector3 TreasureGemBezier(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4, float Value)
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

#region
//public Rigidbody2D RB;
//public BoxCollider2D boxCollider2D;

//public Vector3[] GemVector = new Vector3[4];

//public float Accumulate = 0.0f;

//public bool isActive = false;

//public void Update()
//{
//    if (isActive == true)
//    {
//        if (Accumulate > 1.0f)
//        {
//            Accumulate = 0;
//        }

//        TreasureBezier();
//    }
//}

//public void TreasureGemInit(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4)
//{
//    GemVector[0] = P1;
//    GemVector[1] = P2;
//    GemVector[2] = P3;
//    GemVector[3] = P4;
//}

//public void TreasureBezier()
//{
//    GemTimer();

//    TreasureGemBezier(GemVector[0],GemVector[1], GemVector[2],  GemVector[3], Accumulate);
//}

//public void GemTimer()
//{
//    Accumulate += (1f * Time.deltaTime);
//}

//public void GemDisabled()
//{
//    this.transform.localScale = Vector3.one;

//    Accumulate = 0f;

//    isActive = false;

//    this.gameObject.SetActive(false);

//    this.GetComponent<RectTransform>().anchoredPosition = GemVector[0];
//}

//public void GemActivate(Vector3 Start, Vector3 Ctrl1, Vector3 Ctrl2, Vector3 End)
//{
//    TreasureGemInit(Start, Ctrl1, Ctrl2, End);

//    isActive = true;

//    this.gameObject.SetActive(true);
//}

//private void CheckBounds(Vector3 Pos)
//{
//    Vector3 ScreenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, Pos);
//    if (ScreenPosition.x < 0 || ScreenPosition.x > Screen.width)
//    {
//        // X 방향으로 튕기기
//        GemVector[1].x = -GemVector[1].x;
//        GemVector[2].x = -GemVector[2].x;
//    }
//    if (ScreenPosition.y < 0)
//    {
//        // 화면 아래로 사라지면 비활성화
//       GemDisabled();
//    }
//}

//public Vector3 TreasureGemBezier(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4, float Value)
//{
//    Vector3 A = Vector3.Lerp(P1, P2, Value);
//    Vector3 B = Vector3.Lerp(P2, P3, Value);
//    Vector3 C = Vector3.Lerp(P3, P4, Value);

//    Vector3 D = Vector3.Lerp(A, B, Value);
//    Vector3 E = Vector3.Lerp(B, C, Value);

//    Vector3 F = Vector3.Lerp(D, E, Value);

//    return F;
//}
#endregion