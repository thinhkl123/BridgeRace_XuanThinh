using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickUI : MonoBehaviour
{
    private void Start()
    {
        GameMainManager.Ins.OnStateChange += GameManager_OnStateChange;

        Hide();
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameMainManager.Ins.state == GameMainManager.GameState.Playing || GameMainManager.Ins.state == GameMainManager.GameState.CountDownToStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    { 
        gameObject.SetActive(true);
    }
}
