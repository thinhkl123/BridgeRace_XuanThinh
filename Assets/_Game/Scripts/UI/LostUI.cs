using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostUI : UICanvas
{
    [SerializeField] private Button reStartBtn;
    [SerializeField] private Button homeBtn;

    private void Start()
    {
        reStartBtn.onClick.AddListener(() =>
        {
            Close(0);
            LevelManager.Ins.LoadLevel(LevelManager.Ins.curLevel);
        });

        homeBtn.onClick.AddListener(() =>
        {
            Close(0);
            GameMainManager.Ins.state = GameMainManager.GameState.WaitToStart;
            UIManager.Ins.OpenUI<StartUI>();
        });
    }
}
