using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool isAnimationPlaying;

    public event Action onAnimationEnd;

    //-----------------------------------------------------------------------------------------------------------------------
    //캐릭터 위치 조정

    private void SetCharacterLayer(CharacterBase character, int layerNum)
    {
        foreach(Transform child in character.GetComponentInChildren<Transform>())
        {
            child.gameObject.layer = layerNum;
        }
    }

    private void CharacterSetting(CharacterBase attacker, List<CharacterBase> victims)// 캐릭터 위치 지정
    {
        backGround.gameObject.SetActive(true);
        battleCanvas.gameObject.SetActive(true);

        this.attacker = attacker;
        this.victims = victims;

        attacker.transform.position = attackerPosition.transform.position;
        attacker.transform.localScale = new Vector3(4, 4, 0);

        SetCharacterLayer(attacker, 31);

        attacker.characterAnim.Activate();
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

            SetCharacterLayer(victims[i], 31);

            victims[i].characterAnim.Activate();
            victims[i].characterAnim.FlipCharacterDirection(Vector2.left);


            group.AddMember(victims[i].transform, 1, 5);
        }

        CameraController.instance.SetCharacterCameraMove(1);
    }

    private void CharacterRelease() // 캐릭터 제자리로
    {
        backGround.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(false);

        if(group.m_Targets.Length > 0)
        {
            foreach(var target in group.m_Targets)
            {
                group.RemoveMember(target.target);
            }
        }

        if(attacker == null)
        {
            return;
        }

        attacker.transform.position = attacker.curStandingTile.transform.position;
        attacker.transform.localScale = new Vector3(1, 1, 0);

        SetCharacterLayer(attacker, 0);

        attacker.health.SetHealthBar();
        attacker.characterAnim.ReleaseTargets();
        attacker.characterAnim.EndAnimation(attacker.isWalking);
        attacker.characterAnim.SetDamage(0);
        attacker = null;

        if(victims.Count == 0)
        {
            return;
        }

        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].transform.position = victims[i].curStandingTile.transform.position;
            victims[i].transform.localScale = new Vector3(1, 1, 0);

            SetCharacterLayer(victims[i], 0);

            victims[i].characterAnim.EndAnimation(victims[i].isWalking);
            victims[i].characterAnim.SetDamage(0);

            victims[i].health.SetHealthBar();
        }
        victims.Clear();

        CameraController.instance.SetCharacterCameraMove(0);
    }

    public void EndAimation()
    {
        isAnimationPlaying = false;
        onAnimationEnd?.Invoke();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //공격 애니메이션

    public void EnqueueAttackAnimation(CharacterBase attacker, CharacterBase victim)
    {
        Debug.Log("Enqueue attack");
        animationQueue.Enqueue(() => StartAttackAnimation(attacker, victim));
    }

    public void StartAttackAnimation(CharacterBase attacker, CharacterBase victim)// 공격 애니메이션 재생
    {
        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { victim };

        CharacterRelease();
        CharacterSetting(attacker, victims);

        StartCoroutine(PlayAttackAnimation(attacker, victim));
    }

    private IEnumerator PlayAttackAnimation(CharacterBase attacker, CharacterBase victim)// 공격 애니메이션
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

    private void StartCounterAnimation(CharacterBase attacker, CharacterBase victim)// 반격 애니메이션 재생
    {
        isAnimationPlaying = true;

        StartCoroutine(PlayCounterAttackAnimation(attacker, victim));
    }

    private IEnumerator PlayCounterAttackAnimation(CharacterBase attacker, CharacterBase victim)// 반격 애니메이션
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

    public void StartSkillAnimation(CharacterBase attacker, List<CharacterBase> victims)// 스킬 애니메이션 재생
    {
        isAnimationPlaying = true;

        CharacterRelease();
        CharacterSetting(attacker, victims);

        if(victims.Count == 0)
        {
            CharacterRelease();
            return;
        }

        StartCoroutine(PlaySkillAnimation(victims));
    }

    private IEnumerator PlaySkillAnimation(List<CharacterBase> victim)// 스킬 애니메이션
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

    public void StartDefendAnimation(CharacterBase attacker, CharacterBase defender)// 방어 애니메이션 재생
    {
        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { defender };

        CharacterRelease();
        CharacterSetting(attacker, victims);

        StartCoroutine(PlayDefendAnimation(defender));
    }

    private IEnumerator PlayDefendAnimation(CharacterBase defender)// 방어 애니메이션
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

    public void EnqueueMoveAnimation(CharacterBase mover, OverlayTile prevTile, OverlayTile targetTile)
    {
        Debug.Log("Enqueue Move");
        animationQueue.Enqueue(() => StartMoveAnimation(mover, prevTile, targetTile));
    }

    public void StartMoveAnimation(CharacterBase mover, OverlayTile prevTile, OverlayTile targetTile)// 이동 애니메이션 재생
    {
        isAnimationPlaying = true;

        StartCoroutine(PlayMoveAnimation(mover, prevTile, targetTile));
    }

    public IEnumerator PlayMoveAnimation(CharacterBase mover, OverlayTile prevTile, OverlayTile targetTile)// 이동 애니메이션
    {
        CharacterRelease();

        CameraController.instance.SetCameraOnCharacter(mover);

        mover.characterAnim.PlayMoveAnimation();

        mover.transform.position = prevTile.transform.position;
        while (mover.transform.position != targetTile.transform.position)
        {
            mover.transform.position = Vector2.MoveTowards(mover.transform.position, targetTile.transform.position, 15 * Time.deltaTime);
            mover.characterAnim.FlipCharacter(targetTile.transform.position, false);

            yield return null;
        }

        if (mover.transform.position == targetTile.transform.position)
        {
            mover.characterAnim.EndAnimation(mover.isWalking);
            PlayNextAnimation();
        }
    }

    public void EnqueueBackAnimation(CharacterBase mover, OverlayTile prevTile, OverlayTile targetTile)
    {
        Debug.Log("Enqueue Back");
        animationQueue.Enqueue(() => StartBackAnimation(mover, prevTile, targetTile));
    }

    public void StartBackAnimation(CharacterBase mover, OverlayTile prevTile, OverlayTile targetTile)// 튕겨 나가는 애니메이션 재생
    {
        isAnimationPlaying = true;

        StartCoroutine(PlayBackAnimation(mover, prevTile, targetTile));
    }

    public IEnumerator PlayBackAnimation(CharacterBase mover, OverlayTile prevTile, OverlayTile targetTile)// 튕겨 나가는 애니메이션
    {
        CharacterRelease();

        CameraController.instance.SetCameraOnCharacter(mover);

        mover.characterAnim.PlayHitAnimation();

        mover.transform.position = prevTile.transform.position;
        while (mover.transform.position != targetTile.transform.position)
        {
            mover.transform.position = Vector2.MoveTowards(mover.transform.position, targetTile.transform.position, 50 * Time.deltaTime);
            mover.characterAnim.FlipCharacter(targetTile.transform.position, true);

            yield return null;
        }

        if (mover.transform.position == targetTile.transform.position)
        {
            mover.characterAnim.EndAnimation(mover.isWalking);
            PlayNextAnimation();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 애니메이션 재생 시작

    public void StartAnimationQueue()// 애니메이션 큐 재생 시작
    {
        if(animationQueue.Count == 0 || isAnimationPlaying)
        {
            return;
        }

        Debug.Log("startAnimation");

        animationQueue.Dequeue()?.Invoke();
    }

    private void PlayNextAnimation()// 다음 애니메이션 재생
    {
        if (animationQueue.Count > 0)
        {
            animationQueue.Dequeue()?.Invoke();
        }
        else
        {
            CharacterRelease();
            EndAimation();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 사망 애니메이션

    public void StartDieAnimation(CharacterBase character)// 사망 애니메이션 재생
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
