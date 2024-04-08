using System;
using UnityEngine;

[Serializable]
public class AddressableTest : MonoBehaviour
{
    Database db;
    int id;
    CharacterSO so;

    void Start()
    {
        db = new Database();
        id = 101;
    }
    
    public void OnClick()
    {
        Utility.Id2SO<CharacterSO>(id, (result) => 
        {
            so = (CharacterSO)result;

            Debug.Log(so.id);
        });
    }

}

