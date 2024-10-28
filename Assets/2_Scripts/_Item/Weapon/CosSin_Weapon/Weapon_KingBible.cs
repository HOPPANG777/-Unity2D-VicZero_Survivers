using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_KingBible : Weapon
{
    public int Weapon_KingBible_Count = 0;
    public int Weapon_Mine_KingBible_Count = 0;
    public int Weapon_Max_KingBible_Count = 0;

    public float b_Deg = 0.0f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            KingBible_Attack();

            this.Weapon_KnockBack_Weapon();

            //this.Weapon_Timer();
        }

        else 
        { 
            
        }

    }

    public void KingBible_Attack()
    {
        if (Mine == null) return;  // 코루틴 말고 해결책이 있었네? // Mine이 null이면 반환.

        b_Deg = Time.time * Weapon_Speed;

        float C = Weapon_Mine_KingBible_Count;
        float M = Weapon_Max_KingBible_Count;
        float rad = Mathf.Deg2Rad * (b_Deg + (C * (360f / M)));
        var x = Weapon_Range * Mathf.Sin(rad);
        var y = Weapon_Range * Mathf.Cos(rad);
        this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
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
for (int i = 0; i < Weapon_KingBible_Count; i++){}
var Temp = All_Manager.instance.GameMode_M.Current_Player.Current_Weapon_Count(Weapon_Type.KingBible, All_Manager.instance.GameMode_M.Current_Player.KingBible_LAT);
var STO_Weapon_Temp = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(p_WT);
int Weapon_Count_Value = 0;
Weapon_Count_Value = this.Origin_Stat.Unit_Weapon_Proj_Count + this.Percent_Stat.Unit_Weapon_Proj_Count;
int Temp_Count = STO_Weapon_Temp.Info_s[p_WLAT.Level].Weapon_Count + Weapon_Count_Value;
for (int i = 0; i < Temp; i++)
{
float Temp = 6.28 / 현재 바이블 갯수
float Plus = Temp * Weapon_KingBible_Count;
var rad = Mathf.Deg2Rad * (Weapon_Time Plus) + Plus;
var KingBible_Road = 6.28f / Weapon_KingBible_Count;
var KingBible_Plus = KingBible_Road * Weapon_KingBible_Count;
var rad = Mathf.Deg2Rad * (Weapon_Time + KingBible_Plus); 
Debug.Log("Weapon_Mine_KingBible_Count : " + Weapon_Mine_KingBible_Count);  // 여기서 문제가 발생한다. 즉 플레이어에서 바이블의 Max, Mine 카운트 증가가 안먹힌다는 소리다.
Debug.Log("Weapon_Max_KingBible_Count : " + Weapon_Max_KingBible_Count);
var rad = Mathf.Deg2Rad * (b_Deg + (Weapon_Mine_KingBible_Count * (360f / Temp)));
Debug.Log(b_Deg);
float rad = (b_Deg + (C * (360f / M)));
Debug.Log("KingBible rad : " + rad);
var rad = Mathf.Deg2Rad * (b_Deg + (i * (360f / Weapon_KingBible_Count)));
Debug.Log(this.name + Weapon_Range);
Debug.Log(this.name + Weapon_Range);
Debug.Log("this : " + this.transform.position);
var Temp_Weapon_Count = All_Manager.instance.GameMode_M.Current_Player.All_Weapon;
var Temp_Player = Mine.GetComponent<Unit_Player>();
All_Manager.instance.GameMode_M.Current_Player.All_Weapon[Weapon_Mine_KingBible_Count].transform.position = Mine.transform.position + new Vector3(x, y, 0);
if(All_Manager.instance.GameMode_M.Current_Player.All_Weapon[i].GetComponent<this>())
All_Manager.instance.GameMode_M.Current_Player.All_Weapon[i].transform.position = Mine.transform.position + new Vector3(x, y, 0);
*/
/*
 // 변수들
{
    int count = 5;              // 공회전할 객체 개수
    float distance = 300.0f;    // 중심에서 부터의 거리
    vector<Rect*> others;       // 공회전할 객체
    vector<float> radians;      // 공회전할 객체들의 각도 값
}

// 생성
{
    for (int i = 0; i < count; i++)
    {
        // 객체는 일단 아무렇게나 생성해도 됨
        others.push_back(new Rect({ -5000, -5000,0 }, { 100,100,1 }));

        // 각도들은 360도 / 객체 개수 * 객체 번호 순으로
        radians.push_back((360.0f / count) * i);
    }
}

// 업데이트
{
    for (int i = 0; i < count; i++)
    {
        // 각도 360넘어가는거 방지
        if (radians[i] >= 360.0f) radians[i] = 0.0f;
        radians[i] += 2.0f; // 회전값 // 커지면 빨리 돔

        // 각도 값을 라디안 값으로 변환 (각도값 * 3.14(파이값) / 180 하면 됨)
        float radian = D3DXToRadian(radians[i]);

        // x좌표 = 중심 위치 x 값 + 중심에서의 거리 * cos(라디안값)
        float x = player->GetPosition().x + distance * cos(radian);
        // y좌표 = 중심 위치 y 값 + 중심에서의 거리 * sin(라디안값)
        float y = player->GetPosition().y + distance * sin(radian);

        // 객체 좌표 지정
        others[i]->SetPosition({ x,y,player->GetPosition().z });
    }
}
*/

//time += Time.deltaTime * speed;
//if (time < 360)
//{
//    for (int i = 0; i < level; i++)
//    {
//        float rad = Mathf.Deg2Rad * (time + (i * (360 / level)));
//        float x = range * Mathf.Sin(rad);
//        float y = range * Mathf.Cos(rad);
//        this.transform.localPosition = target.position + new Vector3(x, y);
//        this.transform.rotation = Quaternion.Euler(0, 0, (time + (i * (360 / level))) * -1); //가운데를 바라보게 각도 조절
//    }
//}
//else
//{
//    time = 0;
//}

//if (Mine == null)   // 코루틴 말고 해결책이 있었네? // Mine이 null이면 꺼져.
//    return;

//Weapon_Time += Time.deltaTime * Weapon_Speed;
//if (Weapon_Time < 360)
//{
//    float x = Weapon_Range * Mathf.Cos(Weapon_Time);
//    float y = Weapon_Range * Mathf.Sin(Weapon_Time);
//    this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
//    // 오류 이유 Mine이 생성되기 전에 불러와서
//    // 해결책 내가 아는건 코루틴 
//}
//else
//{
//    Weapon_Time = 0;
//}

//Weapon_Time += Time.deltaTime * Weapon_Speed;
//if (Weapon_Time < 360)
//{
//    if (Mine == null)   // 코루틴 말고 해결책이 있었네? // Mine이 null이면 꺼져.
//        return;

//    var All_Weapon_Count = All_Manager.instance.GameMode_M.Current_Player.All_Weapon.Count;

//    for (int i = 0; i < All_Weapon_Count; i++)
//    {
//        var All_Weapon_Object = All_Manager.instance.GameMode_M.Current_Player.All_Weapon[i];

//        KingBible KingBible_Weapon = All_Weapon_Object.GetComponent<KingBible>();
//        if (KingBible_Weapon != null)
//        {
//            var rad = Mathf.Deg2Rad * (Weapon_Time + (i * (360 / All_Weapon_Count)));
//            var x = Weapon_Range * Mathf.Sin(rad);
//            var y = Weapon_Range * Mathf.Cos(rad);
//            KingBible_Weapon.transform.position = Mine.transform.position + new Vector3(x, y);
//            KingBible_Weapon.transform.rotation = Quaternion.Euler(0, 0, (Weapon_Time + (i * (360 / All_Weapon_Count))) * -1);
//        }
//    }
//}
//else
//{
//    Weapon_Time = 0;
//}
//public void KingBible_Attack()
//{
//    if (Mine == null) return;  // 코루틴 말고 해결책이 있었네? // Mine이 null이면 꺼져.

//    Weapon_Time += Time.deltaTime * Weapon_Speed;
//    if (Weapon_Time < 360)
//    {
//        for (int i = 0; i < Weapon_KingBible_Count; i++)
//        {
//            var rad = Mathf.Deg2Rad * (Weapon_Time + (i * (360 / Weapon_KingBible_Count)));
//            var x = Weapon_Range * Mathf.Sin(rad);
//            var y = Weapon_Range * Mathf.Cos(rad);
//            this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
//            //this.transform.rotation = Quaternion.Euler(0, 0, (Weapon_Time + (i * (360 / Weapon_KingBible_Count))) * -1);
//        }
//        // this가 아니라 All_Weapon의 리스트에 있는 KingBible의 객체의 포지션과 회전값을 사용해야 한다.
//        // this로 하니까 그냥 작동이 안된다.
//        // 해결 내가 잘못하고 있었다 Player에서 int로 KingBible_Count를 만들어서 해결 근데 또다른 문제는 실행하면 간격이 이상하게 된다
//    }
//    else
//    {
//        Weapon_Time = 0;
//    }
//}

/*
             //float Temp = 6.28 / 현재 바이블 갯수
            //float Plus = Temp * Weapon_KingBible_Count;
            //var rad = Mathf.Deg2Rad * (Weapon_Time Plus) + Plus;
            var rad = Mathf.Deg2Rad * (i * (360 / Weapon_KingBible_Count) * Weapon_Time);
            var x = Mathf.Sin(rad) * Weapon_Range;
            var y = Mathf.Cos(rad) * Weapon_Range;
            this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
            //this.transform.rotation = Quaternion.Euler(0, 0, (Weapon_Time + (i * (360 / Weapon_KingBible_Count))) * -1);

            // this가 아니라 All_Weapon의 리스트에 있는 KingBible의 객체의 포지션과 회전값을 사용해야 한다.
            // this로 하니까 그냥 작동이 안된다.
            // 해결 내가 잘못하고 있었다 Player에서 int로 KingBible_Count를 만들어서 해결 근데 또다른 문제는 실행하면 간격이 이상하게 된다
 */
#endregion