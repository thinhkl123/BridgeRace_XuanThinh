using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public static event EventHandler OnWin;
    public static event EventHandler<OnLoseEventArgs> OnLose;

    public class OnLoseEventArgs : EventArgs
    {
        public Bot bot;
    }

    [SerializeField] private GameObject particle1;
    [SerializeField] private GameObject particle2;

    private void Start()
    {
        HideParticle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Win");
            AudioManager.Ins.PlayWinSound();
            GameMainManager.Ins.state = GameMainManager.GameState.GameOver;
            ShowParicle();
            OnWin?.Invoke(this, EventArgs.Empty);
            Invoke(nameof(Win), 1f);
        }
        else if (other.CompareTag("Bot"))
        {
            //Debug.Log("Lose");
            AudioManager.Ins.PlayLoseSound();
            GameMainManager.Ins.state = GameMainManager.GameState.GameOver;
            ShowParicle();
            Bot bot = other.GetComponent<Bot>();
            bot.ClearBrick();
            OnLose?.Invoke(this, new OnLoseEventArgs
            {
                bot = bot
            });
            Invoke(nameof(Lose), 1f);
        }
    }

    private void HideParticle()
    {
        particle1.SetActive(false);
        particle2.SetActive(false);
    }

    private void ShowParicle()
    {
        particle1.SetActive(true);
        particle2.SetActive(true);
    }

    private void Win()
    {
        UIManager.Ins.OpenUI<WinUI>();
        UIManager.Ins.GetUI<WinUI>().UpdateLevelUpText();
    }

    private void Lose()
    {
        UIManager.Ins.OpenUI<LostUI>();
    }
}
