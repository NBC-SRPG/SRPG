using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUI : UIBase
{
    private Constants.GachaType gachaType;
    private string tableName;

    /// <summary>
    /// 현재 진행중인 가챠의 리스트
    /// 0번 인덱스는 통상뽑기로 항상 0의 값이 할당
    /// 1번 인덱스부터는 현재 픽업캐릭터의 id를 할당
    /// </summary>
    private List<int> gachaList;

    private enum Texts
    {
        GachaName,
        GachaInfo,
        GachaPoint,
        EndDate
    }
    private enum Buttons
    {
        GeasButton,
        Geas10Button,
        GeasWithScrollButton,
        PercentageInfoButton,
        CharacterInfoButton,
        PointExchangeButton,
        CommonBanner,
        PickUpBanner
    }
    private enum Images
    {
        PickUpImage,
        CommonBanner,
        PickUpBanner
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.GeasButton).onClick.AddListener(OnGeasButton);
        GetButton((int)Buttons.Geas10Button).onClick.AddListener(OnGeas10Button);
        GetButton((int)Buttons.GeasWithScrollButton).onClick.AddListener(OnGeasWithScrollButton);
        GetButton((int)Buttons.PercentageInfoButton).onClick.AddListener(OnPercentageInfoButton);
        GetButton((int)Buttons.CharacterInfoButton).onClick.AddListener(OnCharacterInfoButton);
        GetButton((int)Buttons.PointExchangeButton).onClick.AddListener(OnPointExchangeButton);
        GetButton((int)Buttons.CommonBanner).onClick.AddListener(OnCommonBannerButton);
        GetButton((int)Buttons.PickUpBanner).onClick.AddListener(OnPickUpBannerButton);

        gachaType = Constants.GachaType.Common;
        gachaList = Managers.AccountData.versionData.curGacha;
    }

    private void OnGeasButton()
    {
        Debug.Log("OnGeasButton");
        // 다이아를 소모해 1회 뽑기
    }

    private void OnGeas10Button()
    {
        Debug.Log("OnGeas10Button");
        // 다이아를 소모해 10회 뽑기
    }

    private void OnGeasWithScrollButton()
    {
        Debug.Log("OnGeasWithScrollButton");
        // 1회 가챠권을 소모해 뽑기
    }

    private void OnPercentageInfoButton()
    {
        Debug.Log("OnPercentageInfoButton");
        // 확률 정보 보기
    }

    private void OnCharacterInfoButton()
    {
        Debug.Log("OnCharacterInfoButton");
        // 픽업 캐릭터 정보 보기
    }

    private void OnPointExchangeButton()
    {
        Debug.Log("OnPointExchangeButton");
        // 포인트로 천장 교환 팝업 열기
    }

    private void OnCommonBannerButton()
    {
        Debug.Log("OnCommonBannerButton");
        // 통상 가챠로 전환
        gachaType = Constants.GachaType.Common;
    }

    private void OnPickUpBannerButton()
    {
        Debug.Log("OnPickUpBannerButton");
        // 해당하는 픽업 가챠로 전환
        gachaType = Constants.GachaType.PickUp;
    }


}
