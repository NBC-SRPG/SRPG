using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleKeyWords
{
    public enum EnemyState
    {
        Waiting,// 대기
        Watching,// 경계
        Finding,// 색적
        Chasing,// 추격
        Run,// 도망
        Stay,// 행동하지 않음 
    }

    public struct Damage// 치명타 피해 판정을 체크하기 위한 데미지 구조체
    {
        public int damage;
        public bool isCriticalHit;
        public AttackDamageType attackType;
        public float attributeDamage;
    }

    public enum AttackDamageType
    {
        None,
        Attack,// 기본 공격
        Skill,// 스킬 공격
        Buf,// 버프로 인한 데미지
        Passive,// 패시브로 인한 데미지
        Extra// 추가타
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
        AtkIncrease,
        DefIncrease,
        DefReduce,
        Quikness,
        Bind,
        Stun,
        ReversalHeal,

        AtkAura,
        Herald,

        ReceivedDgmReduce,
        ReceivedDgmIncrease,

        CrtRateIncrease,
        HealReversal,

        Corrosion,
        TargetMarker,

        AmethystShield,

    }
}
