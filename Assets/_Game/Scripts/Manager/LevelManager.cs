using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    //public static LevelManager instance {  get; private set; }

    public event EventHandler OnLoadLevel;

    [SerializeField] private List<LevelSO> levelList;
    [SerializeField] private Character playerPrefab;
    [SerializeField] private Character botPrefab;
    [SerializeField] private int maxLevel = 4;

    public int MaxLevel
    {
        get { return maxLevel; } 
    }
    public int charAmount;
    public int curMaxLevel;
    public int curLevel;

    public int agentId;

    public Level levelGameObject;
    private List<Character> characterList;
    private List<Transform> spawnCharPosList;

    private void Awake()
    {
        //instance = this;
        characterList = new List<Character>();
        curMaxLevel = PlayerPrefs.GetInt("MaxLevel", 1);
        curLevel = curMaxLevel;
    }

    private void Start()
    {
        //LoadLevel(1);
        FinishPoint.OnWin += FinishPoint_OnWin;
    }

    private void OnDestroy()
    {
        FinishPoint.OnWin -= FinishPoint_OnWin;
    }

    private void FinishPoint_OnWin(object sender, EventArgs e)
    {
        UpdateLevelMax();
    }

    public void LoadLevel(int levelIdx)
    {
        OnLoadLevel?.Invoke(this, EventArgs.Empty);

        UIManager.Ins.OpenUI<PlayUI>();
        UIManager.Ins.GetUI<PlayUI>().UpdateLevelText();

        if (levelGameObject != null)
        {
            Destroy(levelGameObject.gameObject);
        }

        levelGameObject = Instantiate(levelList[levelIdx-1].prefab);
        charAmount = levelList[levelIdx-1].charAmount;

        GetSpawnCharPosList();

        ColorManager.Ins.GetColor();

        agentId = levelList[levelIdx - 1].agentTypeId;

        SpawnChar();


    }

    private void GetSpawnCharPosList()
    {
        spawnCharPosList = new List<Transform>();

        for (int i = 0; i < levelGameObject.spawnCharPosList.Count; i++)
        {
            spawnCharPosList.Add(levelGameObject.spawnCharPosList[i]);
        }
    }

    private void SpawnChar()
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            Destroy(characterList[i].gameObject);
            characterList.RemoveAt(i);
        }

        //Spawn Player
        int ranIdx;

        ranIdx = Random.Range(0, spawnCharPosList.Count);
        playerPrefab.transform.position = spawnCharPosList[ranIdx].position;
        spawnCharPosList.RemoveAt(ranIdx);
        playerPrefab.ChangeColor(ColorManager.Ins.GetColorToObject());
        playerPrefab.gameObject.SetActive(true);

        //Spawn Bot

        for (int i = 1; i < charAmount; i++)
        {
            ranIdx = Random.Range(0, spawnCharPosList.Count);
            Character bot = Instantiate(botPrefab, spawnCharPosList[ranIdx].position, botPrefab.transform.rotation);
            spawnCharPosList.RemoveAt(ranIdx);
            bot.ChangeColor(ColorManager.Ins.GetColorToObject());
            characterList.Add(bot);
        }
    }

    private void UpdateLevelMax()
    {
        if (curMaxLevel < curLevel+1)
        {
            curMaxLevel = curLevel + 1;
            PlayerPrefs.SetInt("MaxLevel", curMaxLevel);
        }
    }
}