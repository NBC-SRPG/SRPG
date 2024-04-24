using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_832: PassiveLogic
{
    //"위압의 마안"
    //자신을 지나가는 적의 이동을 차단한다
    //아메의 3-2 특성

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnPassEnemy(CharacterBase enemtCharacter)// 적군 위를 지나갔을 때 발동
    {

    }

    public override void OnEnemyPassesMe(CharacterBase enemy)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    { 
        AnimationController.instance.EnqueueblockAnimation(enemy, character);
        character.CounterAttack(enemy);

        enemy.BlockMoving();
    }

    public override void OnUpdate()// 실시간 판정
    {

    }
}

