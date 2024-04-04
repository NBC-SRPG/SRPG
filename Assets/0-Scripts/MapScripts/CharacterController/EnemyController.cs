using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<CharacterAI> characterList;
    [SerializeField] private CharacterAI chaPrefabs;

    public GamePlayer player;

    private int index;

    public List<Character> characters = new List<Character>();
    public List<TempGrowth> tempList = new List<TempGrowth>();

    private void Awake()
    {
        characterList = new List<CharacterAI>();
    }

    private void Start()
    {
        player.playerId = "enemy";
        player.playerNumber = 1;

        characters = Managers.GameManager.enemy.party.ToList();

        //if (characters.All(x => x == null))
        //{
        //    characters.Clear();
        //    foreach (TempGrowth tempGrowth in tempList)
        //    {
        //        tempGrowth.Init();
        //        characters.Add(new Character(tempGrowth.tempCharacter, tempGrowth.growth));
        //    }
        //}

        if (!Managers.BattleManager.players.Contains(player))
        {
            Managers.BattleManager.players.Add(player);
        }
        Managers.BattleManager.charactersAsTeam.Add(player.playerId, new List<CharacterBase>());

        Managers.BattleManager.TurnStart += GetPlayerTurn;

        player.party = Managers.GameManager.enemy.party;

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
                character.InitCharacter(charac, player.playerId);

                character.SpawnCharacter(Managers.MapManager.map[Managers.MapManager.startTiles[player.playerNumber][i]], transform);
                i++;

                characterList.Add(character);
            }
        }

        player.isReady = true;
        Managers.BattleManager.GetReady();
    }

    private void GetPlayerTurn()
    {
        if (Managers.BattleManager.nowPlayer.playerId == player.playerId)
        {
            index = 0;

            StartAIActing();
        }
    }

    private void StartAIActing()// Ai 작동
    {
        //Debug.Log("now Acting " + index);

        if (index >= characterList.Count)// 모든 AI가 대기 상태일 때
        {
            //Debug.Log("AI turn end");
            Managers.BattleManager.PlayerTurnEnd();// 턴 종료
            return;
        }

        if (!characterList[index].canActing)// 해당 AI가 행동 불가 상태일 때
        {
            //Debug.Log(index + " already Act");
            index++;// 다음 AI 행동
            StartAIActing();
            return;
        }

        characterList[index].Wait += CheckWait;// 해당 AI가 행동했는지 확인
        characterList[index].Act += CheckActing;

        Debug.Log(index + " start Act");
        characterList[index].StartAI();// AI 작동 시작
    }

    private void CheckWait()// Ai가 대기 상태일 때
    {
        //Debug.Log(index + " is waiting");
        characterList[index].Wait -= CheckWait;
        characterList[index].Act -= CheckActing;

        index++;// 다음 AI 차례로 넘어감

        StartAIActing();
    }

    private void CheckActing()// AI가 행동했을 때
    {
        //Debug.Log(index + " is acting");
        characterList[index].Wait -= CheckWait;
        characterList[index].Act -= CheckActing;

        index = 0;// 다시 처음부터 행동 가능한 캐릭터가 행동함

        StartAIActing();
    }
}
