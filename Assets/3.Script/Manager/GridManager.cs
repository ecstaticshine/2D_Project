using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("전체맵")]
    public Tilemap Map;

    [Header("Tile Layers (타일 레이어)")]
    public Tilemap[] TileLayers;

    public GameObject TopRight_ob, BottomLeft_ob;

    [SerializeField]
    private Vector2Int bottomLeft, topRight;

    public Grid grid;

    public TileData[,] tiles;

    private int minX, maxX, minY, maxY;

    public Dictionary<Vector2Int, UnitController> unitMap = new Dictionary<Vector2Int, UnitController>();

    private void Awake()
    {
        Instance = this;
        SetPosition();
        InitializeTiles();

    }

    private void SetPosition()
    {
        bottomLeft = new Vector2Int((int)BottomLeft_ob.transform.position.x, (int)BottomLeft_ob.transform.position.y);
        topRight = new Vector2Int((int)TopRight_ob.transform.position.x, (int)TopRight_ob.transform.position.y);
    }

    public void InitializeTiles()
    {
        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;

        tiles = new TileData[maxX - minX, maxY - minY];

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                TileType tileType = TileType.Wall;
                bool walkable = true;
                int defenseBonus = 0;
                int restBonus = 0;
                int avoidRate = 0;
                int healValue = 0;

                Vector3Int tilePos = new Vector3Int(x, y, 0);

                foreach (var layer in TileLayers)
                {
                    var tile = layer.GetTile(tilePos);
                    if (tile == null) continue;

                    string tileName = layer.gameObject.name;

                    switch (tileName)
                    {
                        case "Flat":
                            tileType = TileType.Flat;
                            break;
                        case "Floor":
                            tileType = TileType.Floor;
                            break;
                        case "Wall":
                            tileType = TileType.Wall;
                            walkable = false;
                            break;
                        case "BreakableWall":
                            tileType = TileType.BreakableWall;
                            walkable = false;
                            break;
                        case "Stair":
                            tileType = TileType.Stair;
                            break;
                        case "Pillar":
                            tileType = TileType.Pillar;
                            defenseBonus = 1;
                            avoidRate = 20;
                            break;
                        case "Throne":
                            tileType = TileType.Throne;
                            defenseBonus = 3;
                            restBonus = 5;
                            avoidRate = 30;
                            healValue = 10;
                            break;
                        case "Door":
                            tileType = TileType.Door;
                            walkable = false;
                            break;
                        case "TreasureChest":
                            tileType = TileType.TreasureChest;
                            break;

                    }

                }

                tiles[x - minX, y - minY] = new TileData(tileType, walkable, defenseBonus, restBonus, avoidRate, healValue);

            }
        }
    }

    public bool IsWalkable(Vector2Int tilePos)
    {
        int x = tilePos.x - minX;
        int y = tilePos.y - minY;

        if (x < 0 || x >= tiles.GetLength(0) ||
            y < 0 || y >= tiles.GetLength(1))
            return false;

        return tiles[x, y].walkable;
    }

    public TileType GetTileType(Vector2Int tilePos)
    {
        return tiles[tilePos.x - minX, tilePos.y - minY].tiletype;
    }

    public int GetAvoid(Vector2Int tilePos)
    {
        return tiles[tilePos.x - minX, tilePos.y - minY].avoidRate;
    }

    public int GetDef(Vector2Int tilePos)
    {
        return tiles[tilePos.x - minX, tilePos.y - minY].defenseBonus;
    }

    public TileData[,] GetTiles()
    {
        return tiles;
    
    }

    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= minX && pos.x < maxX && pos.y >= minY && pos.y < maxY;
    }

    public Vector3 CellToWorld(Vector2Int pos)
    {
        return grid.CellToWorld(new Vector3Int(pos.x, pos.y, 0));
    }

    public Vector3 TileToWorld(Vector2Int tile)
    {
        Vector3Int cell = new Vector3Int(tile.x, tile.y, 0);
        return Map.GetCellCenterWorld(cell);
    }

    public Vector3 CellToWorldCenter(Vector2Int tile)
    {
        Vector3 basePos = grid.CellToWorld(new Vector3Int(tile.x, tile.y, 0));
        return basePos + new Vector3(0.5f, 0.5f, 0);  // 타일 중앙
    }
}
