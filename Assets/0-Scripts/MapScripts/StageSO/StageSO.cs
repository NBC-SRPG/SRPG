using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "Stage", fileName = "Stage_")]
public class StageSO : SerializedScriptableObject
{
    [Header("StageInfo")]
    public int stageId;
    public string stageName;
    public string stageNumber;
    public StageClear clear;
    public StageType stageType;
    public List<EnemySO> enemiesInfo;
    public int recommendLevel;
    public bool isTutorial = false;
    public string bgm;

    [Header("Enemy")]
    public List<EnemySO> enemies;
    public EnemySpawnType spawnType = EnemySpawnType.Wave;
    public bool chaseAtStart;

    [Header("Prefabs")]
    public string prefabsName;

    [Header("Detail")]
    public int waveNumber = 1;// 웨이브 수
    public Dictionary<int, int> enemyAtWave = new();// 한 웨이브 당 적 수

    [Header("Defence Detail")]
    public int defenceRound;// 버텨야 하는 턴

    [Header("Run Detail")]
    public List<Vector2Int> targetGrid;// 목표 위치

    [Header("Assassinate Detail")]
    public List<int> targetEnemy;// 목표 적

    [Header("Infinite Detail")]
    public int infiniteWave = -1;// 몇 번째 턴부터 같은 적이 계속 등장하는지
    public List<EnemySO> infiniteEnemy;// 무한히 등장할 적
    public bool spawnByRound;// 매턴 소환할지 설정
    public int spawnRound;// 몇 턴 마다 소환될 지 설정

    [Header("ExtraGoal")]
    public ExtraGoalDetail[] extraGoal = new ExtraGoalDetail[3];

    [Header("Reward")]
    public int exp;
    public int gold;
    public Dictionary<int, int> rewards = new();

    [HideInInspector] public List<Character> characterList;

    public List<Character> GetEnemy()
    {
        List<Character> list = new List<Character>();

        foreach(EnemySO enemy in enemies)
        {
            Character enemyCharacter = new Character(enemy);
            list.Add(enemyCharacter);
        }

        characterList = list;

        return list;
    }

    public List<Character> GetTargetEnemy()
    {
        List<Character> list = new List<Character>();

        foreach (int number in targetEnemy)
        {
            list.Add(characterList[number]);
        }

        return list;
    }

    public EnemySO GetSOByInt(int index)
    {
        return enemies[index];
    }

    public Character GetTargetByInt(int index)
    {
        return characterList[index];
    }

    public string GetExtraGoalDetail(int index)
    {
        ExtraGoalDetail detail = extraGoal[index];
        string detailString = "";

        switch (detail.type)
        {
            case ExtraGoal.Clear:
                detailString = "스테이지 클리어";
                break;
            case ExtraGoal.InnerTurn:
                detailString = detail.value + " 라운드 내에 스테이지 클리어";
                break;
            case ExtraGoal.KillOver:
                detailString = "적 " + detail.value + " 명 이상 처치";
                break;
            case ExtraGoal.KillSomeone:
                detailString = GetSOByInt(detail.value).characterName + " 처치";
                break;
            case ExtraGoal.NoDie:
                detailString = "파티원이 모두 생존한 채로 클리어";
                break;
            case ExtraGoal.Empty:
                detailString = "";
                break;
        }

        return detailString;
    }

    public string GetBGMPath()
    {
        return "BGM/" + bgm;
    }
}
