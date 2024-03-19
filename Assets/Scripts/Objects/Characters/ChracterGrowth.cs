using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static Constants;

public class CharacterGrowth : MonoBehaviour
{
    private Character character;

    //'���� �������� Ư�� ��ȣ' ( 0 = None )
    //�� �������� ������ �迭�� index ��ȣ�� �����Ѵ�.
    public int selectTalent_Tier2 { get; private set; }
    public int selectTalent_Tier3 { get; private set; }
    public int selectSuperialClass { get; private set; }


    //'���� ���� ���� Ư�� ��ȣ�� Ư��/Ŭ������ SO �� ������' 
    public TalentSO talent_Tier1 { get; private set; }
    public TalentSO talent_Tier2 { get; private set; }
    public TalentSO talent_Tier3 { get; private set; }

    public ClassSO basicClass { get; private set; }
    public ClassSO superiorClass { get; private set; }


    //����, �ִ뷹��, ����ġ, �ִ� ����ġ
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
            ApplyGrowStat();
            ApplyAdditionStat(); //������ �� ���� ���� �� Ư��/Ŭ���� ���� ��������.
            maxExp = (level * 100 + maxLevel * level * 20); //���Ƿ� �ִ� ����ġ��(�������ϴµ� �ʿ��� ����ġ ��)�� ����. 
        }
    }

    public int maxLevel { get; private set; }

    private int exp;
    public int Exp
    {
        get { return exp; }
        private set //����ġ �����ڿ� ��ü������ ������ ����� ��ġ�߽��ϴ�.
        {
            exp = value;
            while (exp >= maxExp && level < maxLevel && level < Managers.AccountData.playerData.Level)  //������ ���� ������ ���� ���ϵ��� ���� �߰�
            {
                exp -= maxExp;
                level += 1;
            }
            exp = math.clamp(exp, 0, maxExp);
        }
    }

    public int maxExp { get; private set; }

    public Star star { get; private set; } //����

    public int ExSkillLevel { get; private set; } //Ex��ų ���� //Todo: ��ų ������ ���� ��ų ��� �����Ű��, ���� �ΰ��ӿ��� ��ų ������ ���� ȿ�� �޶����� �ϱ�.
    public int PassiveSkillLevel { get; private set; } //�нú� ��ų ����

    public int weaponLevel { get; private set; } //���� ����
    public int armorLevel { get; private set; } //�� ����
    public int affectionLevel { get; private set; } //ȣ���� ����


    //CharacterGrowth�� Character�� �ְ� �������� Ư���� Ŭ������ CharacterData�� SO�� �����Ű�� �޼���.
    public void Init(Character _character)
    {
        character = _character;
        talent_Tier1 = character.characterData.talent_Tier1;
        talent_Tier2 = character.characterData.talent_Tier2[selectTalent_Tier2 -1];
        talent_Tier3 = character.characterData.talent_Tier3[selectTalent_Tier3 -1];

        basicClass = character.characterData.basicClass;
        superiorClass = character.characterData.superiorClass[selectSuperialClass -1];
    }

    //���� �ʱ�ȭ �޼���
    //ĳ���͸� �������� ���ʷ� ȹ�� �� �� �޼��带 ȣ���� ��.
    public void InitialInit() 
    {
        star = character.characterData.defaltStar; //ĳ������ ������ ȹ�� �� �⺻ ����.
        Level = 1;
        maxLevel = (int)star; //�ִ� ������ �⺻ ���޿� ���� ������.
        weaponLevel = 1;
        armorLevel = 1;
        ExSkillLevel = 1;
        PassiveSkillLevel = 1; 
        affectionLevel = 1;
    }

    //2Ƽ�� Ư�� ���� �� ����ϴ� �޼���. UI�� ���� �ʿ���
    public bool SelectTalent_tier2(int select)
    {
        if (select > 0 && select <= character.characterData.talent_Tier2.Length - 1)
        {
            if (Level >= 50)
            {
                selectTalent_Tier2 = select;
                ApplyAdditionStat();
                return true;
            }
            else
            {
                Debug.Log("2Ƽ�� Ư�� ���� ������ �������� ���߽��ϴ�.");
                return false;
            }
        }
        else
        {
            Debug.Log("�߸��� ���ٰ��Դϴ�.");
            return false;
        }
    }

    //3Ƽ�� Ư�� ���� �� ����ϴ� �޼���. UI�� ���� �ʿ���
    public bool SelectTalent_tier3(int select)
    {
        if (select > 0 && select <= character.characterData.talent_Tier3.Length - 1)
        {
            if (level >= 70 && selectTalent_Tier2 != 0)
            {
                selectTalent_Tier3 = select;
                ApplyAdditionStat();
                return true;
            }
            else
            {
                Debug.Log("3Ƽ�� Ư�� ���� ������ �������� ���߽��ϴ�.");
                return false;
            }
        }
        else
        {
            Debug.Log("�߸��� ���ٰ��Դϴ�.");
            return false;
        }
    }

    //���� Ŭ���� ���� �� ����ϴ� �޼���. UI�� ���� �ʿ���.
    public bool SelectSuperialClass(int select)
    {
        if (select > 0 && select <= character.characterData.superiorClass.Length - 1)
        {
            if (level >= 60)
            {
                selectSuperialClass = select;
                ApplyAdditionStat();
                return true;
            }
            else
            {
                Debug.Log("���� Ŭ���� ���� ������ �������� ���߽��ϴ�.");
                return false;
            }
        }
        else
        {
            Debug.Log("�߸��� ���ٰ��Դϴ�.");
            return false;
        }
    }

    public int LimitBreak(int piece) //�ӽ÷� ������ �Ѱ� ���� �޼���. ĳ���� ���� ���� üũ �� ���� �Ҹ� ��� ���� �ʿ�.
                                     //Todo: ���� ������ �����͸� �޾ƿͼ� ������ üũ�ϰ�, ���Ľ� ���� �����ϵ��� ��� �����ϱ�.
    {
        if (star == Star.LimitBreak_4) //�Ѱ� ���İ� �̹� �ִ�ġ�� ���� �Ұ���.
        {
            return 0;
        }
        if (level == maxLevel && piece >= (int)star + 30) //���� ĳ���� ������ �ִ�ġ�̰�, ���� ���� ������ ���� ��ǰ� �̻��� ���. (���� ��ǰ� = ���� �ִ뷹�� + 30) 
        {
            int paidPiece = (int)star + 30;
            if ((int)star <= 70) //���� ������ 5�� �̸��� ���, �ִ� ���� 10 ����.
            {
                star = (Star)((int)star + 10);
            }
            else //���� ������ 5�� �̻��� ���, �ִ� ���� 5 ����.
            {
                star = (Star)((int)star + 5);
            }
            maxLevel = (int)star;
            return paidPiece; //�����ؾ��ϴ� ���� ���� �����Ѵ�.
        }
        else
        {
            //���� ��� ������.
            return 0;
        }
    }

    public void ApplyGrowStat() //ĳ���� ���� ����(����) ���� �޼���. �������� �ٲ� ������ ȣ��ȴ�. ����� �� Character�� �⺻ ���ȿ� �����Ѵ�. 
    {
        //ĳ������ ���� ���� (�⺻ ���Ȱ� + ������ ���� ������ ��ġ��)���� �����Ѵ�.
        //���� : ���Ȱ� = ����ĳ������ �⺻ ���� + (���� ĳ������ ���� ���� * �� ĳ���� ��ü�� ���� ����)
        character.Health = (character.characterData.health + (character.characterData.growHealth * level));
        character.Attack = (character.characterData.atk + (character.characterData.growAtk * level));
        character.Defence = (character.characterData.def + (character.characterData.growDef * level));
    }

    public void ApplyAdditionStat() //ĳ���� Ư�� / Ŭ���� / ��� ���� ���� �޼���, ����� �� ���� Character�� Calc ���ȿ� �����Ѵ�. 
    {
        //Ư������ �ö󰡴� �ɷ�ġ���� �հ踦 ������ ������ �����Ѵ�.
        int talentIncrHp_sum = 0; //ü�� ��� ����ġ �հ�
        int talentIncrDef_sum = 0; //����
        int talentIncrAtk_sum = 0; //���ݷ�
        int talentIncrMov_sum = 0; //�̵��Ÿ�

        float talentIncrCtr_sum = 0; //ġ��Ÿ Ȯ�� ( ��ġ 0.1 = ġ��Ÿ Ȯ�� 10%����)
        float talentIncrCtd_sum = 0; //ġ��Ÿ ����
        float talentIncrInfD_sum = 0; //�ִ� ������ ����
        float talentIncrTakenD_sum = 0; //�޴� ���� ����

        float talentMultiplHp_sum = 0; //ü�� ���� ����ġ �հ�
        float talentMultiplDef_sum = 0; //���� ���� ����ġ �հ�
        float talentMultiplAtk_sum = 0; //���ݷ� ���� ����ġ �հ�

        if (level >= 30) //������ 30 �̻��̾ 1�� Ư���� �رݵǾ����� Ȯ���Ѵ�.
        {
            //Ƽ��1 Ư���� �ϳ� ���̹Ƿ�, ĳ����SO�� ���� 1Ƽ�� Ư���� �������� ������ �հ迡 �����Ѵ�.
            talentIncrHp_sum = talent_Tier1.increaseHealth;
            talentIncrDef_sum = talent_Tier1.increaseDef;
            talentIncrAtk_sum = talent_Tier1.increaseAtk;
            talentIncrMov_sum = talent_Tier1.increasecMov;

            talentIncrCtr_sum = talent_Tier1.increasecCtr;
            talentIncrCtd_sum = talent_Tier1.increasecCtd;
            talentIncrInfD_sum = talent_Tier1.increaseInflictDamage;
            talentIncrTakenD_sum = talent_Tier1.reducedTakenDamage;

            talentMultiplHp_sum = talent_Tier1.multiplyHealth;
            talentMultiplDef_sum = talent_Tier1.multiplyDef;
            talentMultiplAtk_sum = talent_Tier1.multiplyAtk;

            if (talent_Tier2 != null) //'������ 2Ƽ�� Ư��'���� 0�̶�� Ư���� ������� �ʾҰų� ���� �������� ���� ���.
            {
                //selectTrait_Tier ���� 0�� �ƴ϶�� Ư���� �����ߴٴ� ��. ĳ����SO�� ���� '���ð����� Ƽ�� 2 Ư�� �迭'���� ��ġ�ϴ� Ư���� ������ �� �������� �հ迡 ���Ѵ�. 
                talentIncrHp_sum += talent_Tier2.increaseHealth;
                talentIncrDef_sum += talent_Tier2.increaseDef;
                talentIncrAtk_sum += talent_Tier2.increaseAtk;
                talentIncrMov_sum += talent_Tier2.increasecMov;

                talentIncrCtr_sum += talent_Tier2.increasecCtr;
                talentIncrCtd_sum += talent_Tier2.increasecCtd;
                talentIncrInfD_sum += talent_Tier2.increaseInflictDamage;
                talentIncrTakenD_sum += talent_Tier2.reducedTakenDamage;

                talentMultiplHp_sum += talent_Tier2.multiplyHealth;
                talentMultiplDef_sum += talent_Tier2.multiplyDef;
                talentMultiplAtk_sum += talent_Tier2.multiplyAtk;
                if (talent_Tier3 != null)
                {
                    //3Ƽ�� Ư���� ���������� ����ȴ�.
                    talentIncrHp_sum += talent_Tier3.increaseHealth;
                    talentIncrDef_sum += talent_Tier3.increaseDef;
                    talentIncrAtk_sum += talent_Tier3.increaseAtk;
                    talentIncrMov_sum += talent_Tier3.increasecMov;

                    talentIncrCtr_sum += talent_Tier3.increasecCtr;
                    talentIncrCtd_sum += talent_Tier3.increasecCtd;
                    talentIncrInfD_sum += talent_Tier3.increaseInflictDamage;
                    talentIncrTakenD_sum += talent_Tier3.reducedTakenDamage;

                    talentMultiplHp_sum += talent_Tier2.multiplyHealth;
                    talentMultiplDef_sum += talent_Tier2.multiplyDef;
                    talentMultiplAtk_sum += talent_Tier2.multiplyAtk;
                }
            }
        }

        //Ŭ������ ��� �⺻ Ŭ����(1�������� ����)�� ���� ����ǰ�, ���� Ŭ������ �رݵǾ��� �� Ŭ������ �����ߴ��� üũ�� ���� Ŭ���� ���� ����ġ�� �ջ��Ѵ�.
        int classincrHp_sum = basicClass.increaseHealth;
        int classincrDef_sum = basicClass.increaseDef;
        int classincrAtk_sum = basicClass.increaseAtk;
        int classincrMov_sum = basicClass.increasecMov;

        float classIncrCtr_sum = basicClass.increasecCtr;
        float classIncrCtd_sum = basicClass.increasecCtd;
        float classIncrInfD_sum = basicClass.increaseInflictDamage;
        float classIncrTakenD_sum = basicClass.reducedTakenDamage;

        float classMultiplHp_sum = basicClass.multiplyHealth;
        float classMultiplDef_sum = basicClass.multiplyDef;
        float classMultiplAtk_sum = basicClass.multiplyAtk;

        if (superiorClass != null)
        {
            classincrHp_sum += superiorClass.increaseHealth;
            classincrDef_sum += superiorClass.increaseDef;
            classincrHp_sum += superiorClass.increaseAtk;
            classincrHp_sum += superiorClass.increasecMov;

            classIncrCtr_sum += superiorClass.increasecCtr;
            classIncrCtd_sum += superiorClass.increasecCtd;
            classIncrInfD_sum += superiorClass.increaseInflictDamage;
            classIncrTakenD_sum += superiorClass.reducedTakenDamage;

            classMultiplHp_sum = superiorClass.multiplyHealth;
            classMultiplHp_sum = superiorClass.multiplyDef;
            classMultiplHp_sum = superiorClass.multiplyAtk;
        }

        //��� ����.
        //����� ȹ���� �� �ִ� ���� = ���ݷ�, ġȮ, ġ��, �ִ� ��������
        //���� ȹ���� �� �ִ� ���� = ü��, ����, �޴� ���� ����
        int weaponincrAtk_sum = (character.characterData.weapon.increaseAtk * weaponLevel) ;
        float weaponMultiplAtk_sum = (character.characterData.weapon.multiplyAtk * weaponLevel);
        float weaponIncrCtr_sum = (character.characterData.weapon.increasecCtr * weaponLevel);
        float weaponIncrCtd_sum = (character.characterData.weapon.increasecCtd * weaponLevel);
        float weaponIncrInfD_sum = (character.characterData.weapon.increaseInflictDamage * weaponLevel);

        int armorincrHp_sum = (character.characterData.armor.increaseHealth * armorLevel);
        int armorincrDef_sum = (character.characterData.armor.increaseDef * armorLevel);
        float armorMultiplHp_sum = (character.characterData.armor.multiplyHealth * armorLevel);
        float armorMultiplDef_sum = (character.characterData.armor.multiplyDef * armorLevel);
        float armorIncrTakenD_sum = (character.characterData.armor.reducedTakenDamage * armorLevel);


        //�̷��� ������ �հ�ġ�� Calc ���ȿ� ����ؼ� �����Ѵ�.
        //���� ������ : Calc ���� = (�⺻ ���Ȱ� + �տ��� ��� ��ġ���� ��) *(������ ���� ��ġ���� ��)
        //���� �� : Calc ���� = (int)(  (�⺻ ���Ȱ� + Ư������ �����ϴ� ���� �հ�ġ + Ŭ������ �����ϴ� ���� �հ�ġ) * ( 1 + Ư������ �����ϴ� ���� ���� + Ŭ������ �����ϴ� ���� ����)  )
        character.CalcHealth = (int)((character.Health + talentIncrHp_sum + classincrHp_sum + armorincrHp_sum) * (1 + talentMultiplHp_sum + classMultiplHp_sum + armorMultiplHp_sum));
        character.CalcDef = (int)((character.Defence + talentIncrDef_sum + classincrDef_sum + armorincrDef_sum) * (1 + talentMultiplDef_sum + classMultiplDef_sum + armorMultiplDef_sum));
        character.CalcAtk = (int)((character.Attack + talentIncrAtk_sum + classincrAtk_sum + weaponincrAtk_sum) * (1 + talentMultiplAtk_sum + classMultiplAtk_sum + weaponMultiplAtk_sum));
        character.CalcMov = ((character.Mov + talentIncrMov_sum + classincrMov_sum));

        character.CalcCrtRate = (character.CriticalRate + talentIncrCtr_sum + classIncrCtr_sum + weaponIncrCtr_sum);
        character.CalcCrtDMG = (character.CriticalDMG + talentIncrCtd_sum + classIncrCtd_sum + weaponIncrCtd_sum);
        character.CalcInflictDMGRatio = (character.InflictDamageRatio + talentIncrInfD_sum + classIncrInfD_sum + weaponIncrInfD_sum);
        character.CalcTakenDMGRatio = (character.TakenDamageRatio + talentIncrTakenD_sum + classIncrTakenD_sum + armorIncrTakenD_sum);
        //�⺻ ���� ���� Calc ���Ȱ��� �и��Ǿ������Ƿ�, ���� �ΰ��� ���������� Calc������ ������ּ���.
    }

    public bool WeaponLevelUp(int ingredient)
    {
        if (ingredient >= weaponLevel && weaponLevel < level)
        {
            weaponLevel += 1;
            //Todo : ��� �Ҹ�
            return true;
        }
        else
        {
            return false;
        }
    }//�ӽ÷� ������ ������ ���� ������ �޼���. //Todo: ��� ������ ��� �� �䱸 ������. ��� ��� ���, ��� ������ �� ���� ���� ���� �ʿ�.

    public bool ArmorLevelUp(int ingredient)
    {
        if (ingredient >= weaponLevel && weaponLevel < level)
        {
            armorLevel += 1;
            //Todo : ��� �Ҹ�
            return true;
        }
        else
        {
            return false;
        }
    }//�ӽ÷� ������ ������ �� ������ �޼���. //Todo: ���� ����.

    public bool ExSkillLevelUp(int ingredient)
    {
        if (ingredient >= ExSkillLevel && ExSkillLevel < 5)
        {
            ExSkillLevel += 1;
            //Todo : ��� �Ҹ�
            return true;
        }
        else
        {
            return false;
        }
    }//�ӽ÷� ������ ������ ��ų ������ �޼���, //Todo: ��ų ������ ��ȭ �Ҹ�, ��ų ������ �� ȿ�� ��

    public bool AffectionLevelUp(int ingredient)
    {
        if (ingredient >= affectionLevel && affectionLevel < 99)
        {
            affectionLevel += 1;
            //Todo : ��� �Ҹ�
            return true;
        }
        else
        {
            return false;
        }
    }//�ӽ÷� ������ ������ ȣ���� ������ �޼���, //Todo: ȣ���� ������ ��ȭ �Ҹ�, ȣ���� ���� �� �߰� ȿ�� ��.
}
