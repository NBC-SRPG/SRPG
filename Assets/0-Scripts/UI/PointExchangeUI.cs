using UnityEngine;

public class PointExchangeUI : UIBase
{
    private enum Texts
    {
        PointText
    }

    private enum GameObjects
    {
        Content
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
        BindObject(typeof(GameObjects));

        GetText((int)Texts.PointText).text = Managers.AccountData.playerData.gachaPoint.ToString();

        foreach (GachaSO gachaSO in Managers.UI.FindUI<GachaUI>().curGachaList)
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Resource.Load<GameObject>("Prefabs/UI/CharacterExchangeEntryUI"),
                GetObject((int)GameObjects.Content).transform);

            go.GetComponent<CharacterExchangeEntryUI>().Init(gachaSO);
        }

        Managers.AccountData.playerData.OnGachaPointChanged += UpdateGachaPoint;
    }

    private void UpdateGachaPoint(int gachaPoint)
    {
        GetText((int)Texts.PointText).text = gachaPoint.ToString();
    }
}
