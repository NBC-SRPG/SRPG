using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_Test : CharAnimBase
{
    public override void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        base.PlayAttackAnimation(targetCharacter);

    }

    public void HitEnemy()
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.GetKnockBack(transform.position, 5);
    }

    public void MoveToPosition()
    {
        Vector2 moveTarget = new Vector2(
            (targetCharacter.transform.position.x - (Getdirection(targetCharacter.transform.position).x)),
            targetCharacter.transform.position.y);

        StartCoroutine(MoveToTarget(moveTarget));
    }

}
