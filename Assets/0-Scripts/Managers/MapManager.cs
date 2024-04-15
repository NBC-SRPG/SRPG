using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private enum Direction
    {
        Right,
        Left
    }

    [System.Serializable]
    private struct StartPositions
    {
        public Transform positions;
        public Direction direction;
    }

    public struct StartTiles
    {
        public List<OverlayTile> startTile;
        public Vector2 startDirection;
    }

    public static MapManager instance;

    [HideInInspector]
    public Vector2Int[] direction = new Vector2Int[] 
    {
        new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1),
    };

    public Tilemap gridTile;
    [SerializeField] private List<StartPositions> playerStartPosition;
    [SerializeField] private List<StartPositions> enemyStartPosition;

    [SerializeField] private GameObject overlayPrefabs;
    [SerializeField] private GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    public Dictionary<int, StartTiles> playerStartTiles;
    public Dictionary<int, StartTiles> enemyStartTiles;
    //public List<Vector2Int> startTile = new List<Vector2Int>();

    public PolygonCollider2D cameraArea;

    public event Action OnCompleteMove;
    public event Action SetMapComplete;

    List<OverlayTile> surroundingTiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();

        InitiateMapTile();
        InitiateStartTile();
    }

    public void Init()// 모든 타일 초기화
    {
        map = new Dictionary<Vector2Int, OverlayTile>();
        playerStartTiles = new Dictionary<int, StartTiles>();
        enemyStartTiles = new Dictionary<int, StartTiles>();

        surroundingTiles = new List<OverlayTile>();
    }

    public void CompleteMove()// 이동 완료 시 이벤트
    {
        OnCompleteMove?.Invoke();
    }

    public void CompleteSetMap()
    {
        SetMapComplete?.Invoke();
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile, bool clickable = false)//4 방향 타일 가져오기
    {
        surroundingTiles.Clear();
        Vector2Int TileToCheck;

        for (int i = 0; i < 4; i++)
        {
            TileToCheck = new Vector2Int(originTile.x + direction[i].x, originTile.y + direction[i].y);
            if (map.ContainsKey(TileToCheck))
            {
                if (clickable)
                {
                    if (map[TileToCheck].canClick)
                    {
                        surroundingTiles.Add(map[TileToCheck]);
                    }
                }
                else
                {
                    surroundingTiles.Add(map[TileToCheck]);
                }
            }
        }

        return surroundingTiles;
    }

    public List<OverlayTile> GetSurroundingAllTiles(Vector2Int originTile, bool clickable = false)//8 방향 타일 가져오기
    {
        surroundingTiles.Clear();
        Vector2Int TileToCheck;

        for (int i = 0; i < direction.Length; i++)
        {
            TileToCheck = new Vector2Int(originTile.x + direction[i].x, originTile.y + direction[i].y);
            if (map.ContainsKey(TileToCheck))
            {
                if (clickable)
                {
                    if (map[TileToCheck].canClick)
                    {
                        surroundingTiles.Add(map[TileToCheck]);
                    }
                }
                else
                {
                    surroundingTiles.Add(map[TileToCheck]);
                }
            }
        }

        return surroundingTiles;
    }

    private void InitiateMapTile()// 타일맵으로부터 overlayTile 생성
    {

        foreach (Vector3Int pos in gridTile.cellBounds.allPositionsWithin)
        {
            Vector3Int tileLocation = new Vector3Int(pos.x, pos.y, pos.z);
            Vector2Int tileKey = new Vector2Int(pos.x, pos.y);

            if (gridTile.HasTile(tileLocation))
            {
                if (!MapManager.instance.map.ContainsKey(tileKey))
                {
                    GameObject overlayTile = Instantiate(overlayPrefabs, overlayContainer.transform);
                    Vector3 cellWorldPosition = gridTile.GetCellCenterWorld(tileLocation);

                    overlayTile.transform.position = cellWorldPosition;

                    OverlayTile tile = overlayTile.GetComponent<OverlayTile>();
                    tile.gridLocation = tileLocation;

                    MapManager.instance.map.Add(tileKey, tile);

                    if (pos.z >= 1)
                    {
                        tile.canClick = false;
                    }
                    else
                    {
                        tile.canClick = true;
                    }
                }
            }

        }
    }

    private void InitiateStartTile()// 캐릭터 시작 위치 설정
    {
        int i = 0;
        foreach (StartPositions st in playerStartPosition)
        {
            if (st.direction == Direction.Left)
            {
                playerStartTiles.Add(i, new StartTiles
                {
                    startTile = new List<OverlayTile>(),
                    startDirection = Vector2.left
                });
            }
            else
            {
                playerStartTiles.Add(i, new StartTiles
                {
                    startTile = new List<OverlayTile>(),
                    startDirection = Vector2.right
                });
            }

            foreach (Transform child in st.positions)
            {
                Vector2Int position = (Vector2Int)gridTile.WorldToCell(child.position);

                if (map.ContainsKey(position))
                {
                    playerStartTiles[i].startTile.Add(map[position]);
                }

            }

            i++;
        }

        i = 0;
        foreach (StartPositions st in enemyStartPosition)
        {
            if (st.direction == Direction.Left)
            {
                enemyStartTiles.Add(i, new StartTiles
                {
                    startTile = new List<OverlayTile>(),
                    startDirection = Vector2.left
                });
            }
            else
            {
                enemyStartTiles.Add(i, new StartTiles
                {
                    startTile = new List<OverlayTile>(),
                    startDirection = Vector2.right
                });
            }

            foreach (Transform child in st.positions)
            {
                Vector2Int position = (Vector2Int)gridTile.WorldToCell(child.position);

                if (map.ContainsKey(position))
                {
                    enemyStartTiles[i].startTile.Add(map[position]);
                }

            }

            i++;
        }
    }

}
