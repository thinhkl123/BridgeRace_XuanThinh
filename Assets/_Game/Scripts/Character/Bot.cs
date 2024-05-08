using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    public NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private int brickContainerAmount;

    public enum State
    {
        CollectBrick,
        MoveStair,
        Fall,
    }

    private State state;

    public Vector3 destination;

    private void Awake()
    {
        Init();
        destination = transform.position;
        agent.agentTypeID = LevelManager.Ins.agentId;
    }

    private void Start()
    {
        FinishPoint.OnLose += FinishPoint_OnLose;
        LevelManager.Ins.OnLoadLevel += LevelManager_OnLoadLevel;

        ChangeToState(State.CollectBrick);
    }

    private void OnDestroy()
    {
        FinishPoint.OnWin -= FinishPoint_OnLose;
        LevelManager.Ins.OnLoadLevel -= LevelManager_OnLoadLevel;
    }

    private void FinishPoint_OnLose(object sender, System.EventArgs e)
    {
        ClearBrick();
        animator.SetBool("isWin", true);
    }

    private void LevelManager_OnLoadLevel(object sender, System.EventArgs e)
    {
        Init();
        destination = transform.position;
        agent.agentTypeID = LevelManager.Ins.agentId;
        animator.SetFloat("Speed", 0);
        animator.SetBool("isWin", false);
    }

    private void Update()
    {
        if (GameMainManager.Ins.state != GameMainManager.GameState.Playing)
        {
            return;
        }

        //Debug.Log(state);
        switch (state)
        {
            case State.CollectBrick:
                if (Vector3.Distance(transform.position, destination) < 0.1f)
                {
                    animator.SetFloat("Speed", 0);
                    if (brickCount >= brickContainerAmount)
                    {
                        ChangeToState(State.MoveStair);
                    }
                    else
                    {
                        UpdateBrickDestination();
                    }
                }
                break;
            case State.MoveStair:
                if (!CanMove(TF.position))
                {
                    SetDestination(TF.position);
                    ChangeToState(State.CollectBrick);
                }
                else if (Vector3.Distance(transform.position, destination) < 0.1f)
                {
                    MoveStair();
                    
                }
                break;
            case State.Fall:
                break;
        }
    }

    private void ChangeToState(State newState)
    {
        //Exit Current State
        switch (state)
        {
            case State.CollectBrick:
                break;
            case State.MoveStair:
                break;
            case State.Fall:
                break;
        }

        //Enter New State
        state = newState;

        switch (state)
        {
            case State.CollectBrick:
                brickContainerAmount = Random.Range(7,15);
                break;
            case State.MoveStair:
                break;
            case State.Fall:
                animator.SetTrigger("Fall");
                SetDestination(TF.position);
                FallBrick();
                break;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Player") && !isStaring)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player.brickCount > brickCount)
            {
                ChangeToState(State.Fall);
                isFalling = true;
            }
        }
    }

    public void EndFallAnimation()
    {
        ChangeToState(State.CollectBrick);
        isFalling = false;
    }

    private void UpdateBrickDestination()
    {
        Vector3 newDestination;
        if (stage != null)
        {
            newDestination = stage.FindBrick(colorType).transform.position;
            if (newDestination != null)
            {
                SetDestination(newDestination);
            }
        }
    }

    private void MoveStair()
    {
        int idx = Random.Range(0, stage.topStairPosList.Count);
        SetDestination(stage.topStairPosList[idx].position);
    }

    private void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
        agent.SetDestination(destination);
        animator.SetFloat("Speed", 1f);
    }

    public override void SetStage(Stage newStage)
    {
        base.SetStage(newStage);
        destination = TF.position;
        ChangeToState(State.CollectBrick);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 3.5f, Color.black);
    }
}
