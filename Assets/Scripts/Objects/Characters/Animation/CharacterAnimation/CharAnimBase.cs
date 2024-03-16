using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimBase : MonoBehaviour
{
    public Animator Animator { get; private set; }

    protected CharacterBase character;
    protected CharacterBase targetCharacter;

    private string idleParameter = "idle";
    private string moveParameter = "move";
    private string attackParameter = "attack";
    private string hitParameter = "hit";
    private string defendParameter = "defend";

    public int Idle { get; private set; }
    public int Move { get; private set; }
    public int Attack { get; private set; }
    public int Hit { get; private set; }
    public int defend { get; private set; }

    public void Init(CharacterBase thisCharacter)
    {
        this.character = thisCharacter;

        Animator = GetComponent<Animator>();

        Idle = Animator.StringToHash(idleParameter);
        Move = Animator.StringToHash(moveParameter);
        Attack = Animator.StringToHash(attackParameter);
        Hit = Animator.StringToHash(hitParameter);
        defend = Animator.StringToHash(defendParameter);

        Debug.Log("animator init");
    }

    public virtual void PlayAttackAnimation(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;
        Animator.SetTrigger(Attack);
    }

    public virtual void PlayDefendAnimation(CharacterBase targetCharacter)
    {
        this.targetCharacter = targetCharacter;
        Animator.SetTrigger(defend);
    }

    public void PlayMoveAnimation()
    {
        Animator.SetBool(Move, character.isWalking);
    }

    public void PlayHitAnimation()
    {
        Animator.SetBool(Hit, true);
    }

    public void EndAnimation()
    {
        Animator.SetBool(Hit, false);
    }

    protected Vector2 Getdirection(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)character.transform.position).normalized;

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
        Vector2 targetposition = new Vector2(character.transform.position.x + knockBackDirection.x, character.transform.position.y);

        StartCoroutine(MoveToTarget(targetposition));
    }

    protected IEnumerator MoveToTarget(Vector2 targetPosition)
    {

        while((Vector2)character.transform.position != targetPosition)
        {
            character.transform.position = Vector2.MoveTowards(character.transform.position, targetPosition, 80f * Time.deltaTime);

            yield return null;
        }
    }
}
