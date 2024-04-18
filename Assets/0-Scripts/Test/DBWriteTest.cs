using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DBWriteTest : MonoBehaviour
{
    Database db;
    int id;
    CharacterGrowth growth;
    Dictionary<int, int> items;

    void Start()
    {
        items = new Dictionary<int, int>();
        for (int i = 20001; i < 20033; i++)
        {
            items.Add(i, 20000);
        }
    }
    
    public void OnClick()
    {
        Managers.DB.WriteWithJson(Managers.DB.userDB.Child("inventory"), items);
        Debug.Log("Complete");
    }

}

