using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static Constants;

public class SkillMaterials : Dictionary<int, int> //딕셔너리 양식
{
    public Dictionary<int, int> items = new Dictionary<int, int>();
}


public static class RankUp_skillMaterials //진영별/스킬 레벨별 아이템 목록 데이터를 저장하는 스태틱 클래스
{
    public static Dictionary<Belonging, Dictionary<int, SkillMaterials>> allSkillMaterials = new Dictionary<Belonging, Dictionary<int, SkillMaterials>>()
    {
        { Belonging.Demon, new Dictionary<int, SkillMaterials>() //마족진영. 스킬 레벨이 1, 2, 3, 4일 때, 각각 레벨업에 필요한 아이템 목록 딕셔너리.
            {
                { 1, new SkillMaterials() { items = new Dictionary<int, int>() { { 1, 1 } } } },
                { 2, new SkillMaterials() { items = new Dictionary<int, int>() { { 2, 1 } } } },
                { 3, new SkillMaterials() { items = new Dictionary<int, int>() { { 3, 1 } } } },
                { 4, new SkillMaterials() { items = new Dictionary<int, int>() { { 4, 1 } } } }
            }
        },
        { Belonging.League, new Dictionary<int, SkillMaterials>() //연맹
            {
                { 1, new SkillMaterials() { items = new Dictionary<int, int>() { { 1, 1 } } } },
                { 2, new SkillMaterials() { items = new Dictionary<int, int>() { { 2, 1 } } } },
                { 3, new SkillMaterials() { items = new Dictionary<int, int>() { { 3, 1 } } } },
                { 4, new SkillMaterials() { items = new Dictionary<int, int>() { { 4, 1 } } } }
            }
        },
        { Belonging.Order, new Dictionary<int, SkillMaterials>() //교단
            {
                { 1, new SkillMaterials() { items = new Dictionary<int, int>() { { 1, 1 } } } },
                { 2, new SkillMaterials() { items = new Dictionary<int, int>() { { 2, 1 } } } },
                { 3, new SkillMaterials() { items = new Dictionary<int, int>() { { 3, 1 } } } },
                { 4, new SkillMaterials() { items = new Dictionary<int, int>() { { 4, 1 } } } }
            }
        },
        { Belonging.Vegabond, new Dictionary<int, SkillMaterials>() //낭인
            {
                { 1, new SkillMaterials() { items = new Dictionary<int, int>() { { 1, 1 } } } },
                { 2, new SkillMaterials() { items = new Dictionary<int, int>() { { 2, 1 } } } },
                { 3, new SkillMaterials() { items = new Dictionary<int, int>() { { 3, 1 } } } },
                { 4, new SkillMaterials() { items = new Dictionary<int, int>() { { 4, 1 } } } }
            }
        }
    };

    public static SkillMaterials GetSkillMaterials(Belonging belonging, int level) //캐릭터 소속 진영과 스킬 레벨을 입력하면 그에 맞는 아이템 목록을 반환하는 메서드.
    {
        if (allSkillMaterials.ContainsKey(belonging) && allSkillMaterials[belonging].ContainsKey(level))
        {
            return allSkillMaterials[belonging][level];
        }
        else
        {
            return null; // 해당하는 진영과 스킬 레벨의 스킬 재료가 없을 때 null 반환 또는 적절한 예외 처리
        }
    }
}
