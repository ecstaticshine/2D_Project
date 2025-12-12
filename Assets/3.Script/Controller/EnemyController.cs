using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : BaseController
{
    public Vector2Int tilePos { get; set; }

    public List<EnemyUnitController> enemyUnits = new List<EnemyUnitController>();

    public override void StartTurn(Action callback)
    {
        base.StartTurn(callback);
        StartCoroutine(RunAI());

    }

    private IEnumerator RunAI()
    {
        foreach (var enemy in enemyUnits)
        {
            if (enemy == null || enemy.currentHp <= 0)
                continue;

            enemy.hasActed = false;
            enemy.renderer.color = Color.red;

            yield return StartCoroutine(enemy.Act());
        }

        if (AreAllEnemiesDead())
        {
            SceneManager.LoadScene("Clear");
            yield break;
        }

        EndTurn();
    }

    public bool hasActed = false;


    public override void EndTurn()
    {
        base.EndTurn();
    }

    private bool AreAllEnemiesDead()
    {
        foreach (var enemy in enemyUnits)
        {
            if (enemy != null && enemy.currentHp > 0)
                return false;
        }
        return true;
    }
}


