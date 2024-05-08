using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Dialog[] dialogs;

    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private TextMeshProUGUI dialogText;

    [SerializeField]
    private Image characterSprite;

    private int currentIndex = -1;
    private bool isTypingEffect = false;

    public void SetDialog()
    {
        SetNextDialog();
    }

    public bool UpdateDialog()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (isTypingEffect)
            {
                StopCoroutine(nameof(TypingText));
                isTypingEffect = false;

                dialogText.text = dialogs[currentIndex].dialog;

                return false;
            }

            if(dialogs.Length > currentIndex + 1)
            {
                SetNextDialog();
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        currentIndex++;

        characterName.text = dialogs[currentIndex].characterName;
        if (dialogs[currentIndex].spriteName.Equals(""))
        {
            characterSprite.gameObject.SetActive(false);
        }
        else
        {
            characterSprite.gameObject.SetActive(true);
        }

        StartCoroutine(nameof(TypingText));

    }

    private IEnumerator TypingText()
    {
        int index = 0;

        isTypingEffect = true;

        dialogText.text = "";

        for(int i = 0; i < dialogs[currentIndex].dialog.Length; i++)
        {
            dialogText.text += dialogs[currentIndex].dialog[i];

            index++;

            yield return new WaitForSeconds(0.2f);
        }

        isTypingEffect = false;
    }
}
