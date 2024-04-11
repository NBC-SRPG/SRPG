using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendEntryUI : UIBase
{
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

    public void Init(int FriendTab)
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindButton(typeof(GameObjects));

        switch (FriendTab)
        {
            case Constants.FriendTabs:
                GetButton((int)Buttons.SupportCharacterButton).onClick.AddListener(OnClickSupportCharacterButton);
                GetButton((int)Buttons.DeleteButton).onClick.AddListener(OnClickDeleteButton);

                GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
                GetButton((int)Buttons.ApprovalButton).gameObject.SetActive(false);
                GetButton((int)Buttons.RefuseButton).gameObject.SetActive(false);
                break;

            case Constants.ApplyingTabs:
                GetButton((int)Buttons.CancelButton).onClick.AddListener(OnClickCancelButton);

                GetButton((int)Buttons.SupportCharacterButton).gameObject.SetActive(false);
                GetButton((int)Buttons.DeleteButton).gameObject.SetActive(false);
                GetButton((int)Buttons.ApprovalButton).gameObject.SetActive(false);
                GetButton((int)Buttons.RefuseButton).gameObject.SetActive(false);
                break;

            case Constants.WaitingTabs:
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

        // TODO
        // 친구 목록에서 삭제
        // 상대방 친구 목록에서 삭제
    }
    private void OnClickCancelButton()
    {
        Debug.Log("OnClickCancelButton");

        // TODO
        // 친구 신청중에서 삭제
        // 상대방 승인 대기중에서 삭제
    }
    private void OnClickApprovalButton()
    {
        Debug.Log("OnClickApprovalButton");

        // TODO
        // 승인 대기에서 삭제
        //Managers.AccountData.friendData[Constants.WaitingTabs]
        // 친구 목록에 추가
        // 상대방 친구 목록에 추가
    }
    private void OnClickRefuseButton()
    {
        Debug.Log("OnClickRefuseButton");

        // TODO
        // 승인 대기에서 삭제
        // 상대방 친구 신청에서 삭제
    }
}
