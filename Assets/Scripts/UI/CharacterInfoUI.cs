using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
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
        TalentDescriptionText,
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
        ExSkillLevelUpButton,
        WeaponButton,
        ArmorButton,
        EquipmentUpgradeUICloseButton,
        Talent1Button,
        Talent2_1Button,
        Talent2_2Button,
        Talent3_1Button,
        Talent3_2Button,
        TalentApplyButton,
        TalentCancelButton,
        TalentCheckButton,
        TestLevelUpButton // 테스트 버튼
    }
    private enum Images
    {
        IllustrationImage,
        ExSkillImage,
        PassiveSkillImage,
        WeaponImage,
        ArmorImage,
        TalentInfoImage,
        ExpFrontImage,
        TalentPath2,
        TalentPath2_1,
        TalentPath2_2,
        TalentPath3,
        TalentPath3_1,
        TalentPath3_2,
        Talent1Image,
        Talent2_1Image,
        Talent2_2Image,
        Talent3_1Image,
        Talent3_2Image,
    }
    private enum GameObjects
    {
        SkillTab,
        TalentTab,
        ClassTab,
        ExSkillInfoUI,
        PassiveSkillInfoUI,
        TalentInfoUI,
        EquipmentUpgradeUI,
        Star
    }

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

        // 스킬 이미지 클릭 시작, 클릭 끝 이벤트 걸기
        BindEvent(GetImage((int)Images.ExSkillImage).gameObject, OnPointerDownExSkill, UIEvent.PointerDown);
        BindEvent(GetImage((int)Images.ExSkillImage).gameObject, OnPointerUpExSkill, UIEvent.PointerUp);
        BindEvent(GetImage((int)Images.PassiveSkillImage).gameObject, OnPointerDownPassiveSkill, UIEvent.PointerDown);
        BindEvent(GetImage((int)Images.PassiveSkillImage).gameObject, OnPointerUpPassiveSkill, UIEvent.PointerUp);

        // 스킬, 특성, 장비강화 팝업 UI 비활성화 상태로 두기
        GetObject((int)GameObjects.ExSkillInfoUI).SetActive(false);
        GetObject((int)GameObjects.PassiveSkillInfoUI).SetActive(false);
        GetObject((int)GameObjects.TalentInfoUI).SetActive(false);
        GetObject((int)GameObjects.EquipmentUpgradeUI).SetActive(false);

        // 스킬, 특성, 클래스 탭 보여주기
        GetButton((int)Buttons.SkillButton).onClick.AddListener(() => ShowTab(PlayTab.Skill));
        GetButton((int)Buttons.TalentButton).onClick.AddListener(() => ShowTab(PlayTab.Talent));
        GetButton((int)Buttons.ClassButton).onClick.AddListener(() => ShowTab(PlayTab.Class));

        GetImage((int)Images.WeaponImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.weapon.equip_Id}");
        GetImage((int)Images.ArmorImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.armor.equip_Id}");

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.ExSkillLevelUpButton).onClick.AddListener(OnClickExSkillLevelUpButton);
        GetButton((int)Buttons.EquipmentUpgradeUICloseButton).onClick.AddListener(OnClickEquipmentUpgradeUICloseButton);
        GetButton((int)Buttons.TalentCancelButton).onClick.AddListener(OnClickTalentCancelButton);
        GetButton((int)Buttons.TalentCheckButton).onClick.AddListener(OnClickTalentCancelButton);
        GetButton((int)Buttons.WeaponButton).onClick.AddListener(() => OnClickWeaponButton(character.characterData.weapon));
        GetButton((int)Buttons.ArmorButton).onClick.AddListener(() => OnClickArmorButton(character.characterData.armor));

        ShowTab(PlayTab.Skill);

        // 테스트 데이터
        InitSkillTab();
        InitTalentTab();
        InitClassTab();
        InitCharacterInfo();

        GetButton((int)Buttons.TestLevelUpButton).onClick.AddListener(() => { character.characterGrowth.Exp += character.characterGrowth.maxExp; UpdateStat(); });
    }

    private void InitSkillTab()
    {
        GetText((int)Texts.ExSkillText).text = $"{character.characterData.skill.skillName}"; // 뒤에 레벨도 붙어야 함
        GetText((int)Texts.ExSkillDescriptionText).text = $"{character.characterData.skill.description}";
        GetImage((int)Images.ExSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.skill.skill_ID}");

        GetText((int)Texts.PassiveSkillText).text = $"{character.characterData.passive.PassiveName}"; // 뒤에 레벨도 붙어야 함
        GetText((int)Texts.PassiveSkillDescriptionText).text = $"{character.characterData.passive.description}";
        GetImage((int)Images.PassiveSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.passive.passive_Id}");
    }

    private void InitTalentTab()
    {
        // 특성 이미지 세팅
        GetImage((int)Images.Talent1Image).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.talent_Tier1.talent_Id}");
        GetImage((int)Images.Talent2_1Image).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.talent_Tier2[0].talent_Id}");
        GetImage((int)Images.Talent2_2Image).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.talent_Tier2[1].talent_Id}");
        GetImage((int)Images.Talent3_1Image).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.talent_Tier3[0].talent_Id}");
        GetImage((int)Images.Talent3_2Image).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.talent_Tier3[1].talent_Id}");

        // 특성 버튼 세팅
        GetButton((int)Buttons.Talent1Button).onClick.AddListener(() => OnClickTalentButton(1, character.characterData.talent_Tier1));
        GetButton((int)Buttons.Talent2_1Button).onClick.AddListener(() => OnClickTalentButton(2, character.characterData.talent_Tier2[0]));
        GetButton((int)Buttons.Talent2_2Button).onClick.AddListener(() => OnClickTalentButton(2, character.characterData.talent_Tier2[1]));
        GetButton((int)Buttons.Talent3_1Button).onClick.AddListener(() => OnClickTalentButton(3, character.characterData.talent_Tier3[0]));
        GetButton((int)Buttons.Talent3_2Button).onClick.AddListener(() => OnClickTalentButton(3, character.characterData.talent_Tier3[1]));

        TalentPathUpdate();
    }

    // 특성 패스 & 아웃라인 세팅
    private void TalentPathUpdate()
    {
        // 2단계 특성이 찍혀있다면
        if (character.characterGrowth.talent_Tier2 != null)
        {
            GetImage((int)Images.TalentPath2).color = Color.red;

            // 2-1 특성이 찍혀있다면
            if (character.characterGrowth.talent_Tier2 == character.characterData.talent_Tier2[0])
            {
                GetImage((int)Images.TalentPath2_1).color = Color.red;
                GetImage((int)Images.TalentPath2_2).color = Color.black;

                GetImage((int)Images.Talent2_1Image).transform.parent.GetComponent<Outline>().enabled = true;
                GetImage((int)Images.Talent2_2Image).transform.parent.GetComponent<Outline>().enabled = false;
            }
            // 2-2 특성이 찍혀있다면
            else
            {
                GetImage((int)Images.TalentPath2_1).color = Color.black;
                GetImage((int)Images.TalentPath2_2).color = Color.red;

                GetImage((int)Images.Talent2_1Image).transform.parent.GetComponent<Outline>().enabled = false;
                GetImage((int)Images.Talent2_2Image).transform.parent.GetComponent<Outline>().enabled = true;
            }
        }

        // 3번째 특성 찍었을 때
        if (character.characterGrowth.talent_Tier3 != null)
        {
            GetImage((int)Images.TalentPath3).color = Color.red;

            // 3-1 특성
            if (character.characterGrowth.talent_Tier3 == character.characterData.talent_Tier3[0])
            {
                GetImage((int)Images.TalentPath3_1).color = Color.red;
                GetImage((int)Images.TalentPath3_2).color = Color.black;

                GetImage((int)Images.Talent3_1Image).transform.parent.GetComponent<Outline>().enabled = true;
                GetImage((int)Images.Talent3_2Image).transform.parent.GetComponent<Outline>().enabled = false;
            }
            // 3-2 특성
            else
            {
                GetImage((int)Images.TalentPath3_1).color = Color.black;
                GetImage((int)Images.TalentPath3_2).color = Color.red;

                GetImage((int)Images.Talent3_1Image).transform.parent.GetComponent<Outline>().enabled = false;
                GetImage((int)Images.Talent3_2Image).transform.parent.GetComponent<Outline>().enabled = true;
            }
        }
    }

    private void InitClassTab()
    {

    }

    private void InitCharacterInfo()
    {
        GetImage((int)Images.IllustrationImage).sprite = Managers.Resource.Load<Sprite>($"{character.characterData.character_Id}");
        GetText((int)Texts.NameText).text = $"{character.characterData.characterName}";

        // int numberOfStars = character.characterData.defaltStar; // 별의 개수
        int numberOfStars = 3; // 별의 개수 // 테스트 데이터
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

        UpdateStat();
        // TODO
        // 장비 정보는 아직 없는 듯?
    }

    private void UpdateStat()
    {
        GetText((int)Texts.LevelText).text = $"Lv. {character.characterGrowth.Level} / {character.characterGrowth.maxLevel}";
        GetText((int)Texts.ExpText).text = $"{character.characterGrowth.Exp} / {character.characterGrowth.maxExp}";
        GetImage((int)Images.ExpFrontImage).fillAmount = (float)character.characterGrowth.Exp / character.characterGrowth.maxExp;

        GetText((int)Texts.HpText).text = $"{character.CalcHealth}";
        GetText((int)Texts.AtkText).text = $"{character.CalcAtk}";
        GetText((int)Texts.DefText).text = $"{character.CalcDef}";
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
                // TODO
                // 레벨 30이 넘어야 해금
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

        GetObject((int)GameObjects.ExSkillInfoUI).SetActive(false);
    }

    private void OnPointerDownExSkill()
    {
        Debug.Log("OnPointerDownExSkill");

        GetObject((int)GameObjects.ExSkillInfoUI).SetActive(true);
    }

    private void OnClickEquipmentUpgradeUICloseButton()
    {
        Debug.Log("OnClickEquipmentUpgradeUICloseButton");

        GetObject((int)GameObjects.EquipmentUpgradeUI).SetActive(false);
    }

    // TODO 고유 스킬 UI는 따로 할 것인지?
    private void OnPointerUpPassiveSkill()
    {
        Debug.Log("OnPointerUpPassiveSkill");

        GetObject((int)GameObjects.PassiveSkillInfoUI).SetActive(false);
    }

    private void OnPointerDownPassiveSkill()
    {
        Debug.Log("OnPointerDownPassiveSkill");

        GetObject((int)GameObjects.PassiveSkillInfoUI).SetActive(true);
    }

    private void OnClickTalentButton(int talentTier, TalentSO talent)
    {
        Debug.Log("OnClickTalentButton");

        // 특성 선택 UI 켜기
        GetObject((int)GameObjects.TalentInfoUI).SetActive(true);

        // 이미지, 설명 세팅
        GetImage((int)Images.TalentInfoImage).sprite = Managers.Resource.Load<Sprite>($"{talent.talent_Id}");
        GetText((int)Texts.TalentDescriptionText).text = talent.talentDescription;

        // 버튼들 모두 비활성화 상태
        GetButton((int)Buttons.TalentApplyButton).gameObject.SetActive(false);
        GetButton((int)Buttons.TalentCancelButton).gameObject.SetActive(false);
        GetButton((int)Buttons.TalentCheckButton).gameObject.SetActive(false);

        switch (talentTier)
        {
            // 선택한게 1단계 특성인 경우 -> 항상 적용중
            case 1:
                // 선택한 특성이면 아웃라인 켜기
                SetupSelectedTalentUI();
                break;

            case 2:
                // 선택 가능한지 체크
                if(character.characterGrowth.Level < 50)
                {
                    SetupUnselectableTalentUI(talentTier);
                }
                // 이미 적용 된 특성이라면
                else if (character.characterGrowth.talent_Tier2 == talent)
                {
                    SetupSelectedTalentUI();
                }
                // 적용 안된 특성이라면
                else
                {
                    SetupUnselectedTalentUI(talentTier, talent);
                }
                break;

            case 3:
                // 선택 가능한지 체크
                if (character.characterGrowth.Level < 70 || character.characterGrowth.talent_Tier2 == null)
                {
                    SetupUnselectableTalentUI(talentTier);
                }
                // 이미 적용 된 특성이라면
                else if (character.characterGrowth.talent_Tier3 == talent)
                {
                    SetupSelectedTalentUI();
                }
                // 적용 안된 특성이라면
                else
                {
                    SetupUnselectedTalentUI(talentTier, talent);
                }
                break;
        }
    }

    // 이미 적용 된 특성 UI 세팅
    private void SetupSelectedTalentUI()
    {
        GetImage((int)Images.TalentInfoImage).transform.parent.GetComponent<Outline>().enabled = true;

        GetButton((int)Buttons.TalentCheckButton).gameObject.SetActive(true);
    }

    // 적용 안된 특성 UI 세팅
    private void SetupUnselectedTalentUI(int talentTier, TalentSO talent)
    {
        GetImage((int)Images.TalentInfoImage).transform.parent.GetComponent<Outline>().enabled = false;

        GetButton((int)Buttons.TalentApplyButton).gameObject.SetActive(true);
        GetButton((int)Buttons.TalentCancelButton).gameObject.SetActive(true);

        GetButton((int)Buttons.TalentApplyButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.TalentApplyButton).onClick.AddListener(() => OnClickTalentApplyButton(talentTier, talent));
    }

    // 특성 선택이 불가능할 때 UI 세팅
    private void SetupUnselectableTalentUI(int talentTier)
    {
        GetImage((int)Images.TalentInfoImage).transform.parent.GetComponent<Outline>().enabled = false;
        GetButton((int)Buttons.TalentCheckButton).gameObject.SetActive(true);

        if (talentTier == 2)
        {
            GetText((int)Texts.TalentDescriptionText).text += $"\n\n<color=red>* 레벨 50이상이 필요합니다.</color>";
        }
        else if (talentTier == 3)
        {
            GetText((int)Texts.TalentDescriptionText).text += $"\n\n<color=red>* 레벨 70이상, 2단계 특성 활성화가 필요합니다.</color>";
        }
    }

    private void OnClickTalentApplyButton(int talentTier, TalentSO selectTalent)
    {
        Debug.Log("OnClickTalentApplyButton");

        if (talentTier == 2)
        {
            character.characterGrowth.SelectTalent_tier2(selectTalent);
        }
        else if (talentTier == 3)
        {
            character.characterGrowth.SelectTalent_tier3(selectTalent);
        }

        TalentPathUpdate();
        UpdateStat();
        GetObject((int)GameObjects.TalentInfoUI).SetActive(false);
    }

    private void OnClickTalentCancelButton()
    {
        Debug.Log("OnClickTalentCancelButton");

        GetObject((int)GameObjects.TalentInfoUI).SetActive(false);
    }

    private void OnClickWeaponButton(EquipSO weapon)
    {
        Debug.Log("OnClickWeaponButton");

        GetObject((int)GameObjects.EquipmentUpgradeUI).SetActive(false);
    }
    private void OnClickArmorButton(EquipSO armor)
    {
        Debug.Log("OnClickArmorButton");

        GetObject((int)GameObjects.EquipmentUpgradeUI).SetActive(false);
    }
}
