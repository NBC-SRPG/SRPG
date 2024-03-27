using System.Collections.Generic;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, Character> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, bool> missionClearData { get; set; }
    //public Dictionary<ItemData, int> inventory { get; set; }  Todo: 아이템 데이터 추가 시 활성화 필요
    public Dictionary<int, string[]> friendData { get; set; }
    public Dictionary<int, FormationData> formationData { get; set; }
    public VersionData versionData { get; set; }
    public int gachaPoint { get; set; }
    public List<MailSO> mailBox { get; set; }

    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, Character> characterData,
        PlayerData playerData,
        Dictionary<int, bool> missionClearData,
        //Dictionary<ItemData, int> inventory,
        Dictionary<int, string[]> friendData,
        Dictionary<int, FormationData> formationData,
        List<MailSO> mailBox
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, Character>();
        this.playerData = playerData ?? new PlayerData();
        this.missionClearData = missionClearData ?? new Dictionary<int, bool>();
        //this.inventory = inventory ?? new Dictionary<ItemData, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
        this.formationData = formationData ?? new Dictionary<int, FormationData>();
        this.mailBox = mailBox ?? new List<MailSO>();
    }
}
