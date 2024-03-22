using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_Test : CharAnimBase
{
    public override void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        base.PlayAttackAnimation(targetCharacter);

    }

    public override void AttackEnemy()
    {
        base.AttackEnemy();
    }

    public void MoveToPosition()
    {
        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x - (Getdirection(targetCharacter.transform.position).x)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTarget(moveTarget));
    }

}
