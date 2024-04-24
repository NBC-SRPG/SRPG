using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleKeyWords;

public class SkillAbility_108 : ExSkillLogic
{
    //아메
    //Ex스킬
    //자신 또는 아군 1명을 선택해 아메 방어력의 
    //100% 만큼[쉴드] 를 부여한다. 2턴 간 유지된다.
    //아메 자신에게 사용했을 시,
    //다음 내 턴의 시작에 코스트를 15 획득한다.
    //자수정 방패는 쉴드인 동시에 하나의 고유 버프로 취급되며, 버프 제거 스킬로 파괴될 수 있다.
    //자기 자신에게 사용했을 시 코스트 회복은 패시브에서 처리함.

  

 

    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {
    }

    public override void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {
        //실드값 = 캐릭터 방어력 * (Ex스킬 계수 + (캐릭터 Ex스킬 레벨 * 캐릭터 Ex스킬 성장 계수)/100)
        target[0].curCharacterBufList.AddBuf(BufKeyword.AmethystShield, 2, character, 0, (int)(character.Defend * (((float)(character.character.exSkill.coefficient[0].value) + (character.character.Growth.exSkillLevel * character.character.exSkill.growthCoefficient)) / 100)));
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {

    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public override void OnUpdate()// 실시간 판정
    {

    }
}
