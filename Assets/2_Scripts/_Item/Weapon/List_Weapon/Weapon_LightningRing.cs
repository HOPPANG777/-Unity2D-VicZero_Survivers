using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Weapon_LightningRing : Weapon
{
    public Unit_Enemy LightningRing_target;

    public Vector2 BoxSize = new Vector2(2, 2);

    public int Enemy_Count = 0;

    public override void Start()
    {
        base.Start();
        Forever = true;
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            this.Weapon_Timer();
        }

        else
        {

        }


    }

    public override void Upgrade(int p_Level)
    {

    }

    public override void Weapon_Stat_Appliaction()
    {
        base.Weapon_Stat_Appliaction();
    }

    public void LightningRing_Attack()
    {



    }

    public override void Weapon_To_Enemy_Collision()
    {
        base.Weapon_To_Enemy_Collision();
    }



}

#region
//public class SpellBook : Weapon
//{
//    public GameObject LightningRing_Go = null;

//    public Vector2 BoxSize = new Vector2(3, 3);

//    public Enemy M_target = null;

//    public override void Start()
//    {
//        base.Start();
//        Magic();
//        //Spell();
//    }

//    public override void Update()
//    {
//        // Magic();
//    }

//    public override void OnTriggerStay2D(Collider2D collision)
//    {
//        base.OnTriggerStay2D(collision);
//    }

//    public void Spell()
//    {
//        StartCoroutine(C_Spell());
//    }

//    public IEnumerator C_Spell()
//    {
//        float Attack_Rate = 10.0f;

//        while (true)
//        {
//            if (All_Manager.instance.GameMode_M.Enemy_List.Count > 5)
//            {

//                for (int i = 0; i < 5; i++)
//                {
//                    GameObject Temp_Go = Instantiate(LightningRing_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);

//                    if (Temp_Go.GetComponent<LightningRing>())
//                    {
//                        yield return new WaitUntil(() => Temp_Go.GetComponent<LightningRing>().SR);
//                        All_Manager.instance.Resource_M.Weapon_Stat(Temp_Go.GetComponent<LightningRing>(), All_Manager.instance.Resource_M.STO_LightningRing);
//                    }
//                }
//            }
//            else
//            {
//                for (int i = 0; i < All_Manager.instance.GameMode_M.Enemy_List.Count; i++)
//                {
//                    GameObject Temp_Go = Instantiate(LightningRing_Go, All_Manager.instance.GameMode_M.Enemy_List[i].transform.position, Quaternion.identity);

//                    if (Temp_Go.GetComponent<LightningRing>())
//                    {
//                        yield return new WaitUntil(() => Temp_Go.GetComponent<LightningRing>().SR);
//                        All_Manager.instance.Resource_M.Weapon_Stat(Temp_Go.GetComponent<LightningRing>(), All_Manager.instance.Resource_M.STO_LightningRing);
//                    }

//                    Destroy(Temp_Go, 2.5f);
//                }
//            }


//            yield return new WaitForSeconds(Attack_Rate);
//        }


//    }


//    public void Magic()
//    {
//        var collider2Ds = Physics2D.OverlapBoxAll(this.transform.position, BoxSize, 0);

//        float Dis = 10000f;

//        foreach (var collider in collider2Ds)
//        {
//            if (collider.gameObject.GetComponent<Enemy>())
//            {
//                var Enemy_Temp = collider.gameObject.GetComponent<Enemy>();

//                if (Dis > Vector3.Distance(this.transform.position, Enemy_Temp.transform.position))
//                {
//                    Dis = Vector3.Distance(this.transform.position, Enemy_Temp.transform.position);
//                    M_target = Enemy_Temp;
//                }
//            }
//        }

//        if (M_target != null)
//        {
//            Debug.Log("LNNULL");
//            GameObject Temp_Go = Instantiate(LightningRing_Go, M_target.transform.position, Quaternion.identity);

//            if (Temp_Go.GetComponent<LightningRing>())
//            {
//                if (Temp_Go.GetComponent<LightningRing>().SR == null)
//                    return;
//                All_Manager.instance.Resource_M.Weapon_Stat(Temp_Go.GetComponent<LightningRing>(), All_Manager.instance.Resource_M.STO_LightningRing);
//            }


//            Destroy(Temp_Go.gameObject, 2.5f);
//        }

//    }

//}

//        case Weapon_Type.LightningRing:
//{


//    int Max_Count = All_Manager.instance.Resource_M.STO_LightningRing.Info_s[LightningRing_LAT.Level].Weapon_Count;
//    int Count_To_Spawn = Mathf.Min(Max_Count, All_Manager.instance.GameMode_M.Enemy_List.Count); // 최대 10개까지만 생성하도록 제한

//    List<int> Spawned_Indices = new List<int>(); // 이미 생성된 적의 인덱스를 저장할 리스트

//    if (All_Manager.instance.GameMode_M.Enemy_List.Count > Max_Count)
//    {
//        for (int i = 0; i < Max_Count; i++)
//        {
//            int Random_Index = Random.Range(0, All_Manager.instance.GameMode_M.Enemy_List.Count);

//            // 중첩 방지: 이미 생성된 적 위치에 LightningRing이 있으면 건너뜀
//            if (Spawned_Indices.Contains(Random_Index))
//            {
//                continue;
//            }

//            Spawned_Indices.Add(Random_Index);

//            var Check_Enemy_Alive = All_Manager.instance.GameMode_M.Enemy_List[Random_Index];

//            if (Check_Enemy_Alive != null)
//            {

//                Weapon_Go = new GameObject("LightningRing");
//                Weapon_Go.AddComponent<Weapon_LightningRing>();

//                Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[Random_Index].transform.position;
//                Weapon_Go.transform.rotation = Quaternion.identity;
//                Weapon_Go.transform.localScale = Vector3.one;

//                // LightningRing이 데미지를 주는지 확인 후 처리
//                if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//                    // Weapon_Init을 통해 필요한 초기화 작업 수행
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                }

//            }
//            Destroy(Weapon_Go, 2.5f); // 일정 시간 후 삭제
//        }
//    }

//    else
//    {
//        for (int j = 0; j < All_Manager.instance.GameMode_M.Enemy_List.Count; j++)
//        {
//            int Random_Index = Random.Range(0, All_Manager.instance.GameMode_M.Enemy_List.Count);

//            // 중첩 방지: 이미 생성된 적 위치에 LightningRing이 있으면 건너뜀
//            if (Spawned_Indices.Contains(Random_Index))
//            {
//                continue;
//            }

//            Spawned_Indices.Add(Random_Index);

//            var Check_Enemy_Alive = All_Manager.instance.GameMode_M.Enemy_List[Random_Index];

//            if (Check_Enemy_Alive != null)
//            {

//                Weapon_Go = new GameObject("LightningRing");
//                Weapon_Go.AddComponent<Weapon_LightningRing>();

//                Weapon_Go.transform.position = All_Manager.instance.GameMode_M.Enemy_List[Random_Index].transform.position;
//                Weapon_Go.transform.rotation = Quaternion.identity;
//                Weapon_Go.transform.localScale = Vector3.one;

//                // LightningRing이 데미지를 주는지 확인 후 처리
//                if (Weapon_Go.GetComponent<Weapon_LightningRing>())
//                {
//                    yield return new WaitUntil(() => Weapon_Go.GetComponent<Weapon_LightningRing>().SR);
//                    // Weapon_Init을 통해 필요한 초기화 작업 수행
//                    Weapon_Go.GetComponent<Weapon>().Weapon_Init(this, All_Manager.instance.Resource_M.STO_LightningRing, LightningRing_LAT.Level);
//                }

//            }

//            Destroy(Weapon_Go, 2.5f); // 일정 시간 후 삭제
//        }
//    }


//}
//break;
#endregion