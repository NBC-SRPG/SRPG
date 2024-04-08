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

    public int stack;// 스택
    public int power;// 위력

    protected int turnCnt = 0;// 내 다음 턴 까지 지속되야 할 때

    public CharacterBase Buffer { get; protected set; }// 버프 건 캐릭터

    public bool dontDestroy = false;// 외부 효과로 파괴되지 않는 경우
    public bool isPermanent = false;// 영구 지속 효과라면
    public bool cantStack = false;// 중첩 불가 버프라면(같은 버프를 받을 때, 효과가 갱신됨)

    public virtual void Init(CharacterBase character, CharacterBase buffer)
    {
        this.character = character;
        stack = 0;
        power = 0;
        IsDestroyed = false;
        Buffer = buffer;
    }

    public virtual BonusStat GetAdditionalStat()
    {
        return null;
    }

    public virtual void DecreaseStack(int n)
    {
        if(n < 1)
        {
            n = 1;
        }

        stack -= n;

        if (stack == 0)
        {
            DestoyBuf();
        }
    }

    public virtual void OnAddBuf()
    {

    }

    public void DestoyBuf()
    {
        OnDestroy();

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

    public virtual void OnAttackSuccess(CharacterBase character, BattleKeyWords.Damage damage = new BattleKeyWords.Damage())
    {

    }

    public virtual void OnEndAttack(CharacterBase character)
    {

    }

    public virtual void OnTakeAttack(CharacterBase enemy)// 공격 받기 이전에
    {

    }

    public virtual void OnTakeDamage(ref int damage, CharacterBase enemy = null, 
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None, 
        Constants.CharacterAttribute characterAttribute = Constants.CharacterAttribute.None)// 데미지를 입을 때
    {

    }

    public virtual void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.CharacterAttribute characterAttribute = Constants.CharacterAttribute.None)// 힐을 받을 때
    {

    }

    public virtual void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.CharacterAttribute characterAttribute = Constants.CharacterAttribute.None)// 데미지를 받은 이후에
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

    public virtual void OnUpdate()// 실시간 판정
    {

    }
}
