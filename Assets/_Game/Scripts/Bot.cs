using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private int brickContainerAmount;

    public enum State
    {
        CollectBrick,
        MoveStair,
    }

    private State state;

    public Vector3 destination;

    private void Awake()
    {
        Init();
        destination = transform.position;
    }

    private void Start()
    {
        ChangeToState(State.CollectBrick);
    }

    private void Update()
    {
        if (GameMainManager.instance.state != GameMainManager.GameState.Playing)
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
        }

        //Enter New State
        state = newState;

        switch (state)
        {
            case State.CollectBrick:
                brickContainerAmount = Random.Range(7,10);
                break;
            case State.MoveStair:
                break;
        }
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
}
