using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharAnim_107 : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/LightningMageFX"));
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        CameraController.instance.SetMinOrtho(5);

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(transform.parent, 5);

        base.PlaySkillAnimation(targets);

        targetCharacter = targetList[0];
    }

    public void BackStep()
    {
        Vector3 moveTarget = new Vector3(
            (transform.parent.position.x - (GetDirectionOfCharacter().x * 5)),
            transform.parent.position.y, transform.parent.position.z);

        StartCoroutine(MoveToTarget(moveTarget, 20));
    }

    public override void KnockBackEnemy(CharacterBase targetCharacter, int scale)
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.GetKnockBackByLerp(-GetDirectionOfCharacter(), scale, 0.25f);
    }

    public void AttackingTiming()
    {
        MoveToPosition();

        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }

    public void MoveToPosition()
    {
        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x + ((Getdirection(targetCharacter.transform.position).x) * 30)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTargetByLerp(moveTarget, 0.25f));

        particles.PlayParticle("Attack");
    }

    public void Vanish()
    {
        particles.PlayParticle("Spot");
    }

    public void Teleport()
    {
        transform.parent.position = new Vector3(
            (targetCharacter.transform.position.x - ((Getdirection(targetCharacter.transform.position).x) * 3)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        CameraController.instance.AddBattleTargetGroup(transform.parent, 8);
        CameraController.instance.AddBattleTargetGroup(targetCharacter.transform, 8);
    }

    public void SkillTiming()
    {
        particles.PlayParticle("Thunder");

        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        targetCharacter.characterAnim.ShakeCharacter();
        Damage();

        CameraController.instance.ShakeCamera(0.5f, 3f, 0.5f);
    }
}
