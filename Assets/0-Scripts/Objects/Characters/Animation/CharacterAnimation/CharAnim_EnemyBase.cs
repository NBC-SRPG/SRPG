using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharAnim_EnemyBase : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/HeraldFX"));
    }

    public void AttackingTiming()
    {
        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        targetCharacter.characterAnim.ShakeCharacter();
    }

    public void SkillTiming()
    {
        foreach (CharacterBase targets in targetList)
        {
            AttackEnemy(targets);
            KnockBackEnemy(targets, 10);

            targets.characterAnim.ShakeCharacter();
        }
    }

    public void ShowAttackParticle()
    {
        particles.PlayParticle("Attack");
    }

    public void ShowCounterParticle()
    {
        particles.PlayParticle("Counter");
    }


    public void MoveToPosition()
    {
        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x - (Getdirection(targetCharacter.transform.position).x)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTarget(moveTarget, 200f));

        Managers.Sound.Play(Constants.Sound.Effect, "Sounds/Effects/Character_3/Attack_03.mp3");
    }

    public void MoveToLastPosition()
    {
        Vector3 position = targetList.Last().transform.position;

        Vector3 moveTarget = new Vector3(
            (position.x + ((Getdirection(position).x) * 25)),
            position.y, position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTargetByLerp(moveTarget, 0.25f));

    }
}
