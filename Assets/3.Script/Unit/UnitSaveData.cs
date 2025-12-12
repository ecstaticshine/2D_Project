using System;
using System.Collections.Generic;

[Serializable]
public class UnitSaveData
{
    public string unitId;
    public string currentClassId;
    public string unitName;
    public bool isAlive; // 생존해있는가?
    public int level;    // 레벨
    public int exp;      // 경험치
    public int currentHp;// 현재 피
    public Stats currentStats; // 현재 스테이터스
    public int PosX; // 스테이지 X 좌표 
    public int PosY; // 스테이지 Y 좌표 
    public List<Item> Inventory; // 인벤토리
    public List<WeaponRankData> weaponRanks; // 현재 캐릭터 무기 랭크
}
