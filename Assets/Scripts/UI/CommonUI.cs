using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUI : UIBase
{
    // 테스트용 임시 변수들
    private int maxAp = 120;
    private int curAp = 96;
    private int curGold = 32000;
    private int curDiamond = 1600;
    private int recoveryApTime = 1 * 10;
    private float recoveryTimer = 1 * 10;
    private enum Texts
    {
        ApText,
        ApTimerText,
        GoldText,
        DiamondText
    }
    private enum Buttons
    {
        ApButton,
        GoldButton,
        DiamondButton
    }
    private enum Images
    {
        ApFillImage
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // Common UI는 항상 모든 UI의 위에 있어야 함
        Managers.UI.SetCanvas(gameObject, false);
        // UI 내의 텍스트, 버튼, 이미지, 오브젝트 바인딩
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        //BindObject(typeof(GameObjects));

        // 버튼에 클릭 이벤트 추가
        GetButton((int)Buttons.ApButton).onClick.AddListener(OnClickApButton);
        GetButton((int)Buttons.GoldButton).onClick.AddListener(OnClickGoldButton);
        GetButton((int)Buttons.DiamondButton).onClick.AddListener(OnClickDiamondButton);

        // UI 업데이트
        RefreshAp();
        RefreshGold();
        RefreshDiamond();
    }

    private void Update()
    {
        RefreshApTime();
    }
    // Ap 회복 타이머 텍스트 업데이트
    private void RefreshApTime()
    {
        // AP가 MAX가 아니라면 타이머 작동
        if (curAp < maxAp)
        {
            recoveryTimer -= Time.deltaTime;
            // 회복 시간이 되었다면 Ap 1회복 & 타이머 초기화
            if (recoveryTimer <= 0)
            {
                curAp++;
                recoveryTimer = recoveryApTime;
                // Ap 회복 업데이트
                RefreshAp();
            }
            // 타이머를 분, 초로 나누어서 텍스트 업데이트
            int minutes = (int)recoveryTimer / 60;
            int seconds = (int)recoveryTimer % 60;
            GetText((int)Texts.ApTimerText).text = $"{minutes:D2} : {seconds:D2}";
        }
    }
    // Ap, Gold, Diamond 텍스트 업데이트
    // Ap 회복, Ap 획득, Ap 구매, Gold 획득, Gold 구매, Diamond 획득, Diamond 구매 시 Refresh 필요
    public void RefreshAp()
    {
        GetText((int)Texts.ApText).text = $"{curAp} / {maxAp}";
        GetImage((int)Images.ApFillImage).fillAmount = (float)curAp / maxAp;
    }
    public void RefreshGold()
    {
        GetText((int)Texts.GoldText).text = $"{curGold}";
    }
    public void RefreshDiamond()
    {
        GetText((int)Texts.DiamondText).text = $"{curDiamond}";
    }
    // Ap, Gold, Diamond 버튼 눌렀을 때 구매 or 충전 페이지가 열리도록 하기
    // ApChargeUI, GoldChargeUI, DiamondChargeUI로 나누어져 있지 않고 하나의 페이지에서 탭만 다르게 하기
    // or 인게임 재화인 Ap, Gold는 같은 UI 다른 탭, 현금 재화인 다이아는 다른 UI로 하기
    private void OnClickApButton()
    {
        Debug.Log("OnClickApButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<ApChargeUI>();
    }
    private void OnClickGoldButton()
    {
        Debug.Log("OnClickGoldButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<GoldChargeUI>();
    }
    private void OnClickDiamondButton()
    {
        Debug.Log("OnClickDiamondButton");

        // Managers.Sound(Sound.Effect, "ButtonClick");
        // Managers.UI.ShowUI<DiamondChargeUI>();
    }
}
