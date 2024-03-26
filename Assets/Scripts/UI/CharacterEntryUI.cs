using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class CharacterEntryUI : UIBase
{
    // 편성에서 클릭한 것인지 체크 변수
    public static bool isFormation;
    // 편성의 몇번째 인덱스 클린한 것인지 체크 변수
    public static int formationIndex;
    // 캐릭터 id
    public int characterId;
    // 1초 이상 눌렀는지를 체크하는 변수
    private float pressedTimer;
    // 1초 이상 눌렀을 때 캐릭터 정보창이 켜져있는지 체크하는 변수
    private bool hasShownCharacterInfo;

    private enum Texts
    {
        CharacterLevelText
    }

    private enum Buttons
    {
        CharacterButton
    }
    
    private enum Images
    {
        CharacterImage,
        CharacterAttributeImage
    }
    
    private enum GameObjects
    {
        Star
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickButton);
        BindEvent(GetButton((int)Buttons.CharacterButton).gameObject, OnPointerUpCharacterButton, UIEvent.PointerUp);
        BindEvent(GetButton((int)Buttons.CharacterButton).gameObject, OnPressedCharacterButton, UIEvent.Pressed);

        // TODO
        // 캐릭터 정보에서 이미지나 이름 레벨등을 꺼내와서 세팅
        // 테스트 데이터
        GetImage((int)Images.CharacterImage).sprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.characterData[characterId].characterData.character_Id}");
        GetText((int)Texts.CharacterLevelText).text = $"Lv. {Managers.AccountData.characterData[characterId].characterGrowth.Level}";
        // TODO 속성 이미지 세팅
        // TODO 캐릭터 아웃라인 속성 이미지에 맞게 세팅

        SetStar();
    }

    private void SetStar()
    {
        // int numberOfStars = character.characterData.defaltStar; // 별의 개수
        int numberOfStars = 3; // 별의 개수 // 테스트 데이터
        float starWidth = 25f; // 별 이미지의 너비

        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject star = Managers.Resource.Instantiate("Star", GetObject((int)GameObjects.Star).transform);
            star.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            RectTransform rt = star.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(0f, 0f);
            rt.anchorMin = new Vector2(0f, 0f);
            rt.pivot = new Vector2(0f, 0f);
            rt.anchoredPosition = new Vector2(i * starWidth, 0);
        }
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

        ui.SetCharacter(Managers.AccountData.characterData[characterId]);
    }

    // 편성에 추가하기
    private void AddCharacterToFormation()
    {
        Debug.Log("AddCharacterToFormation");

        FormationUI ui = Managers.UI.FindUI<FormationUI>();
        // 이미 편성에 해당 캐릭터가 포함되어 있느지 체크
        bool isCharacterInFormation = Enumerable.Range(0, Managers.AccountData.formationData.GetLength(1))
            .Select(col => Managers.AccountData.formationData[ui.presetIndex, col])
            .Contains(Managers.AccountData.characterData[characterId].characterData.character_Id);

        // 이미 편성에 포함되어 있으면 불가 안내 UI
        if (isCharacterInFormation)
        {
            WarningUI warningUi = Managers.UI.ShowUI<WarningUI>();
            warningUi.SetText("이미 편성에 포함되어 있습니다");
            return;
        }

        // 편성 UI 뽑아서 formationIndex에 해당하는 곳에 캐릭터 정보 전달
        Debug.Log(ui.presetIndex);
        Debug.Log(formationIndex);
        Managers.AccountData.formationData[ui.presetIndex, formationIndex] = Managers.AccountData.characterData[characterId].characterData.character_Id;
        ui.UpdateFormationMember(formationIndex);

        Managers.UI.CloseUI(Managers.UI.PeekUI<CharacterUI>());
    }

    private void OnPointerUpCharacterButton()
    {
        Debug.Log("OnPointerUpCharacterButton");

        pressedTimer = 0f; // 버튼에서 손을 뗐을 때 pressedTimer 리셋
        hasShownCharacterInfo = false;
    }

    private void OnPressedCharacterButton()
    {
        Debug.Log("OnPressedCharacterButton");

        pressedTimer += Time.deltaTime;

        if (pressedTimer > 1f && !hasShownCharacterInfo)
        {
            ShowCharacterInfo();
            hasShownCharacterInfo = true;
        }
    }
}
