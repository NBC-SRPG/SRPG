using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonUI : UIBase
{
    // 메인 -> 캐릭터 -> 클릭 에서는 정보창이 나와야 함
    // 메인 -> 편성 -> 캐릭터 -> 클릭에서는 편성에 들어가야 함
    // 편성에서 클릭한 것인지 체크 변수
    public static bool isFormation;
    public static int formationIndex;
    // 캐릭터 정보
    public Character character;

    private enum Texts
    {
        CharacterNameText,
        CharacterLevelText
    }
    // CharacterButton 이름이 겹침 -> 변경 or private니 그냥 넘김
    private enum Buttons
    {
        CharacterButton
    }
    
    private enum Images
    {
        CharacterImage
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //Managers.UI.SetCanvas(gameObject, false);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickButton);

        // TODO
        // 캐릭터 정보에서 이미지나 이름 레벨등을 꺼내와서 세팅
        // 테스트 데이터
        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>(character.characterData.character_Id);
        GetText((int)Texts.CharacterLevelText).text = $"{character.level} / {character.maxLevel}";
        GetText((int)Texts.CharacterNameText).text = $"{character.characterData.characterName}";
    }

    private void OnClickButton()
    {
        Debug.Log("OnClickButton");
        //Managers.Sound.Play(Constants.Sound.Effect, "ButtonClick");
        if (isFormation)
        {
            AddCharacterToFormation();
        }
        else
        {
            ShowCharacterInfo();
        }
    }

    // 캐릭터 정보창 보여주기
    private void ShowCharacterInfo()
    {
        Debug.Log("ShowCharacterInfo");

        CharacterInfoUI ui = Managers.UI.ShowUI<CharacterInfoUI>();

        ui.SetCharacter(character);
    }
    // 편성에 추가하기
    private void AddCharacterToFormation()
    {
        Debug.Log("AddCharacterToFormation");
        // TODO
        // 편성에 추가
        // 편성에 어떻게 추가하지? -> 버튼을 누르면 바로 추가 or 버튼 누르면 정보창이 뜨고 거기에서 추가
        // 이미 편성에 추가된 캐릭터는 추가 할 수 없음을 보여주어야 함

        // 편성 UI 뽑아서 formationIndex에 해당하는 곳에 캐릭터 정보 전달
        FormationUI ui = Managers.UI.FindUI<FormationUI>();
        Managers.AccountData.formationData[ui.presetIndex][formationIndex] = character;
        ui.UpdateFormationMember(formationIndex);

        Managers.UI.CloseUI(Managers.UI.PeekUI<CharacterUI>());
    }
}
