using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_103 : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/FlameWarriorFX"));
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        base.PlaySkillAnimation(targets);

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["Skill1"], 2);

        Managers.Sound.Play(Constants.Sound.Effect, "Sounds/Effects/Character_3/ExSkill_03.mp3");
    }

    public void AtSlash()
    {
        CameraController.instance.AddBattleTargetGroup(transform.parent, 8);
        foreach (CharacterBase targets in targetList)
        {
            CameraController.instance.AddBattleTargetGroup(targets.transform, 10);
        }

        CameraController.instance.ShakeCamera(0.5f, 5f, 2f);
        DamageAll();
    }

    public void AttackingTiming()
    {
        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }

    public void SkillTiming()
    {
        foreach(CharacterBase targets in targetList)
        {
            AttackEnemy(targets);
            KnockBackEnemy(targets, 3);

            targets.characterAnim.ShakeCharacter();
        }
    }

    public void ShowAttackParticle()
    {
        particles.PlayParticle("Attack");
    }

    public void ShowSkillParticle()
    {
        particles.PlayParticle("Skill");
    }

    public void MoveToPosition()
    {
        Managers.Sound.Play(Constants.Sound.Effect, "Sounds/Effects/Character_3/Attack_03.mp3");

        Vector3 moveTarget = new Vector3(
            (targetCharacter.transform.position.x - (Getdirection(targetCharacter.transform.position).x)),
            targetCharacter.transform.position.y, targetCharacter.transform.position.z);

        FlipCharacter(moveTarget, false);

        StartCoroutine(MoveToTarget(moveTarget, 200f));
    }
}
