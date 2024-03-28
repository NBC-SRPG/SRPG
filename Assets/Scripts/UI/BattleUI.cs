using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BattleUI : UIBase
{
    private enum Texts
    {
        NoMana,
        MaxLeftWalk,
        CurLeftWalk,
        CharacterName,
        LvText,
        HealthText,
        AtkText,
        DefText,
        MovText,
        TypeText,
        RangeText,
        TargetName,
        TargetLevel,
        TargetAtkText,
        TargetDefText,
        TargetHealthText
    }

    private enum GameObjects
    {
        TextPool,
        LeftWalkObject,
        SelectCharacterInfo,
        TargetCharacterInfo,
        RangeObject,

    }

    private enum Buttons
    {
        CancelButton,
        TurnEndButton,
        MoveAndAttackButton,
        UseSkillButton,
        MoveButton,
        AttackButton,
        SkillConFirmButton,
        ClassInfo,

    }
    private enum Images
    {
        JoyStick,
        CharacterImage,
        HealthBar,
        TargetImage,
        TargetHealthBar
    }

    //charactercontroller가 보내주는 캐릭터를 받아옴
    public CharacterBase curSelectedCharacter;
    public CharacterBase curTargetCharacter;

    public event Action OnClickCancelButton;
    public event Action OnClickTurnEndButton;
    public event Action OnClickMoveAndAttackButton;
    public event Action OnClickUseSkillButton;
    public event Action OnClickMoveButton;
    public event Action OnClickAttackButton;
    public event Action OnClickSkillConFirmButton;

    public VirtualJoyStick joyStick;
    public DamageTextPool textPool;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // TODO
        // curTargetCharacter가 null이 아니라면 해당 캐릭터의 정보 보여주기

    }

    public void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        // UI 내의 텍스트, 버튼, 이미지, 오브젝트 바인딩
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        // 버튼에 클릭 이벤트 추가
        GetButton((int)Buttons.CancelButton).onClick.AddListener(OnClickCancel);
        GetButton((int)Buttons.TurnEndButton).onClick.AddListener(OnClickTurnEnd);
        GetButton((int)Buttons.MoveAndAttackButton).onClick.AddListener(OnClickMoveAndAttack);
        GetButton((int)Buttons.UseSkillButton).onClick.AddListener(OnClickUseSkill);
        GetButton((int)Buttons.MoveButton).onClick.AddListener(OnClickMove);
        GetButton((int)Buttons.AttackButton).onClick.AddListener(OnClickAttack);
        GetButton((int)Buttons.SkillConFirmButton).onClick.AddListener(OnClickSkillConfirm);

        //조이스틱 가져오기
        joyStick = GetImage((int)Images.JoyStick).GetComponent<VirtualJoyStick>();

        //텍스트풀 가져오기
        textPool = GetObject((int)GameObjects.TextPool).GetComponent<DamageTextPool>();

        RefreshUI();
    }

    private void OnClickCancel()
    {
        OnClickCancelButton?.Invoke();
    }

    private void OnClickTurnEnd()
    {
        OnClickTurnEndButton?.Invoke();
    }

    private void OnClickMoveAndAttack()
    {
        OnClickMoveAndAttackButton?.Invoke();
    }

    private void OnClickUseSkill()
    {
        OnClickUseSkillButton?.Invoke();
    }

    private void OnClickMove()
    {
        OnClickMoveButton?.Invoke();
    }

    private void OnClickAttack()
    {
        OnClickAttackButton?.Invoke();
    }

    private void OnClickSkillConfirm()
    {
        OnClickSkillConFirmButton?.Invoke();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //UI Active

    private void RefreshUI()//ui 초기화
    {
        ResetUI();
    }

    public void CloseTexts()
    {
        GetText((int)Texts.NoMana).gameObject.SetActive(false);

        GetObject((int)GameObjects.LeftWalkObject).SetActive(false);
    }

    public void ResetUI()
    {
        GetButton((int)Buttons.TurnEndButton).gameObject.SetActive(false);
        GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
        GetButton((int)Buttons.MoveAndAttackButton).gameObject.SetActive(false);
        GetButton((int)Buttons.UseSkillButton).gameObject.SetActive(false);
        GetButton((int)Buttons.MoveButton).gameObject.SetActive(false);
        GetButton((int)Buttons.AttackButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SkillConFirmButton).gameObject.SetActive(false);

        CloseTexts();

        GetObject((int)GameObjects.SelectCharacterInfo).SetActive(false);
        GetObject((int)GameObjects.TargetCharacterInfo).SetActive(false);
    }

    public void ShowAtCharacterSelectPhase()
    {
        GetButton((int)Buttons.TurnEndButton).gameObject.SetActive(true);
    }

    public void ShowAtActingSelectPhase()
    {
        GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
        GetButton((int)Buttons.MoveAndAttackButton).gameObject.SetActive(true);
        GetButton((int)Buttons.UseSkillButton).gameObject.SetActive(true);

        ShowSelectCharacterInfo();
    }

    public void SetCanUseSkill(bool canSkill)
    {
        GetButton((int)Buttons.UseSkillButton).interactable = canSkill;
    }

    public void SetNoManaText(bool haveMana)
    {
        GetText((int)Texts.NoMana).gameObject.SetActive(haveMana);
    }

    public void ShowAtMoveAndAttackPhase()
    {
        GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);

        GetObject((int)GameObjects.LeftWalkObject).SetActive(true);
        GetText((int)Texts.MaxLeftWalk).text = curSelectedCharacter.Mov.ToString();
    }

    public void SetCurLeftWalk(int number)
    {
        if(number <= 0)
        {
            number = 0;
        }

        GetText((int)Texts.CurLeftWalk).text = number.ToString();
    }

    public void ShowMove(bool move)
    {
        GetButton((int)Buttons.MoveButton).gameObject.SetActive(move);
    }

    public void ShowAttack(bool attack)
    {
        GetButton((int)Buttons.AttackButton).gameObject.SetActive(attack);
    }

    public void ShowAtSkillTargetPhase()
    {
        GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
        GetButton((int)Buttons.SkillConFirmButton).gameObject.SetActive(true);
    }

    public void SetCanConfirm(bool isTarget)
    {
        GetButton((int)Buttons.SkillConFirmButton).interactable = isTarget;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //Character Ui

    public void ShowSelectCharacterInfo()
    {
        if(curSelectedCharacter == null)
        {
            GetObject((int)GameObjects.SelectCharacterInfo).SetActive(false);
            return;
        }

        GetObject((int)GameObjects.SelectCharacterInfo).SetActive(true);

        GetText((int)Texts.CharacterName).text = curSelectedCharacter.character.characterData.characterName;
        //GetText((int)Texts.LvText).text = curSelectedCharacter.character.

        GetText((int)Texts.AtkText).text = curSelectedCharacter.Attack.ToString();
        GetText((int)Texts.DefText).text = curSelectedCharacter.Defend.ToString();
        GetText((int)Texts.MovText).text = curSelectedCharacter.Mov.ToString();

        if(curSelectedCharacter.character.CharacterAttackType == Constants.AttackType.Range)
        {
            GetText((int)Texts.TypeText).text = "원거리";

            GetObject((int)GameObjects.RangeObject).SetActive(true);
            GetText((int)Texts.RangeText).text = curSelectedCharacter.character.characterData.atk_range.ToString();
        }
        else
        {
            GetText((int)Texts.TypeText).text = "근거리";

            GetObject((int)GameObjects.RangeObject).SetActive(false);
        }

        GetText((int)Texts.HealthText).text = curSelectedCharacter.health.CurHealth.ToString() + " / " + curSelectedCharacter.health.MaxHealth.ToString();
        GetImage((int)Images.HealthBar).fillAmount = curSelectedCharacter.health.HealthRatio;
    }

    public void ShowTargetInfo()
    {
        if (curTargetCharacter == null)
        {
            GetObject((int)GameObjects.TargetCharacterInfo).SetActive(false);
            return;
        }

        GetObject((int)GameObjects.TargetCharacterInfo).SetActive(true);

        GetText((int)Texts.TargetName).text = curTargetCharacter.character.characterData.characterName;
        //GetText((int)Texts.TargetLevel).text = 
        GetText((int)Texts.TargetAtkText).text = curTargetCharacter.Attack.ToString();
        GetText((int)Texts.TargetDefText).text = curTargetCharacter.Defend.ToString();

        GetText((int)Texts.TargetHealthText).text = curTargetCharacter.health.CurHealth.ToString();
        GetImage((int)Images.TargetHealthBar).fillAmount = curTargetCharacter.health.HealthRatio;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //데미지 표기

    private TextMeshPro ShowText(Transform transform)
    {
        GameObject obj;
        TextMeshPro text;

        obj = textPool.GetText("BattleText");
        text = obj.GetComponentInChildren<TextMeshPro>();

        obj.gameObject.SetActive(true);

        obj.transform.position = new Vector2(transform.position.x, transform.position.y + 5f);
        text.gameObject.layer = transform.gameObject.layer;
        obj.transform.localScale = transform.localScale.magnitude > 2f ?  transform.localScale / 2.5f : obj.transform.localScale;

        return text;
    }

    public void ShowDamageText(int damage, Transform transform, bool isHeal = false)
    {
        TextMeshPro text = ShowText(transform);

        text.text = damage.ToString();
        if (isHeal)
        {
            text.color = Color.green;
        }
    }


    public void ShowCounterText(Transform transform)
    {
        TextMeshPro text = ShowText(transform);

        text.text = "반격";
    }

    public void ShowDefendText(Transform transform)
    {
        TextMeshPro text = ShowText(transform);

        text.text = "가로막힘";
    }
}
