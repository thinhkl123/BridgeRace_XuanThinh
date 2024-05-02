using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    public enum ColorType
    {
        None = 0,
        Blue = 1,
        Green = 2,
        Pink = 3,
        Red = 4,
        Purple = 5,
        Default = 6,
    }

    [CreateAssetMenu(menuName = "ColorData")]
    public class ColorData: ScriptableObject
    {
        [SerializeField] Material[] materials;

        public Material GetMaterial(ColorType colorType)
        {
            return materials[(int) colorType];
        }
    }
}
