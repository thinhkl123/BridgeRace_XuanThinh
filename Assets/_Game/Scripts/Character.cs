using Scriptable;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : ColorObject
{
    const string BRICKTAG = "Brick";
    const string BLOCKSTEP = "BlockStep";
    const string STAGE = "Stage";
    const string FINISHPOINT = "FinishPoint";

    [SerializeField] private Transform brickContainer;
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask stairLayer;
    [SerializeField] protected Transform visual;

    public int brickCount;
    public Stage stage;

    private List<Brick> brickList;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        brickList = new List<Brick>();
        brickCount = 0;
    }

    public bool CanMove(Vector3 nextPos)
    {
        bool isCanMove = true;

        RaycastHit hit;
        if (Physics.Raycast(nextPos, Vector3.down, out hit, 3.5f, stairLayer))
        {
            Stair stair = hit.collider.gameObject.GetComponent<Stair>();

            if (stair.colorType != colorType && brickCount > 0)
            {
                stair.ChangeColor(colorType);
                RemoveBrick();
            }
            else if (stair.colorType != colorType && brickCount <= 0 && visual.forward.z > 0)
            {
                //Debug.LogWarning("Can not Move");
                isCanMove = false;
            }
        }

        return isCanMove;
    }

    public Vector3 CheckNextPosition(Vector3 nextPos)
    {
        RaycastHit hit;

        if (Physics.Raycast(nextPos, Vector3.down, out hit, 5f, groundLayer))
        {
            if (stage != null)
            {
                //Debug.Log(hit.point.y + " " + stage.transform.position.y);
                if (hit.point.y < stage.transform.position.y)
                {
                    return TF.position;
                }
                else
                {
                    return hit.point;
                }
            }
            else
            {
                return hit.point;
            }
        }
        else
        {
            //Debug.Log("Current Pos" + TF.position);
            return TF.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(BRICKTAG))
        {
            Brick brickOb;
            if (other.gameObject.TryGetComponent<Brick>(out brickOb))
            {
                if (brickOb.colorType == colorType)
                {
                    //Brick brick = other.gameObject.GetComponent<Brick>();
                    stage.RemoveBrick(brickOb);
                    AddBirck();
                }
            } 
        } 
        else if (other.CompareTag(STAGE))
        {
            Stage newStage = other.gameObject.GetComponentInParent<Stage>();
            if (newStage != null)
            {
                SetStage(newStage);
            }
        }
        /*
        else if (other.CompareTag(FINISHPOINT))
        {
            Debug.Log("Win");
        }
        */
    }

    private void AddBirck()
    {
        //Debug.Log("AddBrick");
        brickCount++;
        if (brickList.Count == 0)
        {
            Brick brickGo = Instantiate(brickPrefab, brickContainer);
            brickGo.transform.localPosition = new Vector3(0, 0, 0);
            brickGo.GetComponent<Brick>().ChangeColor(colorType);
            brickList.Add(brickGo);
        }
        else
        {
            Vector3 brickPos = brickList[brickList.Count - 1].transform.localPosition + new Vector3(0,0.5f, 0);
            Brick brickGo = Instantiate(brickPrefab, brickContainer);
            brickGo.transform.localPosition = brickPos;
            brickGo.GetComponent<Brick>().ChangeColor(colorType);
            brickList.Add(brickGo);
        }
    }

    private void RemoveBrick()
    {
        //Debug.Log("RemoveBrick");
        brickCount--;
        Brick lastBrick = brickList[brickCount];
        Destroy(lastBrick.gameObject);
        brickList.RemoveAt(brickCount);
    }

    public virtual void SetStage(Stage newStage)
    {
        //Debug.Log("Set Stage");
        if (stage == null)
        {
            stage = newStage;
            stage.SpawnColor(colorType);
        }
        else if (stage.transform.position != newStage.transform.position)
        {
            stage.RemoveAllBrick(colorType);
            stage = newStage;
            stage.SpawnColor(colorType);
        }
    }

    public void ClearBrick()
    {
        for (int i = brickList.Count - 1; i >= 0; i--)
        {
            Brick brick = brickList[i];
            Destroy(brick.gameObject);
            brickList.RemoveAt(i);
        }
        brickCount = 0;
    }
}
