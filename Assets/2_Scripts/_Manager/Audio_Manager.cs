using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : Singleton<Audio_Manager>
{
    public AudioSource Background_AS;
    public AudioSource UI_Click_AS;
    public AudioSource Level_Up_AS;
    public AudioSource Treasure_Chest_AS;

    public List<AudioSource> Effect_Audio_List = new List<AudioSource>();

    void Start()
    {


        for (int i = 0; i < System.Enum.GetValues(typeof(Weapon_Type)).Length; i++)
        {
            GameObject Go = new GameObject(((Weapon_Type)i).ToString() + "-AS");

            Go.transform.parent = this.transform;

            Go.AddComponent<AudioSource>();

            Go.GetComponent<AudioSource>().clip = All_Manager.instance.Resource_M.Convert_Weapon_Type_To_STO_Weapon_Arr((Weapon_Type)i).Attacking_Audio_Clip;

            Go.GetComponent<AudioSource>().playOnAwake = false;
            Go.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            Go.GetComponent<AudioSource>().spatialBlend = 0.5f;

            Effect_Audio_List.Add(Go.GetComponent<AudioSource>());
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(Unit_Type)).Length; i++)
        {
            GameObject Hitted_Go = new GameObject("Hitted-" + ((Unit_Type)i).ToString() + "-AS");
            GameObject Death_Go = new GameObject("Death-" + ((Unit_Type)i).ToString() + "-AS");

            Hitted_Go.transform.parent = this.transform;
            Death_Go.transform.parent = this.transform;

            Hitted_Go.AddComponent<AudioSource>();
            Death_Go.AddComponent<AudioSource>();

            Hitted_Go.GetComponent<AudioSource>().clip = All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr((Unit_Type)i).Hitted_Audio_Clip;
            Death_Go.GetComponent<AudioSource>().clip = All_Manager.instance.Resource_M.Convert_Unit_Type_To_STO_Unit_Arr((Unit_Type)i).Death_Audio_Clip;

            Hitted_Go.GetComponent<AudioSource>().playOnAwake = false;
            Hitted_Go.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            Hitted_Go.GetComponent<AudioSource>().spatialBlend = 0.5f;

            Death_Go.GetComponent<AudioSource>().playOnAwake = false;
            Death_Go.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            Death_Go.GetComponent<AudioSource>().spatialBlend = 0.5f;

            Effect_Audio_List.Add(Hitted_Go.GetComponent<AudioSource>());
            Effect_Audio_List.Add(Death_Go.GetComponent<AudioSource>());
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(MISC_Type)).Length; i++)
        {
            GameObject Go = new GameObject(((MISC_Type)i).ToString() + "-AS");

            Go.transform.parent = this.transform;

            Go.AddComponent<AudioSource>();

            Go.GetComponent<AudioSource>().clip = All_Manager.instance.Resource_M.Convert_MISC_Type_To_STO_MISC_Arr((MISC_Type)i).Get_MISC_Audio_Clip;

            Go.GetComponent<AudioSource>().playOnAwake = false;
            Go.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            Go.GetComponent<AudioSource>().spatialBlend = 0.5f;

            Effect_Audio_List.Add(Go.GetComponent<AudioSource>());
        }
    }

    public void Weapon_Attacking_Effect_Audio_Play(Weapon_Type p_WT)
    {
        int Playing_Audio_Count = 0;

        for (int i = 0; i < Effect_Audio_List.Count; i++)
        {
            if (Effect_Audio_List[i].isPlaying) Playing_Audio_Count++;
        }

        if (Playing_Audio_Count < 6)
        {
            for (int i = 0; i < Effect_Audio_List.Count; i++)
            {
                if (Effect_Audio_List[i].gameObject.name.Contains(p_WT.ToString()))
                {
                    Effect_Audio_List[i].Play();
                    break;
                }
            }
        }
    }

    public void Unit_Hitted_Effect_Audio_Play(Unit_Type p_UT)
    {
        int Playing_Audio_Count = 0;

        for (int i = 0; i < Effect_Audio_List.Count; i++)
        {
            if (Effect_Audio_List[i].isPlaying) Playing_Audio_Count++;
        }

        if (Playing_Audio_Count < 6)
        {
            for (int i = 0; i < Effect_Audio_List.Count; i++)
            {
                if (Effect_Audio_List[i].gameObject.name.Contains("Hitted-" + p_UT.ToString()))
                {
                    Effect_Audio_List[i].Play();
                    break;
                }
            }
        }
    }

    public void Unit_Death_Effect_Audio_Play(Unit_Type p_UT)
    {
        int Playing_Audio_Count = 0;

        for (int i = 0; i < Effect_Audio_List.Count; i++)
        {
            if (Effect_Audio_List[i].isPlaying) Playing_Audio_Count++;
        }

        if (Playing_Audio_Count < 6)
        {
            for (int i = 0; i < Effect_Audio_List.Count; i++)
            {
                if (Effect_Audio_List[i].gameObject.name.Contains("Death-" + p_UT.ToString()))
                {
                    Effect_Audio_List[i].Play();
                    break;
                }
            }
        }
    }

    public void MISC_Get_Effect_Audio_Play(MISC_Type p_MT)
    {
        int Playing_Audio_Count = 0;

        for (int i = 0; i < Effect_Audio_List.Count; i++)
        {
            if (Effect_Audio_List[i].isPlaying) Playing_Audio_Count++;
        }

        if (Playing_Audio_Count < 6)
        {
            for (int i = 0; i < Effect_Audio_List.Count; i++)
            {
                if (Effect_Audio_List[i].gameObject.name.Contains(p_MT.ToString()))
                {
                    Effect_Audio_List[i].Play();
                    break;
                }
            }
        }
    }

    public void Background_Audio_Play(Panel_Style p_PS)
    {
        if (p_PS == Panel_Style.GameStart)
        {
            Background_AS.Play();
        }

        else if (p_PS == Panel_Style.GameOver)
        {
            Background_AS.Stop();
        }

        else if (p_PS == Panel_Style.Level)
        {
            Background_AS.Pause();
        }

        else if (p_PS == Panel_Style.TreasureFound)
        {
            Background_AS.Pause();
        }

        else if (p_PS == Panel_Style.Pause)
        {
            Background_AS.Pause();
        }
    }

    public void UI_Click_Audio_Play()
    {
        UI_Click_AS.Play();
    }

    public void Level_Up_Audio_Play()
    {
       Level_Up_AS.Play();
    }

    public void Treasure_Found_Audio_Play()
    {
        Treasure_Chest_AS.Play();
    }

    public void Audio_Setting()
    {
        Background_AS.volume = 0.3f;
        Background_AS.loop = true;
        Background_AS.playOnAwake = false;
        Background_AS.rolloffMode = AudioRolloffMode.Linear;

        UI_Click_AS.volume = 0.5f;
        UI_Click_AS.playOnAwake = false;
        UI_Click_AS.rolloffMode = AudioRolloffMode.Linear;

        Level_Up_AS.volume = 0.5f;
        Level_Up_AS.playOnAwake = false;
        Level_Up_AS.rolloffMode = AudioRolloffMode.Linear;

        Treasure_Chest_AS.volume = 0.5f;
        Treasure_Chest_AS.playOnAwake = false;
        Treasure_Chest_AS.rolloffMode = AudioRolloffMode.Linear;
    }
}
