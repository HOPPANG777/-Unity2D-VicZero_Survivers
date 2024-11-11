using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

public class Unit_Enemy : Unit
{
    //public Unit_Type UT;

    Dictionary<Vector2Int, List<Unit_Enemy>> gridCells = new Dictionary<Vector2Int, List<Unit_Enemy>>();

    public float Attack_Time = 0.0f;
    public float Wating_Attack = 0.0f;

    public Vector2 MoveVec;

    public float Temp_Time = 1.0f;

    public bool Elite = false;

    private float Push_Strength = 0.5f;

    private float CheckInterval = 0.25f;
    private float LastCheckTime = 0.0f;

    private bool isSpeedBoosted = false;

    public override void Start()
    {
        base.Start();

        Wating_Attack = 3.0f;
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            if (All_Manager.instance.GameMode_M.Current_Player != null)
            {
                base.Update();
                
                if (Current_HP > 0)
                {
                    Update_Enemy_Move();
                }

                Player_To_Far_Enemy();

                Enemy_To_Player_Collision();

                Temp_Time -= Time.deltaTime;

                Attack_Time += Time.deltaTime;

            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time >= LastCheckTime + CheckInterval)
        {
            // 0.25초가 경과한 경우 충돌 검사 수행
            Enemy_To_Enemy_Push();

            // 현재 시간을 기준으로 마지막 체크 시간을 업데이트
            LastCheckTime = Time.time;
        }
    }

    public override void Hitted(float p_Damage_Value)
    {
        base.Hitted(p_Damage_Value);
        string Damage_Text = p_Damage_Value.ToString();
        All_Manager.instance.Pool_M.Damage_Effect.Show_Damage_Effect(transform.position, Damage_Text);
    }

    protected override IEnumerator CheckDeadWithDelay()
    {
        return base.CheckDeadWithDelay();
    }

    protected override bool CheckDead()
    {
        return base.CheckDead();
    }

    public override void Die()
    {
        base.Die();

    }

    protected override void Death_Animation_End()
    {
        base.Death_Animation_End();
        if (this.UT != Unit_Type.Boss || this.UT != Unit_Type.Barrel)
        {
            All_Manager.instance.GameMode_M.Enemy_Down(this);
        }
    }

    protected override IEnumerator C_Death_Animation_End()
    {
        return base.C_Death_Animation_End();
    }

    protected override void Return_Pool()
    {
        base.Return_Pool();
        if (this.UT == Unit_Type.Boss) return;
        if (this.UT == Unit_Type.Barrel)
        {
            All_Manager.instance.GameMode_M.Enemy_Down(this);
        }

        All_Manager.instance.Pool_M.Enemy_Pool_In(this);

    }

    public override void Unit_Clear()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }


    public void Update_Enemy_Move()
    {
        if (isDead == true && All_Manager.instance.GameMode_M.Game_Stop == true) return;

        Vector3 dir = All_Manager.instance.GameMode_M.Current_Player.transform.position - this.transform.position;

        this.transform.position = this.transform.position + (dir.normalized * (UnityEngine.Random.Range(Unit_Enemy_Speed, Unit_Enemy_Speed) * Time.deltaTime));

        if (this.UT == Unit_Type.Barrel) return;

        if (All_Manager.instance.GameMode_M.Enemy_List.Contains(this) || gameObject.GetComponent<Animator>())
        {
            Unit_Animation(Unit_Anim_State.Walk);
        }
        else
        {
            Unit_Animation(Unit_Anim_State.Idle);
        }

        if (All_Manager.instance.GameMode_M.Current_Player.transform.position.x < this.transform.position.x && isDead == false)
        {
            SR.flipX = true;
        }

        else
        {
            SR.flipX = false;
        }


    }
    public virtual void Enemy_To_Player_Collision()
    {
        var Player = All_Manager.instance.GameMode_M.Current_Player;
        var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;

        // 플레이어의 위치를 미리 계산하여 변수에 저장하여 반복 사용
        Vector2 Player_Position = Player.transform.position;
        float Min_Distance = 0.5f; // 충돌 반경 상수를 반복할 필요 없이 상수로 선언

        for (int i = 0; i < Enemy_List.Count; i++)
        {
            // 플레이어와 적의 위치 차이 벡터 계산
            Vector2 Dir = Player_Position - (Vector2)Enemy_List[i].transform.position;

            // 거리 계산을 제곱된 형태로 유지하여 성능 최적화 (루트 계산 방지)
            float Dis_Sqr = Dir.sqrMagnitude;
            float Min_Distance_Sqr = Min_Distance * Min_Distance;

            if (Dis_Sqr < Min_Distance_Sqr)
            {
                if (Attack_Time > Wating_Attack)
                {
                    Unit_Player Temp = Player.GetComponent<Unit_Player>();

                    if (this.UT != Unit_Type.Barrel) // 불필요한 빈 조건문 제거
                    {
                        Temp.Hitted(Unit_Enemy_Damage);
                    }

                    Attack_Time = 0.0f;
                }
            }
        }
    }

    public virtual void Enemy_To_Enemy_Push()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == true) return;

        var Enemy_List = All_Manager.instance.GameMode_M.Enemy_List;
        var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;
        Camera Temp_Camera = All_Manager.instance.GameMode_M.Current_Camera;

        if (this.UT == Unit_Type.Barrel)
        {
            return;
        }

        // 화면 내에 있는 유닛들만 따로 리스트로 관리 (적 및 플레이어 포함)
        List<Unit> InView_Unit_List = new List<Unit>();

        // 적 추가
        for (int i = 0; i < Enemy_List.Count; i++)
        {
            Vector3 View_Pos = Temp_Camera.WorldToViewportPoint(Enemy_List[i].transform.position);
            if (View_Pos.x >= 0 && View_Pos.x <= 1 && View_Pos.y >= 0 && View_Pos.y <= 1 && View_Pos.z >= 0)
            {
                InView_Unit_List.Add(Enemy_List[i]);
            }
        }

        // 그리드 크기 설정 (예: 5x5 유닛 영역)
        float Grid_Size = 5f;
        Dictionary<Vector2Int, List<Unit>> Grid = new Dictionary<Vector2Int, List<Unit>>();

        // 각 유닛을 그리드에 할당
        for (int i = 0; i < InView_Unit_List.Count; i++)
        {
            Vector2Int Grid_Position = new Vector2Int(
                Mathf.FloorToInt(InView_Unit_List[i].transform.position.x / Grid_Size),
                Mathf.FloorToInt(InView_Unit_List[i].transform.position.y / Grid_Size)
            );

            if (!Grid.ContainsKey(Grid_Position))
            {
                Grid[Grid_Position] = new List<Unit>();
            }

            Grid[Grid_Position].Add(InView_Unit_List[i]);
        }

        // 각 그리드 셀 내의 유닛들에 대해 밀쳐내기 처리
        foreach (var Cell in Grid)
        {
            List<Unit> Cluster_Unit_List = Cell.Value;

            for (int i = 0; i < Cluster_Unit_List.Count; i++)
            {
                for (int j = i + 1; j < Cluster_Unit_List.Count; j++)
                {
                    Vector2 Dir = Cluster_Unit_List[j].transform.position - Cluster_Unit_List[i].transform.position;

                    // 캐싱된 거리 제곱값을 사용
                    float sqrDis = Dir.sqrMagnitude;

                    // 충돌 반경 설정
                    float Min_Distance = 0.5f;
                    float minDistanceSquared = Min_Distance * Min_Distance;

                    if (sqrDis < minDistanceSquared)
                    {
                        // 밀쳐내기 계산
                        Vector2 pushDirection = Dir.normalized * (Min_Distance - Mathf.Sqrt(sqrDis)) * Push_Strength;

                        // 충돌이 발생한 경우 위치 업데이트
                        Cluster_Unit_List[i].transform.position -= (Vector3)pushDirection * 0.5f;
                        Cluster_Unit_List[j].transform.position += (Vector3)pushDirection * 0.5f;
                    }
                }
            }
        }
    }

    // 플레이어 추가 (플레이어가 화면 내에 있는 경우)
    //Vector3 Player_View_Pos = Temp_Camera.WorldToViewportPoint(Temp_Player.transform.position);
    //if (Player_View_Pos.x >= 0 && Player_View_Pos.x <= 1 && Player_View_Pos.y >= 0 && Player_View_Pos.y <= 1 && Player_View_Pos.z >= 0)
    //{
    //    InView_Unit_List.Add(Temp_Player);
    //}

    public void Elite_Outline_Set(Unit_Type p_UT)
    {
        //float Set_Elite = 0.05f;
        float Set_Not_Elite = 0.0f;

        if (Elite == false)
        {
            SR.material.SetFloat("_Outline", Set_Not_Elite);
        }

        else
        {
            SR.material.SetFloat("_Outline", Set_Not_Elite);
            Elite_HP_Setting(p_UT);
        }

    }

    public void Elite_HP_Setting(Unit_Type p_UT)
    {
        if (Elite == false)
        {

        }

        else
        {
            float Elite_HP = this.Max_HP;

            Elite_HP = Elite_HP * 3.0f;

            this.Unit_Enemy_HP = this.Current_HP = Max_HP = Elite_HP;
        }
    }

    // 유닛 에너미 스탯 조정
    public override void Unit_Stat_Setting(Unit p_Unit, STO_Unit p_Un_STO)
    {
        p_Unit.SR.sprite = p_Un_STO.Sprite_Image;
        p_Unit.SR.material = p_Un_STO.Elite_Material;

        p_Unit.Max_HP = p_Unit.Current_HP = p_Un_STO.Unit_Enemy_HP;
        p_Unit.Unit_Enemy_HP = p_Un_STO.Unit_Enemy_HP;
        p_Unit.Unit_Enemy_Damage = p_Un_STO.Unit_Enemy_Damage;
        p_Unit.Unit_Enemy_Speed = (p_Un_STO.Unit_Enemy_Speed * 1.5f);

        p_Unit.BC.size = new Vector2(p_Un_STO.Size_X, p_Un_STO.Size_Y);
        p_Unit.BC.offset = new Vector2(p_Un_STO.Offset_X, p_Un_STO.Offset_Y);

        if (p_Un_STO.Unit_Animator_Controller != null)
        {
            p_Unit.AM.runtimeAnimatorController = p_Un_STO.Unit_Animator_Controller;
            p_Unit.AM.speed = 1.0f; // 애니메이션 재생
            p_Unit.AM.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }

    // 너무 멀리있는 적 오브젝트 풀로 반환.
    public void Player_To_Far_Enemy()
    {
        var Temp_Player = All_Manager.instance.GameMode_M.Current_Player;

        if (Temp_Player == null)
        {
            return;
        }

        var distanceThreshold = 100.0f;
        var direction = this.transform.position - Temp_Player.transform.position;
        float distance = direction.magnitude;

        if (!All_Manager.instance.GameMode_M.Enemy_List.Contains(this))
        {
            return;
        }

        if (distance > distanceThreshold && !isSpeedBoosted)
        {
            if (this.UT != Unit_Type.Boss)
            {
                Unit_Enemy_Speed *= 10.0f;
                isSpeedBoosted = true; // 속도 부스터 상태 설정
            }
        }
        else if (distance <= distanceThreshold && isSpeedBoosted)
        {
            if (this.UT != Unit_Type.Boss)
            {
                Unit_Enemy_Speed = All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr(this.UT).Unit_Enemy_Speed * 1.5f;
                isSpeedBoosted = false; // 속도 부스터 해제
            }
        }

    }

    public override void Unit_Animation(Unit_Anim_State State)
    {
        base.Unit_Animation(State);
    }

}

//var target = All_Manager.instance.GameMode_M.Current_Player.transform;

//방향 바꾸고
//float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
//this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
//public virtual void OnTriggerStay2D(Collider2D collision)
//{
//    //충돌체가 이 컴포넌트를 가지고 있는가?
//    if (collision.transform.GetComponent<Unit_Player>())
//    {
//        if (Attack_Time > Wating_Attack)
//        {
//            //collision.transform.GetComponent<Unit_Player>().Hitted(Unit_Enemy_Damage);

//            Unit_Player Temp = collision.transform.GetComponent<Unit_Player>();

//            if (this.UT == Unit_Type.Barrel)
//            {

//            }

//            else
//            {
//                Temp.Hitted(Unit_Enemy_Damage);
//            }

//            Attack_Time = 0.0f;
//        }
//    }
//}