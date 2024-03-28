using JetBrains.Annotations;
using System.Collections.Generic;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, Character> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, bool> missionClearData { get; set; }
    public Dictionary<int, int> inventory { get; set; }  //인벤토리 = 아이템 데이터, 보유 갯수. //Todo : 아이템 DB 작업 완료 후 ItemData → int itemId로 바꾸고 메서드 수정하기.
    public Dictionary<int, string[]> friendData { get; set; }
    public Dictionary<int, FormationData> formationData { get; set; }
    public VersionData versionData { get; set; }
    public int gachaPoint { get; set; }
    public List<MailSO> mailBox { get; set; }

    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, Character> characterData,
        PlayerData playerData,
        Dictionary<int, bool> missionClearData,
        //Dictionary<ItemData, int> inventory,
        Dictionary<int, string[]> friendData,
        Dictionary<int, FormationData> formationData,
        List<MailSO> mailBox
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, Character>();
        this.playerData = playerData ?? new PlayerData();
        this.missionClearData = missionClearData ?? new Dictionary<int, bool>();
        this.inventory = inventory ?? new Dictionary<int, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
        this.formationData = formationData ?? new Dictionary<int, FormationData>();
        this.mailBox = mailBox ?? new List<MailSO>();
    }

    // 임시 메소드: inventory에 아이템 추가
    public bool AddItem(int itemId, int quantity)
    {
        // 이미 최대치에 도달한 경우 추가하지 않음
        if (inventory.ContainsKey(itemId) && inventory[itemId] >= 999) //999를 아이템 DB 추가 후에 Item의 MaxReserve 값으로 바꿀 것
        {
            Debug.LogWarning("아이템 갯수가 최대치입니다.");
            return false;
        }

        // 최대치에 도달하지 않았을 경우 아이템을 추가
        if (inventory.ContainsKey(itemId))
        {
            // 이미 아이템이 inventory에 있으면 수량을 증가시킴
            inventory[itemId] += quantity;
            return true;
        }
        else
        {
            // 아이템이 inventory에 없으면 새로 추가
            inventory.Add(itemId, quantity);
            return true;
        }
    }

    // 임시 메소드: inventory에서 아이템 제거
    public void RemoveItem(int itemId, int quantity)
    {
        if (inventory.ContainsKey(itemId))
        {
            // 해당 아이템이 inventory에 있으면 수량을 감소시킴
            inventory[itemId] -= quantity;

            // 만약 수량이 0 이하로 떨어졌을 경우 아이템을 제거함
            if (inventory[itemId] <= 0)
            {
                inventory.Remove(itemId);
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
            //Debug.Log(item.Key.item_Id + " " + item.Key.itemName + ": " + item.Value); 
        }
    }

    // 인벤토리에서 아이템을 사용할 때 보유량을 확인하고, 보유량이 충분하다면 그 갯수만큼 아이템 갯수를 차감하고, bool값을 반환하는 메서드
    public bool UseItem(int itemId, int requiredQuantity)
    {
        if (inventory.ContainsKey(itemId))
        {
            if (inventory[itemId] >= requiredQuantity) // 아이템을 사용할 수 있는 경우
            {
                RemoveItem(itemId, requiredQuantity); //아이템 갯수만큼 차감

                return true; 
            }
            else
            {
                Debug.LogWarning("Insufficient quantity of " + itemId);
                return false; // 아이템 보유량 부족
            }
        }
        else
        {
            Debug.LogWarning(itemId + " is not in the inventory.");
            return false; // 아이템이 인벤토리에 없음
        }
    }

    // 인벤토리에서 특정 아이템이 있는지 없는지, 있다면 몇개인지 리턴하는 메서드
    public int GetItemQuantity(int itemId)
    {
        if (inventory.ContainsKey(itemId))
        {
            return inventory[itemId];
        }
        else
        {
            return 0; // 아이템이 인벤토리에 없으면 0을 반환
        }
    }
}
