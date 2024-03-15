using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class BattleUI : UIBase
{
    private enum Buttons
    {
        CancelButton,
        TurnEndButton,
        MoveButton,
        AttackButton,
        SkillConFirmButton,
        //ShowCharacterStatusButton,
        //ShowCharacterSkillButton
    }

    //charactercontroller가 보내주는 캐릭터를 받아옴
    public CharacterBase curSelectedCharacter;
    public CharacterBase curTargetCharacter;

    public event Action OnClickCancelButton;
    public event Action OnClickTurnEndButton;
    public event Action OnClickMoveButton;
    public event Action OnClickAttackButton;
    public event Action OnClickSkillConFirmButton;


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
        // UI 내의 텍스트, 버튼, 이미지, 오브젝트 바인딩
        //BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        //BindImage(typeof(Images));
        //BindObject(typeof(GameObjects));

        // 버튼에 클릭 이벤트 추가
        GetButton((int)Buttons.CancelButton).onClick.AddListener(OnClickCancel);
        GetButton((int)Buttons.TurnEndButton).onClick.AddListener(OnClickTurnEnd);
        GetButton((int)Buttons.MoveButton).onClick.AddListener(OnClickMove);
        GetButton((int)Buttons.AttackButton).onClick.AddListener(OnClickAttack);
        GetButton((int)Buttons.SkillConFirmButton).onClick.AddListener(OnClickSkillConfirm);

        RefreshUI();
    }

    private void RefreshUI()//ui 초기화
    {

    }

    private void OnClickCancel()
    {
        OnClickCancelButton?.Invoke();
    }

    private void OnClickTurnEnd()
    {
        OnClickTurnEndButton?.Invoke();
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
}
