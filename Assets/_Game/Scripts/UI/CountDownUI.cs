using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownUI : UICanvas
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Animator animator;

    private float currentTime;
    private float prevTime;

    private void Update()
    {
        currentTime = Mathf.Ceil(GameMainManager.Ins.GetCountDownTime());
        timeText.text = currentTime.ToString();

        if (prevTime != currentTime)
        {
            prevTime = currentTime;
            animator.SetTrigger("NumberPopUp");
        }
    }
}
