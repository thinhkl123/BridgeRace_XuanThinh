using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<Transform> brickTranformList;
    [SerializeField] private Brick brick;

    public List<Transform> topStairPosList;
    public List<Vector3> spawnPosList;
    public List<Brick> brickList;

    private int spawnPosCount;

    private void Awake()
    {
        spawnPosList = new List<Vector3>();
        brickList = new List<Brick>();
        Init();
    }

    public void Init()
    {
        for (int i=0; i<brickTranformList.Count; i++)
        {
            spawnPosList.Add(brickTranformList[i].position);
        }

        spawnPosCount = spawnPosList.Count;
    }

    public void SpawnColor(ColorType colorType)
    {
        int amount = spawnPosCount / LevelManager.instance.charAmount;

        for (int i = 0; i < amount; i++)
        {
            SpawnBrick(colorType);
        }
    }

    public void SpawnBrick(ColorType colorType)
    {
        int idx = Random.Range(0, spawnPosList.Count);

        //Debug.Log(spawnPosList[idx] + " " + idx + " " + spawnPosList.Count);

        Brick brickGO = Instantiate(brick, spawnPosList[idx], Quaternion.identity, this.transform);
        brickGO.ChangeColor(colorType);
        spawnPosList.RemoveAt(idx);
        brickList.Add(brickGO);
    }

    public Brick FindBrick(ColorType colorType)
    {
        for (int i = 0; i < brickList.Count; i++)
        {
            if (brickList[i].colorType == colorType)
            {
                return brickList[i];
            }
        }
        return null;
    }

    public void RemoveBrick(Brick brick)
    {
        ColorType colorType = brick.colorType;
        //Vector3 place = brick.transform.position;
        spawnPosList.Add(brick.transform.position);
        //Debug.Log(brick.transform);
        brickList.Remove(brick);
        Destroy(brick.gameObject);
        StartCoroutine(SpawnBrickAgain(colorType));
    }

    IEnumerator SpawnBrickAgain(ColorType colorType)
    {
        yield return new WaitForSeconds(10);
        SpawnBrick(colorType);
    }
}
