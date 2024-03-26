using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimBase : MonoBehaviour
{
    public Animator Animator { get; private set; }

    protected CharacterBase character;
    protected CharacterBase targetCharacter;
    protected List<CharacterBase> targetList;

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
        this.character = thisCharacter;

        Animator = GetComponent<Animator>();

        Idle = Animator.StringToHash(idleParameter);
        Move = Animator.StringToHash(moveParameter);
        Attack = Animator.StringToHash(attackParameter);
        Skill = Animator.StringToHash(skillParameter);
        Hit = Animator.StringToHash(hitParameter);
        Die = Animator.StringToHash(dieParameter);
        defend = Animator.StringToHash(defendParameter);

        Debug.Log("animator init");
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
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

        targetCharacter.transform.position = new Vector3(character.transform.position.x - 3f, character.transform.position.y, character.transform.position.z);

        Animator.SetTrigger(defend);
    }

    public virtual void HitEnemy(int scale)
    {
        targetCharacter.characterAnim.PlayHitAnimation();
        targetCharacter.characterAnim.GetKnockBack(transform.position, scale);
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

    public void EndAnimation()
    {
        Animator.SetBool(Move, character.isWalking);
        Animator.SetBool(Hit, false);
    }

    protected Vector2 Getdirection(Vector3 target)
    {
        Vector2 direction = (target - character.transform.position).normalized;

        return direction;
    }

    public void FlipCharacter(Vector2 targetPosiition, bool back)
    {
        Vector2 direction = Getdirection(targetPosiition);

        if(direction.x == Vector2.right.x)
        {
            if (back)
            {
                transform.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else if(direction.x == Vector2.left.x)
        {
            if (back)
            {
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    public void GetKnockBack(Vector2 target, int scale)
    {
        Vector2 knockBackDirection = ((Vector2)character.transform.position - target).normalized * scale;
        Vector3 targetposition = new Vector3(character.transform.position.x + knockBackDirection.x, character.transform.position.y, character.transform.position.z);

        FlipCharacter(knockBackDirection, true);

        StartCoroutine(MoveToTarget(targetposition));
    }

    protected IEnumerator MoveToTarget(Vector3 targetPosition)
    {

        while(character.transform.position != targetPosition)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition, 80f * Time.deltaTime);

            yield return null;
        }
    }
}
