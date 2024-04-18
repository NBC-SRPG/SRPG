using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using static Constants;

public class EquipmentUpgradeUI : UIBase
{
    private Character character;
    private EquipType equipType;
    private EquipSO nextEquip;
    private bool isLoaded = false;
    private enum Texts
    {
        EquipmentBeforeName,
        EquipmentAfterName,
        EquipmentUpgradeLevel,
        HpBefore,
        HpAfter,
        AtkBefore,
        AtkAfter,
        DefBefore,
        DefAfter,
        AdditionalText,
        EquipmentUpgradeGoldText
    }

    private enum Images
    {
        EquipmentBeforeImage,
        EquipmentAfterImage
    }

    private enum Buttons
    {
        EquipmentUpgradeButton,
        EquipmentUpgradeUICloseButton
    }

    private enum GameObjects
    {
        EquipmentBeforeStar,
        EquipmentAfterStar,
        EquipmentUpgradeMaterials
    }

    public void Init(Character character, EquipType equipType)
    {
        this.character = character;
        this.equipType = equipType;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        StartCoroutine(InitEquip());

        GetButton((int)Buttons.EquipmentUpgradeButton).onClick.AddListener(OnClickEquipmentUpgradeButton);
        GetButton((int)Buttons.EquipmentUpgradeUICloseButton).onClick.AddListener(OnClickEquipmentUpgradeUICloseButton);
    }

    // 강화
    private void OnClickEquipmentUpgradeButton()
    {
        // 아이템 감소
        foreach (var material in nextEquip.upgradeMaterials)
        {
            Managers.AccountData.ConsumeItems(material.Key, material.Value);
        }

        // 골드 감소
        // 골드 부족 시 경고 UI
        // 현재는 부족 시 버튼을 비활성화
        // TODO: 비활성화 or 경고UI??
        if (Managers.AccountData.playerData.ReduceGold(nextEquip.gold) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.SetText("골드가 부족합니다.");

            return;
        }
        // 장비 레벨 증가
        if (equipType == EquipType.Weapon)
        {
            character.Growth.weapon++;
        }
        else
        {
            character.Growth.armor++;
        }
        // UI 업데이트
        StartCoroutine(InitEquip());
    }

    private void OnClickEquipmentUpgradeUICloseButton()
    {
        Managers.UI.CloseUI(this);
    }

    private IEnumerator InitEquip()
    {
        if (equipType == EquipType.Weapon)
        {
            Utility.Id2SO<EquipSO>(character.SO.weapon[character.Growth.weapon + 1], (result) =>
            {
                nextEquip = (EquipSO)result;
                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);

            GetText((int)Texts.EquipmentBeforeName).text = character.weapon.equipName;
            GetText((int)Texts.EquipmentAfterName).text = nextEquip.equipName;
            // TODO
            // 이미지 초기화
            if (character.Growth.level < nextEquip.upgradeLevel)
            {
                GetText((int)Texts.EquipmentUpgradeLevel).text = $"강화가능레벨 : {nextEquip.upgradeLevel}";
            }
            else
            {
                GetText((int)Texts.EquipmentUpgradeLevel).text = "";
            }

            /*
            var previewStats = character.PreviewEnhancedStats(nextEquip, character.armor);

            GetText((int)Texts.HpBefore).text = character.hp.ToString();
            GetText((int)Texts.HpAfter).text = previewStats.hp.ToString();
            GetText((int)Texts.AtkBefore).text = character.atk.ToString();
            GetText((int)Texts.AtkAfter).text = previewStats.atk.ToString();
            GetText((int)Texts.DefBefore).text = character.def.ToString();
            GetText((int)Texts.DefAfter).text = previewStats.def.ToString();
            */
            GetText((int)Texts.HpBefore).text = character.weapon.hp.ToString();
            GetText((int)Texts.HpAfter).text = nextEquip.hp.ToString();
            GetText((int)Texts.AtkBefore).text = character.weapon.atk.ToString();
            GetText((int)Texts.AtkAfter).text = nextEquip.atk.ToString();
            GetText((int)Texts.DefBefore).text = character.weapon.def.ToString();
            GetText((int)Texts.DefAfter).text = nextEquip.def.ToString();
            
            if (nextEquip.gold > Managers.AccountData.playerData.Gold)
            {
                GetText((int)Texts.EquipmentUpgradeGoldText).text = $"<color=red>{nextEquip.gold} G</color>";
                GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;
            }
            else
            {
                GetText((int)Texts.EquipmentUpgradeGoldText).text = $"{nextEquip.gold} G";
            }

            if (character.weapon.additionalOption != null)
            {
                GetText((int)Texts.AdditionalText).text = character.weapon.additionalOption;
            }
            else
            {
                GetText((int)Texts.AdditionalText).text = "";
            }

            foreach (var material in nextEquip.upgradeMaterials)
            {
                GameObject go = Managers.Resource.Load<GameObject>("Prefabs/UI/EquipmentUpgradeMaterialUI");
                go.GetComponent<EquipmentUpgradeMaterialUI>().Init(material.Key, material.Value);
                Managers.Resource.Instantiate(go, GetObject((int)GameObjects.EquipmentUpgradeMaterials).transform);
            
                if (material.Value > Managers.AccountData.inventory[material.Key])
                {
                    GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;
                }
            }
        }
        else
        {
            Utility.Id2SO<EquipSO>(character.SO.armor[character.Growth.armor + 1], (result) =>
            {
                nextEquip = (EquipSO)result;
                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);

            GetText((int)Texts.EquipmentBeforeName).text = character.armor.equipName;
            GetText((int)Texts.EquipmentAfterName).text = nextEquip.equipName;
            // TODO
            // 이미지 초기화
            if (character.Growth.level < character.armor.upgradeLevel)
            {
                GetText((int)Texts.EquipmentUpgradeLevel).text = $"강화가능레벨 : {nextEquip.upgradeLevel}";
            }
            else
            {
                GetText((int)Texts.EquipmentUpgradeLevel).text = "";
            }
            /*
            var previewStats = character.PreviewEnhancedStats(character.weapon, nextEquip);

            GetText((int)Texts.HpBefore).text = character.hp.ToString();
            GetText((int)Texts.HpAfter).text = previewStats.hp.ToString();
            GetText((int)Texts.AtkBefore).text = character.atk.ToString();
            GetText((int)Texts.AtkAfter).text = previewStats.atk.ToString();
            GetText((int)Texts.DefBefore).text = character.def.ToString();
            GetText((int)Texts.DefAfter).text = previewStats.def.ToString();
            */
            GetText((int)Texts.HpBefore).text = character.armor.hp.ToString();
            GetText((int)Texts.HpAfter).text = nextEquip.hp.ToString();
            GetText((int)Texts.AtkBefore).text = character.armor.atk.ToString();
            GetText((int)Texts.AtkAfter).text = nextEquip.atk.ToString();
            GetText((int)Texts.DefBefore).text = character.armor.def.ToString();
            GetText((int)Texts.DefAfter).text = nextEquip.def.ToString();

            if (nextEquip.gold > Managers.AccountData.playerData.Gold)
            {
                GetText((int)Texts.EquipmentUpgradeGoldText).text = $"<color=red>{nextEquip.gold} G</color>";
                GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;
            }
            else
            {
                GetText((int)Texts.EquipmentUpgradeGoldText).text = $"{nextEquip.gold} G";
            }
            if (character.armor.additionalOption != null)
            {
                GetText((int)Texts.AdditionalText).text = character.armor.additionalOption;
            }
            else
            {
                GetText((int)Texts.AdditionalText).text = "";
            }

            foreach (var material in nextEquip.upgradeMaterials)
            {
                GameObject go = Managers.Resource.Load<GameObject>("Prefabs/UI/EquipmentUpgradeMaterialUI");
                go.GetComponent<EquipmentUpgradeMaterialUI>().Init(material.Key, material.Value);
                Managers.Resource.Instantiate(go, GetObject((int)GameObjects.EquipmentUpgradeMaterials).transform);

                if (material.Value > Managers.AccountData.inventory[material.Key])
                {
                    GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;
                }
            }
        }
    }
}
