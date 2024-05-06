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
            if (LevelManager.Ins.curLevel+1 <= LevelManager.Ins.MaxLevel)
            {
                Close(0);
                LevelManager.Ins.curLevel++;
                LevelManager.Ins.LoadLevel(LevelManager.Ins.curLevel);
            }
        });

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

    private void OnDestroy()
    {
        FinishPoint.OnWin -= FinishPoint_OnWin;
    }

    private void FinishPoint_OnWin(object sender, System.EventArgs e)
    {
        UpdateLevelUpText();
    }

    public void UpdateLevelUpText()
    {
        levelUpText.text = (LevelManager.Ins.curLevel+1).ToString();
        Debug.Log(levelUpText.text);
    }
}
