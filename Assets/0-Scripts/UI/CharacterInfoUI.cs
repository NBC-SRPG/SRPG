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
        Ability,
        Class
    }
    private enum Texts
    {
        NameText,
        ExSkillText,
        ExSkillDescriptionText,
        PassiveSkillText,
        PassiveSkillDescriptionText,
        AbilityDescriptionText,
        HpText,
        AtkText,
        DefText,
        LevelText,
        ExpText
    }
    private enum Buttons
    {
        SkillButton,
        AbilityButton,
        ClassButton,
        BackButton,
        ExSkillLevelUpButton,
        WeaponButton,
        ArmorButton,
        EquipmentUpgradeUICloseButton,
        Ability1Button,
        Ability2_1Button,
        Ability2_2Button,
        Ability3_1Button,
        Ability3_2Button,
        AbilityApplyButton,
        AbilityCancelButton,
        AbilityCheckButton,
        TestLevelUpButton // 테스트 버튼
    }
    private enum Images
    {
        IllustrationImage,
        ExSkillImage,
        PassiveSkillImage,
        WeaponImage,
        ArmorImage,
        AbilityInfoImage,
        ExpFrontImage,
        AbilityPath2,
        AbilityPath2_1,
        AbilityPath2_2,
        AbilityPath3,
        AbilityPath3_1,
        AbilityPath3_2,
        Ability1Image,
        Ability2_1Image,
        Ability2_2Image,
        Ability3_1Image,
        Ability3_2Image,
    }
    private enum GameObjects
    {
        SkillTab,
        AbilityTab,
        ClassTab,
        ExSkillInfoUI,
        PassiveSkillInfoUI,
        AbilityInfoUI,
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
        //GetObject((int)GameObjects.ExSkillInfoUI).SetActive(false);
        GetObject((int)GameObjects.PassiveSkillInfoUI).SetActive(false);
        GetObject((int)GameObjects.AbilityInfoUI).SetActive(false);
        GetObject((int)GameObjects.EquipmentUpgradeUI).SetActive(false);

        // 스킬, 특성, 클래스 탭 보여주기
        GetButton((int)Buttons.SkillButton).onClick.AddListener(() => ShowTab(PlayTab.Skill));
        GetButton((int)Buttons.AbilityButton).onClick.AddListener(() => ShowTab(PlayTab.Ability));
        GetButton((int)Buttons.ClassButton).onClick.AddListener(() => ShowTab(PlayTab.Class));

        //GetImage((int)Images.WeaponImage).sprite = Managers.Resource.Load<Sprite>($"{character.SO.weapon.equip_Id}");
        //GetImage((int)Images.ArmorImage).sprite = Managers.Resource.Load<Sprite>($"{character.SO.armor.equip_Id}");

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        //GetButton((int)Buttons.ExSkillLevelUpButton).onClick.AddListener(OnClickExSkillLevelUpButton);
        GetButton((int)Buttons.EquipmentUpgradeUICloseButton).onClick.AddListener(OnClickEquipmentUpgradeUICloseButton);
        GetButton((int)Buttons.AbilityCancelButton).onClick.AddListener(OnClickAbilityCancelButton);
        GetButton((int)Buttons.AbilityCheckButton).onClick.AddListener(OnClickAbilityCancelButton);
        //GetButton((int)Buttons.WeaponButton).onClick.AddListener(() => OnClickWeaponButton(character.SO.weapon));
        //GetButton((int)Buttons.ArmorButton).onClick.AddListener(() => OnClickArmorButton(character.SO.armor));

        ShowTab(PlayTab.Skill);

        // 테스트 데이터
        InitSkillTab();
        InitAbilityTab();
        InitClassTab();
        InitCharacterInfo();

        GetButton((int)Buttons.TestLevelUpButton).onClick.AddListener(() => { character.Growth.curExp += character.Growth.maxExp; UpdateStat(); });
    }

    
    private void InitSkillTab()
    {
        //GetText((int)Texts.ExSkillText).text = $"{character.SO.skill.skillName}"; // 뒤에 레벨도 붙어야 함
        //GetText((int)Texts.ExSkillDescriptionText).text = $"{character.SO.skill.description}";
        //GetImage((int)Images.ExSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.SO.skill.skill_ID}");

        //GetText((int)Texts.PassiveSkillText).text = $"{character.SO.passive.PassiveName}"; // 뒤에 레벨도 붙어야 함
        //GetText((int)Texts.PassiveSkillDescriptionText).text = $"{character.SO.passive.description}";
        //GetImage((int)Images.PassiveSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.SO.passive.passive_Id}");
    }

    private void InitAbilityTab()
    {
        // 특성 이미지 세팅
        GetImage((int)Images.Ability1Image).sprite = Managers.Resource.Load<Sprite>($"{character.abilityT1.id}");
        GetImage((int)Images.Ability2_1Image).sprite = Managers.Resource.Load<Sprite>($"{character.SO.abilityT2[0]}");
        GetImage((int)Images.Ability2_2Image).sprite = Managers.Resource.Load<Sprite>($"{character.SO.abilityT2[1]}");
        GetImage((int)Images.Ability3_1Image).sprite = Managers.Resource.Load<Sprite>($"{character.SO.abilityT3[0]}");
        GetImage((int)Images.Ability3_2Image).sprite = Managers.Resource.Load<Sprite>($"{character.SO.abilityT3[1]}");

        // 특성 버튼 세팅
        // TODO: IdToSO 구현 후 수정
        /*
        GetButton((int)Buttons.Ability1Button).onClick.AddListener(() => OnClickAbilityButton(1, character.SO.Ability_Tier1));
        GetButton((int)Buttons.Ability2_1Button).onClick.AddListener(() => OnClickAbilityButton(2, character.SO.Ability_Tier2[0]));
        GetButton((int)Buttons.Ability2_2Button).onClick.AddListener(() => OnClickAbilityButton(2, character.SO.Ability_Tier2[1]));
        GetButton((int)Buttons.Ability3_1Button).onClick.AddListener(() => OnClickAbilityButton(3, character.SO.Ability_Tier3[0]));
        GetButton((int)Buttons.Ability3_2Button).onClick.AddListener(() => OnClickAbilityButton(3, character.SO.Ability_Tier3[1]));
        */
        AbilityPathUpdate();
    }

    // 특성 패스 & 아웃라인 세팅
    private void AbilityPathUpdate()
    {
        // 2단계 특성이 찍혀있다면
        if (character.Growth.abilityT2 != NONE_SELECTED)
        {
            GetImage((int)Images.AbilityPath2).color = Color.red;

            // 2-1 특성이 찍혀있다면
            if (character.Growth.abilityT2 == 0)
            {
                GetImage((int)Images.AbilityPath2_1).color = Color.red;
                GetImage((int)Images.AbilityPath2_2).color = Color.black;

                GetImage((int)Images.Ability2_1Image).transform.parent.GetComponent<Outline>().enabled = true;
                GetImage((int)Images.Ability2_2Image).transform.parent.GetComponent<Outline>().enabled = false;
            }
            // 2-2 특성이 찍혀있다면
            else
            {
                GetImage((int)Images.AbilityPath2_1).color = Color.black;
                GetImage((int)Images.AbilityPath2_2).color = Color.red;

                GetImage((int)Images.Ability2_1Image).transform.parent.GetComponent<Outline>().enabled = false;
                GetImage((int)Images.Ability2_2Image).transform.parent.GetComponent<Outline>().enabled = true;
            }
        }

        // 3번째 특성 찍었을 때
        if (character.Growth.abilityT3 != NONE_SELECTED)
        {
            GetImage((int)Images.AbilityPath3).color = Color.red;

            // 3-1 특성
            if (character.Growth.abilityT3 == 0)
            {
                GetImage((int)Images.AbilityPath3_1).color = Color.red;
                GetImage((int)Images.AbilityPath3_2).color = Color.black;

                GetImage((int)Images.Ability3_1Image).transform.parent.GetComponent<Outline>().enabled = true;
                GetImage((int)Images.Ability3_2Image).transform.parent.GetComponent<Outline>().enabled = false;
            }
            // 3-2 특성
            else
            {
                GetImage((int)Images.AbilityPath3_1).color = Color.black;
                GetImage((int)Images.AbilityPath3_2).color = Color.red;

                GetImage((int)Images.Ability3_1Image).transform.parent.GetComponent<Outline>().enabled = false;
                GetImage((int)Images.Ability3_2Image).transform.parent.GetComponent<Outline>().enabled = true;
            }
        }
    }

    private void InitClassTab()
    {

    }

    private void InitCharacterInfo()
    {
        GetImage((int)Images.IllustrationImage).sprite = Managers.Resource.Load<Sprite>($"{character.SO.id}");
        GetText((int)Texts.NameText).text = $"{character.SO.characterName}";

        int numberOfStars = character.Growth.star; // 별의 개수
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
        GetText((int)Texts.LevelText).text = $"Lv. {character.Growth.level} / {character.Growth.GetMaxLevel()}";
        GetText((int)Texts.ExpText).text = $"{character.Growth.curExp} / {character.Growth.maxExp}";
        GetImage((int)Images.ExpFrontImage).fillAmount = (float)character.Growth.curExp / character.Growth.maxExp;

        GetText((int)Texts.HpText).text = $"{character.hp}";
        GetText((int)Texts.AtkText).text = $"{character.atk}";
        GetText((int)Texts.DefText).text = $"{character.def}";
    }

    private void ShowTab(PlayTab tab)
    {
        // 이미 탭에 열려있는 정보를 누르면 아무것도 하지않음
        if (playTab == tab)
        {
            return;
        }

        // 현재 열려있는 탭 업데이트
        playTab = tab;

        // 세팅 초기화
        // 모든 탭 끄기
        // 프리팹에서는 모든 탭이 켜져있어야 Bind 가능
        GetObject((int)GameObjects.SkillTab).SetActive(false);
        GetObject((int)GameObjects.AbilityTab).SetActive(false);
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

            case PlayTab.Ability:
                // TODO
                // 레벨 30이 넘어야 해금
                GetObject((int)GameObjects.AbilityTab).SetActive(true);
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

    private void OnClickAbilityButton(int AbilityTier, int AbilityId)
    {
        Debug.Log("OnClickAbilityButton");

        // 특성 선택 UI 켜기
        GetObject((int)GameObjects.AbilityInfoUI).SetActive(true);

        // 이미지, 설명 세팅
        GetImage((int)Images.AbilityInfoImage).sprite = Managers.Resource.Load<Sprite>($"{AbilityId}");
        // TODO
        // 어빌리티Id로 어빌리티를 Get하여 어빌리티 설명 텍스트 적용
        // GetText((int)Texts.AbilityDescriptionText).text = Ability.abilityDescription;

        // 버튼들 모두 비활성화 상태
        GetButton((int)Buttons.AbilityApplyButton).gameObject.SetActive(false);
        GetButton((int)Buttons.AbilityCancelButton).gameObject.SetActive(false);
        GetButton((int)Buttons.AbilityCheckButton).gameObject.SetActive(false);

        switch (AbilityTier)
        {
            // 선택한게 1단계 특성인 경우 -> 항상 적용중
            case 1:
                // 선택한 특성이면 아웃라인 켜기
                SetupSelectedAbilityUI();
                break;

            case 2:
                // 선택 가능한지 체크
                if(character.Growth.level < 50)
                {
                    SetupUnselectableAbilityUI(AbilityTier);
                }
                // 이미 적용 된 특성이라면
                else if (character.Growth.abilityT2 == AbilityId)
                {
                    SetupSelectedAbilityUI();
                }
                // 적용 안된 특성이라면
                else
                {
                    SetupUnselectedAbilityUI(AbilityTier, AbilityId);
                }
                break;

            case 3:
                // 선택 가능한지 체크
                if (character.Growth.level < 70 || character.Growth.abilityT2 == NONE_SELECTED)
                {
                    SetupUnselectableAbilityUI(AbilityTier);
                }
                // 이미 적용 된 특성이라면
                else if (character.Growth.abilityT3 == AbilityId)
                {
                    SetupSelectedAbilityUI();
                }
                // 적용 안된 특성이라면
                else
                {
                    SetupUnselectedAbilityUI(AbilityTier, AbilityId);
                }
                break;
        }
    }

    // 이미 적용 된 특성 UI 세팅
    private void SetupSelectedAbilityUI()
    {
        GetImage((int)Images.AbilityInfoImage).transform.parent.GetComponent<Outline>().enabled = true;

        GetButton((int)Buttons.AbilityCheckButton).gameObject.SetActive(true);
    }

    // 적용 안된 특성 UI 세팅
    private void SetupUnselectedAbilityUI(int AbilityTier, int AbilityId)
    {
        GetImage((int)Images.AbilityInfoImage).transform.parent.GetComponent<Outline>().enabled = false;

        GetButton((int)Buttons.AbilityApplyButton).gameObject.SetActive(true);
        GetButton((int)Buttons.AbilityCancelButton).gameObject.SetActive(true);

        GetButton((int)Buttons.AbilityApplyButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.AbilityApplyButton).onClick.AddListener(() => OnClickAbilityApplyButton(AbilityTier, AbilityId));
    }

    // 특성 선택이 불가능할 때 UI 세팅
    private void SetupUnselectableAbilityUI(int AbilityTier)
    {
        GetImage((int)Images.AbilityInfoImage).transform.parent.GetComponent<Outline>().enabled = false;
        GetButton((int)Buttons.AbilityCheckButton).gameObject.SetActive(true);

        if (AbilityTier == 2)
        {
            GetText((int)Texts.AbilityDescriptionText).text += $"\n\n<color=red>* 레벨 50이상이 필요합니다.</color>";
        }
        else if (AbilityTier == 3)
        {
            GetText((int)Texts.AbilityDescriptionText).text += $"\n\n<color=red>* 레벨 70이상, 2단계 특성 활성화가 필요합니다.</color>";
        }
    }

    private void OnClickAbilityApplyButton(int AbilityTier, int selectAbilityId)
    {
        Debug.Log("OnClickAbilityApplyButton");

        if (AbilityTier == 2)
        {
            character.Growth.abilityT2 = selectAbilityId;
        }
        else if (AbilityTier == 3)
        {
            character.Growth.abilityT3 = selectAbilityId;
        }

        AbilityPathUpdate();
        UpdateStat();
        GetObject((int)GameObjects.AbilityInfoUI).SetActive(false);
    }

    private void OnClickAbilityCancelButton()
    {
        Debug.Log("OnClickAbilityCancelButton");

        GetObject((int)GameObjects.AbilityInfoUI).SetActive(false);
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
