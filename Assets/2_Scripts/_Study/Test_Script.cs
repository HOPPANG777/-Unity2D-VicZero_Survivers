using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test_Script : MonoBehaviour
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

    }

    private void Update()
    {

    }

}

#region

//var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
//var Dis = 100.0f;

//var Temp_Speed = Unit_Enemy_Speed;

//var Dir = this.transform.position - Temp_Player.transform.position;

//if (Temp_Player == null)
//{
//    return;
//}

//else
//{
//    if (All_Manager.instance.GameMode_M.Enemy_List.Contains(this))
//    {
//        if (Dir.magnitude > Dis)
//        {
//            if (this.UT != Unit_Type.Boss)
//            {
//                //this.gameObject.SetActive(false);

//                //All_Manager.instance.Pool_M.Enemy_Pool_In(this);

//                var Temp = (this.Unit_Enemy_Speed * 10);

//                Unit_Enemy_Speed = Temp;
//            }
//        }

//        else if (Dir.magnitude <= Dis)
//        {
//            if (this.UT != Unit_Type.Boss)
//            {
//                Unit_Enemy_Speed = (All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(this.UT).Unit_Enemy_Speed * 1.5f);
//            }
//        }
//    }

//}

//public virtual void Weapon_To_Enemy_Collision()
//{
//    var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;
//    var Weapon = this;

//    for (int i = 0; i < Enemy_List.Count; i++)
//    {
//        Vector2 Dir = Weapon.transform.position - Enemy_List[i].transform.position;
//        float Dis = Dir.magnitude;

//        var Temp_WTO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(WT);
//        var Temp_WALT = All_Manager.instance.GameMode_M.Current_Player.Convert_WLAT_Player(Temp_WTO);

//        float Min_Distance = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Range(WT, Temp_WALT);

//        if (Dis < Min_Distance)
//        {
//            //collision.transform.GetComponent<Unit_Enemy>().Hitted(Weapon_Damage);

//            Unit_Enemy Temp_Enemy = Enemy_List[i].transform.GetComponent<Unit_Enemy>();

//            //Temp.Hitted(Weapon_Damage_Value);

//            Temp_Enemy.Hitted(Weapon_Damage);

//            if (Forever == false)
//            {
//                if (Collision_Count > Weapon_Pen)
//                {
//                    Destroy_Weapon();
//                }
//            }
//            else
//            {

//            }

//            //Debug.Log("Enemey Hitted");

//            Collision_Count++;
//        }
//    }


//}


//var Player = All_Manager.instance.GameMode_M.Current_Player;
//var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;

//for (int i = 0; i < Enemy_List.Count; i++)
//{
//    Vector2 Dir = Player.transform.position - Enemy_List[i].transform.position;
//    float Dis = Dir.magnitude;

//    float Min_Distance = 0.5f;

//    if (Dis < Min_Distance)
//    {
//        if (Attack_Time > Wating_Attack)
//        {
//            //collision.transform.GetComponent<Unit_Player>().Hitted(Unit_Enemy_Damage);

//            Unit_Player Temp = Player.transform.GetComponent<Unit_Player>();

//            if (this.UT == Unit_Type.Barrel)
//            {

//            }

//            else
//            {
//                Temp.Hitted(Unit_Enemy_Damage);
//            }

//            Attack_Time = 0.0f;
//        }
//    }

//}

//public virtual void Enemy_Knockback()
//{
//    var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;
//    Camera Temp_Camera = All_Manager.instance.GameMode_M.Current_Camera;

//    if (this.UT == Unit_Type.Barrel)
//    {

//    }
//    else
//    {
//        for (int i = 0; i < Enemy_List.Count; i++)
//        {
//            for (int j = i + 1; j < Enemy_List.Count; j++)
//            {
//                Vector3 View_Pos = Temp_Camera.WorldToViewportPoint(Enemy_List[j].transform.position);

//                if (View_Pos.x >= 0 && View_Pos.x <= 1 && View_Pos.y >= 0 && View_Pos.y <= 1 && View_Pos.z >= 0)
//                {
//                    // 화면 안에 있는 몬스터만 충돌 검사 실행
//                    Vector2 Dir = Enemy_List[j].transform.position - Enemy_List[i].transform.position;
//                    float Dis = Dir.magnitude;

//                    // 충돌 반경 설정 (적의 크기와 비례하여 설정)
//                    float Min_Distance = 0.75f;

//                    if (Dis < Min_Distance)
//                    {
//                        // 충돌이 발생한 경우 밀쳐내기
//                        Vector2 Push_Direction = Dir.normalized * (Min_Distance - Dis) * Push_Strength;

//                        Enemy_List[i].transform.position -= (Vector3)Push_Direction * 0.5f;
//                        Enemy_List[j].transform.position += (Vector3)Push_Direction * 0.5f;
//                    }
//                }

//            }
//        }
//    }
//}
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
#endregion


/// 유닛 플레이어 스크립트 주석들
///
/// 
/*
var Weapon_STO = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_Weapon.WT);

var Weapon_WLAT = Convert_WLAT_Player(Weapon_STO);

switch (p_Weapon.WeaponKeywords)
{
    case WeaponKeyword.NONE:
        break;
    case WeaponKeyword.Bound:
        {
            switch (p_Weapon.WT)
            {
                case Weapon_Type.Axe:
                    {
                        if (Axe_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                    }
                    break;
                case Weapon_Type.RuneTracer:
                    {
                        if (RuneTracer_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                    }
                    break;
            }
        }
        break;
    case WeaponKeyword.Phoenix:
        {
            switch (p_Weapon.WT)
            {
                case Weapon_Type.Peachone:
                    {
                        if (Peachone_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                    }
                    break;
                case Weapon_Type.FireWand:
                    {
                        if (FireWand_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                    }
                    break;
            }
        }
        break;
    case WeaponKeyword.Paladin:
        {
            switch (p_Weapon.WT)
            {
                case Weapon_Type.ShadowPinion:
                    {
                        if (ShadowPinion_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                    }
                    break;
                case Weapon_Type.Cross:
                    {
                        if (Cross_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                    }
                    break;
            }
        }
        break;
    case WeaponKeyword.Sonic:
        switch (p_Weapon.WT)
        {
            case Weapon_Type.LightningRing:
                {
                    if (LightningRing_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                }
                break;
            case Weapon_Type.Whip:
                {
                    if (Whip_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                }
                break;
        }
        break;
    case WeaponKeyword.Holy:
        switch (p_Weapon.WT)
        {
            case Weapon_Type.Garlic:
                {
                    if (Garlic_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                }
                break;
            case Weapon_Type.KingBible:
                {
                    if (KingBible_LAT.Level > 0) weaponKeywardList.Add(p_Weapon.WeaponKeywords);
                }
                break;
        }
        break;
}
*/

//if (Dis > Vector3.Distance(this.transform.position, Gem_Temp.transform.position))
//{
//    Dis = Vector3.Distance(this.transform.position, Gem_Temp.transform.position);
//    Target_Gem = Gem_Temp;
//}
//if (Gem_Temp == null) return;

//var Temp_Equipment = 

//if (p_WT == Weapon_Type.Axe)
//{
//    Axe_LAT.Level++;

//    for (int i = 0; i < All_Weapon.Count; i++)
//    {
//        if (All_Weapon[i].GetComponent<Weapon_Axe>())
//        {
//            All_Weapon[i].Upgrade(Axe_LAT.Level);
//        }
//    }

//}
//if (InputVec.x <= 0)
//{
//    Player_Right = false;
//}

//if (Player_Right == true)
//{
//    The_Scale.x *= -1;
//}

//Vector3 The_Scale = this.transform.localScale;
//The_Scale.x *= -1;
//this.transform.localScale = The_Scale;
//if (Target_Gem == null) return;

//if (Target_Gem != null)
//{
//    Debug.Log("Gem Not Null");

//    //���� �ٲٰ�
//    Vector2 Dir = (this.transform.position - Target_Gem.transform.position).normalized;

//    //float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
//    //Target_Gem.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

//    //Target_Gem.transform.position = Target_Gem.transform.position + Target_Gem.transform.right * (5.0f * Time.deltaTime);
//    var Gem_Dir = Dir * 0.1f;

//    Target_Gem.Gem_To_Player(Gem_Dir);
//}
//else
//{
//    Debug.Log("Gem Null");
//}

// �� ������ ���ؼ� ������

//if (All_Manager.instance != null && All_Manager.instance.GameMode_M != null && All_Manager.instance.GameMode_M.Gem_List != null && All_Manager.instance.GameMode_M.Gem_List.Count > 0)
//{
//    int Count_To_Gem_Spawn = All_Manager.instance.GameMode_M.Enemy_List.Count;

//    List<int> Spawned_Gem_Indices = new List<int>();

//    for (int i = 0; i < Count_To_Gem_Spawn; i++)
//    {

//        if (Spawned_Gem_Indices.Contains(i))
//        {

//        }
//        else
//        {
//            continue;
//        }

//        Spawned_Gem_Indices.Add(i);

//        var Check_Null_Gem = All_Manager.instance.GameMode_M.Gem_List[i];

//        if (Check_Null_Gem == null)
//        {
//            continue;
//        }

//        var Gem_trasform = All_Manager.instance.GameMode_M.Gem_List[i].transform;

//        if (Gem_trasform != null)
//        {
//            Debug.Log("Gem Not Null");
//            Vector2 Dir = this.transform.position - Gem_trasform.position;
//            float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
//            Gem_trasform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
//        }
//        else
//        {
//            Debug.Log("Gem NULL");
//        }

//        Gem_trasform.position = Gem_trasform.position + Gem_trasform.right * (5.0f * Time.deltaTime);

//      }


//}

//int Create_KingBible_Count = 0;

//for (int i = 0; i < All_Weapon.Count; i++)
//{
//    if (All_Weapon[i] == GetComponent<Weapon_KingBible>())
//    {
//        if (All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count < Current_Weapon_Count(Weapon_Type.KingBible, KingBible_LAT))
//        {
//            All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count++;
//        }

//    }
//}


// ���⼭ All_Weapon�� �ִ� ��� ������ �ٽ� ����
// All_Weapon.GetComponent<KingBible>().Max = 3;
// All_Weapon.GetComponent<KingBible>().Current = 3;
//
// 1 2 3

//var KingBibles = this.GetComponentsInChildren<Weapon_KingBible>();
//KingBibles.Length

//int Player_Weapon_KingBible_Count = 0;

//for (int i = 0; i < All_Weapon.Count; i++)
//{
//    if (All_Weapon[i].GetComponent<Weapon_KingBible>())
//    {
//        All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count++;

//        if (All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Mine_KingBible_Count > All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count)
//        {
//            All_Weapon[i].GetComponent<Weapon_KingBible>().Weapon_Max_KingBible_Count++;
//        }
//    }

//}

//Debug.Log("Player_Weapon_KingBible_Count : " + Player_Weapon_KingBible_Count);


//float xInput = Input.GetAxis("Horizontal");
//float yInput = Input.GetAxis("Vertical");

//float xSpeed = xInput * Xspeed * Time.deltaTime;
//float ySpeed = yInput * Yspeed * Time.deltaTime;

//�� �����ΰ���
// this.transform.position = this.transform.position + (this.transform.up * ySpeed);

//����
// this.transform.Rotate(new Vector3(0, 0, -xSpeed));

//if (Skill_Full < Skill_Cool)
//{

//    Skill_Full += Time.deltaTime;
//    All_Manager.instance.UI_M.Update_Player_Skill_Image(Skill_Full / Skill_Cool);
//    Temp_Go = Instantiate(Garlic_Go, null);
//    Temp_Go.transform.parent = this.transform;
//    if (Temp_Go.GetComponent<Garlic>())
//    {
//        yield return new WaitUntil(() => Temp_Go.GetComponent<Garlic>().SR);
//        All_Manager.instance.Resource_M.Weapon_Stat(Temp_Go.GetComponent<Garlic>(), All_Manager.instance.Resource_M.STO_Garlic);
//    }
//}
//else
//{
//    Skill_Full = 0.0f;
//}

//switch (p_WT)
//{
//    case Weapon_Type.None:
//        {
//        }
//        break;
//    case Weapon_Type.KingBible:
//        {
//            //Temp_Go = Instantiate(Weapon_Go, null);
//            GameObject Weapon_Go = new GameObject("KingBible");
//            Weapon_Go.AddComponent<KingBible>();
//            Weapon_Go.transform.parent = this.transform;
//            if (Weapon_Go.GetComponent<KingBible>())
//            {
//                yield return new WaitUntil(() => Weapon_Go.GetComponent<KingBible>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(Weapon_Go.GetComponent<KingBible>(), All_Manager.instance.Resource_M.STO_KingBible);
//            }
//            if (Weapon_Go.GetComponent<Weapon>())
//            {
//                Weapon_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.SpellBook:
//        {
//            //Temp_Go = Instantiate(SpellBook_Go, null);
//            GameObject SpellBook_Go = new GameObject("SpellBook");
//            SpellBook_Go.AddComponent<SpellBook>();
//            SpellBook_Go.transform.parent = this.transform;
//            if (SpellBook_Go.GetComponent<SpellBook>())
//            {
//                yield return new WaitUntil(() => SpellBook_Go.GetComponent<SpellBook>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(SpellBook_Go.GetComponent<SpellBook>(), All_Manager.instance.Resource_M.STO_SpellBook);
//            }
//            if (SpellBook_Go.GetComponent<Weapon>())
//            {
//                SpellBook_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.FireWand_Launcher:
//        {
//            //Temp_Go = Instantiate(FireWand_Launcher_Go, null);
//            GameObject FireWand_Launcher_Go = new GameObject("FireWand_Launcher");
//            FireWand_Launcher_Go.AddComponent<FireWand_Launcher>();
//            FireWand_Launcher_Go.transform.parent = this.transform;
//            if (FireWand_Launcher_Go.GetComponent<FireWand_Launcher>())
//            {
//                yield return new WaitUntil(() => FireWand_Launcher_Go.GetComponent<FireWand_Launcher>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(FireWand_Launcher_Go.GetComponent<FireWand_Launcher>(), All_Manager.instance.Resource_M.STO_FireWand_Launcher);
//            }
//            if (FireWand_Launcher_Go.GetComponent<Weapon>())
//            {
//                FireWand_Launcher_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.ShadowPinion_Launcher:
//        {
//            //Temp_Go = Instantiate(ShadowPinion_Launcher_Go, null);
//            GameObject ShadowPinion_Launcher_Go = new GameObject("ShadowPinion_Launcher");
//            ShadowPinion_Launcher_Go.AddComponent<ShadowPinion_Launcher>();
//            ShadowPinion_Launcher_Go.transform.parent = this.transform;
//            if (ShadowPinion_Launcher_Go.GetComponent<ShadowPinion_Launcher>())
//            {
//                yield return new WaitUntil(() => ShadowPinion_Launcher_Go.GetComponent<ShadowPinion_Launcher>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(ShadowPinion_Launcher_Go.GetComponent<ShadowPinion_Launcher>(), All_Manager.instance.Resource_M.STO_ShadowPinion_Launcher);
//            }
//            if (ShadowPinion_Launcher_Go.GetComponent<Weapon>())
//            {
//                ShadowPinion_Launcher_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    case Weapon_Type.Garlic:
//        {
//            //Temp_Go = Instantiate(Garlic_Go, null);
//            GameObject Garlic_Go = new GameObject("Garlic");
//            Garlic_Go.AddComponent<Garlic>();
//            Garlic_Go.transform.parent = this.transform;
//            if (Garlic_Go.GetComponent<Garlic>())
//            {
//                yield return new WaitUntil(() => Garlic_Go.GetComponent<Garlic>().SR);
//                All_Manager.instance.Resource_M.Weapon_Stat(Garlic_Go.GetComponent<Garlic>(), All_Manager.instance.Resource_M.STO_Garlic);
//            }
//            if (Garlic_Go.GetComponent<Weapon>())
//            {
//                Garlic_Go.GetComponent<Weapon>().Weapon_Init(this);
//            }
//        }
//        break;
//    default:
//        break;
//}

//public IEnumerator C_Create_Weapon(Weapon_Type p_PT)
//{
//    yield return new WaitWhile(() => Creating_Weapon == true);

//    Creating_Weapon = true;

//    GameObject Weapon_Go = null;

//    switch (p_PT)
//    {
//        case Weapon_Type.None:
//            break;
//        case Weapon_Type.KingBible:
//            {
//                Weapon_Go = new GameObject("KingBible");

//                Weapon_Go.AddComponent<Weapon_KingBible>();

//                Weapon_Go.transform.parent = this.transform;
//                if (Weapon_Go.GetComponent<Weapon_KingBible>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_KingBible>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_KingBible, KingBible_LAT.Level);

//                    int Create_KingBible_Count = 1;

//                    for (int i = 0; i < All_Weapon.Count; i++)
//                    {
//                        if (All_Weapon[i].GetComponent<Weapon_KingBible>())
//                        {
//                            Create_KingBible_Count++;
//                        }
//                    }

//                    Weapon_Go.GetComponent<Weapon_KingBible>().Weapon_KingBible_Count = Create_KingBible_Count;
//                }
//            }
//            break;
//        case Weapon_Type.Garlic:
//            {
//                Weapon_Go = new GameObject("Garlic");

//                Weapon_Go.AddComponent<Weapon_Garlic>();

//                if (Weapon_Go.GetComponent<Weapon_Garlic>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_Garlic>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_Garlic, Garlic_LAT.Level);
//                }
//            }
//            break;
//        case Weapon_Type.LightningRing:
//            {
//                if (All_Manager.instance.GameMode_M.Enemy_List.Count > 10)
//                {

//                    for (int i = 0; i < 10; i++)
//                    {
//                        GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//                        Weapon_Go = new GameObject("LightningRing");
//                        Weapon_Go.AddComponent<Weapon_LightningRing>();

//                        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//                        Weapon_Go.transform.rotation = Quaternion.identity;

//                        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//                        {
//                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                        }

//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < All_Manager.instance.GameMode_M.Enemy_List.Count; i++)
//                    {
//                        GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//                        Weapon_Go = new GameObject("LightningRing");
//                        Weapon_Go.AddComponent<Weapon_LightningRing>();

//                        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//                        Weapon_Go.transform.rotation = Quaternion.identity;

//                        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//                        {
//                            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//                            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                        }

//                        Destroy(Weapon_Go, 2.5f);
//                    }
//                }

//            }
//            break;
//        case Weapon_Type.FireWand:
//            {
//                Weapon_Go = new GameObject("FireWand");

//                Weapon_Go.AddComponent<Weapon_FireWand>();

//                if (Weapon_Go.GetComponent<Weapon_FireWand>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_FireWand>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_FireWand, FireWand_LAT.Level);
//                }
//            }
//            break;
//        case Weapon_Type.ShadowPinion:
//            {
//                Weapon_Go = new GameObject("ShadowPinion");

//                Weapon_Go.AddComponent<Weapon_ShadowPinion>();

//                if (Weapon_Go.GetComponent<Weapon_ShadowPinion>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_ShadowPinion>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_ShadowPinion, ShadowPinion_LAT.Level);
//                }
//            }
//            break;
//        case Weapon_Type.Axe:
//            {
//                Weapon_Go = new GameObject("Axe");

//                Weapon_Go.AddComponent<Axe>();



//                if (Weapon_Go.GetComponent<Axe>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Axe>().SR);
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_Axe, Axe_LAT.Level);
//                    Vector3 One = this.transform.position;
//                    Vector3 Two = One + new Vector3(0, 5, 0); // ��
//                    Vector3 Three = Two + new Vector3(5, 0, 0); //������
//                    Vector3 Four = One + new Vector3(5, -10, 0); // ������ �Ʒ�


//                    Weapon_Go.GetComponent<Axe>().Axe_Init(One, Two, Three, Four);
//                }

//            }
//            break;
//        default:
//            break;
//    }

//    All_Weapon.Add(Weapon_Go.GetComponent<Weapon>());

//    Creating_Weapon = false;
//}

//if (All_Manager.instance.GameMode_M.Enemy_List.Count > 10)
//{

//    for (int i = 0; i < 10; i++)
//    {
//        //GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//        Weapon_Go = new GameObject("LightningRing");
//        Weapon_Go.AddComponent<Weapon_LightningRing>();

//        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//        Weapon_Go.transform.rotation = Quaternion.identity;
//        Weapon_Go.transform.localScale = Vector3.one;

//        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//        {
//            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//        }

//    }
//}
//else
//{
//    for (int i = 0; i < All_Manager.instance.GameMode_M.Enemy_List.Count; i++)
//    {
//        //GameObject Temp_Go = Instantiate(Weapon_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);
//        Weapon_Go = new GameObject("LightningRing");
//        Weapon_Go.AddComponent<Weapon_LightningRing>();

//        Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[i].transform.position;
//        Weapon_Go.transform.rotation = Quaternion.identity;
//        Weapon_Go.transform.localScale = Vector3.one;

//        if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//        {
//            yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//            Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//        }

//        Destroy(Weapon_Go, 2.5f);
//    }
//}

//public void Get_Equipment(Equipment_Type p_ET)
//{
//    StartCoroutine(C_Get_Equipment(p_ET));
//}

//public IEnumerator C_Get_Equipment(Equipment_Type p_ET)
//{
//    // ���� Temp�� ���̿��� �߻��ϴ� �����ΰ����� ���δ�.
//    // �ϴ� �� ���� ����
//    // ���� �ϴ� �ٽ� �ѹ�
//    yield return new WaitWhile(() => Creating_Equipment == true);

//    var Temp = All_Manager.instance.Resource_M.Convert_Equipment_Type_To_STO_Equipment(p_ET);

//    Creating_Equipment = true;

//    if (p_ET == Equipment_Type.None)
//    {

//    }

//    if (p_ET == Equipment_Type.HollowHeart)
//    {
//        yield return new WaitUntil(() => (Temp = All_Manager.instance.Resource_M.STO_Equipment_HollowHeart));
//        if (Temp == All_Manager.instance.Resource_M.STO_Equipment_HollowHeart)
//        {
//            HollowHeart_Level++;

//            Get_Equipment_Player_Percentage_Stat(this, Temp);
//            Debug.Log(Temp);
//            Debug.Log("��ű� ����");
//        }
//    }

//    if (p_ET == Equipment_Type.Wings)
//    {
//        Wings_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Armour)
//    {
//        Armour_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Spinach)
//    {
//        Spinach_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Bracer)
//    {
//        Bracer_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.SpellBinder)
//    {
//        SpellBinder_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Candelabrador)
//    {
//        Candelabrador_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.EmptyTome)
//    {
//        EmptyTome_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Duplicator)
//    {
//        Duplicator_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (p_ET == Equipment_Type.Attractorb)
//    {
//        Attractorb_Level++;

//        Get_Equipment_Player_Percentage_Stat(this, Temp);
//    }

//    if (Temp != null)
//    {
//        All_Equipment.Add(Temp);
//    }

//    Creating_Equipment = false;
//}