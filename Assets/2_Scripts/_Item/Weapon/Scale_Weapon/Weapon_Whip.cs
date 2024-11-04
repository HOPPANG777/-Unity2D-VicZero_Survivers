using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Weapon_Whip : Weapon
{
    public float Whip_Timer = 0.0f;
    public bool Whip_Right_Attack = false;
    public bool Whip_Left_Attack = false;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (All_Manager.instance.GameMode_M.Game_Stop == false)
        {
            base.Update();

            Weapon_KnockBack_Weapon();

            this.Weapon_Timer();
        }

        else
        {

        }

    }

    public void Whip_Init()
    {
        if (All_Manager.instance.GameMode_M.Current_Player.InputVec.x != 0)
        {
            SR.flipX = All_Manager.instance.GameMode_M.Current_Player.InputVec.x >= 0;
        }
    }

    public override void Upgrade(int p_Level)
    {

    }

    public override void Weapon_Stat_Appliaction()
    {
        base.Weapon_Stat_Appliaction();
    }

    public override void Weapon_To_Enemy_Collision()
    {
        base.Weapon_To_Enemy_Collision();
    }

}

/*
처음 채찍이 생성될때 플레이어가 바라보는 방향으로만 채찍이 나가지만 
강화를 하면 시간차를 두고 양쪽으로 채찍이 나간다.

P---->

<----P---->

근데 이걸 어케하지 갑자기 뇌정지가 왔다.

플레이어가 바라보는 방향으로 공격이 나가게 하는 방법과
채찍을 시간차를 두고 소환하는 방법을 알아야한다.

플레이어가 이동하는 
*/
