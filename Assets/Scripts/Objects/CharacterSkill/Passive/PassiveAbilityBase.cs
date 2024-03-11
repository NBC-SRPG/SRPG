using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbilityBase
{
    CharacterBase character;

    public virtual void init(CharacterBase character)
    {
        this.character = character;
    }
}
