using System.Collections;
using UnityEngine;
public class EquipmentUpgradeMaterialUI : UIBase
{
    private bool isLoaded = false;
    private ItemSO item;
    private enum Texts
    {
        EquipmentUpgradeMaterialName,
        EquipmentUpgradeMaterialQuantity
    }

    private enum Images
    {
        EquipmentUpgradeMaterialImage
    }

    public void Init(int itemId, int itemCount)
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        StartCoroutine(InitItem(itemId, itemCount));
    }

    private IEnumerator InitItem(int itemId, int itemCount)
    {
        Utility.Id2SO<ItemSO>(itemId, (result) =>
        {
            item = (ItemSO)result;
            isLoaded = true;
        });

        yield return new WaitUntil(() => isLoaded);

        if (itemCount > Managers.AccountData.inventory[itemId])
        {
            GetText((int)Texts.EquipmentUpgradeMaterialQuantity).text = $"<color=red>{Managers.AccountData.inventory[itemId]}</color>/{itemCount}";
        }
        else
        {
            GetText((int)Texts.EquipmentUpgradeMaterialQuantity).text = $"{Managers.AccountData.inventory[itemId]}/{itemCount}";
        }

        GetText((int)Texts.EquipmentUpgradeMaterialName).text = item.itemName;
        GetImage((int)Images.EquipmentUpgradeMaterialImage).sprite = item.icon;
    }
}
