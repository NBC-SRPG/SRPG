using Cinemachine;
using System;
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
    public Queue<Action> animationQueue = new Queue<Action>();

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
        attacker.characterAnim.FlipCharacterDirection(Vector2.right);

        group.AddMember(attacker.transform, 1, 5);

        for (int i = 0; i < victims.Count; i++)
        {
            if (victims[i] == attacker)
            {
                continue;
            }

            victims[i].transform.position = new Vector3(victimPosition.transform.position.x + (i * 5), victimPosition.transform.position.y, victimPosition.transform.position.z);
            victims[i].transform.localScale = new Vector3(4, 4, 0);
            victims[i].characterObject.layer = 31;

            victims[i].HealthBar.layer = 31;
            victims[i].characterAnim.FlipCharacterDirection(Vector2.left);


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
        attacker.characterAnim.EndAnimation(attacker.isWalking);
        attacker.characterAnim.SetDamage(0);
        attacker = null;

        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].transform.position = victims[i].curStandingTile.transform.position;
            victims[i].transform.localScale = new Vector3(1, 1, 0);
            victims[i].characterObject.layer = 0;

            victims[i].characterAnim.EndAnimation(victims[i].isWalking);
            victims[i].characterAnim.SetDamage(0);

            victims[i].HealthBar.layer = 0;
            victims[i].health.SetHealthBar();
            group.RemoveMember(victims[i].transform);
        }
        victims.Clear();

        CameraController.instance.SetCharacterCameraMove(0);

        isAnimationPlaying = false;
    }

    public void EndAimation()
    {
        isAnimationPlaying = false;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //공격 애니메이션

    public void EnqueueAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        Debug.Log("Enqueue attack");
        animationQueue.Enqueue(() => StartAttackAnimation(attacker, victim));
    }

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
                    PlayNextAnimation();

                    break;
                }
            }

            yield return null;
        }
    }

    public void EnqueueCounterAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        Debug.Log("Enqueue counterattack");
        animationQueue.Enqueue(() => StartCounterAnimation(attacker, victim));
    }

    private void StartCounterAnimation(CharacterBase attacker, CharacterBase victim)
    {
        StartCoroutine(PlayCounterAttackAnimation(attacker, victim));
    }

    private IEnumerator PlayCounterAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        Managers.UI.FindPopup<BattleUI>().ShowCounterText(attacker.transform);
        attacker.characterAnim.PlayAttackAnimation(victim);

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    PlayNextAnimation();

                    break;
                }
            }

            yield return null;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //스킬 애니메이션

    public void EnqueueSkillAnimation(CharacterBase attacker, List<CharacterBase> victims)
    {
        Debug.Log("Enqueue Skillattack");
        animationQueue.Enqueue(() => StartSkillAnimation(attacker, victims));
    }

    public void StartSkillAnimation(CharacterBase attacker, List<CharacterBase> victims)
    {
        isAnimationPlaying = true;

        CharacterSetting(attacker, victims);

        if(victims.Count == 0)
        {
            CharacterRelease();
            return;
        }

        StartCoroutine(PlaySkillAnimation(victims));
    }

    private IEnumerator PlaySkillAnimation(List<CharacterBase> victim)
    {
        attacker.characterAnim.PlaySkillAnimation(victim);

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("skill"))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    PlayNextAnimation();

                    break;
                }
            }

            yield return null;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 방어 애니메이션

    public void EnqueuedefendAnimation(CharacterBase attacker, CharacterBase defender)
    {
        Debug.Log("Enqueue defend");
        animationQueue.Enqueue(() => StartDefendAnimation(attacker, defender));
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
                    PlayNextAnimation();

                    break;
                }
            }

            yield return null;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //이동 애니메이션

    public void EnqueueMoveAnimation(CharacterBase mover, OverlayTile targetTile)
    {
        Debug.Log("Enqueue Move");
        animationQueue.Enqueue(() => StartMoveAnimation(mover, targetTile));
    }

    public void StartMoveAnimation(CharacterBase mover, OverlayTile targetTile)
    {
        StartCoroutine(PlayMoveAnimation(mover, targetTile));
    }

    public IEnumerator PlayMoveAnimation(CharacterBase mover, OverlayTile targetTile)
    {
        CameraController.instance.SetCameraOnCharacter(mover);

        mover.characterAnim.PlayMoveAnimation();

        while (mover.transform.position != targetTile.transform.position)
        {
            mover.transform.position = Vector2.MoveTowards(mover.transform.position, targetTile.transform.position, 15 * Time.deltaTime);
            mover.characterAnim.FlipCharacter(targetTile.transform.position, false);

            yield return null;
        }

        if (mover.transform.position == targetTile.transform.position)
        {
            mover.characterAnim.EndAnimation(mover.isWalking);
            StartAnimationQueue();
        }
    }

    public void EnqueueBackAnimation(CharacterBase mover, OverlayTile targetTile)
    {
        Debug.Log("Enqueue Back");
        animationQueue.Enqueue(() => StartBackAnimation(mover, targetTile));
    }

    public void StartBackAnimation(CharacterBase mover, OverlayTile targetTile)
    {
        StartCoroutine(PlayBackAnimation(mover, targetTile));
    }

    public IEnumerator PlayBackAnimation(CharacterBase mover, OverlayTile targetTile)
    {
        CameraController.instance.SetCameraOnCharacter(mover);

        mover.characterAnim.PlayHitAnimation();

        while (mover.transform.position != targetTile.transform.position)
        {
            mover.transform.position = Vector2.MoveTowards(mover.transform.position, targetTile.transform.position, 20 * Time.deltaTime);
            mover.characterAnim.FlipCharacter(targetTile.transform.position, true);

            yield return null;
        }

        if (mover.transform.position == targetTile.transform.position)
        {
            mover.characterAnim.EndAnimation(mover.isWalking);
            StartAnimationQueue();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 애니메이션 재생 시작

    public void StartAnimationQueue()
    {
        if(animationQueue.Count == 0)
        {
            return;
        }

        Action nextAnimation = animationQueue.Dequeue();
        nextAnimation?.Invoke();
    }

    private void PlayNextAnimation()
    {
        if (animationQueue.Count > 0)
        {
            StartAnimationQueue();
        }
        else
        {
            CharacterRelease();
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
        yield return new WaitWhile(() => (animationQueue.Count > 0) || isAnimationPlaying);// 애니메이션이 재생중이 아닐 때 사망 애니메이션 재생

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
