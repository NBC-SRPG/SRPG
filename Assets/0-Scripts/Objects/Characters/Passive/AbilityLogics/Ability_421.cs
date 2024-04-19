using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_421: PassiveLogic
{
    BonusStat stat_421_1 = new BonusStat(); // 보너스 스탯 테스트
    BonusStat stat_421_2 = new BonusStat(); // 보너스 스탯 테스트
    //enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, 2);// 상태이상 화상 테스트
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }


    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {

    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }


    public override void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {

    }


    public override void OnTurnEnd()// 턴이 끝날 때
    {

    }

}

