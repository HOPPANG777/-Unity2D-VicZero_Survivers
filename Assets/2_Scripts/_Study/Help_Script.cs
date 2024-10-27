using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;


public class Help_Script : MonoBehaviour
{

    public Vector3[] Gem_Bezier_Vec = new Vector3[4];

    public float Gem_Timer = 0.0f;
    public float Gem_Timer_Power = 1.0f;

    public bool Gem_Trigger = false;

    public float Gem_Spd = 0.1f;

    public void Start()
    {
        Vector3 One = this.transform.parent.position;
        Vector3 Two, Three, Four;

        Two = One + new Vector3(0, 1000, 0);
        Three = Two + new Vector3(Random.Range(-1000, 1000), 0, 0);
        Four = One + new Vector3(Random.Range(-1000, 1000), -1000, 0);

        Treasure_Gem_Init(One, Two, Three, Four);
    }

    public void Update()
    {

        Treasure_Gem_Activate();

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
/*
 *     public GameObject Test_Go;
    public GameObject Test_Go2;

    public Image Test_Image;
    public Image Test_Image2;

    public GameObject Pivot_Go;

    public Text Test_Text;
    public Button Test_Button;
    public Button Test_Button2;

    public Sprite Test_Sprite;
    public Sprite Test_Sprite2;

    public bool Stop = false;

    public bool Test_bool = false;
    public bool Test_bool2 = false;

    public GameObject Shield_Go = null;
    public float Timer = 0;
    public int Timer_Power = 1;

    public float Start_Pos_Y = 0;
    public float Reset_Pos_Y = 2300;
    public float Speed = 2f;

    public float Temp_Time_02 = 0.0f;
    public float Temp_Time = 0.0f;
    public Image Temp;

    public int Number = 0;

    //public Treasure_Gem_Jackpot Treasure;

    void Start()
    {

    }

    void Update()
    {



        //if (Test_Go.GetComponent<RectTransform>().anchoredPosition.y > 1900)
        //{
        //    Test_Go.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        //}

        //if (Test_Go != null && Stop == false)
        //{
        //    Test_Go.transform.Translate(Vector3.up);
        //}


        if (Test_bool == true)
        {
            Timer += (Time.deltaTime * Timer_Power);

            Shield_Go.transform.localScale = new Vector3(1, Mathf.Sin(Timer), 1);

            if (Input.GetKey(KeyCode.Space))
            {
                Stop = true;

                Test_Gem_Go();

                float Dis = 10000;

                for (int i = 0; i < Test_Go.transform.childCount; i++)
                {
                    if (Vector3.Distance(Test_Go.transform.GetChild(i).transform.position, Pivot_Go.transform.position) < Dis)
                    {
                        Dis = Vector3.Distance(Test_Go.transform.GetChild(i).transform.position, Pivot_Go.transform.position);
                        Number = i;
                    }
                }

                for (int j = 0; j < Test_Go2.transform.childCount; j++)
                {
                    if (Vector3.Distance(Test_Go2.transform.GetChild(j).transform.position, Pivot_Go.transform.position) < Dis)
                    {
                        Dis = Vector3.Distance(Test_Go2.transform.GetChild(j).transform.position, Pivot_Go.transform.position);
                        Number = j;
                    }
                }

                Debug.Log(Test_Go.transform.GetChild(Number).name);
            }

            if (Test_Go != null && Test_Go2 != null && Stop == false)
            {
                // 이미지를 위로 이동
                Test_Go.GetComponent<RectTransform>().anchoredPosition += Vector2.up * Speed * Time.deltaTime;
                Test_Go2.GetComponent<RectTransform>().anchoredPosition += Vector2.up * Speed * Time.deltaTime;

                // 첫 번째 이미지가 기준점을 넘으면 재배치
                if (Test_Go.GetComponent<RectTransform>().anchoredPosition.y >= Reset_Pos_Y)
                {
                    RepositionImage(Test_Go.GetComponent<RectTransform>());
                }

                // 두 번째 이미지가 기준점을 넘으면 재배치
                if (Test_Go2.GetComponent<RectTransform>().anchoredPosition.y >= Reset_Pos_Y)
                {
                    RepositionImage(Test_Go2.GetComponent<RectTransform>());
                }
            }
            else if (Stop == true)
            {
                Test_Button2.gameObject.SetActive(true);
            }

            if (Test_Text.GetComponent<RectTransform>().transform.localScale.x > 0.01f && Test_Button.GetComponent<RectTransform>().transform.localScale.x > 0.01f)
            {
                Test_Text.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                Test_Button.GetComponent<RectTransform>().transform.localScale -= Vector3.one * 0.7f * Time.deltaTime;
                Test_Button.GetComponent<RectTransform>().transform.Rotate(0, 0, 100f * Time.deltaTime);
            }
            else
            {
                Test_Text.gameObject.SetActive(false);
                Test_Button.gameObject.SetActive(false);
            }

            Temp_Time += 10.0f * Time.time;
            Temp_Time_02 += 4.0f * Time.time;

            if (Temp_Time < 600)
            {
                Test_Image2.GetComponent<RectTransform>().sizeDelta = new Vector2(100, Temp_Time);
            }

            if (Temp_Time_02 < 250)
            {
                Test_Image2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Temp_Time_02);
            }
        }

        else
        {
            Test_Text.gameObject.SetActive(true);
            Test_Text.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            Test_Text.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
            Test_Button.gameObject.SetActive(true);
            Test_Button.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            Test_Button.GetComponent<RectTransform>().transform.rotation = Quaternion.identity;
            Test_Button2.gameObject.SetActive(false);
        }

        if (Test_bool == true)
        {

        }
    }

    public void Test_Button_Click()
    {
        Test_Image.sprite = Test_Sprite2;
        Test_bool = true;
        Pivot_Go.gameObject.SetActive(true);

    }

    public void Test_Button_Click2()
    {
        Test_Image.sprite = Test_Sprite;
        Test_bool = false;
        Number = 0;
        Pivot_Go.gameObject.SetActive(false);
        Temp.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
        Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50f);
        Temp_Time = 0f;
        Temp_Time_02 = 0f;
        Stop = false;
    }

    public void Test_Gem_Go()
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

        //Treasure.Gem_Activate(Start1, controlPoint1, controlPoint2, End1);
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
//public AudioSource Background_AS;
//public AudioSource UI_Click_AS;


//public List<AudioSource> Effect_Audio_List = new List<AudioSource>();

//void Start()
//{
//    Background_AS.loop = true;

//    for (int i = 0; i < System.Enum.GetValues(typeof(Weapon_Type)).Length; i++)
//    {
//        GameObject Go = new GameObject(((Weapon_Type)i).ToString() + "-AS");

//        Go.transform.parent = this.transform;

//        Go.AddComponent<AudioSource>();

//        Go.GetComponent<AudioSource>().clip = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr((Weapon_Type)i).Hit_Audio_Clip;

//        Effect_Audio_List.Add(Go.GetComponent<AudioSource>());
//    }

//    for (int i = 0; i < System.Enum.GetValues(typeof(Unit_Type)).Length; i++)
//    {
//        GameObject Go = new GameObject(((Unit_Type)i).ToString() + "-AS");

//        Go.transform.parent = this.transform;

//        Go.AddComponent<AudioSource>();

//        Go.GetComponent<AudioSource>().clip = All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr((Unit_Type)i).Death_Audio_Clip;

//        Effect_Audio_List.Add(Go.GetComponent<AudioSource>());
//    }
//}

//public void Weapon_Hit_Effect_Audio_Play(Weapon_Type p_WT)
//{
//    int Playing_Audio_Count = 0;

//    for(int i = 0; i < Effect_Audio_List.Count; i++)
//    {
//        if (Effect_Audio_List[i].isPlaying) Playing_Audio_Count++;
//    }

//    if (Playing_Audio_Count < 6)
//    {
//        for (int i = 0; i < Effect_Audio_List.Count; i++)
//        {
//            if(Effect_Audio_List[i].gameObject.name.Contains(p_WT.ToString()))
//            {
//                Effect_Audio_List[i].Play();
//                break;
//            }
//        }
//    }
//}

//public void Unit_Death_Effect_Audio_Play(Unit_Type p_UT)
//{
//    int Playing_Audio_Count = 0;

//    for (int i = 0; i < Effect_Audio_List.Count; i++)
//    {
//        if (Effect_Audio_List[i].isPlaying) Playing_Audio_Count++;
//    }

//    if (Playing_Audio_Count < 6)
//    {
//        for (int i = 0; i < Effect_Audio_List.Count; i++)
//        {
//            if (Effect_Audio_List[i].gameObject.name.Contains(p_UT.ToString()))
//            {
//                Effect_Audio_List[i].Play();
//                break;
//            }
//        }
//    }
//}

//public void Background_Audio_Play()
//{

//}

//public void UI_Click_Audio_Play()
//{




//}
#endregion