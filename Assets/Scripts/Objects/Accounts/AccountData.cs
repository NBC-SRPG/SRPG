using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, CharacterSO> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, bool> missionClearData { get; set; }
    public Dictionary<ItemData, int> inventory { get; set; }
    public Dictionary<int, string[]> friendData { get; set; }

    // Init ¸Þ¼­µå
    public void Init(
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