using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureUI : UIBase
{
    private enum Buttons
    {
        PVPButton,
        SpecialButton,
        MainStoryButton,
        BackButton
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PVPButton).onClick.AddListener(OnClickPVPButton);
        GetButton((int)Buttons.SpecialButton).onClick.AddListener(OnClickSpecialButtonButton);
        GetButton((int)Buttons.MainStoryButton).onClick.AddListener(OnClickMainStoryButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
    }

    private void OnClickPVPButton()
    {
        Debug.Log("OnClickPVPButton");
    }
    private void OnClickSpecialButtonButton()
    {
        Debug.Log("OnClickSpecialButtonButton");
    }
    private void OnClickMainStoryButton()
    {
        Debug.Log("OnClickMainStoryButton");

        Managers.UI.ShowUI<MainStoryUI>();
        //----------------------------------------------
        //임시로 테스트씬으로 넘기기
        // SceneManager.LoadScene("SCY_AITest");
        //----------------------------------------------
    }
    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        Managers.UI.CloseUI(this);
    }
}
