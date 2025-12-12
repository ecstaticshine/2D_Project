using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class UnitInfoManager : MonoBehaviour
{
    [Header("선택한 캐릭터 정보용")]
    [SerializeField] private Image unitPortrait;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text unitLv;
    [SerializeField] private TMP_Text unitExp;

    [SerializeField] Transform weaponRoot;
    [SerializeField] InventorySlotUI itemPrefab;
    [SerializeField] private GameObject UnitInfo;

    private List<InventorySlotUI> currentSlots = new List<InventorySlotUI>();


    public void showUnitInfo(string unitIdName)
    {
        if (!UnitInfo.activeSelf)
        {
            UnitInfo.SetActive(true);
        }
        unitPortrait.sprite = LoadPortrait(unitIdName);
        UnitSaveData data = SaveManager.Instance.currentSaveData.CharacterData.Find(unit => unit.unitId.Equals(unitIdName));
        unitName.text = data.unitName;
        unitLv.text = $"{data.level}";
        unitExp.text = $"{data.exp}";

        ResetInventory();
        ShowInventory(data.Inventory);
    }

    private Sprite LoadPortrait(string unitId)
    {
        var sprite = Resources.Load<Sprite>($"Portraits/{unitId}");

        return sprite;

    }

    private Sprite LoadItemIcon(string unitId)
    {
        var sprite = Resources.Load<Sprite>($"ItemIcons/{unitId}");
        return sprite;

    }

    private void ResetInventory()
    {
        foreach (Transform child in weaponRoot)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void ShowInventory(List<Item> inventory)
    {
        foreach (var slot in currentSlots)
        {
            Destroy(slot.gameObject);
        }
        currentSlots.Clear();

        foreach (var item in inventory)
        {
            var data = DatabaseManager.Instance.GetItem(item.itemId);

            if (data is WeaponData weapon)
            {
                AddInventorySlot(weapon.ItemName, LoadItemIcon(weapon.ItemId), item.Durability, weapon.Durability);
            }
            else if (data is StaffData staff)
            {
                AddInventorySlot(staff.ItemName, LoadItemIcon(staff.ItemId), item.Durability, staff.Durability);
            }
            else if(data is ConsumableData consumable)
            {
                AddInventorySlot(consumable.ItemName, LoadItemIcon(consumable.ItemId), item.Durability, consumable.Durability);
            }
        }
    }

    private void AddInventorySlot(string ItemName, Sprite itemICon, int durability, int maxDurability)
    {
        InventorySlotUI slot = Instantiate(itemPrefab, weaponRoot);
        slot.Setup(ItemName, itemICon, durability, maxDurability);
    }
}

