using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Pool_Manager : MonoBehaviour
{
    public bool Ready = false;
    public Damage_Effect_Controller Damage_Effect;

    private void Start()
    {
        StartCoroutine(C_Make_Pool());
    }

    public IEnumerator C_Make_Pool()
    {
        Ready = false;

        // i 가 2인 이유는 플레이어가 0, 보스가 1 이기 때문에
        for (int i = 2; i < System.Enum.GetValues(typeof(Unit_Type)).Length; i++)
        {
            var Temp_UT = (Unit_Type)i;

            GameObject Enemy_Pool = new GameObject(Temp_UT.ToString() + "-Pool");
            Enemy_Pool.transform.parent = this.transform;

            yield return new WaitWhile(() => Enemy_Pool == null);

            //50 은 좀 아슬아슬
            int Enemy_Number = 300;

            for (int k = 0; k < Enemy_Number; k++)
            {
                All_Manager.instance.Unit_M.Unit_F.Create_Enemy(All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(Temp_UT), 
                    Vector3.zero, (k + 1), Enemy_Pool.transform);
            }

            yield return new WaitWhile(() => Enemy_Pool.transform.childCount != Enemy_Number);
        }

        for (int j = 0; j < System.Enum.GetValues(typeof(MISC_Type)).Length; j++)
        {
            var Temp_MT = (MISC_Type)j;

            GameObject MISC_Pool = new GameObject(Temp_MT.ToString() + "-Pool");
            MISC_Pool.transform.parent = this.transform;

            yield return new WaitWhile(() => MISC_Pool == null);

            int MISC_Number = 1200;

            for (int k = 0; k < MISC_Number; k++)
            {
                All_Manager.instance.Unit_M.Misc_F.Create_MISC(All_Manager.instance.Resource_M.Convert_MISC_Type_To_STO_MISC_Arr(Temp_MT), this.transform, (k + 1), MISC_Pool.transform);
            }

            yield return new WaitWhile(() => MISC_Pool.transform.childCount != MISC_Number);
        }

        for (int h = 0; h < System.Enum.GetValues(typeof(Weapon_Type)).Length; h++)
        {
            var Temp_WT = (Weapon_Type)h;

            GameObject Weapon_Pool = new GameObject(Temp_WT.ToString() + "-Pool");
            Weapon_Pool.transform.parent = this.transform;

            yield return new WaitWhile(() => Weapon_Pool == null);

            int Weapon_Number = 100;

            for (int k = 0; k < Weapon_Number; k++)
            {
                All_Manager.instance.Unit_M.Projectile_F.Create_Projectile(All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr(Temp_WT), this.transform, (k + 1), Weapon_Pool.transform);
            }

            yield return new WaitWhile(() => Weapon_Pool.transform.childCount != Weapon_Number);
        }

        {
            Damage_Effect.Damage_Effect_Pool();
        }

        Ready = true;
    }

    // 오브젝트 풀에 다시 집어넣기
    public void Enemy_Pool_In(Unit_Enemy p_UE)
    {
        var Target_Pool = this.transform.Find(p_UE.UT.ToString() + "-Pool");

        p_UE.transform.parent = Target_Pool;

        p_UE.Elite = false;

        p_UE.isDead = false;

        p_UE.isEffect = false;

        p_UE.isDeathAnim = false;

        p_UE.AM.SetBool("Death", false);

        p_UE.SR.material.SetFloat("_Outline", 0.0f);

        p_UE.SR.material.SetFloat("_Red_Value", 1.0f);

        p_UE.SR.material.SetFloat("_Alpha", 1.0f);

        p_UE.GetComponent<Unit_Enemy>().SR.drawMode = SpriteDrawMode.Sliced;

        p_UE.GetComponent<Unit_Enemy>().SR.size = new Vector2(1, 1);

        //p_UE.SR_GB = 0.0f;

        p_UE.Dead_Effect = 0.0f;

        p_UE.Hit_Time = 0.0f;

        p_UE.Temp_Time = 1.0f;

        p_UE.Unit_Stat_Setting(p_UE, All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(p_UE.UT));
    }

    // 오브젝트 풀에서 꺼내기
    public void Enemy_Pool_Out(Unit_Type p_UT, Vector3 p_Position, bool p_Elite = false)
    {
        var Target_Pool = this.transform.Find(p_UT.ToString() + "-Pool");

        var Unit_Go = Target_Pool.GetChild(0).gameObject;

        Unit_Go.transform.parent = null;

        Unit_Go.transform.position = p_Position;

        Unit_Go.GetComponent<Unit_Enemy>().Elite = p_Elite;

        Unit_Go.SetActive(true);

        Unit_Go.GetComponent<Unit_Enemy>().SR.drawMode = SpriteDrawMode.Sliced;

        Unit_Go.GetComponent<Unit_Enemy>().SR.size = new Vector2((Unit_Go.GetComponent<Unit_Enemy>().SR.size.x * 4), (Unit_Go.GetComponent<Unit_Enemy>().SR.size.y * 4));

        Unit_Go.GetComponent<Unit_Enemy>().Elite_Outline_Set(p_UT);

        All_Manager.instance.Resource_M.SYGizmo.GizmoTargets.Add(Unit_Go.gameObject);

        Unit_Go.layer = 7;

        All_Manager.instance.GameMode_M.Enemy_List.Add(Unit_Go.GetComponent<Unit_Enemy>());
    }

    public void MISC_Pool_In(MISC p_MISC)
    {
        var Target_Pool = this.transform.Find(p_MISC.MT.ToString() + "-Pool");

        p_MISC.transform.localScale = Vector3.one;

        p_MISC.transform.parent = Target_Pool;
    }

    public void MISC_Pool_Out(MISC_Type p_MT, Transform p_TF)
    {
        var Target_Pool = this.transform.Find(p_MT.ToString() + "-Pool");

        var MISC_Go = Target_Pool.GetChild(0).gameObject;

        MISC_Go.GetComponent<MISC>().SR.drawMode = SpriteDrawMode.Sliced;

        MISC_Go.GetComponent<MISC>().SR.size = new Vector2((0.7f), (0.7f));

        MISC_Go.transform.parent = null;

        MISC_Go.transform.position = p_TF.transform.position;

        MISC_Go.SetActive(true);

        MISC_Go.layer = 8;

        All_Manager.instance.GameMode_M.MISC_List.Add(MISC_Go.GetComponent<MISC>());
    }

    public void Weapon_Pool_In(Weapon p_Weapon)
    {
        var Target_Pool = this.transform.Find(p_Weapon.WT.ToString() + "-Pool");

        p_Weapon.transform.parent = Target_Pool;

        p_Weapon.transform.position = Target_Pool.position;

        p_Weapon.transform.rotation = Quaternion.identity;

        p_Weapon.transform.localScale = Vector3.one;

        p_Weapon.Collision_Count = 0;

        p_Weapon.Closet_Distance = 0;

        p_Weapon.W_Accumulate = 0;

        p_Weapon.Weapon_Time = 0;

        //p_Weapon.GetComponent<Weapon>().SR.drawMode = SpriteDrawMode.Sliced;

        //p_Weapon.GetComponent<Weapon>().SR.size = new Vector2(1, 1);

        //Debug.Log("Weapon Pool");

        p_Weapon.gameObject.SetActive(false);
    }

    public GameObject Weapon_Pool_Out(Weapon_Type p_WT)
    {
        var Target_Pool = this.transform.Find(p_WT.ToString() + "-Pool");

        var Weapon_Go = Target_Pool.GetChild(0).gameObject;

        Weapon_Go.transform.parent = null;

        Weapon_Go.SetActive(true);

        Weapon_Go.layer = 10;

        return Weapon_Go;
        //All_Manager.instance.GameMode_M.All_L.Add(Weapon_Go.GetComponent<Weapon>());
    }


}

#region
// 필요시 새로운 몬스터 생성 (Optional)
//GameObject New_Enemy = Instantiate(Pools[i][0]);
//New_Enemy.SetActive(true);
//Pools[i].Add(New_Enemy);
//return New_Enemy;
//public GameObject[] Object_Go = null; 
//List<GameObject>[] Pools = null;
//private void Awake()
//{
//    Pools = new List<GameObject>[Object_Go.Length];

//    for (int i = 0; i < Pools.Length; i++)
//    {
//        Pools[i] = new List<GameObject>();
//    }
//}

//public GameObject GetPool(int Index)
//{
//    this.transform.Find(Unit_Type.Enemy.ToString()).GetChild(0); // = Enemy

//    this.transform.Find(Unit_Type.Golem.ToString()).GetChild(0);

//    this.transform.Find("Enemy1").GetChild(0);

//    GameObject Sel = null;

//    foreach (GameObject Item in Pools[Index])
//    {
//        if (!Item.activeSelf)
//        {
//            Sel = Item;
//            Sel.SetActive(true);
//            break;
//        }

//    }

//    if (!Sel)
//    {
//        Sel = new GameObject(Object_Go[Index].ToString());
//        Sel.transform.parent = this.transform;
//        //Sel = Instantiate(Object_Go[Index], this.transform);
//        Pools[Index].Add(Sel);
//    }

//    return Sel;
//}
#endregion