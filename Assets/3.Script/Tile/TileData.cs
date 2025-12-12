using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//지형
public enum TileType
{
    Flat = 6,           //평지 설정한 레이어에 맞게 6부터 시작 
    Floor,              //바닥
    Wall,               //벽
    BreakableWall,       //부서질 수 있는 벽
    Stair,              //계단
    Pillar,             //기둥 -> 수비 + 1, 회피 + 20
    Throne,             //왕좌 -> 수비 + 3, 마방 + 5, 회피 + 30, HP + 10프로 회복
    Door,               //열어야 할 문
    TreasureChest,      //보물상자
}


public class TileData 
{
    public bool walkable;
    public int defenseBonus;
    public int restBonus;
    public int avoidRate;
    public int healValue;
    public TileType tiletype;
    public TileData(TileType tiletype, bool walkable, int defenseBonus, int restBonus, int avoidRate, int healValue)
    {
        this.tiletype = tiletype;
        this.walkable = walkable;
        this.defenseBonus = defenseBonus;
        this.restBonus = restBonus;
        this.avoidRate = avoidRate;
        this.healValue = healValue;
    }

}

public class Node
{
    public Vector2Int pos;
    public bool walkable;
    public Node ParentNode;
    public int X, Y;
    public int G;
    public int H;

    public int F;

    public Node(Vector2Int pos, int g, int h)
    {
        this.pos = pos;
        this.G = g;
        this.H = h;
        this.F = g + h;
    }
}
