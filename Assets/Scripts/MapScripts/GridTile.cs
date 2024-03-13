using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTile : MonoBehaviour
{
    private MapTiles tiles;
    [SerializeField] private Tilemap gridTile;
    [SerializeField] private Tilemap StartPosition;

    private void Awake()
    {
        tiles = GetComponentInParent<MapTiles>();
        Debug.Log(tiles.name);

        tiles.gridTile = gridTile;
        tiles.startPosition = StartPosition;
    }
}
