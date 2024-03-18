using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationUI : UIBase
{
    public int presetIndex;
    private enum Texts
    {
        PartyNameText
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
        //PartyNameButton,
        ResetButton,
        //SaveButton,
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
        FormationImage5
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

        GetButton((int)Buttons.PresetButton1).onClick.AddListener(() => UpdateFormationToPreset(1));
        GetButton((int)Buttons.PresetButton2).onClick.AddListener(() => UpdateFormationToPreset(2));
        GetButton((int)Buttons.PresetButton3).onClick.AddListener(() => UpdateFormationToPreset(3));
        GetButton((int)Buttons.PresetButton4).onClick.AddListener(() => UpdateFormationToPreset(4));
        GetButton((int)Buttons.PresetButton5).onClick.AddListener(() => UpdateFormationToPreset(5));
        GetButton((int)Buttons.FormationButton1).onClick.AddListener(() => OnClickFormationButton(1));
        GetButton((int)Buttons.FormationButton2).onClick.AddListener(() => OnClickFormationButton(2));
        GetButton((int)Buttons.FormationButton3).onClick.AddListener(() => OnClickFormationButton(3));
        GetButton((int)Buttons.FormationButton4).onClick.AddListener(() => OnClickFormationButton(4));
        GetButton((int)Buttons.FormationButton5).onClick.AddListener(() => OnClickFormationButton(5));
        GetButton((int)Buttons.LeftArrowButton).onClick.AddListener(OnClickLeftArrowButton);
        GetButton((int)Buttons.RightArrowButton).onClick.AddListener(OnClickRightArrowButton);
        //GetButton((int)Buttons.PartyNameButton).onClick.AddListener(OnClickPartyNameButton);
        GetButton((int)Buttons.ResetButton).onClick.AddListener(OnClickResetButton);
        //GetButton((int)Buttons.SaveButton).onClick.AddListener(OnClickSaveButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        presetIndex = 1;

        UpdateFormationToPreset(presetIndex);

        // TODO
        // 저장되어 있는 프리셋이 있는지 체크해서 로드
    }

    // 프리셋 변경 시 업데이트
    // 프리셋 버튼 클릭이랑 같은 역할을 하게 될 것 같음 -> 둘이 합치고 함수명 고민
    private void UpdateFormationToPreset(int index)
    {
        // TODO
        // 현재 프리셋 버튼의 이미지 기본으로
        // index 프리셋 버튼의 이미지 체크 이미지로
        // 딕셔너리에서 index에 해당하는 캐릭터 정보 들고오기
        // 편성에 채우기

        presetIndex = index;

        if (!Managers.AccountData.formationData.ContainsKey(presetIndex))
        {
            // presetIndex에 해당하는 딕셔너리가 없을 경우, 새로운 딕셔너리를 생성 및 할당
            Managers.AccountData.formationData[presetIndex] = new Dictionary<int, Character>();
        }

        // 바뀐 프리셋 편성 데이터 업데이트
        for (int i = 1; i <= 5; i++)
        {
            UpdateFormationMember(i);
        }
    }

    // 편성의 index에 해당하는 부분 업데이트
    public void UpdateFormationMember(int index)
    {
        // 해당 index값 존재 시 세팅
        if (Managers.AccountData.formationData[presetIndex].ContainsKey(index))
        {
            switch (index)
            {
                case 1:
                    GetImage((int)Images.FormationImage1).sprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.formationData[presetIndex][index].characterData.character_Id}");
                    break;
                case 2:
                    GetImage((int)Images.FormationImage2).sprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.formationData[presetIndex][index].characterData.character_Id}");
                    break;
                case 3:
                    GetImage((int)Images.FormationImage3).sprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.formationData[presetIndex][index].characterData.character_Id}");
                    break;
                case 4:
                    GetImage((int)Images.FormationImage4).sprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.formationData[presetIndex][index].characterData.character_Id}");
                    break;
                case 5:
                    GetImage((int)Images.FormationImage5).sprite = Managers.Resource.Load<Sprite>($"{Managers.AccountData.formationData[presetIndex][index].characterData.character_Id}");
                    break;
            }
        }
        // 없다면 빈칸으로 밀어버리기
        else
        {
            switch (index)
            {
                case 1:
                    GetImage((int)Images.FormationImage1).sprite = null;
                    break;
                case 2:
                    GetImage((int)Images.FormationImage2).sprite = null;
                    break;
                case 3:
                    GetImage((int)Images.FormationImage3).sprite = null;
                    break;
                case 4:
                    GetImage((int)Images.FormationImage4).sprite = null;
                    break;
                case 5:
                    GetImage((int)Images.FormationImage5).sprite = null;
                    break;
            }
        }
    }

    private void OnClickFormationButton(int index)
    {
        Debug.Log($"OnClickFormationButton {index}");
        //Managers.Sound.Play(Constants.Sound.Effect, "ButtonClick");
        // 편성에서 클릭한 것이라고 알려주기
        CharacterEntryUI.isFormation = true;
        // 1번째 버튼이라고 알려주기
        CharacterEntryUI.formationIndex = index;
        // 캐릭터 UI 띄우기
        Managers.UI.ShowUI<CharacterUI>();
    }

    private void OnClickLeftArrowButton()
    {
        Debug.Log("OnClickLeftArrowButton");

        // 현재 프리셋이 1이라면 return
        if (presetIndex == 1)
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

        // 현재 프리셋이 5라면 return
        if (presetIndex == 5)
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
        // TODO
        // 편성 초기화
        Managers.AccountData.formationData[presetIndex].Clear();
        UpdateFormationToPreset(presetIndex);
    }

    private void OnClickSaveButton()
    {
        Debug.Log("OnClickSaveButton");
        // TODO
        // 편성 저장
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        // TODO
        // 저장 되지 않은 프리셋의 캐릭터들 편성 추가 체크 변수 해제 필요

        Managers.UI.CloseUI(this);
    }
}
