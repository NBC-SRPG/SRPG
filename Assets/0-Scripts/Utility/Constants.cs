using System.Collections.Generic;

public class Constants
{

    public static Dictionary<int, int> characterExpTable = new Dictionary<int, int>();

    public enum Sound
    {
        Bgm,
        Effect,
        Max,
    }


    public enum AttackMethod
    {
        Melee,
        Range,
    }

    public enum ElementType
    {
        None,
        Fire,
        Water,
        Grass,
        Bolt,
        Dark,
        Light
    }

    public enum Faction
    {
        Demon,
        Cabal,
        Cathedral,
        Vagabond
    }

    public enum SkillScaleType
    {
        None,// 대상 하나
        Self,// 자신
        Cross,// 십자형 범위
        Square,// 사각형 범위
        Line,// 직선 범위
        Rhombus,// 마름모 범위
        Moon// 초승달 모양
    }

    /* //데미지 타입은 기획안에서 더이상 사용하지 않기로 했으므로 일단 주석처리했습니다.
    public enum SkillDamageType
    {
        MagicalDamage,
        PhysicalDamage
    }
    */

    public enum SkillTargetType
    {
        Me, // 대상을 지정할 필요 없음
        Enemy, // 적군 대상
        Ally, // 아군 대상
        All, //피아 미식별
        AllExceptME //피아 미식별 대신 자신은 제외
    }


    public enum PlayerCons
    {
        DefaltLevel = 100,
        DefaltMaxExp = 100,
        MaxLevel = 90,
        DefaltMaxAp = 160
    }
    


    public enum UIEvent
    {
        Pressed,
        PointerDown,
        PointerUp
    }

    public enum GachaType
    {
        Common,
        PickUp
    }

    public enum BaseClass //기본 클래스 5종.
    {
        Warrior,
        Shooter,
        Assassin,
        Caster,
        Priest
    }

    public enum LimitBreak // 한계돌파. (정수값 = 해당 성급의 최대 레벨)
    {
        LimitBreak_1 = 75,
        LimitBreak_2 = 80,
        LimitBreak_3 = 85,
        LimitBreak_4 = 90
    }


    public enum EquipType
    {
        Weapon,
        Armor
    }

    public enum WeaponType //(무기 종류는 임의로 설정함)
    {
        One_Hand_Blade, //한손 무기 계열
        Two_Hand_Blade, //양손 무기 계열
        Spear, //장병기 계열
        Knuckles, //너클/주먹 계열

        Bow, //활 계열
        Cannon, //대포 계열
        Gun, //총기 계열

        Wand, //완드
        Staff, //지팡이
        Orb //오브
    }

    public enum ArmorType //(방어구 종류는 임의로 설정함)
    {
        Full_Plate_Armor, //전신 갑옷
        Shield, //방패

        Feather_Hat, //깃털 모자
        Chain_Mail, //사슬 갑옷

        Magic_Hat, //마법사 모자
        Robe //로브
    }

    //아이템 분류.
    //아이템 분류 선별 기준 = '인벤토리에 표시되고 정보를 볼 수 있어야 하는지? 여부가 성립하면 아이템'
    public enum ItemType
    {
        Ticket, //캐릭터 뽑기 티켓류
        Piece, //캐릭터 조각류
        Token, //토큰류 = 이벤트 상점 교환 재화
        Consumable, //사용 가능하고 사용 시 보상을 지급하는 아이템류 == AP 충전아이템, 골드 보물상자, 다이아 꾸러미, 캐릭터 선택권, 아이템 박스 등
        //RankUp_character, //캐릭터 돌파에 필요한 아이템류. 일단은 조각이 그 역할을 하므로 더미 데이터.
        ExpUp_character, //캐릭터 경험치를 상승시켜주는 아이템류. 캐릭터 육성 창에서 사용 가능
        RankUp_skill, //캐릭터 스킬 레벨업에 필요한 아이템류.
        RankUp_equip, //장비 등급 상승에 필요한 아이템류. = Material
        Gift, //호감도 상승에 필요한 아이템류. 선물.
        Memorial, //특별한 기능은 없지만 보관 자체로 의미가 있는 기념품/중요한 아이템류. = 트로피, 훈장, 스토리에서 중요한 의미를 가진 아이템 등.
    }

    public enum ItemRank
    {
        Common, //=하얀색
        Rare, //=옅은 하늘색
        Epic, //=보라색
        Legend, //=백금색
        Myth //=검은색
    }

    public enum Rewards //보상이 될 수 있는 것들
    {
        AP,
        Gold,
        Diamond,
        Character,
        Item
    }

    public enum Status
    {
        Atk,
        Def,
        Health,
        Mov,
    }

    public enum MissionCategory
    {
        Daily, // 일일 퀘스트
        Weekly, // 주간 퀘스트
        Achievement, // 업적
        Beginner // 초보자 퀘스트
    }

    public enum MissionType
    {
        GetItem, // 아이템 획득
        UseItem, // 아이템 사용
        KillMonster, // 몬스터 처치
        Login // 로그인
    }
    public enum MissionState
    {
        Waite, // 진행 전
        Progress, // 진행 중
        Complete, // 완료
        Receive // 보상 수령
    }

    public enum StageClear
    {
        ClearAll,// 섬멸
        Assasinate, // 특정 적 처치
        Run, // 특정 위치로 이동
        Defence // 방어
    }

    public const int MaxDiamond = 999999;
    public const int MaxGold = 999999;

    public const int NONE_SELECTED = -1;
    public const int TestApImage = 60001000;
    public const int TestExpImage = 60001001;
    public const int TestGoldImage = 60001002;
    public const int TestDiamondImage = 60001003;

    public const int FriendTabs = 0;
    public const int ApplyingTabs = 1;
    public const int WaitingTabs = 2;
    public const int AbilityTier2 = 50;
    public const int AbilityTier2UnlockLevel = 50;
    public const int AbilityTier3UnlockLevel = 70;
}
