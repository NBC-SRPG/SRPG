using System.Collections.Generic;
using UnityEngine;

public class AccountData
{
    public Dictionary<string, int> StageClearData { get; set; }
    public Dictionary<int, CharacterSO> CharacterData { get; set; }
    public PlayerData PlayerData { get; set; }
    public Dictionary<int, bool> MissionClearData { get; set; }
    public Dictionary<ItemData, int> Inventory { get; set; }
    public Dictionary<int, string[]> FriendData { get; set; }
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
}