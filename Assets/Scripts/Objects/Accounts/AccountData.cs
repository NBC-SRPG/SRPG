using System.Collections.Generic;
using UnityEngine;

public class AccountData : MonoBehaviour
{
    protected Dictionary<string, int> stageClearData;
    protected Dictionary<int, CharacterSO> characterData;
    protected PlayerData playerData;
    protected Dictionary<int, bool> missionClearData;
    protected Dictionary<ItemData, int> inventory;
    protected Dictionary<int, string[]> friendData;

    // �Ű������� �޴� ������ �޼���
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

    // �� ������ �ʵ忡 ���� ������ �޼���
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