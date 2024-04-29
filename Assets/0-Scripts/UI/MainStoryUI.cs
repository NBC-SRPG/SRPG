using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStoryUI : UIBase
{
    private enum Buttons
    {
        BackButton,
        Chapter1Button
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.Chapter1Button).onClick.AddListener(() => OnClickChapterButton(1000100));
    }

    private void OnClickBackButton()
    {
        Managers.UI.CloseUI(this);
    }

    private void OnClickChapterButton(int chapter)
    {
        Debug.Log($"OnClickChapterButton: {chapter}");

        StageSelectUI ui = Managers.UI.ShowUI<StageSelectUI>();
        ui.Init(chapter);
    }
}
