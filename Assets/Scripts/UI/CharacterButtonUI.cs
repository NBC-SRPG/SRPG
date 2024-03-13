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

    private enum Texts
    {
        NameText
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
        // TODO
        // 캐릭터 정보에서 이미지나 이름 레벨등을 꺼내와서 세팅
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CharacterButton).onClick.AddListener(OnClickButton);
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

        // Managers.UI.ShowUI<CharacterInfoUI>;
    }
    // 편성에 추가하기
    private void AddCharacterToFormation()
    {
        Debug.Log("AddCharacterToFormation");
        // TODO
        // 편성에 추가
        // 편성에 어떻게 추가하지? -> 버튼을 누르면 바로 추가 or 버튼 누르면 정보창이 뜨고 거기에서 추가
    }
}
