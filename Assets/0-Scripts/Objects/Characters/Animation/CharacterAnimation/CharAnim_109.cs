using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharAnim_109 : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/MachineFX"));
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        CameraController.instance.SetMinOrtho(5);

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(transform.parent, 1);

        base.PlaySkillAnimation(targets);

        targetCharacter = targetList.Last();
    }

    public void MoveToPosition()
    {
        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x + ((Getdirection(targetCharacter.transform.position).x) * 22)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTargetByLerp(moveTarget, 0.25f));

        particles.PlayParticle("Attack");
    }

    public override void KnockBackEnemy(CharacterBase targetCharacter, int scale)
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.GetKnockBackByLerp(-GetDirectionOfCharacter(), scale, 0.25f);
    }

    public void AttackingTiming()
    {
        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }

    public void MoveToBackPosition()
    {
        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x + ((Getdirection(targetCharacter.transform.position).x) * 3)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTargetByLerp(moveTarget, 0.25f));

        particles.PlayParticle("Move");
    }

    public void Step()
    {
        FlipCharacterDirection(GetDirectionOfCharacter());

        particles.PlayParticle("Move2");
    }

    public void DaggerUp()
    {
        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["DaggerUp"], 0);
    }

    public void SkillTiming()
    {
        particles.PlayParticle("Skill");

        CameraController.instance.AddBattleTargetGroup(transform.parent, 8);
        CameraController.instance.AddBattleTargetGroup(targetCharacter.transform, 8);

        AttackEnemy(targetCharacter);
        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }

}
