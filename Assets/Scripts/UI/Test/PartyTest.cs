using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartyTest : MonoBehaviour
{
    public Character character;

    public void Onclick(int n)
    {
        if (Managers.GameManager.player.party[n] == null)
        {
            Managers.GameManager.player.party[n] = character;
            EventSystem.current.currentSelectedGameObject.transform.GetComponent<Image>().color = Color.green;
        }
        else
        {
            Managers.GameManager.player.party[n] = null;
            EventSystem.current.currentSelectedGameObject.transform.GetComponent<Image>().color = Color.white;
        }
    }

    public void ClickStart()
    {
        Managers.GameManager.player.prioty = 10;
        SceneManager.LoadScene("SCY_BattleTest 1");
    }
}
