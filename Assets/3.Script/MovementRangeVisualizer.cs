using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementRangeVisualizer : MonoBehaviour
{
    public static MovementRangeVisualizer Instance;

    public Tilemap highlightTilemap;
    public Tile highlightTile;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMoveRange(HashSet<Vector2Int> range)
    {
        highlightTilemap.ClearAllTiles();

        foreach (var pos in range)
        {
            highlightTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), highlightTile);
        }
    }

    public void Clear()
    {
        highlightTilemap.ClearAllTiles();
    }
}