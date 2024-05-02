using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : GameUnit
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private Renderer meshRenderer;

    public ColorType colorType;

    public void ChangeColor(ColorType newColorType)
    {
        colorType = newColorType;
        meshRenderer.material = colorData.GetMaterial(colorType);
    }
}
