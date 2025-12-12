using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitJsonData
{
    public string UnitId;
    public string BaseClassId;
    public string UnitName;
    public int Level;
    public int Exp;
    public Stats BaseStats;
    public GrowthRates GrowthRates;
    public List<WeaponRankData> WeaponRanks;
    public List<Item> Inventory;
}

[Serializable]
public class WeaponJsonData
{
    public string ItemId;
    public string ItemName;
    public ItemType ItemType;
    public WeaponType WeaponType;
    public int Durability;
    public WeaponRank WeaponRank;
    public int Power;
    public int AttackRangeMin;
    public int AttackRangeMax;
    public int Accuracy;
    public int Weight;
    public int Crit;
}

[Serializable]
public class WeaponDatabase
{
    public List<WeaponJsonData> weapons = new List<WeaponJsonData>();
}

[Serializable]
public class StaffJsonData
{
    public string ItemId;
    public string ItemName;
    public ItemType ItemType;
    public int Durability;
    public WeaponRank WeaponRank;
    public int Heal;
    public int RangeMin;
    public int RangeMax;
}

[Serializable]
public class StaffDatabase
{
    public List<StaffJsonData> staffs = new List<StaffJsonData>();
}

[Serializable]
public class ConsumableJsonData
{
    public string ItemId;
    public string ItemName;
    public ItemType ItemType;
    public int Durability;
    public ConsumableType effectType;
    public int effectValue;
}

[Serializable]
public class ConsumableDatabase
{
    public List<ConsumableJsonData> consumables = new List<ConsumableJsonData>();
}
