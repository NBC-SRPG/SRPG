using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    private enum PlayerPhase
    {
        Wait,
        Idle,
        CharacterSetting,
        CharacterSelect,
        ActingSelect,
        MoveandAttack,
        SkillTargetSelect
    }

    [SerializeField] private CharacterBase chaPrefabs;
    [SerializeField] private TargetTiles targetTiles;

    public GamePlayer player;

    private RangeFinder rangeFinder;
    private PathFinder pathFinder;

    [SerializeField] private List<OverlayTile> movePath = new List<OverlayTile>();//이동 경로 타일
    private List<OverlayTile> attackRangeTiles = new List<OverlayTile>();//공격 가능한 타일
    private List<OverlayTile> moveRangeTiles = new List<OverlayTile>();//이동 가능한 타일
    public List<OverlayTile> surroundPath = new List<OverlayTile>();//클릭 가능한 타일
    private List<OverlayTile> skillScale = new List<OverlayTile>();//스킬 범위 타일

    private List<OverlayTile> targetAttackRange = new List<OverlayTile>();
    private List<OverlayTile> prevAttackRange = new List<OverlayTile>();

    private List<CharacterBase> skillTargets = new List<CharacterBase>();

    private List<CharacterBase> characterList = new List<CharacterBase>();

    [SerializeField] private bool nowPlayerTurn;
    private bool canClick;//false일 때 터치 안되게

    private PlayerPhase phase;
    public CharacterBase curSelectedCharacter;
    public CharacterBase curTargetCharacter;

    private BattleUI Ui;

    private void Awake()
    {
        rangeFinder = new RangeFinder();
        pathFinder = new PathFinder();

        phase = PlayerPhase.CharacterSetting;

        targetTiles = Instantiate(targetTiles, transform);

    }

    private void Start()
    {
        canClick = false;
        nowPlayerTurn = false;

        player = Managers.GameManager.player;

        if (!BattleManager.Instance.players.Contains(player))
        {
            BattleManager.Instance.players.Add(player);
        }
        BattleManager.Instance.charactersAsTeam.Add(player.playerId, new List<CharacterBase>());

        BattleManager.Instance.TurnStart += GetPlayerTurn;

        foreach(Character charac in Managers.GameManager.player.party)
        {
            if (charac == null)
            {
                continue;
            }

            CharacterBase character = Instantiate(chaPrefabs, transform);
            character.InitCharacter(charac, player);

            characterList.Add(character);
        }

        Ui = Managers.UI.FindUI<BattleUI>();

        BattleManager.Instance.Lose += EndGame;
    }

    private void Update()
    {
        if (curSelectedCharacter && curSelectedCharacter.isDead)
        {
            SelectCurCharacter(null);
        }

        if(curTargetCharacter && curTargetCharacter.isDead)
        {
            SelectTargetCharacter(null);
        }

        switch (phase)
        {
            case PlayerPhase.CharacterSetting:
                CharacterSet();
                break;
            case PlayerPhase.Idle:
                UpdateIdle();
                break;
            case PlayerPhase.CharacterSelect:
                SelectCharacterPhase();
                break;
            case PlayerPhase.ActingSelect:
                ActingSelectPhase();
                break;
            case PlayerPhase.MoveandAttack:
                CharacterMoveandAttackPhase();
                break;
            case PlayerPhase.SkillTargetSelect:
                SkillTargetSelectPhase();
                break;
        }

        targetTiles.ShowSelectedTile(curSelectedCharacter);
        targetTiles.ShowTargetTile(curTargetCharacter);
    }

    private void GetPlayerTurn()
    {
        if (BattleManager.Instance.nowPlayer == player)
        {
            canClick = true;
            nowPlayerTurn = true;

            player.OnstartTurn();

            player.GainMana(10);

            //-----------------------------------------

            Ui.OnClickTurnEndButton += TurnEnd;

            Ui.OnClickCancelButton += OnClickCancelButton;

            Ui.OnClickMoveAndAttackButton += OnClickMoveAndAttack;
            Ui.OnClickUseSkillButton += OnClickUseSkill;

            Ui.OnClickAttackButton += AttackTarget;
            Ui.OnClickMoveButton += MoveCharacter;

            Ui.OnClickSkillConFirmButton += UseSkill;
            //버튼 연결은 UiManager를 통해 BattleUI에 이벤트에 연결하는 식으로 진행
            //----------------------------------------
        }

        Ui.ShowTurn(BattleManager.Instance.nowPlayer == player);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //CharacterSettingPhase 관련
    private void CharacterSet()
    {
        InitiateCharacter();
    }

    public void InitiateCharacter()//캐릭터 스폰위치에 캐릭터 생성
    {
        BattleManager.Instance.SpawnCharacters(characterList, player);

        ChangePhase(PlayerPhase.Idle);

        player.manaCost = 0;

        player.isReady = true;
        BattleManager.Instance.GetReady();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //페이즈 변환 및 타일 관련 함수

    private void ChangePhase(PlayerPhase newPhase)// 페이즈 변환
    {
        phase = newPhase;

        ClearAllTile();

        SelectTargetCharacter(null);

        skillTargets.Clear();

        Ui.ResetUI();
        Ui.ShowManaText();

        ShowPrevAttackRange(null);
        GetTargetAttackRange(null);

        if (phase != PlayerPhase.Idle)
        {
            CameraController.instance.ResetCamera();
        }

        CameraController.instance.isSelected = false;

        switch (phase)
        {
            case PlayerPhase.Idle:
                SelectCurCharacter(null);
                nowPlayerTurn = false;
                canClick = false;
                break;
            case PlayerPhase.CharacterSelect:
                SelectCurCharacter(null);

                Ui.ShowAtCharacterSelectPhase();
                break;
            case PlayerPhase.ActingSelect:
                Ui.ShowAtActingSelectPhase();
                break;
            case PlayerPhase.MoveandAttack:
                movePath.Add(curSelectedCharacter.curStandingTile);

                GetMoveAndAttackTiles();
                Ui.ShowAtMoveAndAttackPhase();
                CameraController.instance.SetCameraOnTile(curSelectedCharacter.curStandingTile);
                CameraController.instance.isSelected = true;
                break;
            case PlayerPhase.SkillTargetSelect:
                Ui.ShowAtSkillTargetPhase();
                Ui.SetManaText();

                CameraController.instance.isSelected = true;
                break;
        }

    }

    //타일 지우기
    private void ClearTile(List<OverlayTile> tileList)
    {
        foreach (OverlayTile tile in tileList)
        {
            tile.ResetTile();
            tile.HideTile();
        }

        tileList.Clear();
    }

    private void ResetTileOnMove(List<OverlayTile> tileList)
    {
        foreach (OverlayTile tile in tileList)
        {
            tile.ResetTileOnMove();
            if (!moveRangeTiles.Contains(tile))
            {
                tile.HideTile();
            }
        }

        tileList.Clear();

        ShowPrevAttackRange(null);
    }

    private void ResetTileOnSkill(List<OverlayTile> tileList)
    {
        foreach (OverlayTile tile in tileList)
        {
            tile.HideScale();
        }

        tileList.Clear();
    }

    private void ResetOnTarget(List<OverlayTile> tileList)
    { 
        foreach(OverlayTile tile in tileList)
        {
            tile.HideTargetAttack();
        }

        tileList.Clear();
    }

    //모든 타일 지우기
    private void ClearAllTile()
    {
        ClearTile(movePath);
        ClearTile(moveRangeTiles);
        ClearTile(attackRangeTiles);
        ClearTile(surroundPath);
        ClearTile(skillScale);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //CharacterSelectPhase 관련

    // 캐릭터 선택 페이즈
    private void SelectCharacterPhase()
    {
        RaycastHit2D hit = GetTouchOnce();

        if (hit)
        {
            OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

            if (curTile.curStandingCharater != null)
            {
                if (curTile.curStandingCharater.playerId == player.playerId && curTile.curStandingCharater != curSelectedCharacter)// 해당 캐릭터가 내 캐릭터일 때
                {
                    SelectCurCharacter(curTile.curStandingCharater);
                    if (curSelectedCharacter.canActing)// 해당 캐릭터가 아직 행동하지 않았을 때
                    {
                        ChangePhase(PlayerPhase.ActingSelect);
                    }
                }
                
                if(curTile.curStandingCharater.playerId != player.playerId)
                {
                    SelectTargetCharacter(curTile.curStandingCharater);
                }
            }
            else
            {
                SelectTargetCharacter(null);
            }
        }
    }

    private void SelectCurCharacter(CharacterBase character)
    {
        if ((character == null && curSelectedCharacter != null) || (character != curSelectedCharacter && curSelectedCharacter != null))
        {
            CameraController.instance.RemoveGroup(curSelectedCharacter);
        }

        curSelectedCharacter = character;
        //이후 ui에 캐릭터 정보를 보내줌
        Ui.curSelectedCharacter = curSelectedCharacter;
        Ui.ShowSelectCharacterInfo();

        CameraController.instance.AddGroup(curSelectedCharacter);
    }

    private void SelectTargetCharacter(CharacterBase character)
    {
        if((character == null && curTargetCharacter != null) || (character != curTargetCharacter && curTargetCharacter != null))
        {
            CameraController.instance.RemoveGroup(curTargetCharacter);
        }

        curTargetCharacter = character;
        //이후 ui에 캐릭터 정보를 보내줌
        Ui.curTargetCharacter = curTargetCharacter;
        Ui.ShowTargetInfo();

        CameraController.instance.AddGroup(curTargetCharacter);

        GetTargetAttackRange(character);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //ActingSelectPhase 관련

    private void ActingSelectPhase()
    {
        if (curSelectedCharacter == null)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        Ui.ActingSelect(curSelectedCharacter.canActing);

        RaycastHit2D hit = GetTouchOnce();

        if (hit)
        {
            OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

            if (curTile.curStandingCharater != null)
            {
                if (curTile.curStandingCharater.playerId == player.playerId && curTile.curStandingCharater != curSelectedCharacter)// 해당 캐릭터가 내 캐릭터일 때
                {
                    SelectCurCharacter(curTile.curStandingCharater);
                }
                
                if(curTile.curStandingCharater.playerId != player.playerId)
                {
                    SelectTargetCharacter(curTile.curStandingCharater);
                }
            }
            else
            {
                SelectTargetCharacter(null);
            }
        }

        Ui.SetDidUseSkill(curSelectedCharacter.canSkill);
    }

    private void OnClickMoveAndAttack()
    {
        if(curSelectedCharacter != null)
        {
            ChangePhase(PlayerPhase.MoveandAttack);
        }
    }

    private void OnClickUseSkill()
    {
        if(curSelectedCharacter != null)
        {
            ChangePhase(PlayerPhase.SkillTargetSelect);
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //CharacterMoveandAttackPhase 관련

    private void GetMoveAndAttackTiles()
    {
        if (!curSelectedCharacter.didWalk)// 아직 한번도 이동하지 않았다면, 이동 범위 가져옴
        {
            GetMoveRangeTile();
        }
        if (curSelectedCharacter.character.SO.attackMethod == Constants.AttackMethod.Range && !curSelectedCharacter.didAttack)// 해당 캐릭터가 원거리형 캐릭터고 공격하지 않았을 때 
        {
            GetAttackRangeTile(curSelectedCharacter.character.SO.range);// 공격 가능 범위 가져옴
        }
    }

    private void CharacterMoveandAttackPhase()// 캐릭터 이동 및 일반 공격 페이즈
    {
        if (curSelectedCharacter == null)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        if (AnimationController.instance.CheckAnimation())
        {
            return;
        }

        if (!curSelectedCharacter.isWalking)// 캐릭터가 이동중이 아닐 때
        {
            GetPathTile();

            RaycastHit2D hit = GetTouchOnce();

            if (hit)
            {
                OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                if(curTile.curStandingCharater != null && curTile.curStandingCharater != curSelectedCharacter && !movePath.Contains(curTile))
                {
                    if (movePath.Count > 1)
                    {
                        ResetTileOnMove(movePath);
                        ResetTileOnMove(surroundPath);
                        movePath.Add(curSelectedCharacter.curStandingTile);
                    }

                    SelectTargetCharacter(curTile.curStandingCharater);
                    CameraController.instance.SetCameraOnSelected();
                }
                else
                {
                    //SelectTargetCharacter(null); // 모바일에서 실행 시 버튼이 제대로 안눌리는 버그가 있음
                }
            }

        }

        //원거리 캐릭터가 공격 가능한 범위 내에 있는 적 터치 시
        Ui.ShowMove(movePath.Count > 1 && curTargetCharacter == null && !curSelectedCharacter.isWalking);
        Ui.ShowAttack(movePath.Count <= 1 && curTargetCharacter != null && attackRangeTiles.Contains(curTargetCharacter.curStandingTile)
            && curTargetCharacter.CheckEnemy(curSelectedCharacter) && !curSelectedCharacter.isAttacking);
    }

    private void AttackTarget()// 캐릭터 공격
    {
        if (curTargetCharacter != null)
        {
            canClick = false;
            AnimationController.instance.onAnimationEnd += EndAttack;

            curSelectedCharacter.SetAttackTarget(curTargetCharacter);
        }
    }

    private void MoveCharacter()// 캐릭터 이동
    {
        if (movePath.Last().CheckCanMove())
        {
            canClick = false;
            ResetTileOnMove(surroundPath);
            Ui.ResetUI();

            AnimationController.instance.onAnimationEnd += EndMove;

            curSelectedCharacter.MoveCharacter();
        }
    }

    private void EndMove()// 캐릭터의 이동이 끝났을 시
    {
        AnimationController.instance.onAnimationEnd -= EndMove;

        canClick = true;

        if (curSelectedCharacter == null)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        ChangePhase(PlayerPhase.MoveandAttack);

        if (!curSelectedCharacter.canActing)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }
    }

    private void EndAttack()// 캐릭터의 공격이 끝났을 시
    {
        AnimationController.instance.onAnimationEnd -= EndAttack;

        canClick = true;

        if (curSelectedCharacter == null)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        ChangePhase(PlayerPhase.MoveandAttack);

        if (!curSelectedCharacter.canActing)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //SkillTargetSeletPhase 관련

    private void SkillTargetSelectPhase()// 스킬 범위 선택
    {
        if (curSelectedCharacter == null)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        if (AnimationController.instance.CheckAnimation())
        {
            return;
        }

        if (!curSelectedCharacter.canActing)
        {
            ChangePhase(PlayerPhase.MoveandAttack);
            return;
        }

        GetAttackRangeTile(curSelectedCharacter.curCharacterSkill.skillData.skillRange);

        if (curSelectedCharacter.curCharacterSkill.skillData.scaleType == Constants.SkillScaleType.Self)// 자신에게 사용하는 스킬의 경우
        {
            GetSkillScaleTile(curSelectedCharacter.curStandingTile.grid2DLocation, 0);
            CameraController.instance.SetCameraOnTile(curSelectedCharacter.curStandingTile);
        }
        else
        {
            RaycastHit2D hit;

            if (curSelectedCharacter.curCharacterSkill.skillData.scaleType != Constants.SkillScaleType.None)// 단일 지정 스킬이 아닐 때
            {
                if(skillScale.Count == 0)
                {
                    GetSkillScaleTile(curSelectedCharacter.curStandingTile.grid2DLocation, curSelectedCharacter.curCharacterSkill.skillData.skillScale);// 처음 한번은 캐릭터 기준으로 가져옴
                    CameraController.instance.SetCameraOnTile(curSelectedCharacter.curStandingTile);
                }


                hit = GetTouching();

                if (hit)
                {
                    OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                    if (attackRangeTiles.Contains(curTile))
                    {

                        ResetTileOnSkill(skillScale);

                        GetSkillScaleTile(curTile.grid2DLocation, curSelectedCharacter.curCharacterSkill.skillData.skillScale);
                        CameraController.instance.SetCameraOnTile(curTile);
                    }
                }
            }
            else// 단일 지정 스킬일 때
            {
                hit = GetTouchOnce();

                if (hit)
                {
                    OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                    //캐릭터가 공격 가능한 범위 내에 있는 캐릭터 터치 시
                    if (attackRangeTiles.Contains(curTile) && curTile.curStandingCharater != null)
                    {
                        if (curSelectedCharacter.curCharacterSkill.skillData.targetType == Constants.SkillTargetType.Enemy && curTile.curStandingCharater.CheckEnemy(curSelectedCharacter))
                        {
                            SelectTargetCharacter(curTile.curStandingCharater);
                            GetSkillScaleTile(curTargetCharacter.curStandingTile.grid2DLocation, 0);
                            CameraController.instance.SetCameraOnSelected();
                        }
                        else if (curSelectedCharacter.curCharacterSkill.skillData.targetType == Constants.SkillTargetType.Ally && !curTile.curStandingCharater.CheckEnemy(curSelectedCharacter))
                        {
                            SelectTargetCharacter(curTile.curStandingCharater);
                            GetSkillScaleTile(curTargetCharacter.curStandingTile.grid2DLocation, 0);
                            CameraController.instance.SetCameraOnSelected();
                        }
                    }
                    else
                    {
                        SelectTargetCharacter(null);
                        ResetTileOnSkill(skillScale);
                    }
                }

            }
        }

        Ui.SetCanUseSkill(curSelectedCharacter.canSkill && player.manaCost - curSelectedCharacter?.skillCost >= 0 && skillScale.Count > 0);
        Ui.SetNoManaText(player.manaCost < curSelectedCharacter?.skillCost);
    }

    private void UseSkill()// 스킬 사용
    {
        if (curSelectedCharacter.canSkill)
        {
            canClick = false;

            player.UseMana(curSelectedCharacter.skillCost);
            curSelectedCharacter.historyCurrentRound.useCostCount += curSelectedCharacter.skillCost;

            AnimationController.instance.onAnimationEnd += EndSkill;

            curSelectedCharacter.targets = skillTargets;
            curSelectedCharacter.UseSkill();
        }
    }

    private void EndSkill()// 캐릭터의 공격이 끝났을 시
    {
        AnimationController.instance.onAnimationEnd -= EndSkill;

        canClick = true;

        if (curSelectedCharacter == null)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        ChangePhase(PlayerPhase.ActingSelect);

        if (!curSelectedCharacter.canActing && curSelectedCharacter)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }
    }


    //-----------------------------------------------------------------------------------------------------------------------
    //거리 탐색 함수들

    //이동 가능 위치 탐색
    private void GetPathTile()
    {
        if (movePath.Count <= curSelectedCharacter.leftWalkRange && !curSelectedCharacter.didWalk)// 선택한 캐릭터의 걸음 횟수가 남아있다면 
        {
            surroundPath = pathFinder.MakePath(movePath[movePath.Count - 1], movePath);

            foreach (OverlayTile tile in surroundPath)
            {
                tile.ShowAsMove();
            }

        }

        Ui.SetCurLeftWalk(curSelectedCharacter.leftWalkRange - movePath.Count + 1);

        RaycastHit2D hit = GetTouching();

        if (hit)
        {
            OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

            if (surroundPath.Contains(curTile) && curTile.canClick)//이동할 위치 선택 시
            {
                SelectTargetCharacter(null);
                movePath.Add(curTile);
                ResetTileOnMove(surroundPath);

                CameraController.instance.SetCameraOnTile(movePath.Last());
            }

            if (movePath.Contains(curTile) && movePath.Count > 0 && movePath[movePath.Count - 1] != curTile)//이미 선택된 타일 터치 시
            {
                int n = movePath.IndexOf(curTile);
                List<OverlayTile> temp = movePath.GetRange(0, n + 1);

                ResetTileOnMove(movePath);
                movePath = temp;

                ResetTileOnMove(surroundPath);

                CameraController.instance.SetCameraOnTile(movePath.Last());
            }

            if(curSelectedCharacter.character.SO.attackMethod == Constants.AttackMethod.Range && !curSelectedCharacter.didAttack)
            {
                ShowPrevAttackRange(movePath.Last());
            }
        }

        curSelectedCharacter.movePath = movePath;

        foreach (OverlayTile tile in movePath)//선택된 이동 위치 표시
        {
            tile.ShowAsScale();
        }
    }


    private void GetMoveRangeTile()// 이동 가능 거리 가져옴
    {
        if(moveRangeTiles.Count > 0)
        {
            return;
        }

        moveRangeTiles = rangeFinder.GetTilesInRange(curSelectedCharacter.curStandingTile.grid2DLocation, curSelectedCharacter.Mov, true);

        foreach (OverlayTile tile in moveRangeTiles)
        {
            tile.ShowTile();
        }

    }

    private void GetAttackRangeTile(int range)// 공격 가능 거리 가져옴
    {
        if (attackRangeTiles.Count > 0)
        {
            return;
        }

        attackRangeTiles = rangeFinder.GetTilesInRange(curSelectedCharacter.curStandingTile.grid2DLocation, range, false);

        foreach (OverlayTile tile in attackRangeTiles)
        {
            tile.ShowAsAttack();
        }

    }

    private void GetSkillScaleTile(Vector2Int location, int scale)// 스킬 범위 가져오기
    {
        if(curSelectedCharacter == null)
        {
            return;
        }

        skillScale = curSelectedCharacter.curCharacterSkill.skillScaleClass.GetSkillScale(location, scale);

        foreach (OverlayTile tile in skillScale)
        {
            tile.ShowAsScale();
        }

        curSelectedCharacter.GetSkillScale(skillScale);
    }

    private void ShowPrevAttackRange(OverlayTile targetTile)
    {
        ResetOnTarget(prevAttackRange);

        if(curSelectedCharacter != null && targetTile != null)
        {
            prevAttackRange = rangeFinder.GetTilesInRange(targetTile.grid2DLocation, curSelectedCharacter.character.SO.range, false);

            foreach (OverlayTile tile in prevAttackRange)
            {
                tile.ShowTagetAttack();
            }
        }
    }

    private void GetTargetAttackRange(CharacterBase target)
    {
        ResetOnTarget(targetAttackRange);

        if(target != null && target.CheckEnemyAsId(player) && phase != PlayerPhase.SkillTargetSelect)
        {
            if(target.character.SO.attackMethod == Constants.AttackMethod.Melee)
            {
                targetAttackRange = rangeFinder.GetTilesInRange(target.curStandingTile.grid2DLocation, target.Mov, true);
            }
            else
            {
                targetAttackRange = rangeFinder.GetTilesInRange(target.curStandingTile.grid2DLocation, target.character.SO.range, false);
            }

            foreach(OverlayTile tile in targetAttackRange)
            {
                tile.ShowTagetAttack();
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //IdlePhase 관련

    private void UpdateIdle()
    {
        if (nowPlayerTurn && canClick)
        {
            ChangePhase(PlayerPhase.Wait);
            StartCoroutine(nameof(ShowAlly));
        }
    }

    private IEnumerator ShowAlly()
    {
        CameraController.instance.ResetGroup();
        CameraController.instance.AddGroupRange(characterList.FindAll(x => !x.isDead));
        CameraController.instance.SetCameraOnSelected();

        yield return new WaitForSeconds(1f);

        ChangePhase(PlayerPhase.CharacterSelect);
        CameraController.instance.ResetGroup();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //기타 함수들

    public RaycastHit2D GetTouchOnce()// 한번 클릭
    {
        RaycastHit2D hit = new RaycastHit2D();

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if ((Input.GetMouseButtonDown(0) && canClick))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            }
        }

        return hit;
    }

    public bool TouchOnce()
    {
        if(Input.touchCount == 1 && canClick)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
    }

    public RaycastHit2D GetTouching()// 드래그 방식
    {
        RaycastHit2D hit = new RaycastHit2D();

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButton(0) && canClick)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            }
        }

        return hit;
    }

    private void OnClickCancelButton()
    {
        switch (phase)
        {
            case PlayerPhase.ActingSelect:
                ChangePhase(PlayerPhase.CharacterSelect);
                break;
            case PlayerPhase.MoveandAttack:
            case PlayerPhase.SkillTargetSelect:
                ChangePhase(PlayerPhase.ActingSelect);
                break;
        }
    }

    public void TurnEnd()// 턴 종료
    {
        if (nowPlayerTurn && (!curSelectedCharacter || !curSelectedCharacter.isWalking || !curSelectedCharacter.isAttacking))
        {
            ChangePhase(PlayerPhase.Idle);

            Ui.OnClickTurnEndButton -= TurnEnd;

            Ui.OnClickCancelButton -= OnClickCancelButton;

            Ui.OnClickMoveAndAttackButton -= OnClickMoveAndAttack;
            Ui.OnClickUseSkillButton -= OnClickUseSkill;

            Ui.OnClickAttackButton -= AttackTarget;
            Ui.OnClickMoveButton -= MoveCharacter;

            Ui.OnClickSkillConFirmButton -= UseSkill;

            canClick = false;
            nowPlayerTurn = false;
            BattleManager.Instance.PlayerTurnEnd();
        }
    }

    public void EndGame(string playerId)
    {
        if(player.playerId != playerId)
        {
            player.isWin = true;
        }
    }

}
