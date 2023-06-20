using System;
using System.Collections.Generic;
using UnityEngine;
using Grid;

namespace Command
{
    public class MagicCommand : BaseCommand
    {
        [SerializeField] private Transform magicProjectilePrefab;

        [SerializeField] private int actionPointsCost;
        [SerializeField] private int maxDistance;


        private void Update()
        {
            if (!isActive)
            {
                return;
            }
        }

        public override string GetCommandName()
        {
            return "爆弾の魔法";
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 200
            };
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = member.GetGridPosition();

            for (int x = -maxDistance; x <= maxDistance; x++)
            {
                for (int z = -maxDistance; z <= maxDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxDistance)
                    {
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            Transform magicObjectTransform = Instantiate(magicProjectilePrefab, member.GetWorldPosition(), Quaternion.identity);
            MagicObject magicObject = magicObjectTransform.GetComponent<MagicObject>();
            magicObject.Setup(gridPosition, OnMagicBehaviourComplete);

            ActionStart(onActionComplete);
        }

        private void OnMagicBehaviourComplete()
        {
            ActionComplete();
        }

        public override int GetActionPointsCost()
        {
            return actionPointsCost;
        }

        public int GetMaxDistance()
        {
            return maxDistance;
        }
    }
}