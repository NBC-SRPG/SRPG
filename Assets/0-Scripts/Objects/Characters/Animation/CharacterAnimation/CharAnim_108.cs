using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_108 : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/DemonNobleFX"));
    }

    public override void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        CameraController.instance.SetMinOrtho(5);

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["Ready"], 5);

        base.PlayAttackAnimation(targetCharacter);

        particles.PlayParticle("Eye");
    }

    public void AttackTiming()
    {
        CameraController.instance.AddBattleTargetGroup(transform.parent, 10);
        CameraController.instance.AddBattleTargetGroup(targetCharacter.transform, 7);

        particles.PlayParticle("Attack");

        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        base.PlaySkillAnimation(targets);

        targetCharacter = targetList[0];

        if (targetCharacter != transform.parent.GetComponent<CharacterBase>())
        {
            targetCharacter.transform.position = new Vector3(transform.parent.position.x + 10, targetCharacter.transform.position.y, targetCharacter.transform.position.z);
            targetCharacter.characterAnim.FlipCharacterDirection(GetDirectionOfCharacter());
        }

        particles.ChangeParent("Shield", targetCharacter.transform);
    }

    public void HandUp()
    {
        particles.PlayParticle("Grab");
    }

    public void SkillTiming()
    {
        particles.PlayParticle("Snap");
        particles.PlayParticle("Shield");
    }

    public void Grab()
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.transform.position = new Vector3(particles.cameraTransform["GrabPoint"].position.x, targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        particles.PlayParticle("Grab");
    }

    public void Catch()
    {
        targetCharacter.transform.position = particles.cameraTransform["GrabPoint"].position;
    }

    protected override void OnCharacterReleased()
    {
        base.OnCharacterReleased();

        particles.ChangeParent("Shield", transform);
    }

}
