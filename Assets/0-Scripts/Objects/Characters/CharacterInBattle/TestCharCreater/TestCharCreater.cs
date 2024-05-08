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

    public int startLevel = 1;

    [Range(-1, 1)] public int ability2 = -1;
    [Range(-1, 1)] public int ability3 = -1;
    [Range(-1, 1)] public int superClass = -1;

    public void Init()
    {
        growth = new CharacterGrowth(tempCharacter);
        growth.level = startLevel;

        growth.abilityT2 = ability2;
        growth.abilityT3 = ability3;

        growth.superiorClass = superClass;
    }
}

public class TestCharCreater : MonoBehaviour
{
    public List<TempGrowth> temp;
    public List<TempGrowth> enemyTemp;

    public List<Character> character = new List<Character>();
    public List<Character> enemy = new List<Character>();

    public StageSO stage;

    private DialogUI dialog;

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

        Managers.GameManager.nowTesting = true;

        Managers.GameManager.player.playerId = "test";

        dialog = Managers.UI.ShowUI<DialogUI>();
    }


    public void OnClickButton()
    {
        SceneManager.LoadScene("SCY_AITest");
    }

    public void OnClickButton2()
    {
        SceneManager.LoadScene("SCY_MapTest");
    }

    public void OnClickDialog()
    {
        dialog.StartDialog();
    }

    private void Update()
    {
        //if (dialog.nowShowDialog)
        //{
        //    dialog.UpdateDialog();
        //}
    }
}
