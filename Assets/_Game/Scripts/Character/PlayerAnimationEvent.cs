using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player player;

    public void EndFallAnimation()
    {
        player.EndFallAnimation();
    }
}
