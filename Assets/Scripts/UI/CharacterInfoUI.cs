using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterInfoUI : UIBase
{
    private PlayTab playTab = PlayTab.None;

    private enum PlayTab
    {
        None,
        Skill,
        Trait,
        Class
    }
    private enum Texts
    {
        NameText,
        EXSkillText,
        ExSkillDescriptionText,
        PassiveSkillText,
        PassiveSkillDescriptionText
    }
    private enum Buttons
    {
        SkillButton,
        TraitButton,
        ClassButton,
        BackButton,
        ExSkillLevelUpButton
    }
    private enum Images
    {
        IllustrationImage,
        ExSkillImage,
        PassiveSkillImage
    }
    private enum GameObjects
    {
        SkillTab,
        TraitTab,
        ClassTab,
        SkillInfoUI,
        TraitInfoUI
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

        // 스킬, 특성 이미지 클릭 시작, 클릭 끝 이벤트 걸기
        BindEvent(GetImage((int)Images.ExSkillImage).gameObject, OnPointerDownExSkill, UIEvent.PointerDown);
        BindEvent(GetImage((int)Images.ExSkillImage).gameObject, OnPointerUpExSkill, UIEvent.PointerUp);
        BindEvent(GetImage((int)Images.PassiveSkillImage).gameObject, OnPointerDownPassiveSkill, UIEvent.PointerDown);
        BindEvent(GetImage((int)Images.PassiveSkillImage).gameObject, OnPointerUpPassiveSkill, UIEvent.PointerUp);

        // 스킬, 특성 팝업 UI 비활성화 상태로 두기
        GetObject((int)GameObjects.SkillInfoUI).SetActive(false);

        // 스킬, 특성, 클래스 탭 보여주기
        GetButton((int)Buttons.SkillButton).onClick.AddListener(() => ShowTab(PlayTab.Skill));
        GetButton((int)Buttons.TraitButton).onClick.AddListener(() => ShowTab(PlayTab.Trait));
        GetButton((int)Buttons.ClassButton).onClick.AddListener(() => ShowTab(PlayTab.Class));

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.ExSkillLevelUpButton).onClick.AddListener(OnClickExSkillLevelUpButton);

        ShowTab(PlayTab.Skill);
    }

    private void ShowTab(PlayTab tab)
    {
        // 이미 탭에 열려있는 정보를 누르면 아무것도 하지않음
        if (playTab == tab)
            return;

        // 현재 열려있는 탭 업데이트
        playTab = tab;

        // 세팅 초기화
        // 모든 탭 끄기
        // 프리팹에서는 모든 탭이 켜져있어야 Bind 가능
        GetObject((int)GameObjects.SkillTab).SetActive(false);
        GetObject((int)GameObjects.TraitTab).SetActive(false);
        GetObject((int)GameObjects.ClassTab).SetActive(false);

        // TODO
        // 모든 버튼 이미지 초기화

        switch (playTab)
        {
            case PlayTab.Skill:
                // TODO
                // 버튼 눌리는 효과음 재생
                // 해당 탭 활성화
                GetObject((int)GameObjects.SkillTab).SetActive(true);
                // 해당 버튼 이미지 변경 (클릭한 버튼임을 보여주기)
                break;

            case PlayTab.Trait:

                GetObject((int)GameObjects.TraitTab).SetActive(true);
                break;

            case PlayTab.Class:

                GetObject((int)GameObjects.ClassTab).SetActive(true);
                break;
        }
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }

    private void OnClickExSkillLevelUpButton()
    {
        Debug.Log("OnClickExSkillLevelUpButton");

        // TODO
        // 버튼 클릭 효과음
        // 스킬 레벨 업
        // 정보 저장
        // 스킬 LV 텍스트 & 설명 업데이트
    }

    // TODO
    // EX, 고유 스킬의 정보창은 CharacterInfo Init에서 초기화 -> 변하지 않음
    // 특성 정보창은 클릭 시 초기화 -> 특성 5가지 클릭 시 매번 바뀜
    // 초기화 전 마지막 클릭 한 특성 정보를 들고 있다가 같으면 아무것도 하지 않고 return
    // 팝업창은 SetActive로 관리하고 있으나 추후 필요 시 기존과 같이 동적 관리
    private void OnPointerUpExSkill()
    {
        Debug.Log("OnPointerUpExSkill");

        GetObject((int)GameObjects.SkillInfoUI).SetActive(false);
    }

    private void OnPointerDownExSkill()
    {
        Debug.Log("OnPointerDownExSkill");

        GetObject((int)GameObjects.SkillInfoUI).SetActive(true);
    }
    private void OnPointerUpPassiveSkill()
    {
        Debug.Log("OnPointerUpPassiveSkill");

        GetObject((int)GameObjects.SkillInfoUI).SetActive(false);
    }

    private void OnPointerDownPassiveSkill()
    {
        Debug.Log("OnPointerDownPassiveSkill");

        GetObject((int)GameObjects.SkillInfoUI).SetActive(true);
    }
    private void OnPointerUpTrait()
    {
        Debug.Log("OnPointerUpTrait");

        GetObject((int)GameObjects.TraitInfoUI).SetActive(false);
    }

    private void OnPointerDownTrait()
    {
        Debug.Log("OnPointerDownTrait");

        GetObject((int)GameObjects.TraitInfoUI).SetActive(true);
    }
}
