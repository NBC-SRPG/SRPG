using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, CharacterGrowth> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, bool> missionClearData { get; set; }
    //public Dictionary<int, int> inventory { get; set; }  Todo: 아이템 데이터 추가 시 활성화 필요
    public Dictionary<string, string[]> friendData { get; set; }
    public VersionData versionData { get; set; }
    public int gachaPoint { get; set; }

    // Init �޼���
    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, CharacterGrowth> characterData,
        PlayerData playerData,
        Dictionary<int, bool> missionClearData,
        //Dictionary<int, int> inventory,
        Dictionary<string, string[]> friendData
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, CharacterGrowth>();
        this.playerData = playerData ?? new PlayerData();
        this.missionClearData = missionClearData ?? new Dictionary<int, bool>();
        //this.inventory = inventory ?? new Dictionary<int, int>();
        this.friendData = friendData ?? new Dictionary<string, string[]>();
    }
}
