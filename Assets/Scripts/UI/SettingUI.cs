using UnityEngine;

public class SettingUI : UIBase
{
    private PlayTab playTab = PlayTab.None;
    private enum PlayTab
    {
        None,
        Game,
        Graphic,
        Sound,
        Notice,
        AccountSetting,
        Language
    }
    private enum Buttons
    {
        GameButton,
        GraphicButton,
        SoundButton,
        NoticeButton,
        AccountSettingButton,
        LanguageButton,
        BackButton
    }
    private enum GameObjects
    {
        GameTab,
        GraphicTab,
        SoundTab,
        NoticeTab,
        AccountSettingTab,
        LanguageTab
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.GameButton).onClick.AddListener(() => ShowTab(PlayTab.Game));
        GetButton((int)Buttons.GraphicButton).onClick.AddListener(() => ShowTab(PlayTab.Graphic));
        GetButton((int)Buttons.SoundButton).onClick.AddListener(() => ShowTab(PlayTab.Sound));
        GetButton((int)Buttons.NoticeButton).onClick.AddListener(() => ShowTab(PlayTab.Notice));
        GetButton((int)Buttons.AccountSettingButton).onClick.AddListener(() => ShowTab(PlayTab.AccountSetting));
        GetButton((int)Buttons.LanguageButton).onClick.AddListener(() => ShowTab(PlayTab.Language));

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        ShowTab(PlayTab.Game);
    }

    // 설정 버튼을 눌렀을 때 해당하는 탭 보여주기
    private void ShowTab(PlayTab tab)
    {
        // 이미 탭에 열려있는 설정을 누르면 아무것도 하지않음
        if (playTab == tab)
            return;
        
        // 현재 열려있는 탭 업데이트
        playTab = tab;
        
        // 세팅 초기화
        // 모든 탭 끄기
        // 프리팹에서는 모든 탭이 켜져있어야 Bind 가능
        GetObject((int)GameObjects.GameTab).SetActive(false);
        GetObject((int)GameObjects.GraphicTab).SetActive(false);
        GetObject((int)GameObjects.SoundTab).SetActive(false);
        GetObject((int)GameObjects.NoticeTab).SetActive(false);
        GetObject((int)GameObjects.AccountSettingTab).SetActive(false);
        GetObject((int)GameObjects.LanguageTab).SetActive(false);

        // TODO
        // 모든 버튼 이미지 초기화

        switch (playTab)
        {
            case PlayTab.Game:
                // TODO
                // 버튼 눌리는 효과음 재생
                // 해당 탭 활성화
                GetObject((int)GameObjects.GameTab).SetActive(true);
                // 해당 버튼 이미지 변경 (클릭한 버튼임을 보여주기)
                break;

            case PlayTab.Graphic:

                GetObject((int)GameObjects.GraphicTab).SetActive(true);
                break;

            case PlayTab.Sound:

                GetObject((int)GameObjects.SoundTab).SetActive(true);
                break;

            case PlayTab.Notice:

                GetObject((int)GameObjects.NoticeTab).SetActive(true);
                break;

            case PlayTab.AccountSetting:

                GetObject((int)GameObjects.AccountSettingTab).SetActive(true);
                break;

            case PlayTab.Language:

                GetObject((int)GameObjects.LanguageTab).SetActive(true);
                break;
        }
    }
    
    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }
}
