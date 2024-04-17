using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_104 : CharAnimBase
{
    private Vector2 left;
    private Vector2 right;

    List<Transform> hitTargets;
    List<Transform> healTargets;

    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/PriestFX"));

        hitTargets = new List<Transform>();
        healTargets = new List<Transform>();
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
            if (target.characterAnim.GetDamage() < 0)
            {
                hitTargets.Add(target.transform);
            }
            else
            {
                healTargets.Add(target.transform);
            }
        }

        Vector3 hitPosition = particles.particleMap["SkillHit"].transform.position;
        for (int i = 0; i < hitTargets.Count; i++)
        {
            particles.particleMap["SkillHit"].transform.GetChild(i).gameObject.SetActive(true);
            particles.particleMap["SkillHit"].transform.GetChild(i).position = new Vector3(hitTargets[i].position.x, hitPosition.y - 1, hitPosition.z);
        }

        Vector3 healPosition = particles.particleMap["SkillHeal"].transform.position;
        for (int i = 0; i < healTargets.Count; i++)
        {
            particles.particleMap["SkillHeal"].transform.GetChild(i).gameObject.SetActive(true);
            particles.particleMap["SkillHeal"].transform.GetChild(i).position = new Vector3(healTargets[i].position.x, healPosition.y - 1, healPosition.z);
        }
    }

    public void SkillHit()
    {
        particles.PlayParticle("SkillHeal");
        particles.PlayParticle("SkillHit");
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

    protected override void OnCharacterReleased()
    {
        base.OnCharacterReleased();

        CameraController.instance.SetMinOrtho(9);
    }
}
