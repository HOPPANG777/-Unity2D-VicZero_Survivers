using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BackgroundAS;
    public AudioSource UIClickAS;
    public AudioSource LevelUpAS;
    public AudioSource TreasureChestAS;

    public List<AudioSource> EffectAudioList = new List<AudioSource>();

    void Start()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(WeaponType)).Length; i++)
        {
            GameObject Go = new GameObject(((WeaponType)i).ToString() + "-AS");

            Go.transform.parent = this.transform;
            AudioSource audioSource = Go.AddComponent<AudioSource>();


            audioSource.clip = AllManager.instance.ResourceM.ConvertWeaponTypeToWeaponDataArr((WeaponType)i).AttackingAudioClip;

            audioSource.playOnAwake = false;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.spatialBlend = 0.5f;

            EffectAudioList.Add(audioSource);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(UnitType)).Length; i++)
        {
            GameObject HittedGo = new GameObject("Hitted-" + ((UnitType)i).ToString() + "-AS");
            GameObject DeathGo = new GameObject("Death-" + ((UnitType)i).ToString() + "-AS");

            HittedGo.transform.parent = this.transform;
            DeathGo.transform.parent = this.transform;

            AudioSource hittedSound = HittedGo.AddComponent<AudioSource>();
            AudioSource deathSound = DeathGo.AddComponent<AudioSource>();

            hittedSound.clip = AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr((UnitType)i).HittedAudioClip;
            deathSound.clip = AllManager.instance.ResourceM.ConvertUnitTypeToUnitDataArr((UnitType)i).DeathAudioClip;

            hittedSound.playOnAwake = false;
            hittedSound.rolloffMode = AudioRolloffMode.Linear;
            hittedSound.spatialBlend = 0.5f;

            deathSound.playOnAwake = false;
            deathSound.rolloffMode = AudioRolloffMode.Linear;
            deathSound.spatialBlend = 0.5f;

            EffectAudioList.Add(hittedSound);
            EffectAudioList.Add(deathSound);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(ItemType)).Length; i++)
        {
            GameObject Go = new GameObject(((ItemType)i).ToString() + "-AS");

            Go.transform.parent = this.transform;

            AudioSource audioSource = Go.AddComponent<AudioSource>();

            audioSource.clip = AllManager.instance.ResourceM.ConvertItemTypeToItemDataArr((ItemType)i).GetItemAudioClip;

            audioSource.playOnAwake = false;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.spatialBlend = 0.5f;

            EffectAudioList.Add(audioSource);
        }
    }

    public void WeaponAttackingEffectAudioPlay(WeaponType pweaponType)
    {
        int PlayingAudioCount = 0;

        for (int i = 0; i < EffectAudioList.Count; i++)
        {
            if (EffectAudioList[i].isPlaying) PlayingAudioCount++;
        }

        if (PlayingAudioCount < 6)
        {
            for (int i = 0; i < EffectAudioList.Count; i++)
            {
                if (EffectAudioList[i].gameObject.name.Contains(pweaponType.ToString()))
                {
                    EffectAudioList[i].Play();
                    break;
                }
            }
        }
    }

    public void UnitHittedEffectAudioPlay(UnitType pUnitType)
    {
        int PlayingAudioCount = 0;

        for (int i = 0; i < EffectAudioList.Count; i++)
        {
            if (EffectAudioList[i].isPlaying) PlayingAudioCount++;
        }

        if (PlayingAudioCount < 6)
        {
            for (int i = 0; i < EffectAudioList.Count; i++)
            {
                if (EffectAudioList[i].gameObject.name.Contains("Hitted-" + pUnitType.ToString()))
                {
                    EffectAudioList[i].Play();
                    break;
                }
            }
        }
    }

    public void UnitDeathEffectAudioPlay(UnitType pUnitType)
    {
        int PlayingAudioCount = 0;

        for (int i = 0; i < EffectAudioList.Count; i++)
        {
            if (EffectAudioList[i].isPlaying) PlayingAudioCount++;
        }

        if (PlayingAudioCount < 6)
        {
            for (int i = 0; i < EffectAudioList.Count; i++)
            {
                if (EffectAudioList[i].gameObject.name.Contains("Death-" + pUnitType.ToString()))
                {
                    EffectAudioList[i].Play();
                    break;
                }
            }
        }
    }

    public void ItemGetEffectAudioPlay(ItemType pItemType)
    {
        int PlayingAudioCount = 0;

        for (int i = 0; i < EffectAudioList.Count; i++)
        {
            if (EffectAudioList[i].isPlaying) PlayingAudioCount++;
        }

        if (PlayingAudioCount < 6)
        {
            for (int i = 0; i < EffectAudioList.Count; i++)
            {
                if (EffectAudioList[i].gameObject.name.Contains(pItemType.ToString()))
                {
                    EffectAudioList[i].Play();
                    break;
                }
            }
        }
    }

    public void BackgroundAudioPlay(PanelStyle pPS)
    {
        if (pPS == PanelStyle.GameStart)
        {
            BackgroundAS.Play();
        }

        else if (pPS == PanelStyle.GameOver)
        {
            BackgroundAS.Stop();
        }

        else if (pPS == PanelStyle.Level)
        {
            //BackgroundAS.Pause();
            BackgroundAS.volume = 0.1f;
        }

        else if (pPS == PanelStyle.TreasureFound)
        {
            //BackgroundAS.Pause();
            BackgroundAS.volume = 0.1f;
        }

        else if (pPS == PanelStyle.Pause)
        {
            //BackgroundAS.Pause();
            BackgroundAS.volume = 0.1f;
        }

        else if (pPS == PanelStyle.GameClear)
        {
            BackgroundAS.Stop();
        }
    }

    public void UIClickAudioPlay()
    {
        UIClickAS.Play();
    }

    public void LevelUpAudioPlay()
    {
        LevelUpAS.Play();
    }

    public void TreasureFoundAudioPlay()
    {
        TreasureChestAS.Play();
    }

    public void AudioSetting()
    {
        BackgroundAS.volume = 0.3f;
        BackgroundAS.loop = true;
        BackgroundAS.playOnAwake = false;
        BackgroundAS.rolloffMode = AudioRolloffMode.Linear;

        UIClickAS.volume = 0.5f;
        UIClickAS.playOnAwake = false;
        UIClickAS.rolloffMode = AudioRolloffMode.Linear;

        LevelUpAS.volume = 0.5f;
        LevelUpAS.playOnAwake = false;
        LevelUpAS.rolloffMode = AudioRolloffMode.Linear;

        TreasureChestAS.volume = 0.5f;
        TreasureChestAS.playOnAwake = false;
        TreasureChestAS.rolloffMode = AudioRolloffMode.Linear;
    }
}
