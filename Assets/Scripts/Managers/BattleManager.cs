using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.TextCore.Text;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BattleManager
{

    public List<GamePlayer> players = new List<GamePlayer>();
    public List<CharacterBase> charactersInBattle = new List<CharacterBase>();
    public Dictionary<string, List<CharacterBase>> charactersAsTeam = new Dictionary<string, List<CharacterBase>>(); 

    public GamePlayer nowPlayer;
    private int nowPlayerNum;

    public event Action TurnStart;

    public bool isShowAnimation;
    //private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    //-----------------------------------------------------------------------------------------------------------------------
    //초기화 함수들

    public void Init()
    {
        players.Clear();
        charactersInBattle.Clear();
        charactersAsTeam.Clear();

        isShowAnimation = false;
    }

    //플레이어가 준비되었는지 확인
    public void GetReady()
    {
        if (players.FindAll(x => x.isReady).Count == players.Count)
        {
            InitBattle();
        }
    }

    //전투 시작
    public void InitBattle()
    {
        nowPlayerNum = 0;

        StartRound();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    //전투 관련 함수들

    private int CheckAttackDamage(CharacterBase attacker, CharacterBase victim)
    {
        attacker.OnStartAttack(victim);

        //------
        //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
        //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
        //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
        //다른 클라이언트는 서버가 준 데미지를 받아옴
        int damage = attacker.character.Attack - victim.character.Defence;// 임시 데미지 계산식

        return damage;
    }

    private int CheckSkillDamage(CharacterBase attacker, CharacterBase victim)
    {

        //------
        //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
        //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
        //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
        //다른 클라이언트는 서버가 준 데미지를 받아옴
        int damage = attacker.curCharacterSkill.SkillFigure - victim.character.Defence;// 임시 데미지 계산식

        return damage;
    }

    //---------------------------------------------------------------------------
    // 이동 관련

    // 서버에 올라가면 어떻게 될지 모르겠음
    public void OnPassCharacter(CharacterBase curCharacter, CharacterBase standingCharacter)
    {
        if (!curCharacter.CheckEnenmy(standingCharacter))// 아군 위를 지나갔을 때
        {
            curCharacter.OnPassAlly(standingCharacter);
            standingCharacter.OnAllyPassedMe(curCharacter);
        }
        else// 적군 위를 지나갔을 때
        {
            curCharacter.OnPassEnemy(standingCharacter);

            if (curCharacter.character.CharacterAttackType == Constants.AttackType.Melee)// 근거리 캐릭터라면
            {
                curCharacter.SetAttackTarget(standingCharacter);
            }

            if (!curCharacter.isDead && !standingCharacter.isDead)
            {
                standingCharacter.OnEnemyPassesMe(curCharacter);
            }
        }
    }


    //---------------------------------------------------------------------------
    // 공격 관련

    public void Attack(CharacterBase attacker, CharacterBase victim)
    {
        attacker.AttackTarget(victim);

        victim.TakeAttacked(attacker);

        if (victim.isDead)
        {
            victim.OnDieInBattle(attacker);
        }

        AnimationController.instance.StartAnimationQueue();
    }

    //-----------------코드 어떤식으로 나눌지 고민 중
    public void DoAttack(CharacterBase attacker, CharacterBase victim)// 공격
    {
        int damage = CheckAttackDamage(attacker, victim);

        //--------------------------------------------------

        victim.characterAnim.SetDamage(damage);

        AnimationController.instance.EnqueueAttackAnimation(attacker, victim);

        victim.health.TakeDamage(damage);

        attacker.OnAttackSuccess(victim, damage);

        victim.OnTakeDamage(attacker);

        attacker.OnEndAttack(victim);

    }

    public void CounterAttack(CharacterBase attacker, CharacterBase victim)// 반격
    {
        int damage = CheckAttackDamage(attacker, victim);

        //--------------------------------------------------

        victim.characterAnim.SetDamage(damage);

        AnimationController.instance.EnqueueCounterAttackAnimation(attacker, victim);

        victim.health.TakeDamage(damage);

        attacker.OnAttackSuccess(victim, damage);

        victim.OnTakeDamage(attacker);

        attacker.OnEndAttack(victim);

    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public void UseSkill(CharacterBase skillUser, List<CharacterBase> target)
    {
        Debug.Log("useSkill");
        skillUser.OnUseSkill(target);

        foreach(var t in target)
        {
            t.health.TakeDamage(20);
            Debug.Log(t + " take skill");
        }

        skillUser.OnEndSkill(target);
    }

    public void SkillAttack(CharacterBase skillUser, List<CharacterBase> target)
    {
        AnimationController.instance.EnqueueSkillAnimation(skillUser, target);

        foreach (CharacterBase victim in target)
        {
            //------
            //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
            //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
            //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
            //다른 클라이언트는 서버가 준 데미지를 받아옴
            int damage = CheckSkillDamage(skillUser, victim);
            //------

            victim.characterAnim.SetDamage(damage);

            victim.health.TakeDamage(damage);

            skillUser.OnSkillAttackSuccess(victim, damage);
        }

    }

    public void SkillHeal(CharacterBase skillUser, List<CharacterBase> target)
    {
        AnimationController.instance.EnqueueSkillAnimation(skillUser, target);

        foreach (CharacterBase victim in target)
        {
            //------
            //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
            //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
            //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
            //다른 클라이언트는 서버가 준 데미지를 받아옴
            int figure = skillUser.curCharacterSkill.SkillFigure;
            //------

            victim.characterAnim.SetDamage(figure);

            victim.health.HealHealth(figure);

            skillUser.OnSkillAttackSuccess(victim, figure);
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //턴 관련 함수들

    public void PlayerTurnEnd()//플레이어 턴 끝
    {
        foreach (CharacterBase characters in charactersInBattle)
        {
            characters.OnEndPlayerTurn();
        }

        nowPlayerNum++;

        if(nowPlayerNum >= players.Count)
        {
            EndRound();
            return;
        }

        PlayerTurnStart();
    }

    public void PlayerTurnStart()//플레이어 턴 시작
    {
        nowPlayer = players[nowPlayerNum];

        TurnStart?.Invoke();
        Debug.Log("nowPlayer" + nowPlayer.playerId);
    }

    private void StartRound()//라운드 시작
    {
        players = players.OrderByDescending(x => x.prioty).ToList();

        foreach (CharacterBase characters in charactersInBattle)
        {
            characters.OnStartTurn();
        }

        PlayerTurnStart();
    }

    private void EndRound()//라운드 끝
    {
        foreach(CharacterBase characters in charactersInBattle)
        {
            characters.OnEndTurn();
        }

        nowPlayerNum = 0;

        StartRound();
    }
}
