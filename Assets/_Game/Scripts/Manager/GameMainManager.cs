using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : Singleton<GameMainManager>
{
    //public static GameMainManager instance {  get; private set; }

    public event EventHandler OnStateChange;

    public enum GameState
    {
        WaitToStart,
        CountDownToStart,
        Playing,
        Pause,
        GameOver
    }

    public GameState state;

    private float countDownToStartTime = 3f;

    private void Awake()
    {
        //instance = this;
        state = GameState.WaitToStart;
    }

    private void Start()
    {
        LevelManager.Ins.OnLoadLevel += LevelManager_OnLoadLevel;

        UIManager.Ins.OpenUI<StartUI>();

        //Open WinUI to update score level up
        UIManager.Ins.OpenUI<WinUI>();
        UIManager.Ins.CloseUI<WinUI>();

        //Open PlayUI to avoid bug
        UIManager.Ins.OpenUI<PlayUI>();
        UIManager.Ins.CloseUI<PlayUI>();
    }

    private void OnDestroy()
    {
        //if (LevelManager.Ins != null)
            //LevelManager.Ins.OnLoadLevel -= LevelManager_OnLoadLevel;
    }

    private void LevelManager_OnLoadLevel(object sender, EventArgs e)
    {
        state = GameState.CountDownToStart;
        UIManager.Ins.OpenUI<CountDownUI>();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.WaitToStart:
                break;
            case GameState.CountDownToStart:
                countDownToStartTime -= Time.deltaTime;
                if (countDownToStartTime <= 0f)
                {
                    state = GameState.Playing;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                    UIManager.Ins.CloseUI<CountDownUI>();
                    countDownToStartTime = 3f;
                }
                break;
            case GameState.Playing:
                break;
            case GameState.Pause:
                break;
            case GameState.GameOver: 
                break;
        }
    }

    public void StateChange()
    {
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    public float GetCountDownTime()
    {
        return countDownToStartTime;
    }
}
