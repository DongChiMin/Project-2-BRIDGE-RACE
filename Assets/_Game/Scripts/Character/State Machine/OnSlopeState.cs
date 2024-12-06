using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSlopeState : CharBaseState<Bot>
{
    public void OnEnter(Bot bot)
    {
        Debug.Log("Start OnSlope State");
        bot.goToWinPos();
    }

    public void OnExecute(Bot bot)
    {
        if (bot.IsOutOfBrickList())
        {
            bot.ChangeStateMachine(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
