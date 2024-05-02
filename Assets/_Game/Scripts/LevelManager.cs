using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance {  get; private set; }

    [SerializeField] private List<LevelSO> levelList;
    [SerializeField] private Character playerPrefab;
    [SerializeField] private Character botPrefab;

    public int charAmount;

    private Level levelGameObject;
    private List<Character> characterList;
    private List<Transform> spawnCharPosList;

    private void Awake()
    {
        instance = this;
        characterList = new List<Character>();  
    }

    private void Start()
    {
        LoadLevel(1);
    }

    private void LoadLevel(int levelIdx)
    {
        if (levelGameObject != null)
        {
            Destroy(levelGameObject);
        }

        levelGameObject = Instantiate(levelList[levelIdx-1].prefab);
        charAmount = levelList[levelIdx-1].charAmount;

        GetSpawnCharPosList();

        ColorManager.instance.GetColor();

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

        int ranIdx;

        ranIdx = Random.Range(0, spawnCharPosList.Count);
        playerPrefab.transform.position = spawnCharPosList[ranIdx].position;
        spawnCharPosList.RemoveAt(ranIdx);
        playerPrefab.ChangeColor(ColorManager.instance.GetColorToObject());
        playerPrefab.gameObject.SetActive(true);
        characterList.Add(playerPrefab);

        for (int i = 1; i < charAmount; i++)
        {
            ranIdx = Random.Range(0, spawnCharPosList.Count);
            Character bot = Instantiate(botPrefab, spawnCharPosList[ranIdx].position, botPrefab.transform.rotation);
            spawnCharPosList.RemoveAt(ranIdx);
            bot.ChangeColor(ColorManager.instance.GetColorToObject());
            characterList.Add(bot);
        }
    }
}
