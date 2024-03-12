using System.Collections.Generic;
using UnityEngine;

public class AccountData
{
    private Dictionary<string, int> stageClearData;
    private Dictionary<int, CharacterSO> characterData;
    private PlayerData playerData;
    private Dictionary<int, bool> missionClearData;
    private Dictionary<ItemData, int> inventory;
    private Dictionary<int, string[]> friendData;

    // 매개변수를 받는 생성자 메서드
    public AccountData
        (
        Dictionary<string, int> stageClearData,
        Dictionary<int, CharacterSO> characterData,
        PlayerData playerData,
        Dictionary<int, bool> missionClearData,
        Dictionary<ItemData, int> inventory,
        Dictionary<int, string[]> friendData
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, CharacterSO>();
        this.playerData = playerData ?? new PlayerData();
        this.missionClearData = missionClearData ?? new Dictionary<int, bool>();
        this.inventory = inventory ?? new Dictionary<ItemData, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
    }

    // 각 데이터 필드에 대한 접근자 메서드
    public Dictionary<string, int> StageClearData
    {
        get { return stageClearData; }
        set { stageClearData = value; }
    }

    public Dictionary<int, CharacterSO> CharacterData
    {
        get { return characterData; }
        set { characterData = value; }
    }

    public PlayerData PlayerData
    {
        get { return playerData; }
        set {  playerData = value; }
    }

    public Dictionary<int, bool> MissionClearData
    {
        get { return missionClearData; }
        set { missionClearData = value; }
    }

    public Dictionary<ItemData, int> Inventory
    {
        get { return inventory; }
        set {  inventory = value; }
    }

    public Dictionary<int, string[]> FriendData
    {
        get { return friendData; }
        set {  friendData = value; }
    }
}