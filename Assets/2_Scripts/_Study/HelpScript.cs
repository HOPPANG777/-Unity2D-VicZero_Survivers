using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;


public class HelpScript : MonoBehaviour
{

    public Vector3[] GemBezierVec = new Vector3[4];

    public float GemTimer = 0.0f;
    public float GemTimerPower = 1.0f;

    public bool GemTrigger = false;

    public float GemSpd = 0.1f;

    public void Start()
    {
        Vector3 One = this.transform.parent.position;
        Vector3 Two, Three, Four;

        Two = One + new Vector3(0, 1000, 0);
        Three = Two + new Vector3(Random.Range(-1000, 1000), 0, 0);
        Four = One + new Vector3(Random.Range(-1000, 1000), -1000, 0);

        TreasureGemInit(One, Two, Three, Four);
    }

    public void Update()
    {

        TreasureGemActivate();

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
/*
 *     public GameObject TestGo;
    public GameObject TestGo2;

    public Image TestImage;
    public Image TestImage2;

    public GameObject PivotGo;

    public Text TestText;
    public Button TestButton;
    public Button TestButton2;

    public Sprite TestSprite;
    public Sprite TestSprite2;

    public bool Stop = false;

    public bool Testbool = false;
    public bool Testbool2 = false;

    public GameObject ShieldGo = null;
    public float Timer = 0;
    public int TimerPower = 1;

    public float StartPosY = 0;
    public float ResetPosY = 2300;
    public float Speed = 2f;

    public float TempTime02 = 0.0f;
    public float TempTime = 0.0f;
    public Image Temp;

    public int Number = 0;

    //public TreasureGemJackpot Treasure;

    void Start()
    {

    }

    void Update()
    {



        //if (TestGo.GetComponent<RectTransform>().anchoredPosition.y > 1900)
        //{
        //    TestGo.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        //}

        //if (TestGo != null && Stop == false)
        //{
        //    TestGo.transform.Translate(Vector3.up);
        //}


        if (Testbool == true)
        {
            Timer += (Time.deltaTime * TimerPower);

            ShieldGo.transform.localScale = new Vector3(1, Mathf.Sin(Timer), 1);

            if (Input.GetKey(KeyCode.Space))
            {
                Stop = true;

                TestGemGo();

                float Dis = 10000;

                for (int i = 0; i < TestGo.transform.childCount; i++)
                {
                    if (Vector3.Distance(TestGo.transform.GetChild(i).transform.position, PivotGo.transform.position) < Dis)
                    {
                        Dis = Vector3.Distance(TestGo.transform.GetChild(i).transform.position, PivotGo.transform.position);
                        Number = i;
                    }
                }

                for (int j = 0; j < TestGo2.transform.childCount; j++)
                {
                    if (Vector3.Distance(TestGo2.transform.GetChild(j).transform.position, PivotGo.transform.position) < Dis)
                    {
                        Dis = Vector3.Distance(TestGo2.transform.GetChild(j).transform.position, PivotGo.transform.position);
                        Number = j;
                    }
                }

                Debug.Log(TestGo.transform.GetChild(Number).name);
            }

            if (TestGo != null && TestGo2 != null && Stop == false)
            {
                // 이미지를 위로 이동
                TestGo.GetComponent<RectTransform>().anchoredPosition += Vector2.up * Speed * Time.deltaTime;
                TestGo2.GetComponent<RectTransform>().anchoredPosition += Vector2.up * Speed * Time.deltaTime;

                // 첫 번째 이미지가 기준점을 넘으면 재배치
                if (TestGo.GetComponent<RectTransform>().anchoredPosition.y >= ResetPosY)
                {
                    RepositionImage(TestGo.GetComponent<RectTransform>());
                }

                // 두 번째 이미지가 기준점을 넘으면 재배치
                if (TestGo2.GetComponent<RectTransform>().anchoredPosition.y >= ResetPosY)
                {
                    RepositionImage(TestGo2.GetComponent<RectTransform>());
                }
            }
            else if (Stop == true)
            {
                TestButton2.gameObject.SetActive(true);
            }

            if (TestText.GetComponent<RectTransform>().transform.localScale.x > 0.01f && TestButton.GetComponent<RectTransform>().transform.localScale.x > 0.01f)
            {
                TestText.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                TestButton.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                TestButton.GetComponent<RectTransform>().transform.Rotate(0, 0, 100f * Time.deltaTime);
            }
            else
            {
                TestText.gameObject.SetActive(false);
                TestButton.gameObject.SetActive(false);
            }

            TempTime += 10.0f * Time.time;
            TempTime02 += 4.0f * Time.time;

            if (TempTime < 600)
            {
                TestImage2.GetComponent<RectTransform>().sizeDelta = new Vector2(100, TempTime);
            }

            if (TempTime02 < 250)
            {
                TestImage2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, TempTime02);
            }
        }

        else
        {
            TestText.gameObject.SetActive(true);
            TestText.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            TestText.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
            TestButton.gameObject.SetActive(true);
            TestButton.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            TestButton.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
            TestButton2.gameObject.SetActive(false);
        }

        if (Testbool == true)
        {

        }
    }

    public void TestButtonClick()
    {
        TestImage.sprite = TestSprite2;
        Testbool = true;
        PivotGo.gameObject.SetActive(true);

    }

    public void TestButtonClick2()
    {
        TestImage.sprite = TestSprite;
        Testbool = false;
        Number = 0;
        PivotGo.gameObject.SetActive(false);
        Temp.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
        Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50f);
        TempTime = 0f;
        TempTime02 = 0f;
        Stop = false;
    }

    public void TestGemGo()
    {
        //Vector3 Start1 = this.transform.position;
        //Vector3 End1 = new Vector3(Screen.width / 2, 0, 0);

        //// 제어점들을 랜덤하게 설정
        //Vector3 controlPoint1 = new Vector3(
        //    Random.Range(Start1.x - 200f, Start1.x + 200f),
        //    Random.Range(Start1.y, Start1.y + 200f),
        //    0f
        //);

        //Vector3 controlPoint2 = new Vector3(
        //Random.Range(End1.x - 200f, End1.x + 200f),
        //    Random.Range(Start1.y, Start1.y + 200f),
        //    0f
        //);

        //Treasure.GemActivate(Start1, controlPoint1, controlPoint2, End1);
    }

    void RepositionImage(RectTransform image)
    {
        // 이미지를 아래로 재배치
        float newY = image.anchoredPosition.y - 2 * 1900f;
        image.anchoredPosition = new Vector2(image.anchoredPosition.x, newY);
    }
 */


// 오디오 매니저
////얘는 오디오 소스 . 스프라이트 랜더러
//// 오디오 플레이어
//public AudioSource BackgroundAS;
//public AudioSource UIClickAS;


//public List<AudioSource> EffectAudioList = new List<AudioSource>();

//void Start()
//{
//    BackgroundAS.loop = true;

//    for (int i = 0; i < System.Enum.GetValues(typeof(WeaponType)).Length; i++)
//    {
//        GameObject Go = new GameObject(((WeaponType)i).ToString() + "-AS");

//        Go.transform.parent = this.transform;

//        Go.AddComponent<AudioSource>();

//        Go.GetComponent<AudioSource>().clip = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr((WeaponType)i).HitAudioClip;

//        EffectAudioList.Add(Go.GetComponent<AudioSource>());
//    }

//    for (int i = 0; i < System.Enum.GetValues(typeof(UnitType)).Length; i++)
//    {
//        GameObject Go = new GameObject(((UnitType)i).ToString() + "-AS");

//        Go.transform.parent = this.transform;

//        Go.AddComponent<AudioSource>();

//        Go.GetComponent<AudioSource>().clip = AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr((UnitType)i).DeathAudioClip;

//        EffectAudioList.Add(Go.GetComponent<AudioSource>());
//    }
//}

//public void WeaponHitEffectAudioPlay(WeaponType pweaponType)
//{
//    int PlayingAudioCount = 0;

//    for(int i = 0; i < EffectAudioList.Count; i++)
//    {
//        if (EffectAudioList[i].isPlaying) PlayingAudioCount++;
//    }

//    if (PlayingAudioCount < 6)
//    {
//        for (int i = 0; i < EffectAudioList.Count; i++)
//        {
//            if(EffectAudioList[i].gameObject.name.Contains(pweaponType.ToString()))
//            {
//                EffectAudioList[i].Play();
//                break;
//            }
//        }
//    }
//}

//public void UnitDeathEffectAudioPlay(UnitType pUnitType)
//{
//    int PlayingAudioCount = 0;

//    for (int i = 0; i < EffectAudioList.Count; i++)
//    {
//        if (EffectAudioList[i].isPlaying) PlayingAudioCount++;
//    }

//    if (PlayingAudioCount < 6)
//    {
//        for (int i = 0; i < EffectAudioList.Count; i++)
//        {
//            if (EffectAudioList[i].gameObject.name.Contains(pUnitType.ToString()))
//            {
//                EffectAudioList[i].Play();
//                break;
//            }
//        }
//    }
//}

//public void BackgroundAudioPlay()
//{

//}

//public void UIClickAudioPlay()
//{




//}
#endregion