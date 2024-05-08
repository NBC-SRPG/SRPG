using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTile : MonoBehaviour
{
    [SerializeField] private Tilemap gridTile;
    [SerializeField] private List<Transform> spawnPosition;

    private void Awake()
    {
    }
}
