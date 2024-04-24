using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static Constants;

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
        TargetHealthText,
        RoundText,
        TurnText,
        GoalText,
        RemainText,
        WaveText,
        SkillText,
        SkillNameText,
        SkillCostText,
        SkillTargetText,
        ManaText,
        FisrtExtraText,
        SecondExtraText,
        ThirdExtraText,

    }

    private enum GameObjects
    {
        TextPool,
        LeftWalkObject,
        SelectCharacterInfo,
        TargetCharacterInfo,
        RangeObject,
        BufList,
        TargetBufList,
        GameResult,
        Win,
        Lose,
        TurnObject,
        SkillInfo,
        ManaObject,
        SettingObject,
        FirstCheck,
        SecondCheck,
        ThirdCheck,
        FirstStar,
        SecondStar,
        ThirdStar,

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
        NextButton,
        SettingButton,
        //Setting,
        GiveUp,
        Resume,

    }
    private enum Images
    {
        JoyStick,
        CharacterImage,
        HealthBar,
        ShieldBar,
        TargetImage,
        TargetHealthBar,
        TargetShieldBar,
        ResultBackGround,
        ClassIcon,
        SkillImage,

    }

    //charactercontroller가 보내주는 캐릭터를 받아옴
    [HideInInspector] public CharacterBase curSelectedCharacter;
    [HideInInspector] public CharacterBase curTargetCharacter;

    public event Action OnClickCancelButton;
    public event Action OnClickTurnEndButton;
    public event Action OnClickMoveAndAttackButton;
    public event Action OnClickUseSkillButton;
    public event Action OnClickMoveButton;
    public event Action OnClickAttackButton;
    public event Action OnClickSkillConFirmButton;

    [HideInInspector] public VirtualJoyStick joyStick;
    [HideInInspector] public DamageTextPool textPool;

    [SerializeField] private GameObject buf;
    private List<GameObject> bufList;
    private List<GameObject> targetBufList;

    private StageSO stage;

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
        GetButton((int)Buttons.NextButton).onClick.AddListener(OnClickNextButton);
        GetButton((int)Buttons.SettingButton).onClick.AddListener(ShowSettingBox);
        GetButton((int)Buttons.GiveUp).onClick.AddListener(OnGiveUpButton);
        GetButton((int)Buttons.Resume).onClick.AddListener(OnResumeButton);

        //조이스틱 가져오기
        joyStick = GetImage((int)Images.JoyStick).GetComponent<VirtualJoyStick>();

        //텍스트풀 가져오기
        textPool = GetObject((int)GameObjects.TextPool).GetComponent<DamageTextPool>();

        RefreshUI();

        bufList = new List<GameObject> ();
        targetBufList = new List<GameObject> ();

        for(int i = 0; i < 30; i++)
        {
            GameObject obj = Instantiate(buf, GetObject((int)GameObjects.BufList).transform);
            bufList.Add(obj);
            obj.SetActive(false);
        }

        for (int i = 0; i < 8; i++)
        {
            GameObject obj = Instantiate(buf, GetObject((int)GameObjects.TargetBufList).transform);
            targetBufList.Add(obj);
            obj.SetActive(false);
        }

        stage = Managers.GameManager.thisStage;

        GetText((int)Texts.FisrtExtraText).text = stage.GetExtraGoalDetail(0);
        GetText((int)Texts.SecondExtraText).text = stage.GetExtraGoalDetail(1);
        GetText((int)Texts.ThirdExtraText).text = stage.GetExtraGoalDetail(2);
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
        CloseOtherUI();

        CloseResult();
    }

    private void CloseOtherUI()
    {
        GetObject((int)GameObjects.TurnObject).SetActive(false);
        GetObject((int)GameObjects.SettingObject).SetActive(false);
    }

    private void CloseResult()
    {
        GetObject((int)GameObjects.Win).SetActive(false);
        GetObject((int)GameObjects.Lose).SetActive(false);

        GetObject((int)GameObjects.FirstStar).SetActive(false);
        GetObject((int)GameObjects.SecondStar).SetActive(false);
        GetObject((int)GameObjects.ThirdStar).SetActive(false);

        GetButton((int)Buttons.NextButton).gameObject.SetActive(false);

        GetImage((int)Images.ResultBackGround).GetComponent<CanvasRenderer>().SetAlpha(0f);

        GetObject((int)GameObjects.GameResult).SetActive(false);
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

        GetObject((int)GameObjects.SkillInfo).SetActive(false);

        GetObject((int)GameObjects.ManaObject).SetActive(false);
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

    public void ActingSelect(bool canActing)
    {
        GetButton((int)Buttons.MoveAndAttackButton).gameObject.SetActive(canActing);
        GetButton((int)Buttons.UseSkillButton).gameObject.SetActive(canActing);
    }

    public void SetCanUseSkill(bool canSkill)
    {
        GetButton((int)Buttons.SkillConFirmButton).interactable = canSkill;
    }

    public void SetNoManaText(bool haveMana)
    {
        GetText((int)Texts.NoMana).gameObject.SetActive(haveMana);
    }

    public void ShowManaText()
    {
        GetObject((int)GameObjects.ManaObject).SetActive(true);

        GetText((int)Texts.ManaText).text = Managers.GameManager.player.manaCost.ToString() + " / 60";
        GetText((int)Texts.ManaText).color = Color.blue;
    }

    public void SetManaText()
    {
        int skillcost = Managers.GameManager.player.manaCost - curSelectedCharacter.skillCost;

        GetText((int)Texts.ManaText).text =  skillcost.ToString() + " / 60";

        if(skillcost < 0)
        {
            GetText((int)Texts.ManaText).color = Color.red;
        }
        else
        {
            GetText((int)Texts.ManaText).color = Color.blue;
        }
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
        ShowSkillInfo();

        GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
        GetButton((int)Buttons.SkillConFirmButton).gameObject.SetActive(true);
    }

    public void ShowRound(int nowRound)
    {
        GetText((int)Texts.RoundText).text = nowRound.ToString();
    }

    public void ShowWave(int nowWave)
    {
        string waveNumber = stage.waveNumber.ToString();
        string nowWaveNumber = nowWave.ToString();

        if(stage.spawnType == EnemySpawnType.Infinite)
        {
            waveNumber = "??";
            if (stage.spawnByRound)
            {
                nowWaveNumber = "??";
            }
        }

        GetText((int)Texts.WaveText).text = nowWaveNumber + " / " + waveNumber;
    }

    public void ShowTurn(bool myTurn)
    {
        if (myTurn)
        {
            GetText((int)Texts.TurnText).color = Color.blue;
            GetText((int)Texts.TurnText).text = "당신의 턴";
        }
        else
        {
            GetText((int)Texts.TurnText).color = Color.red;
            GetText((int)Texts.TurnText).text = "상대의 턴";
        }

        StartCoroutine(ShowNowTurn());
    }

    private IEnumerator ShowNowTurn()
    {
        GetObject((int)GameObjects.TurnObject).SetActive(true);

        yield return new WaitForSeconds(1f);

        GetObject((int)GameObjects.TurnObject).SetActive(false);
    }

    public void SetGoalText()
    {
        TextMeshProUGUI goal = (TextMeshProUGUI)GetText((int)Texts.GoalText);
        TextMeshProUGUI remain = (TextMeshProUGUI)GetText((int)Texts.RemainText);

        switch (stage.clear)
        {
            case StageClear.ClearAll:
                goal.text = "모든 적을 섬멸해야 합니다.";
                remain.text = "남은 적 수 : " + BattleManager.Instance.GetRemainEnemy().ToString();
                break;
            case StageClear.Assasinate:
                goal.text = "대상을 처치해야 합니다.";
                remain.text = "목표 대상 : ";
                foreach(Character character in stage.GetTargetEnemy())
                {
                    remain.text += character.enemySO.characterName;
                    remain.text += (stage.GetTargetEnemy().Count > 1 ? ", " : "");
                }
                break;
            case StageClear.Run:
                goal.text = "목표 지점까지 도달해야 합니다.";
                remain.text = "";
                break;
            case StageClear.Defence:
                goal.text = stage.defenceRound + " 라운드 동안 살아남아야 합니다.";
                remain.text = "남은 라운드 수 : " + (stage.defenceRound - BattleManager.Instance.nowRound).ToString();
                break;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //Setting Ui

    private void ShowSettingBox()
    {
        if (GetObject((int)GameObjects.SettingObject).activeInHierarchy)
        {
            GetObject((int)GameObjects.SettingObject).SetActive(false);
        }
        else
        {
            SetSettingBox();
        }
    }

    private void SetSettingBox()
    {
        BattleManager.Instance.CheckExtraGoal();

        GetObject((int)GameObjects.SettingObject).SetActive(true);

        GetObject((int)GameObjects.FirstCheck).SetActive(BattleManager.Instance.extraClear[0]);
        if(stage.extraGoal[0].type == ExtraGoal.Empty)
        {
            GetObject((int)GameObjects.FirstCheck).SetActive(false);
        }

        GetObject((int)GameObjects.SecondCheck).SetActive(BattleManager.Instance.extraClear[1]);
        if (stage.extraGoal[1].type == ExtraGoal.Empty)
        {
            GetObject((int)GameObjects.SecondCheck).SetActive(false);
        }

        GetObject((int)GameObjects.ThirdCheck).SetActive(BattleManager.Instance.extraClear[2]);
        if (stage.extraGoal[2].type == ExtraGoal.Empty)
        {
            GetObject((int)GameObjects.ThirdCheck).SetActive(false);
        }
    }

    private void OnGiveUpButton()
    {
        GetObject((int)GameObjects.SettingObject).SetActive(false);
        BattleManager.Instance.GiveUpStage();
    }

    private void OnResumeButton()
    {
        GetObject((int)GameObjects.SettingObject).SetActive(false);
    } 


    //-----------------------------------------------------------------------------------------------------------------------
    //Character Ui

    public void ShowSkillInfo()
    {
        if (curSelectedCharacter == null)
        {
            GetObject((int)GameObjects.SkillInfo).SetActive(false);
            return;
        }

        GetImage((int)Images.SkillImage).sprite = curSelectedCharacter.curCharacterSkill.skillData.icon;

        GetObject((int)GameObjects.SkillInfo).SetActive(true);

        GetText((int)Texts.SkillNameText).text = curSelectedCharacter.curCharacterSkill.skillData.skillName;
        GetText((int)Texts.SkillText).text = curSelectedCharacter.curCharacterSkill.skillData.description;

        GetText((int)Texts.SkillCostText).text = curSelectedCharacter.skillCost.ToString();

        string target = "";
        switch (curSelectedCharacter.curCharacterSkill.skillData.targetType)
        {
            case SkillTargetType.Me:
                target = "이 캐릭터";
                break;
            case SkillTargetType.Enemy:
                target = "적";
                break;
            case SkillTargetType.Ally:
                target = "아군";
                break;
            case SkillTargetType.All:
                target = "전체";
                break;
            case SkillTargetType.AllExceptME:
                target = "이 캐릭터를 제외한 전체";
                break;
        }

        GetText((int)Texts.SkillTargetText).text = target;

    }

    public void ShowSelectCharacterInfo()
    {
        if(curSelectedCharacter == null)
        {
            GetObject((int)GameObjects.SelectCharacterInfo).SetActive(false);
            return;
        }

        GetObject((int)GameObjects.SelectCharacterInfo).SetActive(true);

        GetText((int)Texts.CharacterName).text = curSelectedCharacter.character.SO.characterName;
        GetText((int)Texts.LvText).text = "Lv. " + curSelectedCharacter.character.Growth.level.ToString("#00");

        GetImage((int)Images.CharacterImage).sprite = curSelectedCharacter.character.SO.icon;

        GetText((int)Texts.AtkText).text = curSelectedCharacter.Attack.ToString();
        GetText((int)Texts.DefText).text = curSelectedCharacter.Defend.ToString();
        GetText((int)Texts.MovText).text = curSelectedCharacter.Mov.ToString();

        if(curSelectedCharacter.character.SO.attackMethod == Constants.AttackMethod.Range)
        {
            GetText((int)Texts.TypeText).text = "원거리";

            GetObject((int)GameObjects.RangeObject).SetActive(true);
            GetText((int)Texts.RangeText).text = curSelectedCharacter.character.SO.range.ToString();
        }
        else
        {
            GetText((int)Texts.TypeText).text = "근거리";

            GetObject((int)GameObjects.RangeObject).SetActive(false);
        }

        if (curSelectedCharacter.character.Growth.superiorClass == -1)
        {
            GetImage((int)Images.ClassIcon).sprite = curSelectedCharacter.character.basicClass.icon;
        }
        else
        {
            GetImage((int)Images.ClassIcon).sprite = curSelectedCharacter.character.superiorClass.icon;
        }

        GetText((int)Texts.HealthText).text = curSelectedCharacter.health.CurHealth.ToString() +
            ((curSelectedCharacter.health.GetShield() > 0) ?" + " + curSelectedCharacter.health.GetShield().ToString() : "") +
        " / " + curSelectedCharacter.health.TotalHealth.ToString();

        GetImage((int)Images.HealthBar).fillAmount = curSelectedCharacter.health.HealthRatio;
        GetImage((int)Images.ShieldBar).fillAmount = curSelectedCharacter.health.ShieldRatio;

        for (int i = 0; i < bufList.Count; i++)
        {
            if (curSelectedCharacter.curCharacterBufList.bufList.Count < i + 1)
            {
                bufList[i].GetComponent<BufIcon>().SetBufIcon(null);
                continue;
            }
            bufList[i].GetComponent<BufIcon>().SetBufIcon(curSelectedCharacter.curCharacterBufList.bufList[i]);
        }
    }

    public void ShowTargetInfo()
    {
        if (curTargetCharacter == null)
        {
            GetObject((int)GameObjects.TargetCharacterInfo).SetActive(false);
            return;
        }

        GetObject((int)GameObjects.TargetCharacterInfo).SetActive(true);

        GetText((int)Texts.TargetName).text = curTargetCharacter.character.SO.characterName;
        GetText((int)Texts.TargetLevel).text = "Lv. " + curTargetCharacter.character.Growth.level.ToString("#00");

        GetImage((int)Images.TargetImage).sprite = curTargetCharacter.character.SO.icon;

        GetText((int)Texts.TargetAtkText).text = curTargetCharacter.Attack.ToString();
        GetText((int)Texts.TargetDefText).text = curTargetCharacter.Defend.ToString();

        GetText((int)Texts.TargetHealthText).text = curTargetCharacter.health.CurHealth.ToString() +
            ((curTargetCharacter.health.GetShield() > 0) ? " + " + curTargetCharacter.health.GetShield().ToString() : "");
        GetImage((int)Images.TargetHealthBar).fillAmount = curTargetCharacter.health.HealthRatio;
        GetImage((int)Images.TargetShieldBar).fillAmount = curTargetCharacter.health.ShieldRatio;

        for (int i = 0; i < targetBufList.Count; i++)
        {
            if (curTargetCharacter.curCharacterBufList.bufList.Count < i + 1)
            {
                targetBufList[i].GetComponent<BufIcon>().SetBufIcon(null);
                continue;
            }
            targetBufList[i].GetComponent<BufIcon>().SetBufIcon(curTargetCharacter.curCharacterBufList.bufList[i]);
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //GameResult

    public void ShowWin()
    {
        StartCoroutine(Result(true));
    }

    public void ShowLose()
    {
        StartCoroutine(Result(false));
    }

    private IEnumerator Result(bool win)
    {
        yield return new WaitWhile(() => AnimationController.instance.CheckAnimation() || AnimationController.instance.IsWalkingAnimation());

        yield return new WaitForSeconds(0.5f);

        GetObject((int)GameObjects.GameResult).SetActive(true);

        float time = 0f;
        while(time <= 0.25f)
        {
            GetImage((int)Images.ResultBackGround).GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, time / 0.25f));
            time += Time.deltaTime;

            yield return null;
        }

        WaitForSeconds wait = new WaitForSeconds(0.25f);

        if (win)
        {
            GetObject((int)GameObjects.Win).SetActive(true);

            yield return wait;

            if (BattleManager.Instance.extraClear[0])
            {
                GetObject((int)GameObjects.FirstStar).SetActive(true);
                yield return wait;
            }

            if (BattleManager.Instance.extraClear[1])
            {
                GetObject((int)GameObjects.SecondStar).SetActive(true);
                yield return wait;
            }

            if (BattleManager.Instance.extraClear[2])
            {
                GetObject((int)GameObjects.ThirdStar).SetActive(true);
                yield return wait;
            }

        }
        else
        {
            GetObject((int)GameObjects.Lose).SetActive(true);
        }
        GetButton((int)Buttons.NextButton).gameObject.SetActive(true);

    }

    private void OnClickNextButton()
    {
        Managers.GameManager.player.ResetPlayer();
        SceneManager.LoadScene("MainScene");
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

        obj.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);

        text.gameObject.layer = transform.gameObject.layer;
        obj.transform.localScale = transform.localScale.magnitude > 2f ?  transform.localScale / 2.5f : obj.transform.localScale;

        return text;
    }

    public void ShowDamageText(BattleKeyWords.Damage damage, Transform transform, bool isHeal = false)
    {
        TextMeshPro text = ShowText(transform);

        text.text = "";

        if (damage.isCriticalHit)
        {
            text.text += "치명타!\r\n";
        }

        text.text += damage.damage.ToString();

        if (damage.attributeDamage == 1.5f)
        {
            text.text += " 취약";
        }
        else if (damage.attributeDamage == 0.75f)
        {
            text.text += " 내성";
        }

        if (isHeal)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.white;
        }
    }


    public void ShowCounterText(Transform transform)
    {
        TextMeshPro text = ShowText(transform);

        text.text = "반격";
    }

    public void ShowBlockText(Transform transform)
    {
        TextMeshPro text = ShowText(transform);

        text.text = "가로막힘";
    }
}
