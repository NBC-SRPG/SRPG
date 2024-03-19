using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterInfoUI : UIBase
{
    // 캐릭터 정보
    public Character character;

    private PlayTab playTab = PlayTab.None;

    private enum PlayTab
    {
        None,
        Skill,
        Talent,
        Class
    }
    private enum Texts
    {
        NameText,
        ExSkillText,
        ExSkillDescriptionText,
        PassiveSkillText,
        PassiveSkillDescriptionText,
        HpText,
        AtkText,
        DefText,
        LevelText,
        ExpText
    }
    private enum Buttons
    {
        SkillButton,
        TalentButton,
        ClassButton,
        BackButton,
        ExSkillLevelUpButton
    }
    private enum Images
    {
        IllustrationImage,
        ExSkillImage,
        PassiveSkillImage,
        ExpFrontImage
    }
    private enum GameObjects
    {
        SkillTab,
        TalentTab,
        ClassTab,
        SkillInfoUI,
        TalentInfoUI,
        EquipmentUpgradeUI,
        Star
    }
    /*
    private void Start()
    {
        Init();
    }
    */

    public void SetCharacter(Character character)
    {
        this.character = character;
        Debug.Log(character);
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

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
        GetObject((int)GameObjects.TalentInfoUI).SetActive(false);
        GetObject((int)GameObjects.EquipmentUpgradeUI).SetActive(false);

        // 스킬, 특성, 클래스 탭 보여주기
        GetButton((int)Buttons.SkillButton).onClick.AddListener(() => ShowTab(PlayTab.Skill));
        GetButton((int)Buttons.TalentButton).onClick.AddListener(() => ShowTab(PlayTab.Talent));
        GetButton((int)Buttons.ClassButton).onClick.AddListener(() => ShowTab(PlayTab.Class));

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.ExSkillLevelUpButton).onClick.AddListener(OnClickExSkillLevelUpButton);

        ShowTab(PlayTab.Skill);

        // 테스트 데이터
        InitSkillTab();
        InitTalentTab();
        InitClassTab();
        InitCharacterInfo();
    }

    private void InitSkillTab()
    {
        // 데이터에 스킬 레벨이 없음??
        GetText((int)Texts.ExSkillText).text = $"{character.characterData.skill.skillName}"; // 뒤에 레벨도 붙어야 함
        GetText((int)Texts.ExSkillDescriptionText).text = $"{character.characterData.skill.description}";
        GetImage((int)Images.ExSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.skill.skill_ID}");

        GetText((int)Texts.PassiveSkillText).text = $"{character.characterData.passive.PassiveName}"; // 뒤에 레벨도 붙어야 함
        GetText((int)Texts.PassiveSkillDescriptionText).text = $"{character.characterData.passive.description}";
        GetImage((int)Images.PassiveSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.passive.passive_Id}");
    }

    private void InitTalentTab()
    {

    }

    private void InitClassTab()
    {

    }

    private void InitCharacterInfo()
    {
        GetImage((int)Images.IllustrationImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.character_Id}");
        GetText((int)Texts.NameText).text = $"{character.characterData.characterName}";

        int numberOfStars = character.characterData.defaltStar; // 별의 개수
        float starWidth = 100f; // 별 이미지의 너비
        float spacing = 10f; // 별 사이의 간격

        // 별 이미지들의 총 너비 계산
        float totalWidth = numberOfStars * starWidth + (numberOfStars - 1) * spacing;

        // 첫 번째 별 이미지의 시작 위치 계산
        float startX = -(totalWidth / 2) + (starWidth / 2);

        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject star = Managers.Resource.Instantiate("Star", GetObject((int)GameObjects.Star).transform);
            RectTransform rt = star.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(startX + i * (starWidth + spacing), 0);
        }

        GetText((int)Texts.HpText).text = $"{character.characterData.health + character.level * character.characterData.growHealth}";
        GetText((int)Texts.AtkText).text = $"{character.characterData.atk + character.level * character.characterData.growAtk}";
        GetText((int)Texts.DefText).text = $"{character.characterData.def + character.level * character.characterData.growDef}";
        GetText((int)Texts.LevelText).text = $"Lv. {character.level} / {character.maxLevel}";
        GetText((int)Texts.ExpText).text = $"{character.exp} / {character.maxExp}";
        GetImage((int)Images.ExpFrontImage).fillAmount = (float)character.exp / character.maxExp;
        
        // TODO
        // 장비 정보는 아직 없는 듯?
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
        GetObject((int)GameObjects.TalentTab).SetActive(false);
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

            case PlayTab.Talent:

                GetObject((int)GameObjects.TalentTab).SetActive(true);
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
    // TODO 고유 스킬 UI는 따로 할 것인지?
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
    private void OnPointerUpTalent()
    {
        Debug.Log("OnPointerUpTalent");

        GetObject((int)GameObjects.TalentInfoUI).SetActive(false);
    }

    private void OnPointerDownTalent()
    {
        Debug.Log("OnPointerDownTalent");

        GetObject((int)GameObjects.TalentInfoUI).SetActive(true);
    }
}
