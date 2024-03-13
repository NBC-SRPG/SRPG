using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class MapTiles : MonoBehaviour
{
    public List<CharacterBase> characters;

    public Tilemap gridTile;
    public GameObject startPosition;

    [SerializeField] private GameObject overlayPrefabs;
    [SerializeField] private GameObject overlayContainer;

    private void Awake()
    {
        //Managers.MapManager.Init();
        //Managers.BattleManager.Init();
    }

    private void Start()
    {
        Managers.BattleManager.characters = characters;

        InitiateMapTile();
        InitiateStartTile();
        InitiateCharacter();
    }

    private void InitiateMapTile()// Ÿ�ϸ����κ��� overlayTile ����
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

    private void InitiateStartTile()// ĳ���� ���� ��ġ ����
    {
        Tilemap[] tiles = startPosition.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tile in tiles)
        {
            BoundsInt bound = tile.cellBounds;

            Managers.MapManager.startTile.Add(new Vector2Int(bound.x, bound.y));

        }
    }

    public void InitiateCharacter()//ĳ���� ������ġ�� ĳ���� ����(BattleManager�� �Ű��� ���ɼ� ����)
    {
        int i = 0;
        foreach (CharacterBase charac in Managers.BattleManager.characters)
        {
            if (i < Managers.MapManager.startTile.Count)
            {
                CharacterBase character = Instantiate(charac);

                character.curStandingTile = Managers.MapManager.map[Managers.MapManager.startTile[i]];
                character.curStandingTile.curStandingCharater = character;

                character.transform.position = character.curStandingTile.transform.position;
                i++;
            }
        }
    }
}
