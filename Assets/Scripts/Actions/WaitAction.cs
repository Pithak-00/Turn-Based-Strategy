using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction
{
    private int waitTimeSecond = 2;
    private float waitTime;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        waitTime += Time.deltaTime;

        if (waitTime >= waitTimeSecond)
        {
            ActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "Wait";
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
        waitTime = 0;
        ActionStart(onActionComplete);
    }
}
