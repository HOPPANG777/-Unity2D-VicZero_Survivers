using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AllManager.instance.GameModeM.GameStop == false)
            {
                AllManager.instance.UIM.OnPanel(PanelStyle.Pause);
                return;
            }
            else if (AllManager.instance.GameModeM.GameStop == true)
            {
                AllManager.instance.UIM.OffPanel();
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            AllManager.instance.GameModeM.GameOver();
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            AllManager.instance.GameModeM.GameClear();
        }

    }
}
