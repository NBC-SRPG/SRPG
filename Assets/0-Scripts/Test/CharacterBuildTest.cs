using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class CharacterBuildTest : MonoBehaviour
{

    void Start()
    {

    }
    
    public void OnClick()
    {
        Managers.DB.Read(Managers.DB.userDB.Child("characterData"), (snapshot) =>
        {
            foreach (var character in snapshot.Children)
            {
                CharacterGrowth growth = JsonConvert.DeserializeObject<CharacterGrowth>(character.GetRawJsonValue());

                Utility.Id2SO<CharacterSO>(growth.id, (result) => 
                {
                    CharacterSO so = (CharacterSO)result;
                    Managers.AccountData.characterData.Add(growth.id, new Character(so, growth));
                });
                
            }
        });
    }

}

