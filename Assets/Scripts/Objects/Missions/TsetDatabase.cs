using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestDatabase : MonoBehaviour
{
    private static TestDatabase instance = null;
    public static TestDatabase Instance { get { return instance; } }

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

    private MissionDB mission;
    public static MissionDB Mission
    {
        get
        {
            if (Instance.mission == null)
                Instance.mission = new MissionDB();
            return Instance.mission;
        }
    }
}
