using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Factory : MonoBehaviour
{

    public void Create_Player(STO_Unit P_Unit)
    {
        StartCoroutine(C_Create_Player(P_Unit));
    }

    public IEnumerator C_Create_Player(STO_Unit P_Unit)
    {
        GameObject Player_Go = new GameObject("Player");

        Player_Go.AddComponent<Unit_Player>();

        Player_Go.transform.position = Vector3.zero;
        Player_Go.transform.rotation = Quaternion.identity;
        Player_Go.transform.localScale = Vector3.one;

        if (Player_Go.GetComponent<Unit_Player>())
        {
            //SR이 있을때까지 기다려                                                                 
            yield return new WaitUntil(() => Player_Go.GetComponent<Unit_Player>().SR && Player_Go.GetComponent<Unit_Player>().AM);
            Player_Go.GetComponent<Unit_Player>().Unit_Stat_Setting(Player_Go.GetComponent<Unit_Player>(), P_Unit);

            Player_Go.GetComponent<Unit_Player>().SR.drawMode = SpriteDrawMode.Sliced;
            Player_Go.GetComponent<Unit_Player>().SR.size = new Vector2((Player_Go.GetComponent<Unit_Player>().SR.size.x * 0.2f), (Player_Go.GetComponent<Unit_Player>().SR.size.y * 0.2f));

            All_Manager.instance.GameMode_M.Current_Player = Player_Go.GetComponent<Unit_Player>();

            Player_Go.layer = 6;
        }

    }

    public void Create_Enemy(STO_Unit E_Unit, Vector3 Player_Pos, int p_Number, Transform p_Pool)
    {
        StartCoroutine(C_Create_Enemy(E_Unit, Player_Pos, p_Number, p_Pool));
    }

    public IEnumerator C_Create_Enemy(STO_Unit E_Unit, Vector3 Player_Pos, int p_Number, Transform p_Pool)
    {
        GameObject Enemy_Go = new GameObject(E_Unit.UT.ToString() + "-" + p_Number.ToString());

        Enemy_Go.AddComponent<Unit_Enemy>();

        Enemy_Go.transform.position = Player_Pos;
        Enemy_Go.transform.rotation = Quaternion.identity;
        Enemy_Go.transform.localScale = Vector3.one;

        if (Enemy_Go.GetComponent<Unit_Enemy>())
        {
            yield return new WaitUntil(() => Enemy_Go.GetComponent<Unit_Enemy>().SR && Enemy_Go.GetComponent<Unit_Enemy>().AM);
            Enemy_Go.GetComponent<Unit_Enemy>().Unit_Stat_Setting(Enemy_Go.GetComponent<Unit_Enemy>(), E_Unit);
            Enemy_Go.GetComponent<Unit_Enemy>().UT = E_Unit.UT;

            //All_Manager.instance.GameMode_M.Enemy_List.Add(Enemy_Go.GetComponent<Unit_Enemy>());
        }

        Enemy_Go.transform.parent = p_Pool;

        Enemy_Go.SetActive(false);

    }


    public void Create_Boss(STO_Unit Boss_Unit, Vector3 Player_Pos)
    {
        StartCoroutine(C_Create_Boss(Boss_Unit, Player_Pos));
    }

    public IEnumerator C_Create_Boss(STO_Unit Boss_Unit, Vector3 Player_Pos)
    {
        GameObject Boss_Go = new GameObject(Boss_Unit.UT.ToString() + "-The_Undead");

        Boss_Go.AddComponent<Unit_Boss>();

        Boss_Go.transform.position = Player_Pos;
        Boss_Go.transform.rotation = Quaternion.identity;
        Boss_Go.transform.localScale = Vector3.one;

        if (Boss_Go.GetComponent<Unit_Boss>())
        {
            yield return new WaitUntil(() => Boss_Go.GetComponent<Unit_Boss>().SR && Boss_Go.GetComponent<Unit_Boss>().AM);
            Boss_Go.GetComponent<Unit_Boss>().Unit_Stat_Setting(Boss_Go.GetComponent<Unit_Boss>(), Boss_Unit);

            Boss_Go.GetComponent<Unit_Enemy>().SR.drawMode = SpriteDrawMode.Sliced;
            Boss_Go.GetComponent<Unit_Enemy>().SR.size = new Vector2((Boss_Go.GetComponent<Unit_Enemy>().SR.size.x * 4), (Boss_Go.GetComponent<Unit_Enemy>().SR.size.y * 4));

            Boss_Go.GetComponent<Unit_Boss>().UT = Boss_Unit.UT;

            All_Manager.instance.GameMode_M.Enemy_List.Add(Boss_Go.GetComponent<Unit_Boss>());
        }
    }
}
