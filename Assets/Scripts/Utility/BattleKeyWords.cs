using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleKeyWords
{
    public struct Damage// 치명타 피해 판정을 체크하기 위한 데미지 구조체
    {
        public int damage;
        public bool isCriticalHit;
    }

    public enum AttackDamageType
    {
        None,
        Attack,
        Skill,
        Buf,
        Passive,
        Extra
    }

    public enum BufType
    {
        Positive,
        Negative
    }

    public enum BufKeyword
    {
        None,
        Burn,
        Bleed,
        Poison,
        Power,
        Defend,
        Quikness,

        AtkAura,
        Test_UniqBuf,

    }
}
