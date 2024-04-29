using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_TargetMarker : CharacterBuf //피유의 패시브 스킬 "표적" 디버프
{
    //이 디버프를 가진 유닛이 적에게 공격을 받을 때, 데미지를 계산하기 전에 적에게 치명타 확률 보너스 스탯을 부여한다. 적의 공격이 끝난 뒤 보너스 스탯은 제거된다.

    BonusStat stat_TargetMarker;

    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.TargetMarker;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "TargetMarker";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        stat_TargetMarker = new BonusStat();
    }

    public override void AfterTakeAttacked(CharacterBase enemy)
    {
        base.AfterTakeAttacked(enemy);

        DoExtraAttack(enemy);

    }

    public override void OnTakeDamage(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {
        if (character.isDead)
        {
            return;
        }
        if(damageType == BattleKeyWords.AttackDamageType.Buf)
        {
            return;
        }


        stat_TargetMarker.EXCritRate = stack;
        enemy.tempBonusStat.AddBonusStat(stat_TargetMarker);

    }

    //OntakeDamage 함수는 너무 많은 경우에 발생하기 때문에 스킬과 일반공격에 피해를 입었을 때로 제한하겠습니다.
    private void DoExtraAttack(CharacterBase enemy)
    {
        if (character.isDead)
        {
            return;
        }

        if (Buffer.character.abilityT2 != null && Buffer.character.abilityT2.id == 622 && enemy != Buffer) //공격자가 피유가 아니고, 피유가 "지원 사격" 특성을 적용 중일 경우.
        {
            int distance = character.pathFinder.GetManhattenDistance(character.curStandingTile, Buffer.curStandingTile); //피유와 이 표적 디버프를 가진 캐릭터의 거리가 6 이하일 경우

            if ((distance <= 6))
            {
                int figure = (int)((Buffer.character.atk * (5f / 10f))); //피유 공격력의 50%로
                List<CharacterBase> characterSelf = new List<CharacterBase>();
                characterSelf.Add(character);

                //무한 공격하는 것을 방지하기 위해 패시브 데미지로 설정하겠습니다.
                BattleManager.Instance.ExtraSkillAttack(Buffer, figure, characterSelf, BattleKeyWords.AttackDamageType.Passive, Constants.ElementType.Grass, "attack", true); //이 표적 디버프를 가진 캐릭터를 공격.
            }
        }
    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        enemy.tempBonusStat.RemoveBonusStat(stat_TargetMarker);

        if (damageType == BattleKeyWords.AttackDamageType.Skill)
        {
            DoExtraAttack(enemy);
        }
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        if (turnCnt > 0)
        {
            DecreaseDuration(1);
        }

        turnCnt++;
    }

    public override string GetName()
    {
        BufName = "표적";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 해당 캐릭터를 공격하는 적의 치명타 확률이 {stack}만큼 증가합니다.";

        return base.GetDescription();
    }
}
