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

    private RectTransform target;

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
        levelBtnList[LevelManager.instance.curLevel - 1].SetNonSelect();
    }

    private void StartButtonClick()
    {
        //Debug.Log("Start");
        GameMainManager.instance.state = GameMainManager.GameState.CountDownToStart;
        GameMainManager.instance.StateChange();
        Close(0);
        LevelManager.instance.LoadLevel(LevelManager.instance.curLevel);
        UIManager.Ins.OpenUI<CountDownUI>();
    }

    private void InitLevelBtn()
    {
        int maxLevel = LevelManager.instance.curMaxLevel;

        for (int i = 0; i < maxLevel-1; i++)
        {
            levelBtnList[i].SetActive();
            levelBtnList[i].SetNonSelect();
            target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y - 400f, target.localPosition.z);
        }

        levelBtnList[maxLevel-1].SetActive();
        levelBtnList[maxLevel-1].SetSelect();
        if (LevelManager.instance.curLevel > 1)
        {
            SnapTo(target);
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
