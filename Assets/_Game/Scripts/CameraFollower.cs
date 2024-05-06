using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smooth;
    Vector3 distance;

    private Transform initTarget;

    private void Awake()
    {
        initTarget = target;
    }

    // Start is called before the first frame update
    void Start()
    {
        distance = new Vector3(0, 0, -12f) - transform.position;
        FinishPoint.OnLose += FinishPoint_OnLose;
        LevelManager.instance.OnLoadLevel += LevelManager_OnLoadLevel;
    }

    private void LevelManager_OnLoadLevel(object sender, System.EventArgs e)
    {
        Init();
    }

    private void Init()
    {
        target = initTarget;
    }

    private void OnDestroy()
    {
        FinishPoint.OnLose -= FinishPoint_OnLose;
        LevelManager.instance.OnLoadLevel -= LevelManager_OnLoadLevel;
    }

    private void FinishPoint_OnLose(object sender, FinishPoint.OnLoseEventArgs e)
    {
        target = e.bot.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.y >= 0)
        {
            Follow();
        }
    }

    void Follow()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosiotion = target.position - distance;

        transform.position = Vector3.Lerp(currentPosition, newPosiotion, smooth * Time.deltaTime);
    }
}
