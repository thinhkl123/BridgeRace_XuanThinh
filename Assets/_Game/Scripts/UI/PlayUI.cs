using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : UICanvas
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            Close(0);
            UIManager.Ins.OpenUI<PauseUI>();
            GameMainManager.Ins.state = GameMainManager.GameState.Pause;
        });
    }

    public void UpdateLevelText()
    {
        levelText.text = "Level " + (LevelManager.Ins.curLevel).ToString();
    }
}
