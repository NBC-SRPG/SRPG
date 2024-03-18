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
    public int selectTrait_Tier2 { get; private set; }
    public int selectTrait_Tier3 { get; private set; }
    public int selectSuperialClass { get; private set; }


    //'���� ���� ���� Ư�� ��ȣ�� Ư��/Ŭ������ SO �� ������' 
    public TraitSO trait_Tier1 { get; private set; }
    public TraitSO trait_Tier2 { get; private set; }
    public TraitSO trait_Tier3 { get; private set; }

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
        trait_Tier1 = character.characterData.trait_Tier1;
        trait_Tier2 = character.characterData.trait_Tier2[selectTrait_Tier2 -1];
        trait_Tier3 = character.characterData.trait_Tier3[selectTrait_Tier3 -1];

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
    public bool SelectTrait_tier2(int select)
    {
        if (select > 0 && select <= character.characterData.trait_Tier2.Length - 1)
        {
            if (Level >= 50)
            {
                selectTrait_Tier2 = select;
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
    public bool SelectTrait_tier3(int select)
    {
        if (select > 0 && select <= character.characterData.trait_Tier3.Length - 1)
        {
            if (level >= 70 && selectTrait_Tier2 != 0)
            {
                selectTrait_Tier3 = select;
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
        int traitIncrHp_sum = 0; //ü�� ��� ����ġ �հ�
        int traitIncrDef_sum = 0; //����
        int traitIncrAtk_sum = 0; //���ݷ�
        int traitIncrMov_sum = 0; //�̵��Ÿ�

        float traitIncrCtr_sum = 0; //ġ��Ÿ Ȯ�� ( ��ġ 0.1 = ġ��Ÿ Ȯ�� 10%����)
        float traitIncrCtd_sum = 0; //ġ��Ÿ ����
        float traitIncrInfD_sum = 0; //�ִ� ������ ����
        float traitIncrTakenD_sum = 0; //�޴� ���� ����

        float traitMultiplHp_sum = 0; //ü�� ���� ����ġ �հ�
        float traitMultiplDef_sum = 0; //���� ���� ����ġ �հ�
        float traitMultiplAtk_sum = 0; //���ݷ� ���� ����ġ �հ�

        if (level >= 30) //������ 30 �̻��̾ 1�� Ư���� �رݵǾ����� Ȯ���Ѵ�.
        {
            //Ƽ��1 Ư���� �ϳ� ���̹Ƿ�, ĳ����SO�� ���� 1Ƽ�� Ư���� �������� ������ �հ迡 �����Ѵ�.
            traitIncrHp_sum = trait_Tier1.increaseHealth;
            traitIncrDef_sum = trait_Tier1.increaseDef;
            traitIncrAtk_sum = trait_Tier1.increaseAtk;
            traitIncrMov_sum = trait_Tier1.increasecMov;

            traitIncrCtr_sum = trait_Tier1.increasecCtr;
            traitIncrCtd_sum = trait_Tier1.increasecCtd;
            traitIncrInfD_sum = trait_Tier1.increaseInflictDamage;
            traitIncrTakenD_sum = trait_Tier1.reducedTakenDamage;

            traitMultiplHp_sum = trait_Tier1.multiplyHealth;
            traitMultiplDef_sum = trait_Tier1.multiplyDef;
            traitMultiplAtk_sum = trait_Tier1.multiplyAtk;

            if (trait_Tier2 != null) //'������ 2Ƽ�� Ư��'���� 0�̶�� Ư���� ������� �ʾҰų� ���� �������� ���� ���.
            {
                //selectTrait_Tier ���� 0�� �ƴ϶�� Ư���� �����ߴٴ� ��. ĳ����SO�� ���� '���ð����� Ƽ�� 2 Ư�� �迭'���� ��ġ�ϴ� Ư���� ������ �� �������� �հ迡 ���Ѵ�. 
                traitIncrHp_sum += trait_Tier2.increaseHealth;
                traitIncrDef_sum += trait_Tier2.increaseDef;
                traitIncrAtk_sum += trait_Tier2.increaseAtk;
                traitIncrMov_sum += trait_Tier2.increasecMov;

                traitIncrCtr_sum += trait_Tier2.increasecCtr;
                traitIncrCtd_sum += trait_Tier2.increasecCtd;
                traitIncrInfD_sum += trait_Tier2.increaseInflictDamage;
                traitIncrTakenD_sum += trait_Tier2.reducedTakenDamage;

                traitMultiplHp_sum += trait_Tier2.multiplyHealth;
                traitMultiplDef_sum += trait_Tier2.multiplyDef;
                traitMultiplAtk_sum += trait_Tier2.multiplyAtk;
                if (trait_Tier3 != null)
                {
                    //3Ƽ�� Ư���� ���������� ����ȴ�.
                    traitIncrHp_sum += trait_Tier3.increaseHealth;
                    traitIncrDef_sum += trait_Tier3.increaseDef;
                    traitIncrAtk_sum += trait_Tier3.increaseAtk;
                    traitIncrMov_sum += trait_Tier3.increasecMov;

                    traitIncrCtr_sum += trait_Tier3.increasecCtr;
                    traitIncrCtd_sum += trait_Tier3.increasecCtd;
                    traitIncrInfD_sum += trait_Tier3.increaseInflictDamage;
                    traitIncrTakenD_sum += trait_Tier3.reducedTakenDamage;

                    traitMultiplHp_sum += trait_Tier2.multiplyHealth;
                    traitMultiplDef_sum += trait_Tier2.multiplyDef;
                    traitMultiplAtk_sum += trait_Tier2.multiplyAtk;
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
        character.CalcHealth = (int)((character.Health + traitIncrHp_sum + classincrHp_sum + armorincrHp_sum) * (1 + traitMultiplHp_sum + classMultiplHp_sum + armorMultiplHp_sum));
        character.CalcDef = (int)((character.Defence + traitIncrDef_sum + classincrDef_sum + armorincrDef_sum) * (1 + traitMultiplDef_sum + classMultiplDef_sum + armorMultiplDef_sum));
        character.CalcAtk = (int)((character.Attack + traitIncrAtk_sum + classincrAtk_sum + weaponincrAtk_sum) * (1 + traitMultiplAtk_sum + classMultiplAtk_sum + weaponMultiplAtk_sum));
        character.CalcMov = ((character.Mov + traitIncrMov_sum + classincrMov_sum));

        character.CalcCrtRate = (character.CriticalRate + traitIncrCtr_sum + classIncrCtr_sum + weaponIncrCtr_sum);
        character.CalcCrtDMG = (character.CriticalDMG + traitIncrCtd_sum + classIncrCtd_sum + weaponIncrCtd_sum);
        character.CalcInflictDMGRatio = (character.InflictDamageRatio + traitIncrInfD_sum + classIncrInfD_sum + weaponIncrInfD_sum);
        character.CalcTakenDMGRatio = (character.TakenDamageRatio + traitIncrTakenD_sum + classIncrTakenD_sum + armorIncrTakenD_sum);
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
