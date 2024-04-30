using TMPro;
using UnityEngine;

public class ProfileUI : UIBase
{
    private enum Texts
    {
        UIDText,
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
        CharacterProfileButton,
        EditNicknameButton,
        EditBirthdayButton,
        EditComentButton,
        BackButton
    }

    private enum Images
    {
        CharacterProfileImage,
        IllustrationImage,
        PlayerLevelFrontImage
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

        Get<TMP_InputField>((int)InputFields.NicknameInputField).onEndEdit.AddListener(ChangeNickname);
        Get<TMP_InputField>((int)InputFields.BirthdayInputField).onEndEdit.AddListener(ChangeBirthday);
        Get<TMP_InputField>((int)InputFields.ComentInputField).onEndEdit.AddListener(ChangeComent);

        GetText((int)Texts.UIDText).text = $"UID:{Managers.AccountData.playerData.uId}";
        GetText((int)Texts.LevelText).text = $"LV. {Managers.AccountData.playerData.Level}";
        GetText((int)Texts.ExpText).text = $"{Managers.AccountData.playerData.exp} / {Managers.AccountData.playerData.maxExp}";
        GetImage((int)Images.PlayerLevelFrontImage).fillAmount = (float)Managers.AccountData.playerData.exp / (float)Managers.AccountData.playerData.maxExp;

        Get<TMP_InputField>((int)InputFields.NicknameInputField).text = $"{Managers.AccountData.playerData.playerName}";
        Get<TMP_InputField>((int)InputFields.BirthdayInputField).text = $"{Managers.AccountData.playerData.birthday}";
        Get<TMP_InputField>((int)InputFields.ComentInputField).text = $"{Managers.AccountData.playerData.playerComment}";

        GetImage((int)Images.CharacterProfileImage).sprite = Managers.AccountData.characterData[Managers.AccountData.playerData.lobbyCharacter].SO.icon;
        GetImage((int)Images.IllustrationImage).sprite = Managers.AccountData.characterData[Managers.AccountData.playerData.lobbyCharacter].SO.standingImage;

    }

    private void OnClickEditNicknameButton()
    {
        Debug.Log("OnClickEditNicknameButton");

        Get<TMP_InputField>((int)InputFields.NicknameInputField).interactable = true;
        Get<TMP_InputField>((int)InputFields.NicknameInputField).Select();
    }
    private void OnClickEditBirthdayButton()
    {
        Debug.Log("OnClickEditBirthdayButton");

        Get<TMP_InputField>((int)InputFields.BirthdayInputField).interactable = true;
        Get<TMP_InputField>((int)InputFields.BirthdayInputField).Select();
    }
    private void OnClickEditComentButton()
    {
        Debug.Log("OnClickEditComentButton");

        Get<TMP_InputField>((int)InputFields.ComentInputField).interactable = true;
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
            Debug.Log("New nickname: " + newNickname);

            // 닉네임 가능 글자 수 체크
            if (Managers.AccountData.playerData.SetPlayerName(newNickname) == false)
            {
                // 불가능하면 경고 UI 생성
                Managers.UI.ShowUI<WarningUI>().Init("8글자 이하의 닉네임만 가능합니다.");

                // 설정 전 닉네임으로 변경
                Get<TMP_InputField>((int)InputFields.NicknameInputField).text = Managers.AccountData.playerData.playerName;
            }

            // 닉네임이 가능하다면 SetPlayerName()의 내부에서 업데이트
        }

        Get<TMP_InputField>((int)InputFields.NicknameInputField).interactable = false;
    }
    private void ChangeBirthday(string newBirthday)
    {
        if (!string.IsNullOrEmpty(newBirthday))
        {
            // TODO
            // 계정 데이터의 생일 업데이트
            // 올바른 날짜형식인지 체크 필요
            Debug.Log("New birthday: " + newBirthday);


            // 생일이 가능한지 체크
            if (Managers.AccountData.playerData.SetBirthDay(newBirthday) == false)
            {
                // 불가능하면 경고 UI 생성
                Managers.UI.ShowUI<WarningUI>().Init("올바른 날짜 형식이 아닙니다.");

                // 설정 전 생일로 변경
                Get<TMP_InputField>((int)InputFields.BirthdayInputField).text = Managers.AccountData.playerData.birthday;
            }

            // 생일이 가능하다면 SetBirthDay()의 내부에서 업데이트
        }

        Get<TMP_InputField>((int)InputFields.BirthdayInputField).interactable = false;
    }
    private void ChangeComent(string newComent)
    {
        if (!string.IsNullOrEmpty(newComent))
        {
            Debug.Log("New coment: " + newComent);

            // 코멘트 가능 글자 수 체크
            if (Managers.AccountData.playerData.SetPlayerComment(newComent) == false)
            {
                // 불가능하면 경고 UI 생성
                Managers.UI.ShowUI<WarningUI>().Init("40글자 이하의 코멘트만 가능합니다.");

                // 설정 전 코멘트로 변경
                Get<TMP_InputField>((int)InputFields.ComentInputField).text = Managers.AccountData.playerData.playerComment;
            }

            // 코멘트가 가능하다면 SetPlayerComment()의 내부에서 업데이트
        }

        Get<TMP_InputField>((int)InputFields.ComentInputField).interactable = false;
    }
}
