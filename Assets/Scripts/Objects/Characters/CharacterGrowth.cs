using static Constants;

public class CharacterGrowth  //캐릭터의 성장 / 특성 및 클래스 / 기타 등등 캐릭터 객체의 개인적인 고유 데이터만을 저장하는 클래스.
{
    public int id;
    public int level;
    public int exp;
    //public int maxExp
    public int star; //성급
    public int limitBreak; //현재 한계 돌파 정도. 0 = 한돌x
    public int ExSkillLevel;//Ex스킬 레벨 //Todo: 스킬 레벨에 따라 스킬 계수 적용시키기, 실제 인게임에서 스킬 레벨에 따라 효과 달라지게 하기

    public int abilityT1;
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
        exp = 0;
        star = SO.basicStar;
        limitBreak = 0;

        ExSkillLevel = 1;

        abilityT1 = NONE_SELECTED;
        abilityT2 = NONE_SELECTED;
        abilityT3 = NONE_SELECTED;

        superiorClass = NONE_SELECTED;
    }
}
