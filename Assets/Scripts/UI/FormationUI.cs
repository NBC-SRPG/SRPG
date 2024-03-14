using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationUI : UIBase
{
    private int presetIndex;
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
        PartyNameButton,
        ResetButton,
        SaveButton
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

        GetButton((int)Buttons.PresetButton1).onClick.AddListener(OnClickPresetButton1);
        GetButton((int)Buttons.PresetButton2).onClick.AddListener(OnClickPresetButton2);
        GetButton((int)Buttons.PresetButton3).onClick.AddListener(OnClickPresetButton3);
        GetButton((int)Buttons.PresetButton4).onClick.AddListener(OnClickPresetButton4);
        GetButton((int)Buttons.PresetButton5).onClick.AddListener(OnClickPresetButton5);
        GetButton((int)Buttons.FormationButton1).onClick.AddListener(OnClickFormationButton1);
        GetButton((int)Buttons.FormationButton2).onClick.AddListener(OnClickFormationButton2);
        GetButton((int)Buttons.FormationButton3).onClick.AddListener(OnClickFormationButton3);
        GetButton((int)Buttons.FormationButton4).onClick.AddListener(OnClickFormationButton4);
        GetButton((int)Buttons.FormationButton5).onClick.AddListener(OnClickFormationButton5);
        GetButton((int)Buttons.LeftArrowButton).onClick.AddListener(OnClickLeftArrowButton);
        GetButton((int)Buttons.RightArrowButton).onClick.AddListener(OnClickRightArrowButton);
        GetButton((int)Buttons.PartyNameButton).onClick.AddListener(OnClickPartyNameButton);
        GetButton((int)Buttons.ResetButton).onClick.AddListener(OnClickResetButton);
        GetButton((int)Buttons.SaveButton).onClick.AddListener(OnClickSaveButton);

        presetIndex = 1;
    }
    private void OnClickPresetButton1()
    {
        Debug.Log("OnClickPresetButton1");
        // TODO
        // 기본 프리셋
        // 저장 된 프리셋 확인 후 불러오기
        // 없으면 빈 프리셋
    }
    private void OnClickPresetButton2()
    {
        Debug.Log("OnClickPresetButton2");
    }
    private void OnClickPresetButton3()
    {
        Debug.Log("OnClickPresetButton3");
    }
    private void OnClickPresetButton4()
    {
        Debug.Log("OnClickPresetButton4");
    }
    private void OnClickPresetButton5()
    {
        Debug.Log("OnClickPresetButton5");
    }
    private void OnClickFormationButton1()
    {
        Debug.Log("OnClickFormationButton1");
        //Managers.Sound.Play(Constants.Sound.Effect, "ButtonClick");
        // 편성에서 클릭한 것이라고 알려주기
        CharacterButtonUI.isFormation = true;
        // 버튼 1이라고 알려주기
        CharacterButtonUI.formationIndex = 1;
        // 캐릭터 UI 띄우기
        Managers.UI.ShowUI<CharacterUI>();
    }
    private void OnClickFormationButton2()
    {
        Debug.Log("OnClickFormationButton2");
    }
    private void OnClickFormationButton3()
    {
        Debug.Log("OnClickFormationButton3");
    }
    private void OnClickFormationButton4()
    {
        Debug.Log("OnClickFormationButton4");
    }
    private void OnClickFormationButton5()
    {
        Debug.Log("OnClickFormationButton5");
    }
    private void OnClickLeftArrowButton()
    {
        Debug.Log("OnClickLeftArrowButton");
        // TODO
        // 현재 프리셋이 1이라면 return
        if (presetIndex == 1)
        {
            return;
        }
        // 아니라면 프리셋-- & 업데이트
        presetIndex--;
    }
    private void OnClickRightArrowButton()
    {
        Debug.Log("OnClickRightArrowButton");
        // TODO
        // 현재 프리셋이 5라면 return
        if (presetIndex == 5)
        {
            return;
        }
        // 아니라면 프리셋++ & 업데이트
        presetIndex++;
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
    }
    private void OnClickSaveButton()
    {
        Debug.Log("OnClickSaveButton");
        // TODO
        // 편성 저장
    }
}
