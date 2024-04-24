using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_611 : PassiveLogic
{
    //"치유의 바람"
    //피유를 통과하는 아군의 체력을 대상 최대 체력의 10%만큼 회복시킨다
    //피유의 1 특성.

    List<CharacterBase> targetsToHeal = new List<CharacterBase>();
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        int healAmount = (int)(allyCharacter.health.TotalHealth * ((float)(coefficient["healRate"]) / coefficient["denominator"]));
        targetsToHeal.Add(allyCharacter);

        BattleManager.Instance.ExtraSkillHeal(character, healAmount, targetsToHeal, BattleKeyWords.AttackDamageType.Skill);

        targetsToHeal.Clear();
    }
}
