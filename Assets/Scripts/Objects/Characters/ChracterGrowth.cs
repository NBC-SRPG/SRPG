using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static Constants;

public class CharacterGrowth  //캐릭터의 성장 / 특성 및 클래스 / 기타 등등 캐릭터 객체의 개인적인 고유 데이터만을 저장하는 클래스.
{
    private Character character;

    public TalentSO talent_Tier1 { get; set; }
    public TalentSO talent_Tier2 { get; set; }
    public TalentSO talent_Tier3 { get; set; }

    public ClassSO basicClass { get; set; }
    public ClassSO superiorClass { get; set; }


    //레벨, 최대레벨, 경험치, 최대 경험치
    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            if (Managers.AccountData.playerData.Level >= maxLevel) 
            {
                level = math.clamp(value, 1, maxLevel);
            }
            else
            {
                level = math.clamp(value, 1, Managers.AccountData.playerData.Level);
            }
            Managers.CharacterGrowthManager.Init(character);
            Managers.CharacterGrowthManager.ApplyGrowStat();
            Managers.CharacterGrowthManager.ApplyAdditionStat(); //레벨업 시 성장 스탯 및 특성/클래스 스탯 리프레시.
            maxExp = (level * 100 + maxLevel * level * 20); //임의로 최대 경험치값(레벨업하는데 필요한 경험치 값)을 설정. 
        }
    }

    public int maxLevel  //Star와 Limit 값에 따라 다른 값을 리턴함.
    {
        get
        {
            if (Limit > 1)
            {
                switch (limit)
                {
                    case 1:
                        return 75;
                    case 2:
                        return 80;
                    case 3:
                        return 85;
                    case 4:
                        return 90;
                    default:
                        return 70;
                }
            }
            else
            {
                switch (star) 
                {
                    case 1:
                        return 30;
                    case 2:
                        return 40;
                    case 3:
                        return 50;
                    case 4:
                        return 60;
                    case 5:
                        return 70;
                    default:
                        return 0;
                }
            }
        }  
    }


    private int exp;
    public int Exp
    {
        get { return exp; }
        private set //경험치 설정자에 자체적으로 레벨업 기능을 배치.
        {
            exp = value;
            while (exp >= maxExp && level < maxLevel && level < Managers.AccountData.playerData.Level)  //레벨이 계정 레벨을 넘지 못하도록 조건 추가
            {
                exp -= maxExp;
                level += 1;
            }
            exp = math.clamp(exp, 0, maxExp);
        }
    }

    public int maxExp { get; set; }

    private int star; //성급
    public int Star
    {
        get { return star; }
        set
        {
            if (Limit < 1)
            {
                star = math.clamp(value, 1, 5);
            }
            else if (Limit >= 1) //한계돌파 1 이상 = 5성이라는 뜻
            {
                star = 5;
            }
        }
    }

    private int limit; //현재 한계 돌파 정도. 0 = 한돌x
    public int Limit
    {
        get { return limit; }
        set { 
                // Star가 5일 때에만 한계값 설정
                if (Star >= 5)
                {
                    limit = math.clamp(value, 0, 4);
                }
        }
    }

    public int ExSkillLevel { get; set; } //Ex스킬 레벨 //Todo: 스킬 레벨에 따라 스킬 계수 적용시키기, 실제 인게임에서 스킬 레벨에 따라 효과 달라지게 하기.
    public int PassiveSkillLevel { get; set; } //패시브 스킬 레벨

    public int weaponLevel { get; set; } //무기 레벨
    public int armorLevel { get; set; } //방어구 레벨
    public int affectionLevel { get; set; } //호감도 레벨


    //CharacterGrowth에 Character을 넣고 선택중인 특성과 클래스를 CharacterData의 SO와 연결시키는 메서드.
    public void Init(Character _character)
    {
        character = _character;
        talent_Tier1 = character.characterData.talent_Tier1;
        talent_Tier2 = character.characterData.talent_Tier2[0];
        talent_Tier3 = character.characterData.talent_Tier3[0];

        basicClass = character.characterData.basicClass;
        superiorClass = character.characterData.superiorClass[0];
    }

    //최초 초기화 메서드
    //캐릭터를 계정에서 최초로 획득 시 이 메서드를 호출할 것.
    public void InitialInit() 
    {
        star = character.characterData.basicStar; //캐릭터의 성급은 획득 시 기본 성급. //최대 레벨은 기본 성급에 의해 정해짐.
        Level = 1;
        weaponLevel = 1;
        armorLevel = 1;
        ExSkillLevel = 1;
        PassiveSkillLevel = 1; 
        affectionLevel = 1;
    }
}
