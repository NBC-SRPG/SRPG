using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class FriendUI : UIBase
{
    private PlayTab playTab = PlayTab.None;

    private enum PlayTab
    {
        None,
        FriendList,
        FriendRequest,
        AwaitingApproval,
        SearchFriend
    }
    private enum Texts
    {

    }
    private enum Buttons
    {
        FriendListButton,
        FriendRequestButton,
        AwaitingApprovalButton,
        SearchFriendButton,
        BackButton
    }
    private enum Images
    {

    }
    private enum GameObjects
    {
        FriendListTab,
        FriendRequestTab,
        AwaitingApprovalTab,
        SearchFriendTab
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.FriendListButton).onClick.AddListener(() => ShowTab(PlayTab.FriendList));
        GetButton((int)Buttons.FriendRequestButton).onClick.AddListener(() => ShowTab(PlayTab.FriendRequest));
        GetButton((int)Buttons.AwaitingApprovalButton).onClick.AddListener(() => ShowTab(PlayTab.AwaitingApproval));
        GetButton((int)Buttons.SearchFriendButton).onClick.AddListener(() => ShowTab(PlayTab.SearchFriend));

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        ShowTab(PlayTab.FriendList);
    }

    private void ShowTab(PlayTab tab)
    {
        // 이미 탭에 열려있는 정보를 누르면 아무것도 하지않음
        if (playTab == tab)
            return;

        // 현재 열려있는 탭 업데이트
        playTab = tab;

        // 모든 탭 끄기
        GetObject((int)GameObjects.FriendListTab).SetActive(false);
        GetObject((int)GameObjects.FriendRequestTab).SetActive(false);
        GetObject((int)GameObjects.AwaitingApprovalTab).SetActive(false);
        GetObject((int)GameObjects.SearchFriendTab).SetActive(false);

        // TODO
        // 모든 버튼 이미지 초기화

        switch (playTab)
        {
            case PlayTab.FriendList:
                // TODO
                // 버튼 눌리는 효과음 재생
                // 해당 탭 활성화
                GetObject((int)GameObjects.FriendListTab).SetActive(true);
                // 해당 버튼 이미지 변경 (클릭한 버튼임을 보여주기)
                break;

            case PlayTab.FriendRequest:

                GetObject((int)GameObjects.FriendRequestTab).SetActive(true);
                break;

            case PlayTab.AwaitingApproval:

                GetObject((int)GameObjects.SearchFriendTab).SetActive(true);
                break;

            case PlayTab.SearchFriend:

                GetObject((int)GameObjects.SearchFriendTab).SetActive(true);
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
