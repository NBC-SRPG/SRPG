using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class TestGameManager : MonoBehaviour
{
    private static TestGameManager instance = null;
    public static TestGameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    public void GetItem(int itemId)
    {
        Debug.Log($"GetItem + {itemId}");

        Managers.Mission.NotifyMission(MissionType.GetItem, itemId, 1);
    }

    public void UseItem(int itemId)
    {
        Debug.Log($"UseItem + {itemId}");

        Managers.Mission.NotifyMission(MissionType.UseItem, itemId, 3);
    }

    public void KillMonster(int monsterId)
    {
        Debug.Log($"KillMonster + {monsterId}");

        Managers.Mission.NotifyMission(MissionType.KillMonster, monsterId, 2);
    }
}
