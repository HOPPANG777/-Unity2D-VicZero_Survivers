using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test_Script : MonoBehaviour
{
    /*
    가비지 콜렉터
    우리 쓰레기 수집가!

    C/C++과 C#의 가장 큰 차이점이라고
    할 수 있는 GC!

    C++(C)는 메모리에 직접 할당하고
    해제해주었어야 했던것과 다르게
    C#은 자동으로 메모리 할당과 해제를 담당한다.

    하지만 여기서 주의 할 점이 있는데

    GC는 메모리 영역을 해제해서 빈 공간으로
    만드는게 아니다.

    실제로는 그냥 해당 메모리 구역을 쓰레기라고 
    가정했을때 비유했을땐 그 쓰레기를 줍는게 아니라
    주위에 팬스를 쳐서 여기 접근 금지를 
    때리는것과 마찬가지란 소리다.

    그래서 생성과 삭제를 반복하게 된다면
    메모리 누수가 일어날테고
    이 때문에 자주 사용하는 객체를 미리 생성해두어
    필요할때마다 꺼내오고 다시 돌려보내는
    오브젝트 풀링을 사용한다.
    */

    //시간차를 이용해서 만들기
    //이보크 함수 - X
    //Update()
    //코루틴

    //코루틴은 시간차를 이용한 기능이나.
    //일정시간 이후에 처리하는 기능을 원할때 사용하는 함수이다.
    //코루틴은 게임의 큰흐름을 강이라고 쳤을때
    //강 에서 빠져나와서 흐르다 다시 큰강으로 돌아오는
    //시냇물이라 볼수 있다.
    //하지만 코루틴을 지나치게 너무 많이 쓰게 되면
    //성능에 영향을 주어 게임을 망칠수 있으니 
    //적절한 사용이 필요하다.

    public VFX_Force_Area Temp;

    public IEnumerator C_Test()
    {
        while (true)
        {
            Debug.Log("해위");
            yield return new WaitForSeconds(1);
            Debug.Log("안해위");
        }
    }

    private void Start()
    {
        Temp = this.AddComponent<VFX_Force_Area>();

        StartCoroutine(C_Temp_C());
    }

    private void Update()
    {
        
    }

    IEnumerator C_Temp_C()
    {
        yield return new WaitUntil(() => Temp.GetComponent<VFX_Force_Area>());

        Temp.VFX_Force_Particle();
    }

}

//Temp_Time += 10.0f * Time.time;
//Temp_Time_02 += 4.0f * Time.time;

//if (Temp_Time < 600)
//{
//    Temp.GetComponent<RectTransform>().sizeDelta = new Vector2(100, Temp_Time);
//}

//if (Temp_Time_02 < 250)
//{
//    Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Temp_Time_02);
//}
//public void OnTriggerEnter2D(Collider2D collision)
//{
//    // 몬스터끼리 충돌 시 밀려나게 하는 처리
//    if (this.isAlive == true)
//    {
//        if (collision.transform.GetComponent<Unit_
//        >())
//        {
//            Vector3 Other_Enemy_Collision = this.transform.position - collision.transform.position;
//            Other_Enemy_Collision = Other_Enemy_Collision.normalized * 0.1f;

//            this.transform.position = this.transform.position + Other_Enemy_Collision;
//            //for (int i = 0; i < 100; i++)
//            //{
//            //    this.transform.position = this.transform.position + ((Other_Enemy_Collision.normalized * 1f) * 0.005f * (100 - i) * Time.deltaTime);
//            //}
//        }
//    }

//    else
//    {

//    }
//}

//public void Pong_Attack()
//{
//    var Dist = (this.transform.position - Camera.main.transform.position).z;

//    var Left_Border = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Dist)).x;
//    var Right_Border = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Dist)).x;
//    var Top_Border = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Dist)).y;
//    var Bottom_Border = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Dist)).y;

//    Vector3 Pong_Size = this.gameObject.transform.position;

//    this.transform.position = new Vector3(

//    Mathf.Clamp(this.
//    transform.position.x,
//    Left_Border + Pong_Size.x / 2,
//    Right_Border - Pong_Size.x / 2),
//    Mathf.Clamp
//    (this.transform.position.y,
//    Top_Border + Pong_Size.y / 2,
//    Bottom_Border - Pong_Size.y / 2),
//    this.transform.position.z);

//}

/*
     public Image Test_Image = null;

    public float FDZZ = 0.0046296296f;

    private void Update()
    {
        this.transform.position = Convert_RectTransform_To_Position(Test_Image.rectTransform);
    }

    public Vector3 Convert_RectTransform_To_Position(RectTransform p_RT)
    {
        //return new Vector3((p_RT.position.x * 2), (p_RT.position.y * 2 ), 0);

        return new Vector3((p_RT.position.x * 2 - Screen.width) * FDZZ, (p_RT.position.y * 2 - Screen.height) * FDZZ, 0);
        //return new Vector3((p_RT.position.x * 2 - Screen.width), (p_RT.position.y * 2 - Screen.height), 0);

    }
 */

/*
public class Test_Script : MonoBehaviour
{
    public Image Test_Image_1;
    public GameObject Test_1;
    public float Test_Time_1 = 0;

    public void Update()
    {

        Test_Time_1 += Time.deltaTime * 2;
        if (Test_Time_1 < 360)
        {
            float x = 1f * Mathf.Cos(Test_Time_1);
            float y = 1f * Mathf.Sin(Test_Time_1);
            Test_Image_1.rectTransform.position = Test_1.transform.position - new Vector3(x, y, 0);
            // Test_Image_1.rectTransform.position = Test_1.transform.position + new Vector3(Random.Range(0, x), Random.Range(0, y), 0);
            // 오류 이유 Mine이 생성되기 전에 불러와서
            // 해결책 내가 아는건 코루틴 
        }
        else
        {
            Test_Time_1 = 0;
        }

        Test_Image_1.rectTransform.position -= new Vector3(Random.Range(0, 10), Random.Range(0, 10), 0) * (0.005f);
    }
}
*/
//public GameObject GameObject;
//[Range(0, 1)]
//public float Test;

//public Vector3 P1;
//public Vector3 P2;
//public Vector3 P3;
//public Vector3 P4;

//public void Update()
//{
//    GameObject.transform.position = BezierTest(P1, P2, P3, P4, Test);
//}

//public Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)
//{
//    Vector3 A = Vector3.Lerp(P_1, P_2, Value);
//    Vector3 B = Vector3.Lerp(P_2, P_3, Value);
//    Vector3 C = Vector3.Lerp(P_3, P_4, Value);

//    Vector3 D = Vector3.Lerp(A, B, Value);
//    Vector3 E = Vector3.Lerp(B, C, Value);

//    Vector3 F = Vector3.Lerp(D, E, Value);

//    return F;
//}

//public Image Peachone_Range_Image;

//public GameObject Test_1;
//public GameObject Test_2;

//public float Peachone_Time = 0;
//public float Peachone_Speed = 2;
//public float Peachone_Range = 1;

//public void Update()
//{
//    Peachone_Time += Time.deltaTime * Peachone_Speed;
//    if (Peachone_Time < 360)
//    {
//        float x = Peachone_Range * Mathf.Cos(Peachone_Time);
//        float y = Peachone_Range * Mathf.Sin(Peachone_Time);
//        Peachone_Range_Image.rectTransform.position = Test_1.transform.position + new Vector3(x, y, 0);
//    }
//    else
//    {
//        Peachone_Time = 0;
//    }

//}

/////////////////////////////////////////////////////////////////////////////////////

//Vector2[] point = new Vector2[4];
//Animator anim;
//bool hit = false;

//[SerializeField][Range(0, 1)] private float t = 0;
//[SerializeField] public float spd = 5;
//[SerializeField] public float posA = 0.55f;
//[SerializeField] public float posB = 0.45f;

//public GameObject master;
//public GameObject enemy;

//void Start()
//{
//    anim = GetComponent<Animator>();

//    point[0] = master.transform.position; // P0
//    point[1] = PointSetting(master.transform.position); // P1
//    point[2] = PointSetting(enemy.transform.position); // P2
//    point[3] = enemy.transform.position; // P3
//}

//void FixedUpdate()
//{
//    if (t > 1) return;
//    if (hit) return;
//    t += Time.deltaTime * spd;
//    DrawTrajectory();
//}

//Vector2 PointSetting(Vector2 origin)
//{
//    float x, y;

//    x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad)
//    + origin.x;
//    y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad)
//    + origin.y;
//    return new Vector2(x, y);
//}

//void DrawTrajectory()
//{
//    transform.position = new Vector2(
//    FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
//    FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
//    );
//}

//void OnTriggerEnter2D(Collider2D collision)
//{
//    if (collision.gameObject == enemy)
//    {
//        hit = true;
//        anim.SetTrigger("hit");
//        Destroy(gameObject, 0.35f);
//    }
//}


//private float FourPointBezier(float a, float b, float c, float d)
//{
//    return Mathf.Pow((1 - t), 3) * a
//    + Mathf.Pow((1 - t), 2) * 3 * t * b
//    + Mathf.Pow(t, 2) * 3 * (1 - t) * c
//    + Mathf.Pow(t, 3) * d;
//}

/////////////////////////////////////////////////////////////////////////////////////

//[SerializeField] public GameObject ShadowPinion;
//[SerializeField] public GameObject target;

//[SerializeField] public float spd;
//[SerializeField] public int shot = 12;

//public void Shot()
//{
//    StartCoroutine(CreateShadowPinion());
//}

//IEnumerator CreateShadowPinion()
//{
//    int _shot = shot;
//    while (_shot > 0)
//    {
//        _shot--;
//        GameObject FireWand = Instantiate(ShadowPinion, transform);
//        FireWand.GetComponent<BezierShadowPinion>().master = gameObject;
//        FireWand.GetComponent<BezierShadowPinion>().enemy = target;

//        yield return new WaitForSeconds(0.1f);
//    }
//    yield return null;
//}

//[Range(0f, 10f)] public float speed = 1;
//[Range(0f, 10f)] public float radius = 1;

//[Range(0f, 10f)] public float runningTime = 0;
//public Vector2 newPos = new Vector2();

//public void Update()
//{
//    //runningTime += Time.deltaTime * speed;
//    float x = radius * Mathf.Cos(runningTime);
//    float y = radius * Mathf.Sin(runningTime);
//    newPos = new Vector2(x, y);
//    this.transform.position = newPos;
//}

//public Vector3 direction = new Vector3(1, 1, 0); // Initial direction
//public float speed = 5.0f;
//public ParticleSystem Garlic;

//void Update()
//{
//    // Normalize the direction
//    direction = direction.normalized;

//    // Move the object
//    transform.position += direction * speed * Time.deltaTime;

//    // Check for collisions with screen edges and reflect
//    Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

//    if (viewportPos.x <= 0.0f || viewportPos.x >= 1.0f)
//    {
//        direction = Vector3.Reflect(direction, Vector3.right); // Reflect on x-axis
//    }

//    if (viewportPos.y <= 0.0f || viewportPos.y >= 1.0f)
//    {
//        direction = Vector3.Reflect(direction, Vector3.up); // Reflect on y-axis
//    }
//}

//public class Test_Script : MonoBehaviour
//{
//    public Image Test_Image = null;
//    public Text Test_Text = null;
//    public Button Test_Button = null;

//    public Camera CC;

//    public void Start()
//    {
//        Test_Text.text = "해위";
//        Test_Button.onClick.AddListener(() => Debug.Log("fdzz"));
//        Test_Image.color = new Color(1, 0, 0, 1);

//        Test_Image = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();


//    }

//}

//public class Test_Script : MonoBehaviour
//{
//    public Image Test_Image;

//    public void Update()
//    {
//        Test_Image = Get_UI_Component(1, 0, 0, 0, 0).GetComponent<Image>();
//    }

//    public UnityEngine.Transform Get_UI_Component(int How_Many_Child, int Count_1, int Count_2, int Count_3, int Count_4) // How_Many_Child = GetChild를 몇번 할건지 // Count 1~4 번은 몇번째 자식인지
//    {
//        UnityEngine.Transform Temp = null;

//        switch (How_Many_Child)
//        {
//            case 0:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count_1);
//                }
//                break;
//            case 1:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count_1).GetChild(Count_2);
//                }
//                break;
//            case 2:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count_1).GetChild(Count_2).GetChild(Count_3);
//                }
//                break;
//            case 3:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count_1).GetChild(Count_2).GetChild(Count_3).GetChild(Count_4);
//                }
//                break;
//            default:
//                break;
//        }

//        return Temp;
//    }
//}