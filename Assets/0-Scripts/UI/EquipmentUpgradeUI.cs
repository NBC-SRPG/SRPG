using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EquipmentUpgradeUI : UIBase
{
    private Character character;
    private EquipType equipType;
    private EquipSO currentEquip, nextEquip;
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
        EquipmentArrow,
        EquipmentBeforeStar,
        EquipmentAfterStar,
        EquipmentUpgradeMaterials
    }

    public void Init(Character character, EquipType equipType)
    {
        this.character = character;
        this.equipType = equipType;
        currentEquip = equipType == EquipType.Weapon ? character.weapon : character.armor;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        StartCoroutine(InitEquip());

        GetButton((int)Buttons.EquipmentUpgradeButton).onClick.AddListener(OnClickEquipmentUpgradeButton);
        GetButton((int)Buttons.EquipmentUpgradeUICloseButton).onClick.AddListener(OnClickEquipmentUpgradeUICloseButton);
    }

    private IEnumerator InitEquip()
    {
        CleanUpMaterialsUI();

        isLoaded = false;

        yield return StartCoroutine(GetNextEquip((equip) => {
            nextEquip = equip;
        }));

        if (nextEquip == null)
        {
            DisplayMaxUpgradeMessage();
            yield break;
        }

        UpdateUI();
    }
    private IEnumerator GetNextEquip(Action<EquipSO> onEquipLoaded)
    {
        List<int> equipList = equipType == EquipType.Weapon ? character.SO.weapon : character.SO.armor;
        int nextIndex = (equipType == EquipType.Weapon ? character.Growth.weapon : character.Growth.armor) + 1;

        if (nextIndex >= equipList.Count)
        {
            onEquipLoaded(null);
            yield break;
        }

        Utility.Id2SO<EquipSO>(equipList[nextIndex], (result) =>
        {
            EquipSO equip = (EquipSO)result;
            onEquipLoaded(equip);
            isLoaded = true;
        });

        yield return new WaitUntil(() => isLoaded);
    }

    private void UpdateUI()
    {
        SetTextValues();
        SetImageValues();
        UpdateMaterialList();
    }

    private void SetTextValues()
    {
        GetText((int)Texts.EquipmentBeforeName).text = currentEquip.equipName;
        GetText((int)Texts.EquipmentAfterName).text = nextEquip.equipName;

        if (character.Growth.level < nextEquip.upgradeLevel)
        {
            GetText((int)Texts.EquipmentUpgradeLevel).text = $"강화가능레벨 : {nextEquip.upgradeLevel}";
            GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;
        }
        else
        {
            GetText((int)Texts.EquipmentUpgradeLevel).text = "";
        }

        GetText((int)Texts.HpBefore).text = currentEquip.hp.ToString();
        GetText((int)Texts.HpAfter).text = nextEquip.hp.ToString();
        GetText((int)Texts.AtkBefore).text = currentEquip.atk.ToString();
        GetText((int)Texts.AtkAfter).text = nextEquip.atk.ToString();
        GetText((int)Texts.DefBefore).text = currentEquip.def.ToString();
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

        GetText((int)Texts.AdditionalText).text = nextEquip.additionalOption ?? "";
    }

    private void SetImageValues()
    {
        // TODO
        // 이미지 초기화
    }

    private void UpdateMaterialList()
    {
        foreach (var material in nextEquip.upgradeMaterials)
        {
            GameObject go = Managers.Resource.Load<GameObject>("Prefabs/UI/EquipmentUpgradeMaterialUI");
            var instance = Managers.Resource.Instantiate(go, GetObject((int)GameObjects.EquipmentUpgradeMaterials).transform);
            instance.GetComponent<EquipmentUpgradeMaterialUI>().Init(material.Key, material.Value);

            int materialCount;
            Managers.AccountData.inventory.TryGetValue(material.Key, out materialCount);

            if (material.Value > materialCount)
            {
                GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;
            }
        }
    }

    private void CleanUpMaterialsUI()
    {
        foreach (Transform material in GetObject((int)GameObjects.EquipmentUpgradeMaterials).transform)
        {
            Destroy(material.gameObject);
        }
    }

    private void DisplayMaxUpgradeMessage()
    {
        GetText((int)Texts.EquipmentUpgradeLevel).text = $"최대 강화 레벨에 도달하였습니다.";
        GetObject((int)GameObjects.EquipmentArrow).SetActive(false);

        GetText((int)Texts.EquipmentAfterName).transform.parent.gameObject.SetActive(false);

        GetText((int)Texts.HpBefore).text = character.weapon.hp.ToString();
        GetText((int)Texts.AtkBefore).text = character.weapon.atk.ToString();
        GetText((int)Texts.DefBefore).text = character.weapon.def.ToString();

        GetText((int)Texts.EquipmentUpgradeGoldText).text = $"0 G";
        GetButton((int)Buttons.EquipmentUpgradeButton).enabled = false;

        HideAfterUpgradeUI();
    }

    private void HideAfterUpgradeUI()
    {
        GetText((int)Texts.HpAfter).gameObject.SetActive(false);
        GetText((int)Texts.AtkAfter).gameObject.SetActive(false);
        GetText((int)Texts.DefAfter).gameObject.SetActive(false);
        GetText((int)Texts.AdditionalText).text = "";
    }

    private void OnClickEquipmentUpgradeButton()
    {
        // 골드 감소
        // 골드 부족 시 경고 UI
        // 현재는 부족 시 버튼을 비활성화
        // TODO: 비활성화 or 경고UI??
        if (Managers.AccountData.playerData.ReduceGold(nextEquip.gold) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("골드가 부족합니다.");

            return;
        }

        UpgradeEquipment();
        StartCoroutine(InitEquip());
    }

    private void UpgradeEquipment()
    {
        foreach (var material in nextEquip.upgradeMaterials)
        {
            Managers.AccountData.ConsumeItems(material.Key, material.Value);
        }

        if (equipType == EquipType.Weapon)
        {
            currentEquip = nextEquip;
            character.Growth.UpgradeWeapon();
        }
        else
        {
            currentEquip = nextEquip;
            character.Growth.UpgradeArmor();
        }
    }

    private void OnClickEquipmentUpgradeUICloseButton()
    {
        Managers.UI.CloseUI(this);
    }
}
