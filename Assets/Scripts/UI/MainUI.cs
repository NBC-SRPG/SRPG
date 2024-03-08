using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainUI : MonoBehaviour
{
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

    private void OnClickCharacterBtn()
    {
        Debug.Log("OnClickCharacterBtn");
        // TODO
        // 버튼 클릭 사운드 출력 (SoundManager)
        // CharacterUI 생성 (UIManager)
    }
    private void OnClickOrganizationBtn()
    {
        Debug.Log("OnClickOrganizationBtn");
    }
    private void OnClickInventoryBtn()
    {
        Debug.Log("OnClickInventoryBtn");
    }
    private void OnClickGachaBtn()
    {
        Debug.Log("OnClickGachaBtn");
    }
    private void OnClickShopBtn()
    {
        Debug.Log("OnClickShopBtn");
    }
    private void OnClickAdventureBtn()
    {
        Debug.Log("OnClickAdventureBtn");
    }
    private void OnClickFriendBtn()
    {
        Debug.Log("OnClickFriendBtn");
    }
    private void OnClickMailBtn()
    {
        Debug.Log("OnClickMailBtn");
    }
    private void OnClickNoticeBtn()
    {
        Debug.Log("OnClickNoticeBtn");
    }
    private void OnClickMissionBtn()
    {
        Debug.Log("OnClickMissionBtn");
    }
    private void OnClickProfileBtn()
    {
        Debug.Log("OnClickProfileBtn");
    }
    private void OnClickSettingBtn()
    {
        Debug.Log("OnClickSettingBtn");
    }
}
