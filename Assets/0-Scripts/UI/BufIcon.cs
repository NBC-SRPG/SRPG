using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BufIcon : MonoBehaviour
{
    public Image bufIcon;
    public TextMeshProUGUI stack;

    public void SetBufIcon(CharacterBuf buf)
    {
        if(buf == null || buf.IsDestroyed)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        //bufIcon = 
        stack.text = buf.stack.ToString();
    }
}
