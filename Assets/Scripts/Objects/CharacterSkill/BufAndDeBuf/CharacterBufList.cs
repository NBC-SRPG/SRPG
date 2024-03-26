using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBufList
{
    private CharacterBase character;

    public List<CharacterBuf> bufList;

    public CharacterBufList(CharacterBase character)
    {
        this.character = character;
        bufList = new List<CharacterBuf>();
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

    public CharacterBuf FindBuf(BattleKeyWords.BufKeyword key, CharacterBase buffer = null)
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

    public void RemoveBuf(CharacterBuf buf)
    {
        bufList.Remove(buf);
    }

    private void CheckDestroyBuf()
    {
        foreach (CharacterBuf buf in bufList)
        {
            if (buf.IsDestroyed)
            {
                buf.OnDestroy();
                RemoveBuf(buf);
            }
        }
    }



    //-------------------------------------------------------------------------------------------------------------------
    // 버프 적용

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
