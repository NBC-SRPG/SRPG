using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Constants;

public class CharacterGrowthManager //ĳ���� ���� / Ư�� �� Ŭ���� ���� / ���� ��� �޼��� �� �����Ͱ� �ƴ� �޼��常�� ���� �����ϴ� Ŭ����.
{
    private Character character;
    public void Init(Character _character)
    {
        character = _character;
    }

    //2Ƽ�� Ư�� ���� �� ����ϴ� �޼���. UI�� ���� �ʿ���
    public void SelectTalent_tier2(TalentSO select)
    {
        character.characterGrowth.talent_Tier2 = select;
        ApplyAdditionStat();
    }

    //3Ƽ�� Ư�� ���� �� ����ϴ� �޼���. UI�� ���� �ʿ���
    public void SelectTalent_tier3(TalentSO select)
    {
        character.characterGrowth.talent_Tier3 = select;
        ApplyAdditionStat();
    }

    //���� Ŭ���� ���� �� ����ϴ� �޼���. UI�� ���� �ʿ���.
    public void SelectSuperialClass(ClassSO select)
    {
        character.characterGrowth.superiorClass = select;
        ApplyAdditionStat();
    }

    public int LimitBreak(int piece) //�ӽ÷� ������ �Ѱ� ���� �޼���. ĳ���� ���� ���� üũ �� ���� �Ҹ� ��� ���� �ʿ�.
                                     //Todo: ���� ������ �����͸� �޾ƿͼ� ������ üũ�ϰ�, ���Ľ� ���� �����ϵ��� ��� �����ϱ�.
    {
        if (character.characterGrowth.Limit >= 4) //�Ѱ� ���İ� �̹� �ִ�ġ�� ���� �Ұ���.
        {
            return 0;
        }
        if (character.characterGrowth.Level == character.characterGrowth.maxLevel && piece >= character.characterGrowth.maxLevel + 30) //���� ĳ���� ������ �ִ�ġ�̰�, ���� ���� ������ ���� ��ǰ� �̻��� ���. (���� ��ǰ� = ���� �ִ뷹�� + 30) 
        {
            int paidPiece = character.characterGrowth.maxLevel + 30;
            if (character.characterGrowth.Star < 5) //���� ������ 5�� �̸��� ���, �ִ� ���� 10 ����.
            {
                character.characterGrowth.Star += 1;
            }
            else if (character.characterGrowth.Star >= 5 && character.characterGrowth.Limit < 4) //���� ������ 5�� �̻��̰� �Ѱ谡 4 �̸��� ���, �ִ� ���� 5 ����.
            {
                character.characterGrowth.Limit += 1; 
            }
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
        character.Health = (character.characterData.health + (character.characterData.growHealth * character.characterGrowth.Level));
        character.Attack = (character.characterData.atk + (character.characterData.growAtk * character.characterGrowth.Level));
        character.Defence = (character.characterData.def + (character.characterData.growDef * character.characterGrowth.Level));
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

        if (character.characterGrowth.Level >= 30) //������ 30 �̻��̾ 1�� Ư���� �رݵǾ����� Ȯ���Ѵ�.
        {
            //Ƽ��1 Ư���� �ϳ� ���̹Ƿ�, ĳ����SO�� ���� 1Ƽ�� Ư���� �������� ������ �հ迡 �����Ѵ�.
            talentIncrHp_sum = character.characterGrowth.talent_Tier1.increaseHealth;
            talentIncrDef_sum = character.characterGrowth.talent_Tier1.increaseDef;
            talentIncrAtk_sum = character.characterGrowth.talent_Tier1.increaseAtk;
            talentIncrMov_sum = character.characterGrowth.talent_Tier1.increasecMov;

            talentIncrCtr_sum = character.characterGrowth.talent_Tier1.increasecCtr;
            talentIncrCtd_sum = character.characterGrowth.talent_Tier1.increasecCtd;
            talentIncrInfD_sum = character.characterGrowth.talent_Tier1.increaseInflictDamage;
            talentIncrTakenD_sum = character.characterGrowth.talent_Tier1.reducedTakenDamage;

            talentMultiplHp_sum = character.characterGrowth.talent_Tier1.multiplyHealth;
            talentMultiplDef_sum = character.characterGrowth.talent_Tier1.multiplyDef;
            talentMultiplAtk_sum = character.characterGrowth.talent_Tier1.multiplyAtk;

            if (character.characterGrowth.talent_Tier2 != null) //'������ 2Ƽ�� Ư��'���� 0�̶�� Ư���� ������� �ʾҰų� ���� �������� ���� ���.
            {
                //selectTrait_Tier ���� 0�� �ƴ϶�� Ư���� �����ߴٴ� ��. ĳ����SO�� ���� '���ð����� Ƽ�� 2 Ư�� �迭'���� ��ġ�ϴ� Ư���� ������ �� �������� �հ迡 ���Ѵ�. 
                talentIncrHp_sum += character.characterGrowth.talent_Tier2.increaseHealth;
                talentIncrDef_sum += character.characterGrowth.talent_Tier2.increaseDef;
                talentIncrAtk_sum += character.characterGrowth.talent_Tier2.increaseAtk;
                talentIncrMov_sum += character.characterGrowth.talent_Tier2.increasecMov;

                talentIncrCtr_sum += character.characterGrowth.talent_Tier2.increasecCtr;
                talentIncrCtd_sum += character.characterGrowth.talent_Tier2.increasecCtd;
                talentIncrInfD_sum += character.characterGrowth.talent_Tier2.increaseInflictDamage;
                talentIncrTakenD_sum += character.characterGrowth.talent_Tier2.reducedTakenDamage;

                talentMultiplHp_sum += character.characterGrowth.talent_Tier2.multiplyHealth;
                talentMultiplDef_sum += character.characterGrowth.talent_Tier2.multiplyDef;
                talentMultiplAtk_sum += character.characterGrowth.talent_Tier2.multiplyAtk;
                if (character.characterGrowth.talent_Tier3 != null)
                {
                    //3Ƽ�� Ư���� ���������� ����ȴ�.
                    talentIncrHp_sum += character.characterGrowth.talent_Tier3.increaseHealth;
                    talentIncrDef_sum += character.characterGrowth.talent_Tier3.increaseDef;
                    talentIncrAtk_sum += character.characterGrowth.talent_Tier3.increaseAtk;
                    talentIncrMov_sum += character.characterGrowth.talent_Tier3.increasecMov;

                    talentIncrCtr_sum += character.characterGrowth.talent_Tier3.increasecCtr;
                    talentIncrCtd_sum += character.characterGrowth.talent_Tier3.increasecCtd;
                    talentIncrInfD_sum += character.characterGrowth.talent_Tier3.increaseInflictDamage;
                    talentIncrTakenD_sum += character.characterGrowth.talent_Tier3.reducedTakenDamage;

                    talentMultiplHp_sum += character.characterGrowth.talent_Tier2.multiplyHealth;
                    talentMultiplDef_sum += character.characterGrowth.talent_Tier2.multiplyDef;
                    talentMultiplAtk_sum += character.characterGrowth.talent_Tier2.multiplyAtk;
                }
            }
        }

        //Ŭ������ ��� �⺻ Ŭ����(1�������� ����)�� ���� ����ǰ�, ���� Ŭ������ �رݵǾ��� �� Ŭ������ �����ߴ��� üũ�� ���� Ŭ���� ���� ����ġ�� �ջ��Ѵ�.
        int classincrHp_sum = character.characterGrowth.basicClass.increaseHealth;
        int classincrDef_sum = character.characterGrowth.basicClass.increaseDef;
        int classincrAtk_sum = character.characterGrowth.basicClass.increaseAtk;
        int classincrMov_sum = character.characterGrowth.basicClass.increasecMov;

        float classIncrCtr_sum = character.characterGrowth.basicClass.increasecCtr;
        float classIncrCtd_sum = character.characterGrowth.basicClass.increasecCtd;
        float classIncrInfD_sum = character.characterGrowth.basicClass.increaseInflictDamage;
        float classIncrTakenD_sum = character.characterGrowth.basicClass.reducedTakenDamage;

        float classMultiplHp_sum = character.characterGrowth.basicClass.multiplyHealth;
        float classMultiplDef_sum = character.characterGrowth.basicClass.multiplyDef;
        float classMultiplAtk_sum = character.characterGrowth.basicClass.multiplyAtk;

        if (character.characterGrowth.superiorClass != null)
        {
            classincrHp_sum += character.characterGrowth.superiorClass.increaseHealth;
            classincrDef_sum += character.characterGrowth.superiorClass.increaseDef;
            classincrHp_sum += character.characterGrowth.superiorClass.increaseAtk;
            classincrHp_sum += character.characterGrowth.superiorClass.increasecMov;

            classIncrCtr_sum += character.characterGrowth.superiorClass.increasecCtr;
            classIncrCtd_sum += character.characterGrowth.superiorClass.increasecCtd;
            classIncrInfD_sum += character.characterGrowth.superiorClass.increaseInflictDamage;
            classIncrTakenD_sum += character.characterGrowth.superiorClass.reducedTakenDamage;

            classMultiplHp_sum += character.characterGrowth.superiorClass.multiplyHealth;
            classMultiplDef_sum += character.characterGrowth.superiorClass.multiplyDef;
            classMultiplAtk_sum += character.characterGrowth.superiorClass.multiplyAtk;
        }

        //��� ����.
        //����� ȹ���� �� �ִ� ���� = ���ݷ�, ġȮ, ġ��, �ִ� ��������
        //���� ȹ���� �� �ִ� ���� = ü��, ����, �޴� ���� ����
        int weaponincrAtk_sum = (character.characterGrowth.weapon.increaseAtk * (character.characterGrowth.weaponEnhance + 1));
        float weaponMultiplAtk_sum = (character.characterGrowth.weapon.multiplyAtk * (character.characterGrowth.weaponEnhance + 1));
        float weaponIncrCtr_sum = (character.characterGrowth.weapon.increasecCtr * (character.characterGrowth.weaponEnhance + 1));
        float weaponIncrCtd_sum = (character.characterGrowth.weapon.increasecCtd * (character.characterGrowth.weaponEnhance + 1));
        float weaponIncrInfD_sum = (character.characterGrowth.weapon.increaseInflictDamage * (character.characterGrowth.weaponEnhance + 1));

        int armorincrHp_sum = (character.characterGrowth.armor.increaseHealth * (character.characterGrowth.armorEnhance + 1));
        int armorincrDef_sum = (character.characterGrowth.armor.increaseDef * (character.characterGrowth.armorEnhance + 1));
        float armorMultiplHp_sum = (character.characterGrowth.armor.multiplyHealth * (character.characterGrowth.armorEnhance + 1));
        float armorMultiplDef_sum = (character.characterGrowth.armor.multiplyDef * (character.characterGrowth.armorEnhance + 1));
        float armorIncrTakenD_sum = (character.characterGrowth.armor.reducedTakenDamage * (character.characterGrowth.armorEnhance + 1));


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

    public void WeaponRankUp()
    {
        //��� ��ȭ ����
        //0~4���� ĳ���Ͱ� � �����̵� ��ȭ ����
        //4������ +1�� �ϴ� ���� ���� ��� ���� ��ȯ�� �ǹ���.
        //ĳ���� 0~30���� : Ŀ�� ��� ���� ��� ���� ����
        //ĳ���� 30~50���� : ���� ��� ���� ��� ���� ����
        //ĳ���� 50~70���� : ���� ��� ���� ��� ���� ����
        //ĳ���� 70~90���� : ������ ��� ���� ��� ���� ����.
        //����, ��ȭ �ܰ谡 4�� ������ ĳ���� ������ ���� ���� �̻��̾�� ���� ��� ���� ��ȭ�� �� �ִ�.

        if(character.characterGrowth.weaponTier >= (int)ItemRank.Legend) //������ ��� �̻��� ��� ��ȭ �Ұ�. (���빫��� Myth ��������� �������� x)
        {
            return;
        }

        if (character.characterGrowth.weaponEnhance >= 4) //ĳ������ ��� ��ȭ ������ 4 �̻��� ���
        {
            if (character.characterGrowth.Level < (character.characterGrowth.weaponTier * 20 + 30)) //���� �������� üũ�ϴ� ���ǹ�. ���� ��� ����� 0Ƽ��(Ŀ��)�� �� ĳ���� ������ 30 �̸��̸� ��ȭ �Ұ�.
            {
                return;
            }
        }

        // rankUpMaterials�� ��� ��ҿ� ���� �ݺ�
        foreach (var kvp in character.characterGrowth.weapon.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // �䱸�Ǵ� �������� ���̵�
            int requiredItemCount = kvp.Value * (character.characterGrowth.weaponEnhance); // �䱸�Ǵ� �������� ���� = ��ȭ �������� �䱸���� �����Ѵ�.

            // �κ��丮�� �ش� �������� ���ų� ������ �䱸�Ǵ� �������� ���� ���
            if (!Managers.AccountData.inventory.TryGetValue(requiredItemId, out int currentItemCount) ||
                currentItemCount < requiredItemCount)
            {
                // �䱸 ������ �������� ���ϸ� ����
                return;
            }
        }

        // ��� �䱸 ������ �����ϴ� ��� ��ȭ ����
        // ��ȭ�� �ʿ��� ��ȭ ���� �����۵��� ����
        foreach (var kvp in character.characterGrowth.weapon.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // �䱸�Ǵ� �������� ���̵�
            int requiredItemCount = kvp.Value * (character.characterGrowth.weaponEnhance); // �䱸�Ǵ� �������� ����

            // �κ��丮���� �ش� ������ ���� ����
            Managers.AccountData.RemoveItem(requiredItemId, requiredItemCount);
        }
        
        if (character.characterGrowth.weaponEnhance >= 4) //ĳ������ ���� ��ȭ ������ 4 �̻��� ���
        {
            character.characterGrowth.weaponTier += 1; //Ƽ�� 1�ܰ� Up�ϰ� ��ȭ�� �ʱ�ȭ,
            character.characterGrowth.weaponEnhance = 0;
        }
        else
        {
            character.characterGrowth.weaponEnhance += 1;
        }

        ApplyAdditionStat();
        return;
    }

    public void ArmorRankUp()
    {
        //��� ��ȭ ����
        //0~4���� ĳ���Ͱ� � �����̵� ��ȭ ����
        //4������ +1�� �ϴ� ���� ���� ��� ���� ��ȯ�� �ǹ���.
        //ĳ���� 0~30���� : Ŀ�� ��� ���� ��� ���� ����
        //ĳ���� 30~50���� : ���� ��� ���� ��� ���� ����
        //ĳ���� 50~70���� : ���� ��� ���� ��� ���� ����
        //ĳ���� 70~90���� : ������ ��� ���� ��� ���� ����.
        //����, ��ȭ �ܰ谡 4�� ������ ĳ���� ������ ���� ���� �̻��̾�� ���� ��� ���� ��ȭ�� �� �ִ�.

        if (character.characterGrowth.armorTier >= (int)ItemRank.Legend) //������ ��� �̻��� ��� ��ȭ �Ұ�. (���빫��� Myth ��������� �������� x)
        {
            return;
        }

        if (character.characterGrowth.armorEnhance >= 4) //ĳ������ ��� ��ȭ ������ 4 �̻��� ���
        {
            if (character.characterGrowth.Level < (character.characterGrowth.armorTier * 20 + 30)) //���� �������� üũ�ϴ� ���ǹ�. ���� ��� ����� 0Ƽ��(Ŀ��)�� �� ĳ���� ������ 30 �̸��̸� ��ȭ �Ұ�.
            {
                return;
            }
        }

        // rankUpMaterials�� ��� ��ҿ� ���� �ݺ�
        foreach (var kvp in character.characterGrowth.armor.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // �䱸�Ǵ� �������� ���̵�
            int requiredItemCount = kvp.Value * (character.characterGrowth.armorEnhance); // �䱸�Ǵ� �������� ���� = ��ȭ �������� �䱸���� �����Ѵ�.

            // �κ��丮�� �ش� �������� ���ų� ������ �䱸�Ǵ� �������� ���� ���
            if (!Managers.AccountData.inventory.TryGetValue(requiredItemId, out int currentItemCount) ||
                currentItemCount < requiredItemCount)
            {
                // �䱸 ������ �������� ���ϸ� ����
                return;
            }
        }

        // ��� �䱸 ������ �����ϴ� ��� ��ȭ ����
        // ��ȭ�� �ʿ��� ��ȭ ���� �����۵��� ����
        foreach (var kvp in character.characterGrowth.armor.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // �䱸�Ǵ� �������� ���̵�
            int requiredItemCount = kvp.Value * (character.characterGrowth.armorEnhance); // �䱸�Ǵ� �������� ����

            // �κ��丮���� �ش� ������ ���� ����
            Managers.AccountData.RemoveItem(requiredItemId, requiredItemCount);
        }

        if (character.characterGrowth.armorEnhance >= 4) //ĳ������ ���� ��ȭ ������ 4 �̻��� ���
        {
            character.characterGrowth.armorTier += 1; //Ƽ�� 1�ܰ� Up�ϰ� ��ȭ�� �ʱ�ȭ,
            character.characterGrowth.armorEnhance = 0;
        }
        else
        {
            character.characterGrowth.armorEnhance += 1;
        }

        ApplyAdditionStat();
        return;
    }



    public bool ExSkillLevelUp(int ingredient)
    {
        if (ingredient >= character.characterGrowth.ExSkillLevel && character.characterGrowth.ExSkillLevel < 5)
        {
            character.characterGrowth.ExSkillLevel += 1;
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
        if (ingredient >= character.characterGrowth.affectionLevel && character.characterGrowth.affectionLevel < 99)
        {
            character.characterGrowth.affectionLevel += 1;
            //Todo : ��� �Ҹ�
            return true;
        }
        else
        {
            return false;
        }
    }//�ӽ÷� ������ ������ ȣ���� ������ �޼���, //Todo: ȣ���� ������ ��ȭ �Ҹ�, ȣ���� ���� �� �߰� ȿ�� ��.
}
