using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPhase
{
    Idle,
    CharacterSelect,
    ActingSelect,
    MoveandAttack,
    TargetSelect
}

public class CharacterController : MonoBehaviour
{
    private RangeFinder rangeFinder;
    private PathFinder pathFinder;

    [SerializeField] private List<OverlayTile> movePath = new List<OverlayTile>();
    private List<OverlayTile> attackRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> moveRangeTiles = new List<OverlayTile>();
    List<OverlayTile> surroundPath = new List<OverlayTile>();

    private bool nowPlayerTurn;
    private bool canClick;
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

    private void ChangePhase(PlayerPhase newPhase)
    {
        phase = newPhase;

        switch (phase)
        {
            case PlayerPhase.CharacterSelect:
                curSelectedCharacter = null;
                ClearTile(moveRangeTiles, true);
                ClearTile(movePath, true);
                break;
            case PlayerPhase.MoveandAttack:
                GetRangeTile();
                movePath.Clear();
                movePath.Add(curSelectedCharacter.curStandingTile);
                break;
        }

    }

    private void CharacterMoveandAttackPhase()
    {
        if (!isMoving)
        {
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

            if (Input.GetMouseButtonDown(1) && canClick)
            {
                if (movePath[movePath.Count - 1].curStandingCharater == null)
                {
                    canClick = false;
                    ClearTile(surroundPath);
                    isMoving = true;
                }
            }
        }
        else
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        if (movePath.Count > 1)
        {
            curSelectedCharacter.gameObject.transform.position = Vector2.MoveTowards(curSelectedCharacter.gameObject.transform.position, movePath[1].transform.position, 5 * Time.deltaTime);
            curSelectedCharacter.gameObject.transform.position = new Vector3(curSelectedCharacter.gameObject.transform.position.x, curSelectedCharacter.gameObject.transform.position.y, movePath[0].transform.position.z);

            if (curSelectedCharacter.gameObject.transform.position == movePath[1].transform.position)
            {
                movePath.RemoveAt(0);
                curSelectedCharacter.MoveTile(movePath[0]);
                Managers.MapManager.CompleteMove();
            }
        }

        if (movePath.Count == 1 && isMoving)
        {
            movePath.RemoveAt(0);
            ChangePhase(PlayerPhase.CharacterSelect);
            canClick = true;
            isMoving = false;
        }
    }

    private void GetPathTile()
    {
        surroundPath.Clear();

        if (movePath.Count <= curSelectedCharacter.walkRange)
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

                if (surroundPath.Contains(curTile))
                {
                    movePath.Add(curTile);
                    ClearTile(surroundPath);
                }

                if (movePath.Contains(curTile) && movePath.Count > 0 && movePath[movePath.Count - 1] != curTile)
                {
                    int n = movePath.IndexOf(curTile);
                    List<OverlayTile> temp = movePath.GetRange(0, n + 1);

                    ClearTile(movePath);
                    movePath = temp;

                    ClearTile(surroundPath);

                }
            }
        }

        foreach (OverlayTile tile in movePath)
        {
            tile.ShowAsPath();
        }
    }


    private void GetRangeTile()
    {
        moveRangeTiles = rangeFinder.GetTilesInRangeInMove(new Vector2Int(curSelectedCharacter.curStandingTile.gridLocation.x, curSelectedCharacter.curStandingTile.gridLocation.y), curSelectedCharacter.walkRange);

        foreach (OverlayTile tile in moveRangeTiles)
        {
            tile.ShowTile();
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
