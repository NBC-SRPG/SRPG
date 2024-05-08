using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour
{
    public AnimTestCharacter testCharacter;

    public AnimTestCharacter targetCharacter;
    public List<AnimTestCharacter> targetCharacters;

    private void Awake()
    {
        Managers.UI.ShowUI<BattleUI>();
    }

    public void OnAttackButton()
    {
        AnimationController.instance.StartAttackAnimation(testCharacter, targetCharacter);
    }

    public void OnSkillButton()
    {
        List<CharacterBase> list = new List<CharacterBase>();
        list.AddRange(targetCharacters);
        AnimationController.instance.StartSkillAnimation(testCharacter, list);
    }

    public void OnCounterButton()
    {
        //AnimationController.instance.EnqueueAttackAnimation(targetCharacter, testCharacter);
        //AnimationController.instance.EnqueueCounterAttackAnimation(testCharacter, targetCharacter);
        AnimationController.instance.EnqueueblockAnimation(targetCharacter, testCharacter);

        AnimationController.instance.StartAnimationQueue();
    }
}
