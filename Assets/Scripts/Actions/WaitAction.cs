using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction
{
    [SerializeField] private int actionPointsCost;
    [SerializeField] private int maxDistance;

    private int waitTimeSecond = 2;
    private float waitTimer;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        waitTimer += Time.deltaTime;

        if (waitTimer >= waitTimeSecond)
        {
            ActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "待機";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        waitTimer = 0;
        ActionStart(onActionComplete);
    }

    public override int GetActionPointsCost()
    {
        return actionPointsCost;
    }
}
