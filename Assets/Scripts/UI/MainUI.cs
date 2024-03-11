using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class MainUI : UIBase
{
    private enum Texts
    {
        LevelText,
        APText,
        GoldText,
        DiamondText
    }

    private enum Buttons
    {
        CharacterButton,
        OrganizationButton,
        InventoryButton,
        GachaButton,
        ShopButton,
        AdventureButton,
        FriendButton,
        MailButton,
        NoticeButton,
        MissionButton,
        ProfileButtton,
        SettingButton
    }

    private enum Images
    {

    }

    private enum GameObjects
    {

    }

    public void Init()
    {
        // UI 내의 텍스트, 버튼, 이미지, 오브젝트 바인딩
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        // 버튼에 클릭 이벤트 추가
        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickCharacterButton);
        GetButton((int)Buttons.OrganizationButton).onClick.AddListener(OnClickOrganizationButton);
        GetButton((int)Buttons.InventoryButton).onClick.AddListener(OnClickInventoryButton);
        GetButton((int)Buttons.GachaButton).onClick.AddListener(OnClickGachaButton);
        GetButton((int)Buttons.ShopButton).onClick.AddListener(OnClickShopButton);
        GetButton((int)Buttons.AdventureButton).onClick.AddListener(OnClickAdventureButton);
        GetButton((int)Buttons.FriendButton).onClick.AddListener(OnClickFriendButton);
        GetButton((int)Buttons.MailButton).onClick.AddListener(OnClickMailButton);
        GetButton((int)Buttons.NoticeButton).onClick.AddListener(OnClickNoticeButton);
        GetButton((int)Buttons.MissionButton).onClick.AddListener(OnClickMissionButton);
        GetButton((int)Buttons.ProfileButtton).onClick.AddListener(OnClickProfileButton);
        GetButton((int)Buttons.SettingButton).onClick.AddListener(OnClickSettingButton);

        RefreshUI();

        // BGM 재생 (SoundManager)
        // Managers.Sound(Sound.BGM, "BGM_Main");
    }

    private void RefreshUI()
    {
        RefreshLevel();
        RefreshAP();
        RefreshGold();
        RefreshDiamond();
    }
    // Level 텍스트 업데이트
    private void RefreshLevel()
    {

    }
    // AP 텍스트 업데이트
    private void RefreshAP()
    {

    }
    // Gold 텍스트 업데이트
    private void RefreshGold()
    {

    }
    // Diamond 텍스트 업데이트
    private void RefreshDiamond()
    {

    }

    private void OnClickCharacterButton()
    {
        Debug.Log("OnClickCharacterButton");
        // TODO
        // 버튼 클릭 사운드 출력 (SoundManager)
        // Managers.Sound(Sound.Effect, "ButtonClick");
        // CharacterUI 생성 (UIManager)
    }
    private void OnClickOrganizationButton()
    {
        Debug.Log("OnClickOrganizationButton");
    }
    private void OnClickInventoryButton()
    {
        Debug.Log("OnClickInventoryButton");
    }
    private void OnClickGachaButton()
    {
        Debug.Log("OnClickGachaButton");
    }
    private void OnClickShopButton()
    {
        Debug.Log("OnClickShopButton");
    }
    private void OnClickAdventureButton()
    {
        Debug.Log("OnClickAdventureButton");
    }
    private void OnClickFriendButton()
    {
        Debug.Log("OnClickFriendButton");
    }
    private void OnClickMailButton()
    {
        Debug.Log("OnClickMailButton");
    }
    private void OnClickNoticeButton()
    {
        Debug.Log("OnClickNoticeButton");
    }
    private void OnClickMissionButton()
    {
        Debug.Log("OnClickMissionButton");
    }
    private void OnClickProfileButton()
    {
        Debug.Log("OnClickProfileButton");
    }
    private void OnClickSettingButton()
    {
        Debug.Log("OnClickSettingButton");
    }
}
