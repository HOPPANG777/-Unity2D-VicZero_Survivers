using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Unit_Manager : MonoBehaviour
{
    public UnitFactory Unit_F;
    public ProjectileFactory Projectile_F;
    public ItemFactory Item_F;
    public UIFactory UI_F;
    public MissionFactory Mission_F;
}

#region
//public void Create_Gem(Transform p_TF, int p_Number, Transform p_Pool)
//{
//    StartCoroutine(C_Create_Gem(p_TF, p_Number, p_Pool));
//}

//public IEnumerator C_Create_Gem(Transform p_TF, int p_Number, Transform p_Pool)
//{
//    GameObject Gem_Go = new GameObject("Gem" + "-" + p_Number.ToString());

//    Gem_Go.AddComponent<Item_Gem>();

//    Gem_Go.transform.position = p_TF.transform.position;
//    Gem_Go.transform.rotation = Quaternion.identity;
//    Gem_Go.transform.localScale = Vector3.one;

//    // Gem_Go.transform.parent = this.transform;
//    if (Gem_Go.GetComponent<Item_Gem>())
//    {
//        yield return new WaitUntil(() => Gem_Go.GetComponent<Item_Gem>()._spriteRenderer);
//        Gem_Go.GetComponent<Item_Gem>()._spriteRenderer.sprite = All_Manager.instance.Resource_M.Convert_ItemType_To_STO_Item_Arr(ItemType.Gem).Sprite_Image;
//    }

//    Gem_Go.transform.parent = p_Pool;

//    Gem_Go.SetActive(false);
//}

// public GameObject Player_Go;
// public GameObject Enemy_Go;
//50 - 100 구하고
//홀이면 플 짝이면 마
//int Temp_X = Random.Range(5, 10);
//Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

//int Temp_Y = Random.Range(5, 10);
//Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

//Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

//public void Start()
//{
//    StartCoroutine(EnemySpawn());
//}
//
//IEnumerator EnemySpawn()
//{
//    RandomEnemySpawn();
//    yield return new WaitForSecond_spriteRendererealtime(0.7f);
//}
//
//public void RandomEnemySpawn()
//{
//    GameObject Temp_Go = Instantiate(Enemy_Go, null);
//
//    //var 란 형태에 맞춰서 변수 타입을 자동으로 받는다
//    var Player_Pos = All_Manager.instance.GameMode_Manager.Current_Player.transform.position;
//
//    //50 - 100 구하고
//    //홀이면 플 짝이면 마
//    int Temp_X = Random.Range(5, 10);
//    Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;
//
//    int Temp_Y = Random.Range(5, 10);
//    Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;
//
//    Player_Pos += new Vector3(Temp_X, Temp_Y, 0);
//
//    Temp_Go.transform.position = Player_Pos;
//    Temp_Go.transform.rotation = Quaternion.identity;
//    Temp_Go.transform.localScale = Vector3.one;
//}

//public void Create_Unit(Unit_Name p_Unit_Name)
//{
//    StartCoroutine(C_Create_Unit(p_Unit_Name));
//}

////유닛 생성하는 함수
//public IEnumerator C_Create_Unit(Unit_Name p_Unit_Name)
//{
//    //플레이어 생성
//    if (p_Unit_Name == Unit_Name.Player)
//    {
//        GameObject Temp_Go = Instantiate(Player_Go, null);
//        Temp_Go.transform.position = Vector3.zero;
//        Temp_Go.transform.rotation = Quaternion.identity;
//        Temp_Go.transform.localScale = Vector3.one;

//        //이 게임오브젝트 이 컴포넌트를 갖고 있는지의 bool 값
//        if (Temp_Go.GetComponent<Player>())
//        {
//            //_spriteRenderer이 있을때까지 기다려
//            yield return new WaitUntil(() => Temp_Go.GetComponent<Player>()._spriteRenderer);
//            All_Manager.instance.Resource_M.Unit_Stat(Temp_Go.GetComponent<Player>(), All_Manager.instance.Resource_M.STO_Player);

//            All_Manager.instance.GameMode_M.Current_Player = Temp_Go.GetComponent<Player>();
//        }
//    }

//    // 적 생성
//    else if (p_Unit_Name == Unit_Name.Enemy)
//    {
//        GameObject Temp_Go = Instantiate(Enemy_Go, null);

//        //var 란 형태에 맞춰서 변수 타입을 자동으로 받는다
//        var Player_Pos = All_Manager.instance.GameMode_M.Current_Player.transform.position;

//        //50 - 100 구하고
//        //홀이면 플 짝이면 마
//        int Temp_X = Random.Range(5, 10);
//        Temp_X = ((Temp_X % 2) == 1) ? -Temp_X : Temp_X;

//        int Temp_Y = Random.Range(5, 10);
//        Temp_Y = ((Temp_Y % 2) == 1) ? -Temp_Y : Temp_Y;

//        Player_Pos += new Vector3(Temp_X, Temp_Y, 0);

//        Temp_Go.transform.position = Player_Pos;
//        Temp_Go.transform.rotation = Quaternion.identity;
//        Temp_Go.transform.localScale = Vector3.one;

//        if (Temp_Go.GetComponent<Enemy>())
//        {
//            yield return new WaitUntil(() => Temp_Go.GetComponent<Enemy>()._spriteRenderer);
//            All_Manager.instance.Resource_M.Unit_Stat(Temp_Go.GetComponent<Enemy>(), All_Manager.instance.Resource_M.STO_Enemy1);

//            All_Manager.instance.GameMode_M.Enemy_List.Add(Temp_Go.GetComponent<Enemy>());
//        }
//    }
//}


//public void Create_Player(STO_Unit P_Unit)
//{
//    StartCoroutine(C_Create_Player(P_Unit));
//}

//public IEnumerator C_Create_Player(STO_Unit P_Unit)
//{
//    GameObject Player_Go = new GameObject("Player");

//    Player_Go.AddComponent<Unit_Player>();

//    Player_Go.transform.position = Vector3.zero;
//    Player_Go.transform.rotation = Quaternion.identity;
//    Player_Go.transform.localScale = Vector3.one;

//    if (Player_Go.GetComponent<Unit_Player>())
//    {
//        //_spriteRenderer이 있을때까지 기다려                                                                 
//        yield return new WaitUntil(() => Player_Go.GetComponent<Unit_Player>()._spriteRenderer && Player_Go.GetComponent<Unit_Player>()._animator);
//        Player_Go.GetComponent<Unit_Player>().Unit_Stat_Setting(Player_Go.GetComponent<Unit_Player>(), P_Unit);

//        Player_Go.GetComponent<Unit_Player>()._spriteRenderer.drawMode = SpriteDrawMode.Sliced;
//        Player_Go.GetComponent<Unit_Player>()._spriteRenderer.size = new Vector2((Player_Go.GetComponent<Unit_Player>()._spriteRenderer.size.x * 0.2f), (Player_Go.GetComponent<Unit_Player>()._spriteRenderer.size.y * 0.2f));

//        All_Manager.instance.GameMode_M.Current_Player = Player_Go.GetComponent<Unit_Player>();
//    }

//}

//public void Create_Enemy(STO_Unit E_Unit, Vector3 Player_Pos, int p_Number, Transform p_Pool)
//{
//    StartCoroutine(C_Create_Enemy(E_Unit, Player_Pos, p_Number, p_Pool));
//}

//public IEnumerator C_Create_Enemy(STO_Unit E_Unit, Vector3 Player_Pos, int p_Number, Transform p_Pool)
//{
//    GameObject Enemy_Go = new GameObject(E_Unit.UnitType.ToString() + "-" + p_Number.ToString());

//    Enemy_Go.AddComponent<Unit_Enemy>();

//    Enemy_Go.transform.position = Player_Pos;
//    Enemy_Go.transform.rotation = Quaternion.identity;
//    Enemy_Go.transform.localScale = Vector3.one;

//    if (Enemy_Go.GetComponent<Unit_Enemy>())
//    {
//        yield return new WaitUntil(() => Enemy_Go.GetComponent<Unit_Enemy>()._spriteRenderer && Enemy_Go.GetComponent<Unit_Enemy>()._animator);
//        Enemy_Go.GetComponent<Unit_Enemy>().Unit_Stat_Setting(Enemy_Go.GetComponent<Unit_Enemy>(), E_Unit);
//        Enemy_Go.GetComponent<Unit_Enemy>().UnitType = E_Unit.UnitType;

//        Enemy_Go.GetComponent<Unit_Enemy>()._spriteRenderer.drawMode = SpriteDrawMode.Sliced;
//        Enemy_Go.GetComponent<Unit_Enemy>()._spriteRenderer.size = new Vector2((Enemy_Go.GetComponent<Unit_Enemy>()._spriteRenderer.size.x * 4), (Enemy_Go.GetComponent<Unit_Enemy>()._spriteRenderer.size.y * 4));

//        //All_Manager.instance.GameMode_M.Enemy_List.Add(Enemy_Go.GetComponent<Unit_Enemy>());
//    }

//    Enemy_Go.transform.parent = p_Pool;

//    Enemy_Go.SetActive(false);

//}


//public void Create_Boss(STO_Unit Boss_Unit, Vector3 Player_Pos)
//{
//    StartCoroutine(C_Create_Boss(Boss_Unit, Player_Pos));
//}

//public IEnumerator C_Create_Boss(STO_Unit Boss_Unit, Vector3 Player_Pos)
//{
//    GameObject Boss_Go = new GameObject(Boss_Unit.UnitType.ToString() + "-The_Undead");

//    Boss_Go.AddComponent<Unit_Boss>();

//    Boss_Go.transform.position = Player_Pos;
//    Boss_Go.transform.rotation = Quaternion.identity;
//    Boss_Go.transform.localScale = Vector3.one;

//    if (Boss_Go.GetComponent<Unit_Boss>())
//    {
//        yield return new WaitUntil(() => Boss_Go.GetComponent<Unit_Boss>()._spriteRenderer && Boss_Go.GetComponent<Unit_Boss>()._animator);
//        Boss_Go.GetComponent<Unit_Boss>().Unit_Stat_Setting(Boss_Go.GetComponent<Unit_Boss>(), Boss_Unit);
//        Boss_Go.GetComponent<Unit_Boss>().UnitType = Boss_Unit.UnitType;

//        All_Manager.instance.GameMode_M.Enemy_List.Add(Boss_Go.GetComponent<Unit_Boss>());
//    }
//}

//public void Create_Item(STO_Item p_ItemTypeO, Transform p_TF, int p_Number, Transform p_Pool)
//{
//    StartCoroutine(C_Create_Item(p_ItemTypeO, p_TF, p_Number, p_Pool));
//}

//public IEnumerator C_Create_Item(STO_Item p_ItemTypeO, Transform p_TF, int p_Number, Transform p_Pool)
//{
//    GameObject Item_Go = new GameObject(p_ItemTypeO.ItemType.ToString() + "-" + p_Number.ToString());

//    Item_Go.AddComponent<Item>();

//    Item_Go.transform.position = p_TF.transform.position;
//    Item_Go.transform.rotation = Quaternion.identity;
//    Item_Go.transform.localScale = Vector3.one;

//    if (Item_Go.GetComponent<Item>())
//    {
//        yield return new WaitUntil(() => Item_Go.GetComponent<Item>()._spriteRenderer);
//        Item_Go.GetComponent<Item>().Item_Setting(Item_Go.GetComponent<Item>(), p_ItemTypeO);

//        Item_Go.GetComponent<Item>()._spriteRenderer.drawMode = SpriteDrawMode.Sliced;
//        Item_Go.GetComponent<Item>()._spriteRenderer.size = new Vector2((Item_Go.GetComponent<Item>()._spriteRenderer.size.x * 2), (Item_Go.GetComponent<Item>()._spriteRenderer.size.y * 2));

//        Item_Go.GetComponent<Item>().ItemType = p_ItemTypeO.ItemType;
//    }

//    Item_Go.transform.parent = p_Pool;

//    Item_Go.SetActive(false);
//}
#endregion