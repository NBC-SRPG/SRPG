using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryUI : UIBase
{
    private enum Texts
    {

    }

    private enum Images
    {

    }

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
        Option2
    }

    private Dictionary<FilterType, string> filterDic = new Dictionary<FilterType, string>
    {
        { FilterType.Option1, "필터 옵션 1" },
        { FilterType.Option2, "필터 옵션 2" }
    };

    private enum SortType
    {
        Option1,
        Option2
    }

    private Dictionary<SortType, string> sortDic = new Dictionary<SortType, string>
    {
        { SortType.Option1, "정렬 옵션 1" },
        { SortType.Option2, "정렬 옵션 2" }
    };

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        Bind<TMP_Dropdown>(typeof(Dropdowns));
        InitDropdown();

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        foreach (var item in Managers.AccountData.inventory)
        {
            Utility.Id2SO<ItemData>(item.Key, (result) =>
            {
                ItemEntryUI ui = Managers.UI.ShowUI<ItemEntryUI>();
                ui.Init(result as ItemData);
            });
        }
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
        Get<TMP_Dropdown>((int)Dropdowns.FilterDropdown).onValueChanged.AddListener(delegate { FilterSelect(); });
        Get<TMP_Dropdown>((int)Dropdowns.SortDropdown).onValueChanged.AddListener(delegate { SortSelect(); });
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }

    // 필터 선택
    private void FilterSelect()
    {
        // 현재 선택된 필터 타입 Get
        FilterType filterType = (FilterType)Get<TMP_Dropdown>((int)Dropdowns.FilterDropdown).value;
        // 필터 타입에 따라 필터 실행
        switch (filterType)
        {
            case FilterType.Option1:
                Debug.Log("Selected option: " + filterDic[filterType]);
                break;
            case FilterType.Option2:
                Debug.Log("Selected option: " + filterDic[filterType]);
                break;
        }
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
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
            case SortType.Option2:
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
        }
    }
}
