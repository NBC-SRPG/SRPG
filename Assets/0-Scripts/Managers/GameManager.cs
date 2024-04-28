using System;

public class GameManager
{
    public GamePlayer player;
    public GamePlayer enemy;

    public string stageName;
    public StageSO thisStage;

    public bool nowTesting;

    public void Init()
    {
        player = new GamePlayer();
        enemy = new GamePlayer();

        //player.playerId = Managers.AccountData.playerData.uId;
        player.playerId = "test";
        player.prioty = 10;

        InitParty();

        nowTesting = false;
    }

    public void SetEnemy(GamePlayer enemy)
    {
        this.enemy = enemy;
    }

    public void InitParty()
    {
        if(Managers.AccountData.formationData == null || Managers.AccountData.formationData.Count == 0)
        {
            return;
        }

        FormationData formation = Managers.AccountData.formationData[0];

        for(int i = 0; i < formation.characterId.Length; i++)
        {
            Character character;

            if(!Managers.AccountData.characterData.TryGetValue(formation.characterId[i], out character))
            {
                continue;
            }

            player.party[i] = character;
        }

    }

    public void UpdateParty(FormationData formation)
    {
        for (int i = 0; i < formation.characterId.Length; i++)
        {
            Character character;

            if (!Managers.AccountData.characterData.TryGetValue(formation.characterId[i], out character))
            {
                continue;
            }

            UpdatePartyCharacter(formation, character, i);
        }
    }

    public void UpdatePartyCharacter(FormationData formationData, Character character, int index)
    {
        //파티에 이미 캐릭터가 있는지 확인
        int exist = Array.IndexOf(Managers.GameManager.player.party, character);

        if (exist > -1)//있다면 해당 자리를 null로
        {
            player.party[exist] = null;
        }
        player.party[index] = character;
    }
}
