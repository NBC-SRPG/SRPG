using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<CharacterAI> characterList;
    [SerializeField] private CharacterAI chaPrefabs;

    public GamePlayer player;

    private void Awake()
    {
        characterList = new List<CharacterAI>();
    }

    private void Start()
    {
        player.playerId = "enemy";
        player.playerNumber = 1;

        if (!Managers.BattleManager.players.Contains(player))
        {
            Managers.BattleManager.players.Add(player);
        }
        Managers.BattleManager.charactersAsTeam.Add(player.playerId, new List<CharacterBase>());

        Managers.BattleManager.TurnStart += GetPlayerTurn;

        InitiateCharacter();
    }

    public void InitiateCharacter()//캐릭터 스폰위치에 캐릭터 생성
    {
        int i = 0;
        foreach (Character charac in player.party)
        {
            if (i < Managers.MapManager.startTiles[player.playerNumber].Count)
            {
                CharacterAI character = Instantiate(chaPrefabs, transform);

                character.curStandingTile = Managers.MapManager.map[Managers.MapManager.startTiles[player.playerNumber][i]];
                character.curStandingTile.curStandingCharater = character;

                character.transform.position = character.curStandingTile.transform.position;
                i++;

                characterList.Add(character);
            }
        }
    }

    private void GetPlayerTurn()
    {
        if (Managers.BattleManager.nowPlayer.playerId == player.playerId)
        {

        }
    }
}
