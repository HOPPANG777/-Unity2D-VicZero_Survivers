using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Manager : Singleton_Base<All_Manager>
{
    //public static All_Manager instance = null;

    public UI_Manager UI_M;
    public Unit_Manager Unit_M;
    public GameMode_Manager GameMode_M;
    public Pool_Manager Pool_M;
    public Resource_Manager Resource_M;
    public Audio_Manager Audio_M;

    public Weapon_Type TEST_WP;
    public Equipment_Type TEST_EQ;
    public Panel_Style TEST_PS;

    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //}

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;

        if (UI_M == null) Debug.Log("UI_Manager 없음");
        if (Unit_M == null) Debug.Log("Unit_Manager 없음");
        if (GameMode_M == null) Debug.Log("GameMode_Manager 없음");
        if (Pool_M == null) Debug.Log("Pool_Manager 없음");
        if (Resource_M == null) Debug.Log("Resource_Manager 없음");
        if (Audio_M == null) Debug.Log("Audio_Manager 없음");

        GameMode_M.Game_Start();
    }

}
