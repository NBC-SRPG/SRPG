using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimBase : MonoBehaviour
{
    public Animator Animator { get; private set; }

    protected CharacterBase targetCharacter;
    protected List<CharacterBase> targetList;
    protected SpriteRenderer sprite;
    private Color color;
    protected Rigidbody2D rb;
    protected Particles particles;

    private string idleParameter = "idle";
    private string moveParameter = "move";
    private string attackParameter = "attack";
    private string skillParameter = "skill";
    private string hitParameter = "hit";
    private string dieParameter = "die";
    private string defendParameter = "defend";

    public int Idle { get; private set; }
    public int Move { get; private set; }
    public int Attack { get; private set; }
    public int Skill { get; private set; }
    public int Hit { get; private set; }
    public int Die { get; private set; }
    public int defend { get; private set; }

    protected int damage;

    public void Init(CharacterBase thisCharacter)
    {
        Animator = GetComponent<Animator>();

        Idle = Animator.StringToHash(idleParameter);
        Move = Animator.StringToHash(moveParameter);
        Attack = Animator.StringToHash(attackParameter);
        Skill = Animator.StringToHash(skillParameter);
        Hit = Animator.StringToHash(hitParameter);
        Die = Animator.StringToHash(dieParameter);
        defend = Animator.StringToHash(defendParameter);

        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;

        rb = GetComponentInParent<Rigidbody2D>();
        particles = GetComponent<Particles>();
    }

    public void DeActivate()
    {
        sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    public void Activate()
    {
        sprite.color = color;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public virtual void ShowDamage()
    {
        //targetCharacter.health.TakeDamageHealthBar(targetCharacter.characterAnim.damage);
    }

    public virtual void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;
        Animator.SetTrigger(Attack);
    }


    public virtual void PlaySkillAnimation(List<CharacterBase> targets)
    {
        this.targetList = targets;
        Animator.SetTrigger(Skill);
    }

    public virtual void PlayDefendAnimation(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;

        targetCharacter.transform.position = new Vector3(transform.parent.position.x - 3f, transform.parent.position.y, transform.parent.position.z);

        Animator.SetTrigger(defend);
    }

    public void PlayExtraAnimation(CharacterBase targetCharacter, string anim)
    {
        this.targetCharacter = targetCharacter;
        Animator.SetTrigger(anim);
    }

    public virtual void ShowDefend()
    {
        Managers.UI.FindPopup<BattleUI>().ShowDefendText(targetCharacter.transform);
    }

    public virtual void AttackEnemy()
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.ShowHitParticle();
    }

    public virtual void KnockBackEnemy(int scale)
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.GetKnockBack(transform.parent.position, scale);
    }

    public void ShowHitParticle()
    {
        particles.PlayHitParticle();
    }

    public void PlayMoveAnimation()
    {
        Animator.SetBool(Move, true);
    }

    public void PlayHitAnimation()
    {
        Animator.SetBool(Hit, true);
    }

    public void PlayDieAnimation()
    {
        Animator.SetTrigger(Die);
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
    }

    protected Vector2 Getdirection(Vector3 target)
    {
        Vector2 direction = (target - transform.parent.position).normalized;

        return direction;
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

        //StartCoroutine(KnockBack(knockBackDirection));
        StartCoroutine(MoveToTarget(targetposition));
    }

    protected IEnumerator KnockBack(Vector2 direction)
    {
        float timeSet = 0;
        while (true)
        {
            rb.AddRelativeForce(direction / 2, ForceMode2D.Impulse);
            timeSet += Time.deltaTime;
            if (timeSet > 0.25f)
            {
                Debug.Log("111");
                rb.velocity = Vector2.zero;
                break;
            }
            yield return null;
        }
    }

    protected IEnumerator MoveToTarget(Vector3 targetPosition)
    {

        while(transform.parent.position != targetPosition)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPosition, 100f * Time.deltaTime);

            yield return null;
        }
    }
}
