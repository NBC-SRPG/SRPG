using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;
using UnityEngine.UI;

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
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject battleCanvas;

    [SerializeField] private CinemachineTargetGroup group;

    private CharacterBase attacker;
    private List<CharacterBase> victims;

    public bool isAnimationPlaying;
    public bool doCounter;

    //-----------------------------------------------------------------------------------------------------------------------
    //캐릭터 위치 조정

    private void CharacterSetting(CharacterBase attacker, List<CharacterBase> victims)// 캐릭터 위치 지정
    {
        backGround.gameObject.SetActive(true);
        battleCanvas.gameObject.SetActive(true);

        this.attacker = attacker;
        this.victims = victims;

        attacker.transform.position = attackerPosition.transform.position;
        attacker.transform.localScale = new Vector3(4, 4, 0);
        attacker.characterObject.layer = 31;

        attacker.HealthBar.layer = 31;
        attacker.GetComponentInChildren<SpriteRenderer>().flipX = true;

        group.AddMember(attacker.transform, 1, 5);

        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].transform.position = new Vector3(victimPosition.transform.position.x + (i * 5), victimPosition.transform.position.y, victimPosition.transform.position.z);
            victims[i].transform.localScale = new Vector3(4, 4, 0);
            victims[i].characterObject.layer = 31;

            victims[i].HealthBar.layer = 31;
            victims[i].GetComponentInChildren<SpriteRenderer>().flipX = false;


            group.AddMember(victims[i].transform, 1, 5);
        }

        CameraController.instance.SetCharacterCameraMove(1);
    }

    private void CharacterRelease() // 캐릭터 제자리로
    {
        backGround.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(false);

        attacker.transform.position = attacker.curStandingTile.transform.position;
        attacker.transform.localScale = new Vector3(1, 1, 0);
        attacker.characterObject.layer = 0;
        group.RemoveMember(attacker.transform);

        attacker.HealthBar.layer = 0;
        attacker.health.SetHealthBar();
        attacker.characterAnim.ReleaseTargets();
        attacker.characterAnim.EndAnimation();
        attacker.characterAnim.SetDamage(0);
        attacker = null;

        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].transform.position = victims[i].curStandingTile.transform.position;
            victims[i].transform.localScale = new Vector3(1, 1, 0);
            victims[i].characterObject.layer = 0;

            victims[i].characterAnim.EndAnimation();
            victims[i].characterAnim.SetDamage(0);

            victims[i].HealthBar.layer = 0;
            victims[i].health.SetHealthBar();
            group.RemoveMember(victims[i].transform);
        }
        victims.Clear();

        CameraController.instance.SetCharacterCameraMove(0);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //공격 애니메이션

    public void StartAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { victim };

        CharacterSetting(attacker, victims);

        StartCoroutine(PlayAttackAnimation(attacker, victim));
    }

    private IEnumerator PlayAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        attacker.characterAnim.PlayAttackAnimation(victim);

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    if (!victim.isDead && victim.doCounterAttack)
                    {
                        StartCoroutine(PlayCounterAttackAnimation(victim, attacker));
                        doCounter = false;
                        break;
                    }
                    else
                    {
                        CharacterRelease();
                        isAnimationPlaying = false;
                        break;
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator PlayCounterAttackAnimation(CharacterBase attacker, CharacterBase victim)
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

    //-----------------------------------------------------------------------------------------------------------------------
    //스킬 애니메이션

    public void StartSkillAnimation(CharacterBase attacker, List<CharacterBase> victims)
    {
        isAnimationPlaying = true;

        CharacterSetting(attacker, victims);

        StartCoroutine(PlaySkillAnimation(victims));
    }

    private IEnumerator PlaySkillAnimation(List<CharacterBase> victim)
    {
        attacker.characterAnim.PlaySkillAnimation(victim);

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

    //-----------------------------------------------------------------------------------------------------------------------
    // 방어 애니메이션

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

    //-----------------------------------------------------------------------------------------------------------------------
    // 사망 애니메이션

    public void StartDieAnimation(CharacterBase character)
    {
        StartCoroutine(PlayDieAnimation(character));
    }

    public IEnumerator PlayDieAnimation(CharacterBase character)
    {
        character.characterAnim.PlayDieAnimation();

        while (true)
        {
            if (character.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("die"))
            {
                float animTime = character.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    character.gameObject.SetActive(false);
                    break;
                }
            }

            yield return null;
        }
    }
}
