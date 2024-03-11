using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerData : MonoBehaviour
{   //순서대로 UID, 닉네임, 다이아, 골드. AP, AP최대치, 계정 레벨, 계정 최대 레벨, 현재 경험치, 최대 경험치, 생일, 선호 캐릭터 목록, 캐릭터 아이콘, 지원 캐릭터
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
        DateTime birthday,
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
    public bool IsTodayBirthDay() //오늘이 생일인지 체크하는 메서드
    {
        // 생일의 월과 일이 오늘의 월과 일과 일치하는지 확인
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
    public void SetExp(int value) //경험치값을 증가시킬 때 호출하는 메서드. 경험치가 최대 경험치 이상일 시 경험치가 maxExp 미만이 될 때까지 레벨업 메서드를 반복해서 실행한다.
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
    protected bool LevelUp() //레벨업 메서드. 경험치값에서 최대 경험치값 만큼 차감하고 레벨을 1 올린다.
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
    protected void SetLevel(int value) //레벨값을 직접 설정하는 메서드. 현재 레벨값에 따라 maxExp와 maxAp도 적절한 값으로 변경한다.
    {
        level = math.clamp(value, 1, maxLevel);
        maxExp = (level * 70);
        maxAp = math.clamp((160 + ((level - 1) * 2)), 160, 240);
    }
    public void SetBirthday(string MMDD) //생일값을 설정할 때 쓰는 메서드.
    {
        //입력받은 4자리 문자열을 MMdd 형식으로 파싱
        if (DateTime.TryParseExact(MMDD, "MMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            // 만약 파싱이 성공하면 연도를 현재 연도, 시간을 00:00으로 설정하여 저장
            birthDay = parsedDate.Date;
            Debug.Log("생일이 설정되었습니다: " + birthDay.ToString("MM/dd"));
        }
        else
        {
            Debug.Log("올바른 날짜 형식이 아닙니다.");
        }
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