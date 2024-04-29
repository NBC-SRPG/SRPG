using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_106 : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/ArcherFX"));
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        CameraController.instance.SetMinOrtho(5);

        foreach (CharacterBase target in targets)
        {
            SetRangePosition(target, GetDirectionOfCharacter(), 20);
        }

        base.PlaySkillAnimation(targets);

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["BowUp"], 0);

        Managers.Sound.Play(Constants.Sound.Effect, "Effects/Character_6/ExSkill_06.mp3");
    }

    public override void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        SetRangePosition(targetCharacter, GetDirectionOfCharacter(), 20);

        base.PlayAttackAnimation(targetCharacter);

        Managers.Sound.Play(Constants.Sound.Effect, "Effects/Character_6/Attack_06.mp3");
    }

    public override void PlayExtraAnimation(List<CharacterBase> victims, string anim)
    {
        base.PlayExtraAnimation(victims, anim);

        Managers.Sound.Play(Constants.Sound.Effect, "Effects/Character_6/Attack_06.mp3");
    }

    public void SetSkilPosition()
    {
        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["BowDown"], 2);
    }

    public void SetSkillPosition2()
    {
        CameraController.instance.ResetBattleGroup();

        CameraController.instance.AddBattleTargetGroup(transform.parent, 8);
        foreach (CharacterBase targets in targetList)
        {
            CameraController.instance.AddBattleTargetGroup(targets.transform, 6);
        }

        particles.PlayParticle("Charging");
        CameraController.instance.ShakeCamera(3f, 5f, 2f);
    }

    public void SkillTiming()
    {
        Vector2 direction = -GetDirectionOfCharacter();

        particles.StopParticle("Charging");
        particles.PlayParticle("ShootArrow");
        GetKnockBackByLerp(direction, 10, 0.25f);
    }

    public void SkillHit()
    {
        foreach (CharacterBase target in targetList)
        {
            AttackEnemy(target);
            target.characterAnim.ShakeCharacter();
            KnockBackEnemy(target, 15);
        }
    }

    public void AttackTiming()
    {
        particles.PlayParticle("Attack");

        particles.ChangeParent("Hit", targetCharacter.characterAnim.transform);

        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 15);

        particles.PlayParticle("Hit");
        targetCharacter.characterAnim.ShakeCharacter();

        Damage();
    }

    protected override void OnCharacterReleased()
    {
        base.OnCharacterReleased();

        particles.ChangeParent("Attack", transform);
    }

}
