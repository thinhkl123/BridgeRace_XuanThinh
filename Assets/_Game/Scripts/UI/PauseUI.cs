using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UICanvas
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button reStartBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            Close(0);
            GameMainManager.Ins.state = GameMainManager.GameState.Playing;
            UIManager.Ins.OpenUI<PlayUI>();
        });

        reStartBtn.onClick.AddListener(() =>
        {
            Close(0);
            LevelManager.Ins.LoadLevel(LevelManager.Ins.curLevel);
        });

        homeBtn.onClick.AddListener(() =>
        {
            Close(0);
            UIManager.Ins.OpenUI<StartUI>();
            GameMainManager.Ins.state = GameMainManager.GameState.WaitToStart;
        });
    }

    public void UpdateLevelText()
    {
        levelText.text = "Level " + LevelManager.Ins.curLevel.ToString();
    }
}
