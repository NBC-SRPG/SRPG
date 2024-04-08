using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class DBReadTest : MonoBehaviour
{
    Database db;
    int id;
    CharacterGrowth growth;

    void Start()
    {
        db = new Database();
        id = 101;
        growth = new CharacterGrowth();
    }
    
    public void OnClick()
    {
        db.Read(db.userDB.Child("characterData").Child(id.ToString()), (snapshot) =>
        {
            string s = snapshot.GetRawJsonValue();
            Debug.Log(s);

            growth = JsonConvert.DeserializeObject<CharacterGrowth>(s);
            Debug.Log(growth.maxExp);
        });
    }

}

