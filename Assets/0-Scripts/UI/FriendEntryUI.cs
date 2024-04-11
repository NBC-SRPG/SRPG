using UnityEngine;
using static Constants;

public class FriendEntryUI : UIBase
{
    private string uId;
    private enum Texts
    {
        NameText,
        LevelText
    }

    private enum Images
    {
        IconImage
    }

    private enum Buttons
    {
        SupportCharacterButton,
        DeleteButton,
        CancelButton,
        ApprovalButton,
        RefuseButton
    }

    private enum GameObjects
    {

    }

    public void Init(int FriendTab, string uId)
    {
        this.uId = uId;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        switch (FriendTab)
        {
            case FriendTabs:
                GetButton((int)Buttons.SupportCharacterButton).onClick.AddListener(OnClickSupportCharacterButton);
                GetButton((int)Buttons.DeleteButton).onClick.AddListener(OnClickDeleteButton);

                GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
                GetButton((int)Buttons.ApprovalButton).gameObject.SetActive(false);
                GetButton((int)Buttons.RefuseButton).gameObject.SetActive(false);
                break;

            case ApplyingTabs:
                GetButton((int)Buttons.CancelButton).onClick.AddListener(OnClickCancelButton);

                GetButton((int)Buttons.SupportCharacterButton).gameObject.SetActive(false);
                GetButton((int)Buttons.DeleteButton).gameObject.SetActive(false);
                GetButton((int)Buttons.ApprovalButton).gameObject.SetActive(false);
                GetButton((int)Buttons.RefuseButton).gameObject.SetActive(false);
                break;

            case WaitingTabs:
                GetButton((int)Buttons.ApprovalButton).onClick.AddListener(OnClickApprovalButton);
                GetButton((int)Buttons.RefuseButton).onClick.AddListener(OnClickRefuseButton);

                GetButton((int)Buttons.SupportCharacterButton).gameObject.SetActive(false);
                GetButton((int)Buttons.DeleteButton).gameObject.SetActive(false);
                GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
                break;
        }
    }
    private void OnClickSupportCharacterButton()
    {
        Debug.Log("OnClickSupportCharacterButton");

        // TODO
        // 지원 캐릭터 설정
    }
    private void OnClickDeleteButton()
    {
        Debug.Log("OnClickDeleteButton");

        // 친구 목록에서 삭제
        Managers.AccountData.friendData[FriendTabs].Remove(uId);

        // TODO
        // 상대방 친구 목록에서 삭제
        // 엔트리 파괴
        // UI 업데이트
    }
    private void OnClickCancelButton()
    {
        Debug.Log("OnClickCancelButton");

        // 친구 신청중에서 삭제
        Managers.AccountData.friendData[WaitingTabs].Remove(uId);

        // TODO
        // 상대방 승인 대기중에서 삭제
        // 엔트리 파괴
        // UI 업데이트
    }
    private void OnClickApprovalButton()
    {
        Debug.Log("OnClickApprovalButton");

        // 승인 대기에서 삭제
        Managers.AccountData.friendData[WaitingTabs].Remove(uId);
        // 친구 목록에 추가
        Managers.AccountData.friendData[FriendTabs].Add(uId);

        // TODO
        // 상대방 친구 목록에 추가
        // 엔트리 파괴
        // UI 업데이트
    }
    private void OnClickRefuseButton()
    {
        Debug.Log("OnClickRefuseButton");

        // 승인 대기에서 삭제
        Managers.AccountData.friendData[WaitingTabs].Remove(uId);

        // TODO
        // 상대방 친구 신청에서 삭제
        // 엔트리 파괴
        // UI 업데이트
    }
}
