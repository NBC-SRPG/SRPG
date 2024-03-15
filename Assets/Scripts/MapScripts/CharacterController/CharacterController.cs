using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    private enum PlayerPhase
    {
        Idle,
        CharacterSetting,
        CharacterSelect,
        ActingSelect,
        MoveandAttack,
        SkillTargetSelect
    }

    public Button button;

    [SerializeField] private CharacterBase chaPrefabs;
    [SerializeField] private TargetTiles targetTiles;

    [field: SerializeField] public GamePlayer player;

    private RangeFinder rangeFinder;
    private PathFinder pathFinder;

    [SerializeField] private List<OverlayTile> movePath = new List<OverlayTile>();//이동 경로 타일
    private List<OverlayTile> attackRangeTiles = new List<OverlayTile>();//공격 가능한 타일
    private List<OverlayTile> moveRangeTiles = new List<OverlayTile>();//이동 가능한 타일
    private List<OverlayTile> surroundPath = new List<OverlayTile>();//클릭 가능한 타일
    private List<OverlayTile> skillScale = new List<OverlayTile>();//스킬 범위 타일

    [SerializeField] private bool nowPlayerTurn;
    private bool canClick;//false일 때 터치 안되게

    private PlayerPhase phase;
    public CharacterBase curSelectedCharacter;
    public CharacterBase curTargetCharacter;

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

        if (!Managers.BattleManager.players.Contains(player))
        {
            Managers.BattleManager.players.Add(player);
        }
        Managers.BattleManager.charactersAsTeam.Add(player.playerId, new List<CharacterBase>());

        Managers.BattleManager.TurnStart += GetPlayerTurn;

        //-----------------------------------------
        //button.onClick.AddListener(TurnEnd);
        //버튼 연결은 UiManager를 통해 BattleUI에 이벤트에 연결하는 식으로 진행
        //----------------------------------------

    }

    private void Update()
    {
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
        if (Managers.BattleManager.nowPlayer.playerId == player.playerId)
        {
            canClick = true;
            nowPlayerTurn = true;

            button.onClick.AddListener(TurnEnd);
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //CharacterSettingPhase 관련
    private void CharacterSet()
    {
        InitiateCharacter();
    }

    public void InitiateCharacter()//캐릭터 스폰위치에 캐릭터 생성
    {
        int i = 0;
        foreach (Character charac in player.party)
        {
            if (i < Managers.MapManager.startTiles[player.playerNumber].Count)
            {
                CharacterBase character = Instantiate(chaPrefabs, transform);
                character.InitCharacter(charac, player.playerId);

                character.curStandingTile = Managers.MapManager.map[Managers.MapManager.startTiles[player.playerNumber][i]];
                character.curStandingTile.curStandingCharater = character;

                character.transform.position = character.curStandingTile.transform.position;
                i++;
            }
        }
        ChangePhase(PlayerPhase.Idle);

        player.isReady = true;
        Managers.BattleManager.GetReady();

        Debug.Log(player.playerId + "character set");
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //페이즈 변환 및 타일 관련 함수

    private void ChangePhase(PlayerPhase newPhase)// 페이즈 변환
    {
        phase = newPhase;

        switch (phase)
        {
            case PlayerPhase.Idle:
                ClearAllTile();
                nowPlayerTurn = false;
                canClick = false;
                SelectCurCharacter(null);
                SelectTargetCharacter(null);
                break;
            case PlayerPhase.CharacterSelect:
                ClearAllTile();
                SelectCurCharacter(null);
                SelectTargetCharacter(null);
                break;
            case PlayerPhase.ActingSelect:
                ClearAllTile();
                SelectTargetCharacter(null);
                break;
            case PlayerPhase.MoveandAttack:
                ClearAllTile();
                movePath.Add(curSelectedCharacter.curStandingTile);
                SelectTargetCharacter(null);
                break;
            case PlayerPhase.SkillTargetSelect:
                ClearAllTile();
                SelectTargetCharacter(null);
                break;
        }

    }

    //타일 지우기
    private void ClearTile(List<OverlayTile> tileList, bool unable = false)
    {
        foreach (OverlayTile tile in tileList)
        {
            tile.ResetTile();
            if (unable)
            {
                tile.HideTile();
            }
        }

        tileList.Clear();
    }

    //모든 타일 지우기
    private void ClearAllTile()
    {
        ClearTile(movePath, true);
        ClearTile(moveRangeTiles, true);
        ClearTile(attackRangeTiles, true);
        ClearTile(surroundPath, true);
        ClearTile(skillScale, true);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //CharacterSelectPhase 관련

    // 캐릭터 선택 페이즈
    private void SelectCharacterPhase()
    {
        RaycastHit2D hit = GetTouchDown();

        if (hit)
        {
            OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

            if (curTile.curStandingCharater != null)
            {
                if (curTile.curStandingCharater.playerId == player.playerId)// 해당 캐릭터가 내 캐릭터일 때
                {
                    SelectCurCharacter(curTile.curStandingCharater);
                    if (curSelectedCharacter.canActing)// 해당 캐릭터가 아직 행동하지 않았을 때
                    {
                        ChangePhase(PlayerPhase.MoveandAttack);
                    }
                }
                else
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
        curSelectedCharacter = character;
        //이후 ui에 캐릭터 정보를 보내줌
    }

    private void SelectTargetCharacter(CharacterBase character)
    {
        curTargetCharacter = character;
        //이후 ui에 캐릭터 정보를 보내줌
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //CharacterMoveandAttackPhase 관련

    private void CharacterMoveandAttackPhase()// 캐릭터 이동 및 일반 공격 페이즈
    {
        if (!curSelectedCharacter.canActing)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        if (!curSelectedCharacter.isWalking)// 캐릭터가 이동중이 아닐 때
        {
            if (!curSelectedCharacter.didWalk)// 아직 한번도 이동하지 않았다면, 이동 범위 가져옴
            {
                GetMoveRangeTile();
                GetPathTile();
            }
            if (curSelectedCharacter.character.CharacterAttackType == Constants.AttackType.Range && !curSelectedCharacter.didAttack)// 해당 캐릭터가 원거리형 캐릭터고 공격하지 않았을 때 
            {
                GetAttackRangeTile(curSelectedCharacter.character.characterData.atk_range);// 공격 가능 범위 가져옴
            }

            RaycastHit2D hit = GetTouchDown();

            if (hit)
            {
                OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                if (curTile.curStandingCharater == curSelectedCharacter)// 현재 캐릭터 클릭 시 취소(취소 버튼으로 바꿀 예정)
                {
                    ChangePhase(PlayerPhase.CharacterSelect);
                    return;
                }

                //원거리 캐릭터가 공격 가능한 범위 내에 있는 적 터치 시
                if (curSelectedCharacter.character.CharacterAttackType == Constants.AttackType.Range && attackRangeTiles.Contains(curTile)
                   && curTile.curStandingCharater != null && curTile.curStandingCharater.CheckEnenmy(curSelectedCharacter))
                {
                    if (!curSelectedCharacter.didWalk && movePath.Count > 1)
                    {
                        ClearTile(movePath);
                        ClearTile(surroundPath);
                        movePath.Add(curSelectedCharacter.curStandingTile);
                    }
                    SelectTargetCharacter(curTile.curStandingCharater);
                }
                else
                {
                    SelectTargetCharacter(null);
                }
            }

            //마우스 오른쪽 클릭 시 행동 확정(임시) - 버튼 클릭으로 바꿀 예정
            if (Input.GetMouseButtonDown(1) && canClick)
            {
                AttackTarget();

                MoveCharacter();
            }
        }
    }

    private void AttackTarget()
    {
        if (curTargetCharacter != null)
        {
            canClick = false;
            curSelectedCharacter.OnEndAttacking += EndAttack;

            Managers.BattleManager.Attack(curSelectedCharacter, curTargetCharacter);
        }
    }

    private void MoveCharacter()
    {
        if (movePath[movePath.Count - 1].curStandingCharater == null)
        {
            canClick = false;
            ClearTile(surroundPath);

            curSelectedCharacter.OnEndWalk += EndMove;

            StartCoroutine(curSelectedCharacter.MoveCharacter());
        }
    }

    private void EndMove()// 캐릭터의 이동이 끝났을 시
    {
        curSelectedCharacter.OnEndWalk -= EndMove;

        canClick = true;
        ChangePhase(PlayerPhase.MoveandAttack);
    }

    private void EndAttack()// 캐릭터의 공격이 끝났을 시
    {
        curSelectedCharacter.OnEndAttacking -= EndAttack;

        canClick = true;
        ChangePhase(PlayerPhase.MoveandAttack);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //SkillTargetSeletPhase 관련

    private void SkillTargetSelectPhase()// 스킬 범위 선택
    {
        if (!curSelectedCharacter.canActing)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        GetAttackRangeTile(curSelectedCharacter.curCharacterSkill.skillData.skillRange);

        if (curSelectedCharacter.curCharacterSkill.skillData.scaleType != Constants.SkillScaleType.None)// 단일 지정 스킬이 아닐 때
        {
            RaycastHit2D hit = GetTouching();

            if (hit)
            {
                OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                if (attackRangeTiles.Contains(curTile))
                {

                    ClearTile(skillScale, true);

                    GetSkillScaleTile(curTile.grid2DLocation, curSelectedCharacter.curCharacterSkill.skillData.skillScale);
                }
            }
        }
        else
        {
            RaycastHit2D hit = GetTouchDown();

            if (hit)
            {
                OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                //원거리 캐릭터가 공격 가능한 범위 내에 있는 적 터치 시
                if (attackRangeTiles.Contains(curTile) && curTile.curStandingCharater != null)
                {
                    if (curSelectedCharacter.curCharacterSkill.skillData.targetType == Constants.SkillTargetType.Enemy && curTile.curStandingCharater.CheckEnenmy(curSelectedCharacter))
                    {
                        SelectTargetCharacter(curTile.curStandingCharater);
                    }
                    else if(curSelectedCharacter.curCharacterSkill.skillData.targetType == Constants.SkillTargetType.Ally && !curTile.curStandingCharater.CheckEnenmy(curSelectedCharacter))
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
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //거리 탐색 함수들

    //이동 가능 위치 탐색
    private void GetPathTile()
    {
        if (movePath.Count <= curSelectedCharacter.character.Mov)// 선택한 캐릭터의 걸음 횟수가 남아있다면 
        {
            surroundPath = pathFinder.MakePath(movePath[movePath.Count - 1], movePath);

            foreach (OverlayTile tile in surroundPath)
            {
                tile.ShowAsMove();
            }

        }

        RaycastHit2D hit = GetTouching();

        if (hit)
        {
            OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

            if (surroundPath.Contains(curTile) && curTile.canClick)//이동할 위치 선택 시
            {
                SelectTargetCharacter(null);
                movePath.Add(curTile);
                ClearTile(surroundPath, true);
            }

            if (movePath.Contains(curTile) && movePath.Count > 0 && movePath[movePath.Count - 1] != curTile)//이미 선택된 타일 터치 시
            {
                int n = movePath.IndexOf(curTile);
                List<OverlayTile> temp = movePath.GetRange(0, n + 1);

                ClearTile(movePath, true);
                movePath = temp;

                ClearTile(surroundPath, true);

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
        moveRangeTiles = rangeFinder.GetTilesInRange(curSelectedCharacter.curStandingTile.grid2DLocation, curSelectedCharacter.character.Mov, true);

        foreach (OverlayTile tile in moveRangeTiles)
        {
            tile.ShowTile();
        }
    }

    private void GetAttackRangeTile(int range)// 공격 가능 거리 가져옴
    {
        attackRangeTiles = rangeFinder.GetTilesInRange(curSelectedCharacter.curStandingTile.grid2DLocation, range, false);

        foreach (OverlayTile tile in attackRangeTiles)
        {
            if (!moveRangeTiles.Contains(tile))
            {
                tile.ShowAsAttack();
            }
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
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //IdlePhase 관련

    private void UpdateIdle()
    {
        if (nowPlayerTurn && canClick)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //기타 함수들

    public RaycastHit2D GetTouchDown()// 한번 클릭
    {
        RaycastHit2D hit = new RaycastHit2D();

        if (Input.GetMouseButtonDown(0) && canClick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        }

        return hit;
    }

    public RaycastHit2D GetTouching()// 드래그 방식
    {
        RaycastHit2D hit = new RaycastHit2D();

        if (Input.GetMouseButton(0) && canClick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        }

        return hit;
    }

    private void OnClickCancelButton()
    {
        switch (phase)
        {
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

            button.onClick.RemoveListener((TurnEnd));
            canClick = false;
            nowPlayerTurn = false;
            Managers.BattleManager.PlayerTurnEnd();
        }
    }
}
