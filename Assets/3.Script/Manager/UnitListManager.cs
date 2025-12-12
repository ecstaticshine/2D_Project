using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class UnitListManager : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private UnitPrefab itemPrefab;

    private List<UnitPrefab> allUnits = new List<UnitPrefab>();
    private List<UnitWarData> warUnits = new List<UnitWarData>();

    [Header("출전 유닛 카운트용")]
    [SerializeField] private Text RemainUnitCount;
    [SerializeField] private Text unitSelectedCount;
    [SerializeField] private Text MaxUnits;

    [SerializeField] UnitInfoManager unitInfoPanel;

    private readonly int maxUnitCount = 11;
    private int unitSelected = 0;
    private void CreateList()
    {
        foreach (var unitData in SaveManager.Instance.currentSaveData.CharacterData)
        {
            UnitPrefab unit = Instantiate(itemPrefab, contentRoot);
            Sprite portrait = LoadMapUnits(unitData.unitId);

            UnitWarData unitWardata = new UnitWarData();
            unitWardata.unitId = unitData.unitId;
            if (unitSelected < maxUnitCount)
            {
                ++unitSelected;
                unitWardata.isSelected = true;
                warUnits.Add(unitWardata);

                unitData.PosX = 0;
                unitData.PosY = 0;

               // Debug.Log($"[초기출전] {unitData.unitId} 좌표: {unitData.PosX},{unitData.PosY}");

            }
            else
            {
                unitWardata.isSelected = false;

                unitData.PosX = -1;
                unitData.PosY = -1;

               // Debug.Log($"[초기비출전] {unitData.unitId} 좌표: {unitData.PosX},{unitData.PosY}");
            }

            allUnits.Add(unit);

            unit.SetUp(portrait, unitData.unitName, unitWardata, this);
        }
    }

    private void Start()
    {
        CreateList();
        UpdateUI();
    }

    public void CheckUnitSelected(UnitWarData target)
    {
        var saveUnit = SaveManager.Instance.currentSaveData.CharacterData
            .Find(unit => unit.unitId == target.unitId);

        int unitSelected = warUnits.Count(unit => unit.isSelected);

        if (!target.isSelected)
        {

            if (unitSelected >= maxUnitCount)
            {
                Debug.Log("더 이상 선택할 수 없엉");
                return;
            }
            target.isSelected = true;
            warUnits.Add(target);

            // 선택된 캐릭터는 일단 출전한다는 식으로 설정.
            saveUnit.PosX = 0;
            saveUnit.PosY = 0;
        }
        else
        {
            target.isSelected = false;
            warUnits.Remove(target);
            // 선택 안된 캐릭터는 일단 출전하지 않는다고 설정.
            saveUnit.PosX = -1;
            saveUnit.PosY = -1;

        }
        unitInfoPanel.showUnitInfo(target.unitId);
        UpdateUI();
    }


    private Sprite LoadMapUnits(string unitId)
    {
        var sprite  = Resources.Load<Sprite>($"MapUnits/{unitId}");

        return sprite;

    }



    private void UpdateUI()
    {
        int unitSelected = warUnits.Count(unit => unit.isSelected);

        RemainUnitCount.text = $"{maxUnitCount - unitSelected}";
        unitSelectedCount.text = $"{unitSelected}";
        MaxUnits.text = $"{maxUnitCount}";

        foreach (var unit in allUnits)
        {
            unit.Refresh();
        }
    }

    public void SaveSelectedUnits()
    {
        foreach (var saveData in warUnits)
        {
            //UnitSaveData 안에 isSelected 반영
            var warData = warUnits.Find(w => w.unitId == saveData.unitId);
            saveData.isSelected = (warData != null);
        }

        //SaveManager.Instance.SaveGame();
    }
}
