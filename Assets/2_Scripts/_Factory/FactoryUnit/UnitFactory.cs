using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 유닛 생성작업 SerializeField로 미리 연결해놓고 관리하는게 더 효율적이여서 그렇게 수정할게.
public class UnitFactory : MonoBehaviour
{
    [SerializeField] GameObject _bossPrefab;
    [SerializeField] GameObject _playerPrefab;

    public void Create_Player(STO_Unit P_Unit)
    {
        GameObject Player_Go = new GameObject("Player");

        Unit_Player player = Player_Go.AddComponent<Unit_Player>();

        if (player == null)
        {
            Debug.Log("플레이어가 null임");
            return;
        }

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.transform.localScale = Vector3.one;

        //_spriteRenderer�� ���������� ��ٷ�        
        // 여기서 코루틴으로 WaitUntil 사용했던데, 그거 거의 의미 없는 로직인듯...?                                                         
        player.Unit_Stat_Setting(player, P_Unit);

        player.UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
        player.UnitSpriteRenderer.size = new Vector2(player.UnitSpriteRenderer.size.x * 0.2f, player.UnitSpriteRenderer.size.y * 0.2f);

        All_Manager.instance.GameMode_M.Current_Player = Player_Go.GetComponent<Unit_Player>();

        Player_Go.layer = 6;

    }
    // 왜 코루틴으로 작성했었는지 의문이좀 듦.

    public void Create_Enemy(STO_Unit E_Unit, Vector3 Player_Pos, int p_Number, Transform p_Pool)
    {
        GameObject Enemy_Go = new GameObject(E_Unit.UnitType.ToString() + "-" + p_Number.ToString());

        Unit_Enemy enemy = Enemy_Go.AddComponent<Unit_Enemy>();
        if (enemy == null)
        {
            Debug.Log("enemy가 null임");
            return;
        }

        enemy.transform.position = Player_Pos;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.localScale = Vector3.one;

        // WaitUntil 왜 쓴건지 여기도 의문..
        //yield return new WaitUntil(() => enemy._spriteRenderer && enemy._animator);
        enemy.Unit_Stat_Setting(enemy, E_Unit);
        enemy.UnitType = E_Unit.UnitType;

        //All_Manager.instance.GameMode_M.Enemy_List.Add(Enemy_Go.GetComponent<Unit_Enemy>());
        Enemy_Go.transform.parent = p_Pool;

        Enemy_Go.SetActive(false);
    }


    public void Create_Boss(STO_Unit Boss_Unit, Vector3 Player_Pos)
    {
        GameObject Boss_Go = new GameObject(Boss_Unit.UnitType.ToString() + "-The_Undead");

        Unit_Boss boss = Boss_Go.AddComponent<Unit_Boss>();

        if (boss == null)
        {
            Debug.Log("boss가 null임");
            return;
        }

        boss.transform.position = Player_Pos;
        boss.transform.rotation = Quaternion.identity;
        boss.transform.localScale = Vector3.one;

        // 여기도 왜 WaitUntil 쓴건지 의문..
        //yield return new WaitUntil(() => boss._spriteRenderer && boss._animator);
        boss.Unit_Stat_Setting(boss, Boss_Unit);

        boss.UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
        boss.UnitSpriteRenderer.size = new Vector2(boss.UnitSpriteRenderer.size.x * 4, boss.UnitSpriteRenderer.size.y * 4);

        boss.UnitType = Boss_Unit.UnitType;

        All_Manager.instance.GameMode_M.Enemy_List.Add(Boss_Go.GetComponent<Unit_Boss>());

    }
}
