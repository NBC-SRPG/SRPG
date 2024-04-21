using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static BattleKeyWords;
using static Constants;

[CreateAssetMenu(menuName = "Stage", fileName = "Stage_")]
public class StageSO : SerializedScriptableObject
{
    [Header("StageInfo")]
    public string stageName;
    public StageClear clear;
    public StageType stageType;

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
}
