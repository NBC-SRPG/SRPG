using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_106 : PassiveLogic
{
    //ID 006. 피유 Piyu
    //패시브 스킬
    //피유의 일반 공격 및 스킬 공격은 적에게 1턴동안 [표적] 상태이상을 부여한다. ( 표적 : 공격을 받을 때 치명타가 발생할 확률이  20%  증가한다.) 
    //세부 설명: 일반 공격은 공격 적중 후에 디버프를 걸지만, 스킬 공격은 스킬을 사용하기 이전에 디버프를 걸도록 해서 스킬 공격의 메리트를 조금 더 높였습니다.

    bool IsabilityT3_632; //특성 632번 "끝없는 추적"이 적용 중이면 표적을 걸 때 지속 턴 +1

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;

    }

    public override void OnStageStart()
    {
        if (character.character.abilityT3 != null && character.character.abilityT3.id == coefficient["abilityId_T3"])
        {
            IsabilityT3_632 = true;
        }
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        if (IsabilityT3_632)
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration_2"], character, coefficient["debufPower"], coefficient["debufStack"]);
        }
        else
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration_1"], character, coefficient["debufPower"], coefficient["debufStack"]);
        }
    }
    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        foreach (CharacterBase target in targets)
        {
            if (IsabilityT3_632)
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration_2"], character, coefficient["debufPower"], coefficient["debufStack"]);
            }
            else
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration_1"], character, coefficient["debufPower"], coefficient["debufStack"]);
            }
        }
    }
}

