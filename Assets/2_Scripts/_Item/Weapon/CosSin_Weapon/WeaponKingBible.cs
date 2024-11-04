using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKingBible : Weapon
{
    public int WeaponKingBibleCount = 0;
    public int WeaponMineKingBibleCount = 0;
    public int WeaponMaxKingBibleCount = 0;

    public float bDeg = 0.0f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            KingBibleAttack();

            this.WeaponKnockBackWeapon();

            //this.WeaponTimer();
        }

        else 
        { 
            
        }

    }

    public void KingBibleAttack()
    {
        if (Mine == null) return;  // 코루틴 말고 해결책이 있었네? // Mine이 null이면 반환.

        bDeg = Time.time * WeaponSpeed;

        float C = WeaponMineKingBibleCount;
        float M = WeaponMaxKingBibleCount;
        float rad = Mathf.Deg2Rad * (bDeg + (C * (360f / M)));
        var x = WeaponRange * Mathf.Sin(rad);
        var y = WeaponRange * Mathf.Cos(rad);
        this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
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


}

#region
/*
for (int i = 0; i < WeaponKingBibleCount; i++){}
var Temp = AllManager.instance.GameModeM.CurrentPlayer.CurrentWeaponCount(WeaponType.KingBible, AllManager.instance.GameModeM.CurrentPlayer.KingBibleLAT);
var WeaponDataTemp = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);
int WeaponCountValue = 0;
WeaponCountValue = this.OriginStat.UnitWeaponProjCount + this.PercentStat.UnitWeaponProjCount;
int TempCount = WeaponDataTemp.Infos[pWLAT.Level].WeaponCount + WeaponCountValue;
for (int i = 0; i < Temp; i++)
{
float Temp = 6.28 / 현재 바이블 갯수
float Plus = Temp * WeaponKingBibleCount;
var rad = Mathf.Deg2Rad * (WeaponTime Plus) + Plus;
var KingBibleRoad = 6.28f / WeaponKingBibleCount;
var KingBiblePlus = KingBibleRoad * WeaponKingBibleCount;
var rad = Mathf.Deg2Rad * (WeaponTime + KingBiblePlus); 
Debug.Log("WeaponMineKingBibleCount : " + WeaponMineKingBibleCount);  // 여기서 문제가 발생한다. 즉 플레이어에서 바이블의 Max, Mine 카운트 증가가 안먹힌다는 소리다.
Debug.Log("WeaponMaxKingBibleCount : " + WeaponMaxKingBibleCount);
var rad = Mathf.Deg2Rad * (bDeg + (WeaponMineKingBibleCount * (360f / Temp)));
Debug.Log(bDeg);
float rad = (bDeg + (C * (360f / M)));
Debug.Log("KingBible rad : " + rad);
var rad = Mathf.Deg2Rad * (bDeg + (i * (360f / WeaponKingBibleCount)));
Debug.Log(this.name + WeaponRange);
Debug.Log(this.name + WeaponRange);
Debug.Log("this : " + this.transform.position);
var TempWeaponCount = AllManager.instance.GameModeM.CurrentPlayer.AllWeapon;
var TempPlayer = Mine.GetComponent<UnitPlayer>();
AllManager.instance.GameModeM.CurrentPlayer.AllWeapon[WeaponMineKingBibleCount].transform.position = Mine.transform.position + new Vector3(x, y, 0);
if(AllManager.instance.GameModeM.CurrentPlayer.AllWeapon[i].GetComponent<this>())
AllManager.instance.GameModeM.CurrentPlayer.AllWeapon[i].transform.position = Mine.transform.position + new Vector3(x, y, 0);
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
        others.pushback(new Rect({ -5000, -5000,0 }, { 100,100,1 }));

        // 각도들은 360도 / 객체 개수 * 객체 번호 순으로
        radians.pushback((360.0f / count) * i);
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

//WeaponTime += Time.deltaTime * WeaponSpeed;
//if (WeaponTime < 360)
//{
//    float x = WeaponRange * Mathf.Cos(WeaponTime);
//    float y = WeaponRange * Mathf.Sin(WeaponTime);
//    this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
//    // 오류 이유 Mine이 생성되기 전에 불러와서
//    // 해결책 내가 아는건 코루틴 
//}
//else
//{
//    WeaponTime = 0;
//}

//WeaponTime += Time.deltaTime * WeaponSpeed;
//if (WeaponTime < 360)
//{
//    if (Mine == null)   // 코루틴 말고 해결책이 있었네? // Mine이 null이면 꺼져.
//        return;

//    var AllWeaponCount = AllManager.instance.GameModeM.CurrentPlayer.AllWeapon.Count;

//    for (int i = 0; i < AllWeaponCount; i++)
//    {
//        var AllWeaponObject = AllManager.instance.GameModeM.CurrentPlayer.AllWeapon[i];

//        KingBible KingBibleWeapon = AllWeaponObject.GetComponent<KingBible>();
//        if (KingBibleWeapon != null)
//        {
//            var rad = Mathf.Deg2Rad * (WeaponTime + (i * (360 / AllWeaponCount)));
//            var x = WeaponRange * Mathf.Sin(rad);
//            var y = WeaponRange * Mathf.Cos(rad);
//            KingBibleWeapon.transform.position = Mine.transform.position + new Vector3(x, y);
//            KingBibleWeapon.transform.rotation = Quaternion.Euler(0, 0, (WeaponTime + (i * (360 / AllWeaponCount))) * -1);
//        }
//    }
//}
//else
//{
//    WeaponTime = 0;
//}
//public void KingBibleAttack()
//{
//    if (Mine == null) return;  // 코루틴 말고 해결책이 있었네? // Mine이 null이면 꺼져.

//    WeaponTime += Time.deltaTime * WeaponSpeed;
//    if (WeaponTime < 360)
//    {
//        for (int i = 0; i < WeaponKingBibleCount; i++)
//        {
//            var rad = Mathf.Deg2Rad * (WeaponTime + (i * (360 / WeaponKingBibleCount)));
//            var x = WeaponRange * Mathf.Sin(rad);
//            var y = WeaponRange * Mathf.Cos(rad);
//            this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
//            //this.transform.rotation = Quaternion.Euler(0, 0, (WeaponTime + (i * (360 / WeaponKingBibleCount))) * -1);
//        }
//        // this가 아니라 AllWeapon의 리스트에 있는 KingBible의 객체의 포지션과 회전값을 사용해야 한다.
//        // this로 하니까 그냥 작동이 안된다.
//        // 해결 내가 잘못하고 있었다 Player에서 int로 KingBibleCount를 만들어서 해결 근데 또다른 문제는 실행하면 간격이 이상하게 된다
//    }
//    else
//    {
//        WeaponTime = 0;
//    }
//}

/*
             //float Temp = 6.28 / 현재 바이블 갯수
            //float Plus = Temp * WeaponKingBibleCount;
            //var rad = Mathf.Deg2Rad * (WeaponTime Plus) + Plus;
            var rad = Mathf.Deg2Rad * (i * (360 / WeaponKingBibleCount) * WeaponTime);
            var x = Mathf.Sin(rad) * WeaponRange;
            var y = Mathf.Cos(rad) * WeaponRange;
            this.transform.position = Mine.transform.position + new Vector3(x, y, 0);
            //this.transform.rotation = Quaternion.Euler(0, 0, (WeaponTime + (i * (360 / WeaponKingBibleCount))) * -1);

            // this가 아니라 AllWeapon의 리스트에 있는 KingBible의 객체의 포지션과 회전값을 사용해야 한다.
            // this로 하니까 그냥 작동이 안된다.
            // 해결 내가 잘못하고 있었다 Player에서 int로 KingBibleCount를 만들어서 해결 근데 또다른 문제는 실행하면 간격이 이상하게 된다
 */
#endregion