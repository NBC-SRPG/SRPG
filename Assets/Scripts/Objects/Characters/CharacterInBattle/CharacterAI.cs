using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    List<CharacterBase> enemyList;

    public override void InitCharacter(Character charac, string id)
    {
        base.InitCharacter(charac, id);

        enemyList = new List<CharacterBase>();
    }
}
