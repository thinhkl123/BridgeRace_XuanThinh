using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : UICanvas
{
    [SerializeField] private Button startBtn;
    [SerializeField] private LevelButton[] levelBtnList;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform levelContainer;
    [SerializeField] private Button settingBtn;

    private RectTransform target;

    private void OnEnable()
    {
        startBtn.onClick.AddListener(() =>
        {
            StartButtonClick();   
        });

        settingBtn.onClick.AddListener(() =>
        {
            UIManager.Ins.OpenUI<SettingUI>();
        });
    }

    private void OnDisable()
    {
        startBtn.onClick.RemoveListener(() => 
        {  
            StartButtonClick(); 
        });
    }


    private void Start()
    {
        LevelButton.OnLevelChange += LevelButton_OnLevelChange;
        FinishPoint.OnWin += FinishPoint_OnWin;

        target = levelContainer;

        InitLevelBtn();
    }

    private void OnDestroy()
    {
        LevelButton.OnLevelChange -= LevelButton_OnLevelChange;
        FinishPoint.OnWin -= FinishPoint_OnWin;
    }

    private void FinishPoint_OnWin(object sender, System.EventArgs e)
    {
        InitLevelBtn();
    }

    private void LevelButton_OnLevelChange(object sender, System.EventArgs e)
    {
        levelBtnList[LevelManager.Ins.curLevel - 1].SetNonSelect();
    }

    private void StartButtonClick()
    {
        //Debug.Log("Start");
        //GameMainManager.instance.state = GameMainManager.GameState.CountDownToStart;
        Close(0);
        LevelManager.Ins.LoadLevel(LevelManager.Ins.curLevel);
        GameMainManager.Ins.StateChange();
    }

    private void InitLevelBtn()
    {
        int maxLevel = LevelManager.Ins.curMaxLevel;

        for (int i = 0; i < maxLevel-1; i++)
        {
            levelBtnList[i].SetActive();
            levelBtnList[i].SetNonSelect();
            target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y - 400f, target.localPosition.z);
        }

        levelBtnList[maxLevel-1].SetActive();
        levelBtnList[maxLevel-1].SetSelect();

        if (LevelManager.Ins.curLevel > 1)
        {
            SnapTo(levelBtnList[maxLevel-1].GetComponent<RectTransform>());
        }

        for (int i=maxLevel; i<levelBtnList.Length; i++)
        {
            levelBtnList[i].SetNonActive();
            levelBtnList[i].SetNonSelect();
        }
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        levelContainer.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(levelContainer.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
}
