using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab; // 유닛 프리팹
    [SerializeField] private GameObject enemyPrefab; // 적군 프리팹
    [SerializeField] private Transform unitPrefabParents; // 유닛 프리팹을 담을 곳
    [SerializeField] private Transform enemyPrefabParents; // 유닛 프리팹을 담을 곳
    [SerializeField] private Grid grid; // 타일맵의 Grid 컴포넌트
    [SerializeField] private EnemyController enemyController;

    public List<UnitController> playerUnits = new List<UnitController>();


    private void Start()
    {
        // 아군 스폰
        SpawnAllUnits();
        // 적군 스폰
        SpawnEnemies();
    }

    public void SpawnAllUnits()
    {
        SaveData savedata = SaveManager.Instance.currentSaveData;
        int index = 0;
        foreach(var unitData in savedata.CharacterData)
        {

            if (unitData.PosX < 0 || unitData.PosY < 0)
                continue;

            if (unitData.PosX == 0 && unitData.PosY == 0)
            {
                int row = index / 6; // y 좌표 6명씩 한 줄에 배치
                int col = index % 6; // x 좌표

                unitData.PosX = col - 7;      // x 좌표
                unitData.PosY = row + 5;      // y 좌표 (0줄, 1줄)

                index++;
                //    Debug.Log($"{unitData.PosX}, { unitData.PosY}");
            }
            SpawnUnit(unitPrefab, unitData.unitId, unitData.PosX, unitData.PosY, unitData);
        }
    }


    public void SpawnEnemies()
    {
        SpawnEnemy("Archer", -5, 10, 3);
        SpawnEnemy("Archer", -3, 10, 2);
        SpawnEnemy("ArmorKnight", -4, 11, 3);
        SpawnEnemy("Archer", 0, 10, 3);
        SpawnEnemy("Archer", 0, 12, 3);
        SpawnEnemy("Archer", -10, 11, 3);
        SpawnEnemy("Archer", 0, 17, 3);
        SpawnEnemy("Archer", -1, 18, 3);
        SpawnEnemy("Archer", -12, 17, 3);
        SpawnEnemy("ArmorKnight", -11, 18, 3);
        SpawnEnemy("Archer", -13, 21, 3);
        SpawnEnemy("Archer", -5, 24, 3);
        SpawnEnemy("Archer", -14, 26, 3);
        SpawnEnemy("Archer", -15, 27, 3);
        SpawnEnemy("Archer", -8, 31, 3);
        SpawnEnemy("Archer", -7, 31, 3);
        SpawnEnemy("Archer", -6, 31, 3);
        SpawnEnemy("ArmorKnight", -7, 30, 3);
        SpawnEnemy("ArmorKnight", -2, 30, 3);
        SpawnEnemy("ArmorKnight", 1, 30, 3);
        SpawnEnemy("ArmorKnight", -1, 28, 3);
        SpawnEnemy("ArmorKnight", -1, 31, 3);

    }

    public void SpawnEnemy(string id, int unitPosX, int unitPosY, int level)
    {
        Vector2Int tile = new Vector2Int(unitPosX, unitPosY);
        Vector3 worldPos = GridManager.Instance.TileToWorld(tile);

        GameObject enemyObject = Instantiate(enemyPrefab, worldPos, Quaternion.identity, enemyPrefabParents);

        EnemyUnitController enemyUnitController = enemyObject.GetComponent<EnemyUnitController>();
        enemyUnitController.Initialize(id, level, 20 + level * 2);

        Vector3Int cell = GridManager.Instance.Map.WorldToCell(worldPos);

        enemyUnitController.tilePos = tile;
        GridManager.Instance.unitMap[tile] = enemyUnitController;

        enemyController.enemyUnits.Add(enemyUnitController);
    }


    private void SpawnUnit(GameObject prefab, string unitId, int unitPosX, int unitPosY, UnitSaveData unit)
    {

        Vector2Int tile = new Vector2Int(unitPosX, unitPosY);

        //  타일 → 월드
        Vector3 worldPos = GridManager.Instance.TileToWorld(tile);

        GameObject unitObject = Instantiate(prefab, worldPos, Quaternion.identity, unitPrefabParents);
        UnitController controller = unitObject.GetComponent<UnitController>();

        Vector3Int cell = GridManager.Instance.Map.WorldToCell(worldPos);

        controller.tilePos = tile;
        GridManager.Instance.unitMap[tile] = controller;

        controller.Init(unit);

        playerUnits.Add(controller);
    }
}


