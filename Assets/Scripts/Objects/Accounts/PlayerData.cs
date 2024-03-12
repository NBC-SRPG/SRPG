using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class PlayerData
{   //������� UID, �г���, ���̾�, ���. AP, AP�ִ�ġ, ���� ����, ���� �ִ� ����, ���� ����ġ, �ִ� ����ġ, ����, ��ȣ ĳ���� ���, ĳ���� ������, ���� ĳ����
    public string uId { get; private set; }
    public string playerName { get; private set; }
    public int diamond { get; private set; }
    public int gold { get; private set; }
    public int ap { get; private set; }
    public int maxAp { get; private set; } = (int)PlayerCons.DefaltMaxAp;
    private int level = (int)PlayerCons.DefaltLevel;
    public int Level { get { return level; } 
                       private set
                        {
                            level = math.clamp(value, (int)PlayerCons.DefaltLevel, maxLevel);
                            maxExp = (level * 70);
                            maxAp = math.clamp((160 + ((level - 1) * 2)), (int)PlayerCons.DefaltMaxAp, 240);
                        }
                     }
    public int maxLevel { get; private set; } = (int)PlayerCons.MaxLevel;
    public int exp { get; private set; }
    public int maxExp { get; private set; } = (int)PlayerCons.DefaltMaxExp;
    public string birthDay { get; private set; }
    public int[] favoriteCharacter { get; private set; }
    public int lobbyCharacter { get; private set; }
    public int characterIcon { get; private set; }
    public int supportCharacter { get; private set; }

    // Init �޼���
    public void Init(
        string uId,
        string playerName,
        int diamond,
        int gold,
        int ap,
        int maxAp,
        int level,
        int exp,
        int maxExp,
        string birthday,
        int[] favoriteCharacter,
        int lobbyCharacter,
        int characterIcon
        )
    {
        this.uId = uId ?? "0000000"; // �ӽ� �⺻��
        this.playerName = playerName ?? "DefaultName";
        this.diamond = diamond;
        this.gold = gold;
        this.ap = ap;
        this.maxAp = maxAp;
        this.level = level;
        this.exp = exp;
        this.maxExp = maxExp;
        this.birthDay = birthday;
        this.favoriteCharacter = favoriteCharacter ?? new int[3];
        this.lobbyCharacter = lobbyCharacter;
        this.characterIcon = characterIcon;
    }
    public bool IsTodayBirthDayCheck() //������ �������� üũ�ϴ� �޼���
    {
        DateTime dateBirthDay;       //����� ���� ���ڿ��� MMdd �������� �Ľ�
        if (DateTime.TryParseExact(birthDay, "MMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            // ���� �Ľ��� �����ϸ� ������ ���� ����, �ð��� 00:00���� �����Ͽ� ����
            dateBirthDay = parsedDate.Date;
            if ((dateBirthDay.Month == DateTime.Today.Month) && (dateBirthDay.Day == DateTime.Today.Day))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    // �� ������ �ʵ忡 ���� ������ �޼���
    public bool SetPlayerName(string playerName) //�г��� ���� �� ����ϴ� �޼���.
    {
        if (playerName.Length <= 8)
        {
            this.playerName = playerName;
            return true; // ���� �� ���� ������ �����ϸ� true ��ȯ
        }
        else
        {
            return false; // ���� �� ���� ������ �������� ������ false ��ȯ
        }
    }

    public bool AddDiamond(int amount) //���̾� ȹ��
    {
        int calcedDiamond = diamond + amount;

        if (amount < 0)
        {
            Debug.Log("���Ϸ��� ���� �������Դϴ�.");
            return false;
        }
        else if (calcedDiamond <= 999999)
        {
            diamond = calcedDiamond;
            return true;
        }
        else
        {
            Debug.Log("���̾� ���� �ѵ� �ʰ�");
            return false;
        }
    }

    public bool ReduceDiamond(int amount) //���̾� ����
    {
        int calcedDiamond = diamond - amount;

        if (calcedDiamond >= 0)
        {
            diamond = calcedDiamond;
            return true;
        }
        else
        {
            Debug.Log("���̾� �ܾ� ����");
            return false;
        }
    }

    public bool AddGold(int amount) //��� ȹ��
    {
        int calcedGold = gold + amount;
        if ( amount < 0)
        {
            Debug.Log("���Ϸ��� ���� �������Դϴ�.");
            return false;
        }
        else if (calcedGold <= 9999999)
        {
            gold = calcedGold;
            return true;
        }
        else
        {
            Debug.Log("��� ���� �ѵ� �ʰ�");
            return false;
        }
    }

    public bool ReduceGold(int amount) //��� ����
    {
        int calcedGold = gold - amount;

        if (calcedGold >= 0)
        {
            gold = calcedGold;
            return true;
        }
        else
        {
            Debug.Log("��� �ܾ� ����");
            return false;
        }
    }
    public bool AddAP(int value) //Ap ����. �������� ������ ������ ����
    {
            ap += value;
            return true;
    }
    public bool ReduceAP(int value) //Ap ����
    {
        if ((ap - value) >= 0)
        {
            ap -= value;
            return true;
        }
        else
        {
            Debug.Log("AP �ܷ� ����");
            return false;
        }
    }
    public void RegenAP() //Ap ���� �� ��� �� �޼���. ���� Ap�� �ִ�ġ�� �Ѿ �������� �ʴ´�. ��������� 6�п� 1��, 1�ð��� 10 ���. �Ϸ� �� ������� 240.
    {
        if ((ap + 1) <= maxAp)
        {
            ap += 1;
        }
    }

    public void AddExp(int value) //����ġ���� ������ų �� ȣ���ϴ� �޼���. ����ġ�� �ִ� ����ġ �̻��� �� ����ġ�� maxExp �̸��� �� ������ ������ �޼��带 �ݺ��ؼ� �����Ѵ�.
    {
        exp += value;
        while (exp >= maxExp && level < maxLevel)
        {
            LevelUp();
        }
        exp = math.clamp(exp, 0, maxExp);
    }
    private void LevelUp() //������ �޼���. ����ġ������ �ִ� ����ġ�� ��ŭ �����ϰ� ������ 1 �ø���. ���� �޼��带 �и��� ������ ���� ������ �� �ٸ� �߰� ������ �߰��� ���� �����Ƿ�.
    {
        exp -= maxExp;
        Level += 1;
    }
    public bool SetBirthDay(string MMDD) //���ϰ� ���� �޼���. ��ȿ�� ���� ������ �˻��Ѵ�.
    {
        // MMDD�� ���ڷ� ��ȯ
        if (int.TryParse(MMDD, out int numericValue))
        {
            // ��¥ ��ȿ�� Ȯ��
            int month = numericValue / 100;
            int day = numericValue % 100;

            if (month >= 1 && month <= 12 && day >= 1 && day <= 31)
            {
                // ��¥�� ��ȿ�ϸ� ����
                birthDay = MMDD;
                return true;
            }
        }

        Debug.Log("��ȿ�� ��¥ ������ �ƴմϴ�.");
        return false;
    }

    public void SetFavoriteCharacter(int? a, int? b, int? c) //��ȣ ĳ���� ����. null üũ
    {
        favoriteCharacter[0] = a ?? 0;
        favoriteCharacter[1] = b ?? 0;
        favoriteCharacter[2] = c ?? 0;
    }
    public void SetLobbyCharacter(int? a) //�κ� ĳ���� ����. null üũ
    {
        if (a != null)
        {
            lobbyCharacter = (int)a;
        }
        else
        {
            lobbyCharacter = 0;
        }
    }
    public void SetCharacterIcon(int? a) //������ ����. null üũ
    {
        if(a != null)
        {
            characterIcon = (int)a;
        }
        else
        {
            characterIcon = 0;
        }

    }
    public void SetSupportCharacter(int? a) //����ĳ���� ����. null üũ
    {
        if (a != null)
        {
            supportCharacter = (int)a;
        }
        else
        {
            supportCharacter = 0;
        }
    }
}