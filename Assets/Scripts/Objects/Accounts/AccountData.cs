using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, Character> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, bool> missionClearData { get; set; }
    public Dictionary<ItemData, int> inventory { get; set; }  //인벤토리 = 아이템 데이터, 보유 갯수. //Todo : 아이템 DB 작업 완료 후 ItemData → int itemId로 바꾸고 메서드 수정하기.
    public Dictionary<int, string[]> friendData { get; set; }
    public VersionData versionData { get; set; }
    public int gachaPoint { get; set; }

    // Init 
    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, Character> characterData,
        PlayerData playerData,
        Dictionary<int, bool> missionClearData,
        Dictionary<ItemData, int> inventory,
        Dictionary<int, string[]> friendData
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, Character>();
        this.playerData = playerData ?? new PlayerData();
        this.missionClearData = missionClearData ?? new Dictionary<int, bool>();
        this.inventory = inventory ?? new Dictionary<ItemData, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
    }

    // 임시 메소드: inventory에 아이템 추가
    public bool AddItem(ItemData item, int quantity)
    {
        // 이미 최대치에 도달한 경우 추가하지 않음
        if (inventory.ContainsKey(item) && inventory[item] >= item.maxReservesNumbers)
        {
            Debug.LogWarning("아이템 갯수가 최대치입니다.");
            return false;
        }

        // 최대치에 도달하지 않았을 경우 아이템을 추가
        if (inventory.ContainsKey(item))
        {
            // 이미 아이템이 inventory에 있으면 수량을 증가시킴
            inventory[item] += quantity;
            return true;
        }
        else
        {
            // 아이템이 inventory에 없으면 새로 추가
            inventory.Add(item, quantity);
            return true;
        }
    }

    // 임시 메소드: inventory에서 아이템 제거
    public void RemoveItem(ItemData item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            // 해당 아이템이 inventory에 있으면 수량을 감소시킴
            inventory[item] -= quantity;

            // 만약 수량이 0 이하로 떨어졌을 경우 아이템을 제거함
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
        else
        {
            Debug.LogWarning("아이템이 인벤토리에 없습니다.");
        }
    }

    // 임시 메소드: inventory의 아이템을 출력
    public void PrintInventory()
    {
        foreach (var item in inventory)
        {
            Debug.Log(item.Key.item_Id + " " + item.Key.itemName + ": " + item.Value);
        }
    }

    // 인벤토리에서 아이템을 사용할 때 보유량을 확인하고, 보유량이 충분하다면 그 갯수만큼 아이템 갯수를 차감하고, bool값을 반환하는 메서드
    public bool UseItem(ItemData item, int requiredQuantity)
    {
        if (inventory.ContainsKey(item))
        {
            if (inventory[item] >= requiredQuantity) // 아이템을 사용할 수 있는 경우
            {
                RemoveItem(item, requiredQuantity); //아이템 갯수만큼 차감

                return true; 
            }
            else
            {
                Debug.LogWarning("Insufficient quantity of " + item.item_Id);
                return false; // 아이템 보유량 부족
            }
        }
        else
        {
            Debug.LogWarning(item.item_Id + " is not in the inventory.");
            return false; // 아이템이 인벤토리에 없음
        }
    }

    // 인벤토리에서 특정 아이템이 있는지 없는지, 있다면 몇개인지 리턴하는 메서드
    public int GetItemQuantity(ItemData item)
    {
        if (inventory.ContainsKey(item))
        {
            return inventory[item];
        }
        else
        {
            return 0; // 아이템이 인벤토리에 없으면 0을 반환
        }
    }
}
