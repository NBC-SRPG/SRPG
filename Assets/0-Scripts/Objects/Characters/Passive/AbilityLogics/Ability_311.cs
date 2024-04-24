using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_311: PassiveLogic
{
    //"무자비한 일격" (1)
    //상태 이상을 가진 적을 공격할 때, 데미지 증가 보너스 부여.
    // *"공격할 때"는 기본 공격과 스킬 공격을 모두 포함하는 것으로 간주했습니다.
    //파티에 "시스" 캐릭터가 있을 경우 2배 적용.
    //제네의 1 특성


    BonusStat stat_311 = new BonusStat(); // 보너스 스탯
    bool hasCharacterWithId7; //파티에 "시스"가 있는지 체크해서 bool 값을 저장하는 필드.


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;


    }

    public override void OnStageStart()
    {
        checkExisitSis();
    }

    private void checkExisitSis()
    {
        hasCharacterWithId7 = false;
        foreach (Character ally in character.player.party)// 플레이어의 파티에 시스가 있을 경우를 체크하도록 변경
        {
            if (ally != null && ally.SO.id == 7)
            {
                hasCharacterWithId7 = true;
            }
        }
    }


    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {

        if (enemy.curCharacterBufList.FindNegativeBufAll().Count > 0) //대상이 보유한 디버프 효과의 갯수가 1개 이상이면, 보너스 스탯(주는 피해+15%) 획득
        {
            checkExisitSis();
            if (hasCharacterWithId7)
            {
                stat_311.EnhancedDmg = ((float)(coefficient["enhancedDmgRate"]) /coefficient["denominator"]) * coefficient["multiply"]; //파티에 시스가 있으면 계수 2배
            }
            else
            {
                stat_311.EnhancedDmg = ((float)(coefficient["enhancedDmgRate"]) / coefficient["denominator"]);
            }
            character.tempBonusStat.AddBonusStat(stat_311);

            /*
            참고 사항
            1. 이 방식을 사용하면 다수의 적을 공격할 때, 상태이상을 갖고 있는 적과 갖고 있지 않은 적이 섞여있어도 모두에게 15% 증가한 데미지를 가하게 됨.
            2. 편성 중인 파티에 특정 캐릭터(여기서는 ID 7번 시스)가 있는지 확인하는 로직 구현 완료.
             */
        }
        else
        {

        }

    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        int bufCount = coefficient["defaltBufCount"]; //defaltBufCount = 0
        foreach (CharacterBase target in targets)
        {
            bufCount += target.curCharacterBufList.FindNegativeBufAll().Count;
        }

        if (bufCount > coefficient["defaltBufCount"]) //대상이 보유한 디버프 효과의 갯수가 1개 이상이면, 보너스 스탯(주는 피해+15%) 획득
        {
            checkExisitSis();
            if (hasCharacterWithId7)
            {
                stat_311.EnhancedDmg = ((float)(coefficient["enhancedDmgRate"]) / coefficient["denominator"]) * coefficient["multiply"]; //파티에 시스가 있으면 계수 2배
            }
            else
            {
                stat_311.EnhancedDmg = ((float)(coefficient["enhancedDmgRate"]) / coefficient["denominator"]);
            }
            character.tempBonusStat.AddBonusStat(stat_311);

            /*
            참고 사항
            1. 이 방식을 사용하면 다수의 적을 공격할 때, 상태이상을 갖고 있는 적과 갖고 있지 않은 적이 섞여있어도 모두에게 15% 증가한 데미지를 가하게 됨.
             2. 편성 중인 파티에 특정 캐릭터(여기서는 ID 7번 시스)가 있는지 확인하는 로직 구현 완료.
             */
        }
        else
        {

        }

    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_311);
    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_311);
    }
}

