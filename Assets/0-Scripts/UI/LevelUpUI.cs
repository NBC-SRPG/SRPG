using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class LevelUpUI : UIBase
{
    private Character character;
    // TODO
    // 레벨업 아이템 Id 다른 곳에서도 사용한다면 Constants로 옮기기
    private const int LevelUpItem1Id = 10001;
    private const int LevelUpItem2Id = 10002;
    private const int LevelUpItem3Id = 10003;
    private const int LevelUpItem4Id = 10004;

    private int LevelUpItem1ExpValue;
    private int LevelUpItem2ExpValue;
    private int LevelUpItem3ExpValue;
    private int LevelUpItem4ExpValue;
    private int totalExp;

    // pressInterval 시간만큼 누르고 있을 시 레벨업 아이템 최대 개수
    private float pressInterval = 1f;
    private float pressTimer = 0f;
    private bool isPressed;
    private bool canUseLevelUpItem;

    private enum Texts
    {
        LevelUpItemQuantity_1,
        LevelUpItemQuantity_2,
        LevelUpItemQuantity_3,
        LevelUpItemQuantity_4,
        LevelUpItemSelectNumber1,
        LevelUpItemSelectNumber2,
        LevelUpItemSelectNumber3,
        LevelUpItemSelectNumber4,
        LevelBefore,
        HpBefore,
        AtkBefore,
        DefBefore,
        LevelAfter,
        HpAfter,
        AtkAfter,
        DefAfter,
        LevelUpGoldText,
        MaxLevelBefore,
        MaxLevelAfter,
        AwakeningPieceQuantity,
        AwakeningGoldText,
        ExpText,
    }

    private enum Images
    {
        LevelUpBarChangeImage,
        LevelUpBarFrontImage,
        LevelUpItemImage_1,
        LevelUpItemImage_2,
        LevelUpItemImage_3,
        LevelUpItemImage_4,
        AwakeningPieceImage
    }

    private enum Buttons
    {
        CloseButton,
        LevelUpItemButton_1,
        LevelUpItemButton_2,
        LevelUpItemButton_3,
        LevelUpItemButton_4,
        LevelUpButton,
        LevelUpTab,
        AwakeningTab,
        AwakeningButton,
        ItemReduceButton_1,
        ItemReduceButton_2,
        ItemReduceButton_3,
        ItemReduceButton_4
    }

    private enum GameObjects
    {
        LevelUp,
        Awakening,
        AwakeningLimitText
    }

    public void Init(Character character)
    {
        this.character = character;

        LoadAllItemsExpValue();

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.LevelUpItemButton_1).gameObject, OnPointerUpLevelUpItemButton, UIEvent.PointerUp);
        BindEvent(GetButton((int)Buttons.LevelUpItemButton_2).gameObject, OnPointerUpLevelUpItemButton, UIEvent.PointerUp);
        BindEvent(GetButton((int)Buttons.LevelUpItemButton_3).gameObject, OnPointerUpLevelUpItemButton, UIEvent.PointerUp);
        BindEvent(GetButton((int)Buttons.LevelUpItemButton_4).gameObject, OnPointerUpLevelUpItemButton, UIEvent.PointerUp);

        BindEvent(GetButton((int)Buttons.LevelUpItemButton_1).gameObject, () => OnPressedLevelUpItemButton(1), UIEvent.Pressed);
        BindEvent(GetButton((int)Buttons.LevelUpItemButton_2).gameObject, () => OnPressedLevelUpItemButton(2), UIEvent.Pressed);
        BindEvent(GetButton((int)Buttons.LevelUpItemButton_3).gameObject, () => OnPressedLevelUpItemButton(3), UIEvent.Pressed);
        BindEvent(GetButton((int)Buttons.LevelUpItemButton_4).gameObject, () => OnPressedLevelUpItemButton(4), UIEvent.Pressed);

        GetText((int)Texts.LevelUpItemQuantity_1).text = $"x{Managers.AccountData.inventory[LevelUpItem1Id]}";
        GetText((int)Texts.LevelUpItemQuantity_2).text = $"x{Managers.AccountData.inventory[LevelUpItem2Id]}";
        GetText((int)Texts.LevelUpItemQuantity_3).text = $"x{Managers.AccountData.inventory[LevelUpItem3Id]}";
        GetText((int)Texts.LevelUpItemQuantity_4).text = $"x{Managers.AccountData.inventory[LevelUpItem4Id]}";

        GetText((int)Texts.LevelUpGoldText).text = "0 G";

        GetText((int)Texts.LevelBefore).text = $"Lv.{character.Growth.level}";
        GetText((int)Texts.HpBefore).text = $"{character.hp}";
        GetText((int)Texts.AtkBefore).text = $"{character.atk}";
        GetText((int)Texts.DefBefore).text = $"{character.def}";

        GetText((int)Texts.LevelAfter).gameObject.SetActive(false);
        GetText((int)Texts.HpAfter).gameObject.SetActive(false);
        GetText((int)Texts.AtkAfter).gameObject.SetActive(false);
        GetText((int)Texts.DefAfter).gameObject.SetActive(false);

        GetButton((int)Buttons.LevelUpTab).onClick.AddListener(OnClickLevelUpTab);
        GetButton((int)Buttons.AwakeningTab).onClick.AddListener(OnClickAwakeningTab);
        GetObject((int)GameObjects.Awakening).SetActive(false);
        GetObject((int)GameObjects.AwakeningLimitText).gameObject.SetActive(false);

        GetButton((int)Buttons.CloseButton).onClick.AddListener(OnClickLevelUpUICloseButton);
        GetButton((int)Buttons.LevelUpItemButton_1).onClick.AddListener(() => OnClickLevelUpItemButton(1));
        GetButton((int)Buttons.LevelUpItemButton_2).onClick.AddListener(() => OnClickLevelUpItemButton(2));
        GetButton((int)Buttons.LevelUpItemButton_3).onClick.AddListener(() => OnClickLevelUpItemButton(3));
        GetButton((int)Buttons.LevelUpItemButton_4).onClick.AddListener(() => OnClickLevelUpItemButton(4));

        GetButton((int)Buttons.ItemReduceButton_1).onClick.AddListener(() => OnClickLevelUpItemButton(1, false));
        GetButton((int)Buttons.ItemReduceButton_2).onClick.AddListener(() => OnClickLevelUpItemButton(2, false));
        GetButton((int)Buttons.ItemReduceButton_3).onClick.AddListener(() => OnClickLevelUpItemButton(3, false));
        GetButton((int)Buttons.ItemReduceButton_4).onClick.AddListener(() => OnClickLevelUpItemButton(4, false));

        GetImage((int)Images.LevelUpBarFrontImage).fillAmount = (float)character.Growth.curExp / character.Growth.maxExp;
        GetText((int)Texts.ExpText).text = $"{character.Growth.curExp}/{character.Growth.maxExp}";
        LevelUpCalc();
        LevelUpButtonActiveFalse();

        AwakeningCalc();
    }

    private void OnClickLevelUpTab()
    {
        GetObject((int)GameObjects.LevelUp).SetActive(true);
        GetObject((int)GameObjects.Awakening).SetActive(false);
    }
    private void OnClickAwakeningTab()
    {
        GetObject((int)GameObjects.LevelUp).SetActive(false);
        GetObject((int)GameObjects.Awakening).SetActive(true);
    }

    private void OnClickLevelUpUICloseButton()
    {
        Managers.UI.CloseUI(this);
    }
    // 아이템 1회 클릭
    private void OnClickLevelUpItemButton(int itemNum, bool up = true)
    {
        // 이미 맥스레벨
        if (character.Growth.level >= character.Growth.GetMaxLevel() && up)
        {
            return;
        }
        // 아이템 먹일 시 맥스레벨
        if (character.Growth.CalcExp(totalExp)[0] >= character.Growth.GetMaxLevel() && up)
        {
            return;
        }
        // 아이템 사용 불가
        if (canUseLevelUpItem == false && up)
        {
            return;
        }

        // 개수가 충분하다면 아이템 1개 사용 체크
        int curNum;
        switch (itemNum)
        {
            case 1:
                curNum = int.Parse(GetText((int)Texts.LevelUpItemSelectNumber1).text);

                if (up)
                {
                    if (curNum >= Managers.AccountData.inventory[LevelUpItem1Id])
                    {
                        break;
                    }

                    curNum++;
                }
                else
                {
                    curNum--;
                    if(curNum < 0)
                    {
                        curNum = 0;
                    }
                }

                GetText((int)Texts.LevelUpItemSelectNumber1).text = curNum.ToString();
                break;
            case 2:
                curNum = int.Parse(GetText((int)Texts.LevelUpItemSelectNumber2).text);

                if (up)
                {
                    if (curNum >= Managers.AccountData.inventory[LevelUpItem2Id])
                    {
                        break;
                    }

                    curNum++;
                }
                else
                {
                    curNum--;
                    if (curNum < 0)
                    {
                        curNum = 0;
                    }
                }

                GetText((int)Texts.LevelUpItemSelectNumber2).text = curNum.ToString();
                break;
            case 3:
                curNum = int.Parse(GetText((int)Texts.LevelUpItemSelectNumber3).text);

                if (up)
                {
                    if (curNum >= Managers.AccountData.inventory[LevelUpItem3Id])
                    {
                        break;
                    }

                    curNum++;
                }
                else
                {
                    curNum--;
                    if (curNum < 0)
                    {
                        curNum = 0;
                    }
                }

                GetText((int)Texts.LevelUpItemSelectNumber3).text = curNum.ToString();
                break;
            case 4:
                curNum = int.Parse(GetText((int)Texts.LevelUpItemSelectNumber4).text);

                if (up)
                {
                    if (curNum >= Managers.AccountData.inventory[LevelUpItem4Id])
                    {
                        break;
                    }

                    curNum++;
                }
                else
                {
                    curNum--;
                    if (curNum < 0)
                    {
                        curNum = 0;
                    }
                }

                GetText((int)Texts.LevelUpItemSelectNumber4).text = curNum.ToString();
                break;
        }
        // 아이템 먹이고 난 뒤 스탯 계산하여 UI 업데이트
        LevelUpCalc();
    }
    // 꾹눌렀다가 땔 때 초기화
    private void OnPointerUpLevelUpItemButton()
    {
        pressTimer = 0;
        isPressed = false;
    }
    // 아이템 꾹 누르기
    private void OnPressedLevelUpItemButton(int itemNum)
    {
        // 이미 맥스레벨
        if (character.Growth.level >= character.Growth.GetMaxLevel())
        {
            return;
        }
        // 아이템 먹일 때 맥스레벨
        if (character.Growth.CalcExp(totalExp)[0] >= character.Growth.GetMaxLevel())
        {
            return;
        }

        pressTimer += Time.deltaTime;

        if (pressTimer >= pressInterval && isPressed == false)
        {
            pressTimer = 0f;
            isPressed = true;

            switch (itemNum)
            {
                case 1:
                    StartCoroutine(LevelUpItem(1));
                    break;
                case 2:
                    StartCoroutine(LevelUpItem(2));
                    break;
                case 3:
                    StartCoroutine(LevelUpItem(3));
                    break;
                case 4:
                    StartCoroutine(LevelUpItem(4));
                    break;
            }
        }
    }

    private void AwakeningCalc()
    {
        AwakeningButtonActiveTrue();
        int nowStar =  character.Growth.star;
        int nowLimit = character.Growth.limitBreak;
        int maxLevel = character.Growth.GetMaxLevel();

        // 풀돌일 때, 텍스트 출력
        if (nowStar+nowLimit == 9)
        {
            GetObject((int)GameObjects.Awakening).SetActive(false);
            GetObject((int)GameObjects.AwakeningLimitText).gameObject.SetActive(true);
        }

        GetText((int)Texts.MaxLevelBefore).text = maxLevel.ToString();
        GetText((int)Texts.MaxLevelAfter).text = (nowStar == 5 ? maxLevel+5 : maxLevel+10).ToString();

        // TODO: 성급별 변경점 
        // GetText((int)Texts.AwakeningChangeText).text = "70레벨에 특성 개방";

        // TODO: 성급별 별, 한계돌파 이미지 변경

        Utility.Id2SO<ItemSO>(character.SO.id, (result) =>
        {
            GetImage((int)Images.AwakeningPieceImage).sprite = (result as ItemSO).icon;
        });
        int[] costs = GetCost(nowStar+nowLimit);

        GetText((int)Texts.AwakeningPieceQuantity).text = $"{Managers.AccountData.GetItemQuantity(character.SO.id)} / {costs[0]}";
        GetText((int)Texts.AwakeningGoldText).text = costs[1].ToString();

        if (Managers.AccountData.GetItemQuantity(character.SO.id) < costs[0] || Managers.AccountData.playerData.Gold < costs[1])
        {
            AwakeningButtonActiveFalse();
        }
    }

    private void OnClickAwakeningButton()
    {
        int[] costs = GetCost(character.Growth.star + character.Growth.limitBreak);
        Managers.AccountData.ConsumeItems(character.SO.id, costs[0]);
        Managers.AccountData.playerData.ReduceGold(costs[1]);
        Managers.AccountData.characterData[character.SO.id].Growth.Awake();
        AwakeningCalc();
    }

    private int[] GetCost(int Awakening)
    {
        int[] costs = { 0, 0 };
        switch(Awakening)
        {
            case 1:
                costs[0] = 30;
                costs[1] = 10000;
                break;
            case 2:
                costs[0] = 80;
                costs[1] = 40000;
                break;
            case 3:
                costs[0] = 100;
                costs[1] = 200000;
                break;
            case 4:
                costs[0] = 120;
                costs[1] = 1000000;
                break;
            case 5:
                costs[0] = 100;
                costs[1] = 1500000;
                break;
            case 6:
                costs[0] = 100;
                costs[1] = 1500000;
                break;
            case 7:
                costs[0] = 100;
                costs[1] = 1500000;
                break;
            case 8:
                costs[0] = 100;
                costs[1] = 1500000;
                break;
            default:
                costs[0] = 0;
                costs[1] = 0;
                break;
        }
        return costs;
    }

    private void AwakeningButtonActiveFalse()
    {
        GetButton((int)Buttons.AwakeningButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.AwakeningButton).enabled = false;
        Color newColor = GetButton((int)Buttons.AwakeningButton).GetComponent<Image>().color;
        newColor.a = 0.5f;
        GetButton((int)Buttons.AwakeningButton).GetComponent<Image>().color = newColor;
    }
    private void AwakeningButtonActiveTrue()
    {
        GetButton((int)Buttons.AwakeningButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.AwakeningButton).onClick.AddListener(OnClickAwakeningButton);
        GetButton((int)Buttons.AwakeningButton).enabled = true;
        Color newColor = GetButton((int)Buttons.AwakeningButton).GetComponent<Image>().color;
        newColor.a = 1f;
        GetButton((int)Buttons.AwakeningButton).GetComponent<Image>().color = newColor;
    }

    // 0.02초마다 아이템 1개씩 추가
    private IEnumerator LevelUpItem(int itemNum)
    {
        while (true)
        {
            if (canUseLevelUpItem == false)
            {
                yield break;
            }

            if (isPressed == false)
            {
                yield break;
            }

            switch (itemNum)
            {
                case 1:
                    if (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber1).text) >= Managers.AccountData.inventory[LevelUpItem1Id])
                    {
                        yield break;
                    }

                    GetText((int)Texts.LevelUpItemSelectNumber1).text = (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber1).text) + 1).ToString();
                    break;
                case 2:
                    if (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber2).text) >= Managers.AccountData.inventory[LevelUpItem2Id])
                    {
                        yield break;
                    }

                    GetText((int)Texts.LevelUpItemSelectNumber2).text = (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber2).text) + 1).ToString();
                    break;
                case 3:
                    if (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber3).text) >= Managers.AccountData.inventory[LevelUpItem3Id])
                    {
                        yield break;
                    }

                    GetText((int)Texts.LevelUpItemSelectNumber3).text = (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber3).text) + 1).ToString();
                    break;
                case 4:
                    if (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber4).text) >= Managers.AccountData.inventory[LevelUpItem4Id])
                    {
                        yield break;
                    }

                    GetText((int)Texts.LevelUpItemSelectNumber4).text = (int.Parse(GetText((int)Texts.LevelUpItemSelectNumber4).text) + 1).ToString();
                    break;
            }

            LevelUpCalc();
            yield return new WaitForSeconds(0.02f);
        }
    }
    // UI업데이트
    private void LevelUpCalc()
    {
        // 총 경험치량
        totalExp = LevelUpItem1ExpValue * int.Parse(GetText((int)Texts.LevelUpItemSelectNumber1).text) +
            LevelUpItem2ExpValue * int.Parse(GetText((int)Texts.LevelUpItemSelectNumber2).text) +
            LevelUpItem3ExpValue * int.Parse(GetText((int)Texts.LevelUpItemSelectNumber3).text) +
            LevelUpItem4ExpValue * int.Parse(GetText((int)Texts.LevelUpItemSelectNumber4).text);

        // TODO
        // 필요 골드 = 경험치량 * 5
        // 매직넘버 5 피하고 싶음..
        GetText((int)Texts.LevelUpGoldText).text = $"{totalExp * 5} G";

        int[] result = character.Growth.CalcExp(totalExp);

        // 현재 레벨과 경험치를 먹인 레벨이 같을 때
        if (character.Growth.level == result[0])
        {
            GetImage((int)Images.LevelUpBarFrontImage).gameObject.SetActive(true);
            GetImage((int)Images.LevelUpBarChangeImage).fillAmount = (float)result[1] / character.Growth.GetMaxExp(result[0]);
            GetText((int)Texts.ExpText).text = $"{result[1]}/{character.Growth.GetMaxExp(result[0])}";
        }
        // 레벨업을 했을 때
        else
        {
            GetImage((int)Images.LevelUpBarFrontImage).gameObject.SetActive(false);
            GetImage((int)Images.LevelUpBarChangeImage).fillAmount = (float)result[1] / character.Growth.GetMaxExp(result[0]);
            GetText((int)Texts.ExpText).text = $"{result[1]}/{character.Growth.GetMaxExp(result[0])}";
            GetText((int)Texts.LevelAfter).gameObject.SetActive(true);
            GetText((int)Texts.HpAfter).gameObject.SetActive(true);
            GetText((int)Texts.AtkAfter).gameObject.SetActive(true);
            GetText((int)Texts.DefAfter).gameObject.SetActive(true);
        }

        GetText((int)Texts.HpAfter).text = $"{character.hp + (result[0] - character.Growth.level) * character.SO.hpPerLv}";
        GetText((int)Texts.AtkAfter).text = $"{character.atk + (result[0] - character.Growth.level) * character.SO.atkPerLv}";
        GetText((int)Texts.DefAfter).text = $"{character.def + (result[0] - character.Growth.level) * character.SO.defPerLv}";
    
        if (character.Growth.level >= character.Growth.GetMaxLevel())
        {
            LevelUpButtonActiveFalse();
        }

        if (result[0] >= character.Growth.GetMaxLevel())
        {
            result[0] = character.Growth.GetMaxLevel();
            result[1] = 0;
            canUseLevelUpItem = false;
        }
        else
        {
            canUseLevelUpItem = true;
        }
        // 골드 업데이트
        string goldText = GetText((int)Texts.LevelUpGoldText).text;
        string digits = Regex.Match(goldText, @"\d+").Value;

        int goldRequired = int.Parse(digits);

        // 총 경험치량이 0보다 크고 필요 골드가 소지 골드 보다 적다면 활성화
        if (totalExp > 0 && goldRequired <= Managers.AccountData.playerData.Gold)
        {
            LevelUpButtonActiveTrue();
        }
        else
        {
            LevelUpButtonActiveFalse();
        }

        GetText((int)Texts.LevelAfter).text = $"Lv.{result[0]}";
    }

    private void LevelUpButtonActiveFalse()
    {
        GetButton((int)Buttons.LevelUpButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.LevelUpButton).enabled = false;
        Color newColor = GetButton((int)Buttons.LevelUpButton).GetComponent<Image>().color;
        newColor.a = 0.5f;
        GetButton((int)Buttons.LevelUpButton).GetComponent<Image>().color = newColor;
    }

    private void LevelUpButtonActiveTrue()
    {
        GetButton((int)Buttons.LevelUpButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.LevelUpButton).onClick.AddListener(OnClickLevelUpButton);
        GetButton((int)Buttons.LevelUpButton).enabled = true;
        Color newColor = GetButton((int)Buttons.LevelUpButton).GetComponent<Image>().color;
        newColor.a = 1f;
        GetButton((int)Buttons.LevelUpButton).GetComponent<Image>().color = newColor;
    }
    // 레벨업 적용
    private void OnClickLevelUpButton()
    {
        string goldText = GetText((int)Texts.LevelUpGoldText).text;
        string digits = Regex.Match(goldText, @"\d+").Value;

        int gold = int.Parse(digits);
        
        // 골드 부족 시 경고 UI
        // 현재는 부족 시 버튼을 비활성화
        // TODO: 비활성화 or 경고UI??
        if (Managers.AccountData.playerData.ReduceGold(gold) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("골드가 부족합니다.");

            return;
        }

        Managers.AccountData.ConsumeItems(LevelUpItem1Id, int.Parse(GetText((int)Texts.LevelUpItemSelectNumber1).text));
        Managers.AccountData.ConsumeItems(LevelUpItem2Id, int.Parse(GetText((int)Texts.LevelUpItemSelectNumber2).text));
        Managers.AccountData.ConsumeItems(LevelUpItem3Id, int.Parse(GetText((int)Texts.LevelUpItemSelectNumber3).text));
        Managers.AccountData.ConsumeItems(LevelUpItem4Id, int.Parse(GetText((int)Texts.LevelUpItemSelectNumber4).text));

        GetText((int)Texts.LevelUpItemQuantity_1).text = $"x{Managers.AccountData.inventory[LevelUpItem1Id]}";
        GetText((int)Texts.LevelUpItemQuantity_2).text = $"x{Managers.AccountData.inventory[LevelUpItem2Id]}";
        GetText((int)Texts.LevelUpItemQuantity_3).text = $"x{Managers.AccountData.inventory[LevelUpItem3Id]}";
        GetText((int)Texts.LevelUpItemQuantity_4).text = $"x{Managers.AccountData.inventory[LevelUpItem4Id]}";

        Managers.AccountData.characterData[character.SO.id].Growth.LevelUp(totalExp);

        GetText((int)Texts.LevelUpGoldText).text = "0 G";

        GetText((int)Texts.LevelUpItemSelectNumber1).text = "0";
        GetText((int)Texts.LevelUpItemSelectNumber2).text = "0";
        GetText((int)Texts.LevelUpItemSelectNumber3).text = "0";
        GetText((int)Texts.LevelUpItemSelectNumber4).text = "0";

        GetText((int)Texts.LevelBefore).text = $"Lv.{character.Growth.level}";
        GetText((int)Texts.HpBefore).text = $"{character.hp}";
        GetText((int)Texts.AtkBefore).text = $"{character.atk}";
        GetText((int)Texts.DefBefore).text = $"{character.def}";

        GetText((int)Texts.LevelAfter).gameObject.SetActive(false);
        GetText((int)Texts.HpAfter).gameObject.SetActive(false);
        GetText((int)Texts.AtkAfter).gameObject.SetActive(false);
        GetText((int)Texts.DefAfter).gameObject.SetActive(false);

        GetImage((int)Images.LevelUpBarFrontImage).fillAmount = GetImage((int)Images.LevelUpBarChangeImage).fillAmount;
        GetImage((int)Images.LevelUpBarChangeImage).fillAmount = 0;

        LevelUpCalc();
    }

    private void LoadAllItemsExpValue()
    {
        Utility.Id2SO<ItemSO>(LevelUpItem1Id, (result) =>
        {
            if (result != null)
            {
                LevelUpItem1ExpValue = (result as ItemSO).values["exp"];
            }
        });
        Utility.Id2SO<ItemSO>(LevelUpItem2Id, (result) =>
        {
            if (result != null)
            {
                LevelUpItem2ExpValue = (result as ItemSO).values["exp"];
            }
        });
        Utility.Id2SO<ItemSO>(LevelUpItem3Id, (result) =>
        {
            if (result != null)
            {
                LevelUpItem3ExpValue = (result as ItemSO).values["exp"];
            }
        });
        Utility.Id2SO<ItemSO>(LevelUpItem4Id, (result) =>
        {
            if (result != null)
            {
                LevelUpItem4ExpValue = (result as ItemSO).values["exp"];
            }
        });
    }
}
