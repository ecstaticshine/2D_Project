using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected System.Action onTurnFinished;

    // 오버라이드로 PlayerController는 캐릭터 조작
    // EnemyController는 자동으로 조작
    public virtual void StartTurn(System.Action callback)
    {
        onTurnFinished = callback;
    }

    //EnemyController랑 PlayerController 둘 다 사용
    public virtual void EndTurn()
    {
        onTurnFinished?.Invoke();
    }
}
