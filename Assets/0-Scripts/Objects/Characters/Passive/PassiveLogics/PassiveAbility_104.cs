using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_104 : PassiveLogic
{
    //ID 004. 포르테 Forte
    //패시브 스킬
    //매 턴의 종료시 범위 내의 체력이 가장 낮은 아군 2명의 체력을 자신의 최대 체력의 10%만큼 회복시킨다.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    //0. 턴이 끝날 때.
    //1. 범위 내에 자신을 제외한 인접한 아군 목록을 리스트에 저장.
    //2. 리스트를 체력 순으로 정렬하고 가장 체력이 낮은 아군과, 두번째로 체력이 낮은 아군을 구한다.
    //3. 해당 아군에게 포르테 최대 체력의 10%만큼 치유 적용
    // 캐릭터 베이스 리스트를 멤버 변수로 선언
    List<CharacterBase> charactersInRange = new List<CharacterBase>(); //범위 내의 아군 캐릭터를 저장할 리스트
    List<CharacterBase> targetsToHeal = new List<CharacterBase>(); //범위 내의 아군 캐릭터 중 치유 대상을 따로 저장할 리스트

    // OnTurnEnd 메서드
    public override void OnTurnEnd()
    {
        // 힐량 = 최대 체력 * 10% ( 10 / 100 )
        int healAmount = (int)((character.health.TotalHealth) * ((float)(coefficient["healRate"]) / coefficient["denominator"]));

        // 타일 범위 내의 캐릭터들을 모두 수집하여 리스트를 업데이트
        UpdateCharactersInRangeList();

        // 가장 체력이 적은 두 캐릭터에게 치유를 시도
        TryHealWeakestCharacters(ref healAmount);
    }

    // 타일 범위 내의 캐릭터들을 수집하여 리스트를 업데이트하는 메서드
    private void UpdateCharactersInRangeList()
    {
        charactersInRange.Clear(); // 리스트를 비우고 다시 채움

        foreach (OverlayTile tile in character.rangeFinder.GetTilesInRange(character.curStandingTile.grid2DLocation, coefficient["range"], false).FindAll(x => x.curStandingCharater != null && !x.curStandingCharater.CheckEnemy(character)))
        {
            if (tile.curStandingCharater != null && tile.curStandingCharater != character && !character.CheckEnemy(tile.curStandingCharater)) // 자기 자신은 대상에서 제외하고, 범위 내의 적을 제외한 모든 캐릭터 베이스를 리스트에 저장
            {
                charactersInRange.Add(tile.curStandingCharater);
            }
        }
    }

    // 가장 체력이 적은 두 캐릭터에게 치유를 시도하는 메서드
    private void TryHealWeakestCharacters(ref int healAmount)
    {
        // 가장 체력이 적은 캐릭터와 두 번째로 체력이 적은 캐릭터를 찾기 위해 정렬
        charactersInRange.Sort((a, b) => a.health.CurHealth.CompareTo(b.health.CurHealth));

        // 가장 체력이 적은 캐릭터와 두 번째로 체력이 적은 캐릭터를 치유 대상 목록에 추가
        if (charactersInRange.Count > coefficient["count1"])
        {
            if (charactersInRange[coefficient["count1"]].health.CurHealth < charactersInRange[coefficient["count1"]].health.TotalHealth)
            {
                targetsToHeal.Add(charactersInRange[coefficient["count1"]]); // 가장 체력이 적은 캐릭터
                if (charactersInRange.Count > coefficient["count2"])
                {
                    if (charactersInRange[coefficient["count2"]].health.CurHealth < charactersInRange[coefficient["count2"]].health.TotalHealth)
                    {
                        targetsToHeal.Add(charactersInRange[coefficient["count2"]]); // 두 번째로 체력이 적은 캐릭터
                    }
                }
            }
        }

        // 치유 대상 목록이 비어 있는지 확인하고, 비어 있지 않은 경우에만 치유를 시도
        if (targetsToHeal.Count > 0)
        {
            // 치유 대상 목록에 저장된 캐릭터들에게 치유를 시도
            foreach (CharacterBase target in targetsToHeal)
            {
                    BattleManager.Instance.ExtraSkillHeal(character, healAmount, targetsToHeal, BattleKeyWords.AttackDamageType.Skill);
            }

            Managers.Sound.Play(Constants.Sound.Effect, "Sounds/Effects/Character_4/Passive_04.mp3");
        }

        // 치유 대상 목록 비우기
        targetsToHeal.Clear();
    }
}

