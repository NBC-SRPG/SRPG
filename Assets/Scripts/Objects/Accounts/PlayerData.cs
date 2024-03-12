using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerData
{   //순서대로 UID, 닉네임, 다이아, 골드. AP, AP최대치, 계정 레벨, 계정 최대 레벨, 현재 경험치, 최대 경험치, 생일, 선호 캐릭터 목록, 캐릭터 아이콘, 지원 캐릭터
    private string uId;
    private string playerName;
    private int diamond;
    private int gold;
    private int ap;
    private int maxAp = 160;
    private int level = 1;
    private int maxLevel = 90;
    private int exp;
    private int maxExp = 100;
    private string birthDay;
    private int[] favoriteCharacter;
    private int lobbyCharacter;
    private int characterIcon;
    private int supportCharacter;

    // 생성자 메서드
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
        string birthday,
        int[] favoriteCharacter,
        int lobbyCharacter,
        int characterIcon
        )
    {
        this.uId = uId ?? "0000000"; // 임시 기본값
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

    // 각 데이터 필드에 대한 접근자 메서드
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

    public int GetExp()
    {
        return exp;
    }
    public int GetMaxExp()
    {
        return maxExp;
    }

    public string GetBirthday()
    {
        return birthDay;
    }
    public bool IsTodayBirthDayCheck() //오늘이 생일인지 체크하는 메서드
    {
        DateTime dateBirthDay;       //저장된 생일 문자열을 MMdd 형식으로 파싱
        if (DateTime.TryParseExact(birthDay, "MMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            // 만약 파싱이 성공하면 연도를 현재 연도, 시간을 00:00으로 설정하여 저장
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




    // 각 데이터 필드에 대한 설정자 메서드


    public bool SetPlayerName(string playerName) //닉네임 설정 시 사용하는 메서드.
    {
        if (playerName.Length <= 8)
        {
            this.playerName = playerName;
            return true; // 글자 수 제한 조건을 만족하면 true 반환
        }
        else
        {
            return false; // 글자 수 제한 조건을 만족하지 않으면 false 반환
        }
    }

    public bool SetDiamond(int amount) //다이아 값 설정 시 사용하는 메서드.
    {
        int calcedDiamond = diamond + amount;

        if (calcedDiamond >= 0 && calcedDiamond <= 999999)
        {
            diamond = calcedDiamond;
            return true;
        }
        else
        {
            Debug.Log(calcedDiamond < 0 ? "다이아 잔액 부족" : "다이아 보유 한도 초과");
            return false;
        }
    }
    public bool SetGold(int amount) //골드값 설정 시 사용하는 메서드.
    {
        int calcedGold = gold + amount;

        if (calcedGold >= 0 && calcedGold <= 9999999)
        {
            gold = calcedGold;
            return true;
        }
        else
        {
            Debug.Log(calcedGold < 0 ? "골드 잔액 부족" : "골드 보유 한도 초과");
            return false;
        }
    }

    public bool SetAP(int value) //Ap를 사용/충전하는 경우 사용하는 메서드. 충전 시 최대치를 넘어서 충전 가능.
    {
            if ((ap + value) >= 0)
            {
                ap += value;
                return true;
            }
            else
            {
                Debug.Log("AP 잔량 부족");
                return false;
            }
    }

    public void RegenAP() //Ap 리젠 시 사용 될 메서드. 리젠 Ap는 최대치를 넘어서 증가하지 않는다. 통상적으로 6분에 1씩, 1시간에 10 재생. 하루 총 재생량은 240.
    {
        if ((ap + 1) <= maxAp)
        {
            ap += 1;
        }
    }
    public void AddExp(int value) //경험치값을 증가시킬 때 호출하는 메서드. 경험치가 최대 경험치 이상일 시 경험치가 maxExp 미만이 될 때까지 레벨업 메서드를 반복해서 실행한다.
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
    private bool LevelUp() //레벨업 메서드. 경험치값에서 최대 경험치값 만큼 차감하고 레벨을 1 올린다.
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
    private void SetLevel(int value) //계정 레벨값에 대한 설정자 메서드. 어떤 방식으로든 레벨값을 변동시킬 때 이 메서드를 사용하게끔 해서 레벨값에 대한 콜백을 용이하게 한다.
    {
        level = math.clamp(value, 1, maxLevel);
        maxExp = (level * 70);
        maxAp = math.clamp((160 + ((level - 1) * 2)), 160, 240);
    }
    public void SetBirthday(string MMDD) //생일값을 설정할 때 쓰는 메서드.
    {
        birthDay = MMDD;
    }
    public void SetFavoriteCharacter(int? a, int? b, int? c) //선호 캐릭터 설정
    {
        favoriteCharacter[0] = a ?? 0;
        favoriteCharacter[1] = b ?? 0;
        favoriteCharacter[2] = c ?? 0;
    }
    public void SetLobbyCharacter(int? a) //로비 캐릭터 설정
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
    public void SetCharacterIcon(int? a) //아이콘 설정
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
    public void SetSupportCharacter(int? a) //지원캐릭터 설정
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