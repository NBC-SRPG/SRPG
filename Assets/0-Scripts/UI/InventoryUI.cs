using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Constants;

public class InventoryUI : UIBase
{
    private List<ItemSO> loadedItems = new();

    private enum Buttons
    {
        BackButton
    }

    private enum GameObjects
    {
        Content
    }

    private enum Dropdowns
    {
        FilterDropdown,
        SortDropdown
    }

    private enum FilterType
    {
        Option1,
        Option2,
        Option3,
        Option4,
        Option5,
        Option6,
        Option7,
        Option8,
        Option9,
        Option10
    }

    private Dictionary<FilterType, string> filterDic = new Dictionary<FilterType, string>
    {
        { FilterType.Option1, "전체" },
        { FilterType.Option2, "조각" },
        { FilterType.Option3, "성장" },
        { FilterType.Option4, "재료" },
        { FilterType.Option5, "소비" },
        { FilterType.Option6, "티켓" },
        { FilterType.Option7, "토큰" },
        { FilterType.Option8, "선물" },
        { FilterType.Option9, "기타" }
    };

    private enum SortType
    {
        Option1,
        Option2
    }

    private Dictionary<SortType, string> sortDic = new Dictionary<SortType, string>
    {
        { SortType.Option1, "오름차순" },
        { SortType.Option2, "내림차순" }
    };

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        Bind<TMP_Dropdown>(typeof(Dropdowns));
        InitDropdown();

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        StartCoroutine(LoadAllItems());
    }

    private IEnumerator LoadAllItems()
    {
        loadedItems.Clear();
        int itemsCount = Managers.AccountData.inventory.Count;
        int loadedCount = 0;

        foreach (var item in Managers.AccountData.inventory)
        {
            Debug.Log(item.Key + ": " + item.Value);
            if (item.Value <= 0)
            {
                itemsCount--;
                continue;
            }

            Utility.Id2SO<ItemSO>(item.Key, (result) =>
            {
                if (result != null)
                {
                    loadedItems.Add(result as ItemSO);
                }
                loadedCount++;
            });
        }

        yield return new WaitUntil(() => loadedCount == itemsCount);

        foreach (var itemSO in loadedItems)
        {
            GameObject go = Managers.Resource.Instantiate("UI/ItemEntryUI", GetObject((int)GameObjects.Content).transform);
            go.GetComponent<ItemEntryUI>().Init(itemSO);
        }

        // 아이템 다 생성 후 기본 정렬(Id 오름차순)
        SortById();
    }

    private void InitDropdown()
    {
        // 드랍다운 리스트 클리어
        Get<TMP_Dropdown>((int)Dropdowns.FilterDropdown).ClearOptions();
        Get<TMP_Dropdown>((int)Dropdowns.SortDropdown).ClearOptions();

        // 딕셔너리의 한글 목록 필터목록에 추가
        List<string> filterOptions = filterDic.Values.ToList();
        Get<TMP_Dropdown>((int)Dropdowns.FilterDropdown).AddOptions(filterOptions);

        // 딕셔너리의 한글 목록 정렬목록에 추가
        List<string> sortOptions = sortDic.Values.ToList();
        Get<TMP_Dropdown>((int)Dropdowns.SortDropdown).AddOptions(sortOptions);

        // 목록 아이템 선택 시 실행 할 함수 추가
        Get<TMP_Dropdown>((int)Dropdowns.FilterDropdown).onValueChanged.AddListener(delegate { FilterItems(); });
        Get<TMP_Dropdown>((int)Dropdowns.SortDropdown).onValueChanged.AddListener(delegate { SortSelect(); });
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }

    // 정렬 선택
    private void SortSelect()
    {
        // 현재 선택된 정렬 타입 Get
        SortType sortType = (SortType)Get<TMP_Dropdown>((int)Dropdowns.SortDropdown).value;
        // 정렬 타입에 따라 정렬 실행
        switch (sortType)
        {
            case SortType.Option1:
                SortById();
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
            case SortType.Option2:
                SortByIdDescending();
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
        }
    }
    // 기본 정렬 (Id 오름차순)
    private void SortById()
    {
        var items = GetObject((int)GameObjects.Content).GetComponentsInChildren<ItemEntryUI>().ToList();
        items.Sort((x, y) => x.GetComponent<ItemEntryUI>().item.id.CompareTo(y.GetComponent<ItemEntryUI>().item.id));

        foreach (var item in items)
        {
            item.transform.SetAsLastSibling();
        }
    }

    private void SortByIdDescending()
    {
        var items = GetObject((int)GameObjects.Content).GetComponentsInChildren<ItemEntryUI>().ToList();
        items.Sort((x, y) => y.GetComponent<ItemEntryUI>().item.id.CompareTo(x.GetComponent<ItemEntryUI>().item.id));

        foreach (var item in items)
        {
            item.transform.SetAsLastSibling();
        }
    }


    // 필터 선택
    private void FilterItems()
    {
        ItemType selectedType = (ItemType)Get<TMP_Dropdown>((int)Dropdowns.FilterDropdown).value - 1;

        Debug.Log("SelectedType: " + selectedType);
        if ((int)selectedType == -1)
        {
            UpdateItemList(loadedItems);
        }
        else
        {
            List<ItemSO> filteredItems = loadedItems.Where(item => item.itemType == selectedType).ToList();
            UpdateItemList(filteredItems);
        }

        // 필터 후에 선택 된 정렬로 재정렬
        SortSelect();
    }

    // 필터에 맞는 아이템 재생성
    private void UpdateItemList(List<ItemSO> items)
    {
        Transform content = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var itemSO in items)
        {
            GameObject go = Managers.Resource.Instantiate("UI/ItemEntryUI", content);
            go.GetComponent<ItemEntryUI>().Init(itemSO);
        }
    }
}
