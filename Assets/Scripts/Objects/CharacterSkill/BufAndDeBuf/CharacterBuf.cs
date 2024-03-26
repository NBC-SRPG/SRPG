using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf
{
    public bool IsDestroyed {  get; private set; }

    protected CharacterBase character;// 버프 걸린 캐릭터

    public virtual BattleKeyWords.BufKeyword BufKeyword { get; protected set; }
    public virtual BattleKeyWords.BufType BufType { get; protected set; }

    public virtual string Keyword {  get; protected set; }

    public int stack;

    public CharacterBase Buffer { get; protected set; }// 버프 건 캐릭터

    public virtual void Init(CharacterBase character, CharacterBase buffer)
    {
        this.character = character;
        stack = 0;
        IsDestroyed = false;
        Buffer = buffer;
    }

    public virtual void OnAddBuf()
    {

    }

    protected void DestoyBuf()
    {
        stack = 0;
        IsDestroyed = true;
    }

    public virtual void OnDestroy()
    {

    }

    public virtual void OnRoundStart()
    {

    }

    public virtual void OnRoundEnd()
    {

    }

    public virtual void OnTurnStart()
    {

    }

    public virtual void OnTurnEnd()
    {

    }

    public virtual void OnEndActing()
    {

    }

    public virtual void OnPassAlly(CharacterBase allyCharacter)// 아군 위를 지나갔을 때 발동
    {

    }

    public virtual void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {

    }

    public virtual void OnPassEnemy(CharacterBase enemtCharacter)// 적군 위를 지나갔을 때 발동
    {

    }

    public virtual void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {

    }

    public virtual void OnStartAttack(CharacterBase character)
    {

    }

    public virtual void OnAttackSuccess(CharacterBase character, int damage = 0)
    {

    }

    public virtual void OnEndAttack(CharacterBase character)
    {

    }

    public virtual void OnTakeDamage(CharacterBase character)
    {

    }

    public virtual void OnUseSkill()
    {

    }

    public virtual void OnSkillSuccess()
    {

    }


    public virtual void OnDie()
    {

    }
}
