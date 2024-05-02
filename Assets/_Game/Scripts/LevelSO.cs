using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelSO : ScriptableObject
{
    public int levelIndex;
    public Level prefab;
    public int charAmount;
}
