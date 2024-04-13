using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.TextCore.Text;
using static UnityEngine.RuleTile.TilingRuleOutput;
using GooglePlayGames.BasicApi;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public List<GamePlayer> players;
    public List<CharacterBase> charactersInBattle;
    public Dictionary<string, List<CharacterBase>> charactersAsTeam;

    public GamePlayer nowPlayer;
    private int nowPlayerNum;
    public int nowRound; 

    public event Action TurnStart;
    public event Action<string> Win;
    public event Action<string> Lose;

    public bool isShowAnimation;
    public bool gameEnd;
    //private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    private BattleUI Ui;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //초기화 함수들

    public void Init()
    {
        players = new List<GamePlayer>();
        charactersInBattle = new List<CharacterBase>();
        charactersAsTeam = new Dictionary<string, List<CharacterBase>>();

        isShowAnimation = false;

        Managers.UI.ShowUI<BattleUI>();
        Ui = Managers.UI.FindUI<BattleUI>();
    }

    //플레이어가 준비되었는지 확인
    public void GetReady()
    {
        int cnt = 0;

        foreach (GamePlayer player in players)
        {
            if (player.isReady)
            {
                cnt++;
            }
        }

        if (players.Count >= 2 && cnt == players.Count)
        {
            InitBattle();
        }
    }

    //전투 시작
    public void InitBattle()
    {
        nowPlayerNum = 0;
        nowRound = 0;

        gameEnd = false;

        StartRound();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    //전투 관련 함수들

    private BattleKeyWords.Damage CheckAttackDamage(CharacterBase attacker, CharacterBase victim)
    {
        BattleKeyWords.Damage damagest = new BattleKeyWords.Damage();

        attacker.OnStartAttack(victim);

        //------
        //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
        //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
        //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
        //다른 클라이언트는 서버가 준 데미지를 받아옴
        int damage = attacker.Attack - victim.Defend;// 임시 데미지 계산식

        damage = (int)((float)damage * ExtraDmgbyAttribute(attacker.character.SO.elementType, victim.character.SO.elementType));

        if(damage < 0)
        {
            damage = 0;
        }

        damagest.damage = damage;
        damagest.attackType = BattleKeyWords.AttackDamageType.Attack;

        return damagest;
    }

    private BattleKeyWords.Damage CheckSkillDamage(CharacterBase attacker, int figure, CharacterBase victim)
    {
        BattleKeyWords.Damage damagest = new BattleKeyWords.Damage();

        //------
        //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
        //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
        //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
        //다른 클라이언트는 서버가 준 데미지를 받아옴
        int damage = figure - victim.Defend;// 임시 데미지 계산식

        damage = (int)((float)damage * ExtraDmgbyAttribute(attacker.character.SO.elementType, victim.character.SO.elementType));

        if (damage < 0)
        {
            damage = 0;
        }

        damagest.damage = damage;
        damagest.attackType = BattleKeyWords.AttackDamageType.Skill;

        return damagest;
    }

    private BattleKeyWords.Damage CheckSkillHealDamage(CharacterBase skillUser, int figure)
    {
        BattleKeyWords.Damage damagest = new BattleKeyWords.Damage();

        int damage = figure;

        if (damage < 0)
        {
            damage = 0;
        }

        damagest.damage = damage;
        damagest.attackType = BattleKeyWords.AttackDamageType.Skill;

        return damagest;
    }

    public BattleKeyWords.Damage CheckExtraDamage(CharacterBase victim, int figure, Constants.ElementType damageType = Constants.ElementType.None, BattleKeyWords.AttackDamageType attackType = BattleKeyWords.AttackDamageType.None)
    {
        BattleKeyWords.Damage damagest = new BattleKeyWords.Damage();

        int damage = figure - victim.Defend;

        damage = (int)((float)damage * ExtraDmgbyAttribute(damageType, victim.character.SO.elementType));

        if (damage < 0)
        {
            damage = 0;
        }

        damagest.damage = damage;
        damagest.attackType = attackType;

        return damagest;
    }

    public float ExtraDmgbyAttribute(Constants.ElementType attackerAttribute, Constants.ElementType victimAttribute)
    {
        switch (attackerAttribute)
        {
            case Constants.ElementType.Fire:
                if(victimAttribute == Constants.ElementType.Water)
                {
                    return 0.75f;
                }
                else if(victimAttribute == Constants.ElementType.Grass)
                {
                    return 1.5f;
                }
                else
                {
                    return 1;
                }
            case Constants.ElementType.Water:
                if (victimAttribute == Constants.ElementType.Bolt)
                {
                    return 0.75f;
                }
                else if (victimAttribute == Constants.ElementType.Fire)
                {
                    return 1.5f;
                }
                else
                {
                    return 1;
                }
            case Constants.ElementType.Bolt:
                if (victimAttribute == Constants.ElementType.Grass)
                {
                    return 0.75f;
                }
                else if (victimAttribute == Constants.ElementType.Water)
                {
                    return 1.5f;
                }
                else
                {
                    return 1;
                }
            case Constants.ElementType.Grass:
                if (victimAttribute == Constants.ElementType.Fire)
                {
                    return 0.75f;
                }
                else if (victimAttribute == Constants.ElementType.Bolt)
                {
                    return 1.5f;
                }
                else
                {
                    return 1;
                }
            case Constants.ElementType.Light:
                if (victimAttribute == Constants.ElementType.Dark)
                {
                    return 1.5f;
                }
                else
                {
                    return 1;
                }
            case Constants.ElementType.Dark:
                if (victimAttribute == Constants.ElementType.Light)
                {
                    return 1.5f;
                }
                else
                {
                    return 1;
                }
            default:
                return 1;
        }
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

            if (curCharacter.character.SO.attackMethod == Constants.AttackMethod.Melee)// 근거리 캐릭터라면
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

        victim.AfterTakeAttacked(attacker);

        if (victim.isDead)
        {
            victim.OnDieInBattle(attacker);
            attacker.OnKillEnemy(victim);
        }

        AnimationController.instance.StartAnimationQueue();
    }

    public void DoAttack(CharacterBase attacker, CharacterBase victim)// 공격
    {
        victim.OnTakeAttack(attacker);

        BattleKeyWords.Damage damage = CheckAttackDamage(attacker, victim);

        //--------------------------------------------------

        AnimationController.instance.EnqueueAttackAnimation(attacker, victim);

        victim.OnTakeDamage(ref damage, attacker, BattleKeyWords.AttackDamageType.Attack);

        attacker.OnAttackSuccess(victim, damage);
            
        attacker.OnEndAttack(victim);

    }

    public void CounterAttack(CharacterBase attacker, CharacterBase victim)// 반격
    {
        victim.OnTakeAttack(attacker);

        BattleKeyWords.Damage damage = CheckAttackDamage(attacker, victim);

        //--------------------------------------------------

        AnimationController.instance.EnqueueCounterAttackAnimation(attacker, victim);

        victim.OnTakeDamage(ref damage, attacker, BattleKeyWords.AttackDamageType.Attack);

        attacker.OnAttackSuccess(victim, damage);

        attacker.OnEndAttack(victim);

    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public void UseSkill(CharacterBase skillUser, List<CharacterBase> target)// 스킬 사용
    {
        Debug.Log("useSkill");
        skillUser.OnUseSkill(target);

        skillUser.curCharacterSkill.skillAbility.UseSkill(target);

        AnimationController.instance.EnqueueSkillAnimation(skillUser, target);

        skillUser.OnEndSkill(target);

        AnimationController.instance.StartAnimationQueue();
    }

    public void EXSkillAttack(CharacterBase skillUser, List<CharacterBase> target)// 스킬 공격
    {
        foreach (CharacterBase victim in target)
        {
            //------
            //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
            //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
            //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
            //다른 클라이언트는 서버가 준 데미지를 받아옴
            BattleKeyWords.Damage damage = CheckSkillDamage(skillUser, skillUser.curCharacterSkill.SkillFigure, victim);
            //------

            victim.OnTakeDamage(ref damage, skillUser, BattleKeyWords.AttackDamageType.Skill);

            skillUser.OnSkillAttackSuccess(victim, damage);
        }

        foreach (var t in target)
        {
            if (t.isDead)
            {
                t.OnDieInBattle(skillUser);
                skillUser.OnKillEnemy(t);
            }
        }

    }

    public void EXSkillHeal(CharacterBase skillUser, List<CharacterBase> target)// 힐
    {
        foreach (CharacterBase victim in target)
        {
            //------
            //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
            //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
            //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
            //다른 클라이언트는 서버가 준 데미지를 받아옴
            BattleKeyWords.Damage figure = CheckSkillHealDamage(skillUser , skillUser.curCharacterSkill.SkillFigure);
            //------

            victim.OnTakeHeal(ref figure, victim, BattleKeyWords.AttackDamageType.Skill);

            skillUser.OnSkillHealSuccess(victim, figure);
        }

        foreach (var t in target)
        {
            if (t.isDead)
            {
                t.OnDieInBattle(skillUser);
                skillUser.OnKillEnemy(t);
            }
        }
    }

    public void ExtraSkillAttack(CharacterBase skillUser, int figure, List<CharacterBase> target, BattleKeyWords.AttackDamageType attackType = BattleKeyWords.AttackDamageType.Skill, string anim = null)// 기타 스킬(추가타 등)
    {
        if(anim != null)
        {
            AnimationController.instance.EnqueueExtraAnimation(skillUser, target, anim);
        }

        foreach (CharacterBase victim in target)
        {
            //------
            //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
            //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
            //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
            //다른 클라이언트는 서버가 준 데미지를 받아옴
            BattleKeyWords.Damage damage;

            if(attackType == BattleKeyWords.AttackDamageType.Skill)
            {
                damage = CheckSkillDamage(skillUser, figure, victim);
            }
            else if(attackType == BattleKeyWords.AttackDamageType.Attack)
            {
                damage = CheckAttackDamage(skillUser, victim);
            }
            else
            {
                damage = CheckExtraDamage(victim, figure, skillUser.character.SO.elementType, attackType);
            }

            //------

            victim.OnTakeDamage(ref damage, skillUser, attackType);

            if (attackType == BattleKeyWords.AttackDamageType.Skill)
            {
                skillUser.OnSkillAttackSuccess(victim, damage);
            }
            else if (attackType == BattleKeyWords.AttackDamageType.Attack)
            {
                skillUser.OnAttackSuccess(victim, damage);
            }
        }

        foreach (var t in target)
        {
            if (t.isDead)
            {
                t.OnDieInBattle(skillUser);
                skillUser.OnKillEnemy(t);
            }
        }
    }

    public void ExtraSkillHeal(CharacterBase skillUser, int figure, List<CharacterBase> target, BattleKeyWords.AttackDamageType attackType = BattleKeyWords.AttackDamageType.Skill)// 기타 스킬(추가타 등)
    {
        foreach (CharacterBase victim in target)
        {
            //------
            //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
            //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
            //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
            //다른 클라이언트는 서버가 준 데미지를 받아옴
            BattleKeyWords.Damage heal = CheckSkillHealDamage(skillUser, figure);
            //------

            victim.OnTakeHeal(ref heal, skillUser, attackType);

            if (attackType == BattleKeyWords.AttackDamageType.Skill)
            {
                skillUser.OnSkillHealSuccess(victim, heal);
            }
        }

        foreach (var t in target)
        {
            if (t.isDead)
            {
                t.OnDieInBattle(skillUser);
                skillUser.OnKillEnemy(t);
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //턴 관련 함수들

    public void PlayerTurnEnd()//플레이어 턴 끝
    {
        foreach (CharacterBase characters in charactersAsTeam[nowPlayer.playerId].FindAll(x => !x.isDead))
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

        foreach (CharacterBase characters in charactersAsTeam[nowPlayer.playerId].FindAll(x => !x.isDead))
        {
            characters.OnStartPlayerTurn();
        }

        Debug.Log("nowPlayer" + nowPlayer.playerId);
        TurnStart?.Invoke();
    }

    private void StartRound()//라운드 시작
    {
        nowRound++;

        Ui.ShowRound(nowRound);
        
        players = players.OrderByDescending(x => x.prioty).ToList();

        foreach (CharacterBase characters in charactersInBattle.FindAll(x => !x.isDead))
        {
            characters.OnRoundStart();
        }

        PlayerTurnStart();
    }

    private void EndRound()//라운드 끝
    {
        foreach(CharacterBase characters in charactersInBattle.FindAll(x => !x.isDead))
        {
            characters.OnRoundEnd();
        }

        nowPlayerNum = 0;

        CheckWin();

        StartRound();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //기타 함수들

    public void CheckWin(CharacterBase dieChracter = null)
    {
        PVEWin(dieChracter);
    }

    public void PVEWin(CharacterBase dieChracter)
    {
        if (gameEnd)
        {
            return;
        }

        //----- 스테이지 목표에 따라 추가
        //switch(stageinfo)
        //{

        //}

        int numbers = 0;

        foreach (CharacterBase chracter in charactersAsTeam[Managers.GameManager.player.playerId])
        {
            if (chracter.isDead)
            {
                numbers++;
            }

            if (numbers == charactersAsTeam[Managers.GameManager.player.playerId].Count)
            {
                EndGame("enemy");
            }
        }
        //-----




        //----- 스테이지 목표에 따라 추가
        //switch(stageinfo)
        //{

        //}

        numbers = 0;
        foreach (CharacterBase chracter in charactersAsTeam["enemy"])
        {
            if (chracter.isDead)
            {
                numbers++;
            }

            if (numbers == charactersAsTeam["enemy"].Count)
            {
                EndGame(Managers.GameManager.player.playerId);
            }
        }
        //-----

    }

    private void EndGame(string player)
    {
        Win?.Invoke(player);
        gameEnd = true;
    }

}
