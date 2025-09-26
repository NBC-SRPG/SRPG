using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialMove : TutorialBase
{
    private BattleUI Ui;
    private bool checkTrigger;
    [SerializeField] private TextMeshProUGUI messege;

    [SerializeField] private Tilemap targetTile;
    private List<OverlayTile> targetPosition;

    private void Start()
    {
        targetPosition = new List<OverlayTile>();

        InitiateMapTile();

        messege.gameObject.SetActive(false);
        targetTile.gameObject.SetActive(false);
    }

    private void InitiateMapTile()// 타일맵으로부터 overlayTile 생성
    {

        foreach (Vector3Int pos in targetTile.cellBounds.allPositionsWithin)
        {
            Vector3Int tileLocation = new Vector3Int(pos.x, pos.y, pos.z);
            Vector2Int tileKey = new Vector2Int(pos.x, pos.y);

            if (targetTile.HasTile(tileLocation))
            {
                if (MapManager.instance.map.ContainsKey(tileKey))
                {
                    targetPosition.Add(MapManager.instance.map[tileKey]);
                }
            }

        }
    }

    public override void Enter()
    {
        checkTrigger = false;
        Ui = Managers.UI.FindUI<BattleUI>();

        Ui.OnClickMoveButton += MoveCharacter;

        messege.gameObject.SetActive(true);
        targetTile.gameObject.SetActive(true);
    }

    private void MoveCharacter()
    {
        AnimationController.instance.onAnimationEnd += EndMove;
    }

    private void EndMove()// 캐릭터의 이동이 끝났을 시
    {
        AnimationController.instance.onAnimationEnd -= EndMove;

        foreach(OverlayTile tile in targetPosition)
        {
            if(tile.curStandingCharater != null && tile.curStandingCharater.player == Managers.GameManager.player)
            {
                checkTrigger = true;
                break;
            }
        }
    }

    public override void Execute(TutorialController controller)
    {
        Ui.HideSkillButton();

        if (checkTrigger)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        Ui.OnClickMoveButton -= MoveCharacter;
        messege.gameObject.SetActive(false);
        targetTile.gameObject.SetActive(false);
    }

}
