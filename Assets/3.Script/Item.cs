using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Weapon,
    Staff,
    Consumable
}

public enum ConsumableType
{
    Heal,
    OpenAll,
    OpenTreasure
}
[Serializable]
public class Item
{
    public string itemId;
    public int Durability;

}

public class Weapon : Item
{
    public string itemName;
    public ItemType itemType;
    public WeaponType WeaponType;
    public WeaponRank WeaponRank;
    public int AttackRangeMin; // 최소 공격 거리
    public int AttackRangeMax; // 최장 공격 거리
    public int Weight;
    public int Power;
    public int Accuracy;
    public int Crit;
}

public class Consumable : Item
{
    public string itemName;
    public int effect;
    public ItemType itemType;
}

public class Staff : Item
{
    public string itemName;
    public ItemType itemType;
    public WeaponRank WeaponRank;
    public int Heal;
    public int RangeMin; // 최소 공격 거리
    public int RangeMax; // 최장 공격 거리

}