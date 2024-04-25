using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharAnimBase : MonoBehaviour
{
    public Animator Animator { get; private set; }

    protected CharacterBase targetCharacter;
    protected List<CharacterBase> targetList;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rb;
    protected Particles particles;

    protected HealthSystem healthSystem;

    private string idleParameter = "idle";
    private string moveParameter = "move";
    private string attackParameter = "attack";
    private string skillParameter = "skill";
    private string hitParameter = "hit";
    private string dieParameter = "die";
    private string defendParameter = "defend";
    private string blockParameter = "block";

    public int Idle { get; private set; }
    public int Move { get; private set; }
    public int Attack { get; private set; }
    public int Skill { get; private set; }
    public int Hit { get; private set; }
    public int Die { get; private set; }
    public int Defend { get; private set; }
    public int Block { get; private set; }

    protected Queue<BattleKeyWords.Damage> damages;
    protected Queue<BattleKeyWords.Damage> extraDamages;

    public void Init(HealthSystem characterHealth)
    {
        Animator = GetComponent<Animator>();

        Idle = Animator.StringToHash(idleParameter);
        Move = Animator.StringToHash(moveParameter);
        Attack = Animator.StringToHash(attackParameter);
        Skill = Animator.StringToHash(skillParameter);
        Hit = Animator.StringToHash(hitParameter);
        Die = Animator.StringToHash(dieParameter);
        Defend = Animator.StringToHash(defendParameter);
        Block = Animator.StringToHash(blockParameter);

        sprite = GetComponent<SpriteRenderer>();

        rb = GetComponentInParent<Rigidbody2D>();
        particles = GetComponent<Particles>();
        particles.Init();

        damages = new Queue<BattleKeyWords.Damage>();
        extraDamages = new Queue<BattleKeyWords.Damage>();

        healthSystem = characterHealth;

        AnimationController.instance.OnCharacterReleased += OnCharacterReleased;

        LoadParticles();
    }

    protected virtual void LoadParticles()
    {

    }

    public void DeActivate()
    {
        sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    public void Activate()
    {
        sprite.color = new Color(1f, 1f, 1f, 1f);
    }

    public void SetDamage(BattleKeyWords.Damage damage)
    {
        if (damage.attackType == BattleKeyWords.AttackDamageType.Extra)
        {
            extraDamages.Enqueue(damage);
        }
        else
        {
            damages.Enqueue(damage);
        }
    }

    public virtual void ShowDamage()
    {
        if (damages.Count > 0)
        {
            BattleKeyWords.Damage damage = damages.Dequeue();
            Managers.UI.FindUI<BattleUI>().ShowDamageText(damage, transform.parent, damage.damage > 0);
        }
        ShowExtraDamage();
    }

    public void ShowExtraDamage()
    {
        if (extraDamages.Count > 0)
        {
            BattleKeyWords.Damage damage = extraDamages.Dequeue();
            Managers.UI.FindUI<BattleUI>().ShowDamageText(damage, transform.parent, damage.damage > 0);
        }
    }

    public int GetDamageFigure()
    {
        if(damages.Count > 0)
        {
            return damages.First().damage;
        }

        return 0;
    }

    public virtual void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;
        Animator.SetTrigger(Attack);
    }

    public void SetTarget(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;
    }

    public virtual void PlaySkillAnimation(List<CharacterBase> targets)
    {
        this.targetList = targets;
        Animator.SetTrigger(Skill);
    }

    public virtual void PlayBlockAnimation(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;

        targetCharacter.transform.position = new Vector3(transform.parent.position.x - 3f, transform.parent.position.y, transform.parent.position.z);

        Animator.SetTrigger(Block);
    }

    public void PlayExtraAnimation(List<CharacterBase> victims, string anim)
    {
        this.targetList = victims;
        if(victims.Count == 1)
        {
            targetCharacter = victims[0];
        }
        Animator.SetTrigger(anim);
    }

    public virtual void ShowBlock()
    {
        Managers.UI.FindUI<BattleUI>().ShowBlockText(targetCharacter.transform);
    }

    public virtual void AttackEnemy(CharacterBase targetCharacter)
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.ShowHitParticle();
    }

    public virtual void KnockBackEnemy(CharacterBase targetCharacter ,int scale)
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.GetKnockBackByLerp(GetDirectionOfCharacter(), scale, 0.25f);
    }

    public void ShowHitParticle()
    {
        particles.HitParticle();
    }

    public void PlayMoveAnimation()
    {
        Animator.SetBool(Move, true);
    }

    public void PlayHitAnimation()
    {
        if (healthSystem.GetShield() <= 0)
        {
            Animator.SetBool(Hit, true);
        }
        else
        {
            Animator.SetBool(Defend, true);
        }
    }

    public void PlayDieAnimation()
    {
        Animator.SetTrigger(Die);
    }

    public void PlayDefendAnimation()
    {
        Animator.SetBool(Defend, true);
    }

    public void ReleaseTargets()
    {
        targetCharacter = null;
        targetList = null;
    }

    public void EndAnimation(bool isWalking)
    {
        Animator.SetBool(Move, isWalking);
        Animator.SetBool(Hit, false);
        Animator.SetBool(Defend, false);
    }

    protected Vector2 Getdirection(Vector3 target)
    {
        Vector2 direction = (target - transform.parent.position).normalized;

        return direction;
    }

    public Vector2 GetDirectionOfCharacter()
    {
        if (transform.localScale.x >= 0)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }

    public void FlipCharacterDirection(Vector2 direction)
    {
        if(direction == Vector2.right)
        {
            transform.localScale = new Vector2(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if(direction == Vector2.left)
        {
            transform.localScale = new Vector2(1 * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    public void FlipCharacter(Vector2 targetPosiition, bool back)
    {
        Vector2 direction = Getdirection(targetPosiition);

        if(direction.x == Vector2.right.x)
        {
            if (back)
            {
                FlipCharacterDirection(Vector2.left);
            }
            else
            {
                FlipCharacterDirection(Vector2.right);
            }
        }
        else if(direction.x == Vector2.left.x)
        {
            if (back)
            {
                FlipCharacterDirection(Vector2.right);
            }
            else
            {
                FlipCharacterDirection(Vector2.left);
            }
        }
    }

    public void ShakeCharacter()
    {
        StartCoroutine(nameof(Shake));
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.localPosition;

        float shakeTime = 0.5f;

        while (shakeTime > 0.0f)
        {
            transform.localPosition = new Vector2(startPosition.x , startPosition.y + UnityEngine.Random.insideUnitSphere.y * 0.05f);

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = startPosition;
    }

    public void GetKnockBack(Vector2 target, int scale)
    {
        Vector2 knockBackDirection = ((Vector2)transform.parent.position - target).normalized * scale;
        Vector3 targetposition = new Vector3(transform.parent.position.x + knockBackDirection.x, transform.parent.position.y, transform.parent.position.z);

        FlipCharacter(knockBackDirection, true);

        StartCoroutine(MoveToTarget(targetposition));
    }

    public void GetKnockBackByLerp(Vector2 direction, int scale, float duration)
    {
        Vector2 knockBackDirection = direction * scale;
        Vector3 targetposition = new Vector3(transform.parent.position.x + knockBackDirection.x, transform.parent.position.y, transform.parent.position.z);

        FlipCharacterDirection(-knockBackDirection);


        StartCoroutine(MoveToTargetByLerp(targetposition, duration));
    }

    public void GetKnockBackByLerpToPosition(Vector2 target, int scale, float duration)
    {
        Vector2 knockBackDirection = ((Vector2)transform.parent.position - target).normalized * scale;
        Vector3 targetposition = new Vector3(transform.parent.position.x + knockBackDirection.x, transform.parent.position.y, transform.parent.position.z);

        FlipCharacter(knockBackDirection, true);

        StartCoroutine(MoveToTargetByLerp(targetposition, duration));
    }

    protected IEnumerator MoveToTargetByLerp(Vector3 targetPosition, float duration)
    {
        float time = 0;
        while (time <= duration)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, targetPosition, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        transform.parent.position = targetPosition;
    }

    protected IEnumerator MoveToTarget(Vector3 targetPosition, float speed = 100f)
    {

        while(transform.parent.position != targetPosition)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPosition, speed * Time.deltaTime);

            yield return null;
        }
    }

    protected virtual void OnCharacterReleased()
    {
        CameraController.instance.ResetBattleGroup();
        CameraController.instance.SetMinOrtho(9);
    }

    public void Damage()
    {
        targetCharacter.characterAnim.ShowDamage();
    }

    public void DamageAll()
    {
        foreach (CharacterBase targets in targetList)
        {
            targets.characterAnim.ShowDamage();
        }
    }

    public void SetRangePosition(CharacterBase target, Vector2 direction, float distance)
    {
        target.transform.position = new Vector3(target.transform.position.x + (direction.x * distance), target.transform.position.y, target.transform.position.z);
    }
}
