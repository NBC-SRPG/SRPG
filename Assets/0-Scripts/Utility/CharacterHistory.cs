using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHistory
{
    public Dictionary<CharacterBase, int> enemyDidAttack;

    public int takeDamageFigure;
    public int takeDamageCount;

    public int takeHealFigure;
    public int takeHealCount;

    public int dealDamageFigure;
    public int dealDamageCount;

    public int attackCount;

    public int useCostCount;
    public int useSkillCount;

    public int healFigure;
    public int healCount;

    public int moveFigure;

    public void ResetHistory()
    {
        enemyDidAttack.Clear();

        takeDamageFigure = 0;
        takeDamageCount = 0;

        takeHealFigure = 0;
        takeHealCount = 0;

        dealDamageCount = 0;
        dealDamageFigure = 0;

        attackCount = 0;

        useCostCount = 0;

        healFigure = 0;
        healCount = 0;

        useSkillCount = 0;

        moveFigure = 0;
    }

    public CharacterHistory()
    {
        enemyDidAttack = new Dictionary<CharacterBase, int>();

        ResetHistory();
    }

    public CharacterHistory(CharacterHistory history)
    {
        enemyDidAttack = new Dictionary<CharacterBase, int>(history.enemyDidAttack);

        takeDamageFigure = history.takeDamageFigure;
        takeDamageCount = history.takeDamageCount;

        takeHealFigure = history.takeHealFigure;
        takeHealCount = history.takeHealCount;

        dealDamageFigure = history.dealDamageFigure;
        dealDamageCount = history.dealDamageCount;

        attackCount = history.attackCount;

        useCostCount = history.useCostCount;

        healFigure = history.healFigure;
        healCount = history.healCount;

        useSkillCount = history.useSkillCount;

        moveFigure = history.moveFigure;
    }

    public void AddAttackEnemy(CharacterBase enemy, int damage)
    {
        if(enemyDidAttack.ContainsKey(enemy))
        {
            enemyDidAttack[enemy] += damage;
        }
        else
        {
            enemyDidAttack.Add(enemy, damage);
        }
    }
}
