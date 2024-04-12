using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTile : MonoBehaviour
{
    private MapTiles tiles;
    [SerializeField] private Tilemap gridTile;
    [SerializeField] private List<Transform> spawnPosition;

    private void Awake()
    {
        tiles = GetComponentInParent<MapTiles>();

        tiles.gridTile = gridTile;
        tiles.startPosition = spawnPosition; ;
    }
}
