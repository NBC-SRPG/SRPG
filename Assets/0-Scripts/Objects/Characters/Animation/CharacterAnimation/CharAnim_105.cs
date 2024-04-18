using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_105 : CharAnimBase
{
    WaitForSeconds delay = new WaitForSeconds(0.8f);

    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/WaterMageFX"));
    }

    public override void PlaySkillAnimation(List<CharacterBase> targets)
    {
        CameraController.instance.SetMinOrtho(5);
        transform.parent.transform.position = CameraController.instance.battleTargetGroup.transform.position;

        foreach (CharacterBase target in targets)
        {
            SetRangePosition(target, -GetDirectionOfCharacter(), 20);
        }

        base.PlaySkillAnimation(targets);

        CameraController.instance.ResetBattleGroup();
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["Pray"], 0);
    }

    public void OpenEye()
    {
        CameraController.instance.AddBattleTargetGroup(particles.cameraTransform["Pray"], 2);
    }

    public void SkillTiming()
    {
        particles.PlayParticle("Rain");

        CameraController.instance.AddBattleTargetGroup(transform.parent, 8);
        foreach(CharacterBase target in targetList)
        {
            CameraController.instance.AddBattleTargetGroup(target.transform, 8);
        }

        StartCoroutine(nameof(RainDamage));
        CameraController.instance.ShakeCamera(2.6f, 5f, 2f);
        Invoke(nameof(StopRain), 2.6f);
    }

    private IEnumerator RainDamage()
    {
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < 3; i++)
        {
            foreach (CharacterBase targets in targetList)
            {
                AttackEnemy(targets);

                targets.characterAnim.ShakeCharacter();
            }
            DamageAll();

            yield return delay;
        }
    }

    private void StopRain()
    {
        particles.StopParticle("Rain");
    }

    public void AttackingTiming()
    {
        particles.PlayParticle("Attack");

        particles.ChangeParent("Spark", targetCharacter.characterAnim.transform);

        AttackEnemy(targetCharacter);
        KnockBackEnemy(targetCharacter, 10);

        particles.PlayParticle("Spark");
        targetCharacter.characterAnim.ShakeCharacter();
        Damage();
    }
}
