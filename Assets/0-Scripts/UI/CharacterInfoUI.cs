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
        ExpText,
        ClassNameText,
        ClassDescriptionText
    }
    private enum Buttons
    {
        SkillButton,
        AbilityButton,
        ClassButton,
        BackButton,
        ExSkillButton,
        WeaponButton,
        ArmorButton,
        Ability1Button,
        Ability2_1Button,
        Ability2_2Button,
        Ability3_1Button,
        Ability3_2Button,
        Class1Button,
        Class2_1Button,
        Class2_2Button,
        ClassSelectButton,
        LevelUpButton,
        HomeButton
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
        Class1Image,
        Class2_1Image,
        Class2_2Image,
        ClassPath2,
        ClassPath2_1,
        ClassPath2_2
    }
    private enum GameObjects
    {
        SkillTab,
        AbilityTab,
        ClassTab,
        PassiveSkillInfoUI,
        Star
    }

    private void OnDestroy()
    {
        character.Growth.OnLevelUp -= UpdateStat;
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

        // 스킬, 특성, 장비강화 팝업 UI 비활성화 상태로 두기
        GetObject((int)GameObjects.PassiveSkillInfoUI).SetActive(false);

        // 스킬, 특성, 클래스 탭 보여주기
        GetButton((int)Buttons.SkillButton).onClick.AddListener(() => ShowTab(PlayTab.Skill));
        GetButton((int)Buttons.AbilityButton).onClick.AddListener(() => ShowTab(PlayTab.Ability));
        GetButton((int)Buttons.ClassButton).onClick.AddListener(() => ShowTab(PlayTab.Class));

        GetImage((int)Images.WeaponImage).sprite = character.weapon.sprite;
        GetImage((int)Images.ArmorImage).sprite = character.armor.sprite;

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.ExSkillButton).onClick.AddListener(OnClickExSkillButton);
        GetButton((int)Buttons.WeaponButton).onClick.AddListener(OnClickWeaponButton);
        GetButton((int)Buttons.ArmorButton).onClick.AddListener(OnClickArmorButton);
        GetButton((int)Buttons.HomeButton).onClick.AddListener(OnClickHomeButton);

        ShowTab(PlayTab.Skill);

        InitSkillTab();
        InitAbilityTab();
        InitClassTab();
        InitCharacterInfo();

        GetButton((int)Buttons.LevelUpButton).onClick.AddListener(OnClickLevelUpButton);

        character.Growth.OnLevelUp += UpdateStat;
    }

    private void OnClickLevelUpButton()
    {
        Debug.Log("OnClickLevelUpButton");

        LevelUpUI ui = Managers.UI.ShowUI<LevelUpUI>();
        ui.Init(character);
    }

    
    private void InitSkillTab()
    {
        GetText((int)Texts.ExSkillText).text = $"{character.exSkill.skillName}"; // 뒤에 레벨도 붙어야 함
        GetText((int)Texts.ExSkillDescriptionText).text = $"{character.exSkill.description}";
        GetImage((int)Images.ExSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.exSkill.skill_ID}");

        GetText((int)Texts.PassiveSkillText).text = $"{character.passiveSkill.passiveName}"; // 뒤에 레벨도 붙어야 함
        GetText((int)Texts.PassiveSkillDescriptionText).text = $"{character.passiveSkill.description}";
        GetImage((int)Images.PassiveSkillImage).sprite = Managers.Resource.Load<Sprite>($"{character.passiveSkill.id}");
    }

    private void InitAbilityTab()
    {
        // 특성 이미지 세팅
        Utility.Id2SO<AbilitySO>(character.SO.abilityT1, (result) =>
        {
            GetImage((int)Images.Ability1Image).sprite = (result as AbilitySO).icon;
        });
        Utility.Id2SO<AbilitySO>(character.SO.abilityT2[0], (result) =>
        {
            GetImage((int)Images.Ability2_1Image).sprite = (result as AbilitySO).icon;
        });
        Utility.Id2SO<AbilitySO>(character.SO.abilityT2[1], (result) =>
        {
            GetImage((int)Images.Ability2_2Image).sprite = (result as AbilitySO).icon;
        });
        Utility.Id2SO<AbilitySO>(character.SO.abilityT3[0], (result) =>
        {
            GetImage((int)Images.Ability3_1Image).sprite = (result as AbilitySO).icon;
        });
        Utility.Id2SO<AbilitySO>(character.SO.abilityT3[1], (result) =>
        {
            GetImage((int)Images.Ability3_2Image).sprite = (result as AbilitySO).icon;
        });

        
        GetButton((int)Buttons.Ability1Button).onClick.AddListener(() => OnClickAbilityButton(1, 0));
        GetButton((int)Buttons.Ability2_1Button).onClick.AddListener(() => OnClickAbilityButton(2, 0));
        GetButton((int)Buttons.Ability2_2Button).onClick.AddListener(() => OnClickAbilityButton(2, 1));
        GetButton((int)Buttons.Ability3_1Button).onClick.AddListener(() => OnClickAbilityButton(3, 0));
        GetButton((int)Buttons.Ability3_2Button).onClick.AddListener(() => OnClickAbilityButton(3, 1));
        
        AbilityPathUpdate();
    }

    // 특성 패스 & 아웃라인 세팅
    public void AbilityPathUpdate()
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
        Utility.Id2SO<ClassSO>(character.SO.basicClass, (result) =>
        {
            GetImage((int)Images.Class1Image).sprite = (result as ClassSO).icon;
            OnClickClassButton(1, 0, result as ClassSO);
            GetButton((int)Buttons.Class1Button).onClick.AddListener(() => OnClickClassButton(1, 0, result as ClassSO));
        });
        Utility.Id2SO<ClassSO>(character.SO.superiorClass[0], (result) =>
        {
            GetImage((int)Images.Class2_1Image).sprite = (result as ClassSO).icon;
            GetButton((int)Buttons.Class2_1Button).onClick.AddListener(() => OnClickClassButton(2, 0, result as ClassSO));
        });
        Utility.Id2SO<ClassSO>(character.SO.superiorClass[1], (result) =>
        {
            GetImage((int)Images.Class2_2Image).sprite = (result as ClassSO).icon;
            GetButton((int)Buttons.Class2_2Button).onClick.AddListener(() => OnClickClassButton(2, 1, result as ClassSO));
        });

        ClassPathUpdate();

        GetButton((int)Buttons.ClassSelectButton).gameObject.SetActive(false);
    }

    private void ClassPathUpdate()
    {
        // 2단계 클래스가 찍혀있다면
        if (character.Growth.superiorClass != NONE_SELECTED)
        {
            GetImage((int)Images.ClassPath2).color = Color.red;

            // 2-1 클래스가 찍혀있다면
            if (character.Growth.superiorClass == 0)
            {
                GetImage((int)Images.ClassPath2_1).color = Color.red;
                GetImage((int)Images.ClassPath2_2).color = Color.black;

                GetImage((int)Images.Class2_1Image).transform.parent.GetComponent<Outline>().enabled = true;
                GetImage((int)Images.Class2_2Image).transform.parent.GetComponent<Outline>().enabled = false;
            }
            // 2-2 클래스가 찍혀있다면
            else
            {
                GetImage((int)Images.ClassPath2_1).color = Color.black;
                GetImage((int)Images.ClassPath2_2).color = Color.red;

                GetImage((int)Images.Class2_1Image).transform.parent.GetComponent<Outline>().enabled = false;
                GetImage((int)Images.Class2_2Image).transform.parent.GetComponent<Outline>().enabled = true;
            }
        }
    }

    private void InitCharacterInfo()
    {
        GetImage((int)Images.IllustrationImage).sprite = character.SO.standing;
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
    }

    public void UpdateStat()
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
    private void OnClickExSkillButton()
    {
        Debug.Log("OnClickExSkillButton");

        ExSkillInfoUI ui = Managers.UI.ShowUI<ExSkillInfoUI>();
        ui.Init(character.SO.id);
    }

    private void OnClickAbilityButton(int AbilityTier, int AbilityIndex)
    {
        Debug.Log("OnClickAbilityButton");

        AbilityInfoUI ui = Managers.UI.ShowUI<AbilityInfoUI>();
        ui.Init(character, AbilityTier, AbilityIndex);
    }

    private void OnClickClassButton(int classTier, int classIndex, ClassSO classSO)
    {
        Debug.Log("OnClickAbilityButton");

        GetText((int)Texts.ClassNameText).text = classSO.className;
        GetText((int)Texts.ClassDescriptionText).text = classSO.description;

        // 1티어 클래스는 항상 선택되어있는 기본 클래스
        if (classTier == 1)
        {
            GetButton((int)Buttons.ClassSelectButton).gameObject.SetActive(false);
        }
        // 선택한 클래스가 현재 적용중이라면 선택 버튼 비활성화
        else if (character.Growth.superiorClass == classIndex)
        {
            GetButton((int)Buttons.ClassSelectButton).gameObject.SetActive(false);
        }
        // 선택 가능할 때 버튼 세팅
        else
        {
            GetButton((int)Buttons.ClassSelectButton).gameObject.SetActive(true);
            GetButton((int)Buttons.ClassSelectButton).onClick.RemoveAllListeners();
            GetButton((int)Buttons.ClassSelectButton).onClick.AddListener(() => OnClickClassSelectButton(classIndex));
        }
    }

    private void OnClickClassSelectButton(int classIndex)
    {
        Debug.Log($"OnClockClassSelectButton: {classIndex}");

        character.Growth.SelectClass(classIndex);

        ClassPathUpdate();
    }

    private void OnClickWeaponButton()
    {
        Debug.Log("OnClickWeaponButton");

        EquipmentUpgradeUI ui = Managers.UI.ShowUI<EquipmentUpgradeUI>();
        ui.Init(character, EquipType.Weapon);
    }
    private void OnClickArmorButton()
    {
        Debug.Log("OnClickArmorButton");

        EquipmentUpgradeUI ui = Managers.UI.ShowUI<EquipmentUpgradeUI>();
        ui.Init(character, EquipType.Armor);
    }
    
    private void OnClickHomeButton()
    {
        Debug.Log("OnClickHomeButton");

        Managers.UI.ReturnMainUI();
    }
}
