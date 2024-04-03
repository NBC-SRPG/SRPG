using static Constants;

public class CharacterGrowth  //캐릭터의 성장 / 특성 및 클래스 / 기타 등등 캐릭터 객체의 개인적인 고유 데이터만을 저장하는 클래스.
{
    public int id;
    public int level;
    public int curExp;
    public int maxExp;
    public int star; //성급
    public int limitBreak; //현재 한계 돌파 정도. 0 = 한돌x
    public int ExSkillLevel;//Ex스킬 레벨 //Todo: 스킬 레벨에 따라 스킬 계수 적용시키기, 실제 인게임에서 스킬 레벨에 따라 효과 달라지게 하기

    public int abilityT2;
    public int abilityT3;

    public int superiorClass;
    
    /*
    public WeaponSO weapon
    public ArmorSO armor
    */


    //최초 초기화 메서드
    //캐릭터를 계정에서 최초로 획득 시 호출
    public void Init(CharacterSO SO) 
    {
        id = SO.id;
        level = 1;
        curExp = 0;
        maxExp = GetMaxExp(level);
        star = SO.basicStar;
        limitBreak = 0;

        ExSkillLevel = 1;

        abilityT2 = NONE_SELECTED;
        abilityT3 = NONE_SELECTED;

        superiorClass = NONE_SELECTED;
    }


    // 현재 캐릭터가 레벨업 가능한 최대 레벨을 반환
    public int GetMaxLevel()
    {
        int maxLevel;
        switch(star)
        {
            case 1:
                maxLevel = 30;
                break;
            case 2:
                maxLevel = 40;
                break;
            case 3:
                maxLevel = 50;
                break;
            case 4:
                maxLevel = 60;
                break;
            default:
                switch(limitBreak)
                {
                    default:
                        maxLevel = 70;
                        break;
                    case 1:
                        maxLevel = 75;
                        break;
                    case 2:
                        maxLevel = 80;
                        break;
                    case 3:
                        maxLevel = 85;
                        break;
                    case 4:
                        maxLevel = 90;
                        break;
                }
                break;
        }

        if (maxLevel > Managers.AccountData.playerData.Level)
        {
            maxLevel = Managers.AccountData.playerData.Level;
        }

        return maxLevel;
    }

    // Constants의 경험치테이블을 참조해 특정 level에서의 최대 경험치를 반환
    private int GetMaxExp(int level)
    {
        return characterExpTable[level];
    }

    // CalcExp를 바탕으로 레벨업 진행
    public void LevelUp(int exp)
    {
        int[] result = CalcExp(exp);
        level = result[0];
        curExp = result[1];
    }

    // exp만큼의 경험치를 획득했을 때의 level과 curExp를 배열로 반환
    public int[] CalcExp(int exp)
    {
        int[] result = new int[2] { level, curExp };

        while (true)
        {
            int expRequired = GetMaxExp(result[0]) - result[1];

            if (expRequired > exp)
            {
                result[1] += exp;
                break;
            }
            else
            {
                result[0]++;
                result[1] = 0;
                exp -= expRequired;
            }
        }
        // TODO : 경험치 획득시 결과가 최대레벨을 넘어갈 경우 예외처리 필요

        return result;
    }
}
