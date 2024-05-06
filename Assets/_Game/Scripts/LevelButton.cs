using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public static event EventHandler OnLevelChange;

    [SerializeField] private Sprite active;
    [SerializeField] private Sprite nonActive;
    [SerializeField] private Image image;
    [SerializeField] private GameObject select;
    [SerializeField] private TextMeshProUGUI level;

    public void SetActive()
    {
        image.sprite = active;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //Debug.Log("Click");
            if (GetLevelIdx() == LevelManager.Ins.curLevel)
            {
                return;
            }
            SetSelect();
            OnLevelChange?.Invoke(this, EventArgs.Empty);
            LevelManager.Ins.curLevel = GetLevelIdx();
        });
    }

    public void SetNonActive()
    {
        image.sprite = nonActive;
    }

    public void SetSelect()
    {
        select.SetActive(true);
    }

    public void SetNonSelect()
    {
        select.SetActive(false);
    }

    public int GetLevelIdx()
    {
        return int.Parse(level.text);
    }
}
