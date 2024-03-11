using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerData : MonoBehaviour
{   //������� UID, �г���, ���̾�, ���. AP, AP�ִ�ġ, ���� ����, ���� �ִ� ����, ���� ����ġ, �ִ� ����ġ, ����, ��ȣ ĳ���� ���, ĳ���� ������, ���� ĳ����
    protected string uId;
    protected string playerName;
    protected int diamond;
    protected int gold;
    protected int ap;
    protected int maxAp = 160;
    protected int level = 1;
    protected int maxLevel = 80;
    protected int exp;
    protected int maxExp = 100;
    protected DateTime birthDay;
    protected int[] favoriteCharacter;
    protected int lobbyCharacter;
    protected int characterIcon;
    protected int supportCharacter;

    // ������ �޼���
    public PlayerData( 
        string uId,
        string playerName,
        int diamond,
        int gold,
        int ap,
        int maxAp,
        int level,
        int exp,
        int maxExp,
        DateTime birthday,
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

    // �� ������ �ʵ忡 ���� ������ �޼���
    public string GetUId()
    {
        return uId;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public int GetDiamond()
    {
        return diamond;
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetAP()
    {
        return ap;
    }

    public int GetMaxAP()
    {
        return maxAp;
    }

    public int GetLevel()
    {
        return level;
    }

    public int Exp()
    {
        return exp;
    }
    public int MaxExp()
    {
        return maxExp;
    }

    public DateTime GetBirthday()
    {
        return birthDay;
    }
    public bool IsTodayBirthDay() //������ �������� üũ�ϴ� �޼���
    {
        // ������ ���� ���� ������ ���� �ϰ� ��ġ�ϴ��� Ȯ��
        if ((birthDay.Month == DateTime.Today.Month) && (birthDay.Day == DateTime.Today.Day))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int[] GetFavoriteCharacter()
    {
        return favoriteCharacter;
    }

    public int GetLobbyCharacter()
    {
        return lobbyCharacter;
    }
    public int GetCharacterIcon()
    {
        return characterIcon;
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

    public bool SetDiamond(int amount) //���̾� �� ���� �� ����ϴ� �޼���.
    {
        int calcedDiamond = diamond + amount;

        if (calcedDiamond >= 0 && calcedDiamond <= 999999)
        {
            diamond = calcedDiamond;
            return true;
        }
        else
        {
            Debug.Log(calcedDiamond < 0 ? "���̾� �ܾ� ����" : "���̾� ���� �ѵ� �ʰ�");
            return false;
        }
    }
    public bool SetGold(int amount) //��尪 ���� �� ����ϴ� �޼���.
    {
        int calcedGold = gold + amount;

        if (calcedGold >= 0 && calcedGold <= 9999999)
        {
            gold = calcedGold;
            return true;
        }
        else
        {
            Debug.Log(calcedGold < 0 ? "��� �ܾ� ����" : "��� ���� �ѵ� �ʰ�");
            return false;
        }
    }

    public bool SetAP(int value) //Ap�� ���/�����ϴ� ��� ����ϴ� �޼���. ���� �� �ִ�ġ�� �Ѿ ���� ����.
    {
            if ((ap + value) >= 0)
            {
                ap += value;
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
    public void SetExp(int value) //����ġ���� ������ų �� ȣ���ϴ� �޼���. ����ġ�� �ִ� ����ġ �̻��� �� ����ġ�� maxExp �̸��� �� ������ ������ �޼��带 �ݺ��ؼ� �����Ѵ�.
    {
        exp += value;
        if (exp >= maxExp && level < maxLevel)
        {
            while (exp >= maxExp)
            {
                if(LevelUp() == false)
                {
                    break;
                }
            }
        }
        exp = math.clamp(exp, 0, maxExp);
    }
    protected bool LevelUp() //������ �޼���. ����ġ������ �ִ� ����ġ�� ��ŭ �����ϰ� ������ 1 �ø���.
    {
        if ((level + 1) <= maxLevel)
        {
            exp -= maxExp;
            SetLevel(level + 1);
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void SetLevel(int value) //�������� ���� �����ϴ� �޼���. ���� �������� ���� maxExp�� maxAp�� ������ ������ �����Ѵ�.
    {
        level = math.clamp(value, 1, maxLevel);
        maxExp = (level * 70);
        maxAp = math.clamp((160 + ((level - 1) * 2)), 160, 240);
    }
    public void SetBirthday(string MMDD) //���ϰ��� ������ �� ���� �޼���.
    {
        //�Է¹��� 4�ڸ� ���ڿ��� MMdd �������� �Ľ�
        if (DateTime.TryParseExact(MMDD, "MMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            // ���� �Ľ��� �����ϸ� ������ ���� ����, �ð��� 00:00���� �����Ͽ� ����
            birthDay = parsedDate.Date;
            Debug.Log("������ �����Ǿ����ϴ�: " + birthDay.ToString("MM/dd"));
        }
        else
        {
            Debug.Log("�ùٸ� ��¥ ������ �ƴմϴ�.");
        }
    }
    public void SetFavoriteCharacter(int? a, int? b, int? c) //��ȣ ĳ���� ����
    {
        favoriteCharacter[0] = a ?? 0;
        favoriteCharacter[1] = b ?? 0;
        favoriteCharacter[2] = c ?? 0;
    }
    public void SetLobbyCharacter(int? a) //�κ� ĳ���� ����
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
    public void SetCharacterIcon(int? a) //������ ����
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
    public void SetSupportCharacter(int? a) //����ĳ���� ����
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