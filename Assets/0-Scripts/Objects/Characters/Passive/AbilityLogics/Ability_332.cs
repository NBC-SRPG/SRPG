using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_332: PassiveLogic
{
    //"멸악의 불꽃"
    //자신이 공격을 시작 하기 전 & 스킬을 사용할 때 타겟의 무작위 이로운 버프 하나를 제거한다.
    //캐릭터 성능 상 피해를 주기 전에 버프를 제거하는 것이 더 유리하니까 피해를 주기 전에 제거하도록 했는데, 피해를 준 뒤에 제거하도록 변경을 고민해 볼 수도 있을 것 같습니다.
    //일단 툴팁이 "기본 공격 시" 가 아니라 "공격 시"이고 버프를 태운다는 컨셉인데 스킬에는 적용 안되면 이상할 것 같아서 스킬에도 적용되도록 했습니다.
    //스킬에도 적용되기 때문에 다수의 적의 이로운 버프를 제거할 수 있는데, 이 부분의 밸런스는 차후 고려해 봐야 할 것 같습니다.
    //제네의 3-2 특성.


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        //대상이 가진 파괴 가능한 버프 중 무작위 하나를 불러와 파괴한다.
        enemy.curCharacterBufList.RemoveBuf(enemy.curCharacterBufList.FindPositiveBufRandom(1, true)[0]);
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {

    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        foreach (CharacterBase target in targets)
        {
            target.curCharacterBufList.RemoveBuf(target.curCharacterBufList.FindPositiveBufRandom(1, true)[0]);
        }
    }


    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }
}

