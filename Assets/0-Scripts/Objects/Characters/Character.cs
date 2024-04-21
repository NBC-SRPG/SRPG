using UnityEngine;
public class Character
{
    public CharacterSO SO;
    public CharacterGrowth Growth;
    public EnemySO enemySO;     // enemy로 사용할 경우에만 사용

    [Header("VisibleStatus")]
    public int hp;
    public int atk;
    public int def;
    public int mov;


    [Header("InvisibleStatus")]
    public float atkIncrease;     // 공격력 %증가량 (곱연산)
    public float defIncrease;     // 방어력 %증가량 (곱연산)

    public int critRate;       // 치명타 확률 (합연산)
    public int critDmg;         // 치명타 데미지 (합연산)

    public float EnhancedDmg;     // 데미지 증가 (합연산)
    public float ReducedDmg;     // 받는 데미지 감소 (곱연산)


    [Header("Skill")]
    // TODO: refactor with chai227chai
    public ExSkillSO exSkill;
    public PassiveSkillSO passiveSkill;

    public AbilitySO abilityT1;
    public AbilitySO abilityT2;
    public AbilitySO abilityT3;

    public ClassSO basicClass;
    public ClassSO superiorClass;

    public EquipSO weapon;
    public EquipSO armor;


    public Character(CharacterSO SO, CharacterGrowth Growth)
    {
        this.SO = SO;
        this.Growth = Growth;
        enemySO = null;

        mov = SO.mov;

        atkIncrease = 1f;
        defIncrease = 1f;

        critRate = 20;      // 기본 치명타 확률 20%
        critDmg = 50;       // 기본 치명타 데미지 50%

        EnhancedDmg = 1f;
        ReducedDmg = 1f;

        // EX스킬 초기화
        Utility.Id2SO<ExSkillSO>(SO.id, (result) =>
        {
            exSkill = (ExSkillSO)result;
        });

        // passiveSkill 초기화
        Utility.Id2SO<PassiveSkillSO>(SO.id, (result) =>
        {
            passiveSkill = (PassiveSkillSO)result;
        });

        // 특성 초기화
        Utility.Id2SO<AbilitySO>(SO.abilityT1, (result) =>
        {
            abilityT1 = (AbilitySO)result;
        });
        LoadAbilityT2();
        LoadAbilityT3();


        // 클래스 초기화
        Utility.Id2SO<ClassSO>(SO.basicClass, (result) =>
        {
            basicClass = (ClassSO)result;
        });

        if (Growth.superiorClass !=-1)
        {
            Utility.Id2SO<ClassSO>(SO.superiorClass[Growth.superiorClass], (result) =>
        {
            superiorClass = (ClassSO)result;
        });
        }

        // 장비 초기화
        Utility.Id2SO<EquipSO>(SO.weapon[Growth.weapon], (result) =>
        {
            weapon = (EquipSO)result;
            LoadArmor();
        });

        // TODO
        // MonoBehaviour를 상속받지 않아 해당 이벤트를 언제 해제 할지??
        Growth.OnLevelUp += CalculateStat;
        Growth.OnAbilityT2Changed += LoadAbilityT2;
        Growth.OnAbilityT3Changed += LoadAbilityT3;
        Growth.OnWeaponChanged += LoadWeapon;
        Growth.OnArmorChanged += LoadArmor;
    }

    public Character(EnemySO so)
    {
        Growth = new CharacterGrowth { level = so.level };
        enemySO = so;

        SO = new CharacterSO();

        SO.range = so.range;
        SO.attackMethod = so.attackMethod;

        SO.id = enemySO.id;
        SO.animatorName = enemySO.animatorName;
        SO.icon = enemySO.icon;
        SO.characterName = enemySO.characterName;
        SO.story = enemySO.story;
        SO.faction = enemySO.faction;

        hp = so.hp;
        atk = so.atk;
        def = so.def;
        mov = so.mov;

        atkIncrease = 0;
        defIncrease = 0;

        critRate = 20;      // 기본 치명타 확률 20%
        critDmg = 50;       // 기본 치명타 데미지 50%

        EnhancedDmg = 0;
        ReducedDmg = 0;

        exSkill = so.exSkillSO;
        passiveSkill =  so.passiveSkillSO;
        abilityT1 = so.abilityT1;
        abilityT2 = so.abilityT2;
        abilityT3 = so.abilityT3;
        basicClass = so.basicClass;
        superiorClass = so.superiorClass;
        weapon = so.weapon;
        armor = so.armor;
    }

    private void CalculateStat()
    {
        hp = SO.hp + SO.hpPerLv * Growth.level + weapon.hp + armor.hp;
        atk = SO.atk + SO.atkPerLv * Growth.level + weapon.atk + armor.atk;
        def = SO.def + SO.defPerLv * Growth.level + weapon.def + armor.def;
    }

    private void LoadAbilityT2()
    {
        if (Growth.abilityT2 !=-1)
        {
            Utility.Id2SO<AbilitySO>(SO.abilityT2[Growth.abilityT2], (result) =>
            {
                abilityT2 = (AbilitySO)result;
            });
        }
    }

    private void LoadAbilityT3()
    {
        if (Growth.abilityT3 !=-1)
        {
            Utility.Id2SO<AbilitySO>(SO.abilityT3[Growth.abilityT3], (result) =>
            {
                abilityT3 = (AbilitySO)result;
            });
        }
    }

    private void LoadWeapon()
    {
        Utility.Id2SO<EquipSO>(SO.weapon[Growth.weapon], (result) =>
        {
            weapon = (EquipSO)result;
            CalculateStat();
            Managers.UI.FindUI<CharacterInfoUI>().UpdateStat();
        });
    }

    private void LoadArmor()
    {
        Utility.Id2SO<EquipSO>(SO.armor[Growth.armor], (result) =>
        {
            armor = (EquipSO)result;
            CalculateStat();

            // TODO
            // 비동기 처리인지라 여기에서 CharacterInfoUI를 찾고 UI를 업데이트
            // 구조가 맘에 안듦..
            Managers.UI.FindUI<CharacterInfoUI>().UpdateStat();
        });
    }

    public (int hp, int atk, int def) PreviewEnhancedStats(EquipSO newWeapon, EquipSO newArmor)
    {
        int previewHp = SO.hp + SO.hpPerLv * Growth.level + newWeapon.hp + newArmor.hp;
        int previewAtk = SO.atk + SO.atkPerLv * Growth.level + newWeapon.atk + newArmor.atk;
        int previewDef = SO.def + SO.defPerLv * Growth.level + newWeapon.def + newArmor.def;

        return (previewHp, previewAtk, previewDef);
    }
}
