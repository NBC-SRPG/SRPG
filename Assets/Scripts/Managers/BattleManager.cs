using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager
{

    public List<GamePlayer> players = new List<GamePlayer>();
    public List<CharacterBase> charactersInBattle = new List<CharacterBase>();
    public Dictionary<string, List<CharacterBase>> charactersAsTeam = new Dictionary<string, List<CharacterBase>>(); 

    public GamePlayer nowPlayer;
    private int nowPlayerNum;

    public event Action TurnStart;

    //-----------------------------------------------------------------------------------------------------------------------
    //초기화 함수들

    public void Init()
    {
        players.Clear();
        charactersInBattle.Clear();
        charactersAsTeam.Clear();
    }

    public void GetReady()
    {
        if (players.FindAll(x => x.isReady).Count == players.Count)
        {
            InitBattle();
        }
    }

    public void InitBattle()
    {
        nowPlayerNum = 0;

        StartRound();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //전투 관련 함수들

    public void OnPassCharacter(CharacterBase curCharacter, CharacterBase standingCharacter)
    {
        if (curCharacter.playerId == standingCharacter.playerId)
        {
            curCharacter.OnPassAlly(standingCharacter);
            standingCharacter.OnAllyPassedMe(curCharacter);
        }
        else
        {
            if (curCharacter.character.CharacterAttackType == Constants.AttackType.Melee)
            {
                Attack(curCharacter, standingCharacter);
            }
            standingCharacter.OnEnemyPassesMe(curCharacter);
        }
    }

    public void Attack(CharacterBase attacker, CharacterBase victim)
    {
        Debug.Log("attack");
        attacker.OnStartAttack(victim);

        //---
        //victim.takedamage
        //attacker.OnAttackSuccess(victim, damage);
        victim.OnTakeDamage(attacker);
        //---

        attacker.OnEndAttack(victim);
        
    }

    public void UseSkill(CharacterBase skillUser, List<CharacterBase> target)
    {

    }

    //-----------------------------------------------------------------------------------------------------------------------
    //턴 관련 함수들

    public void PlayerTurnEnd()
    {
        nowPlayerNum++;

        if(nowPlayerNum >= players.Count)
        {
            EndRound();
            return;
        }

        PlayerTurnStart();
    }

    public void PlayerTurnStart()
    {
        nowPlayer = players[nowPlayerNum];

        TurnStart?.Invoke();
        Debug.Log("nowPlayer" + nowPlayer.playerId);
    }

    private void StartRound()
    {
        players = players.OrderByDescending(x => x.prioty).ToList();

        foreach (CharacterBase characters in charactersInBattle)
        {
            characters.OnStartTurn();
        }

        PlayerTurnStart();
    }

    private void EndRound()
    {
        foreach(CharacterBase characters in charactersInBattle)
        {
            characters.OnEndTurn();
        }

        nowPlayerNum = 0;

        StartRound();
    }
}
