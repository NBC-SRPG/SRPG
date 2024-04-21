using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BufIcon : MonoBehaviour
{
    [SerializeField] private Image bufIcon;
    [SerializeField] private TextMeshProUGUI stack;

    [SerializeField] private GameObject positive;
    [SerializeField] private GameObject negative;

    public void SetBufIcon(CharacterBuf buf)
    {
        if(buf == null || buf.IsDestroyed)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        //bufIcon = 
        stack.text = buf.duration.ToString();

        if(buf.BufType == BattleKeyWords.BufType.Positive)
        {
            positive.SetActive(true);
            negative.SetActive(false);
        }
        else
        {
            positive.SetActive(false);
            negative.SetActive(true);
        }
    }
}
