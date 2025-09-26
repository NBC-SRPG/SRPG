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

    public string Description { get; protected set; }
    public string BufName { get; protected set; }

    public int duration;//지속 턴 수 (또는 적용 횟수)
    public int power;//위력 (또는 버프 수치)
    public int stack;//중첩 횟수
    public int maxStack; //최대 중첩 가능 횟수

    protected int turnCnt = 0;// 내 다음 턴 까지 지속되야 할 때

    public CharacterBase Buffer { get; protected set; }// 버프 건 캐릭터

    public bool dontDestroy = false;// 외부 효과로 파괴되지 않는 경우
    public bool isPermanent = false;// 영구 지속 효과라면
    public bool isIndependent = false; // 지속시간이 갱신되지 않으며 따로 돌아야하는 독립 버프의 경우 (예시: 치명타 확률 증가 등) (이 속성은 power가 각 개체마다 달라지는 타입의 버프라면 모두 true로 적용되야함.)
    public bool onlyOne = false;// 한 번에 한 개만 존재해야 하는 버프라면 (예시: 치유 감소)

    public virtual void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        this.character = character;
        duration = 0;
        power = 0;
        stack = 0;
        maxStack = 99;
        IsDestroyed = false;
        Buffer = buffer;
    }

    public virtual BonusStat GetAdditionalStat()
    {
        return null;
    }

    public virtual BonusStat GetDecreaseStat()
    {
        return null;
    }

    public virtual void DecreaseDuration(int n) //지속 턴 감소
    {
        if(n < 1)
        {
            n = 1;
        }

        duration -= n;

        if (duration == 0)
        {
            DestoyBuf();
        }
    }

    public virtual void OnAddBuf()
    {

    }

    public void DestoyBuf()
    {
        stack = 0;
        duration = 0;
        IsDestroyed = true;

        OnDestroy();
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

    public virtual void AfterTakeAttacked(CharacterBase enemy)// 공격 받은 이후에
    {

    }

    public virtual void OnTakeDamage(ref int damage, CharacterBase enemy = null, 
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None, 
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {

    }

    public virtual void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {

    }

    public virtual void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {

    }

    public virtual void AfterTakeHeal(int heal, CharacterBase skillUser = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐 받은 이후에
    {

    }

    public virtual void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public virtual void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {

    }

    public virtual void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public virtual void OnDie()
    {

    }

    public virtual void OnUpdate()// 실시간 판정
    {

    }

    public virtual string GetName()
    {
        return BufName;
    }

    public virtual string GetDescription()
    {
        return Description;
    }
}
