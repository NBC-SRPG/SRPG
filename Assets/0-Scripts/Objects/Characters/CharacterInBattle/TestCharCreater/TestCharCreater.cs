using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

[Serializable]
public class TempGrowth
{
    public CharacterGrowth growth = new CharacterGrowth();

    public CharacterSO tempCharacter;

    public void Init()
    {
        growth = new CharacterGrowth(tempCharacter);
    }
}

public class TestCharCreater : MonoBehaviour
{
    public List<TempGrowth> temp;
    public List<TempGrowth> enemyTemp;

    public List<Character> character = new List<Character>();
    public List<Character> enemy = new List<Character>();

    public StageSO stage;

    private void Awake()
    {
        foreach (TempGrowth tempGrowth in temp)
        {
            tempGrowth.Init();
            character.Add(new Character(tempGrowth.tempCharacter, tempGrowth.growth));
        }

        foreach (TempGrowth tempGrowth in enemyTemp)
        {
            tempGrowth.Init();
            enemy.Add(new Character(tempGrowth.tempCharacter, tempGrowth.growth));
        }

        Managers.GameManager.player.party = character.ToArray();

        Managers.GameManager.enemy.party = enemy.ToArray();

        Managers.GameManager.thisStage = stage;
    }


    public void OnClickButton()
    {
        SceneManager.LoadScene("SCY_AITest");
    }

}
