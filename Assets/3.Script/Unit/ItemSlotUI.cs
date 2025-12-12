using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text ItemNameText;
    public TMP_Text DurabilityText;
    public TMP_Text MaxDurabilityText;

    public void Setup(string ItemName, Sprite ItemICon, int durability ,int maxDurability)
    {
        icon.sprite = ItemICon;
        ItemNameText.text = ItemName;
        DurabilityText.text = $"{durability}";
        MaxDurabilityText.text = $"{maxDurability}";
    }
}
