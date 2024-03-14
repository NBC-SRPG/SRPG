using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, Character> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, bool> missionClearData { get; set; }
    //public Dictionary<ItemData, int> inventory { get; set; }  Todo: ������ ������ �߰� �� Ȱ��ȭ �ʿ�
    public Dictionary<int, string[]> friendData { get; set; }

    public int gachaPoint { get; set; } = 0;

    // Init �޼���
    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, Character> characterData,
        PlayerData playerData,
        Dictionary<int, bool> missionClearData,
        //Dictionary<ItemData, int> inventory,
        Dictionary<int, string[]> friendData
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, Character>();
        this.playerData = playerData ?? new PlayerData();
        this.missionClearData = missionClearData ?? new Dictionary<int, bool>();
        //this.inventory = inventory ?? new Dictionary<ItemData, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
    }
}