using System.Collections.Generic;
using UnityEngine;

public class CharacterListPopUpUI : UIBase
{
    private List<CharacterIconUI> characterIcons = new List<CharacterIconUI>();
    private CharacterIconUI selectedIcon;

    private enum Buttons
    {
        CloseButton,
        CheckButton
    }

    private enum GameObjects
    {
        Content
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).onClick.AddListener(OnClickCloseButton);
        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);

        foreach (var character in Managers.AccountData.characterData.Values)
        {
            GameObject go = Managers.Resource.Instantiate("UI/CharacterIconUI", GetObject((int)GameObjects.Content).transform);
            CharacterIconUI iconUI = go.GetComponent<CharacterIconUI>();
            iconUI.Init(character);

            if (character.SO.id == Managers.AccountData.playerData.lobbyCharacter)
            {
                selectedIcon = iconUI;
                selectedIcon.SetOutline(true);
            }

            characterIcons.Add(iconUI);
        }
    }

    private void OnClickCloseButton()
    {
        Managers.UI.CloseUI(this);
    }

    private void OnClickCheckButton()
    {
        Managers.AccountData.playerData.SetLobbyCharacter(selectedIcon.character.SO.id);
        Managers.UI.CloseUI(this);
    }

    public void SelectCharacter(CharacterIconUI iconUI)
    {
        // 이미 선택 된 아이콘 아웃라인 해제
        if (selectedIcon != null)
        {
            selectedIcon.SetOutline(false);
        }

        selectedIcon = iconUI;
        // 선택한 아이콘 아웃라인
        selectedIcon.SetOutline(true);
    }
}