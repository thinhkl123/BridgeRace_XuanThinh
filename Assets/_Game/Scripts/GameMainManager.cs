using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour
{
    public static GameMainManager instance {  get; private set; }

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
        instance = this;
        state = GameState.WaitToStart;
        UIManager.Ins.OpenUI<StartUI>();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.WaitToStart:
                countDownToStartTime = 3f;
                break;
            case GameState.CountDownToStart:
                countDownToStartTime -= Time.deltaTime;
                if (countDownToStartTime <= 0f)
                {
                    state = GameState.Playing;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                    UIManager.Ins.CloseUI<CountDownUI>();
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
