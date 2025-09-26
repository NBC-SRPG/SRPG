using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultUI : UIBase
{
    private enum Texts
    {
        PointText
    }

    private enum Buttons
    {
        CheckButton
    }

    private enum GameObjects
    {
        Content
    }

    public void Init(List<int> gachaResultList)
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetText((int)Texts.PointText).text = Managers.AccountData.playerData.gachaPoint.ToString();
        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);

        foreach (int gachaResult in gachaResultList)
        {
            GameObject go = Managers.Resource.Load<GameObject>("Prefabs/UI/GachaEntryUI");
            go.GetComponent<GachaEntryUI>().characterId = gachaResult;
            Managers.Resource.Instantiate(go, GetObject((int)GameObjects.Content).transform);

        }
    }

    private void OnClickCheckButton()
    {
        Managers.UI.CloseUI(this);
    }
}
