using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUI : UIBase
{
    private enum Texts
    {
        NicknameText,
        UIDText,
        BirthdayText,
        ComentText,
        LevelText,
        ExpText,
        FriendText
    }

    private enum InputFields
    {
        NicknameInputField,
        BirthdayInputField,
        ComentInputField
    }
    
    private enum Buttons
    {
        EditNicknameButton,
        EditBirthdayButton,
        EditComentButton,
        BackButton
    }

    private enum Images
    {
        IconImage,
        IllustrationImage
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
        Bind<TMP_InputField>(typeof(InputFields));

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.EditNicknameButton).onClick.AddListener(OnClickEditNicknameButton);
        GetButton((int)Buttons.EditBirthdayButton).onClick.AddListener(OnClickEditBirthdayButton);
        GetButton((int)Buttons.EditComentButton).onClick.AddListener(OnClickEditComentButton);
        Get<TMP_InputField>((int)InputFields.NicknameInputField).gameObject.SetActive(false);
        Get<TMP_InputField>((int)InputFields.BirthdayInputField).gameObject.SetActive(false);
        Get<TMP_InputField>((int)InputFields.ComentInputField).gameObject.SetActive(false);
        Get<TMP_InputField>((int)InputFields.NicknameInputField).onEndEdit.AddListener(ChangeNickname);
        Get<TMP_InputField>((int)InputFields.BirthdayInputField).onEndEdit.AddListener(ChangeBirthday);
        Get<TMP_InputField>((int)InputFields.ComentInputField).onEndEdit.AddListener(ChangeComent);

        GetText((int)Texts.BirthdayText).text = Managers.AccountData.playerData.birthday;
    }

    private void OnClickEditNicknameButton()
    {
        Debug.Log("OnClickEditNicknameButton");

        Get<TMP_InputField>((int)InputFields.NicknameInputField).gameObject.SetActive(true);
        Get<TMP_InputField>((int)InputFields.NicknameInputField).Select();
    }
    private void OnClickEditBirthdayButton()
    {
        Debug.Log("OnClickEditBirthdayButton");

        Get<TMP_InputField>((int)InputFields.BirthdayInputField).gameObject.SetActive(true);
        Get<TMP_InputField>((int)InputFields.BirthdayInputField).Select();
    }
    private void OnClickEditComentButton()
    {
        Debug.Log("OnClickEditComentButton");

        Get<TMP_InputField>((int)InputFields.ComentInputField).gameObject.SetActive(true);
        Get<TMP_InputField>((int)InputFields.ComentInputField).Select();
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        Managers.UI.CloseUI(this);
    }

    private void ChangeNickname(string newNickname)
    {
        if (!string.IsNullOrEmpty(newNickname))
        {
            // TODO
            // 계정 데이터의 닉네임 업데이트
            Debug.Log("New nickname: " + newNickname);

            GetText((int)Texts.NicknameText).text = newNickname;
            Get<TMP_InputField>((int)InputFields.NicknameInputField).gameObject.SetActive(false);
        }
    }
    private void ChangeBirthday(string newBirthday)
    {
        if (!string.IsNullOrEmpty(newBirthday))
        {
            // TODO
            // 계정 데이터의 생일 업데이트
            // 올바른 날짜형식인지 체크 필요
            Debug.Log("New birthday: " + newBirthday);

            GetText((int)Texts.BirthdayText).text = newBirthday;
            Get<TMP_InputField>((int)InputFields.BirthdayInputField).gameObject.SetActive(false);
        }
    }
    private void ChangeComent(string newComent)
    {
        if (!string.IsNullOrEmpty(newComent))
        {
            // TODO
            // 계정 데이터의 코멘트 업데이트
            Debug.Log("New coment: " + newComent);

            GetText((int)Texts.ComentText).text = newComent;
            Get<TMP_InputField>((int)InputFields.ComentInputField).gameObject.SetActive(false);
        }
    }
}
