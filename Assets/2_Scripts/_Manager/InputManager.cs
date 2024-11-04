using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (All_Manager.instance.GameMode_M.Game_Stop == false)
            {
                All_Manager.instance.UI_M.On_Panel(Panel_Style.Pause);
                return;
            }
            else if (All_Manager.instance.GameMode_M.Game_Stop == true)
            {
                All_Manager.instance.UI_M.Off_Panel();
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            All_Manager.instance.GameMode_M.GameOver();
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            All_Manager.instance.GameMode_M.GameClear();
        }

    }
}
