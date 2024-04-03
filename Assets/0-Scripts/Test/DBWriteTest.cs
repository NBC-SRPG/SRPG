using System;
using UnityEngine;

[Serializable]
public class DBWriteTest : MonoBehaviour
{
    Database db;
    int id;
    public CharacterGrowth growth;

    void Start()
    {
        db = new Database();
        id = 101;
        Utility.Id2SO<CharacterSO>(id, (result) => 
        {
            growth = new CharacterGrowth((CharacterSO)result);
        });
    }
    
    public void OnClick()
    {
        Debug.Log(growth.maxExp);

        db.WriteWithJson(db.userDB.Child("characterData").Child(id.ToString()), growth);

    }

}

