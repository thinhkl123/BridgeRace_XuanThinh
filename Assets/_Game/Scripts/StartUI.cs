using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : UICanvas
{
    [SerializeField] private Button startBtn;

    private void OnEnable()
    {
        startBtn.onClick.AddListener(() =>
        {
            StartButtonClick();   
        });
    }

    private void OnDisable()
    {
        startBtn.onClick.RemoveListener(() => 
        {  
            StartButtonClick(); 
        });
    }

    private void StartButtonClick()
    {
        //Debug.Log("Start");
        GameMainManager.instance.state = GameMainManager.GameState.CountDownToStart;
        GameMainManager.instance.StateChange();
        Close(0);
        UIManager.Ins.OpenUI<CountDownUI>();
    }
}
