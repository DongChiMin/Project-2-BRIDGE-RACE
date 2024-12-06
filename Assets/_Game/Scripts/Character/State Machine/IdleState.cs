using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CharBaseState<Bot>
{
    public void OnEnter(Bot bot)
    {
        Debug.Log("Start Idle State");
    }

    public void OnExecute(Bot bot)
    {
        if (Input.touchCount > 0 )
        {

            bot.ChangeStateMachine(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {

    }
}