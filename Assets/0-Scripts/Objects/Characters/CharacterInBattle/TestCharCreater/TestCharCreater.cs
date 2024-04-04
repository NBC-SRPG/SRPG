using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class TempGrowth
{
    public CharacterGrowth growth = new CharacterGrowth();

    public CharacterSO tempCharacter;

    public void Init()
    {
        growth.Init(tempCharacter);
    }
}


public class TestCharCreater : MonoBehaviour
{
    public List<TempGrowth> temp;

    public List<Character> character = new List<Character>();

    private void Awake()
    {
        foreach (TempGrowth tempGrowth in temp)
        {
            tempGrowth.Init();
            character.Add(new Character(tempGrowth.tempCharacter, tempGrowth.growth));
        }

        Managers.GameManager.player.party = character.ToArray();
    }
}
