using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Constants;

public class CharacterUI : UIBase
{
    private List<Character> characters = new();
    private enum Buttons
    {
        BackButton
    }
    private enum Dropdowns
    {
        FilterDropdown,
        SortDropdown
    }
    private enum GameObjects
    {
        Content
    }
    // 필터 타입
    private enum FilterType
    {
        // 타입? 진영? 클래스?
        // 전체, 세부
        Option1,
        Option2,
        Option3
    }
    // 실제 게임에서 보이게 할 한글 목록
    // 한글로 List를 만들어서 넘겨주어도 되지만 가독성 때문에 해당 방법 채택
    private Dictionary<FilterType, string> filterDic = new Dictionary<FilterType, string>
    {
        { FilterType.Option1, "전체" },
        { FilterType.Option2, "근거리" },
        { FilterType.Option3, "원거리" }
    };
    // 정렬 타입
    private enum SortType
    {
        // 레벨 오름차순, 내림차순
        // 편성된 애들은 따로 Sort
        // 별 순으로 오름차순, 내림차순
        Option1,
        Option2,
        Option3,
        Option4
    }
    // 실제 게임에서 보이게 할 한글 목록
    private Dictionary<SortType, string> sortDic = new Dictionary<SortType, string>
    {
        { SortType.Option1, "별 내림차순" },
        { SortType.Option2, "별 오름차순" },
        { SortType.Option3, "레벨 내림차순" },
        { SortType.Option4, "레벨 오름차순" }
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

        foreach (Character character in Managers.AccountData.characterData.Values)
        {
            GameObject go = Managers.Resource.Instantiate("UI/CharacterEntryUI", GetObject((int)GameObjects.Content).transform);
            go.GetComponent<CharacterEntryUI>().Init(character);

            characters.Add(character);
        }

        SortSelect();
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
        List<Character> filteredCharacters = new();
        switch (filterType)
        {
            case FilterType.Option1:
                Debug.Log("Selected option: " + filterDic[filterType]);
                UpdateCharacterList(characters);
                break;
            case FilterType.Option2:
                Debug.Log("Selected option: " + filterDic[filterType]);
                filteredCharacters = characters.Where(character => character.SO.attackMethod == AttackMethod.Melee).ToList();
                UpdateCharacterList(filteredCharacters);
                break;
            case FilterType.Option3:
                Debug.Log("Selected option: " + filterDic[filterType]);
                filteredCharacters = characters.Where(character => character.SO.attackMethod == AttackMethod.Range).ToList();
                UpdateCharacterList(filteredCharacters);
                break;
        }

        SortSelect();
    }

    // 정렬 선택
    private void SortSelect()
    {
        // 현재 선택된 정렬 타입 Get
        SortType sortType = (SortType)Get<TMP_Dropdown>((int)Dropdowns.SortDropdown).value;
        List<CharacterEntryUI> entries = GetObject((int)GameObjects.Content).GetComponentsInChildren<CharacterEntryUI>().ToList();
        // 정렬 타입에 따라 정렬 실행
        switch (sortType)
        {
            case SortType.Option1:
                entries.Sort((x, y) => y.character.Growth.star.CompareTo(x.character.Growth.star));
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
            case SortType.Option2:
                entries.Sort((x, y) => x.character.Growth.star.CompareTo(y.character.Growth.star));
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
            case SortType.Option3:
                entries.Sort((x, y) => y.character.Growth.level.CompareTo(x.character.Growth.level));
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
            case SortType.Option4:
                entries.Sort((x, y) => x.character.Growth.level.CompareTo(y.character.Growth.level));
                Debug.Log("Selected option: " + sortDic[sortType]);
                break;
        }

        foreach (var entry in entries)
        {
            entry.transform.SetAsLastSibling();
        }
    }

    private void UpdateCharacterList(List<Character> characters)
    {
        Transform content = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var character in characters)
        {
            GameObject go = Managers.Resource.Instantiate("UI/CharacterEntryUI", content);
            go.GetComponent<CharacterEntryUI>().Init(character);
        }
    }
}
