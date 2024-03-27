using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Constants;

public class FormationUI : UIBase
{
    public int presetIndex;
    // 1초 이상 눌렀는지를 체크하는 변수
    private float pressedTimer;
    // 1초 이상 눌렀을 때 캐릭터 정보창이 켜져있는지 체크하는 변수
    private bool hasShownCharacterInfo;
    private enum Texts
    {
        PartyNameText,
        FormationLevelText1,
        FormationLevelText2,
        FormationLevelText3,
        FormationLevelText4,
        FormationLevelText5
    }
    private enum InputFields
    {
        PartyNameInputField
    }
    private enum Buttons
    {
        PresetButton1,
        PresetButton2,
        PresetButton3,
        PresetButton4,
        PresetButton5,
        FormationButton1,
        FormationButton2,
        FormationButton3,
        FormationButton4,
        FormationButton5,
        LeftArrowButton,
        RightArrowButton,
        ResetButton,
        BackButton
    }
    private enum Images
    {
        PresetImage1,
        PresetImage2,
        PresetImage3,
        PresetImage4,
        PresetImage5,
        FormationImage1,
        FormationImage2,
        FormationImage3,
        FormationImage4,
        FormationImage5,
        FormationAttributeImage1,
        FormationAttributeImage2,
        FormationAttributeImage3,
        FormationAttributeImage4,
        FormationAttributeImage5
    }
    private enum GameObjects
    {
        FormationStar1,
        FormationStar2,
        FormationStar3,
        FormationStar4,
        FormationStar5
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
        Bind<TMP_InputField>(typeof(InputFields));

        Buttons presetButton;
        Buttons formationButton;

        for (int i = 0; i < 5; i++)
        {
            int index = i;

            presetButton = (Buttons)Enum.Parse(typeof(Buttons), $"PresetButton{index + 1}");
            GetButton((int)presetButton).onClick.AddListener(() => UpdateFormationToPreset(index));

            formationButton = (Buttons)Enum.Parse(typeof(Buttons), $"FormationButton{index + 1}");
            GetButton((int)formationButton).onClick.AddListener(() => OnClickFormationButton(index));
            BindEvent(GetButton((int)formationButton).gameObject, OnPointerUpFormationButton, UIEvent.PointerUp);
            BindEvent(GetButton((int)formationButton).gameObject, () => OnPressedFormationButton(index), UIEvent.Pressed);
        }

        GetButton((int)Buttons.LeftArrowButton).onClick.AddListener(OnClickLeftArrowButton);
        GetButton((int)Buttons.RightArrowButton).onClick.AddListener(OnClickRightArrowButton);
        GetButton((int)Buttons.ResetButton).onClick.AddListener(OnClickResetButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        Get<TMP_InputField>((int)InputFields.PartyNameInputField).onEndEdit.AddListener(ChangePartyName);

        presetIndex = 0;

        UpdateFormationToPreset(presetIndex);
    }

    // 프리셋 변경 시 업데이트
    private void UpdateFormationToPreset(int index)
    {
        // TODO
        // 현재 프리셋 버튼의 이미지 기본으로
        // index 프리셋 버튼의 이미지 체크 이미지로
        // 테스트로 색상 변경 사용중
        Images presetImage;

        for (int i = 0; i < 5; i++)
        {
            presetImage = (Images)Enum.Parse(typeof(Images), $"PresetImage{i + 1}");
            GetImage((int)presetImage).color = Color.white;
        }

        presetImage = (Images)Enum.Parse(typeof(Images), $"PresetImage{index + 1}");
        GetImage((int)presetImage).color = Color.green;
        // 딕셔너리에서 index에 해당하는 캐릭터 정보 들고오기
        // 편성에 채우기

        presetIndex = index;

        // 파티 이름 불러오기
        Get<TMP_InputField>((int)InputFields.PartyNameInputField).text = $"{Managers.AccountData.formationData[presetIndex].partyName}";
       
        // 바뀐 프리셋 편성 데이터 업데이트
        for (int i = 0; i < 5; i++)
        {
            UpdateFormationMember(i);
        }
    }

    // 편성의 index에 해당하는 부분 업데이트
    public void UpdateFormationMember(int index)
    {
        Images formationImageEnum = (Images)Enum.Parse(typeof(Images), $"FormationImage{index + 1}");
        Images formationAttributeImageEnum = (Images)Enum.Parse(typeof(Images), $"FormationAttributeImage{index + 1}");
        GameObjects formationStarEnum = (GameObjects)Enum.Parse(typeof(GameObjects), $"FormationStar{index + 1}");
        Texts formationLevelTextEnum = (Texts)Enum.Parse(typeof(Texts), $"FormationLevelText{index + 1}");

        // 해당 index값 존재 시 세팅 -> 캐릭터 id는 0 존재하면 안됨
        if (Managers.AccountData.formationData.ContainsKey(presetIndex) && Managers.AccountData.formationData[presetIndex].characterId[index] != 0)
        {
            Sprite characterSprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.formationData[presetIndex].characterId[index]}");
            GetImage((int)formationImageEnum).sprite = characterSprite;
            // TODO
            // 아웃라인 활성화 및 색상 설정
            GetImage((int)formationImageEnum).transform.parent.GetComponent<Outline>().enabled = true;
            // GetImage((int)formationImageEnum).transform.parent.GetComponent<Outline>().effectColor = Color.red;
            // 속성 이미지 변경
            GetImage((int)formationAttributeImageEnum).color = Color.red;

            // 캐릭터 레벨 설정
            int characterLevel = Managers.AccountData.characterData[Managers.AccountData.formationData[presetIndex].characterId[index]].characterGrowth.Level;
            GetText((int)formationLevelTextEnum).text = $"Lv. {characterLevel}";

            // 별 개수 꺼내오기 및 설정
            int numberOfStars = 3; // 테스트 데이터, 실제 값으로 교체 필요
            float starWidth = 50f; // 별 이미지의 너비
            for (int starIndex = 0; starIndex < numberOfStars; starIndex++)
            {
                GameObject star = Managers.Resource.Instantiate("Star", GetObject((int)formationStarEnum).transform);
                star.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                RectTransform rt = star.GetComponent<RectTransform>();
                rt.anchorMax = new Vector2(0f, 0f);
                rt.anchorMin = new Vector2(0f, 0f);
                rt.pivot = new Vector2(0f, 0f);
                rt.anchoredPosition = new Vector2(starIndex * starWidth, 0);
            }
        }
        // 없다면(0이라면) 빈칸으로 밀어버리기
        else
        {
            GetImage((int)formationImageEnum).sprite = null;
            GetImage((int)formationImageEnum).transform.parent.GetComponent<Outline>().enabled = false;
            GetImage((int)formationAttributeImageEnum).color = Color.white;
            GetText((int)formationLevelTextEnum).text = "";

            for (int i = GetObject((int)formationStarEnum).transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = GetObject((int)formationStarEnum).transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
    }

    private void ChangePartyName(string newPartyName)
    {
        if (!string.IsNullOrEmpty(newPartyName))
        {
            Debug.Log("New PartyName: " + newPartyName);

            Managers.AccountData.formationData[presetIndex].partyName = newPartyName;
        }
    }

    private void OnClickFormationButton(int index)
    {
        Debug.Log($"OnClickFormationButton {index}");
        //Managers.Sound.Play(Constants.Sound.Effect, "ButtonClick");
        // 편성에서 클릭한 것이라고 알려주기
        CharacterEntryUI.isFormation = true;
        // index번째 버튼이라고 알려주기
        CharacterEntryUI.formationIndex = index;
        // 캐릭터 UI 띄우기
        Managers.UI.ShowUI<CharacterUI>();
    }

    private void OnClickLeftArrowButton()
    {
        Debug.Log("OnClickLeftArrowButton");

        // 현재 프리셋이 0이라면 return
        if (presetIndex == 0)
        {
            return;
        }
        // 아니라면 프리셋-- & 업데이트
        presetIndex--;
        UpdateFormationToPreset(presetIndex);
    }

    private void OnClickRightArrowButton()
    {
        Debug.Log("OnClickRightArrowButton");

        // 현재 프리셋이 4라면 return
        if (presetIndex == 4)
        {
            return;
        }
        // 아니라면 프리셋++ & 업데이트
        presetIndex++;
        UpdateFormationToPreset(presetIndex);
    }

    private void OnClickPartyNameButton()
    {
        Debug.Log("OnClickPartyNameButton");
        // TODO
        // 키보드창 뜨기
        // 입력한 문자열로 업데이트
    }

    private void OnClickResetButton()
    {
        Debug.Log("OnClickResetButton");

        // 편성 초기화
        for (int i = 0; i < 5; i++)
        {
            Managers.AccountData.formationData[presetIndex].characterId[i] = 0;
        }
        UpdateFormationToPreset(presetIndex);
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        Managers.UI.CloseUI(this);
    }

    private void OnPointerUpFormationButton()
    {
        Debug.Log("OnPointerUpFormationButton");

        pressedTimer = 0f; // 버튼에서 손을 뗐을 때 pressedTimer 리셋
        hasShownCharacterInfo = false;
    }

    private void OnPressedFormationButton(int index)
    {
        Debug.Log("OnPressedFormationButton");

        if (Managers.AccountData.formationData[presetIndex].characterId[index] == 0)
        {
            return;
        }

        pressedTimer += Time.deltaTime;

        if (pressedTimer > 1f && !hasShownCharacterInfo)
        {
            CharacterInfoUI ui = Managers.UI.ShowUI<CharacterInfoUI>();

            ui.SetCharacter(Managers.AccountData.characterData[Managers.AccountData.formationData[presetIndex].characterId[index]]);
            hasShownCharacterInfo = true;
        }
    }
}
