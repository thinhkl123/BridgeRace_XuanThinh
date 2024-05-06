using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UICanvas
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button reStartBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private TextMeshProUGUI levelUpText;

    private void Start()
    {
        FinishPoint.OnWin += FinishPoint_OnWin;

        continueBtn.onClick.AddListener(() =>
        {
            Close(0);
            LevelManager.instance.curLevel++;
            LevelManager.instance.LoadLevel(LevelManager.instance.curLevel);
        });

        reStartBtn.onClick.AddListener(() =>
        {
            Close(0);
            LevelManager.instance.LoadLevel(LevelManager.instance.curLevel);
        });

        homeBtn.onClick.AddListener(() =>
        {
            Close(0);
            GameMainManager.instance.state = GameMainManager.GameState.WaitToStart;
            UIManager.Ins.OpenUI<StartUI>();
        });
    }

    private void OnDestroy()
    {
        FinishPoint.OnWin -= FinishPoint_OnWin;
    }

    private void FinishPoint_OnWin(object sender, System.EventArgs e)
    {
        UpdateLevelUpText();
    }

    private void UpdateLevelUpText()
    {
        levelUpText.text = (LevelManager.instance.curLevel+1).ToString();
    }
}
