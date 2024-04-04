using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GamePlayer player;
    public GamePlayer enemy;


    public void Init()
    {
        player = new GamePlayer();
        enemy = new GamePlayer();

        //player.playerId = Managers.AccountData.playerData.uId;
        player.playerId = "test";
        player.prioty = 10;

    }

    public void SetEnemy(GamePlayer enemy)
    {
        this.enemy = enemy;
    }
}
