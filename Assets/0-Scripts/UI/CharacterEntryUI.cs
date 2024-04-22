using System;
using System.Linq;
using UnityEngine;
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
    private bool isCharacterInFormation;

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
        CharacterAttributeImage,
        InFormationImage
    }
    
    private enum GameObjects
    {
        Star
    }

    private void Start()
    {
        Init();
        Managers.AccountData.characterData[characterId].Growth.OnLevelUp += UpdateLevel;
    }

    private void OnDestroy()
    {
        Managers.AccountData.characterData[characterId].Growth.OnLevelUp -= UpdateLevel;
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
        GetImage((int)Images.CharacterImage).sprite = Managers.AccountData.characterData[characterId].SO.icon;
        UpdateLevel();
        // TODO 속성 이미지 세팅
        // TODO 캐릭터 아웃라인 속성 이미지에 맞게 세팅

        if (isFormation)
        {
            FormationUI ui = Managers.UI.FindUI<FormationUI>();
            // ui.presetIndex를 사용하여 현재 선택된 프리셋(파티)를 참조
            FormationData currentFormation = Managers.AccountData.formationData[ui.presetIndex];
            // 주어진 characterId가 현재 파티에 포함되어 있는지 확인
            isCharacterInFormation = currentFormation.characterId.Contains(characterId);

            // 편성에 포함되어 있지 않다면 편성됨 이미지 끄기
            if (isCharacterInFormation == false)
            {
                GetImage((int)Images.InFormationImage).gameObject.SetActive(false);
            }
        }
        // 메인화면에서 캐릭터 버튼을 누르고 왔다면 편성됨 이미지 끄기
        else
        {
            GetImage((int)Images.InFormationImage).gameObject.SetActive(false);
        }

        SetStar();
    }

    private void UpdateLevel()
    {
        GetText((int)Texts.CharacterLevelText).text = $"Lv. {Managers.AccountData.characterData[characterId].Growth.level}";
    }

    private void SetStar()
    {
        // int numberOfStars = character.characterData.defaltStar; // 별의 개수
        int numberOfStars = Managers.AccountData.characterData[characterId].Growth.star;
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

        /*
        // 이미 편성에 포함되어 있으면 불가 안내 UI
        if (isCharacterInFormation)
        {
            WarningUI warningUi = Managers.UI.ShowUI<WarningUI>();
            warningUi.Init("이미 편성에 포함되어 있습니다");
            return;
        }
        */

        // 이미 편성에 포함되어 있다면 편성 해제
        if (isCharacterInFormation)
        {
            // 해당 캐릭터가 있는 인덱스
            int characterIndex = Array.IndexOf(Managers.AccountData.formationData[ui.presetIndex].characterId, characterId);
            // 편성 이미지 비활성화
            GetImage((int)Images.InFormationImage).gameObject.SetActive(false);
            // 편성 데이터 업데이트
            Managers.AccountData.SetFormationCharacter(ui.presetIndex, characterIndex, 0);
            // UI 업데이트
            ui.UpdateFormationMember(characterIndex);

            isCharacterInFormation = false;

            return;
        }

        // 편성 UI 뽑아서 formationIndex에 해당하는 곳에 캐릭터 정보 전달
        Debug.Log(ui.presetIndex);
        Debug.Log(formationIndex);
        Managers.AccountData.SetFormationCharacter(ui.presetIndex, formationIndex, Managers.AccountData.characterData[characterId].SO.id);
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
