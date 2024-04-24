using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public GamePlayer player;
    public GamePlayer enemy;

    public string stageName;
    public StageSO thisStage; 

    public void Init()
    {
        player = new GamePlayer();
        enemy = new GamePlayer();

        //player.playerId = Managers.AccountData.playerData.uId;
        player.playerId = "test";
        player.prioty = 10;

        InitParty();
    }

    public void SetEnemy(GamePlayer enemy)
    {
        this.enemy = enemy;
    }

    public void InitParty()
    {
        if(Managers.AccountData.formationData == null || Managers.AccountData.formationData.Count == 0)
        {
            return;
        }

        FormationData formation = Managers.AccountData.formationData[0];

        for(int i = 0; i < formation.characterId.Length; i++)
        {
            if(Managers.AccountData.characterData[formation.characterId[i]] == null)
            {
                continue;
            }

            player.party[i] = Managers.AccountData.characterData[formation.characterId[i]];
        }

    }

    public void UpdateParty(FormationData formation)
    {
        for (int i = 0; i < formation.characterId.Length; i++)
        {
            if (Managers.AccountData.characterData[formation.characterId[i]] == null)
            {
                continue;
            }

            UpdatePartyCharacter(formation, i);
        }
    }

    public void UpdatePartyCharacter(FormationData formationData, int index)
    {
        //파티에 이미 캐릭터가 있는지 확인
        int exist = Array.IndexOf(Managers.GameManager.player.party, Managers.AccountData.characterData[formationData.characterId[index]]);

        if (exist > -1)//있다면 해당 자리를 null로
        {
            player.party[exist] = null;
        }
        player.party[index] = Managers.AccountData.characterData[formationData.characterId[index]];
    }
}
