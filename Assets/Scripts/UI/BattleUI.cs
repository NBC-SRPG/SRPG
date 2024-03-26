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
        NoMana
    }

    private enum GameObjects
    {
        TextPool
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
        //ShowCharacterStatusButton,
        //ShowCharacterSkillButton
    }
    private enum Images
    {
        JoyStick
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
        ResetButtons();

        CloseTexts();
    }

    public void CloseTexts()
    {
        GetText((int)Texts.NoMana).gameObject.SetActive(false);
    }

    public void ResetButtons()
    {
        GetButton((int)Buttons.TurnEndButton).gameObject.SetActive(false);
        GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
        GetButton((int)Buttons.MoveAndAttackButton).gameObject.SetActive(false);
        GetButton((int)Buttons.UseSkillButton).gameObject.SetActive(false);
        GetButton((int)Buttons.MoveButton).gameObject.SetActive(false);
        GetButton((int)Buttons.AttackButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SkillConFirmButton).gameObject.SetActive(false);
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
    //데미지 표기

    private TextMeshPro ShowText(Transform transform)
    {
        GameObject obj;
        TextMeshPro text;

        obj = textPool.GetText("BattleText");
        text = obj.GetComponentInChildren<TextMeshPro>();

        obj.gameObject.SetActive(true);

        obj.transform.position = new Vector2(transform.position.x, transform.position.y + 2f);
        text.gameObject.layer = transform.gameObject.layer;

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

        text.text = "Counter";
    }

    public void ShowDefendText(Transform transform)
    {
        TextMeshPro text = ShowText(transform);

        text.text = "Blocked";
    }
}
