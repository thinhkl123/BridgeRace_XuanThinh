using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    //public static ColorManager instance {  get; private set; }

    [SerializeField] private int maxColorAmout;

    public List<ColorType> colorTypes;

    public List<ColorType> colorTypeList;

    private void Awake()
    {
        //instance = this;
    }

    private void Init()
    {
        colorTypeList = new List<ColorType>(); 

        for (int i = 1; i <= maxColorAmout; i++)
        {
            colorTypeList.Add((ColorType) i);
        }
    }

    public void GetColor()
    {
        Init();

        colorTypes = new List<ColorType>();

        for (int i = 0; i < LevelManager.Ins.charAmount; i++)
        {
            int ranIdx = Random.Range(0, colorTypeList.Count);

            colorTypes.Add(colorTypeList[ranIdx]);
            colorTypeList.RemoveAt(ranIdx);
        }
    }

    public ColorType GetColorToObject()
    {
        int ranIdx = Random.Range(0, colorTypes.Count);
        ColorType colorType = colorTypes[ranIdx];
        colorTypes.RemoveAt(ranIdx);
        return colorType;
    }
}
