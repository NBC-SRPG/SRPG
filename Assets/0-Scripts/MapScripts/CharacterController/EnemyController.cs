using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyController : MonoBehaviour
{
    private List<CharacterAI> characterList;
    private Dictionary<int, List<CharacterAI>> characterWave;

    private int nowWave;

    [SerializeField] private CharacterAI chaPrefabs;

    private StageSO stage;

    public GamePlayer player;

    private int index;
    private int roundCount;

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
            Debug.Log(charac.SO.characterName);
            character.InitCharacter(charac, player);

            characterList.Add(character);
        }

        SetWave();

        player.isReady = true;

        BattleManager.Instance.GameStart += GameStart;

        if (stage.spawnByRound)// 몬스터가 매턴 소환되는 구조라면
        {
            BattleManager.Instance.RoundStart += SpawnEnemyByTurn;
            roundCount = stage.spawnRound;
        }
    }

    //--------------------------------------------------------------------------------------------------
    //게임 시작 설정

    public void SetWave()// 웨이브 설정
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
                    characterWave.Add(i, new List<CharacterAI>(wave));
                    wave.Clear();
                    i++;
                }
            }
        }

        if (i <= stage.waveNumber)
        {
            characterWave.Add(i, new List<CharacterAI>(wave));
        }

        nowWave = 0;
    }

    public void GameStart()
    {
        InitiateWave();
    }

    public void InitiateWave()// 웨이브 소환
    {
        if (BattleManager.Instance.gameEnd)
        {
            return;
        }

        if (stage.stageType == Constants.StageType.MainStory)
        {
            InitiateWaveDefault();
        }

        if (stage.spawnByRound)
        {
            roundCount = stage.spawnRound;
        }
    }

    //-----------------------------------------------------------------------
    //웨이브 생성

    private void InitiateWaveDefault()// 기본적인 적 생성(웨이브 출현, 전부 처치 시 다음 웨이브 출현)
    {
        nowWave++;

        if (stage.spawnType == Constants.EnemySpawnType.Infinite && nowWave >= stage.infiniteWave)// 무한 웨이브에 도달했을 때
        {
            if (characterWave.ContainsKey(nowWave))// 이미 있는 파티 웨이브라면
            {
                characterWave.Remove(nowWave);//없애버림
            }
            characterWave.Add(nowWave, new List<CharacterAI>());

            foreach (EnemySO enemy in stage.infiniteEnemy)// 적을 생성함
            {
                Character enemyCharacter = new Character(enemy);
                CharacterAI character = Instantiate(chaPrefabs, transform);
                character.InitCharacter(enemyCharacter, player);

                characterWave[nowWave].Add(character);
            }
        }

        if (MapManager.instance.enemyStartTiles.Count > 1)// 특정 웨이브는 특정 위치에 소환
        {
            BattleManager.Instance.SpawnEnemy(characterWave[nowWave], nowWave - 1);
        }
        else
        {
            BattleManager.Instance.SpawnEnemy(characterWave[nowWave], 0);
        }


        foreach (CharacterAI character in characterWave[nowWave])// 소환된 캐릭터에 이벤트 연결
        {
            character.Disable += CheckRemainEnemy;

            if (stage.chaseAtStart)
            {
                if (character.gameObject.activeInHierarchy)
                {
                    character.ChaseStart();
                }
            }
        }

        BattleManager.Instance.SetWave(nowWave);

        Debug.Log("nowWave " + nowWave);
    }



    //-----------------------------------------------------------------------
    //다음 웨이브 조건

    private void SpawnEnemyByTurn()
    {
        roundCount--;

        if(roundCount == 0)
        {
            InitiateWave();
        }
    }

    private void CheckRemainEnemy()
    {
        int cnt = 0;

        foreach (CharacterAI character in characterWave[nowWave])
        {
            if (character.isDead || !character.gameObject.activeInHierarchy)
            {
                cnt++;
                character.Disable -= CheckRemainEnemy;

                if (cnt == characterWave[nowWave].Count)
                {
                    InitiateWave();
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    //게임 플레이


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
        CameraController.instance.ResetGroup();
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

        Debug.Log("now Acting " + index);

        if (index >= characterWave[nowWave].Count)// 모든 AI가 대기 상태일 때
        {
            //Debug.Log("AI turn end");
            Invoke(nameof(TurnEnd), 0.1f);// 턴 종료
            return;
        }

        if (!characterWave[nowWave][index].canActing)// 해당 AI가 행동 불가 상태일 때
        {
            //Debug.Log(index + " already Act");
            index++;// 다음 AI 행동
            StartAIActing();
            return;
        }

        characterWave[nowWave][index].Wait += CheckWait;// 해당 AI가 행동했는지 확인
        characterWave[nowWave][index].Act += CheckActing;

        Debug.Log(index + " start Act");
        characterWave[nowWave][index].StartAI();// AI 작동 시작
    }

    private void CheckWait()// Ai가 대기 상태일 때
    {
        //Debug.Log(index + " is waiting");
        characterWave[nowWave][index].Wait -= CheckWait;
        characterWave[nowWave][index].Act -= CheckActing;

        index++;// 다음 AI 차례로 넘어감

        StartAIActing();
    }

    private void CheckActing()// AI가 행동했을 때
    {
        //Debug.Log(index + " is acted");
        characterWave[nowWave][index].Wait -= CheckWait;
        characterWave[nowWave][index].Act -= CheckActing;

        index = 0;// 다시 처음부터 행동 가능한 캐릭터가 행동함

        StartAIActing();
    }

    private void TurnEnd()
    {
        BattleManager.Instance.PlayerTurnEnd();
    }


}
