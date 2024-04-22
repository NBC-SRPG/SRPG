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
        isIndependent = true;
        BonusStat stat_TargetMarker = new BonusStat();
    }

    public override void OnTakeDamage(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {
        stat_TargetMarker.EXCritRate = power;
        enemy.tempBonusStat.AddBonusStat(stat_TargetMarker);

        if(Buffer.character.abilityT2.id == 622 && enemy != Buffer) //공격자가 피유가 아니고, 피유가 "지원 사격" 특성을 적용 중일 경우.
        {
            int distance = character.pathFinder.GetManhattenDistance(character.curStandingTile, Buffer.curStandingTile); //피유와 이 표적 디버프를 가진 캐릭터의 거리가 6 이하일 경우

            if ((distance <= 6))
            {
                int figure = (int)((Buffer.character.atk * (5f / 10f))); //피유 공격력의 50%로
                List<CharacterBase> characterSelf = new List<CharacterBase>();
                characterSelf.Add(character);

                BattleManager.Instance.ExtraSkillAttack(Buffer, figure, characterSelf, BattleKeyWords.AttackDamageType.Skill); //이 표적 디버프를 가진 캐릭터를 공격.
            }
        }

    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        enemy.tempBonusStat.RemoveBonusStat(stat_TargetMarker);
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
}
