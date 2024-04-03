using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GamePlayer player;

    public void Init()
    {
        player = new GamePlayer();
        //player.playerId = Managers.AccountData.playerData.uId;
        player.playerId = "test";
        player.prioty = 10;
    }
}
