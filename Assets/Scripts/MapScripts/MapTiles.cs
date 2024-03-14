using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class MapTiles : MonoBehaviour
{
    public List<CharacterBase> characters;

    [HideInInspector] public Tilemap gridTile;
    [HideInInspector] public List<Tilemap> startPosition;

    [SerializeField] private GameObject overlayPrefabs;
    [SerializeField] private GameObject overlayContainer;

    private void Awake()
    {
        Managers.MapManager.Init();
        Managers.BattleManager.Init();
    }

    private void Start()
    {
        InitiateMapTile();
        InitiateStartTile();
        //InitiateCharacter();
    }

    private void InitiateMapTile()// 타일맵으로부터 overlayTile 생성
    {
        BoundsInt bounds = gridTile.cellBounds;

        for (int z = bounds.min.z; z <= bounds.max.z; z++)
        {
            for(int y = bounds.min.y; y <= bounds.max.y; y++)
            {
                for(int x = bounds.min.x; x <= bounds.max.x; x++)
                {
                    Vector3Int tileLocation = new Vector3Int(x, y, z);
                    Vector2Int tileKey = new Vector2Int(x, y);

                    if (gridTile.HasTile(tileLocation))
                    {
                        if (!Managers.MapManager.map.ContainsKey(tileKey))
                        {
                            GameObject overlayTile = Instantiate(overlayPrefabs, overlayContainer.transform);
                            Vector3 cellWorldPosition = gridTile.GetCellCenterWorld(tileLocation);

                            overlayTile.transform.position = cellWorldPosition;
                            
                            OverlayTile tile = overlayTile.GetComponent<OverlayTile>();
                            tile.gridLocation = tileLocation;

                            Managers.MapManager.map.Add(tileKey, tile);

                            if(z >= 1)
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
        }
    }

    private void InitiateStartTile()// 캐릭터 시작 위치 설정
    {
        int i = 0;
        foreach (Tilemap st in startPosition)
        {
            Tilemap tiles = st;
            List<Vector2Int> positions = new List<Vector2Int>();

            BoundsInt bounds = tiles.cellBounds;

            for (int z = bounds.min.z; z <= bounds.max.z; z++)
            {
                for (int y = bounds.min.y; y <= bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x <= bounds.max.x; x++)
                    {
                        Vector3Int tileLocation = new Vector3Int(x, y, z);

                        if (tiles.HasTile(tileLocation))
                        {
                            positions.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }

            Managers.MapManager.startTiles.Add(i, positions);
            i++;
        }
    }

    //public void InitiateCharacter()//캐릭터 스폰위치에 캐릭터 생성(BattleManager로 옮겨질 가능성 높음)
    //{
    //    int i = 0;
    //    foreach (CharacterBase charac in characters)
    //    {
    //        if (i < Managers.MapManager.startTile.Count)
    //        {
    //            CharacterBase character = Instantiate(charac);

    //            character.curStandingTile = Managers.MapManager.map[Managers.MapManager.startTile[i]];
    //            character.curStandingTile.curStandingCharater = character;

    //            character.transform.position = character.curStandingTile.transform.position;
    //            i++;
    //        }
    //    }
    //}
}
