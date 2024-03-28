using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_Test : CharAnimBase
{
    public void AttackingTiming()
    {
        AttackEnemy();
        KnockBackEnemy(10);

        targetCharacter.characterAnim.ShakeCharacter();
    }

    public void Damage()
    {
        targetCharacter.characterAnim.ShowDamage();
    }

    public void MoveToPosition()
    {
        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x - (Getdirection(targetCharacter.transform.position).x)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTarget(moveTarget, 200f));
    }

}
