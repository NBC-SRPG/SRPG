using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class CharacterGrowth  //ĳ������ ���� / Ư�� �� Ŭ���� / ��Ÿ ��� ĳ���� ��ü�� �������� ���� �����͸��� �����ϴ� Ŭ����.
{
    private Character character;

    public TalentSO talent_Tier1 { get; set; }
    public TalentSO talent_Tier2 { get; set; }
    public TalentSO talent_Tier3 { get; set; }

    public ClassSO basicClass { get; set; }
    public ClassSO superiorClass { get; set; }

    public WeaponSO weapon { get; set; }
    public ArmorSO armor { get; set; }


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
            Managers.CharacterGrowthManager.ApplyAdditionStat(); //������ �� ���� ���� �� Ư��/Ŭ���� ���� ��������.
            maxExp = (level * 100 + maxLevel * level * 20); //���Ƿ� �ִ� ����ġ��(�������ϴµ� �ʿ��� ����ġ ��)�� ����. 
        }
    }

    public int maxLevel  //Star�� Limit ���� ���� �ٸ� ���� ������.
    {
        get
        {
            if (Limit >= 1)
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
        private set //����ġ �����ڿ� ��ü������ ������ ����� ��ġ.
        {
            exp = value;
            while (exp >= maxExp && Level < maxLevel && Level < Managers.AccountData.playerData.Level)  //������ ���� ������ ���� ���ϵ��� ���� �߰�
            {
                exp -= maxExp;
                Level += 1;
            }
            exp = math.clamp(exp, 0, maxExp);
        }
    }

    public int maxExp { get; set; }

    private int star; //����
    public int Star
    {
        get { return star; }
        set
        {
            if (Limit < 1)
            {
                star = math.clamp(value, 1, 5);
            }
            else if (Limit >= 1) //�Ѱ赹�� 1 �̻� = 5���̶�� ��
            {
                star = 5;
            }
        }
    }

    private int limit; //���� �Ѱ� ���� ����. 0 = �ѵ�x
    public int Limit
    {
        get { return limit; }
        set { 
                // Star�� 5�� ������ �Ѱ谪 ����
                if (Star >= 5)
                {
                    limit = math.clamp(value, 0, 4);
                }
        }
    }

    public int ExSkillLevel { get; set; } //Ex��ų ���� //Todo: ��ų ������ ���� ��ų ��� �����Ű��, ���� �ΰ��ӿ��� ��ų ������ ���� ȿ�� �޶����� �ϱ�.
    public int PassiveSkillLevel { get; set; } //�нú� ��ų ����

    public int weaponTier;
    public int armorTier;

    public int weaponEnhance { get; set; } //���� ��ȭ����
    public int armorEnhance { get; set; } //�� ��ȭ����
    public int affectionLevel { get; set; } //ȣ���� ����


    //CharacterGrowth에 Character을 넣고 선택중인 특성과 클래스를 CharacterData의 SO와 연결시키는 메서드.
    public void Init(Character _character)
    {
        character = _character;
        talent_Tier1 = character.characterData.talent_Tier1;
        talent_Tier2 = character.characterData.talent_Tier2[0];
        talent_Tier3 = character.characterData.talent_Tier3[0];

        basicClass = character.characterData.basicClass;
        superiorClass = character.characterData.superiorClass[0];

        weapon = character.characterData.weapon[weaponTier];
        armor = character.characterData.armor[armorTier];
    }

    //최초 초기화 메서드
    //캐릭터를 계정에서 최초로 획득 시 이 메서드를 호출할 것.
    public void InitialInit() 
    {
        star = character.characterData.basicStar; //ĳ������ ������ ȹ�� �� �⺻ ����. //�ִ� ������ �⺻ ���޿� ���� ������.
        Level = 1;
        ExSkillLevel = 1;
        PassiveSkillLevel = 1; 
        affectionLevel = 1;
    }
}
