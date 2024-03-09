using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainUI : MonoBehaviour
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
        // TODO
        // 버튼 리스트를 받아와서 클릭 이벤트 추가
    }

    public void RefreshUI()
    {
        // TODO
        // 메인화면의 UI 업데이트
        // AP회복, 골드 획득, 다이아 충전, 레벨업 등
    }

    private void OnClickCharacterButton()
    {
        Debug.Log("OnClickCharacterButton");
        // TODO
        // 버튼 클릭 사운드 출력 (SoundManager)
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
