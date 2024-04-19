using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<CharacterAI> characterList;
    private Dictionary<int, List<CharacterAI>> characterWave;

    private int nowWave;

    [SerializeField] private CharacterAI chaPrefabs;

    private StageSO stage;

    public GamePlayer player;

    private int index;

    public List<Character> characters = new List<Character>();
    public List<EnemySO> enemyList = new List<EnemySO>();

    private void Awake()
    {
        characterList = new List<CharacterAI>();
        characterWave = new Dictionary<int, List<CharacterAI>>();

        stage = Managers.GameManager.thisStage;
    }

    private void Start()
    {
        player.playerId = "enemy";

        if (!BattleManager.Instance.players.Contains(player))
        {
            BattleManager.Instance.players.Add(player);
        }
        BattleManager.Instance.charactersAsTeam.Add(player.playerId, new List<CharacterBase>());

        BattleManager.Instance.TurnStart += GetPlayerTurn;

        if (enemyList.Count == 0)
        {
            characters = Managers.GameManager.enemy.party.ToList();
            player.party = Managers.GameManager.enemy.party;
        }
        else
        {
            foreach(EnemySO enemy in enemyList)
            {
                Character enemyCharacter = new Character(enemy);
                characters.Add(enemyCharacter);
            }

            player.party = characters.ToArray();
        }

        player.party = stage.GetEnemy().ToArray();

        foreach (Character charac in player.party)
        {
            if (charac == null)
            {
                continue;
            }

            CharacterAI character = Instantiate(chaPrefabs, transform);
            character.InitCharacter(charac, player);

            characterList.Add(character);
        }

        SetWave();

        player.isReady = true;

        BattleManager.Instance.GameStart += GameStart;
    }

    public void SetWave()
    {
        int i = 1;
        List<CharacterAI> wave = new List<CharacterAI>();

        foreach(CharacterAI ai in characterList)
        {
            if(i > stage.waveNumber)
            {
                break;
            }

            wave.Add(ai);

            if (stage.enemyAtWave.ContainsKey(i))
            {
                if (wave.Count == stage.enemyAtWave[i] && stage.enemyAtWave[i] > 0)
                {
                    characterWave.Add(i, wave);
                    wave.Clear();
                    i++;
                }
            }
        }

        if (i <= stage.waveNumber)
        {
            characterWave.Add(i, wave);
        }

        nowWave = 0;
    }

    public void GameStart()
    {
        InitiateWave();
    }

    public void InitiateWave()// 웨이브 소환
    {
        nowWave++;

        if(stage.spawnType == Constants.EnemySpawnType.Infinite && nowWave == stage.infiniteWave)
        {
            characterWave.Remove(nowWave);
            characterWave.Add(nowWave, new List<CharacterAI>());

            foreach (EnemySO enemy in stage.infiniteEnemy)
            {
                Character enemyCharacter = new Character(enemy);
                CharacterAI character = Instantiate(chaPrefabs, transform);
                character.InitCharacter(enemyCharacter, player);

                characterWave[nowWave].Add(character);
            }

        }

        BattleManager.Instance.SpawnEnemy(characterWave[nowWave], player.playerStartPosition);
        BattleManager.Instance.nowWave = nowWave;

        Debug.Log("nowWave " + nowWave);
    }

    private void GetPlayerTurn()
    {
        if (BattleManager.Instance.nowPlayer.playerId == player.playerId)
        {
            index = 0;

            StartCoroutine(nameof(WaitForaSecond));
        }
    }

    private IEnumerator WaitForaSecond()
    {
        CameraController.instance.AddGroupRange(new List<CharacterBase>(characterWave[nowWave].FindAll(x => !x.isDead)));
        CameraController.instance.SetCameraOnSelected();

        yield return new WaitForSeconds(1f);

        StartAIActing();
        CameraController.instance.ResetGroup();
    }

    private void StartAIActing()// Ai 작동
    {
        if (BattleManager.Instance.gameEnd)
        {
            return;
        }

        //Debug.Log("now Acting " + index);

        if (index >= characterList.Count)// 모든 AI가 대기 상태일 때
        {
            //Debug.Log("AI turn end");
            Invoke(nameof(TurnEnd), 0.1f);// 턴 종료
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

    private void TurnEnd()
    {
        BattleManager.Instance.PlayerTurnEnd();
    }
}
