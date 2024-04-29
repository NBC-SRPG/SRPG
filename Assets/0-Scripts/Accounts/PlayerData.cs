using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class PlayerData
{
    public event Action<int> OnDiamondChanged;
    public event Action<int> OnGoldChanged;
    public event Action<int> OnApChanged;
    public event Action<int> OnGachaPointChanged;

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

    public int maxAp { get; private set; } = DEFAULT_AP; // maxAp
    public int Level { get; private set; } = 1; // 레벨
    public int exp { get; private set; } = 0; // 경험치
    public int maxExp { get; private set; } = 8; // 최대 경험치
    public string birthday { get; private set; } = ""; // 생일
    public int lobbyCharacter { get; private set; } = 3; // 로비 캐릭터
    public int gachaPoint { get; private set;} = 0;

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
        //int[] favoriteCharacter,
        int lobbyCharacter,
        int gachaPoint
        )
    {
        this.uId = uId ?? "0000000"; // 임시 기본값
        this.playerName = playerName ?? "DefaultName";
        this.playerComment = playerComment ?? "잘 부탁 드립니다.";
        this.diamond = diamond;
        this.gold = gold;
        this.ap = ap;
        this.maxAp = maxAp;
        this.Level = level;
        this.exp = exp;
        this.maxExp = maxExp;
        this.birthday = birthday;
        //this.favoriteCharacter = favoriteCharacter ?? new int[3];
        this.lobbyCharacter = lobbyCharacter;
        this.gachaPoint = gachaPoint;
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
            Managers.DB.Write<string>(Managers.DB.userDB.Child("playerData").Child("playerName"), playerName);
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
            Managers.DB.Write<string>(Managers.DB.userDB.Child("playerData").Child("playerComment"), playerComment);
            return true; // 글자 수 제한 조건을 만족하면 true 반환
        }
        else
        {
            return false; // 글자 수 제한 조건을 만족하지 않으면 false 반환
        }
    }

    public bool CanAddDiamond(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        if (Diamond + amount > MaxDiamond)
        {
            return false;
        }

        return true;
    }

    public void AddDiamond(int amount) //다이아 획득
    {
        Diamond += amount;
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Diamond"), Diamond);
    }

    public bool ReduceDiamond(int amount) //다이아 지불
    {
        int calcedDiamond = Diamond - amount;

        if (calcedDiamond >= 0)
        {
            Diamond = calcedDiamond;
            Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Diamond"), Diamond);
            return true;
        }
        else
        {
            Debug.Log("다이아 잔액 부족");
            return false;
        }
    }
    public bool CanAddGold(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        if (Gold + amount > MaxGold)
        {
            return false;
        }

        return true;
    }

    public void AddGold(int amount) //골드 획득
    {
        Gold += amount;
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Gold"), Gold);
    }

    public bool ReduceGold(int amount) //골드 지불
    {
        int calcedGold = Gold - amount;

        if (calcedGold >= 0)
        {
            Gold = calcedGold;
            Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Gold"), Gold);
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
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Ap"), Ap);
        return true;
    }
    public bool ReduceAP(int value) //Ap 차감
    {
        if ((Ap - value) >= 0)
        {
            Ap -= value;
            Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Ap"), Ap);
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
            Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Ap"), Ap);
        }
    }

    public void AddGachaPoint(int amount)
    {
        gachaPoint += amount;
        OnGachaPointChanged?.Invoke(gachaPoint);
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("gachaPoint"), gachaPoint);
    }

    public void ReduceGachaPoint(int amount)
    {
        gachaPoint -= amount;
        OnGachaPointChanged?.Invoke(gachaPoint);
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("gachaPoint"), gachaPoint);
    }

    public void AddExp(int value) //경험치값을 증가시킬 때 호출하는 메서드. 경험치가 최대 경험치 이상일 시 경험치가 maxExp 미만이 될 때까지 레벨업 메서드를 반복해서 실행한다.
    {
        exp += value;
        while (exp >= maxExp && Level < MAX_LEVEL)
        {
            LevelUp();
        }
        exp = math.clamp(exp, 0, maxExp);
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("exp"), exp);
    }
    private void LevelUp() //레벨업 메서드. 경험치값에서 최대 경험치값 만큼 차감하고 레벨을 1 올린다. 따로 메서드를 분리한 이유는 추후 레벨업 시 다른 추가 동작을 추가할 수도 있으므로.
    {
        exp -= maxExp;
        Level += 1;
        maxExp = dataTables["playerExpTable"][Level];
        maxAp = DEFAULT_AP + Level*2;
        AddAP(maxAp);

        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("maxExp"), maxExp);
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("maxAp"), maxAp);
        Managers.DB.Write<int>(Managers.DB.userDB.Child("playerData").Child("Level"), Level);
    }
    public bool SetBirthDay(string MMDD) //생일값 설정 메서드. 유효한 생일 값인지 검사한다.
    {
        // MMDD를 숫자로 변환
        if (int.TryParse(MMDD, out int numericValue))
        {
            // 날짜 유효성 확인
            int month = numericValue / 100;
            int day = numericValue % 100;

            // TODO
            // 이런 경우 30일까지 밖에 없거나, 2월의 경우 윤년 체크 불가
            if (month >= 1 && month <= 12 && day >= 1 && day <= 31)
            {
                // 날짜가 유효하면 저장
                birthday = MMDD;
                return true;
            }
            Managers.DB.Write<string>(Managers.DB.userDB.Child("playerData").Child("birthday"), birthday);
        }

        Debug.Log("유효한 날짜 형식이 아닙니다.");
        return false;
    }

    /*
    public void SetFavoriteCharacter(int? a, int? b, int? c) //선호 캐릭터 설정. null 체크
    {
        favoriteCharacter[0] = a ?? 0;
        favoriteCharacter[1] = b ?? 0;
        favoriteCharacter[2] = c ?? 0;
    }
    */
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
    /*
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
    */
}