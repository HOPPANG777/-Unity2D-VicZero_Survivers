using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 유닛 생성작업 SerializeField로 미리 연결해놓고 관리하는게 더 효율적이여서 그렇게 수정할게.
public class UnitFactory : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject playerPrefab;

    public void CreatePlayer(UnitData PUnit)
    {
        GameObject PlayerGo = new GameObject("Player");

        UnitPlayer player = PlayerGo.AddComponent<UnitPlayer>();

        if (player == null)
        {
            Debug.Log("플레이어가 null임");
            return;
        }

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.transform.localScale = Vector3.one;

        //spriteRenderer�� ���������� ��ٷ�        
        // 여기서 코루틴으로 WaitUntil 사용했던데, 그거 거의 의미 없는 로직인듯...?                                                         
        player.UnitStatSetting(player, PUnit);

        player.UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
        player.UnitSpriteRenderer.size = new Vector2(player.UnitSpriteRenderer.size.x * 0.2f, player.UnitSpriteRenderer.size.y * 0.2f);

        AllManager.instance.GameModeM.CurrentPlayer = PlayerGo.GetComponent<UnitPlayer>();

        PlayerGo.layer = 6;

    }
    // 왜 코루틴으로 작성했었는지 의문이좀 듦.

    public void CreateEnemy(UnitData EUnit, Vector3 PlayerPos, int pNumber, Transform pPool)
    {
        GameObject EnemyGo = new GameObject(EUnit.UnitType.ToString() + "-" + pNumber.ToString());

        UnitEnemy enemy = EnemyGo.AddComponent<UnitEnemy>();
        if (enemy == null)
        {
            Debug.Log("enemy가 null임");
            return;
        }

        enemy.transform.position = PlayerPos;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.localScale = Vector3.one;

        // WaitUntil 왜 쓴건지 여기도 의문..
        //yield return new WaitUntil(() => enemy.spriteRenderer && enemy.animator);
        enemy.UnitStatSetting(enemy, EUnit);
        enemy.UnitType = EUnit.UnitType;

        //AllManager.instance.GameModeM.EnemyList.Add(EnemyGo.GetComponent<UnitEnemy>());
        EnemyGo.transform.parent = pPool;

        EnemyGo.SetActive(false);
    }


    public void CreateBoss(UnitData BossUnit, Vector3 PlayerPos)
    {
        GameObject BossGo = new GameObject(BossUnit.UnitType.ToString() + "-TheUndead");

        UnitBoss boss = BossGo.AddComponent<UnitBoss>();

        if (boss == null)
        {
            Debug.Log("boss가 null임");
            return;
        }

        boss.transform.position = PlayerPos;
        boss.transform.rotation = Quaternion.identity;
        boss.transform.localScale = Vector3.one;

        // 여기도 왜 WaitUntil 쓴건지 의문..
        //yield return new WaitUntil(() => boss.spriteRenderer && boss.animator);
        boss.UnitStatSetting(boss, BossUnit);

        boss.UnitSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
        boss.UnitSpriteRenderer.size = new Vector2(boss.UnitSpriteRenderer.size.x * 4, boss.UnitSpriteRenderer.size.y * 4);

        boss.UnitType = BossUnit.UnitType;

        AllManager.instance.GameModeM.EnemyList.Add(BossGo.GetComponent<UnitBoss>());

    }
}
