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
            GameObject go = Managers.Resource.Instantiate(Managers.Resource.Load<GameObject>("Prefabs/UI/CharacterImage"),
                GetObject((int)GameObjects.Content).transform);

            go.GetComponent<Image>().sprite = Managers.AccountData.characterData[gachaResult].SO.icon;
        }

        // TODO
        // 전달받은 캐릭터 리스트 획득 처리
    }

    private void OnClickCheckButton()
    {
        Managers.UI.CloseUI(this);
    }
}
