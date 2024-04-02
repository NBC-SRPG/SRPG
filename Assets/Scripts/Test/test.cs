using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[Serializable]
public class test : MonoBehaviour
{
    Database db;
    public CharacterSO so;

    void Start()
    {
        db = new Database();
    }
    
    public void OnClick()
    {
        db.WriteWithJson(db.userDB.Child("characterData").Child(so.id.ToString()), so);
    }

}

