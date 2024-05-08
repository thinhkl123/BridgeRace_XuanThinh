using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private Brick brick;
    [SerializeField] private PlayerBrick playerBrick;

    Dictionary<GameObject, List<GameObject>> objectList = new Dictionary<GameObject, List<GameObject>>();

    private void Start()
    {
        LevelManager.Ins.OnLoadLevel += LevelManager_OnLoadLevel;
    }

    private void LevelManager_OnLoadLevel(object sender, System.EventArgs e)
    {
        if (objectList.ContainsKey(brick.gameObject))
        {
            objectList.Remove(brick.gameObject);
        }
        if (objectList.ContainsKey(playerBrick.gameObject))
        {
            objectList.Remove(playerBrick.gameObject);
        }
    }

    public GameObject GetGameObject(GameObject defaultGameObject)
    {
        if (objectList.ContainsKey(defaultGameObject))
        {
            foreach (GameObject o in objectList[defaultGameObject])
            {
                if (o.activeSelf)
                {
                    continue;
                }
                return o;
            }

            GameObject g = Instantiate(defaultGameObject, this.transform.position, Quaternion.identity);
            objectList[defaultGameObject].Add(g);
            g.SetActive(false);

            return g;
        }

        List<GameObject> instantObjectList = new List<GameObject>();

        GameObject g2 = Instantiate(defaultGameObject, this.transform.position, Quaternion.identity);
        instantObjectList.Add(g2);
        g2.SetActive(false);
        objectList.Add(defaultGameObject, instantObjectList);

        return g2;
    }

    public void RemoveKey(GameObject key)
    {
        if (objectList.ContainsKey(key))
        {
            objectList.Remove(key);
        }
    }
}
