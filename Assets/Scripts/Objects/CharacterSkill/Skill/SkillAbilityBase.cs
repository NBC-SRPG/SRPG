using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillAbilityBase
{
    CharacterBase character;

    public virtual void init(CharacterBase character)
    {
        this.character = character;
    }
}
