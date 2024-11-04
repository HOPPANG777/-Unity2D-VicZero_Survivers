using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllManager : Singleton<AllManager>
{
    //public static AllManager instance = null;

    public UIManager UIM;
    public UnitManager UnitM;
    public GameModeManager GameModeM;
    public PoolManager PoolM;
    public ResourceManager ResourceM;
    public AudioManager AudioM;
    public SynergyManager SynergyM;
    public InputManager inputManager;

    public WeaponType TESTWP;
    public AccessoryType TESTEQ;
    public PanelStyle TESTPS;

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
        for (int i = 0; i < GameModeM.TilemapList.Count; i++)
        {
            GameModeM.TilemapList[i].layer = 9;
        }

        if (UIM == null) Debug.Log("UIManager ����");
        if (UnitM == null) Debug.Log("UnitManager ����");
        if (GameModeM == null) Debug.Log("GameModeManager ����");
        if (PoolM == null) Debug.Log("PoolManager ����");
        if (ResourceM == null) Debug.Log("ResourceManager ����");
        if (AudioM == null) Debug.Log("AudioManager ����");
        if (SynergyM == null) Debug.Log("SynergyManager ����");

        GameModeM.GameStart();
    }

}
