using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static BattleKeyWords;

public class CharacterBufList
{
    private CharacterBase character;

    public List<CharacterBuf> bufList;
    private List<CharacterBuf> removeList;

    private List<CharacterBuf> bufs;

    public bool debufimmunity;

    public CharacterBufList(CharacterBase character)
    {
        this.character = character;
        bufList = new List<CharacterBuf>();
        removeList = new List<CharacterBuf>();

        bufs = new List<CharacterBuf>();
    }

    //-------------------------------------------------------------------------------------------------------------------
    // 버프 컨트롤

    public void AddBuf(BattleKeyWords.BufKeyword key, int duration,  CharacterBase buffer = null, int power = 0, int stack = 1)
    {

        CharacterBuf buf;




        // 리스트에 버프가 있는지 확인
        if(buffer == null)
        {
            buffer = character;
        }

        buf = bufList.Find(x => x.BufKeyword == key && x.Buffer == buffer && !x.IsDestroyed);

        if (buf == null || (buf != null && buf.onlyOne) || (buf != null && buf.isIndependent))// 버프가 없거나, 한번에 하나만 존재해야 하거나, 개별 적용되는 버프인 경우
        {
            switch (key)
            {
                case BattleKeyWords.BufKeyword.Burn:
                    buf = new CharacterBuf_Burn();
                    break;
                case BattleKeyWords.BufKeyword.Bleed:
                    buf = new CharacterBuf_Bleed();
                    break;
                case BattleKeyWords.BufKeyword.Bind:
                    buf = new CharacterBuf_Bind();
                    break;
                case BattleKeyWords.BufKeyword.Stun:
                    buf = new CharacterBuf_Stun();
                    break;

                case BattleKeyWords.BufKeyword.AtkAura:
                    buf = new CharacterBuf_AtkAura();
                    break;
                case BattleKeyWords.BufKeyword.Herald:
                    buf = new CharacterBuf_Herald();
                    break;
                case BattleKeyWords.BufKeyword.ReceivedDgmReduce:
                    buf = new CharacterBuf_ReceivedDgmReduce();
                    break;
                case BattleKeyWords.BufKeyword.ReceivedDgmIncrease:
                    buf = new CharacterBuf_ReceivedDgmIncrease();
                    break;
                case BattleKeyWords.BufKeyword.CrtRateIncrease:
                    buf = new CharacterBuf_CrtRateIncrease();
                    break;
                case BattleKeyWords.BufKeyword.HealReversal:
                    buf = new CharacterBuf_HealReversal();
                    break;
                case BattleKeyWords.BufKeyword.Corrosion:
                    buf = new CharacterBuf_Corrosion();
                    break;
                case BattleKeyWords.BufKeyword.TargetMarker:
                    buf = new CharacterBuf_TargetMarker();
                    break;
                case BattleKeyWords.BufKeyword.AmethystShield:
                    buf = new CharacterBuf_AmethystShield();
                    break;
                case BattleKeyWords.BufKeyword.DefReduce:
                    buf = new CharacterBuf_DefReduce();
                    break;

            }

            if(buf.BufType == BufType.Negative && debufimmunity == true) //디버프 면역 상태일 경우(아메의 자수정 방패가 있고 3-1 특성 적용중일 경우)
            {
                return;
            }


            if (buf != null)// 버프 생성 이후 리스트에 삽입
            {
                buf.Init(character, buffer, duration, power, stack);

                if (buf.onlyOne)//한 캐릭터의 버프 리스트에서 한 번에 하나만 존재해야 하는 버프의 경우
                {
                   
                    CharacterBuf originBuf = FindBuf(key);// 기존의 존재하던 버프를 찾아서

                    if (originBuf != null)
                    {
                        if (originBuf.duration <= duration)// 지속 턴이 더 긴 쪽으로 적용함.
                        {
                            originBuf.DestoyBuf();
                        }
                        else
                        {
                            buf.DestoyBuf();
                        }
                    }
                
                }
                else if (buf.isIndependent) // 독립형 버프인 경우
                {
                    //그냥 새로 버프를 생성-리스트 추가. 별도 행동 안함.
                }


                bufList.Add(buf);
                buf.OnAddBuf();
            }
        }

        if (buf != null)//리스트에 동일한 키워드의 버프가 이미 존재하고, 단일형도 독립형도 아닌 경우 (= 스택 중첩이 가능한 경우)
        {

            if (power != 0)// 위력 수치가 있다면
            {
                if(buf.power < power)// 큰쪽으로 덮어씀 (단, 단일형도 독립형도 아닌 버프는 대체로 위력값을 사용하지 않아야 함.)
                {
                    buf.power = power;
                }
            }

            if (buf.isPermanent)// 영구 지속 버프라면 지속 턴을 99로 고정
            {
                buf.duration = 99;
                //buf.power += power;//대신 위력이 조금 증가 
            }
            else //영구 지속이 아닐 경우, 지속 턴은 더 큰 쪽으로 적용
            {
                if(buf.duration <= duration)
                {
                    buf.duration = duration;
                }
                else
                {

                }

                if((buf.stack + stack) >= buf.maxStack)
                {
                    buf.stack = buf.maxStack;
                }
                else
                {
                    buf.stack += stack;
                }
                //스택은 중첩되며, 최대 중첩 이상은 중첩되지 않음.
            }

            buf.OnAddBuf();
        }
    }

    public CharacterBuf FindBuf(BattleKeyWords.BufKeyword key, CharacterBase buffer = null)// 특정 버프 찾기
    {
        CharacterBuf buf;

        if (buffer == null)
        {
            buf = bufList.Find(x => x.BufKeyword == key && !x.IsDestroyed);
        }
        else
        {
            buf = bufList.Find(x => x.BufKeyword == key && x.Buffer == buffer && !x.IsDestroyed);
        }

        return buf;
    }

    public List<CharacterBuf> FindPositiveBufAll(bool forDestroy = false)// 모든 긍정적 버프 가져오기
    {
        bufs.Clear();

        foreach (CharacterBuf buf in bufs)
        {
            if(buf.BufType == BattleKeyWords.BufType.Positive)
            {
                bufs.Add(buf);
            }
        }

        if (forDestroy)// 디버프 파괴용이라면
        {
            bufs = bufs.FindAll(x => x.dontDestroy == false);// 파괴 불가능한 버프 제외
        }

        return bufs;
    }

    public List<CharacterBuf> FindPositiveBuf(int number, bool forDestroy = false)// 긍정적 버프 특정 갯수 가져오기
    {
        bufs.Clear();

        foreach (CharacterBuf buf in bufs)
        {
            if (buf.BufType == BattleKeyWords.BufType.Positive)
            {
                bufs.Add(buf);
            }
        }

        if (forDestroy)// 디버프 파괴용이라면
        {
            bufs = bufs.FindAll(x => x.dontDestroy == false);// 파괴 불가능한 버프 제외
        }

        bufs = bufs.Take(number).ToList();
        return bufs;
    }

    public List<CharacterBuf> FindPositiveBufRandom(int number, bool forDestroy = false)// 무작위 긍정적 버프 가져오기
    {
        bufs.Clear();

        foreach (CharacterBuf buf in bufs)
        {
            if (buf.BufType == BattleKeyWords.BufType.Positive)
            {
                bufs.Add(buf);
            }
        }

        List<CharacterBuf> randomBufs = new List<CharacterBuf>();

        if (forDestroy)// 디버프 파괴용이라면
        {
            bufs = bufs.FindAll(x => x.dontDestroy == false);// 파괴 불가능한 버프 제외
        }

        while (randomBufs.Count < number)
        {
            int ran = UnityEngine.Random.Range(0, bufs.Count);

            CharacterBuf buf = bufs[ran];
            if (!randomBufs.Contains(buf))
            {
                randomBufs.Add(buf);
            }
        }

        return randomBufs;
    }

    public List<CharacterBuf> FindNegativeBufAll(bool forDestroy = false)// 모든 부정적 버프 가져오기
    {
        bufs.Clear();

        foreach (CharacterBuf buf in bufs)
        {
            if (buf.BufType == BattleKeyWords.BufType.Negative)
            {
                bufs.Add(buf);
            }
        }

        if (forDestroy)// 디버프 파괴용이라면
        {
            bufs = bufs.FindAll(x => x.dontDestroy == false);// 파괴 불가능한 버프 제외
        }

        return bufs;
    }

    public List<CharacterBuf> FindNegativeBuf(int number, bool forDestroy = false)// 부정적 버프 특정 갯수 가져오기
    {
        bufs.Clear();

        foreach (CharacterBuf buf in bufs)
        {
            if (buf.BufType == BattleKeyWords.BufType.Negative)
            {
                bufs.Add(buf);
            }
        }

        if (forDestroy)// 디버프 파괴용이라면
        {
            bufs = bufs.FindAll(x => x.dontDestroy == false);// 파괴 불가능한 버프 제외
        }

        bufs = bufs.Take(number).ToList();

        return bufs;
    }

    public List<CharacterBuf> FindNegativeBufRandom(int number, bool forDestroy = false)// 무작위 부정적 버프 가져오기
    {
        bufs.Clear();

        foreach (CharacterBuf buf in bufs)
        {
            if (buf.BufType == BattleKeyWords.BufType.Negative)
            {
                bufs.Add(buf);
            }
        }
        List<CharacterBuf> randomBufs = new List<CharacterBuf>();

        if (forDestroy)// 디버프 파괴용이라면
        {
            bufs = bufs.FindAll(x => x.dontDestroy == false);// 파괴 불가능한 버프 제외
        }

        while (randomBufs.Count < number)
        {
            int ran = UnityEngine.Random.Range(0, bufs.Count);

            CharacterBuf buf = bufs[ran];
            if (!randomBufs.Contains(buf))
            {
                randomBufs.Add(buf);
            }
        }

        return randomBufs;
    }
    
    public void RemoveBuf(CharacterBuf buf)// 버프 제거(주로 외부에서 접근)
    {
        buf.DestoyBuf();
    }

    public void ReduceBufStack(CharacterBuf buf, int power)// 버프 스택 감소(주로 외부에서 접근)
    {
        buf.duration -= power;

        if(buf.duration <= 0)
        {
            buf.DestoyBuf();
        }
    }

    public void ApplyRemovedBuf()// 버프 제거 목록에서 버프 제거
    {
        foreach(CharacterBuf buf in removeList)
        {
            bufList.Remove(buf);
        }

        removeList.Clear();
    }

    private void CheckDestroyBuf()// 버프 제거 목록 확보
    {

        foreach (CharacterBuf buf in bufList)
        {
            if (buf.IsDestroyed)
            {
                buf.OnDestroy();
                removeList.Add(buf);
            }
        }

        ApplyRemovedBuf();
    }



    //-------------------------------------------------------------------------------------------------------------------
    // 버프 적용

    public BonusStat GetAdditionalStat()// 추가 스탯 적용
    {
        BonusStat stat = new BonusStat
        {
            ExtraAtk = 1f,
            ExtraDefend = 1f,
            PenetrateDef = 1f,
            EnhancedDmg = 1f,
            ReducedDmg = 1f,
        };// 곱연산인 변수들은 초기값 1로

        foreach (CharacterBuf buf in bufList)
        {
            if (!buf.IsDestroyed && buf.GetAdditionalStat() != null)
            {
                stat.AddBonusStat(buf.GetAdditionalStat());
            }
        }

        return stat;
    }

    public void OnRoundStart()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnRoundStart();
            }
        }

        CheckDestroyBuf();
    }

    public void OnRoundEnd()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnRoundEnd();
            }
        }
    }

    public void OnTurnStart()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnTurnStart();
            }
        }
    }

    public void OnTurnEnd()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnTurnEnd();
            }
        }
    }

    public void OnEndActing()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnEndActing();
            }
        }
    }

    public void OnPassAlly(CharacterBase character)// 아군 위를 지나갔을 때 발동
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnPassAlly(character);
            }
        }
    }

    public void OnAllyPassedMe(CharacterBase character)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnAllyPassedMe(character);
            }
        }
    }

    public void OnPassEnemy(CharacterBase character)// 적군 위를 지나갔을 때 발동
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnPassEnemy(character);
            }
        }
    }

    public void OnEnemyPassesMe(CharacterBase character)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnEnemyPassesMe(character);
            }
        }
    }

    public void OnStartAttack(CharacterBase character)
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnStartAttack(character);
            }
        }
    }

    public void OnAttackSuccess(CharacterBase character, BattleKeyWords.Damage damage = new BattleKeyWords.Damage())
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnAttackSuccess(character, damage);
            }
        }
    }

    public void OnEndAttack(CharacterBase character)
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnEndAttack(character);
            }
        }
    }

    public void OnTakeAttack(CharacterBase enemy)// 공격 받기 이전에
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnTakeAttack(enemy);
            }
        }
    }

    public void OnTakeDamage(ref int damage, CharacterBase character = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnTakeDamage(ref damage, character, damageType, characterAttribute);
            }
        }
    }

    public void OnTakeHeal(ref int damage, CharacterBase character = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnTakeHeal(ref damage, character, damageType, characterAttribute);
            }
        }
    }

    public virtual void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.AfterTakeDamage(damage, character, damageType, characterAttribute);
            }
        }
    }

    public virtual void AfterTakeHeal(int heal, CharacterBase skillUser = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐 받은 이후에
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.AfterTakeHeal(heal, character, damageType, characterAttribute);
            }
        }
    }

    public void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnUseSkill(target);
            }
        }
    }

    public void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnSkillAttackSuccess(target, damage);
            }
        }
    }

    public void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnSkillHealSuccess(target, heal);
            }
        }
    }

    public void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnEndSkill(target);
            }
        }
    }

    public void OnDie()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnDie();
            }
        }
    }

    public void OnUpdate()// 실시간 판정
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnUpdate();
            }
        }
    }
}
