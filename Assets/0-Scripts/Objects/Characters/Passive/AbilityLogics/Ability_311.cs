using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_311: PassiveLogic
{
    BonusStat stat = new BonusStat(); // 보너스 스탯 테스트
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        if (enemy.curCharacterBufList.FindNegativeBufAll().Count > 0) //대상이 보유한 디버프 효과의 갯수가 1개 이상이면, 보너스 스탯(주는 피해+15%) 획득
        {
            stat.EnhancedDmg = 15;
            character.tempBonusStat.AddBonusStat(stat);

            /*
            참고 사항
            1. 이 방식을 사용하면 다수의 적을 공격할 때, 상태이상을 갖고 있는 적과 갖고 있지 않은 적이 섞여있어도 모두에게 15% 증가한 데미지를 가하게 됨.
            2. 편성 중인 파티에 특정 캐릭터(여기서는 107. 시스)가 있는지 확인하는 로직 구현 필요.
             */
        }
        else
        {

        }

    }


    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat);
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        int bufCount = 0;
        foreach (CharacterBase target2 in targets)
        {
            bufCount += target2.curCharacterBufList.FindNegativeBufAll().Count;
        }


        if (bufCount > 0) //대상이 보유한 디버프 효과의 갯수가 1개 이상이면, 보너스 스탯(주는 피해+15%) 획득
        {
            stat.EnhancedDmg = 15;
            character.tempBonusStat.AddBonusStat(stat);

            /*
            참고 사항
            1. 이 방식을 사용하면 다수의 적을 공격할 때, 상태이상을 갖고 있는 적과 갖고 있지 않은 적이 섞여있어도 모두에게 15% 증가한 데미지를 가하게 됨.
            2. 편성 중인 파티에 특정 캐릭터(여기서는 107. 시스)가 있는지 확인하는 로직 구현 필요.
             */
        }
        else
        {

        }

    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat);
    }
}

