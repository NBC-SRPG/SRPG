using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    private enum PlayerPhase
    {
        Idle,
        CharacterSetting,
        CharacterSelect,
        ActingSelect,
        MoveandAttack,
        TargetSelect
    }

    public CharacterBase chaPrefabs;

    [field: SerializeField] public GamePlayer player;

    private RangeFinder rangeFinder;
    private PathFinder pathFinder;

    [SerializeField] private List<OverlayTile> movePath = new List<OverlayTile>();//이동 경로 타일
    private List<OverlayTile> attackRangeTiles = new List<OverlayTile>();//공격 가능한 타일
    private List<OverlayTile> moveRangeTiles = new List<OverlayTile>();//이동 가능한 타일
    private List<OverlayTile> surroundPath = new List<OverlayTile>();//클릭 가능한 타일

    private bool nowPlayerTurn;
    private bool canClick;//false일 때 터치 안되게

    private PlayerPhase phase;
    public CharacterBase curSelectedCharacter;
    public CharacterBase curTargetCharacter;

    private void Awake()
    {
        rangeFinder = new RangeFinder();
        pathFinder = new PathFinder();

        phase = PlayerPhase.CharacterSetting;
    }

    private void Start()
    {
        canClick = false;
        nowPlayerTurn = false;

        if (!Managers.BattleManager.players.Contains(player))
        {
            Managers.BattleManager.players.Add(player);
            Managers.BattleManager.charactersAsTeam.Add(player.playerId, new List<CharacterBase>());
        }
    }

    private void Update()
    {
        if(Managers.BattleManager.nowPlayer.playerId == player.playerId)
        {
            canClick = true;
            nowPlayerTurn = true;
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
            case PlayerPhase.MoveandAttack:
                CharacterMoveandAttackPhase();
                break;
        }
    }

    private void CharacterSet()
    {
        InitiateCharacter();

        ChangePhase(PlayerPhase.Idle);
    }

    public void InitiateCharacter()//캐릭터 스폰위치에 캐릭터 생성
    {
        int i = 0;
        foreach (Character charac in player.party)
        {
            if (i < Managers.MapManager.startTiles[player.playerNumber].Count)
            {
                CharacterBase character = Instantiate(chaPrefabs);
                character.character = charac;
                character.playerId = player.playerId;

                character.curStandingTile = Managers.MapManager.map[Managers.MapManager.startTiles[player.playerNumber][i]];
                character.curStandingTile.curStandingCharater = character;

                character.transform.position = character.curStandingTile.transform.position;
                i++;
            }
        }
    }

    private void ChangePhase(PlayerPhase newPhase)// 페이즈 변환
    {
        phase = newPhase;

        switch (phase)
        {
            case PlayerPhase.Idle:
                ClearAllTile();
                nowPlayerTurn = false;
                canClick = false;
                curSelectedCharacter = null;
                curTargetCharacter = null;
                break;
            case PlayerPhase.CharacterSelect:
                ClearAllTile();
                curSelectedCharacter = null;
                curTargetCharacter = null;
                break;
            case PlayerPhase.MoveandAttack:
                ClearAllTile();
                movePath.Add(curSelectedCharacter.curStandingTile);
                break;
        }

    }

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

    private void ClearAllTile()
    {
        ClearTile(movePath, true);
        ClearTile(moveRangeTiles, true);
        ClearTile(attackRangeTiles, true);
        ClearTile(surroundPath, true);
    }

    // 캐릭터 선택 페이즈
    private void SelectCharacterPhase()
    {
        RaycastHit2D hit;

        if (UnityEngine.Input.GetMouseButtonDown(0) && canClick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit)
            {
                OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                if (curTile.curStandingCharater != null)
                {
                    curSelectedCharacter = curTile.curStandingCharater;
                    if (curSelectedCharacter.playerId == player.playerId && curSelectedCharacter.canActing)// 해당 캐릭터가 내 캐릭터이고, 아직 행동하지 않았을 때
                    {
                        ChangePhase(PlayerPhase.MoveandAttack);
                    }
                }
            }
        }

    }

    private void CharacterMoveandAttackPhase()// 캐릭터 이동 및 일반 공격 페이즈
    {
        if (!curSelectedCharacter.canActing)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
            return;
        }

        if (!curSelectedCharacter.isWalking)
        {
            if (!curSelectedCharacter.didWalk)
            {
                GetMoveRangeTile();
                GetPathTile();
            }
            if (curSelectedCharacter.character.CharacterAttackType == Constants.AttackType.Range && !curSelectedCharacter.didAttack)// 해당 캐릭터가 원거리형 캐릭터고 공격하지 않았을 때 
            {
                GetAttackRangeTile(curSelectedCharacter.character.characterData.atk_range);
            }

            RaycastHit2D hit;

            if (Input.GetMouseButtonDown(0) && canClick)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit)
                {
                    OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                    if (curTile.curStandingCharater == curSelectedCharacter)// 현재 캐릭터 클릭 시 취소(취소 버튼으로 바꿀 예정)
                    {
                        ChangePhase(PlayerPhase.CharacterSelect);
                        return;
                    }

                    //원거리 캐릭터가 공격 가능한 범위 내에 있는 적 터치 시
                    if(curSelectedCharacter.character.CharacterAttackType == Constants.AttackType.Range && attackRangeTiles.Contains(curTile)
                       && curTile.curStandingCharater != null && curTile.curStandingCharater.CheckEnenmy(curSelectedCharacter))
                    {
                        if (!curSelectedCharacter.didWalk && movePath.Count > 1)
                        {
                            ClearTile(movePath);
                            ClearTile(surroundPath);
                            movePath.Add(curSelectedCharacter.curStandingTile);
                        }
                        curTargetCharacter = curTile.curStandingCharater;
                    }
                    else
                    {
                        curTargetCharacter = null;
                    }
                }
            }

            //마우스 오른쪽 클릭 시 행동 확정(임시) - 버튼 클릭으로 바꿀 예정
            if (Input.GetMouseButtonDown(1) && canClick)
            {
                if (curTargetCharacter != null)
                {
                    canClick = false;
                    curSelectedCharacter.OnEndAttacking += EndAttack;

                    Managers.BattleManager.Attack(curSelectedCharacter, curTargetCharacter);
                }
                
                if (movePath[movePath.Count - 1].curStandingCharater == null)
                {
                    canClick = false;
                    ClearTile(surroundPath);

                    curSelectedCharacter.OnEndWalk += EndMove;

                    StartCoroutine(curSelectedCharacter.MoveCharacter());
                }

            }
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

    //이동 가능 위치 탐색
    private void GetPathTile()
    {
        surroundPath.Clear();

        if (movePath.Count <= curSelectedCharacter.character.Mov)// 선택한 캐릭터의 걸음 횟수가 남아있다면 
        {
            surroundPath = pathFinder.MakePath(movePath[movePath.Count - 1], movePath);

            foreach (OverlayTile tile in surroundPath)
            {
                tile.ShowAsMove();
            }

        }

        if (Input.GetMouseButton(0) && canClick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit)
            {
                OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                if (surroundPath.Contains(curTile) && curTile.canClick)//이동할 위치 선택 시
                {
                    curTargetCharacter = null;
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
        }

        curSelectedCharacter.movePath = movePath;

        foreach (OverlayTile tile in movePath)//선택된 이동 위치 표시
        {
            tile.ShowAsScale();
        }
    }


    private void GetMoveRangeTile()// 이동 가능 거리 가져옴
    {
        moveRangeTiles = rangeFinder.GetTilesInRange(new Vector2Int(curSelectedCharacter.curStandingTile.gridLocation.x, curSelectedCharacter.curStandingTile.gridLocation.y), curSelectedCharacter.character.Mov, true);

        foreach (OverlayTile tile in moveRangeTiles)
        {
            tile.ShowTile();
        }
    }

    private void GetAttackRangeTile(int range)// 공격 가능 거리 가져옴
    {
        attackRangeTiles = rangeFinder.GetTilesInRange(new Vector2Int(curSelectedCharacter.curStandingTile.gridLocation.x, curSelectedCharacter.curStandingTile.gridLocation.y), range, false);

        foreach (OverlayTile tile in attackRangeTiles)
        {
            if (!moveRangeTiles.Contains(tile))
            {
                tile.ShowAsAttack();
            }
        }
    }

    private void UpdateIdle()
    {
        if (nowPlayerTurn && canClick)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
        }
    }

    public void TurnEnd()// 턴 종료
    {
        if (nowPlayerTurn && (!curSelectedCharacter || !curSelectedCharacter.isWalking || !curSelectedCharacter.isAttacking))
        {
            ChangePhase(PlayerPhase.Idle);

            Managers.BattleManager.PlayerTurnEnd();
        }
    }
}
