using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_108 : PassiveLogic
{
    //ID 008. 아메 Ame
    //패시브 스킬
    //죽음에 이르는 치명적인 피해를 입으면.체력을 40% 회복하고 다음의 효과를 얻는다.
    //부활 시, 자수정 방패를 자신에게 적용하며, 공격력이 50%, 받는 피해가 25%, 받는 체력 회복이 25% 증가한다. 전투중 1회만 발동한다.
    //정확한 메커니즘은
    //1. 죽음에 이르는 치명적인 데미지를 입게 되면
    //2. 치명적인 데미지의 값을 자신의 남은 체력(또는 체력 + 실드) -1 값으로 변경하고 데미지를 받는다. (체력이 1 남고 생존한다)
    //3. 데미지를 받고 생존한 뒤,  패시브 효과를 발동함. 따라서, "죽을 시" 트리거는 발동하지 않음.
    //("죽을 시" 트리거를 발동시키도록 로직을 짜면, 배틀 매니저가 게임 승패를 검사할 때 아메가 패시브를 통해 부활할 수 있어도 무시하고 게임이 끝날 수 있기 때문에.)  

    int isSkill_008;
    BonusStat stat_008;
    List<CharacterBase> characterSelf;


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        characterSelf.Add(character);
        isSkill_008 = 0;
        stat_008 = new BonusStat();
    }

    public override void OnTakeDamage(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {
        if(character.health.shieldList.Count > 0 && damage >= (character.health.GetShield() + character.health.CurHealth) && isSkill_008 == 0)
        {
            damage = (character.health.GetShield() + character.health.CurHealth) - 1;
            isSkill_008 = 1;
        }
        else if (damage >= character.health.CurHealth && isSkill_008 == 0)
        {
            damage = character.health.CurHealth - 1;
            isSkill_008 = 1;
        }
    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
    BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
    Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        if (isSkill_008 == 1)
        {
            stat_008.ExtraAtk = (float)(coefficient["increaseAtkRate"] / coefficient["denominator"]);
            stat_008.ReducedDmg = -1 * (float)(coefficient["reducedDmgRate"]) / coefficient["denominator"];
            character.tempBonusStat.AddBonusStat(stat_008);

            int healAmount = (int)(character.health.TotalHealth * ((float)(coefficient["healRate"]) / coefficient["denominator"]));

            BattleManager.Instance.UseSkill(character, characterSelf);
            BattleManager.Instance.ExtraSkillHeal(character, healAmount, characterSelf, BattleKeyWords.AttackDamageType.Skill);
            isSkill_008 = 2;
        }

    }



    public override void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {
        if (isSkill_008 == 2)
        {
            //패시브가 활성화 된 후라면 힐량을 회복효율 값만큼 증가시킴.
            damage = (int)(damage * (1 + ((float)(coefficient["healEfficiency"]) / coefficient["denominator"])));
        }
    }
}

