using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_AtkAura : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.AtkAura;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "Unique";

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);

        dontDestroy = true;
        isPermanent = true;
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraAtk = power
        };
    }

    public override void OnUpdate()// 버프를 걸어준 캐릭터 주변에 이 캐릭터가 있는지 판단해서 없다면 버프 제거
    {
        base.OnUpdate();

        if(!character.rangeFinder.GetTilesInRange(Buffer.curStandingTile.grid2DLocation, 3, false).Contains(character.curStandingTile))
        {
            DestoyBuf();
        }
    }
}
