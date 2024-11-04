using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class UnitManager : MonoBehaviour
{
    public UnitFactory UnitF;
    public ProjectileFactory ProjectileF;
    public ItemFactory ItemF;
    public UIFactory UIF;
    public MissionFactory MissionF;
}

#region
//public void CreateGem(Transform pTF, int pNumber, Transform pPool)
//{
//    StartCoroutine(CCreateGem(pTF, pNumber, pPool));
//}

//public IEnumerator CCreateGem(Transform pTF, int pNumber, Transform pPool)
//{
//    GameObject GemGo = new GameObject("Gem" + "-" + pNumber.ToString());

//    GemGo.AddComponent<ItemGem>();

//    GemGo.transform.position = pTF.transform.position;
//    GemGo.transform.rotation = Quaternion.identity;
//    GemGo.transform.localScale = Vector3.one;

//    // GemGo.transform.parent = this.transform;
//    if (GemGo.GetComponent<ItemGem>())
//    {
//        yield return new WaitUntil(() => GemGo.GetComponent<ItemGem>().spriteRenderer);
//        GemGo.GetComponent<ItemGem>().spriteRenderer.sprite = AllManager.instance.ResourceM.ConvertItemTypeToItemDataArr(ItemType.Gem).SpriteImage;
//    }

//    GemGo.transform.parent = pPool;

//    GemGo.SetActive(false);
//}

// public GameObject PlayerGo;
// public GameObject EnemyGo;
//50 - 100 구하고
//홀이면 플 짝이면 마
//int TempX = Random.Range(5, 10);
//TempX = ((TempX % 2) == 1) ? -TempX : TempX;

//int TempY = Random.Range(5, 10);
//TempY = ((TempY % 2) == 1) ? -TempY : TempY;

//PlayerPos += new Vector3(TempX, TempY, 0);

//public void Start()
//{
//    StartCoroutine(EnemySpawn());
//}
//
//IEnumerator EnemySpawn()
//{
//    RandomEnemySpawn();
//    yield return new WaitForSecondspriteRendererealtime(0.7f);
//}
//
//public void RandomEnemySpawn()
//{
//    GameObject TempGo = Instantiate(EnemyGo, null);
//
//    //var 란 형태에 맞춰서 변수 타입을 자동으로 받는다
//    var PlayerPos = AllManager.instance.GameModeManager.CurrentPlayer.transform.position;
//
//    //50 - 100 구하고
//    //홀이면 플 짝이면 마
//    int TempX = Random.Range(5, 10);
//    TempX = ((TempX % 2) == 1) ? -TempX : TempX;
//
//    int TempY = Random.Range(5, 10);
//    TempY = ((TempY % 2) == 1) ? -TempY : TempY;
//
//    PlayerPos += new Vector3(TempX, TempY, 0);
//
//    TempGo.transform.position = PlayerPos;
//    TempGo.transform.rotation = Quaternion.identity;
//    TempGo.transform.localScale = Vector3.one;
//}

//public void CreateUnit(UnitName pUnitName)
//{
//    StartCoroutine(CCreateUnit(pUnitName));
//}

////유닛 생성하는 함수
//public IEnumerator CCreateUnit(UnitName pUnitName)
//{
//    //플레이어 생성
//    if (pUnitName == UnitName.Player)
//    {
//        GameObject TempGo = Instantiate(PlayerGo, null);
//        TempGo.transform.position = Vector3.zero;
//        TempGo.transform.rotation = Quaternion.identity;
//        TempGo.transform.localScale = Vector3.one;

//        //이 게임오브젝트 이 컴포넌트를 갖고 있는지의 bool 값
//        if (TempGo.GetComponent<Player>())
//        {
//            //spriteRenderer이 있을때까지 기다려
//            yield return new WaitUntil(() => TempGo.GetComponent<Player>().spriteRenderer);
//            AllManager.instance.ResourceM.UnitStat(TempGo.GetComponent<Player>(), AllManager.instance.ResourceM.STOPlayer);

//            AllManager.instance.GameModeM.CurrentPlayer = TempGo.GetComponent<Player>();
//        }
//    }

//    // 적 생성
//    else if (pUnitName == UnitName.Enemy)
//    {
//        GameObject TempGo = Instantiate(EnemyGo, null);

//        //var 란 형태에 맞춰서 변수 타입을 자동으로 받는다
//        var PlayerPos = AllManager.instance.GameModeM.CurrentPlayer.transform.position;

//        //50 - 100 구하고
//        //홀이면 플 짝이면 마
//        int TempX = Random.Range(5, 10);
//        TempX = ((TempX % 2) == 1) ? -TempX : TempX;

//        int TempY = Random.Range(5, 10);
//        TempY = ((TempY % 2) == 1) ? -TempY : TempY;

//        PlayerPos += new Vector3(TempX, TempY, 0);

//        TempGo.transform.position = PlayerPos;
//        TempGo.transform.rotation = Quaternion.identity;
//        TempGo.transform.localScale = Vector3.one;

//        if (TempGo.GetComponent<Enemy>())
//        {
//            yield return new WaitUntil(() => TempGo.GetComponent<Enemy>().spriteRenderer);
//            AllManager.instance.ResourceM.UnitStat(TempGo.GetComponent<Enemy>(), AllManager.instance.ResourceM.STOEnemy1);

//            AllManager.instance.GameModeM.EnemyList.Add(TempGo.GetComponent<Enemy>());
//        }
//    }
//}


//public void CreatePlayer(UnitData PUnit)
//{
//    StartCoroutine(CCreatePlayer(PUnit));
//}

//public IEnumerator CCreatePlayer(UnitData PUnit)
//{
//    GameObject PlayerGo = new GameObject("Player");

//    PlayerGo.AddComponent<UnitPlayer>();

//    PlayerGo.transform.position = Vector3.zero;
//    PlayerGo.transform.rotation = Quaternion.identity;
//    PlayerGo.transform.localScale = Vector3.one;

//    if (PlayerGo.GetComponent<UnitPlayer>())
//    {
//        //spriteRenderer이 있을때까지 기다려                                                                 
//        yield return new WaitUntil(() => PlayerGo.GetComponent<UnitPlayer>().spriteRenderer && PlayerGo.GetComponent<UnitPlayer>().animator);
//        PlayerGo.GetComponent<UnitPlayer>().UnitStatSetting(PlayerGo.GetComponent<UnitPlayer>(), PUnit);

//        PlayerGo.GetComponent<UnitPlayer>().spriteRenderer.drawMode = SpriteDrawMode.Sliced;
//        PlayerGo.GetComponent<UnitPlayer>().spriteRenderer.size = new Vector2((PlayerGo.GetComponent<UnitPlayer>().spriteRenderer.size.x * 0.2f), (PlayerGo.GetComponent<UnitPlayer>().spriteRenderer.size.y * 0.2f));

//        AllManager.instance.GameModeM.CurrentPlayer = PlayerGo.GetComponent<UnitPlayer>();
//    }

//}

//public void CreateEnemy(UnitData EUnit, Vector3 PlayerPos, int pNumber, Transform pPool)
//{
//    StartCoroutine(CCreateEnemy(EUnit, PlayerPos, pNumber, pPool));
//}

//public IEnumerator CCreateEnemy(UnitData EUnit, Vector3 PlayerPos, int pNumber, Transform pPool)
//{
//    GameObject EnemyGo = new GameObject(EUnit.UnitType.ToString() + "-" + pNumber.ToString());

//    EnemyGo.AddComponent<UnitEnemy>();

//    EnemyGo.transform.position = PlayerPos;
//    EnemyGo.transform.rotation = Quaternion.identity;
//    EnemyGo.transform.localScale = Vector3.one;

//    if (EnemyGo.GetComponent<UnitEnemy>())
//    {
//        yield return new WaitUntil(() => EnemyGo.GetComponent<UnitEnemy>().spriteRenderer && EnemyGo.GetComponent<UnitEnemy>().animator);
//        EnemyGo.GetComponent<UnitEnemy>().UnitStatSetting(EnemyGo.GetComponent<UnitEnemy>(), EUnit);
//        EnemyGo.GetComponent<UnitEnemy>().UnitType = EUnit.UnitType;

//        EnemyGo.GetComponent<UnitEnemy>().spriteRenderer.drawMode = SpriteDrawMode.Sliced;
//        EnemyGo.GetComponent<UnitEnemy>().spriteRenderer.size = new Vector2((EnemyGo.GetComponent<UnitEnemy>().spriteRenderer.size.x * 4), (EnemyGo.GetComponent<UnitEnemy>().spriteRenderer.size.y * 4));

//        //AllManager.instance.GameModeM.EnemyList.Add(EnemyGo.GetComponent<UnitEnemy>());
//    }

//    EnemyGo.transform.parent = pPool;

//    EnemyGo.SetActive(false);

//}


//public void CreateBoss(UnitData BossUnit, Vector3 PlayerPos)
//{
//    StartCoroutine(CCreateBoss(BossUnit, PlayerPos));
//}

//public IEnumerator CCreateBoss(UnitData BossUnit, Vector3 PlayerPos)
//{
//    GameObject BossGo = new GameObject(BossUnit.UnitType.ToString() + "-TheUndead");

//    BossGo.AddComponent<UnitBoss>();

//    BossGo.transform.position = PlayerPos;
//    BossGo.transform.rotation = Quaternion.identity;
//    BossGo.transform.localScale = Vector3.one;

//    if (BossGo.GetComponent<UnitBoss>())
//    {
//        yield return new WaitUntil(() => BossGo.GetComponent<UnitBoss>().spriteRenderer && BossGo.GetComponent<UnitBoss>().animator);
//        BossGo.GetComponent<UnitBoss>().UnitStatSetting(BossGo.GetComponent<UnitBoss>(), BossUnit);
//        BossGo.GetComponent<UnitBoss>().UnitType = BossUnit.UnitType;

//        AllManager.instance.GameModeM.EnemyList.Add(BossGo.GetComponent<UnitBoss>());
//    }
//}

//public void CreateItem(ItemData pItemTypeO, Transform pTF, int pNumber, Transform pPool)
//{
//    StartCoroutine(CCreateItem(pItemTypeO, pTF, pNumber, pPool));
//}

//public IEnumerator CCreateItem(ItemData pItemTypeO, Transform pTF, int pNumber, Transform pPool)
//{
//    GameObject ItemGo = new GameObject(pItemTypeO.ItemType.ToString() + "-" + pNumber.ToString());

//    ItemGo.AddComponent<Item>();

//    ItemGo.transform.position = pTF.transform.position;
//    ItemGo.transform.rotation = Quaternion.identity;
//    ItemGo.transform.localScale = Vector3.one;

//    if (ItemGo.GetComponent<Item>())
//    {
//        yield return new WaitUntil(() => ItemGo.GetComponent<Item>().spriteRenderer);
//        ItemGo.GetComponent<Item>().ItemSetting(ItemGo.GetComponent<Item>(), pItemTypeO);

//        ItemGo.GetComponent<Item>().spriteRenderer.drawMode = SpriteDrawMode.Sliced;
//        ItemGo.GetComponent<Item>().spriteRenderer.size = new Vector2((ItemGo.GetComponent<Item>().spriteRenderer.size.x * 2), (ItemGo.GetComponent<Item>().spriteRenderer.size.y * 2));

//        ItemGo.GetComponent<Item>().ItemType = pItemTypeO.ItemType;
//    }

//    ItemGo.transform.parent = pPool;

//    ItemGo.SetActive(false);
//}
#endregion