using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : UIBase
{
    public DialogSystem dialogSystem;

    public bool nowShowDialog;

    private enum GameObjects
    {
        DialogObject,

    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        BindObject(typeof(GameObjects));

        dialogSystem = GetComponent<DialogSystem>();

        CloseDialog();
    }

    public void StartDialog()
    {
        GetObject((int)GameObjects.DialogObject).SetActive(true);
        dialogSystem.SetDialog();
        nowShowDialog = true;
    }

    public bool UpdateDialog()
    {
        bool isCompleted = dialogSystem.UpdateDialog();

        if(isCompleted)
        {
            CloseDialog();
        }

        return isCompleted;
    }

    public void CloseDialog()
    {
        GetObject((int)GameObjects.DialogObject).SetActive(false);
        nowShowDialog = false;
    }
}
