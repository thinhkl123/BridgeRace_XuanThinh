using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAnimationEvent : MonoBehaviour
{
    [SerializeField] private Bot bot;

    public void EndFallAnimation()
    {
        bot.EndFallAnimation();
    }
}
