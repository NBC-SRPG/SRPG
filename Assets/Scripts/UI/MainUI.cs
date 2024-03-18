using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class MainUI : UIBase
{
    private enum Texts
    {
        LevelText
    }

    private enum Buttons
    {
        CharacterButton,
        FormationButton,
        InventoryButton,
        GachaButton,
        ShopButton,
        AdventureButton,
        FriendButton,
        MailButton,
        NoticeButton,
        MissionButton,
        ProfileButtton,
    }

    private enum Images
    {

    }

    private enum GameObjects
    {

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // TODO
        // AP가 MAX가 아니라면 타이머 작동
        // AP, Gold, Diamond를 각 UI마다 둘 것인지? or AP, Gold, Diamond만 있는 UI 생성 후 가장 위에 두기
    }

    public void Init()
    {
        Managers.UI.SetCanvas(gameObject);
        // UI 내의 텍스트, 버튼, 이미지, 오브젝트 바인딩
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        // 버튼에 클릭 이벤트 추가
        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickCharacterButton);
        GetButton((int)Buttons.FormationButton).onClick.AddListener(OnClickFormationButton);
        GetButton((int)Buttons.InventoryButton).onClick.AddListener(OnClickInventoryButton);
        GetButton((int)Buttons.GachaButton).onClick.AddListener(OnClickGachaButton);
        GetButton((int)Buttons.ShopButton).onClick.AddListener(OnClickShopButton);
        GetButton((int)Buttons.AdventureButton).onClick.AddListener(OnClickAdventureButton);
        GetButton((int)Buttons.FriendButton).onClick.AddListener(OnClickFriendButton);
        GetButton((int)Buttons.MailButton).onClick.AddListener(OnClickMailButton);
        GetButton((int)Buttons.NoticeButton).onClick.AddListener(OnClickNoticeButton);
        GetButton((int)Buttons.MissionButton).onClick.AddListener(OnClickMissionButton);
        GetButton((int)Buttons.ProfileButtton).onClick.AddListener(OnClickProfileButton);

        RefreshUI();

        // BGM 재생 (SoundManager)
        // Managers.Sound(Sound.BGM, "BGM_Main");
    }

    private void RefreshUI()
    {
        RefreshLevel();
    }

    // Level 텍스트 업데이트
    private void RefreshLevel()
    {

    }


    private void OnClickCharacterButton()
    {
        Debug.Log("OnClickCharacterButton");
        // TODO

        // 버튼 클릭 사운드 출력 (SoundManager)
        // Managers.Sound(Sound.Effect, "ButtonClick");

        // 메인UI -> 캐릭터UI 접근임을 알림
        CharacterEntryUI.isFormation = false;
        Managers.UI.ShowUI<CharacterUI>();
    }
    private void OnClickFormationButton()
    {
        Debug.Log("OnClickFormationButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<FormationUI>();
    }
    private void OnClickInventoryButton()
    {
        Debug.Log("OnClickInventoryButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<InventoryUI>();
    }
    private void OnClickGachaButton()
    {
        Debug.Log("OnClickGachaButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<GachaUI>();
    }
    private void OnClickShopButton()
    {
        Debug.Log("OnClickShopButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<ShopUI>();
    }
    private void OnClickAdventureButton()
    {
        Debug.Log("OnClickAdventureButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<AdventureUI>();
    }
    private void OnClickFriendButton()
    {
        Debug.Log("OnClickFriendButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<FriendUI>();
    }
    private void OnClickMailButton()
    {
        Debug.Log("OnClickMailButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<MailUI>();
    }
    private void OnClickNoticeButton()
    {
        Debug.Log("OnClickNoticeButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<NoticeUI>();
    }
    private void OnClickMissionButton()
    {
        Debug.Log("OnClickMissionButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<MissionUI>();
    }
    private void OnClickProfileButton()
    {
        Debug.Log("OnClickProfileButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<ProfileUI>();
    }
}
