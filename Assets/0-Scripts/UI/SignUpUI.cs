using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignUpUI : UIBase
{
    private enum Texts
    {
        WarningText
    }

    private enum InputFields
    {
        NicknameInputField,
    }

    private enum Buttons
    {
        ConfirmButton,
        EditNicknameButton
    }

    public void Init()
    {
        Managers.AccountData.AcquireCharacter(3);
        Managers.AccountData.SetFormationCharacter(0,0,3);
        createSupportMail();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));

        GetButton((int)Buttons.EditNicknameButton).onClick.AddListener(OnClickEditNicknameButton);
        Get<TMP_InputField>((int)InputFields.NicknameInputField).onEndEdit.AddListener(SetNickname);

        GetButton((int)Buttons.ConfirmButton).onClick.AddListener(TutorialStart);
    }


    private void OnClickEditNicknameButton()
    {
        Debug.Log("OnClickEditNicknameButton");

        Get<TMP_InputField>((int)InputFields.NicknameInputField).interactable = true;
        Get<TMP_InputField>((int)InputFields.NicknameInputField).Select();
    }

    private void SetNickname(string newNickname)
    {
        if (!string.IsNullOrEmpty(newNickname))
        {
            Debug.Log("New nickname: " + newNickname);

            // 닉네임 가능 글자 수 체크
            if (Managers.AccountData.playerData.SetPlayerName(newNickname) == false)
            {
                // 불가능하면 경고 UI 생성
                WarningUI warningUi = Managers.UI.ShowUI<WarningUI>();
                warningUi.Init("8글자 이하의 닉네임만 가능합니다.");

                // 설정 전 닉네임으로 변경
                Get<TMP_InputField>((int)InputFields.NicknameInputField).text = "";
            }

            // 닉네임이 가능하다면 SetPlayerName()의 내부에서 업데이트
        }

        Get<TMP_InputField>((int)InputFields.NicknameInputField).interactable = false;
    }

    private void TutorialStart()
    {
        Managers.GameManager.UpdateParty(Managers.AccountData.formationData[0]);
        Managers.GameManager.thisStage = Utility.Id2SOWait<StageSO>(1000101);
        BattleLoadingController.LoadBattle("BattleScene");
    }

    private void createSupportMail()
    {
        MailSO mail = new MailSO();
        mail.key = "BetaSupport";
        mail.title = "베타테스터를 위한 선물";
        mail.rewards = new();
        mail.ap = 0;
        mail.gold = 10000000;
        mail.diamond = 1000000;
        mail.dateSent = DateTime.Now;
        mail.expiration = 30;

        Managers.DB.WriteWithJson(Managers.DB.userDB.Child("mailBox").Child(mail.key), mail);

    }

}
