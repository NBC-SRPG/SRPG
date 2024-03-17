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

    public bool isShowAnimation;
    private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    //-----------------------------------------------------------------------------------------------------------------------
    //초기화 함수들

    public void Init()
    {
        players.Clear();
        charactersInBattle.Clear();
        charactersAsTeam.Clear();

        isShowAnimation = false;
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
    //-----------------------------------------------------------------------------------------------------------------------
    //전투 관련 함수들

    //---------------------------------------------------------------------------
    // 이동 관련
    public IEnumerator OnPassCharacter(CharacterBase curCharacter, CharacterBase standingCharacter)
    {
        if (curCharacter.playerId == standingCharacter.playerId)// 아군 위를 지나갔을 때
        {
            curCharacter.OnPassAlly(standingCharacter);
            standingCharacter.OnAllyPassedMe(curCharacter);
        }
        else// 적군 위를 지나갔을 때
        {
            if (curCharacter.character.CharacterAttackType == Constants.AttackType.Melee)// 근거리 캐릭터라면
            {
                Attack(curCharacter, standingCharacter);
                yield return animationWait;
            }
            curCharacter.OnPassEnemy(standingCharacter);
            standingCharacter.OnEnemyPassesMe(curCharacter);
        }
    }

    //---------------------------------------------------------------------------
    // 공격 관련

    public void Attack(CharacterBase attacker, CharacterBase victim)
    {
        Debug.Log("attack");
        attacker.OnStartAttack(victim);

        //---
        //victim.takedamage// characterBase에 attack함수로 이동시킬까 생각 중
        //attacker.OnAttackSuccess(victim, damage);
        //victim.OnTakeDamage(attacker);// takedamage 내로 이동
        //---

        AnimationController.instance.StartAttackAnimation(attacker, victim);

        attacker.OnEndAttack(victim);
        
    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public IEnumerator UseSkill(CharacterBase skillUser, List<CharacterBase> target)
    {
        Debug.Log("useSkill");
        skillUser.OnUseSkill(target);

        foreach(var t in target)
        {
            Debug.Log(t + " take skill");
        }

        yield return animationWait;

        skillUser.OnEndSkill(target);
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
