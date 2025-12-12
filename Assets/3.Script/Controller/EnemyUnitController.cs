using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitController
{

    [Header("Enemy Info")]
    public string unitId;
    public string unitName = "라우스병";
    public int level = 1;
    public int maxHp = 20;

    private GridManager grid;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        grid = FindAnyObjectByType<GridManager>();
    }

    public void Initialize(string id, int lv, int hp)
    {
        this.unitId = id;
        this.level = lv;
        this.currentHp = hp;
        this.maxHp = hp;

        currentStats = new Stats
        {
            Power = 8 + lv * 2,     // 공격력
            Skill = 5 + lv,         // 명중
            Spd = 4 + lv,           // 회피
            Def = 3 + lv,           // 방어
            Res = 1 + lv,           // 마방
            MAXHP = hp,             // HP
        };

        currentHp = hp;


        // 애니메이터 로드 (Resources/MapAnimators/폴더)
        var controller =
            Resources.Load<RuntimeAnimatorController>($"MapEnemyAnimators/{unitId}");

        animator.runtimeAnimatorController = controller;
    }

    public void TakeTurn()
    {
        Debug.Log($"Enemy tilePos = {tilePos}");
        // 간단한 AI: 인접한 아군 공격
        UnitController target = GetAdjacentPlayer();

        if (target != null)
        {
            Attack(target);
            hasActed = true;
            return;
        }

        // 인접 아군 없으면 대기
        hasActed = true;
    }

    private UnitController GetAdjacentPlayer()
    {
        Vector2Int[] dirs =
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };
        foreach (var dir in dirs)
        {
            Vector2Int p = tilePos + dir;

            if (!GridManager.Instance.IsInsideGrid(p))
                continue;

            if (GridManager.Instance.unitMap.TryGetValue(p, out UnitController unit))
            {
                if (unit != null && unit.CompareTag("Player"))
                    return unit as EnemyUnitController;
            }
        }

        return null;
    }

    public IEnumerator Act()
    {
        Debug.Log($"{name} 행동 시작!");

        yield return renderer.color = Color.darkRed;

        yield return new WaitForSeconds(0.5f);


    }
}
