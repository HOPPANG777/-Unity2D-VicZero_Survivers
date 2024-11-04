using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    #region
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
    #endregion

    public IEnumerator CTest()
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

    }

    private void Update()
    {

    }

}

#region

//var TempPlayer = AllManager.instance.GameModeM.CurrentPlayer;
//var Dis = 100.0f;

//var TempSpeed = UnitEnemySpeed;

//var Dir = this.transform.position - TempPlayer.transform.position;

//if (TempPlayer == null)
//{
//    return;
//}

//else
//{
//    if (AllManager.instance.GameModeM.EnemyList.Contains(this))
//    {
//        if (Dir.magnitude > Dis)
//        {
//            if (this.UnitType != UnitType.Boss)
//            {
//                //this.gameObject.SetActive(false);

//                //AllManager.instance.PoolM.EnemyPoolIn(this);

//                var Temp = (this.UnitEnemySpeed * 10);

//                UnitEnemySpeed = Temp;
//            }
//        }

//        else if (Dir.magnitude <= Dis)
//        {
//            if (this.UnitType != UnitType.Boss)
//            {
//                UnitEnemySpeed = (AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr(this.UnitType).UnitEnemySpeed * 1.5f);
//            }
//        }
//    }

//}

//public virtual void WeaponToEnemyCollision()
//{
//    var EnemyList = AllManager.instance.GameModeM.EnemyList;
//    var Weapon = this;

//    for (int i = 0; i < EnemyList.Count; i++)
//    {
//        Vector2 Dir = Weapon.transform.position - EnemyList[i].transform.position;
//        float Dis = Dir.magnitude;

//        var TempweaponTypeO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(weaponType);
//        var TempWALT = AllManager.instance.GameModeM.CurrentPlayer.ConvertWLATPlayer(TempweaponTypeO);

//        float MinDistance = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponRange(weaponType, TempWALT);

//        if (Dis < MinDistance)
//        {
//            //collision.transform.GetComponent<UnitEnemy>().Hitted(WeaponDamage);

//            UnitEnemy TempEnemy = EnemyList[i].transform.GetComponent<UnitEnemy>();

//            //Temp.Hitted(WeaponDamageValue);

//            TempEnemy.Hitted(WeaponDamage);

//            if (Forever == false)
//            {
//                if (CollisionCount > WeaponPen)
//                {
//                    DestroyWeapon();
//                }
//            }
//            else
//            {

//            }

//            //Debug.Log("Enemey Hitted");

//            CollisionCount++;
//        }
//    }


//}


//var Player = AllManager.instance.GameModeM.CurrentPlayer;
//var EnemyList = AllManager.instance.GameModeM.EnemyList;

//for (int i = 0; i < EnemyList.Count; i++)
//{
//    Vector2 Dir = Player.transform.position - EnemyList[i].transform.position;
//    float Dis = Dir.magnitude;

//    float MinDistance = 0.5f;

//    if (Dis < MinDistance)
//    {
//        if (AttackTime > WatingAttack)
//        {
//            //collision.transform.GetComponent<UnitPlayer>().Hitted(UnitEnemyDamage);

//            UnitPlayer Temp = Player.transform.GetComponent<UnitPlayer>();

//            if (this.UnitType == UnitType.Barrel)
//            {

//            }

//            else
//            {
//                Temp.Hitted(UnitEnemyDamage);
//            }

//            AttackTime = 0.0f;
//        }
//    }

//}

//public virtual void EnemyKnockback()
//{
//    var EnemyList = AllManager.instance.GameModeM.EnemyList;
//    Camera TempCamera = AllManager.instance.GameModeM.CurrentCamera;

//    if (this.UnitType == UnitType.Barrel)
//    {

//    }
//    else
//    {
//        for (int i = 0; i < EnemyList.Count; i++)
//        {
//            for (int j = i + 1; j < EnemyList.Count; j++)
//            {
//                Vector3 ViewPos = TempCamera.WorldToViewportPoint(EnemyList[j].transform.position);

//                if (ViewPos.x >= 0 && ViewPos.x <= 1 && ViewPos.y >= 0 && ViewPos.y <= 1 && ViewPos.z >= 0)
//                {
//                    // 화면 안에 있는 몬스터만 충돌 검사 실행
//                    Vector2 Dir = EnemyList[j].transform.position - EnemyList[i].transform.position;
//                    float Dis = Dir.magnitude;

//                    // 충돌 반경 설정 (적의 크기와 비례하여 설정)
//                    float MinDistance = 0.75f;

//                    if (Dis < MinDistance)
//                    {
//                        // 충돌이 발생한 경우 밀쳐내기
//                        Vector2 PushDirection = Dir.normalized * (MinDistance - Dis) * PushStrength;

//                        EnemyList[i].transform.position -= (Vector3)PushDirection * 0.5f;
//                        EnemyList[j].transform.position += (Vector3)PushDirection * 0.5f;
//                    }
//                }

//            }
//        }
//    }
//}
//TempTime += 10.0f * Time.time;
//TempTime02 += 4.0f * Time.time;

//if (TempTime < 600)
//{
//    Temp.GetComponent<RectTransform>().sizeDelta = new Vector2(100, TempTime);
//}

//if (TempTime02 < 250)
//{
//    Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, TempTime02);
//}
//public void OnTriggerEnter2D(Collider2D collision)
//{
//    // 몬스터끼리 충돌 시 밀려나게 하는 처리
//    if (this.isAlive == true)
//    {
//        if (collision.transform.GetComponent<Unit
//        >())
//        {
//            Vector3 OtherEnemyCollision = this.transform.position - collision.transform.position;
//            OtherEnemyCollision = OtherEnemyCollision.normalized * 0.1f;

//            this.transform.position = this.transform.position + OtherEnemyCollision;
//            //for (int i = 0; i < 100; i++)
//            //{
//            //    this.transform.position = this.transform.position + ((OtherEnemyCollision.normalized * 1f) * 0.005f * (100 - i) * Time.deltaTime);
//            //}
//        }
//    }

//    else
//    {

//    }
//}

//public void PongAttack()
//{
//    var Dist = (this.transform.position - Camera.main.transform.position).z;

//    var LeftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Dist)).x;
//    var RightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Dist)).x;
//    var TopBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Dist)).y;
//    var BottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Dist)).y;

//    Vector3 PongSize = this.gameObject.transform.position;

//    this.transform.position = new Vector3(

//    Mathf.Clamp(this.
//    transform.position.x,
//    LeftBorder + PongSize.x / 2,
//    RightBorder - PongSize.x / 2),
//    Mathf.Clamp
//    (this.transform.position.y,
//    TopBorder + PongSize.y / 2,
//    BottomBorder - PongSize.y / 2),
//    this.transform.position.z);

//}

/*
     public Image TestImage = null;

    public float FDZZ = 0.0046296296f;

    private void Update()
    {
        this.transform.position = ConvertRectTransformToPosition(TestImage.rectTransform);
    }

    public Vector3 ConvertRectTransformToPosition(RectTransform pRT)
    {
        //return new Vector3((pRT.position.x * 2), (pRT.position.y * 2 ), 0);

        return new Vector3((pRT.position.x * 2 - Screen.width) * FDZZ, (pRT.position.y * 2 - Screen.height) * FDZZ, 0);
        //return new Vector3((pRT.position.x * 2 - Screen.width), (pRT.position.y * 2 - Screen.height), 0);

    }
 */

/*
public class TestScript : MonoBehaviour
{
    public Image TestImage1;
    public GameObject Test1;
    public float TestTime1 = 0;

    public void Update()
    {

        TestTime1 += Time.deltaTime * 2;
        if (TestTime1 < 360)
        {
            float x = 1f * Mathf.Cos(TestTime1);
            float y = 1f * Mathf.Sin(TestTime1);
            TestImage1.rectTransform.position = Test1.transform.position - new Vector3(x, y, 0);
            // TestImage1.rectTransform.position = Test1.transform.position + new Vector3(Random.Range(0, x), Random.Range(0, y), 0);
            // 오류 이유 Mine이 생성되기 전에 불러와서
            // 해결책 내가 아는건 코루틴 
        }
        else
        {
            TestTime1 = 0;
        }

        TestImage1.rectTransform.position -= new Vector3(Random.Range(0, 10), Random.Range(0, 10), 0) * (0.005f);
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

//public Vector3 BezierTest(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4, float Value)
//{
//    Vector3 A = Vector3.Lerp(P1, P2, Value);
//    Vector3 B = Vector3.Lerp(P2, P3, Value);
//    Vector3 C = Vector3.Lerp(P3, P4, Value);

//    Vector3 D = Vector3.Lerp(A, B, Value);
//    Vector3 E = Vector3.Lerp(B, C, Value);

//    Vector3 F = Vector3.Lerp(D, E, Value);

//    return F;
//}

//public Image PeachoneRangeImage;

//public GameObject Test1;
//public GameObject Test2;

//public float PeachoneTime = 0;
//public float PeachoneSpeed = 2;
//public float PeachoneRange = 1;

//public void Update()
//{
//    PeachoneTime += Time.deltaTime * PeachoneSpeed;
//    if (PeachoneTime < 360)
//    {
//        float x = PeachoneRange * Mathf.Cos(PeachoneTime);
//        float y = PeachoneRange * Mathf.Sin(PeachoneTime);
//        PeachoneRangeImage.rectTransform.position = Test1.transform.position + new Vector3(x, y, 0);
//    }
//    else
//    {
//        PeachoneTime = 0;
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

//[SerializeField] public GameObject ShieldChasing;
//[SerializeField] public GameObject target;

//[SerializeField] public float spd;
//[SerializeField] public int shot = 12;

//public void Shot()
//{
//    StartCoroutine(CreateShieldChasing());
//}

//IEnumerator CreateShieldChasing()
//{
//    int shot = shot;
//    while (shot > 0)
//    {
//        shot--;
//        GameObject FireWand = Instantiate(ShieldChasing, transform);
//        FireWand.GetComponent<BezierShieldChasing>().master = gameObject;
//        FireWand.GetComponent<BezierShieldChasing>().enemy = target;

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
//public ParticleSystem Force;

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

//public class TestScript : MonoBehaviour
//{
//    public Image TestImage = null;
//    public Text TestText = null;
//    public Button TestButton = null;

//    public Camera CC;

//    public void Start()
//    {
//        TestText.text = "해위";
//        TestButton.onClick.AddListener(() => Debug.Log("fdzz"));
//        TestImage.color = new Color(1, 0, 0, 1);

//        TestImage = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();


//    }

//}

//public class TestScript : MonoBehaviour
//{
//    public Image TestImage;

//    public void Update()
//    {
//        TestImage = GetUIComponent(1, 0, 0, 0, 0).GetComponent<Image>();
//    }

//    public UnityEngine.Transform GetUIComponent(int HowManyChild, int Count1, int Count2, int Count3, int Count4) // HowManyChild = GetChild를 몇번 할건지 // Count 1~4 번은 몇번째 자식인지
//    {
//        UnityEngine.Transform Temp = null;

//        switch (HowManyChild)
//        {
//            case 0:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count1);
//                }
//                break;
//            case 1:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count1).GetChild(Count2);
//                }
//                break;
//            case 2:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count1).GetChild(Count2).GetChild(Count3);
//                }
//                break;
//            case 3:
//                {
//                    Temp = this.gameObject.transform.GetChild(Count1).GetChild(Count2).GetChild(Count3).GetChild(Count4);
//                }
//                break;
//            default:
//                break;
//        }

//        return Temp;
//    }
//}
#endregion