using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.AI;

public class PatrolState : CharBaseState<Bot>
{


    public void OnEnter(Bot bot)
    {
        Debug.Log("Start Patrol State");
        bot.RandomBrickAmountTarget();
        bot.SetIsCollecting(false);
    }

    public void OnExecute(Bot bot)
    {
        bot.CollectBrick();
        if (bot.IsFinishCollecting())
        {
            bot.ChangeStateMachine(new OnSlopeState());
        }
    }

    public void OnExit(Bot bot)
    {

    }

}
