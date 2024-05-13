using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UICanvas
{
    public static event EventHandler<OnToogleChangedEventArgs> OnToggleChanged;
    public class OnToogleChangedEventArgs: EventArgs
    {
        public Transform pos;
    }

    [SerializeField] private Button closeBtn;
    [SerializeField] private Slider soundFXVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Toggle leftToggle;
    [SerializeField] private Toggle rightToggle;

    //ChangPosJoyStick
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;
    [SerializeField] private GameObject JoyStick;

    private void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close(0);
        });

        //Slider
        soundFXVolume.value = 1;
        musicVolume.value = 1;

        soundFXVolume.onValueChanged.AddListener(delegate
        {
            AudioManager.Ins.ChangeSoundFXVolume(soundFXVolume.value);
        });

        musicVolume.onValueChanged.AddListener(delegate
        {
            AudioManager.Ins.ChangeMusicVolume(musicVolume.value);
        });

        //Toggle
        leftToggle.isOn = true;
        rightToggle.isOn = false;

        leftToggle.onValueChanged.AddListener(delegate
        {
            if (leftToggle.isOn == rightToggle.isOn)
            {
                rightToggle.isOn = !leftToggle.isOn;
            }

            if (leftToggle.isOn)
            {
                //JoyStick.transform.position = leftPos.position;
                OnToggleChanged?.Invoke(this, new OnToogleChangedEventArgs
                {
                    pos = leftPos
                });
            }
        });

        rightToggle.onValueChanged.AddListener(delegate
        {
            if (leftToggle.isOn == rightToggle.isOn)
            {
                leftToggle.isOn = !rightToggle.isOn;
            }

            if (rightToggle.isOn)
            {
                //JoyStick.transform.position = rightPos.position;
                OnToggleChanged?.Invoke(this, new OnToogleChangedEventArgs
                {
                    pos = rightPos
                });
            }
        });
    }
}
