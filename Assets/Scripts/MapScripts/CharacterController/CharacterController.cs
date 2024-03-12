using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private enum PlayerPhase
    {
        Idle,
        CharacterSelect,
        ActingSelect,
        MoveandAttack,
        TargetSelect
    }

    private RangeFinder rangeFinder;
    private PathFinder pathFinder;

    [SerializeField] private List<OverlayTile> movePath = new List<OverlayTile>();
    private List<OverlayTile> attackRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> moveRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> surroundPath = new List<OverlayTile>();

    private bool nowPlayerTurn;
    private bool canClick;//false일 때 터치 안되게
    private bool isMoving;

    private PlayerPhase phase;
    public CharacterBase curSelectedCharacter;

    private void Awake()
    {
        rangeFinder = new RangeFinder();
        pathFinder = new PathFinder();

        phase = PlayerPhase.Idle;
    }

    private void Start()
    {
        nowPlayerTurn = true;
        canClick = true;
        isMoving = false;
    }

    private void Update()
    {
        switch (phase)
        {
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

    private void ChangePhase(PlayerPhase newPhase)// 페이즈 변환
    {
        phase = newPhase;

        switch (phase)
        {
            case PlayerPhase.CharacterSelect:
                ClearTile(movePath, true);
                ClearTile(moveRangeTiles, true);
                ClearTile(attackRangeTiles, true);
                curSelectedCharacter = null;
                break;
            case PlayerPhase.MoveandAttack:
                ClearTile(movePath, true);
                ClearTile(moveRangeTiles, true);
                ClearTile(attackRangeTiles, true);
                movePath.Add(curSelectedCharacter.curStandingTile);
                break;
        }

    }

    private void CharacterMoveandAttackPhase()// 캐릭터 이동 및 일반 공격
    {
        if (!curSelectedCharacter.isWalking)
        {
            GetMoveRangeTile();
            if (curSelectedCharacter.character.CharacterAttackType == Constants.AttackType.Range && !curSelectedCharacter.didAttack)// 해당 캐릭터가 원거리형 캐릭터고 공격하지 않았을 때 
            {
                GetAttackRangeTile(curSelectedCharacter.character.characterData.atk_range);
            }
            GetPathTile();

            RaycastHit2D hit;

            if (Input.GetMouseButtonDown(0) && canClick)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit)
                {
                    OverlayTile curTile = hit.transform.GetComponent<OverlayTile>();

                    if (curTile.curStandingCharater == curSelectedCharacter)
                    {
                        ChangePhase(PlayerPhase.CharacterSelect);
                    }

                }
            }

            //마우스 오른쪽 클릭 시 행동 확정(임시) - 버튼 클릭으로 바꿀 예정
            if (Input.GetMouseButtonDown(1) && canClick)
            {
                if (movePath[movePath.Count - 1].curStandingCharater == null)
                {
                    curSelectedCharacter.movePath = movePath;
                    canClick = false;
                    ClearTile(surroundPath);
                    curSelectedCharacter.isWalking = true;

                    curSelectedCharacter.OnEndWalk += EndMove;
                }
            }
        }
        else
        {
            curSelectedCharacter.MoveCharacter();
        }
    }

    private void EndMove()// 캐릭터의 이동이 끝났을 시
    {
        curSelectedCharacter.OnEndWalk -= EndMove;

        canClick = true;
        ChangePhase(PlayerPhase.MoveandAttack);
    }

    //이동 가능 위치 탐색
    private void GetPathTile()
    {
        surroundPath.Clear();

        if (movePath.Count <= curSelectedCharacter.leftWalkRange)// 선택한 캐릭터의 걸음 횟수가 남아있다면 
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
                    movePath.Add(curTile);
                    ClearTile(surroundPath);
                }

                if (movePath.Contains(curTile) && movePath.Count > 0 && movePath[movePath.Count - 1] != curTile)//이미 선택된 타일 터치 시
                {
                    int n = movePath.IndexOf(curTile);
                    List<OverlayTile> temp = movePath.GetRange(0, n + 1);

                    ClearTile(movePath);
                    movePath = temp;

                    ClearTile(surroundPath);

                }
            }
        }

        foreach (OverlayTile tile in movePath)//선택된 이동 위치 표시
        {
            tile.ShowAsScale();
        }
    }


    private void GetMoveRangeTile()// 이동 가능 거리 가져옴
    {
        moveRangeTiles = rangeFinder.GetTilesInRangeInMove(new Vector2Int(curSelectedCharacter.curStandingTile.gridLocation.x, curSelectedCharacter.curStandingTile.gridLocation.y), curSelectedCharacter.leftWalkRange); ;

        foreach (OverlayTile tile in moveRangeTiles)
        {
            tile.ShowTile();
        }
    }

    private void GetAttackRangeTile(int range)// 공격 가능 거리 가져옴
    {
        attackRangeTiles = rangeFinder.GetTilesInRange(new Vector2Int(curSelectedCharacter.curStandingTile.gridLocation.x, curSelectedCharacter.curStandingTile.gridLocation.y), range); ;

        foreach (OverlayTile tile in attackRangeTiles)
        {
            if (!moveRangeTiles.Contains(tile))
            {
                tile.ShowAsAttack();
            }
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

    private void UpdateIdle()
    {
        if (nowPlayerTurn && canClick)
        {
            ChangePhase(PlayerPhase.CharacterSelect);
        }
    }

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
                    ChangePhase(PlayerPhase.MoveandAttack);
                }
            }
        }

    }
}
