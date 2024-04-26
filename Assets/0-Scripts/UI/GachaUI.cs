using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class GachaUI : UIBase
{
    /// <summary>
    /// 현재 진행중인 가챠의 리스트
    /// 0번 인덱스는 통상뽑기로 항상 0의 값이 할당
    /// 1번 인덱스부터는 현재 픽업캐릭터의 id를 할당
    /// </summary>

    private const int GACHA_TICKET = 90000;
    private const int GACHA_TICKET_10 = 90001;
    
    private Dictionary<int, Dictionary<int, float>> tables = new Dictionary<int, Dictionary<int, float>>();
    public List<GachaSO> curGachaList { get; } = new(); // 현재 진행중인 가챠리스트. 0번은 항상 통상
    private GachaSO curGacha;


    private enum Texts
    {
        GachaName,
        GachaInfo,
        GachaPoint,
        EndDate,
        PickUpCharacterName,
        GachaCountText,
        Gacha10CountText
    }
    private enum Buttons
    {
        GachaButton,
        Gacha10Button,
        GachaWithTicketButton,
        Gacha10WithTicketButton,
        PercentageInfoButton,
        CharacterInfoButton,
        PointExchangeButton,
        BackButton
    }
    private enum Images
    {
        GachaImage,
    }

    private enum GameObjects
    {
        Content,
        PickUpInfo,
        GachaPointUI
        // Star : 모든 픽업은 3성이라 고정 이미지 사용
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Managers.AccountData.playerData.OnGachaPointChanged -= UpdateGachaPoint;
    }

    private void Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        StartCoroutine(SetGachaInfo());
        StartCoroutine(GetTableFromDB());

        GetText((int)Texts.GachaPoint).text = Managers.AccountData.playerData.gachaPoint.ToString();

        GetButton((int)Buttons.GachaButton).onClick.AddListener(OnClickGachaButton);
        GetButton((int)Buttons.Gacha10Button).onClick.AddListener(OnClickGacha10Button);
        GetButton((int)Buttons.GachaWithTicketButton).onClick.AddListener(OnClickGachaWithTicketButton);
        GetButton((int)Buttons.Gacha10WithTicketButton).onClick.AddListener(OnClickGacha10WithTicketButton);

        GetButton((int)Buttons.PercentageInfoButton).onClick.AddListener(OnClickPercentageInfoButton);
        GetButton((int)Buttons.CharacterInfoButton).onClick.AddListener(OnClickCharacterInfoButton);
        GetButton((int)Buttons.PointExchangeButton).onClick.AddListener(OnClickPointExchangeButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);


        if (Managers.AccountData.GetItemQuantity(GACHA_TICKET) == 0) GetButton((int)Buttons.GachaWithTicketButton).gameObject.SetActive(false);
        if (Managers.AccountData.GetItemQuantity(GACHA_TICKET_10) == 0) GetButton((int)Buttons.Gacha10WithTicketButton).gameObject.SetActive(false);

        Managers.AccountData.playerData.OnGachaPointChanged += UpdateGachaPoint;
    }

    // 클릭한 배너에 맞게 가챠 정보 세팅
    public void UpdateGachaInfoUI(GachaSO gachaSO)
    {
        curGacha = gachaSO;

        if (gachaSO.gachaType == Constants.GachaType.Common)
        {
            GetObject((int)GameObjects.PickUpInfo).SetActive(false);
            GetObject((int)GameObjects.GachaPointUI).SetActive(false);

            GetText((int)Texts.GachaName).text = "통상 계약";
            //GetText((int)Texts.GachaInfo).text = "10연차 시 ★2 이상의 캐릭터 100% 계약";

            GetImage((int)Images.GachaImage).sprite = gachaSO.gachaImage;
        }
        else
        {
            GetObject((int)GameObjects.PickUpInfo).SetActive(true);
            GetObject((int)GameObjects.GachaPointUI).SetActive(true);

            GetText((int)Texts.GachaName).text = "픽업 계약";
            GetText((int)Texts.PickUpCharacterName).text = gachaSO.pickUpcharacterName;
            //GetText((int)Texts.GachaInfo).text = "10연차 시 ★2 이상의 캐릭터 100% 계약";
            GetText((int)Texts.EndDate).text = $"남은 기간\n{gachaSO.GetRemainingTimeText()}";

            GetImage((int)Images.GachaImage).sprite = gachaSO.gachaImage;
        }
    }

    private void UpdateGachaPoint(int gachaPoint)
    {
        GetText((int)Texts.GachaPoint).text = gachaPoint.ToString();
    }

    private void OnClickGachaButton()
    {
        if (Managers.AccountData.playerData.ReduceDiamond(250) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("다이아가 부족합니다.");
            return;
        }
        ShowResult(Draw(curGacha.tableId));
    }

    private void OnClickGacha10Button()
    {
        if (Managers.AccountData.playerData.ReduceDiamond(2500) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("다이아가 부족합니다.");
            return;
        }
        ShowResult(Draw10Times(curGacha.tableId));
    }

    private void OnClickGachaWithTicketButton()
    {
        Managers.AccountData.ConsumeItems(GACHA_TICKET, 1);
        ShowResult(Draw(curGacha.tableId));
    }

    private void OnClickGacha10WithTicketButton()
    {
        Managers.AccountData.ConsumeItems(GACHA_TICKET_10, 1);
        ShowResult(Draw10Times(curGacha.tableId));
    }

    private void OnClickPercentageInfoButton()
    {
        // TODO: 확률정보 UI 제작 후 작성
    }

    private void OnClickCharacterInfoButton()
    {
        // TODO: 픽업 캐릭터 정보 UI 제작 후 작성
    }

    private void OnClickPointExchangeButton()
    {
        // 포인트로 천장 교환 팝업 열기

        Managers.UI.ShowUI<PointExchangeUI>();
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        Managers.UI.CloseUI(this);
    }

    private IEnumerator SetGachaInfo()
    {
        long count = -1;    // count가 0이 되면 모든 데이터 로드 완료
        Managers.DB.Read(Managers.DB.reference.Child("Version").Child("curGacha"), (snapshot) =>
        {
            count = snapshot.ChildrenCount;
            foreach (var gachaInfo in snapshot.Children)
            {
                // TODO
                // 비동기라 curGachaList에 들어가는 순서가 보장되지 않을 것 같음
                // 0번째가 항상 통상이 되리라는 보장이 없음
                // 딕셔너리로 변경??
                // curGachaList가 여기에서만 쓰인다면 삭제 후 Id2SO 콜백 안에서 BannerUI 생성이 더 좋아보임
                Utility.Id2SO<GachaSO>(int.Parse(gachaInfo.Value.ToString()), (result) =>
                {
                    curGachaList.Add(result as GachaSO);
                    count--;
                });
            }
        });
        yield return new WaitUntil(() => count == 0);
        //curGacha = curGachaList[0];
        UpdateGachaInfoUI(curGachaList[0]);

        // 가챠 배너 생성
        foreach (GachaSO curGachaSO in curGachaList)
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Resource.Load<GameObject>("Prefabs/UI/BannerUI"),
                GetObject((int)GameObjects.Content).transform);

            go.GetComponent<BannerUI>().Init(curGachaSO);
        }
    }
    private IEnumerator GetTableFromDB()
    {
        long count = -1;    // count가 0이 되면 모든 데이터 로드 완료
        Managers.DB.Read(Managers.DB.reference.Child("GachaTables"), (snapshot) =>
        {
            count = snapshot.ChildrenCount;
            foreach (DataSnapshot data in snapshot.Children)
            {
                Dictionary<int, float> table = new();
                foreach(var exp in data.Children)
                {
                    table.Add(int.Parse(exp.Key), float.Parse(exp.Value.ToString()));
                }

                foreach (var item in table)
                {
                    Debug.Log(item.Value.ToString());
                }
                tables.Add(int.Parse(data.Key), table);
                count--;
            }
            // TODO: 이중반복 구조가 마음에 안듦... JSON 역직렬화로 바꿀 것
        });
        yield return new WaitUntil(() => count == 0);
    }

    private Dictionary<int, float> GetTable(int tableId)
    {
        if (tables.ContainsKey(tableId))
        {
            return tables[tableId];
        }
        else
        {
            return null;
        }
    }

    // 테이블에서 확률에 따라 캐릭터 id를 하나 반환
    private int GetRandomCharacterFromTable(Dictionary<int, float> table)
    {
        double chance = Random.Range(0.0f, 100.0f); // [0.0, 100.0]
        Debug.Log(chance);

        int result = 0;
        double current = 0.0;
        foreach (var pair in table)
        {
            result = pair.Key;
            current += (double)pair.Value;
            if (chance < current)
            {
                break;
            }
        }
        return result;
    }

    // 1회 뽑기
    private List<int> Draw(int tableId)
    {
        List<int> list = new List<int>();
        int result = GetRandomCharacterFromTable(GetTable(tableId));
        Managers.AccountData.AcquireCharacter(result, result == curGacha.id);
        list.Add(result);
        if (curGacha.gachaType == Constants.GachaType.PickUp)
        {
            Managers.AccountData.playerData.AddGachaPoint(1);
        }
        return list;
    }

    // 10회 뽑기
    private List<int> Draw10Times(int tableId)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < 9; i++)
        {
            int result = GetRandomCharacterFromTable(GetTable(tableId));
            Managers.AccountData.AcquireCharacter(result, result == curGacha.id);
            list.Add(result);
        }
        // 2성 이상만 나오는 테이블
        int result2 = GetRandomCharacterFromTable(GetTable(tableId + 10000));
        Managers.AccountData.AcquireCharacter(result2, result2 == curGacha.id);
        list.Add(result2);


        if (curGacha.gachaType == Constants.GachaType.PickUp)
        {
            Managers.AccountData.playerData.AddGachaPoint(10);
        }
        return list;
    }

    private void ShowResult(List<int> result)
    {
        // 전달받은 리스트로 가차 결과창(GachaResultUI) 보여주기
        GachaResultUI GachaResultUI = Managers.UI.ShowUI<GachaResultUI>();
        GachaResultUI.Init(result);
    }
}
