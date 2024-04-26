using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BufIcon : MonoBehaviour
{
    [SerializeField] private Image bufIcon;
    [SerializeField] private TextMeshProUGUI stack;
    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private TextMeshProUGUI duration;

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

        bufIcon.sprite = Managers.Resource.Load<Sprite>("BufIcon/" + buf.Keyword);

        if (buf.stack > 0)
        {
            stack.gameObject.SetActive(true);
            stack.text = buf.stack.ToString();
        }
        else
        {
            stack.gameObject.SetActive(false);
        }

        if (buf.power > 0)
        {
            power.gameObject.SetActive(true);
            power.text = buf.power.ToString();
        }
        else
        {
            power.gameObject.SetActive(false);
        }

        if (buf.duration > 0)
        {
            duration.gameObject.SetActive(true);
            duration.text = buf.duration.ToString();
        }
        else
        {
            duration.gameObject.SetActive(false);
        }

        if (buf.BufType == BattleKeyWords.BufType.Positive)
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
