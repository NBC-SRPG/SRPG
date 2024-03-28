using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class PlayerData
{
    public event Action<int> OnDiamondChanged;
    public event Action<int> OnGoldChanged;
    public event Action<int> OnApChanged;

    public string uId { get; private set; } // UID
    public string playerName { get; private set; } // 닉네임
    public string playerComment { get; private set; } // 코멘트

    private int diamond;
    public int Diamond
    {
        get => diamond;
        private set
        {
            if (diamond != value)
            {
                diamond = value;
                OnDiamondChanged?.Invoke(diamond);
            }
        }
    } // 다이아

    private int gold;
    public int Gold
    {
        get => gold;
        private set
        {
            if (gold != value)
            {
                gold = value;
                OnGoldChanged?.Invoke(gold);
            }
        }
    } // 골드

    private int ap;
    public int Ap
    {
        get => ap;
        private set
        {
            if (ap != value)
            {
                ap = value;
                OnApChanged?.Invoke(ap);
            }
        }
    } // ap

    public int maxAp { get; private set; } = (int)PlayerCons.DefaltMaxAp; // maxAp
    private int level = (int)PlayerCons.DefaltLevel; // 레벨
    public int Level { get { return level; }  // 레벨 & 경험치, ap 자동 설정
                       private set
                        {
                            level = math.clamp(value, (int)PlayerCons.DefaltLevel, maxLevel);
                            maxExp = (level * 70);
                            maxAp = math.clamp((160 + ((level - 1) * 2)), (int)PlayerCons.DefaltMaxAp, 240);
                        }
                     }
    public int maxLevel { get; private set; } = (int)PlayerCons.MaxLevel; // 최대 레벨
    public int exp { get; private set; } // 경험치
    public int maxExp { get; private set; } = (int)PlayerCons.DefaltMaxExp; // 최대 경험치
    public string birthday { get; private set; } // 생일
    public int[] favoriteCharacter { get; private set; } // 선호 캐릭터
    public int lobbyCharacter { get; private set; } // 로비 캐릭터
    public int characterIcon { get; private set; } // 캐릭터 아이콘
    public int supportCharacter { get; private set; } // 지원 캐릭터

    // Init 메서드
    public void Init(
        string uId,
        string playerName,
        string playerComment,
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
        this.playerComment = playerComment ?? "잘 부탁 드립니다.";
        this.diamond = diamond;
        this.gold = gold;
        this.ap = ap;
        this.maxAp = maxAp;
        this.level = level;
        this.exp = exp;
        this.maxExp = maxExp;
        this.birthday = birthday;
        this.favoriteCharacter = favoriteCharacter ?? new int[3];
        this.lobbyCharacter = lobbyCharacter;
        this.characterIcon = characterIcon;
    }
    public bool IsTodayBirthDayCheck() //오늘이 생일인지 체크하는 메서드
    {
        DateTime dateBirthDay;       //저장된 생일 문자열을 MMdd 형식으로 파싱
        if (DateTime.TryParseExact(birthday, "MMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
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
    public bool SetPlayerComment(string playerComment) //플레이어 코멘트 설정 시 사용하는 메서드.
    {
        if (playerName.Length <= 40)
        {
            this.playerComment = playerComment;
            return true; // 글자 수 제한 조건을 만족하면 true 반환
        }
        else
        {
            return false; // 글자 수 제한 조건을 만족하지 않으면 false 반환
        }
    }

    public bool AddDiamond(int amount) //다이아 획득
    {
        int calcedDiamond = Diamond + amount;

        if (amount < 0)
        {
            Debug.Log("더하려는 값이 음수값입니다.");
            return false;
        }
        else if (calcedDiamond <= MaxDiamond)
        {
            Diamond = calcedDiamond;
            return true;
        }
        else
        {
            Debug.Log("다이아 보유 한도 초과");
            return false;
        }
    }

    public bool ReduceDiamond(int amount) //다이아 지불
    {
        int calcedDiamond = Diamond - amount;

        if (calcedDiamond >= 0)
        {
            Diamond = calcedDiamond;
            return true;
        }
        else
        {
            Debug.Log("다이아 잔액 부족");
            return false;
        }
    }

    public bool AddGold(int amount) //골드 획득
    {
        int calcedGold = Gold + amount;
        if ( amount < 0)
        {
            Debug.Log("더하려는 값이 음수값입니다.");
            return false;
        }
        else if (calcedGold <= MaxGold)
        {
            Gold = calcedGold;
            return true;
        }
        else
        {
            Debug.Log("골드 보유 한도 초과");
            return false;
        }
    }

    public bool ReduceGold(int amount) //골드 지불
    {
        int calcedGold = Gold - amount;

        if (calcedGold >= 0)
        {
            Gold = calcedGold;
            return true;
        }
        else
        {
            Debug.Log("골드 잔액 부족");
            return false;
        }
    }
    public bool AddAP(int value) //Ap 충전. 충전에는 별도의 제한이 없음
    {
        Ap += value;
        return true;
    }
    public bool ReduceAP(int value) //Ap 차감
    {
        if ((Ap - value) >= 0)
        {
            Ap -= value;
            return true;
        }
        else
        {
            Debug.Log("AP 잔량 부족");
            return false;
        }
    }
    // Ap 리젠 시 사용 될 메서드. 리젠 Ap는 최대치를 넘어서 증가하지 않는다.
    // 통상적으로 6분에 1씩, 1시간에 10 재생. 하루 총 재생량은 240.
    public void RegenAP() 
    {
        if ((Ap + 1) <= maxAp)
        {
            Ap += 1;
        }
    }

    public void AddExp(int value) //경험치값을 증가시킬 때 호출하는 메서드. 경험치가 최대 경험치 이상일 시 경험치가 maxExp 미만이 될 때까지 레벨업 메서드를 반복해서 실행한다.
    {
        exp += value;
        while (exp >= maxExp && level < maxLevel)
        {
            LevelUp();
        }
        exp = math.clamp(exp, 0, maxExp);
    }
    private void LevelUp() //레벨업 메서드. 경험치값에서 최대 경험치값 만큼 차감하고 레벨을 1 올린다. 따로 메서드를 분리한 이유는 추후 레벨업 시 다른 추가 동작을 추가할 수도 있으므로.
    {
        exp -= maxExp;
        Level += 1;
    }
    public bool SetBirthDay(string MMDD) //생일값 설정 메서드. 유효한 생일 값인지 검사한다.
    {
        // MMDD를 숫자로 변환
        if (int.TryParse(MMDD, out int numericValue))
        {
            // 날짜 유효성 확인
            int month = numericValue / 100;
            int day = numericValue % 100;

            if (month >= 1 && month <= 12 && day >= 1 && day <= 31)
            {
                // 날짜가 유효하면 저장
                birthday = MMDD;
                return true;
            }
        }

        Debug.Log("유효한 날짜 형식이 아닙니다.");
        return false;
    }

    public void SetFavoriteCharacter(int? a, int? b, int? c) //선호 캐릭터 설정. null 체크
    {
        favoriteCharacter[0] = a ?? 0;
        favoriteCharacter[1] = b ?? 0;
        favoriteCharacter[2] = c ?? 0;
    }
    public void SetLobbyCharacter(int? a) //로비 캐릭터 설정. null 체크
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
    public void SetCharacterIcon(int? a) //아이콘 설정. null 체크
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
    public void SetSupportCharacter(int? a) //지원캐릭터 설정. null 체크
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