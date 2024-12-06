using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Bot : Character
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject winPos;
    [SerializeField] int randomAmountEachCollect;
    CharBaseState<Bot> currentStateMachine;

    GBrickManager gBrickManager;
    [SerializeField] List<GroundBrick> brickOnGround = new List<GroundBrick>();
    bool isCollecting;
    Vector3 target;
    Vector3 previousTarget;
    int brickAmountTarget;
    private void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        ChangeStateMachine(new IdleState());

        //set mau nhan vat va vong tron
        colorCircle.material = DataManager.instance.colorData.GetMaterial(characterColor);

        //set trang thai nhan vat
        isRunning = false;
        currenState = CharState.OnGround;
        currentFloor = 1;

        //set animation
        animator.SetBool("Torch Idle", true);

        isCollecting = false;

    }

    private void Update()
    {
        currentStateMachine.OnExecute(this);
        
        
        RunAnim();
        CheckState();
    }


    public void CollectBrick()
    {
        if (!isCollecting)
        {
            isRunning = true;
            int cnt = 0;
            while (target == previousTarget && brickOnGround.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, brickOnGround.Count);
                if (brickOnGround[index].ColorCompare(characterColor))
                {
                    target = brickOnGround[index].transform.position;
                }

                cnt++;
                if (cnt == 100) break;
            }

  
            agent.destination = target;
            isCollecting = true;
        }
    }


    public void goToWinPos()
    {
        agent.destination = winPos.transform.position;
    }

    public void SetIsCollecting(Boolean b)
    {
        isCollecting = b;
    }

    void CheckCollect()
    {
        isCollecting = false;
        previousTarget = target;
        brickAmountTarget--;
    }

    public bool IsFinishCollecting()
    {
        return brickAmountTarget <= 0;
    }

    public bool IsOutOfBrickList()
    {
        return BrickList.Count <= 0;
    }

    public void AddBrickOnGround(GroundBrick brick)
    {
        brickOnGround.Add(brick);
    }



    public void ChangeStateMachine(CharBaseState<Bot> state)
    {
        if (currentStateMachine != null)
        {
            currentStateMachine.OnExit(this);
        }
        currentStateMachine = state;

        if (currentStateMachine != null)
        {
            currentStateMachine.OnEnter(this);
        }
    }

    public void RandomBrickAmountTarget()
    {
        brickAmountTarget = UnityEngine.Random.Range(0, randomAmountEachCollect);
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.tag == "Door")
        {
            gBrickManager = other.gameObject.GetComponent<GBrickManager>();
            brickOnGround = gBrickManager.GetBrickOnGround();
        }
        if(other.tag == "GroundBrick")
        {
            GroundBrick brick = other.gameObject.GetComponent<GroundBrick>();
            if (other.gameObject.activeSelf && brick.ColorCompare(characterColor))
            {
                brickOnGround.Remove(brick);
                CheckCollect();
            }
        }
    }
}
