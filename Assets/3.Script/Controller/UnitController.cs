using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 유닛의 공통 부모
public class UnitController : MonoBehaviour
{
    public UnitSaveData data;
    protected Animator animator;
    public SpriteRenderer renderer;
    public bool hasActed = false;

    public Stats currentStats;
    public int currentHp;

    public Vector2Int tilePos { get; set; }

    private int SizeX, SizeY; // 전체 타일 사이즈



    public void Init(UnitSaveData unitsaveData)
    {
        data = unitsaveData;

        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        // 플레이어 스탯 적용
        currentStats = unitsaveData.currentStats;
        currentHp = unitsaveData.currentHp;

        // 애니메이터 로드 (Resources/MapAnimators/폴더)
        var controller =
            Resources.Load<RuntimeAnimatorController>($"MapAnimators/{data.unitId}");

        if (controller != null)
        {
            animator.runtimeAnimatorController = controller;
        }
        else
        {
            Debug.LogError($"{data.unitId}용 AnimatorController 없음!");
        }
    }


    //유닛이 이동리스트를 가지고 움직여야 한다.
    public void MoveToTile(Vector2Int targetTile)
    {
        var path = Pathfinding.Instance.FindPath(
                tilePos,
                targetTile
        );

        if (path == null)
        {
            Debug.Log("길 없음");
            return;
        }

        StartCoroutine(MovePathCoroutine(path));

    }

    private IEnumerator MovePathCoroutine(List<Vector2Int> path)
    {
        GridManager.Instance.unitMap.Remove(tilePos);

        foreach (var moveOneTile in path)
        {

            Vector3 targetPosition =
                GridManager.Instance.Map.GetCellCenterWorld(
                    new Vector3Int(moveOneTile.x, moveOneTile.y, 0)
                );

            SetMoveAnimation(moveOneTile);

            while ((transform.position - targetPosition).sqrMagnitude > 0.01f)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3f);
                yield return null;
            }
        }
        animator.Play($"Base Layer.{data.unitId}");

        //  타일 중심에서 정확한 셀 좌표 얻기
        Vector3Int cell = GridManager.Instance.Map.WorldToCell(transform.position);
        tilePos = new Vector2Int(cell.x, cell.y);
        GridManager.Instance.unitMap[tilePos] = this;

        Debug.Log($"Move End: transform = {transform.position}, tilePos = {tilePos}");

        yield return renderer.color = Color.darkGray;
    }


    private void SetMoveAnimation(Vector2Int targetTile)
    {
        Vector3Int cell = GridManager.Instance.Map.WorldToCell(transform.position);
        Vector2Int onCurrentTile = new Vector2Int(cell.x, cell.y);

        Vector2Int direction = targetTile - onCurrentTile;

        if (direction.x > 0)
        {
            animator.Play($"Base Layer.{data.unitId}_Side"); renderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            animator.Play($"Base Layer.{data.unitId}_Side"); renderer.flipX = false;
        }
        else if (direction.y > 0)
        {
            animator.Play($"Base Layer.{data.unitId}_Up");
        }
        else
        {
            animator.Play($"Base Layer.{data.unitId}_Down");
        }
    }


    public EnemyUnitController GetAdjacentEnemy()
    {
        Vector2Int[] dirs = {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1)
    };

        foreach (var dir in dirs)
        {
            Vector2Int checkPos = tilePos + dir;
            if (GridManager.Instance.unitMap.TryGetValue(checkPos, out UnitController unit))
            {
                if (unit.CompareTag("Enemy"))
                    return (EnemyUnitController)unit;
            }
        }
        return null;
    }


    public void Attack(UnitController target)
    {
        int damage = 0;

        // Player가 공격하는 경우 (무기 있음)
        if (data != null && data.Inventory.Count > 0)
        {
            var itemData = DatabaseManager.Instance.GetItem(data.Inventory[0].itemId);

            if (itemData is WeaponData weapon)
                damage = data.currentStats.Power + weapon.Power;

            Debug.Log(target.currentStats.Power);

            target.currentHp -= damage;
            Debug.Log($"{name} → {target.name} 에게 {damage} 데미지!");

            if (Math.Abs(Vector3.Distance(this.transform.position, target.transform.position)) > 1.3)
            {
                if (target.currentHp > 0)
                {
                    this.currentHp -= target.currentStats.Power;
                    data.currentHp = currentHp;
                }
            }

            if (this.currentHp <= 0)
            {
                this.Die();
            }
        }
        else
        {
            // Enemy가 공격하는 경우 (하드코딩 스탯)
            damage = currentStats.Power;
        }



        Vector2Int pos = target.tilePos;


        if (target.currentHp <= 0)
        {
            if (GridManager.Instance.unitMap.ContainsKey(pos))
            {
                GridManager.Instance.unitMap.Remove(pos);
            }

            target.Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject, 0.6f);
    }
}
