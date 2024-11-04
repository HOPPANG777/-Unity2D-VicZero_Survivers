using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEngine.UI.CanvasScaler;

[System.Serializable]
public class WeaponLevelAndTimer
{
    public int Level = 0;
    public float Timer = 0;
}

public class UnitPlayer : Unit, IWeaponType, IAccessoryType
{
    [SerializeField] Rigidbody2D playerRigidBody;
    public Rigidbody2D PlayerRigidBody { get { return playerRigidBody; } }

    [SerializeField] public WeaponType WeaponType { get; set; }
    [SerializeField] public AccessoryType AccessoryType { get; set; }

    //찾아보면서 써보기
    public List<Weapon> AllWeapon = new List<Weapon>();
    public List<AccessoryData> AllAccessory = new List<AccessoryData>();
    public Dictionary<WeaponKeyword, int> weaponKeywordCounts = new Dictionary<WeaponKeyword, int>();

    public Vector2 InputVec;

    public bool PlayerRight = false;
    public bool PlayerLeft = false;

    public WeaponLevelAndTimer AxeLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer KingBibleLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer PeachoneLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer FireWandLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer BananaLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer ForceLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer LightningRingLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer ShieldChasingLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer RuneTracerLAT = new WeaponLevelAndTimer();
    public WeaponLevelAndTimer NunchakuLAT = new WeaponLevelAndTimer();

    public int BangLevel;
    public int BracerLevel;
    public int TimesLevel;
    public int EmptyTomeLevel;
    public int ThunderLevel;
    public int TheFlagsLevel;
    public int MagicHoodLevel;
    public int FloppyDiskLevel;
    public int AttractorbLevel;
    public int RedAjaLevel;

    public bool CreatingWeapon = false;
    public bool CreatingAccessory = false;
    public bool WeaponTrigger = false;

    public bool AccessoryLevelUp = false;

    public override void Start()
    {
        base.Start();

        playerRigidBody = this.AddComponent<Rigidbody2D>();
        playerRigidBody.bodyType = RigidbodyType2D.Kinematic;

        AllManager.instance.ResourceM.GhostEffect.AddGhostTarget(this.transform);
    }

    public override void Update()
    {
        if (AllManager.instance.GameModeM.GameStop == false)
        {
            base.Update();

            PlayerMove();

            SpawnWeaponTimerUpdate();
        }
    }

    public override void Hitted(float pDamageValue)
    {
        base.Hitted(pDamageValue);

        AllManager.instance.UIM.UpdatePlayerHPBarImage(CurrentHP / MaxHP);
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

        AllManager.instance.GameModeM.GameOver();
    }

    protected override void DeathAnimationEnd()
    {
        base.DeathAnimationEnd();
    }

    protected override IEnumerator CDeathAnimationEnd()
    {
        return base.CDeathAnimationEnd();
    }

    protected override void ReturnPool()
    {
        base.ReturnPool();
    }

    public override void UnitClear()
    {
        for (int i = 0; i < AllWeapon.Count; i++)
        {
            AllWeapon[i].DestroyWeapon();
        }

        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void CreateWeapon(WeaponType pweaponType)
    {
        StartCoroutine(CCreateWeapon(pweaponType));
    }

    #region
    // ���� ����
    public IEnumerator CCreateWeapon(WeaponType pweaponType)
    {
        yield return new WaitWhile(() => CreatingWeapon == true);

        var WeaponSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        var WeaponWLAT = ConvertWLATPlayer(WeaponSTO);

        CreatingWeapon = true;

        //GameObject WeaponGo = null;
        GameObject WeaponGo = null;

        switch (pweaponType)
        {
            // ����
            case WeaponType.Axe:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);

                    //WeaponGo.AddComponent<WeaponAxe>();

                    // WeaponGo�� �ش� ������ ������Ʈ�� �����ϰ� �ִٸ�.
                    if (WeaponGo.GetComponent<WeaponAxe>())
                    {
                        // ��ġ �ʱ�ȭ
                        WeaponGo.transform.position = this.transform.position;
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        // �� ������ ��������Ʈ�� �ҷ��ö����� ��ٷ���.
                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponAxe>().WeaponSpriteRenderer);
                        Vector3 One = this.transform.position;
                        Vector3 Two, Three, Four;

                        // ���� ���⿡ ����
                        if (WeaponTrigger)
                        {
                            Two = One + new Vector3(0, 5, 0);
                            Three = Two + new Vector3(Random.Range(1, 5), 0, 0);        // ������
                            Four = One + new Vector3(Random.Range(1, 5), -10, 0);       // ������ �Ʒ�
                        }
                        else
                        {
                            Two = One + new Vector3(0, 5, 0);
                            Three = Two + new Vector3(Random.Range(-5, -1), 0, 0);      // ����
                            Four = One + new Vector3(Random.Range(-5, -1), -10, 0);     // ���� �Ʒ�
                        }

                        // ���� �ʱ�ȭ
                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, AxeLAT.Level);
                        WeaponGo.GetComponent<WeaponAxe>().AxeInit(One, Two, Three, Four);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponAxe>().WeaponKeyword);

                        // ������ �����ɶ� ���� �ݴ�� ����
                        WeaponTrigger = !WeaponTrigger;

                        // ���� ������ ȿ����
                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.Axe);
                    }
                }
                break;
            case WeaponType.KingBible:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);

                    //WeaponGo.AddComponent<WeaponKingBible>();

                    if (WeaponGo.GetComponent<WeaponKingBible>())
                    {
                        WeaponGo.transform.position = this.transform.position;
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        //Start()
                        //Update()

                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponKingBible>().WeaponSpriteRenderer);

                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, KingBibleLAT.Level);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponKingBible>().WeaponKeyword);

                        // ���� ���� ������ �ִ� ������ ����
                        int Max = 0;

                        // �÷��̾ ����ϰ� �ִ� ������ ����
                        for (int i = 0; i < AllWeapon.Count; i++)
                        {
                            if (AllWeapon[i].GetComponent<WeaponKingBible>())
                            {
                                Max++;
                            }
                        }

                        // 
                        Max++;

                        //
                        for (int i = 0; i < AllWeapon.Count; i++)
                        {
                            if (AllWeapon[i].GetComponent<WeaponKingBible>())
                            {
                                AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMaxKingBibleCount = Max;
                            }
                        }

                        WeaponGo.GetComponent<WeaponKingBible>().WeaponMaxKingBibleCount = Max;
                        WeaponGo.GetComponent<WeaponKingBible>().WeaponMineKingBibleCount = Max;

                        //var TempKingBibleRange = CurrentWeaponRange(WeaponType.KingBible, KingBibleLAT);

                        //WeaponGo.GetComponent<WeaponKingBible>().WeaponRange = TempKingBibleRange;

                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.KingBible);
                    }
                }
                break;
            case WeaponType.Peachone:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);

                    //WeaponGo.AddComponent<WeaponPeachone>();

                    if (WeaponGo.GetComponent<WeaponPeachone>())
                    {
                        WeaponGo.transform.position = AllManager.instance.UIM.PeachoneImage.rectTransform.position;
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponPeachone>().WeaponSpriteRenderer);

                        var PPeachoneUIRange = WeaponGo.GetComponent<WeaponPeachone>().ConvertRectTransformToPosition(AllManager.instance.UIM.PeachoneRangeImage.rectTransform);
                        var PPeachoneUI = WeaponGo.GetComponent<WeaponPeachone>().ConvertRectTransformToPosition(AllManager.instance.UIM.PeachoneImage.rectTransform);

                        Vector3 One = AllManager.instance.UIM.PeachoneImage.transform.position;
                        Vector3 Two = WeaponGo.GetComponent<WeaponPeachone>().PeachonePointSetting(AllManager.instance.UIM.PeachoneImage.transform.position);
                        Vector3 Three = WeaponGo.GetComponent<WeaponPeachone>().PeachonePointSetting(PPeachoneUIRange);
                        Vector3 Four = PPeachoneUIRange;

                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, PeachoneLAT.Level);
                        WeaponGo.GetComponent<WeaponPeachone>().PeachoneInit(One, Two, Three, Four);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponPeachone>().WeaponKeyword);
                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.Peachone);
                    }

                }
                break;
            case WeaponType.FireWand:
                {
                    var FireWandStat = WeaponSTO.Infos[FireWandLAT.Level];

                    for (int i = 0; i < CurrentWeaponCount(WeaponType.FireWand, FireWandLAT); i++)
                    {
                        WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);

                        //WeaponGo.AddComponent<WeaponFireWand>();

                        if (WeaponGo.GetComponent<WeaponFireWand>())
                        {
                            WeaponGo.transform.position = this.transform.position + new Vector3(0, -0.3f, 0);
                            WeaponGo.transform.rotation = Quaternion.identity;
                            WeaponGo.transform.localScale = Vector3.one;

                            yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponFireWand>().WeaponSpriteRenderer);

                            WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, FireWandLAT.Level);
                            CheckSynergyEffects(WeaponGo.GetComponent<WeaponFireWand>().WeaponKeyword);

                            Vector3 DirVec = new Vector3(Mathf.Cos((Mathf.PI * 2 * i / CurrentWeaponCount(WeaponType.FireWand, FireWandLAT))), Mathf.Sin((Mathf.PI * i * 2 / CurrentWeaponCount(WeaponType.FireWand, FireWandLAT)))); //���� �������� ������ ����ü���� ���͸� ����

                            if (AllManager.instance.GameModeM.GameStop == false)
                            {
                                WeaponGo.transform.position = WeaponGo.transform.position + (DirVec.normalized * FireWandStat.WeaponSpeed * Time.deltaTime);
                            }
                            else { }

                            WeaponGo.transform.rotation = Quaternion.Euler(0, 0, (360 * i / (CurrentWeaponCount(WeaponType.FireWand, FireWandLAT))) + 180); //����ü�� ������ �ڿ������� ȸ������ �־���.
                            AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.FireWand);
                        }
                    }
                }
                break;
            case WeaponType.Banana:
                {
                    var WBananaS = WeaponSTO.Infos[BananaLAT.Level];

                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);

                    //WeaponGo.AddComponent<WeaponBanana>();

                    if (WeaponGo.GetComponent<WeaponBanana>())
                    {
                        WeaponGo.transform.position = this.transform.position;
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponBanana>().WeaponSpriteRenderer);

                        Vector3 One = this.transform.position;
                        Vector3 Target = One + new Vector3(Random.Range(-WBananaS.WeaponRange, WBananaS.WeaponRange), Random.Range(-WBananaS.WeaponRange, WBananaS.WeaponRange), 0);

                        Vector3 Two = One + (Target - One) / 3 + new Vector3(Random.Range(-WBananaS.WeaponRange, WBananaS.WeaponRange) / 2, Random.Range(-WBananaS.WeaponRange, WBananaS.WeaponRange) / 2, 0);
                        Vector3 Three = Target + (One - Target) / 3 + new Vector3(Random.Range(-WBananaS.WeaponRange, WBananaS.WeaponRange) / 2, Random.Range(-WBananaS.WeaponRange, WBananaS.WeaponRange) / 2, 0);
                        Vector3 Four = One + ((One - Target) * WBananaS.WeaponRange);

                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, BananaLAT.Level);
                        WeaponGo.GetComponent<WeaponBanana>().BananaInit(One, Two, Three, Four);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponBanana>().WeaponKeyword);
                        //WeaponGo.GetComponent<WeaponBanana>().WeaponApplicationAccessory();
                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.Banana);
                    }
                }
                break;
            case WeaponType.Force:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);
                    //WeaponGo.AddComponent<WeaponForce>();

                    if (WeaponGo.GetComponent<WeaponForce>())
                    {
                        WeaponGo.transform.position = this.transform.position;
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        WeaponGo.transform.parent = this.transform;

                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponForce>().WeaponSpriteRenderer);

                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, ForceLAT.Level);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponForce>().WeaponKeyword);
                        //WeaponGo.GetComponent<WeaponForce>().WeaponApplicationAccessory();
                        //AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.Force);
                    }
                }
                break;
            case WeaponType.LightningRing:
                {
                    if (AllManager.instance != null && AllManager.instance.GameModeM != null && AllManager.instance.GameModeM.EnemyList != null && AllManager.instance.GameModeM.EnemyList.Count > 0)
                    {
                        var MaxCount = CurrentWeaponCount(WeaponType.LightningRing, LightningRingLAT);
                        int CountToSpawn = Mathf.Min(MaxCount, AllManager.instance.GameModeM.EnemyList.Count); // �ִ� 10�������� �����ϵ��� ����

                        List<int> SpawnedIndices = new List<int>(); // �̹� ������ ���� �ε����� ������ ����Ʈ

                        for (int i = 0; i < CountToSpawn; i++)
                        {
                            int RandomIndex = Random.Range(0, AllManager.instance.GameModeM.EnemyList.Count);

                            // ��ø ����: �̹� ������ �� ��ġ�� LightningRing�� ������ �ǳʶ�
                            if (SpawnedIndices.Contains(RandomIndex))
                            {
                                continue;
                            }

                            SpawnedIndices.Add(RandomIndex);

                            var CheckEnemyAlive = AllManager.instance.GameModeM.EnemyList[RandomIndex];

                            if (CheckEnemyAlive == null)
                            {
                                //Debug.LogWarning("None Enemy! : " + RandomIndex);
                                continue;
                            }

                            WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);
                            //WeaponGo.AddComponent<WeaponLightningRing>();


                            // LightningRing�� �������� �ִ��� Ȯ�� �� ó��
                            if (WeaponGo.GetComponent<WeaponLightningRing>() != null)
                            {
                                WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[RandomIndex].transform.position;
                                WeaponGo.transform.rotation = Quaternion.identity;
                                WeaponGo.transform.localScale = Vector3.one;

                                yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().WeaponSpriteRenderer);

                                // WeaponInit�� ���� �ʿ��� �ʱ�ȭ �۾� ����
                                WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, LightningRingLAT.Level);
                                CheckSynergyEffects(WeaponGo.GetComponent<WeaponLightningRing>().WeaponKeyword);
                                //WeaponGo.GetComponent<WeaponLightningRing>().WeaponApplicationAccessory();
                                AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.LightningRing);
                            }
                            else
                            {
                                //Debug.Log("LightningRing Gone!");
                            }

                        }
                    }



                }
                break;
            case WeaponType.ShieldChasing:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);
                    //WeaponGo.AddComponent<WeaponShieldChasing>();

                    if (WeaponGo.GetComponent<WeaponShieldChasing>())
                    {
                        WeaponGo.transform.position = this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponShieldChasing>().WeaponSpriteRenderer);

                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, ShieldChasingLAT.Level);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponShieldChasing>().WeaponKeyword);
                        //WeaponGo.GetComponent<WeaponShieldChasing>().WeaponApplicationAccessory();
                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.ShieldChasing);
                    }
                }
                break;
            case WeaponType.RuneTracer:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);
                    //WeaponGo.AddComponent<WeaponRuneTracer>();

                    if (WeaponGo.GetComponent<WeaponRuneTracer>())
                    {
                        WeaponGo.transform.position = this.transform.position;
                        WeaponGo.transform.rotation = Quaternion.identity;
                        WeaponGo.transform.localScale = Vector3.one;

                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponRuneTracer>().WeaponSpriteRenderer);

                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, RuneTracerLAT.Level);
                        //WeaponGo.GetComponent<WeaponRuneTracer>().WeaponApplicationAccessory();
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponRuneTracer>().WeaponKeyword);
                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.RuneTracer);
                    }
                }
                break;
            case WeaponType.Nunchaku:
                {
                    WeaponGo = AllManager.instance.PoolM.WeaponPoolOut(pweaponType);
                    //WeaponGo.AddComponent<WeaponNunchaku>();

                    //WeaponGo.transform.parent = this.transform;

                    if (WeaponGo.GetComponent<WeaponNunchaku>())
                    {
                        yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponNunchaku>().WeaponSpriteRenderer);

                        if (WeaponTrigger)
                        {
                            WeaponGo.transform.position = this.transform.position + new Vector3(-2f + Random.Range(-0.5f, 0.5f), -0.1f + Random.Range(-0.2f, 0.2f), 0);
                            WeaponGo.transform.rotation = Quaternion.identity;
                            WeaponGo.transform.localScale = new Vector3(-4, -1, -1);
                        }
                        else
                        {
                            WeaponGo.transform.position = this.transform.position + new Vector3(2f + Random.Range(-0.5f, 0.5f), -0.1f + Random.Range(-0.2f, 0.2f), 0);
                            WeaponGo.transform.rotation = Quaternion.identity;
                            WeaponGo.transform.localScale = new Vector3(4, 1, 1);
                        }


                        WeaponGo.GetComponent<Weapon>().WeaponInit(this, WeaponSTO, NunchakuLAT.Level);
                        CheckSynergyEffects(WeaponGo.GetComponent<WeaponNunchaku>().WeaponKeyword);
                        WeaponTrigger = !WeaponTrigger;

                        AllManager.instance.AudioM.WeaponAttackingEffectAudioPlay(WeaponType.Nunchaku);
                    }

                }
                break;
            default:
                break;
        }

        //Debug.Log(WeaponGo);

        if (WeaponGo != null)
        {
            AllWeapon.Add(WeaponGo.GetComponent<Weapon>());
            //WeaponGo.transform.parent = this.transform;
        }

        else
        {
            //Debug.Log("WeaponGo Null");
        }

        CreatingWeapon = false;
    }

    void UseWeapon()
    {

    }

    // ���� ���׷��̵�
    public void UpgradeWeapon(WeaponType pweaponType)
    {
        if (pweaponType == WeaponType.Axe)
        {
            AxeLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponAxe>())
                {
                    AllWeapon[i].Upgrade(AxeLAT.Level);
                }
            }

            //Debug.Log(AxeLAT.Level);
        }

        else if (pweaponType == WeaponType.KingBible)
        {
            KingBibleLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponKingBible>())
                {
                    AllWeapon[i].Upgrade(KingBibleLAT.Level);
                }
            }

            CreateWeapon(WeaponType.KingBible);
        }

        else if (pweaponType == WeaponType.Peachone)
        {
            PeachoneLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponPeachone>())
                {
                    AllWeapon[i].Upgrade(PeachoneLAT.Level);
                }
            }
        }

        else if (pweaponType == WeaponType.FireWand)
        {
            FireWandLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponFireWand>())
                {
                    AllWeapon[i].Upgrade(FireWandLAT.Level);
                }
            }

        }

        else if (pweaponType == WeaponType.Banana)
        {
            BananaLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponBanana>())
                {
                    AllWeapon[i].Upgrade(BananaLAT.Level);
                }
            }

        }

        else if (pweaponType == WeaponType.Force)
        {
            ForceLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponForce>())
                {
                    AllWeapon[i].Upgrade(ForceLAT.Level);
                }
            }


        }

        else if (pweaponType == WeaponType.LightningRing)
        {
            LightningRingLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponLightningRing>())
                {
                    AllWeapon[i].Upgrade(LightningRingLAT.Level);
                }
            }

        }

        else if (pweaponType == WeaponType.ShieldChasing)
        {
            ShieldChasingLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponShieldChasing>())
                {
                    AllWeapon[i].Upgrade(ShieldChasingLAT.Level);
                }
            }

        }

        else if (pweaponType == WeaponType.RuneTracer)
        {
            RuneTracerLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponRuneTracer>())
                {
                    AllWeapon[i].Upgrade(RuneTracerLAT.Level);
                }
            }

        }

        else if (pweaponType == WeaponType.Nunchaku)
        {
            NunchakuLAT.Level++;

            for (int i = 0; i < AllWeapon.Count; i++)
            {
                if (AllWeapon[i].GetComponent<WeaponNunchaku>())
                {
                    AllWeapon[i].Upgrade(NunchakuLAT.Level);
                }
            }

        }


    }

    // ������ �ִ���
    public bool CheckWeapon(WeaponType pPT)
    {
        return false;
    }

    // ���� ���� Ÿ�̸� ������Ʈ
    public void SpawnWeaponTimerUpdate()
    {
        if (AxeLAT.Level > 0)
        {
            if (AxeLAT.Timer > CurrentWeaponCoolDown(WeaponType.Axe, AxeLAT))
            {
                AxeLAT.Timer = 0;

                //for (int i = 0; i < WeaponDataTemp.Infos[AxeLAT.Level].WeaponCount; i++)
                //{
                //    CreateWeapon(WeaponType.Axe);
                //}

                for (int i = 0; i < CurrentWeaponCount(WeaponType.Axe, AxeLAT); i++)
                {
                    CreateWeapon(WeaponType.Axe);
                }
            }

            AxeLAT.Timer += Time.deltaTime;
        }

        if (KingBibleLAT.Level > 0)
        {
            if (KingBibleLAT.Timer > CurrentWeaponCoolDown(WeaponType.KingBible, KingBibleLAT))
            {
                KingBibleLAT.Timer = 0;

                //for (int i = 0; i < CurrentWeaponCount(WeaponType.KingBible, KingBibleLAT); i++)    // ������ �� ������ ���� ������ �ȳ������� ����, ���� �̰� STO�� ���� ������ ȸ�� ��Ų�� �� �̰� ���ľ� �Ѵ�.
                //{
                //    CreateWeapon(WeaponType.KingBible);
                //}
            }

            KingBibleLAT.Timer += Time.deltaTime;
        }

        if (PeachoneLAT.Level > 0)
        {
            if (PeachoneLAT.Timer > CurrentWeaponCoolDown(WeaponType.Peachone, PeachoneLAT))
            {
                PeachoneLAT.Timer = 0;


                for (int i = 0; i < CurrentWeaponCount(WeaponType.Peachone, PeachoneLAT); i++)
                {
                    CreateWeapon(WeaponType.Peachone);
                }
            }

            PeachoneLAT.Timer += Time.deltaTime;
        }

        if (FireWandLAT.Level > 0)
        {
            if (FireWandLAT.Timer > CurrentWeaponCoolDown(WeaponType.FireWand, FireWandLAT))
            {
                FireWandLAT.Timer = 0;

                CreateWeapon(WeaponType.FireWand);
                //Debug.Log("FireWand!");
            }

            FireWandLAT.Timer += Time.deltaTime;
        }

        if (BananaLAT.Level > 0)
        {
            if (BananaLAT.Timer > CurrentWeaponCoolDown(WeaponType.Banana, BananaLAT))
            {
                BananaLAT.Timer = 0;

                for (int i = 0; i < CurrentWeaponCount(WeaponType.Banana, BananaLAT); i++)
                {
                    CreateWeapon(WeaponType.Banana);
                }
            }

            BananaLAT.Timer += Time.deltaTime;
        }

        if (ForceLAT.Level > 0)
        {
            var WeaponDataTemp = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(WeaponType.Force);

            if (ForceLAT.Timer > CurrentWeaponCoolDown(WeaponType.Force, ForceLAT))
            {
                ForceLAT.Timer = 0;

                CreateWeapon(WeaponType.Force);
            }

            ForceLAT.Timer += Time.deltaTime;
        }

        if (LightningRingLAT.Level > 0)
        {
            if (LightningRingLAT.Timer > CurrentWeaponCoolDown(WeaponType.LightningRing, LightningRingLAT))
            {
                LightningRingLAT.Timer = 0;

                CreateWeapon(WeaponType.LightningRing);
            }

            LightningRingLAT.Timer += Time.deltaTime;
        }

        if (ShieldChasingLAT.Level > 0)
        {
            if (ShieldChasingLAT.Timer > CurrentWeaponCoolDown(WeaponType.ShieldChasing, ShieldChasingLAT))
            {
                ShieldChasingLAT.Timer = 0;

                for (int i = 0; i < CurrentWeaponCount(WeaponType.ShieldChasing, ShieldChasingLAT); i++)
                {
                    CreateWeapon(WeaponType.ShieldChasing);
                }
            }

            ShieldChasingLAT.Timer += Time.deltaTime;
        }

        if (RuneTracerLAT.Level > 0)
        {
            if (RuneTracerLAT.Timer > CurrentWeaponCoolDown(WeaponType.RuneTracer, RuneTracerLAT))
            {
                RuneTracerLAT.Timer = 0;

                for (int i = 0; i < CurrentWeaponCount(WeaponType.RuneTracer, RuneTracerLAT); i++)
                {
                    CreateWeapon(WeaponType.RuneTracer);
                }
            }

            RuneTracerLAT.Timer += Time.deltaTime;
        }

        if (NunchakuLAT.Level > 0)
        {
            if (NunchakuLAT.Timer > CurrentWeaponCoolDown(WeaponType.Nunchaku, NunchakuLAT))
            {
                NunchakuLAT.Timer = 0;

                for (int i = 0; i < CurrentWeaponCount(WeaponType.Nunchaku, NunchakuLAT); i++)
                {
                    CreateWeapon(WeaponType.Nunchaku);
                }
            }

            NunchakuLAT.Timer += Time.deltaTime;
        }

        //Debug.Log("WeaponSpawn!");
    }

    // ��ű� ����
    public void UpdateGetAccessory(AccessoryType pET)
    {
        var Temp = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(pET);

        if (pET == AccessoryType.RedAja)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.RedAja))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.Thunder)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Thunder))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.MagicHood)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.MagicHood))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.Bang)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Bang))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.Bracer)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Bracer))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.FloppyDisk)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.FloppyDisk))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.TheFlags)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.TheFlags))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.EmptyTome)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.EmptyTome))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.Times)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Times))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        else if (pET == AccessoryType.Attractorb)
        {

            if (Temp == AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryDataArr(AccessoryType.Attractorb))
            {
                GetAccessoryPlayerPercentageStat(this, Temp);
            }

            //GetAccessoryPlayerPercentageStat(this, AllManager.instance.ResourceM.AccessoryDataRedAja);
        }

        //Debug.Log("��ű� ����");

        if (Temp != null)
        {
            AllAccessory.Add(Temp);
        }
    }

    // ��ű� ���׷��̵�
    public void UpgradeAccessory(AccessoryType pET)
    {
        if (pET == AccessoryType.RedAja)
        {
            RedAjaLevel += 1;

            UpdateGetAccessory(AccessoryType.RedAja);
        }

        else if (pET == AccessoryType.Thunder)
        {
            ThunderLevel += 1;

            UpdateGetAccessory(AccessoryType.Thunder);
        }

        else if (pET == AccessoryType.MagicHood)
        {
            MagicHoodLevel += 1;

            UpdateGetAccessory(AccessoryType.MagicHood);
        }

        else if (pET == AccessoryType.Bang)
        {
            BangLevel += 1;

            UpdateGetAccessory(AccessoryType.Bang);
        }

        else if (pET == AccessoryType.Bracer)
        {
            BracerLevel += 1;

            UpdateGetAccessory(AccessoryType.Bracer);
        }

        else if (pET == AccessoryType.FloppyDisk)
        {
            FloppyDiskLevel += 1;

            UpdateGetAccessory(AccessoryType.FloppyDisk);
        }

        else if (pET == AccessoryType.TheFlags)
        {
            TheFlagsLevel += 1;

            UpdateGetAccessory(AccessoryType.TheFlags);
        }

        else if (pET == AccessoryType.EmptyTome)
        {
            EmptyTomeLevel += 1;

            UpdateGetAccessory(AccessoryType.EmptyTome);
        }

        else if (pET == AccessoryType.Times)
        {
            TimesLevel += 1;

            UpdateGetAccessory(AccessoryType.Times);
        }

        else if (pET == AccessoryType.Attractorb)
        {
            AttractorbLevel += 1;

            UpdateGetAccessory(AccessoryType.Attractorb);
        }
    }

    // �÷��̾� ������ ���ȿ� �ۼ�Ƽ�� ���� ����
    public void OriginStatApplicationPlayer(AccessoryType pET)
    {
        //
        // this.OriginStat.UnitHP 

        if (pET == AccessoryType.Bang)
        {
            if (BangLevel > 0)
            {
                float UnitPlayerDamage = 0;
                UnitPlayerDamage = this.PercentStat.UnitWeaponDamage;

                this.OriginStat.UnitWeaponDamage += UnitPlayerDamage;
            }
        }

        else if (pET == AccessoryType.Bracer)
        {
            if (BracerLevel > 0)
            {
                float UnitPlayerProjSpd = 0;
                UnitPlayerProjSpd = this.PercentStat.UnitWeaponProjSpeed;

                this.OriginStat.UnitWeaponProjSpeed += UnitPlayerProjSpd;
            }
        }

        else if (pET == AccessoryType.Times)
        {
            if (TimesLevel > 0)
            {
                int UnitPlayerProjCount = 0;
                UnitPlayerProjCount = this.PercentStat.UnitWeaponProjCount;

                this.OriginStat.UnitWeaponProjCount += UnitPlayerProjCount;
            }
        }

        else if (pET == AccessoryType.EmptyTome)
        {
            if (EmptyTomeLevel > 0)
            {
                float UnitPlayerRange = 0;
                UnitPlayerRange = this.PercentStat.UnitWeaponRange;

                this.OriginStat.UnitWeaponRange += UnitPlayerRange;
            }
        }

        else if (pET == AccessoryType.Thunder)
        {
            if (ThunderLevel > 0)
            {
                float UnitPlayerSpeed = 0;
                UnitPlayerSpeed = (this.OriginStat.UnitSpeed * this.PercentStat.UnitSpeed * 0.01f);

                this.OriginStat.UnitSpeed += UnitPlayerSpeed;
            }
        }

        else if (pET == AccessoryType.TheFlags)
        {
            if (TheFlagsLevel > 0)
            {
                float UnitPlayerRange = 0;
                float UnitPlayerSize = 0;
                UnitPlayerRange = this.PercentStat.UnitWeaponRange;
                UnitPlayerSize = this.PercentStat.UnitWeaponSize;

                this.OriginStat.UnitWeaponRange += UnitPlayerRange;
                this.OriginStat.UnitWeaponSize += UnitPlayerSize;
            }
        }

        else if (pET == AccessoryType.MagicHood)
        {
            if (MagicHoodLevel > 0)
            {
                float UnitPlayerDefense = 0;
                UnitPlayerDefense = this.PercentStat.UnitDefense;

                this.OriginStat.UnitDefense += UnitPlayerDefense;
            }
        }

        else if (pET == AccessoryType.RedAja)
        {
            if (RedAjaLevel > 0)
            {
                float UnitPlayerHP = 0;
                UnitPlayerHP = (this.OriginStat.UnitHP * this.PercentStat.UnitHP * 0.01f); // (this.OriginStat.UnitHP * this.PercentStat.UnitHP * 0.01f);

                var TempHP = this.OriginStat.UnitHP + UnitPlayerHP;

                this.OriginStat.UnitHP = TempHP;

                this.MaxHP = this.OriginStat.UnitHP;
            }
        }

        else if (pET == AccessoryType.FloppyDisk)
        {
            if (FloppyDiskLevel > 0)
            {
                float UnitPlayerDuration = 0;
                UnitPlayerDuration = this.PercentStat.UnitWeaponDuration;

                this.OriginStat.UnitWeaponDuration += UnitPlayerDuration;
            }
        }

        else if (pET == AccessoryType.Attractorb)
        {
            if (AttractorbLevel > 0)
            {
                float UnitPlayerGetGemRange = 0;
                UnitPlayerGetGemRange = (this.OriginStat.UnitGetGemRange * this.PercentStat.UnitGetGemRange * 0.01f);

                this.OriginStat.UnitGetGemRange += UnitPlayerGetGemRange;
            }
        }

    }


    // ��ű��� �нú� ���� ����
    public void GetAccessoryPlayerPercentageStat(Unit PlayerUnit, AccessoryData EqSTO)
    {
        var TempAccessorySTO = AllManager.instance.ResourceM.AccessoryDatas;

        // PercentDamage = (1 + EqSTO.stat / 100f);
        if (PlayerUnit == null)
        {
            Debug.Log("Unit NULL");
            return;
        }

        if (EqSTO == null) //
        {
            //Debug.Log("EQSTO NULL");
            return;
        }

        if (EqSTO.ET == TempAccessorySTO[0].ET)
        {
            PlayerUnit.PercentStat.UnitWeaponDamage += EqSTO.AccessoryStatSheet.UnitWeaponDamage;
        }

        else if (EqSTO.ET == TempAccessorySTO[1].ET)
        {
            PlayerUnit.PercentStat.UnitWeaponProjSpeed += EqSTO.AccessoryStatSheet.UnitWeaponProjSpeed;
        }

        else if (EqSTO.ET == TempAccessorySTO[2].ET)
        {
            PlayerUnit.PercentStat.UnitWeaponProjCount += EqSTO.AccessoryStatSheet.UnitWeaponProjCount;
        }

        else if (EqSTO.ET == TempAccessorySTO[3].ET)
        {
            PlayerUnit.PercentStat.UnitWeaponCoolDown += EqSTO.AccessoryStatSheet.UnitWeaponCoolDown;
        }

        else if (EqSTO.ET == TempAccessorySTO[4].ET)
        {
            PlayerUnit.PercentStat.UnitSpeed += EqSTO.AccessoryStatSheet.UnitSpeed;
        }

        else if (EqSTO.ET == TempAccessorySTO[5].ET)
        {
            PlayerUnit.PercentStat.UnitWeaponRange += EqSTO.AccessoryStatSheet.UnitWeaponRange;
            PlayerUnit.PercentStat.UnitWeaponSize += EqSTO.AccessoryStatSheet.UnitWeaponSize;
        }

        else if (EqSTO.ET == TempAccessorySTO[6].ET)
        {
            PlayerUnit.PercentStat.UnitDefense += EqSTO.AccessoryStatSheet.UnitDefense;
        }

        else if (EqSTO.ET == TempAccessorySTO[7].ET)
        {
            PlayerUnit.PercentStat.UnitHP += EqSTO.AccessoryStatSheet.UnitHP;
        }

        else if (EqSTO.ET == TempAccessorySTO[8].ET)
        {
            PlayerUnit.PercentStat.UnitWeaponDuration += EqSTO.AccessoryStatSheet.UnitWeaponDuration;
        }

        else if (EqSTO.ET == TempAccessorySTO[9].ET)
        {
            PlayerUnit.PercentStat.UnitGetGemRange += EqSTO.AccessoryStatSheet.UnitGetGemRange;
        }

    }

    // ���� ���� ã�ư��� �ְ�
    public WeaponLevelAndTimer ConvertWLATPlayer(WeaponData pweaponTypeO)
    {
        WeaponLevelAndTimer WLAT = null;

        //var Temp = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr()

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[0])
        {
            WLAT = AxeLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[1])
        {
            WLAT = KingBibleLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[2])
        {
            WLAT = PeachoneLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[3])
        {
            WLAT = FireWandLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[4])
        {
            WLAT = BananaLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[5])
        {
            WLAT = ForceLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[6])
        {
            WLAT = LightningRingLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[7])
        {
            WLAT = ShieldChasingLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[8])
        {
            WLAT = RuneTracerLAT;
        }

        if (pweaponTypeO == AllManager.instance.ResourceM.WeaponDatas[9])
        {
            WLAT = NunchakuLAT;
        }

        return WLAT;

    }

    // ��ű� ���� ã�ư��� �ְ� // �ٵ� �̰� ���� �̳��̳� ��Ʈ��Ʈ�� ���� �����ϸ� ���ﲨ �����ѵ�
    public int ConvertAccessoryLevelPlayer(AccessoryType pET)
    {
        var AccessoryLevel = 0;

        if (pET == AccessoryType.Bang)
        {
            AccessoryLevel += BangLevel;
        }

        if (pET == AccessoryType.Bracer)
        {
            AccessoryLevel += BracerLevel;
        }

        if (pET == AccessoryType.Times)
        {
            AccessoryLevel += TimesLevel;
        }

        if (pET == AccessoryType.EmptyTome)
        {
            AccessoryLevel += EmptyTomeLevel;
        }

        if (pET == AccessoryType.Thunder)
        {
            AccessoryLevel += ThunderLevel;
        }

        if (pET == AccessoryType.TheFlags)
        {
            AccessoryLevel += TheFlagsLevel;
        }

        if (pET == AccessoryType.MagicHood)
        {
            AccessoryLevel += MagicHoodLevel;
        }

        if (pET == AccessoryType.RedAja)
        {
            AccessoryLevel += RedAjaLevel;
        }

        if (pET == AccessoryType.FloppyDisk)
        {
            AccessoryLevel += FloppyDiskLevel;
        }

        if (pET == AccessoryType.Attractorb)
        {
            AccessoryLevel += AttractorbLevel;
        }

        //Debug.Log("AccessoryLevel : " + AccessoryLevel);

        return AccessoryLevel;
    }


    // ��ű��� �����ϴ� ���� ����ü ����
    public int CurrentWeaponCount(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        int WeaponCountValue = 0;
        WeaponCountValue = this.OriginStat.UnitWeaponProjCount;

        int pWeaponCount = WeaponDataSTO.Infos[pWLAT.Level].WeaponCount + WeaponCountValue;

        return pWeaponCount;
    }

    // ��ű��� ���ҵǴ� ������ ��Ÿ��
    public float CurrentWeaponCoolDown(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        float WeaponCoolDownValue = 0;
        WeaponCoolDownValue = (WeaponDataSTO.Infos[pWLAT.Level].WeaponCoolDown * this.OriginStat.UnitWeaponCoolDown * 0.01f);

        float pWeaponCoolDown = WeaponDataSTO.Infos[pWLAT.Level].WeaponCoolDown - WeaponCoolDownValue;

        return pWeaponCoolDown;
    }

    // ��ű��� �����ϴ� ���� ������ ��Ÿ�
    public float CurrentWeaponRange(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        float WeaponRangeValue = 0;
        WeaponRangeValue = (WeaponDataSTO.Infos[pWLAT.Level].WeaponRange * this.OriginStat.UnitWeaponRange * 0.01f);

        float pWeaponRange = WeaponDataSTO.Infos[pWLAT.Level].WeaponRange + WeaponRangeValue;

        return pWeaponRange;
    }

    // ��ű��� �����ϴ� ���� ������ ũ��
    public float CurrentWeaponSize(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        float WeaponSizeValue = 0;
        WeaponSizeValue = (WeaponDataSTO.Infos[pWLAT.Level].WeaponSize * this.OriginStat.UnitWeaponSize * 0.01f);

        float pWeaponSize = WeaponDataSTO.Infos[pWLAT.Level].WeaponSize + WeaponSizeValue;

        return pWeaponSize;
    }

    //���� ���� ������ ���ݼӵ�.
    public float CurrentWeaponSpeed(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        float WeaponSpeedValue = 0;
        WeaponSpeedValue = (WeaponDataSTO.Infos[pWLAT.Level].WeaponSpeed * this.OriginStat.UnitWeaponProjSpeed * 0.01f);

        float pWeaponSpeed = WeaponDataSTO.Infos[pWLAT.Level].WeaponSpeed + WeaponSpeedValue;

        return pWeaponSpeed;
    }

    //���� ���� ������ ���ݷ�.
    public float CurrentWeaponDamage(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        float WeaponDamageValue = 0;
        WeaponDamageValue = (WeaponDataSTO.Infos[pWLAT.Level].WeaponDamage * this.OriginStat.UnitWeaponDamage * 0.01f);

        float pWeaponDamage = WeaponDataSTO.Infos[pWLAT.Level].WeaponDamage + WeaponDamageValue;

        return pWeaponDamage;
    }

    //���� ���� ������ ���ӽð�.
    public float CurrentWeaponDuration(WeaponType pweaponType, WeaponLevelAndTimer pWLAT)
    {
        var WeaponDataSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pweaponType);

        float WeaponDurationValue = 0;
        WeaponDurationValue = (WeaponDataSTO.Infos[pWLAT.Level].WeaponDuration * this.OriginStat.UnitWeaponDuration * 0.01f);

        float pWeaponDuration = WeaponDataSTO.Infos[pWLAT.Level].WeaponDuration + WeaponDurationValue;

        return pWeaponDuration;
    }
    #endregion

    // �ó��� ȿ�� ���� Ȯ�� �� ó��
    public void CheckSynergyEffects(WeaponKeyword keyword)
    {
        if (keyword == WeaponKeyword.NONE) return;

        int keywordCount = 0;
        foreach (Weapon weapon in AllWeapon)
        {
            if (weapon.WeaponKeyword == keyword)
            {
                keywordCount++;
            }
        }

        if (keywordCount >= 2)
        {
            ApplySynergyEffect(keyword);
        }
    }

    // �ó��� ȿ�� ���� �Լ�
    public void ApplySynergyEffect(WeaponKeyword keyword)
    {
        switch (keyword)
        {
            case WeaponKeyword.Bound:
                foreach (Weapon weapon in AllWeapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.WeaponSpeed *= 1.5f;
                        Debug.Log("바운드");
                    }
                }
                break;
            case WeaponKeyword.Phoenix:
                foreach (Weapon weapon in AllWeapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.WeaponSpeed *= 1.5f;
                    }
                }
                break;
            case WeaponKeyword.Paladin:
                foreach (Weapon weapon in AllWeapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.WeaponRange *= 1.5f;
                    }
                }
                break;
            case WeaponKeyword.Sonic:
                foreach (Weapon weapon in AllWeapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.WeaponCoolDown *= 0.5f;
                    }
                }
                break;
            case WeaponKeyword.Holy:
                foreach (Weapon weapon in AllWeapon)
                {
                    if (weapon.WeaponKeyword == keyword)
                    {
                        weapon.WeaponSize *= 1.5f;
                    }
                }
                break;
                // �ٸ� Ű���忡 ���� �ó��� ȿ�� �߰� ����
        }
    }

    // �÷��̾� �̵�
    public void PlayerMove()
    {
        if (isDead == true) return;

        InputVec.x = Input.GetAxisRaw("Horizontal");
        InputVec.y = Input.GetAxisRaw("Vertical");

        if (InputVec.x != 0 || InputVec.y != 0)
        {
            unitAnimator.SetBool("Move", true);
        }
        else
        {
            unitAnimator.SetBool("Move", false);
        }

        Vector3 NextVec = InputVec.normalized * (this.OriginStat.UnitSpeed * Time.deltaTime);
        //RB.MovePosition(RB.position + NextVec);

        this.transform.position = this.transform.position + NextVec;

        //this.transform.position = this.transform.position + NextVec;
        if (InputVec.x != 0)
        {
            unitSpriteRenderer.flipX = InputVec.x >= 0;
        }
    }


    // ü�� ȸ�� �Լ�
    public void RecoveryCurrentHP()
    {
        int RecoveryHP = 25;

        if (this.CurrentHP < this.MaxHP)
        {
            this.CurrentHP += RecoveryHP;
        }

        else
        {
            Debug.Log("�̹� �ִ� ü�� �Դϴ�.");
        }

        AllManager.instance.UIM.UpdatePlayerHPBarImage(CurrentHP / MaxHP);
    }

    // �� ȹ�� �Լ�
    public void GetGem(float pGetGemCurrentCount)
    {
        //AllManager.instance.UIM.UpdateGemCountImage(pGetGemCurrentCount);
        AllManager.instance.GameModeM.AddGem(pGetGemCurrentCount);
    }

    // ���� �÷��̾� ���� ����
    public override void UnitStatSetting(Unit pUnit, UnitData pUnSTO)
    {
        if (pUnit == null)
        {
            Debug.Log("unit이 null임");
        }
        if (pUnSTO == null)
        {
            Debug.Log("unit STO가 null임");
        }
        pUnit.UnitSpriteRenderer.sprite = pUnSTO.SpriteImage;
        pUnit.UnitSpriteRenderer.material = pUnSTO.EliteMaterial;

        pUnit.OriginStat.UnitHP = pUnSTO.Playerinfos.HP;
        pUnit.MaxHP = pUnit.CurrentHP = pUnSTO.Playerinfos.HP;
        pUnit.OriginStat.UnitSpeed = (pUnSTO.Playerinfos.UnitSpeed * 1.5f);
        pUnit.OriginStat.UnitDefense = pUnSTO.Playerinfos.UnitDefense;
        pUnit.OriginStat.UnitGetGemRange = pUnSTO.Playerinfos.UnitGetGemRange;

        pUnit.UnitBoxCollider2D.size = new Vector2(pUnSTO.SizeX, pUnSTO.SizeY);
        pUnit.UnitBoxCollider2D.offset = new Vector2(pUnSTO.OffsetX, pUnSTO.OffsetY);

        if (pUnSTO.UnitAnimatorController != null)
        {
            pUnit.UnitAnimator.runtimeAnimatorController = pUnSTO.UnitAnimatorController;
            pUnit.UnitAnimator.speed = 1.0f; // �ִϸ��̼� ���
            pUnit.UnitAnimator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }

    public override void UnitAnimation(UnitAnimationState State)
    {
        base.UnitAnimation(State);
    }

}

#region
/*
         var WeaponSTO = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr(pWeapon.weaponType);

        var WeaponWLAT = ConvertWLATPlayer(WeaponSTO);

        switch (pWeapon.WeaponKeywords)
        {
            case WeaponKeyword.NONE:
                break;
            case WeaponKeyword.Bound:
                {
                    switch (pWeapon.weaponType)
                    {
                        case WeaponType.Axe:
                            {
                                if (AxeLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                            }
                            break;
                        case WeaponType.RuneTracer:
                            {
                                if (RuneTracerLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Phoenix:
                {
                    switch (pWeapon.weaponType)
                    {
                        case WeaponType.Peachone:
                            {
                                if (PeachoneLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                            }
                            break;
                        case WeaponType.FireWand:
                            {
                                if (FireWandLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Paladin:
                {
                    switch (pWeapon.weaponType)
                    {
                        case WeaponType.ShieldChasing:
                            {
                                if (ShieldChasingLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                            }
                            break;
                        case WeaponType.Banana:
                            {
                                if (BananaLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                            }
                            break;
                    }
                }
                break;
            case WeaponKeyword.Sonic:
                switch (pWeapon.weaponType)
                {
                    case WeaponType.LightningRing:
                        {
                            if (LightningRingLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                        }
                        break;
                    case WeaponType.Nunchaku:
                        {
                            if (NunchakuLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                        }
                        break;
                }
                break;
            case WeaponKeyword.Holy:
                switch (pWeapon.weaponType)
                {
                    case WeaponType.Force:
                        {
                            if (ForceLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                        }
                        break;
                    case WeaponType.KingBible:
                        {
                            if (KingBibleLAT.Level > 0) weaponKeywardList.Add(pWeapon.WeaponKeywords);
                        }
                        break;
                }
                break;
        }
 */
//if (Dis > Vector3.Distance(this.transform.position, GemTemp.transform.position))
//{
//    Dis = Vector3.Distance(this.transform.position, GemTemp.transform.position);
//    TargetGem = GemTemp;
//}
//if (GemTemp == null) return;

//var TempAccessory = 

//if (pweaponType == WeaponType.Axe)
//{
//    AxeLAT.Level++;

//    for (int i = 0; i < AllWeapon.Count; i++)
//    {
//        if (AllWeapon[i].GetComponent<WeaponAxe>())
//        {
//            AllWeapon[i].Upgrade(AxeLAT.Level);
//        }
//    }

//}
//if (InputVec.x <= 0)
//{
//    PlayerRight = false;
//}

//if (PlayerRight == true)
//{
//    TheScale.x *= -1;
//}

//Vector3 TheScale = this.transform.localScale;
//TheScale.x *= -1;
//this.transform.localScale = TheScale;
//if (TargetGem == null) return;

//if (TargetGem != null)
//{
//    Debug.Log("Gem Not Null");

//    //���� �ٲٰ�
//    Vector2 Dir = (this.transform.position - TargetGem.transform.position).normalized;

//    //float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
//    //TargetGem.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

//    //TargetGem.transform.position = TargetGem.transform.position + TargetGem.transform.right * (5.0f * Time.deltaTime);
//    var GemDir = Dir * 0.1f;

//    TargetGem.GemToPlayer(GemDir);
//}
//else
//{
//    Debug.Log("Gem Null");
//}

// �� ������ ���ؼ� ������

//if (AllManager.instance != null && AllManager.instance.GameModeM != null && AllManager.instance.GameModeM.GemList != null && AllManager.instance.GameModeM.GemList.Count > 0)
//{
//    int CountToGemSpawn = AllManager.instance.GameModeM.EnemyList.Count;

//    List<int> SpawnedGemIndices = new List<int>();

//    for (int i = 0; i < CountToGemSpawn; i++)
//    {

//        if (SpawnedGemIndices.Contains(i))
//        {

//        }
//        else
//        {
//            continue;
//        }

//        SpawnedGemIndices.Add(i);

//        var CheckNullGem = AllManager.instance.GameModeM.GemList[i];

//        if (CheckNullGem == null)
//        {
//            continue;
//        }

//        var Gemtrasform = AllManager.instance.GameModeM.GemList[i].transform;

//        if (Gemtrasform != null)
//        {
//            Debug.Log("Gem Not Null");
//            Vector2 Dir = this.transform.position - Gemtrasform.position;
//            float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
//            Gemtrasform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
//        }
//        else
//        {
//            Debug.Log("Gem NULL");
//        }

//        Gemtrasform.position = Gemtrasform.position + Gemtrasform.right * (5.0f * Time.deltaTime);

//      }


//}

//int CreateKingBibleCount = 0;

//for (int i = 0; i < AllWeapon.Count; i++)
//{
//    if (AllWeapon[i] == GetComponent<WeaponKingBible>())
//    {
//        if (AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMineKingBibleCount < CurrentWeaponCount(WeaponType.KingBible, KingBibleLAT))
//        {
//            AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMineKingBibleCount++;
//        }

//    }
//}


// ���⼭ AllWeapon�� �ִ� ��� ������ �ٽ� ����
// AllWeapon.GetComponent<KingBible>().Max = 3;
// AllWeapon.GetComponent<KingBible>().Current = 3;
//
// 1 2 3

//var KingBibles = this.GetComponentsInChildren<WeaponKingBible>();
//KingBibles.Length

//int PlayerWeaponKingBibleCount = 0;

//for (int i = 0; i < AllWeapon.Count; i++)
//{
//    if (AllWeapon[i].GetComponent<WeaponKingBible>())
//    {
//        AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMineKingBibleCount++;

//        if (AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMineKingBibleCount > AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMaxKingBibleCount)
//        {
//            AllWeapon[i].GetComponent<WeaponKingBible>().WeaponMaxKingBibleCount++;
//        }
//    }

//}

//Debug.Log("PlayerWeaponKingBibleCount : " + PlayerWeaponKingBibleCount);


//float xInput = Input.GetAxis("Horizontal");
//float yInput = Input.GetAxis("Vertical");

//float xSpeed = xInput * Xspeed * Time.deltaTime;
//float ySpeed = yInput * Yspeed * Time.deltaTime;

//�� �����ΰ���
// this.transform.position = this.transform.position + (this.transform.up * ySpeed);

//����
// this.transform.Rotate(new Vector3(0, 0, -xSpeed));

//if (SkillFull < SkillCool)
//{

//    SkillFull += Time.deltaTime;
//    AllManager.instance.UIM.UpdatePlayerSkillImage(SkillFull / SkillCool);
//    TempGo = Instantiate(ForceGo, null);
//    TempGo.transform.parent = this.transform;
//    if (TempGo.GetComponent<Force>())
//    {
//        yield return new WaitUntil(() => TempGo.GetComponent<Force>().spriteRenderer);
//        AllManager.instance.ResourceM.WeaponStat(TempGo.GetComponent<Force>(), AllManager.instance.ResourceM.STOForce);
//    }
//}
//else
//{
//    SkillFull = 0.0f;
//}

//switch (pweaponType)
//{
//    case WeaponType.None:
//        {
//        }
//        break;
//    case WeaponType.KingBible:
//        {
//            //TempGo = Instantiate(WeaponGo, null);
//            GameObject WeaponGo = new GameObject("KingBible");
//            WeaponGo.AddComponent<KingBible>();
//            WeaponGo.transform.parent = this.transform;
//            if (WeaponGo.GetComponent<KingBible>())
//            {
//                yield return new WaitUntil(() => WeaponGo.GetComponent<KingBible>().spriteRenderer);
//                AllManager.instance.ResourceM.WeaponStat(WeaponGo.GetComponent<KingBible>(), AllManager.instance.ResourceM.STOKingBible);
//            }
//            if (WeaponGo.GetComponent<Weapon>())
//            {
//                WeaponGo.GetComponent<Weapon>().WeaponInit(this);
//            }
//        }
//        break;
//    case WeaponType.SpellBook:
//        {
//            //TempGo = Instantiate(SpellBookGo, null);
//            GameObject SpellBookGo = new GameObject("SpellBook");
//            SpellBookGo.AddComponent<SpellBook>();
//            SpellBookGo.transform.parent = this.transform;
//            if (SpellBookGo.GetComponent<SpellBook>())
//            {
//                yield return new WaitUntil(() => SpellBookGo.GetComponent<SpellBook>().spriteRenderer);
//                AllManager.instance.ResourceM.WeaponStat(SpellBookGo.GetComponent<SpellBook>(), AllManager.instance.ResourceM.STOSpellBook);
//            }
//            if (SpellBookGo.GetComponent<Weapon>())
//            {
//                SpellBookGo.GetComponent<Weapon>().WeaponInit(this);
//            }
//        }
//        break;
//    case WeaponType.FireWandLauncher:
//        {
//            //TempGo = Instantiate(FireWandLauncherGo, null);
//            GameObject FireWandLauncherGo = new GameObject("FireWandLauncher");
//            FireWandLauncherGo.AddComponent<FireWandLauncher>();
//            FireWandLauncherGo.transform.parent = this.transform;
//            if (FireWandLauncherGo.GetComponent<FireWandLauncher>())
//            {
//                yield return new WaitUntil(() => FireWandLauncherGo.GetComponent<FireWandLauncher>().spriteRenderer);
//                AllManager.instance.ResourceM.WeaponStat(FireWandLauncherGo.GetComponent<FireWandLauncher>(), AllManager.instance.ResourceM.STOFireWandLauncher);
//            }
//            if (FireWandLauncherGo.GetComponent<Weapon>())
//            {
//                FireWandLauncherGo.GetComponent<Weapon>().WeaponInit(this);
//            }
//        }
//        break;
//    case WeaponType.ShieldChasingLauncher:
//        {
//            //TempGo = Instantiate(ShieldChasingLauncherGo, null);
//            GameObject ShieldChasingLauncherGo = new GameObject("ShieldChasingLauncher");
//            ShieldChasingLauncherGo.AddComponent<ShieldChasingLauncher>();
//            ShieldChasingLauncherGo.transform.parent = this.transform;
//            if (ShieldChasingLauncherGo.GetComponent<ShieldChasingLauncher>())
//            {
//                yield return new WaitUntil(() => ShieldChasingLauncherGo.GetComponent<ShieldChasingLauncher>().spriteRenderer);
//                AllManager.instance.ResourceM.WeaponStat(ShieldChasingLauncherGo.GetComponent<ShieldChasingLauncher>(), AllManager.instance.ResourceM.STOShieldChasingLauncher);
//            }
//            if (ShieldChasingLauncherGo.GetComponent<Weapon>())
//            {
//                ShieldChasingLauncherGo.GetComponent<Weapon>().WeaponInit(this);
//            }
//        }
//        break;
//    case WeaponType.Force:
//        {
//            //TempGo = Instantiate(ForceGo, null);
//            GameObject ForceGo = new GameObject("Force");
//            ForceGo.AddComponent<Force>();
//            ForceGo.transform.parent = this.transform;
//            if (ForceGo.GetComponent<Force>())
//            {
//                yield return new WaitUntil(() => ForceGo.GetComponent<Force>().spriteRenderer);
//                AllManager.instance.ResourceM.WeaponStat(ForceGo.GetComponent<Force>(), AllManager.instance.ResourceM.STOForce);
//            }
//            if (ForceGo.GetComponent<Weapon>())
//            {
//                ForceGo.GetComponent<Weapon>().WeaponInit(this);
//            }
//        }
//        break;
//    default:
//        break;
//}

//public IEnumerator CCreateWeapon(WeaponType pPT)
//{
//    yield return new WaitWhile(() => CreatingWeapon == true);

//    CreatingWeapon = true;

//    GameObject WeaponGo = null;

//    switch (pPT)
//    {
//        case WeaponType.None:
//            break;
//        case WeaponType.KingBible:
//            {
//                WeaponGo = new GameObject("KingBible");

//                WeaponGo.AddComponent<WeaponKingBible>();

//                WeaponGo.transform.parent = this.transform;
//                if (WeaponGo.GetComponent<WeaponKingBible>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponKingBible>().spriteRenderer);
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOKingBible, KingBibleLAT.Level);

//                    int CreateKingBibleCount = 1;

//                    for (int i = 0; i < AllWeapon.Count; i++)
//                    {
//                        if (AllWeapon[i].GetComponent<WeaponKingBible>())
//                        {
//                            CreateKingBibleCount++;
//                        }
//                    }

//                    WeaponGo.GetComponent<WeaponKingBible>().WeaponKingBibleCount = CreateKingBibleCount;
//                }
//            }
//            break;
//        case WeaponType.Force:
//            {
//                WeaponGo = new GameObject("Force");

//                WeaponGo.AddComponent<WeaponForce>();

//                if (WeaponGo.GetComponent<WeaponForce>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponForce>().spriteRenderer);
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOForce, ForceLAT.Level);
//                }
//            }
//            break;
//        case WeaponType.LightningRing:
//            {
//                if (AllManager.instance.GameModeM.EnemyList.Count > 10)
//                {

//                    for (int i = 0; i < 10; i++)
//                    {
//                        GameObject TempGo = Instantiate(WeaponGo, AllManager.instance.GameModeM.EnemyList[i].transform.position, Quaternion.identity);
//                        WeaponGo = new GameObject("LightningRing");
//                        WeaponGo.AddComponent<WeaponLightningRing>();

//                        WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[i].transform.position;
//                        WeaponGo.transform.rotation = Quaternion.identity;

//                        if (WeaponGo.GetComponent<WeaponLightningRing>())
//                        {
//                            yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().spriteRenderer);
//                            WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOLightningRing, LightningRingLAT.Level);
//                        }

//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < AllManager.instance.GameModeM.EnemyList.Count; i++)
//                    {
//                        GameObject TempGo = Instantiate(WeaponGo, AllManager.instance.GameModeM.EnemyList[i].transform.position, Quaternion.identity);
//                        WeaponGo = new GameObject("LightningRing");
//                        WeaponGo.AddComponent<WeaponLightningRing>();

//                        WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[i].transform.position;
//                        WeaponGo.transform.rotation = Quaternion.identity;

//                        if (WeaponGo.GetComponent<WeaponLightningRing>())
//                        {
//                            yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().spriteRenderer);
//                            WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOLightningRing, LightningRingLAT.Level);
//                        }

//                        Destroy(WeaponGo, 2.5f);
//                    }
//                }

//            }
//            break;
//        case WeaponType.FireWand:
//            {
//                WeaponGo = new GameObject("FireWand");

//                WeaponGo.AddComponent<WeaponFireWand>();

//                if (WeaponGo.GetComponent<WeaponFireWand>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponFireWand>().spriteRenderer);
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOFireWand, FireWandLAT.Level);
//                }
//            }
//            break;
//        case WeaponType.ShieldChasing:
//            {
//                WeaponGo = new GameObject("ShieldChasing");

//                WeaponGo.AddComponent<WeaponShieldChasing>();

//                if (WeaponGo.GetComponent<WeaponShieldChasing>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponShieldChasing>().spriteRenderer);
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOShieldChasing, ShieldChasingLAT.Level);
//                }
//            }
//            break;
//        case WeaponType.Axe:
//            {
//                WeaponGo = new GameObject("Axe");

//                WeaponGo.AddComponent<Axe>();



//                if (WeaponGo.GetComponent<Axe>())
//                {
//                    yield return new WaitUntil(() => WeaponGo.GetComponent<Axe>().spriteRenderer);
//                    WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOAxe, AxeLAT.Level);
//                    Vector3 One = this.transform.position;
//                    Vector3 Two = One + new Vector3(0, 5, 0); // ��
//                    Vector3 Three = Two + new Vector3(5, 0, 0); //������
//                    Vector3 Four = One + new Vector3(5, -10, 0); // ������ �Ʒ�


//                    WeaponGo.GetComponent<Axe>().AxeInit(One, Two, Three, Four);
//                }

//            }
//            break;
//        default:
//            break;
//    }

//    AllWeapon.Add(WeaponGo.GetComponent<Weapon>());

//    CreatingWeapon = false;
//}

//if (AllManager.instance.GameModeM.EnemyList.Count > 10)
//{

//    for (int i = 0; i < 10; i++)
//    {
//        //GameObject TempGo = Instantiate(WeaponGo, AllManager.instance.GameModeM.EnemyList[i].transform.position, Quaternion.identity);
//        WeaponGo = new GameObject("LightningRing");
//        WeaponGo.AddComponent<WeaponLightningRing>();

//        WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[i].transform.position;
//        WeaponGo.transform.rotation = Quaternion.identity;
//        WeaponGo.transform.localScale = Vector3.one;

//        if (WeaponGo.GetComponent<WeaponLightningRing>())
//        {
//            yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().spriteRenderer);
//            WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOLightningRing, LightningRingLAT.Level);
//        }

//    }
//}
//else
//{
//    for (int i = 0; i < AllManager.instance.GameModeM.EnemyList.Count; i++)
//    {
//        //GameObject TempGo = Instantiate(WeaponGo, AllManager.instance.GameModeM.EnemyList[i].transform.position, Quaternion.identity);
//        WeaponGo = new GameObject("LightningRing");
//        WeaponGo.AddComponent<WeaponLightningRing>();

//        WeaponGo.transform.position = AllManager.instance.GameModeM.EnemyList[i].transform.position;
//        WeaponGo.transform.rotation = Quaternion.identity;
//        WeaponGo.transform.localScale = Vector3.one;

//        if (WeaponGo.GetComponent<WeaponLightningRing>())
//        {
//            yield return new WaitUntil(() => WeaponGo.GetComponent<WeaponLightningRing>().spriteRenderer);
//            WeaponGo.GetComponent<Weapon>().WeaponInit(this, AllManager.instance.ResourceM.STOLightningRing, LightningRingLAT.Level);
//        }

//        Destroy(WeaponGo, 2.5f);
//    }
//}

//public void GetAccessory(AccessoryType pET)
//{
//    StartCoroutine(CGetAccessory(pET));
//}

//public IEnumerator CGetAccessory(AccessoryType pET)
//{
//    // ���� Temp�� ���̿��� �߻��ϴ� �����ΰ����� ���δ�.
//    // �ϴ� �� ���� ����
//    // ���� �ϴ� �ٽ� �ѹ�
//    yield return new WaitWhile(() => CreatingAccessory == true);

//    var Temp = AllManager.instance.ResourceM.ConvertAccessoryTypeToAccessoryData(pET);

//    CreatingAccessory = true;

//    if (pET == AccessoryType.None)
//    {

//    }

//    if (pET == AccessoryType.RedAja)
//    {
//        yield return new WaitUntil(() => (Temp = AllManager.instance.ResourceM.AccessoryDataRedAja));
//        if (Temp == AllManager.instance.ResourceM.AccessoryDataRedAja)
//        {
//            RedAjaLevel++;

//            GetAccessoryPlayerPercentageStat(this, Temp);
//            Debug.Log(Temp);
//            Debug.Log("��ű� ����");
//        }
//    }

//    if (pET == AccessoryType.Thunder)
//    {
//        ThunderLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.MagicHood)
//    {
//        MagicHoodLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.Bang)
//    {
//        BangLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.Bracer)
//    {
//        BracerLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.FloppyDisk)
//    {
//        FloppyDiskLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.TheFlags)
//    {
//        TheFlagsLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.EmptyTome)
//    {
//        EmptyTomeLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.Times)
//    {
//        TimesLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (pET == AccessoryType.Attractorb)
//    {
//        AttractorbLevel++;

//        GetAccessoryPlayerPercentageStat(this, Temp);
//    }

//    if (Temp != null)
//    {
//        AllAccessory.Add(Temp);
//    }

//    CreatingAccessory = false;
//}
#endregion
