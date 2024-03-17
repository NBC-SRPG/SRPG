using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject attackerPosition;
    [SerializeField] private GameObject victimPosition;

    [SerializeField] private CinemachineTargetGroup group;

    private CharacterBase attacker;
    private List<CharacterBase> victims;

    public bool isAnimationPlaying;

    private void CharacterSetting(CharacterBase attacker, List<CharacterBase> victims)
    {
        this.attacker = attacker;
        this.victims = victims;

        attacker.transform.position = attackerPosition.transform.position;
        attacker.transform.localScale = new Vector3(4, 4, 0);
        attacker.character.gameObject.layer = 31;
        attacker.GetComponentInChildren<SpriteRenderer>().flipX = true;

        group.AddMember(attacker.transform, 1, 5);

        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].transform.position = new Vector3(victimPosition.transform.position.x + (i * 5), victimPosition.transform.position.y, victimPosition.transform.position.z);
            victims[i].transform.localScale = new Vector3(4, 4, 0);
            victims[i].character.gameObject.layer = 31;
            victims[i].GetComponentInChildren<SpriteRenderer>().flipX = false;


            group.AddMember(victims[i].transform, 1, 5);
        }

    }

    private void CharacterRelease()
    {
        attacker.transform.position = attacker.curStandingTile.transform.position;
        attacker.transform.localScale = new Vector3(1, 1, 0);
        attacker.character.gameObject.layer = 0;
        group.RemoveMember(attacker.transform);

        attacker = null;

        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].transform.position = victims[i].curStandingTile.transform.position;
            victims[i].transform.localScale = new Vector3(1, 1, 0);
            victims[i].character.gameObject.layer = 0;
            victims[i].characterAnim.EndAnimation();
            group.RemoveMember(victims[i].transform);
        }
        victims.Clear();

    }

    public void StartAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { victim };

        CharacterSetting(attacker, victims);

        StartCoroutine(PlayAttackAnimation(victim));
    }

    private IEnumerator PlayAttackAnimation(CharacterBase victim)
    {
        attacker.characterAnim.PlayAttackAnimation(victim);

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    CharacterRelease();
                    isAnimationPlaying = false;
                    break;
                }
            }

            yield return null;
        }
    }

    public void StartDefendAnimation(CharacterBase attacker, CharacterBase defender)
    {
        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { defender };

        CharacterSetting(attacker, victims);

        StartCoroutine(PlayDefendAnimation(defender));
    }

    private IEnumerator PlayDefendAnimation(CharacterBase defender)
    {
        defender.characterAnim.PlayDefendAnimation(attacker);

        while (true)
        {
            if (defender.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("defend"))
            {
                float animTime = defender.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    CharacterRelease();
                    isAnimationPlaying = false;
                    break;
                }
            }

            yield return null;
        }
    }
}
