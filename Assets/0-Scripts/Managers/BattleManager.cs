using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.TextCore.Text;
using static UnityEngine.RuleTile.TilingRuleOutput;
using GooglePlayGames.BasicApi;
using static Constants;
using UnityEditor.Experimental.GraphView;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public List<GamePlayer> players;
    public List<CharacterBase> charactersInBattle;
    public Dictionary<string, List<CharacterBase>> charactersAsTeam;

    public GamePlayer nowPlayer;
    private int nowPlayerNum;
    public int nowRound;

    public int nowWave;

    public event Action GameStart;
    public event Action TurnStart;
    public event Action RoundStart;
    public event Action<string> Win;
    public event Action<string> Lose;

    public bool isShowAnimation;
    public bool gameEnd;
    //private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    private BattleUI Ui;

    public StageSO stage;

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

        stage = Managers.GameManager.thisStage;

        Managers.Resource.Instantiate("Map/" + stage.prefabsName);
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

        GameStart?.Invoke();

        Ui.SetGoalText();

        StartCoroutine(intro());
    }

    private IEnumerator intro()
    {
        float waitTime = 1f;

        if(stage.clear == StageClear.Run && stage.targetGrid.Count > 0)
        {
            foreach(Vector2Int trans in stage.targetGrid)
            {
                CameraController.instance.AddTargetGroup(MapManager.instance.map[trans].transform);
            }

            CameraController.instance.SetCameraOnSelected();

            waitTime = 1.5f;
        }
        
        if(stage.clear == StageClear.Assasinate && stage.targetEnemy.Count > 0)
        {
            foreach(CharacterBase enemy in charactersAsTeam["enemy"])
            {
                if (stage.GetTargetEnemy().Contains(enemy.character))
                {
                    CameraController.instance.AddTargetGroup(enemy.transform);
                }
            }

            CameraController.instance.SetCameraOnSelected();

            waitTime = 1.5f;
        }

        CameraController.instance.CameraIntro();

        yield return new WaitForSeconds(waitTime);

        StartRound();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    //전투 관련 함수들

    private bool CheckCrit(int critRate)
    {
        int random;

        random = UnityEngine.Random.Range(0, 100);

        if(random < critRate)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    // 데미지 계산식 : Dmg = (Attack(ex스킬일 경우 x계수/100) - 0.25*enemy.Defend*PenetrateDef) * EnhanceDMG * enemy.ReduceDMG * (치명타시)CritDMG * 속성상성


    private BattleKeyWords.Damage CheckAttackDamage(CharacterBase attacker, CharacterBase victim)
    {
        BattleKeyWords.Damage damagest = new BattleKeyWords.Damage();

        attacker.OnStartAttack(victim);

        //------
        //이 부분은 서버에서 처리한 뒤 클라이언트로 전달하도록 후에 변경(치명타 발생 확률 때문)
        //입력의 주체인 클라이언트가 서버에 데미지 계산 요청 
        //이후 서버가 데미지를 계산해서 모든 클라이언트에 전달
        //다른 클라이언트는 서버가 준 데미지를 받아옴
        float totalDefend = (0.25f * ((float)victim.Defend * (1 - attacker.PenetrateDef)));

        float damage = attacker.Attack - totalDefend;// 방어력 계산

        damage = damage * (1 + attacker.EnhanceDMG) * (1 - victim.ReduceDMG);// 데미지 증감 계산

        if (CheckCrit(attacker.CritRate))// 치명타 계산
        {
            damagest.isCriticalHit = true;
            damage = damage * attacker.CritDMG;
        }

        damage = (damage * ExtraDmgbyAttribute(attacker.character.SO.elementType, victim.character.SO.elementType));

        if(damage < 0)
        {
            damage = 0;
        }

        damagest.damage = (int)damage;
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
        float totalDefend = (0.25f * ((float)victim.Defend * (1 - attacker.PenetrateDef)));

        float damage = figure - totalDefend;// 방어력 계산

        damage = damage * (1 + attacker.EnhanceDMG) * (1 - victim.ReduceDMG);// 데미지 증감 계산

        if (CheckCrit(attacker.CritRate))// 치명타 계산
        {
            damagest.isCriticalHit = true;
            damage = damage * attacker.CritDMG;
        }

        damage = (damage * ExtraDmgbyAttribute(attacker.character.SO.elementType, victim.character.SO.elementType));

        if (damage < 0)
        {
            damage = 0;
        }

        damagest.damage = (int)damage;
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

    public BattleKeyWords.Damage CheckExtraDamage(CharacterBase skillUser, CharacterBase victim, int figure, bool isCrit, Constants.ElementType damageType = Constants.ElementType.None, BattleKeyWords.AttackDamageType attackType = BattleKeyWords.AttackDamageType.None)
    {
        BattleKeyWords.Damage damagest = new BattleKeyWords.Damage();

        float totalDefend = (0.25f * ((float)victim.Defend * (1 - skillUser.PenetrateDef)));

        float damage = figure - totalDefend;// 방어력 계산

        damage = damage * (1 + skillUser.EnhanceDMG) * (1 - victim.ReduceDMG);// 데미지 증감 계산

        if (CheckCrit(skillUser.CritRate) && isCrit)// 치명타 계산
        {
            damagest.isCriticalHit = true;
            damage = damage * skillUser.CritDMG;
        }

        damage = (damage * ExtraDmgbyAttribute(skillUser.character.SO.elementType, victim.character.SO.elementType));

        if (damage < 0)
        {
            damage = 0;
        }

        damagest.damage = (int)damage;
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
        AnimationController.instance.EnqueueSkillAnimation(skillUser, target);

        Debug.Log("useSkill");
        skillUser.OnUseSkill(target);

        skillUser.curCharacterSkill.skillAbility.UseSkill(target);

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

    public void ExtraSkillAttack(CharacterBase skillUser, int figure, List<CharacterBase> target, BattleKeyWords.AttackDamageType attackType = BattleKeyWords.AttackDamageType.Skill, string anim = null, bool isCrit = false)// 기타 스킬(추가타 등)
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
                damage = CheckExtraDamage(skillUser, victim, figure, isCrit, skillUser.character.SO.elementType, attackType);
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

        RoundStart?.Invoke();

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

    public void SetWave(int wave)
    {
        nowWave = wave;
        Ui.ShowWave(nowWave);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //기타 함수들

    public void CharacterDie(CharacterBase character)
    {
        CheckWin(character);
    }

    public void CheckWin(CharacterBase dieChracter = null, OverlayTile location = null)
    {
        PVEWin(dieChracter, location);
    }

    public void PVEWin(CharacterBase dieChracter, OverlayTile location)
    {
        if (gameEnd)
        {
            return;
        }

        //-----스테이지 목표에 따라 추가
        //switch (stage.clear)
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
        switch (stage.clear)
        {
            case StageClear.ClearAll:
                EnemyAllDead(); 
                break;
            case StageClear.Assasinate:
                TargetEnemyDead(dieChracter);
                break;
            case StageClear.Run:
                MoveToTarget(location);
                break;
            case StageClear.Defence:
                DefenceTurn();
                break;
        }

        //-----
        Ui.SetGoalText();

    }

    private void EndGame(string player)
    {
        Win?.Invoke(player);
        gameEnd = true;
    }


    public void SpawnCharacters(List<CharacterBase> characterList, GamePlayer player)
    {
        int i = 0;

        foreach (CharacterBase character in characterList)
        {
            if (i < MapManager.instance.playerStartTiles[player.playerStartPosition].startTile.Count)
            {
                character.transform.SetParent(transform);

                character.SpawnCharacter(MapManager.instance.playerStartTiles[player.playerStartPosition].startTile[i], transform, MapManager.instance.playerStartTiles[player.playerStartPosition].startDirection);
                i++;
            }
            else
            {
                character.gameObject.SetActive(false);
                i++;
            }
        }
    }

    public void SpawnEnemy(List<CharacterAI> characterList, int spawnPosition)
    {
        if(MapManager.instance.enemyStartTiles == null || MapManager.instance.enemyStartTiles.Count <= 0)
        {
            return;
        }

        int i = 0;

        foreach (CharacterAI character in characterList)
        {
            if (i < MapManager.instance.enemyStartTiles[spawnPosition].startTile.Count)
            {
                if(MapManager.instance.enemyStartTiles[spawnPosition].startTile[i].curStandingCharater != null)// 소환할 자리에 무언가가 있다면
                {
                    continue;
                }

                character.transform.SetParent(transform);

                character.SpawnCharacter(MapManager.instance.enemyStartTiles[spawnPosition].startTile[i], transform, MapManager.instance.enemyStartTiles[spawnPosition].startDirection);
                i++;
            }
            else
            {
                character.gameObject.SetActive(false);
                i++;
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //조건 판단 함수

    public int GetRemainEnemy()
    {
        int cnt = 0;

        foreach (CharacterBase chracter in charactersAsTeam["enemy"])
        {
            if (!chracter.isDead)
            {
                cnt++;
            }
        }

        return cnt;
    }

    public void EnemyAllDead()
    {
        int numbers = 0;
        foreach (CharacterBase chracter in charactersAsTeam["enemy"])
        {
            if (chracter.isDead)
            {
                numbers++;
            }

            if (numbers == charactersAsTeam["enemy"].Count && nowWave == stage.waveNumber)
            {
                EndGame(Managers.GameManager.player.playerId);
            }
        }
    }

    public void TargetEnemyDead(CharacterBase dieCharacter)
    {
        if(dieCharacter == null)
        {
            return;
        }

        if(stage.targetEnemy == null || stage.targetEnemy.Count == 0)
        {
            Debug.Log("no targetEnemy");
            return;
        }

        int cnt = 0;
        if(dieCharacter.character != null && stage.GetTargetEnemy().Contains(dieCharacter.character))
        {
            cnt++;
            if(cnt == stage.targetEnemy.Count)
            {
                EndGame(Managers.GameManager.player.playerId);
            }
        }
    }

    public void MoveToTarget(OverlayTile location)
    {
        if(location == null)
        {
            return;
        }

        if(stage.targetGrid == null || stage.targetGrid.Count == 0)
        {
            Debug.Log("no targetGrid");
            return;
        }

        if (stage.targetGrid.Contains(location.grid2DLocation))
        {
            EndGame(Managers.GameManager.player.playerId);
        }
    }

    public void DefenceTurn()
    {
        if(stage.defenceRound == 0)
        {
            Debug.Log("no turn");
            return;
        }

        if(nowRound == stage.defenceRound)
        {
            EndGame(Managers.GameManager.player.playerId);
        }
    }
}
