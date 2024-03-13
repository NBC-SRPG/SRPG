using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    private enum NowPlayer
    {
        Ally,
        Enemy,
        Aid
    }

    public List<GamePlayer> Players = new List<GamePlayer>();
    public List<CharacterBase> charactersInBattle = new List<CharacterBase>();

    private NowPlayer nowPlayer;
    private int nowPlayerNum;

    public void Init()
    {
        Players.Clear();
        charactersInBattle.Clear();
    }

    public void InitBattle()
    {
        nowPlayerNum = 0;
    }

    public void OnPassCharacter(CharacterBase curCharacter, CharacterBase standingCharacter)
    {
        if (curCharacter.playerId == standingCharacter.playerId)
        {
            curCharacter.OnPassAlly(standingCharacter);
            standingCharacter.OnAllyPassedMe(curCharacter);
        }
        else
        {
            Debug.Log(curCharacter.playerId);
            Debug.Log(standingCharacter.playerId);
            if (curCharacter.character.CharacterAttackType == Constants.AttackType.Melee)
            {
                Attack(curCharacter, standingCharacter);
            }
        }
    }

    public void Attack(CharacterBase attacker, CharacterBase victim)
    {
        Debug.Log("attack");
        attacker.isAttacking = true;
        attacker.OnStartAttack(victim);

        //---
        //victim.takedamage
        //attacker.OnAttackSuccess(victim, damage);
        victim.OnTakeDamage(attacker);
        //---

        attacker.OnEndAttack(victim);
        
    }

    public void PlayerTurnEnd()
    {
        nowPlayerNum++;
        if(nowPlayerNum >= Players.Count)
        {
            EndTurn();
        }
    }

    private void StartTurn()
    {
        foreach (CharacterBase characters in charactersInBattle)
        {
            characters.OnStartTurn();
        }
    }

    private void EndTurn()
    {
        foreach(CharacterBase characters in charactersInBattle)
        {
            characters.OnEndTurn();
        }

        nowPlayerNum = 0;

        StartTurn();
    }
}
