using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager
{
    private Constants.GachaType gachaType;

    // 확률 테이블의 캐싱 딕셔너리
    private Dictionary<string, Dictionary<int, float>> tables = new Dictionary<string, Dictionary<int, float>>();


    public void Init()
    {
        //tables.Clear();
        tables = new Dictionary<string, Dictionary<int, float>>();
        gachaType = Constants.GachaType.Common;
    }

    private Dictionary<int, float> GetTableFromDB(string tableName)
    {
        // TODO: DB에서 확률 테이블을 불러와 캐시에 등록하는 로직

        return new Dictionary<int, float>();
    }

    public Dictionary<int, float> GetTable(string tableName)
    {
        if (tables.ContainsKey(tableName))
        {
            return tables[tableName];
        }
        else
        {
            return GetTableFromDB(tableName);
        }
    }

    // 테이블에서 확률에 따라 캐릭터 id를 하나 반환
    private int GetRandomCharacterFromTable(string tableName)
    {
        Dictionary<int, float> table = GetTable(tableName);

        double chance = Random.Range(0.0f, 100.0f); // [0.0, 100.0]

        int result = 0;
        double current = 0.0;
        foreach (var pair in table)
        {
            result = pair.Key;
            current += pair.Value;
            if (chance < current)
            {
                break;
            }
        }
        return result;
    }

    // 1회 뽑기
    public int Draw(string tableName)
    {
        int result = GetRandomCharacterFromTable(tableName);
        Result(result);
        if (gachaType == Constants.GachaType.PickUp)
        {
            Managers.AccountData.gachaPoint += 1;
        }

        return result;
    }

    // 10회 뽑기
    public Queue<int> Draw10Times(string tableName)
    {
        Queue<int> queue = new Queue<int>();

        for (int i = 0; i < 9; i++)
        {
            queue.Enqueue(GetRandomCharacterFromTable(tableName));
        }
        // 2성 이상만 나오는 테이블
        queue.Enqueue(GetRandomCharacterFromTable(tableName + "Upper_2"));

        foreach (var id in queue)
        {
            Result(id);
        }

        if (gachaType == Constants.GachaType.PickUp)
        {
            Managers.AccountData.gachaPoint += 10;
        }

        return queue;
    }

    // 뽑은 캐릭터의 최종 획득 처리
    private void Result(int id)
    {
        if (IsDuplicated(id))
        {
            // TODO: 성급에 비례한 조각 획득
        }
        else
        {
            // Managers.AccountData.Add(characterId)
        }
    }

    // 뽑은 캐릭터를 이미 보유중인지 확인
    public bool IsDuplicated(int characterId)
    {
        if (Managers.AccountData.characterData.ContainsKey(characterId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
