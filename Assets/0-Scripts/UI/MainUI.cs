using System.Collections;
using UnityEngine;
public class MainUI : UIBase
{
    private enum Texts
    {
        NameText,
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
        IllustrationImage,
        ProfileImage
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Managers.AccountData.playerData.OnPlayerLevelChanged -= RefreshLevel;
        Managers.AccountData.playerData.OnPlayerNameChanged -= RefreshName;
        Managers.AccountData.playerData.OnLobbyCharacterChanged -= RefreshCharacter;
    }

    public void Init()
    {
        Managers.Sound.Play(Constants.Sound.Bgm, "BGM/MainBGM");
        // UI 내의 텍스트, 버튼, 이미지, 오브젝트 바인딩
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        // 버튼에 클릭 이벤트 추가
        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickCharacterButton);
        GetButton((int)Buttons.FormationButton).onClick.AddListener(OnClickFormationButton);
        GetButton((int)Buttons.InventoryButton).onClick.AddListener(OnClickInventoryButton);
        GetButton((int)Buttons.GachaButton).onClick.AddListener(OnClickGachaButton);
        //GetButton((int)Buttons.ShopButton).onClick.AddListener(OnClickShopButton);
        GetButton((int)Buttons.AdventureButton).onClick.AddListener(OnClickAdventureButton);
        //GetButton((int)Buttons.FriendButton).onClick.AddListener(OnClickFriendButton);
        GetButton((int)Buttons.MailButton).onClick.AddListener(OnClickMailButton);
        //GetButton((int)Buttons.NoticeButton).onClick.AddListener(OnClickNoticeButton);
        GetButton((int)Buttons.MissionButton).onClick.AddListener(OnClickMissionButton);
        GetButton((int)Buttons.ProfileButtton).onClick.AddListener(OnClickProfileButton);
        

        RefreshUI();

        Managers.AccountData.playerData.OnPlayerLevelChanged += RefreshLevel;
        Managers.AccountData.playerData.OnPlayerNameChanged += RefreshName;
        Managers.AccountData.playerData.OnLobbyCharacterChanged += RefreshCharacter;
    }

    private void RefreshUI()
    {
        RefreshLevel(Managers.AccountData.playerData.Level);
        RefreshName(Managers.AccountData.playerData.playerName);
        RefreshCharacter(Managers.AccountData.playerData.lobbyCharacter);
    }

    // Level 텍스트 업데이트
    private void RefreshLevel(int newLevel)
    {
        GetText((int)Texts.LevelText).text = $"LV.{newLevel}";
    }

    private void RefreshName(string newName)
    {
        GetText((int)Texts.NameText).text = newName;
    }
    
    private void RefreshCharacter(int characterId)
    {
        GetImage((int)Images.ProfileImage).sprite = Managers.AccountData.characterData[characterId].SO.icon;
        GetImage((int)Images.IllustrationImage).sprite = Managers.AccountData.characterData[characterId].SO.standingImage;
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
        Managers.UI.ShowUI<InventoryUI>();
    }
    private void OnClickGachaButton()
    {
        Debug.Log("OnClickGachaButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<GachaUI>();
    }
    private void OnClickShopButton()
    {
        Debug.Log("OnClickShopButton");
        Managers.AccountData.playerData.AddGold(10000000);
        Managers.AccountData.playerData.AddDiamond(10000);
        Managers.AccountData.playerData.AddAP(50);

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<ShopUI>();
    }
    private void OnClickAdventureButton()
    {
        Debug.Log("OnClickAdventureButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<AdventureUI>();
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

        // 메일은 게임 진행 중에 새로 올 수도 있음
        // 메일을 열 때마다 데이터 받아오기
        StartCoroutine(ShowMailUI());
        //Managers.UI.ShowUI<MailUI>();
    }
    private IEnumerator ShowMailUI()
    {
        yield return Managers.DB.MailLoad();
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
        Managers.UI.ShowUI<MissionUI>();
    }
    private void OnClickProfileButton()
    {
        Debug.Log("OnClickProfileButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        Managers.UI.ShowUI<ProfileUI>();
    }
}
