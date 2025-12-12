using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private TurnUIManager turnUI;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private GridManager gridManager;

    public static GameFlowManager Instance;

    private int turnCount = 1;
    public bool isPlayerTurn = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        // 스폰매니져의 모든 유닛 스폰
        spawnManager.SpawnAllUnits();
        // 그리고 플레이 턴이 되기에
        yield return StartPlayerTurn();
    }

    private IEnumerator StartPlayerTurn()
    {
        isPlayerTurn = true;

        // FE 스타일 UI 연출 (Player Phase)
        yield return turnUI.ShowTurnText("Player Phase");
        playerController.SetPlayerUnits(spawnManager.playerUnits);

        // 플레이어에게 턴 제어권 넘김
        playerController.StartTurn(OnPlayerTurnFinished);
    }

    private void OnPlayerTurnFinished()
    {
        // PlayerController에서 호출됨
        StartCoroutine(StartEnemyTurn());
    }


    private IEnumerator StartEnemyTurn()
    {
        isPlayerTurn = false;

        // FE 스타일 Enemy Phase 연출
        yield return turnUI.ShowTurnText("Enemy Phase");

        // 적 턴 시작
        enemyController.StartTurn(OnEnemyTurnFinished);
    }

    private void OnEnemyTurnFinished()
    {
        // 적 턴 끝 → 다음 턴으로
        turnCount++;
        StartCoroutine(StartPlayerTurn());
    }

}
