using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBufList
{
    private CharacterBase character;

    public List<CharacterBuf> bufList;
    private List<CharacterBuf> removeList;

    public CharacterBufList(CharacterBase character)
    {
        this.character = character;
        bufList = new List<CharacterBuf>();
        removeList = new List<CharacterBuf>();
    }

    //-------------------------------------------------------------------------------------------------------------------
    // 버프 컨트롤

    public void AddBuf(BattleKeyWords.BufKeyword key, int stack, CharacterBase buffer = null)
    {
        CharacterBuf buf;

        // 리스트에 버프가 있는지 확인
        if(buffer == null)
        {
            buffer = character;
        }

        buf = bufList.Find(x => x.BufKeyword == key && !x.IsDestroyed);

        if (buf == null)// 없다면 새로 생성
        {
            switch (key)
            {
                case BattleKeyWords.BufKeyword.Burn:
                    buf = new CharacterBuf_Burn();
                    break;
                case BattleKeyWords.BufKeyword.Bleed:
                    buf = new CharacterBuf_Bleed();
                    break;

                case BattleKeyWords.BufKeyword.Test_UniqBuf:
                    buf = new CharacterBuf_Herald();
                    break;
            }

            if (buf != null)// 버프 생성 이후 리스트에 삽입
            {
                buf.Init(character, buffer);
                bufList.Add(buf);
                buf.OnAddBuf();
            }
        }

        if (buf != null)//리스트에 버프가 있다면, 스택 증가
        {
            buf.stack += stack;
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

    public List<CharacterBuf> FindPositiveBuf(int number)// 긍정적 버프 가져오기
    {
        List<CharacterBuf> bufs = bufList.FindAll(x => x.BufType == BattleKeyWords.BufType.Positive);

        bufs = bufs.Take(number).ToList();

        return bufs;
    }

    public List<CharacterBuf> FindNegativeBuf(int number)// 부정적 버프 가져오기
    {
        List<CharacterBuf> bufs = bufList.FindAll(x => x.BufType == BattleKeyWords.BufType.Negative);

        bufs = bufs.Take(number).ToList();

        return bufs;
    }

    public void RemoveBuf()// 버프 제거
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

        RemoveBuf();
    }



    //-------------------------------------------------------------------------------------------------------------------
    // 버프 적용

    public BonusStat GetAdditionalStat()// 추가 스탯 적용
    {
        BonusStat stat = new BonusStat();

        foreach(CharacterBuf buf in bufList)
        {
            if (!buf.IsDestroyed && buf.GetAdditionalStat() != null)
            {
                stat.AddStat(buf.GetAdditionalStat());
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

    public void OnAttackSuccess(CharacterBase character, int damage = 0)
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

    public void OnTakeDamage(CharacterBase character)
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf != null && !buf.IsDestroyed)
            {
                buf.OnTakeDamage(character);
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
}
