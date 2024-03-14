using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamePlayer
{
    public string playerId;

    [Range (0, 2)]public int playerNumber = 0;//플레이어의 시작 위치를 잡기 위한 번호

    public int prioty;
    public bool isReady;

    public List<Character> party;
}
