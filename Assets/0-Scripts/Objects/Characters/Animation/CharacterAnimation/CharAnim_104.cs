using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_104 : CharAnimBase
{
    private Vector2 left;
    private Vector2 right;

    List<CharacterBase> hitTargets;
    List<CharacterBase> healTargets;


    Vector3 hitPosition = new Vector3();
    Vector3 healPosition = new Vector3();

    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/PriestFX"));

        hitTargets = new List<CharacterBase>();
        healTargets = new List<CharacterBase>();
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        CameraController.instance.SetMinOrtho(5);
        transform.parent.transform.position = CameraController.instance.battleTargetGroup.transform.position;

        base.PlaySkillAnimation(targets);


        int p = 0;
        int i = 0;

        left = particles.cameraTransform["Left"].transform.localToWorldMatrix.GetPosition();
        right = particles.cameraTransform["Right"].transform.localToWorldMatrix.GetPosition();

        foreach (CharacterBase target in targets)
        {
            if(target != transform.GetComponentInParent<CharacterBase>())
            {
                if(p == 0)
                {
                    target.transform.position = new Vector2(right.x + (2 * i), right.y);
                    target.characterAnim.FlipCharacterDirection(Vector2.left);
                    p++;
                }
                else
                {
                    target.transform.position = new Vector2(left.x - (2 * i), left.y);
                    target.characterAnim.FlipCharacterDirection(Vector2.right);
                    p = 0;
                    i++;
                }
            }
        }

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["Pray"], 0);

        SetSkilPosition();
    }

    public void SetSkilPosition()
    {
        hitTargets.Clear();
        healTargets.Clear();

        foreach (CharacterBase target in targetList)
        {
            if (target.characterAnim.GetDamageFigure() < 0)
            {
                hitTargets.Add(target);
            }
            else
            {
                healTargets.Add(target);
            }
        }

        for (int i = 0; i < hitTargets.Count; i++)
        {
            hitPosition = particles.particleMap["SkillHit"].transform.GetChild(i).position;
            particles.particleMap["SkillHit"].transform.GetChild(i).gameObject.SetActive(true);
            particles.particleMap["SkillHit"].transform.GetChild(i).position = new Vector3(hitTargets[i].transform.position.x, hitPosition.y, hitPosition.z);
        }

        for (int i = 0; i < healTargets.Count; i++)
        {
            healPosition = particles.particleMap["SkillHeal"].transform.GetChild(i).position;
            particles.particleMap["SkillHeal"].transform.GetChild(i).gameObject.SetActive(true);
            particles.particleMap["SkillHeal"].transform.GetChild(i).position = new Vector3(healTargets[i].transform.position.x, healPosition.y, healPosition.z);
        }
    }

    public void SkillHit()
    {
        particles.PlayParticle("SkillHeal");
        particles.PlayParticle("SkillHit");

        foreach(CharacterBase target in hitTargets)
        {
            AttackEnemy(target);
            target.characterAnim.ShakeCharacter();
            Damage();
        }
    }


    public void AtSlash()
    {
        CameraController.instance.AddBattleTargetGroup(transform.parent, 8);
        foreach (CharacterBase targets in targetList)
        {
            CameraController.instance.AddBattleTargetGroup(targets.transform, 10);
        }

        particles.PlayParticle("Skill");
    }

    public void AttackingTiming()
    {
        particles.PlayParticle("Attack2");

        particles.ChangeParent("Attack", targetCharacter.characterAnim.transform);

        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 5);

        particles.PlayParticle("Attack");
        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }

    protected override void OnCharacterReleased()
    {
        base.OnCharacterReleased();

        CameraController.instance.SetMinOrtho(9);

        for(int i =0; i < particles.particleMap["SkillHit"].transform.childCount; i++)
        {
            hitPosition = particles.particleMap["SkillHit"].transform.GetChild(i).position;
            particles.particleMap["SkillHit"].transform.GetChild(i).gameObject.SetActive(false);
            particles.particleMap["SkillHit"].transform.GetChild(i).position = new Vector3(transform.position.x, hitPosition.y, hitPosition.z);
        }

        for (int i = 0; i < particles.particleMap["SkillHeal"].transform.childCount; i++)
        {
            healPosition = particles.particleMap["SkillHeal"].transform.GetChild(i).position;
            particles.particleMap["SkillHeal"].transform.GetChild(i).gameObject.SetActive(false);
            particles.particleMap["SkillHeal"].transform.GetChild(i).position = new Vector3(transform.position.x, healPosition.y, healPosition.z);
        }

        particles.ChangeParent("Attack", transform);
    }
}
