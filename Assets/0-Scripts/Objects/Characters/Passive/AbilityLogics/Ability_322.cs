using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_322: PassiveLogic
{
    //"무너지지 않는 마음"
    //자신의 체력이 50%, 30%,10% 이하일 때 자신에게 받는 피해 감소 보너스 스탯을 부여한다.
    //제네의 2-2 특성.

    BonusStat stat_322 = new BonusStat(); // 보너스 스탯

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    private void DamageReduceInit()
    {
        int healthPercentage = (character.health.CurHealth * coefficient["denominator"]) / character.health.TotalHealth; // 체력 비율 계산

        //조건을 체크한다.
        //일정 체력 비율 이하일 경우 그에 맞는 데미지 감소 수치 적용.
        //체력 조건이 전부 해당하지 않을 경우 이 특성으로 얻는 데미지 감소 보너스를 지운다.
        //이 메서드를 각각 "공격을 받기 이전" / "공격을 받은 후" / "데미지를 받기 이전" / "데미지를 받은 후" / "힐을 받은 후" 에 호출한다.
        //HealthSystem에 체력 변경 메서드에 콜백을 걸면 더 간결해 질 수 있을 것 같은데, 로직을 짜기가 어렵네요.
        //"체력이 변경 된 후" 라는 조건이 있으면 좋을 것 같습니다.

        if (healthPercentage <= coefficient["hpRatio_1"])
        {
            stat_322.ReducedDmg = (float)(coefficient["reducedDmgRate_1"]) / coefficient["denominator"];
            character.tempBonusStat.AddBonusStat(stat_322);
        }
        else if (healthPercentage <= coefficient["hpRatio_2"])
        {
            stat_322.ReducedDmg = (float)(coefficient["reducedDmgRate_2"]) / coefficient["denominator"];
            character.tempBonusStat.AddBonusStat(stat_322);
        }
        else if (healthPercentage <= coefficient["hpRatio_3"])
        {
            stat_322.ReducedDmg = (float)(coefficient["reducedDmgRate_3"]) / coefficient["denominator"];
            character.tempBonusStat.AddBonusStat(stat_322);
        }
        else
        {
            character.tempBonusStat.RemoveBonusStat(stat_322);
        }
    }

    public override void OnTakeAttack(CharacterBase enemy)// 공격 받기 이전에
    {
        DamageReduceInit();
    }

    public override void AfterTakeAttacked(CharacterBase enemy)// 공격 받은 이후에
    {
        DamageReduceInit();
    }


    public override void OnTakeDamage(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {
        DamageReduceInit();
    }


    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
    BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
    Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        DamageReduceInit();
    }

    public override void AfterTakeHeal(int heal, CharacterBase skillUser = null,
    BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
    Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐 받은 이후에
    {
        DamageReduceInit();
    }






    public override void OnEndActing()// 행동이 끝난 뒤
    {

    }

    public override void OnRoundEnd()
    {

    }

    public override void OnTurnEnd()// 턴이 끝날 때
    {

    }


    public override void OnUpdate()// 실시간 판정
    {

    }
}

