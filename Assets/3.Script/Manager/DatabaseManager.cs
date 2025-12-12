using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{


    public static DatabaseManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);// 로드되는 동안은 파괴시키지 마세요.
            LoadDatabase();
        }
        else
        {

            Destroy(gameObject);

        }
    }


    private Dictionary<string, object> itemDict = new Dictionary<string, object>();
    private ItemDB database;

    void LoadDatabase()
    {
        string path = $"Json/Items/ItemDatabase";
        TextAsset JsonFile = Resources.Load<TextAsset>(path);

        database = JsonUtility.FromJson<ItemDB>(JsonFile.text);
        foreach (var item in database.weapons)
        {
            itemDict[item.ItemId] = item;
        }


        foreach (var item in database.staffs)
        {
            itemDict[item.ItemId] = item;
        }

        foreach (var item in database.consumables)
        {
            itemDict[item.ItemId] = item;
        }
    }

    public object GetItem(string itemId)
    {
        if (itemDict.TryGetValue(itemId, out var item))
            return item;

        Debug.LogError($"[ItemDB] 아이템 없음: {itemId}");
        return null;
    }
}
[Serializable]
public class WeaponData
{
    public string ItemId;
    public string ItemName;
    public ItemType ItemType;
    public WeaponType WeaponType;
    public int Durability;
    public WeaponRank WeaponRank;
    public int Power;
    public int AttackRangeMin; // 최소 공격 거리
    public int AttackRangeMax; // 최장 공격 거리
    public int Accuracy;
    public int Weight;
    public int Crit;
}

[Serializable]
public class ConsumableData
{
    public string ItemId;
    public string ItemName;
    public ItemType ItemType;
    public int Durability;
    public ConsumableType consumableType;
    public int effect;
}

[Serializable]
public class StaffData
{
    public string ItemId;
    public string ItemName;
    public ItemType itemType;
    public int Durability;
    public WeaponRank WeaponRank;
    public int Heal;
    public int RangeMin; // 최소 공격 거리
    public int RangeMax; // 최장 공격 거리
}

[System.Serializable]
public class ItemDB
{
    public List<WeaponData> weapons;
    public List<StaffData> staffs;
    public List<ConsumableData> consumables;
}

