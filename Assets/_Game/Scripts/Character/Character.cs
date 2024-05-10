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
    [SerializeField] private PlayerBrick brickPrefab;
    [SerializeField] private Brick brick;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask stairLayer;
    [SerializeField] protected Transform visual;

    public int brickCount;
    public Stage stage;
    public bool isFalling;
    public bool isStaring;

    private List<PlayerBrick> brickList;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        brickList = new List<PlayerBrick>();
        brickCount = 0;
        isFalling = false;
        isStaring = false;
        visual.rotation = Quaternion.identity;
        ClearBrick();
    }

    public bool CanMove(Vector3 nextPos)
    {
        bool isCanMove = true;
        isStaring = false;

        RaycastHit hit;
        if (Physics.Raycast(nextPos, Vector3.down, out hit, 3f, stairLayer))
        {
            isStaring = true;
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(BRICKTAG))
        {
            if (!isFalling)
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
                    else if (brickOb.colorType == ColorType.None)
                    {
                        //Destroy(other.gameObject);
                        other.gameObject.SetActive(false);
                        AddBirck();
                    }
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

    public virtual void AddBirck()
    {
        //Debug.Log("AddBrick");
        brickCount++;
        if (brickList.Count == 0)
        {
            //PlayerBrick brickGo = Instantiate(brickPrefab, brickContainer);
            PlayerBrick brickGo = ObjectPooling.Ins.GetGameObject(brickPrefab.gameObject).GetComponent<PlayerBrick>();
            brickGo.gameObject.SetActive(true);
            brickGo.transform.SetParent(brickContainer);
            brickGo.transform.localPosition = new Vector3(0, 0, 0);
            brickGo.transform.rotation = brickContainer.rotation;
            brickGo.ChangeColor(colorType);
            brickList.Add(brickGo);
        }
        else
        {
            Vector3 brickPos = brickList[brickList.Count - 1].transform.localPosition + new Vector3(0,0.5f, 0);
            //PlayerBrick brickGo = Instantiate(brickPrefab, brickContainer);
            PlayerBrick brickGo = ObjectPooling.Ins.GetGameObject(brickPrefab.gameObject).GetComponent<PlayerBrick>();
            brickGo.gameObject.SetActive(true);
            brickGo.transform.SetParent(brickContainer);
            brickGo.transform.localPosition = brickPos;
            brickGo.transform.rotation = brickContainer.rotation;
            brickGo.ChangeColor(colorType);
            brickList.Add(brickGo);
        }
    }

    public virtual void RemoveBrick()
    {
        //Debug.Log("RemoveBrick");
        brickCount--;
        PlayerBrick lastBrick = brickList[brickCount];
        //Destroy(lastBrick.gameObject);
        lastBrick.transform.SetParent(null);
        lastBrick.gameObject.SetActive(false);
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
            PlayerBrick brick = brickList[i];
            brick.transform.SetParent(null);
            brick.gameObject.SetActive(false);
            //Destroy(brick.gameObject);
            brickList.RemoveAt(i);
        }
        brickCount = 0;
    }

    public void FallBrick()
    {
        AudioManager.Ins.PlayFallSound();

        for (int i=brickList.Count-1; i >= 0; i--)
        {
            //Brick brickOb = Instantiate(brick, brickList[i].transform.position, Quaternion.identity, stage.transform);
            Brick brickOb = ObjectPooling.Ins.GetGameObject(brick.gameObject).GetComponent<Brick>();
            brickOb.gameObject.SetActive(true);
            brickOb.transform.SetParent(null);
            brickOb.transform.position = brickList[i].transform.position;
            RemoveBrick();
            brickOb.ChangeColor(ColorType.None);
            Rigidbody brickRb = brickOb.GetComponent<Rigidbody>();
            brickRb.isKinematic = false;
            brickRb.AddExplosionForce(1000f, transform.position, 5f);
        }
        
    }
}
