using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : UIBase
{
    private enum Buttons
    {
        BackButton
    }
    private enum Dropdowns
    {
        FilterDropdown,
        SortDropdown
    }
    // 필터 타입
    private enum FilterType
    {
        // 타입? 진영? 클래스?
        // 전체, 세부
        Option1,
        Option2
    }
    // 실제 게임에서 보이게 할 한글 목록
    // 한글로 List를 만들어서 넘겨주어도 되지만 가독성 때문에 해당 방법 채택
    private Dictionary<FilterType, string> filterDic = new Dictionary<FilterType, string>
    {
        { FilterType.Option1, "필터 옵션 1" },
        { FilterType.Option2, "필터 옵션 2" }
    };
    // 정렬 타입
    private enum SortType
    {
        // 레벨 오름차순, 내림차순
        // 편성된 애들은 따로 Sort
        // 별 순으로 오름차순, 내림차순
        Option1,
        Option2
    }
    // 실제 게임에서 보이게 할 한글 목록
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
        // TODO
        // 캐릭터 UI 생성 시 보유 캐릭터 정보를 가지고 초기화
        // 캐릭터 버튼 UI를 새로 만들기
        // 캐릭터 UI 생성 시 보유 캐릭터부터 시작해서 캐릭터 버튼 UI를 동적 생성
        // 가지고 있지 않은 캐릭터도 보게 한다면 가지고 있지 않은 캐릭터도 버튼 UI 동적 생성
        // 1. 플레이어 데이터에서 가지고있는 캐릭터 목록을 가져온다.
        // 2. 정렬 타입에 맞게 정렬한다.
        // 3. 플레이어 버튼 UI를 생성한다.
        // 4. 이 때 버튼 UI에게 정보를 넘겨준다.
        // 5. 똑같이 갖고 있지 않는 캐릭터 목록에도 적용
        // 
        // 1. 필터 적용 시
        // 2. UI를 밀어버리고
        // 3. 플레이어 데이터에서 가지고 있는 캐릭터 목록을 가져온다.
        // 4. 필터에 맞는 애들만 생성
        BindButton(typeof(Buttons));

        Bind<TMP_Dropdown>(typeof(Dropdowns));
        InitDropdown();

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
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
    /*
필터 항목을 클릭하였을 때 전체 캐릭터 삭제 & 필터에 해당하는 캐릭터들로 초기화

정렬 항목에 맞게 캐릭터 정렬

캐릭터 버튼 클릭 (캐릭터 데이터 or 캐릭터 id)

캐릭터 UI 생성 및 해당 값 넘겨주기
    */
}
