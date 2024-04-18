using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestCharacter : CharacterBase
{
    // Start is called before the first frame update
    void Start()
    {
        curCharacterSkill = new ExSkillBase();
        curCharacterPassive = new List<PassiveLogic>();

        tempBonusStat = new TempBonusStat();
        curCharacterBufList = new CharacterBufList(this);

        characterAnim = characterObject.GetComponent<CharAnimBase>();
        health = GetComponent<HealthSystem>();

        health.InitHealth(1, tempBonusStat, curCharacterBufList, characterAnim);
        characterAnim.Init(health);
    }

}
