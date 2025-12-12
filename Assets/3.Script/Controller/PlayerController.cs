using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//턴 관리자
public class PlayerController : BaseController
{
    private UnitController selectedUnit;

    private List<UnitController> playerUnits = new List<UnitController>();

    private bool canUserControl = false;

    private HashSet<Vector2Int> currentMovableTiles;

    private List<GameObject> attackTiles = new List<GameObject>();

    [SerializeField] private GameObject attackTilePrefab;

    [SerializeField] private GameObject turnEndPanel;

    private bool isTurnEndPanelOpen = false;

    public int defaultMinRange = 1;
    public int defaultMaxRange = 1;
    public override void StartTurn(Action callback)
    {
        base.StartTurn(callback);

        MovementRangeVisualizer.Instance.Clear();

        canUserControl = true;
        Debug.Log("Player Turn Started");

        foreach (var unit in playerUnits)
        {
            unit.hasActed = false;
            unit.renderer.color = Color.white;
        }
    }

    private void Update()
    {
        if (!GameFlowManager.Instance.isPlayerTurn)
            return;

        if (isTurnEndPanelOpen) //엔터
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                CloseTurnEndPanel();
                EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelAction();
                CloseTurnEndPanel();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OpenTurnEndPanel();
            return;
        }

        HandleUnitSelection();
        HandleUnitMovement();

        // 공격 입력
        HandleAttackInput();
    }

    private void CancelAction()
    {
        // 여기에 원하는 ‘돌아가기’ 로직
        // 예: 현재 선택 해제
        selectedUnit = null;

        // UI 리셋
        MovementRangeVisualizer.Instance.Clear();

        Debug.Log("행동 취소!");
    }

    private void OpenTurnEndPanel()
    {
        turnEndPanel.SetActive(true);
        isTurnEndPanelOpen = true;
    }

    private void CloseTurnEndPanel()
    {
        turnEndPanel.SetActive(false);
        isTurnEndPanelOpen = false;
    }

    public override void EndTurn()
    {
        canUserControl = false;
        base.EndTurn();
    }

    private void HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Vector3Int cell = GridManager.Instance.Map.WorldToCell(worldPos);
            Vector2Int tile = new Vector2Int(cell.x, cell.y);

            if (GridManager.Instance.unitMap.TryGetValue(tile, out UnitController unit))
            {
                if (unit.CompareTag("Player"))
                {
                    selectedUnit = unit;

                    // 이동 범위 계산은 반드시 tilePos 기준으로
                    currentMovableTiles =
                        Pathfinding.Instance.GetMovableTiles(selectedUnit.tilePos, selectedUnit.data.currentStats.Move);

                    MovementRangeVisualizer.Instance.ShowMoveRange(currentMovableTiles);
                }
            }
        }
    }

    private void HandleUnitMovement()
    {
        if (selectedUnit == null || selectedUnit.hasActed)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1)) // 우클릭으로 이동 테스트
        {
            // 1) 마우스를 타일 좌표로 변환
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Vector3Int cell = GridManager.Instance.Map.WorldToCell(worldPos);
            Vector2Int targetTile = new Vector2Int(cell.x, cell.y);

            // 2) 빈 타일인지 검사
            if (GridManager.Instance.unitMap.ContainsKey(targetTile))
            {
                Debug.Log("해당 타일에 유닛 있음");
                return;
            }

            // 3) 이동 가능 타일인지 검사
            if (!currentMovableTiles.Contains(targetTile))
            {
                Debug.Log("이동 불가능한 타일");
                return;
            }

            // 4) 이동 실행
            selectedUnit.MoveToTile(targetTile);

            MovementRangeVisualizer.Instance.Clear();

            // 이동 후 턴 종료 or 행동 선택
            StartCoroutine(EndAfterMove());
        }
    }

    private void HandleAttackInput()
    {
        int range = 0;

        // 선택된 유닛 없으면 공격 불가
        if (selectedUnit == null) return;
        if (selectedUnit.hasActed) return;

        // 왼쪽 클릭으로 공격 시도
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Vector3Int cell = GridManager.Instance.Map.WorldToCell(worldPos);
            Vector2Int tile = new Vector2Int(cell.x, cell.y);

            // 클릭한 곳에 유닛 있는지 검사
            if (GridManager.Instance.unitMap.TryGetValue(tile, out UnitController target))
            {
                if (selectedUnit.data.currentClassId.Equals("Mage")|| selectedUnit.data.currentClassId.Equals("Archer"))
                {
                    range = 1;
                }
                // 적이면 공격
                if (target.CompareTag("Enemy")&& Math.Abs(Vector3.Distance(selectedUnit.transform.position, target.transform.position)) < 2 + range)
                {
                    Debug.Log($"공격 성공! {target.name} 공격");
                    selectedUnit.Attack(target);

                    selectedUnit.hasActed = true;
                    selectedUnit = null;

                    CheckAllUnitsActed();
                }
                else
                {
                    Debug.Log("아군이어서 공격 불가");
                }
            }
            else
            {
                Debug.Log("클릭한 칸에 유닛 없음");
            }
        }
    }
    private IEnumerator EndAfterMove()
    {
        yield return new WaitForEndOfFrame();

    }

    public void SetPlayerUnits(List<UnitController> playerUnits)
    {
        this.playerUnits = playerUnits;
    }

    private void CheckAllUnitsActed()
    {
        foreach (var units in playerUnits)
        {

            if (!units.hasActed)
            {
                return;
            }

        }
        EndTurn();
    }


}
