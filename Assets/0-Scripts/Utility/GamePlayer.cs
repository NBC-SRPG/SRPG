using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamePlayer
{
    public string playerId;

    public int playerStartPosition = 0;//플레이어의 시작 위치를 잡기 위한 번호

    public int prioty;
    public bool isReady = false;
    public bool isWin = false;

    public int manaCost;
    public int manaNextTurn;

    public Character[] party = new Character[5];

    public void ResetPlayer()
    {
        playerStartPosition = 0;
        prioty = 10;
        isReady = false;
        isWin = false;

        manaCost = 0;
        manaNextTurn = 0;
    }

    public void GainMana(int n)
    {
        manaCost += n;
        if(manaCost > 300) //테스트용 수정
        {
            manaCost = 300;
        }
    }

    public void GainManaNextTurn(int n)
    {
        manaNextTurn += n;
    }
}
