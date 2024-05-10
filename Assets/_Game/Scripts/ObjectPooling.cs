using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private Brick brick;
    //[SerializeField] private PlayerBrick playerBrick;

    Dictionary<GameObject, List<GameObject>> objectList = new Dictionary<GameObject, List<GameObject>>();

    private void Start()
    {
        LevelManager.Ins.OnLoadLevel += LevelManager_OnLoadLevel;
    }

    private void OnDestroy()
    {
        //if (LevelManager.Ins != null)
            //LevelManager.Ins.OnLoadLevel -= LevelManager_OnLoadLevel;
    }

    private void LevelManager_OnLoadLevel(object sender, System.EventArgs e)
    {
        if (objectList.ContainsKey(brick.gameObject))
        {
            for (int i = 0; i < objectList[brick.gameObject].Count; i++)
            {
                objectList[brick.gameObject][i].SetActive(false);
            }
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
