using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    private Queue<Action> animationQueue = new Queue<Action>();
    private Queue<Action> animationAtRelease = new Queue<Action>();

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
    private List<CharacterBase> attackTargets;

    private Dictionary<Action, List<Action>> stitchedAnim = new Dictionary<Action, List<Action>>();
    private Dictionary<CharacterBase, Vector3> originPos = new Dictionary<CharacterBase, Vector3>();

    private bool isAnimationPlaying;
    private bool isWalkPlaying;
    private bool isSetting;

    public event Action onAnimationEnd;

    private Action prevAnimation;

    private WaitForSeconds delay = new WaitForSeconds(0.15f);

    //-----------------------------------------------------------------------------------------------------------------------
    //캐릭터 위치 조정

    private void SetCharacterLayer(Transform character, int layerNum)
    {
        character.gameObject.layer = layerNum;

        foreach(Transform child in character.GetComponentInChildren<Transform>())
        {
            //child.gameObject.layer = layerNum;
            SetCharacterLayer(child, layerNum);
        }
    }

    private void CharacterSetting(CharacterBase attacker, List<CharacterBase> victims)// 캐릭터 위치 지정
    {
        isSetting = true;

        CameraController.instance.SetCharacterCameraMove(2);


        backGround.gameObject.SetActive(true);
        battleCanvas.gameObject.SetActive(true);

        this.attacker = attacker;
        this.attackTargets = victims.ConvertAll(data => data);

        originPos.Add(attacker, attacker.transform.position);

        attacker.transform.position = attackerPosition.transform.position;
        attacker.transform.localScale = new Vector3(4, 4, 0);

        SetCharacterLayer(attacker.transform, 31);

        attacker.characterAnim.Activate();
        attacker.characterAnim.FlipCharacterDirection(Vector2.right);

        attacker.health.healthBarCanvas.SetActive(false);

        group.AddMember(attacker.transform, 1, 2);

        for (int i = 0; i < this.attackTargets.Count; i++)
        {
            if (this.attackTargets[i] == attacker)
            {
                continue;
            }

            originPos.Add(this.attackTargets[i], this.attackTargets[i].transform.position);

            this.attackTargets[i].transform.position = new Vector3(victimPosition.transform.position.x + (i * 5), victimPosition.transform.position.y, victimPosition.transform.position.z);
            this.attackTargets[i].transform.localScale = new Vector3(4, 4, 0);

            SetCharacterLayer(this.attackTargets[i].transform, 31);

            this.attackTargets[i].characterAnim.Activate();
            this.attackTargets[i].characterAnim.FlipCharacterDirection(Vector2.left);

            this.attackTargets[i].health.healthBarCanvas.SetActive(false);

            group.AddMember(this.attackTargets[i].transform, 1, 2);
        }
    }

    private void CharacterRelease() // 캐릭터 제자리로
    {
        isSetting = false;

        CameraController.instance.SetCharacterCameraMove(0);

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

        attacker.transform.position = originPos[attacker];
        attacker.transform.localScale = new Vector3(1, 1, 0);

        SetCharacterLayer(attacker.transform, 0);

        attacker.characterAnim.ReleaseTargets();
        attacker.characterAnim.EndAnimation(attacker.isWalking);
        //attacker.characterAnim.SetDamage(0);

        attacker.health.healthBarCanvas.SetActive(true);

        if(attackTargets.Count == 0)
        {
            return;
        }

        for (int i = 0; i < attackTargets.Count; i++)
        {
            attackTargets[i].transform.position = originPos[attackTargets[i]];
            attackTargets[i].transform.localScale = new Vector3(1, 1, 0);

            SetCharacterLayer(attackTargets[i].transform, 0);

            attackTargets[i].characterAnim.EndAnimation(attackTargets[i].isWalking);
            attackTargets[i].characterAnim.FlipCharacter(attacker.transform.position, false);
            //victims[i].characterAnim.SetDamage(0);

            attackTargets[i].health.healthBarCanvas.SetActive(true);
        }

        attacker.characterAnim.FlipCharacter(attackTargets[0].transform.position, false);
        attacker = null;
        attackTargets.Clear();
        originPos.Clear();

        while (animationAtRelease.Count > 0)
        {
            animationAtRelease.Dequeue()?.Invoke();
        }
    }

    public void EndAimation()
    {
        isAnimationPlaying = false;
        isWalkPlaying = false;
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
        CharacterRelease();
        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { victim };
        
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
                    yield return delay;

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
        Managers.UI.FindUI<BattleUI>().ShowCounterText(attacker.transform);
        attacker.characterAnim.PlayExtraAnimation(new List<CharacterBase> { victim }, "counter_attack");

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    yield return delay;

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
        List<CharacterBase> v = new List<CharacterBase>();

        v.AddRange(victims.ConvertAll(data => data));

        Debug.Log("Enqueue Skillattack");
        animationQueue.Enqueue(() => StartSkillAnimation(attacker, v));
    }

    public void StartSkillAnimation(CharacterBase attacker, List<CharacterBase> victims)// 스킬 애니메이션 재생
    {
        isAnimationPlaying = true;

        CharacterRelease();

        if (victims.Count == 0)
        {
            Debug.Log("no target");
            PlayNextAnimation();
            return;
        }

        CharacterSetting(attacker, victims);

        StartCoroutine(PlaySkillAnimation(victims));
    }

    private IEnumerator PlaySkillAnimation(List<CharacterBase> victims)// 스킬 애니메이션
    {
        attacker.characterAnim.PlaySkillAnimation(victims);

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsTag("skill"))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    yield return delay;

                    PlayNextAnimation();

                    break;
                }
            }

            yield return null;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 방어 애니메이션

    public void EnqueueblockAnimation(CharacterBase attacker, CharacterBase defender)
    {
        Debug.Log("Enqueue defend");
        animationQueue.Enqueue(() => StartBlockAnimation(attacker, defender));
    }

    public void StartBlockAnimation(CharacterBase attacker, CharacterBase defender)// 방어 애니메이션 재생
    {
        CharacterRelease();

        isAnimationPlaying = true;

        List<CharacterBase> victims = new List<CharacterBase>() { defender };

        CharacterSetting(attacker, victims);

        StartCoroutine(PlayBlockAnimation(defender));
    }

    private IEnumerator PlayBlockAnimation(CharacterBase defender)// 방어 애니메이션
    {
        defender.characterAnim.PlayBlockAnimation(attacker);

        while (true)
        {
            if (defender.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName("block"))
            {
                float animTime = defender.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    yield return delay;

                    PlayNextAnimation();

                    break;
                }
            }

            yield return null;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //기타 애니메이션

    public void EnqueueExtraAnimation(CharacterBase attacker, List<CharacterBase> victims, string anim, bool needSetting = false)
    {
        Debug.Log("Enqueue Extra");
        animationQueue.Enqueue(() => StartExtraAnimation(attacker, victims, anim, needSetting));
    }

    private void StartExtraAnimation(CharacterBase attacker, List<CharacterBase> victims, string anim, bool needSetting = false)// 기타 애니메이션 재생
    {
        if (needSetting)
        {
            CharacterRelease();
        }

        isAnimationPlaying = true;

        if (needSetting)
        {
            CharacterSetting(attacker, victims);
        }

        StartCoroutine(PlayExtraAnimation(attacker, victims, anim));
    }

    private IEnumerator PlayExtraAnimation(CharacterBase attacker, List<CharacterBase> victims, string anim)// 기타 애니메이션
    {
        attacker.characterAnim.PlayExtraAnimation(victims, anim);

        while (true)
        {
            if (attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            {
                float animTime = attacker.characterAnim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime > 0.9f)
                {
                    yield return delay;

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
        isWalkPlaying = true;

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
            isWalkPlaying = false;
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
        isWalkPlaying = true;

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
            isWalkPlaying = false;
            PlayNextAnimation();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 애니메이션 재생 시작

    public void StartAnimationQueue()// 애니메이션 큐 재생 시작
    {
        if(animationQueue.Count == 0)
        {
            EndAimation();
            return;
        }

        if(isAnimationPlaying || isWalkPlaying)
        {
            return;
        }

        prevAnimation = animationQueue.Dequeue();
        prevAnimation?.Invoke();
    }

    private void PlayNextAnimation()// 다음 애니메이션 재생
    {
        Action animation = null;

        if (stitchedAnim.ContainsKey(prevAnimation))
        {
            if (isSetting)
            {
                foreach (Action action in stitchedAnim[prevAnimation])
                {
                    animationAtRelease.Enqueue(action);
                }
            }
            else
            {
                foreach (Action action in stitchedAnim[prevAnimation])
                {
                    animation += action;
                }
            }

            stitchedAnim.Remove(prevAnimation);
        }

        if (animationQueue.Count > 0)
        {
            prevAnimation = animationQueue.Dequeue();
            prevAnimation?.Invoke();
            animation?.Invoke();
        }
        else
        {
            CharacterRelease();
            animation?.Invoke();
            EndAimation();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 사망 애니메이션

    public void EnqueueDieAnimation(CharacterBase character)
    {
        Debug.Log("Enqueue Die");
        animationQueue.Enqueue(() => StartDieAnimation(character));
    }

    public void StartDieAnimation(CharacterBase character)// 사망 애니메이션 재생
    {
        StartCoroutine(PlayDieAnimation(character));
    }

    public IEnumerator PlayDieAnimation(CharacterBase character)
    {
        //PlayNextAnimation();

        //yield return new WaitWhile(() => (animationQueue.Count > 0) || isAnimationPlaying);// 애니메이션이 재생중이 아닐 때 사망 애니메이션 재생
        yield return new WaitWhile(() => character.hasAnimationBeforDIe);// 사망시 재생되는 애니메이션이 있을 시, 대기

        character.characterAnim.Activate();

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

    //-----------------------------------------------------------------------------------------------------------------------
    // 기타 함수들

    public bool CheckSetting()
    {
        return isSetting;
    }

    public bool CheckAnimation()
    {
        if(animationQueue.Count > 0 || isAnimationPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWalkingAnimation()
    {
        if (isWalkPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StitchAnimation(Action action)
    {
        Debug.Log("Stitched");

        if (stitchedAnim.ContainsKey(animationQueue.Last()))
        {
            stitchedAnim[animationQueue.Last()].Add(action);
        }
        else
        {
            stitchedAnim.Add(animationQueue.Last(), new List<Action>() { action });
        }
    }

    public void StitchAnimationAtFirst(Action action)
    {
        if (stitchedAnim.ContainsKey(animationQueue.First()))
        {
            stitchedAnim[animationQueue.First()].Add(action);
        }
        else
        {
            stitchedAnim.Add(animationQueue.First(), new List<Action>() { action });
        }
    }
}
