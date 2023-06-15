using System;
using System.Collections.Generic;
using UnityEngine;
using Member;
using Grid;
using UniRx;

namespace Command
{
    public class AttackCommand : BaseCommand
    {
        public static ISubject<Unit> OnEndAttack => new Subject<Unit>();

        [SerializeField] private int actionPointsCost;
        [SerializeField] private int maxDistance;
        [SerializeField] private int damagePoint;

        private enum State
        {
            BeforeHit,
            AfterHit,
        }

        private State state;
        private float stateTimer;
        private MemberCharacter targetUnit;

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.BeforeHit:
                    Vector3 aimDir = (targetUnit.GetWorldPosition() - member.GetWorldPosition()).normalized;
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                case State.AfterHit:
                    break;
            }

            if (stateTimer <= 0f)
            {
                NextState();
            }
        }

        private void NextState()
        {
            switch (state)
            {
                case State.BeforeHit:
                    state = State.AfterHit;
                    float afterHitStateTime = 0.5f;
                    stateTimer = afterHitStateTime;
                    targetUnit.Damage(damagePoint);
                    break;
                case State.AfterHit:
                    ActionComplete();
                    OnEndAttack.OnNext(Unit.Default);
                    break;
            }
        }

        public override string GetCommandName()
        {
            return "攻撃";
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 200,
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

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // その位置には空
                        continue;
                    }

                    MemberCharacter targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    if (targetUnit.IsEnemy() == member.IsEnemy())
                    {
                        // 敵の仲間
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            state = State.BeforeHit;
            float beforeHitStateTime = 0.7f;
            stateTimer = beforeHitStateTime;

            ActionStart(onActionComplete);
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
