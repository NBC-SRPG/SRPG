using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaitNextTurn : TutorialBase
{
    private GamePlayer player;
    private bool playerTurn; 

    private void Start()
    {
        player = Managers.GameManager.player;
        playerTurn = false;
    }

    public override void Enter()
    {
        BattleManager.Instance.TurnStart += GetPlayerTurn;
    }

    private void GetPlayerTurn()
    {
        if (BattleManager.Instance.nowPlayer == player)
        {
            StartCoroutine(TurnStart());
        }

    }

    private IEnumerator TurnStart()
    {
        yield return new WaitForSeconds(0.5f);

        playerTurn = true;
    }

    public override void Execute(TutorialController controller)
    {
        if(playerTurn == true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        BattleManager.Instance.TurnStart -= GetPlayerTurn;
    }
}
