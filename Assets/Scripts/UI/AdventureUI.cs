using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureUI : UIBase
{
    private enum Buttons
    {
        PVPButton,
        WeeklyContentButton,
        CommodityFarmingButton,
        MainStoryButton,
        BackButton
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.PVPButton).onClick.AddListener(OnClickPVPButton);
        GetButton((int)Buttons.WeeklyContentButton).onClick.AddListener(OnClickWeeklyContentButton);
        GetButton((int)Buttons.CommodityFarmingButton).onClick.AddListener(OnClickCommodityFarmingButton);
        GetButton((int)Buttons.MainStoryButton).onClick.AddListener(OnClickMainStoryButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
    }

    private void OnClickPVPButton()
    {
        Debug.Log("OnClickPVPButton");
    }
    private void OnClickWeeklyContentButton()
    {
        Debug.Log("OnClickWeeklyContentButton");
    }
    private void OnClickCommodityFarmingButton()
    {
        Debug.Log("OnClickCommodityFarmingButton");
    }
    private void OnClickMainStoryButton()
    {
        Debug.Log("OnClickMainStoryButton");
    }
    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");

        Managers.UI.CloseUI(this);
    }
}
