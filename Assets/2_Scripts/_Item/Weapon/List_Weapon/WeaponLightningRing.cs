using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WeaponLightningRing : Weapon
{
    public UnitEnemy LightningRingtarget;

    public Vector2 BoxSize = new Vector2(2, 2);

    public int EnemyCount = 0;

    public override void Start()
    {
        base.Start();
        Forever = true;
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            this.WeaponTimer();
        }

        else
        {

        }


    }

    public override void Upgrade(int pLevel)
    {

    }

    public override void WeaponStatAppliaction()
    {
        base.WeaponStatAppliaction();
    }

    public void LightningRingAttack()
    {



    }

    public override void WeaponToEnemyCollision()
    {
        base.WeaponToEnemyCollision();
    }



}

#region
//public class SpellBook : Weapon
//{
//    public GameObject LightningRingGo = null;

//    public Vector2 BoxSize = new Vector2(3, 3);

//    public Enemy Mtarget = null;

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
//        StartCoroutine(CSpell());
//    }

//    public IEnumerator CSpell()
//    {
//        float AttackRate = 10.0f;

//        while (true)
//        {
//            if (AllManager.instance.GameModeM.EnemyList.Count > 5)
//            {

//                for (int i = 0; i < 5; i++)
//                {
//                    GameObject TempGo = Instantiate(LightningRingGo, AllManager.instance.GameModeM.EnemyList[i].transform.position, Quaternion.identity);

//                    if (TempGo.GetComponent<LightningRing>())
//                    {
//                        yield return new WaitUntil(() => TempGo.GetComponent<LightningRing>().spriteRenderer);
//                        AllManager.instance.ResourceM.WeaponStat(TempGo.GetComponent<LightningRing>(), AllManager.instance.ResourceM.STOLightningRing);
//                    }
//                }
//            }
//            else
//            {
//                for (int i = 0; i < AllManager.instance.GameModeM.EnemyList.Count; i++)
//                {
//                    GameObject TempGo = Instantiate(LightningRingGo, AllManager.instance.GameModeM.EnemyList[i].transform.position, Quaternion.identity);

//                    if (TempGo.GetComponent<LightningRing>())
//                    {
//                        yield return new WaitUntil(() => TempGo.GetComponent<LightningRing>().spriteRenderer);
//                        AllManager.instance.ResourceM.WeaponStat(TempGo.GetComponent<LightningRing>(), AllManager.instance.ResourceM.STOLightningRing);
//                    }

//                    Destroy(TempGo, 2.5f);
//                }
//            }


//            yield return new WaitForSeconds(AttackRate);
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
//                var EnemyTemp = collider.gameObject.GetComponent<Enemy>();

//                if (Dis > Vector3.Distance(this.transform.position, EnemyTemp.transform.position))
//                {
//                    Dis = Vector3.Distance(this.transform.position, EnemyTemp.transform.position);
//                    Mtarget = EnemyTemp;
//                }
//            }
//        }

//        if (Mtarget != null)
//        {
//            Debug.Log("LNNULL");
//            GameObject TempGo = Instantiate(LightningRingGo, Mtarget.transform.position, Quaternion.identity);

//            if (TempGo.GetComponent<LightningRing>())
//            {
//                if (TempGo.GetComponent<LightningRing>().spriteRenderer == null)
//                    return;
//                AllManager.instance.ResourceM.WeaponStat(TempGo.GetComponent<LightningRing>(), AllManager.instance.ResourceM.STOLightningRing);
//            }


//            Destroy(TempGo.gameObject, 2.5f);
//        }

//    }

//}

//        case WeaponType.LightningRing:
//{


//    int MaxCount = AllManager.instance.ResourceM.STOLightningRing.Infos[LightningRingLAT.Level].WeaponCount;
//    int CountToSpawn = Mathf.Min(MaxCount, AllManager.instance.GameModeM.EnemyList.Count); // 최대 10개까지만 생성하도록 제한

//    List<int> SpawnedIndices = new List<int>(); // 이미 생성된 적의 인덱스를 저장할 리스트

//    if (AllManager.instance.GameModeM.EnemyList.Count > MaxCount)
//    {
//        for (int i = 0; i < MaxCount; i++)
//        {
//            int RandomIndex = Random.Range(0, AllManager.instance.GameModeM.EnemyList.Count);

//            // 중첩 방지: 이미 생성된 적 위치에 LightningRing이 있으면 건너뜀
//            if (SpawnedIndices.Contains(RandomIndex))
//            {
//                continue;
//            }

//            SpawnedIndices.Add(RandomIndex);

//            var CheckEnemyAlive = AllManager.instance.GameModeM.EnemyList[RandomIndex];

//            if (CheckEnemyAlive != null)
//            {

//                WeaponGo = new GameObject("LightningRing");
//                WeaponGo.AddComponent<WeaponLightningRing>();

//                WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[RandomIndex].transform.position;
//                WeaponGo.transform.rotation = Quaternion.identity;
//                WeaponGo.transform.localScale = Vector3.one;

//                // LightningRing이 데미지를 주는지 확인 후 처리
//                if (WeaponGo.GetComponent<WeaponLightningRing>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().spriteRenderer);
//                    // WeaponInit을 통해 필요한 초기화 작업 수행
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOLightningRing, LightningRingLAT.Level);
//                }

//            }
//            Destroy(WeaponGo, 2.5f); // 일정 시간 후 삭제
//        }
//    }

//    else
//    {
//        for (int j = 0; j < AllManager.instance.GameModeM.EnemyList.Count; j++)
//        {
//            int RandomIndex = Random.Range(0, AllManager.instance.GameModeM.EnemyList.Count);

//            // 중첩 방지: 이미 생성된 적 위치에 LightningRing이 있으면 건너뜀
//            if (SpawnedIndices.Contains(RandomIndex))
//            {
//                continue;
//            }

//            SpawnedIndices.Add(RandomIndex);

//            var CheckEnemyAlive = AllManager.instance.GameModeM.EnemyList[RandomIndex];

//            if (CheckEnemyAlive != null)
//            {

//                WeaponGo = new GameObject("LightningRing");
//                WeaponGo.AddComponent<WeaponLightningRing>();

//                WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[RandomIndex].transform.position;
//                WeaponGo.transform.rotation = Quaternion.identity;
//                WeaponGo.transform.localScale = Vector3.one;

//                // LightningRing이 데미지를 주는지 확인 후 처리
//                if (WeaponGo.GetComponent<WeaponLightningRing>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().spriteRenderer);
//                    // WeaponInit을 통해 필요한 초기화 작업 수행
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOLightningRing, LightningRingLAT.Level);
//                }

//            }

//            Destroy(WeaponGo, 2.5f); // 일정 시간 후 삭제
//        }
//    }


//}
//break;
#endregion