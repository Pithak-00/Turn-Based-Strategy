using System;
using System.Collections.Generic;
using UnityEngine;
using Member;
using Grid;

namespace Command
{
    public class HealCommand : BaseCommand
    {
        [SerializeField] private int actionPointsCost;
        [SerializeField] private int maxDistance;
        [SerializeField] private int healPoint;

        private enum State
        {
            Aiming,
            Casting,
            Cooloff,
        }

        [SerializeField] private LayerMask obstaclesLayerMask;

        private State state;
        private float stateTimer;
        private MemberCharacter targetMember;
        private bool canHeal;

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.Aiming:
                    Vector3 aimDir = (targetMember.GetWorldPosition() - member.GetWorldPosition()).normalized;
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                case State.Casting:
                    if (canHeal)
                    {
                        Heal();
                        canHeal = false;
                    }
                    break;
                case State.Cooloff:

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
                case State.Aiming:
                    state = State.Casting;
                    float castingStateTime = 0.1f;
                    stateTimer = castingStateTime;
                    break;
                case State.Casting:
                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                    break;
                case State.Cooloff:
                    ActionComplete();
                    break;
            }
        }

        private void Heal()
        {
            targetMember.Heal(healPoint);
        }

        public override string GetCommandName()
        {
            return "ヒール";
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = member.GetGridPosition();
            return GetValidActionGridPositionList(unitGridPosition);
        }

        public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

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

                    if (!LevelGrid.Instance.HasAnyMemberOnGridPosition(testGridPosition))
                    {
                        // その位置には空
                        continue;
                    }

                    MemberCharacter targetMember = LevelGrid.Instance.GetMemberAtGridPosition(testGridPosition);

                    if (targetMember.IsEnemy() != member.IsEnemy())
                    {
                        // 仲間同士ではない
                        continue;
                    }

                    if (targetMember.GetHealthNormalized() == 1)
                    {
                        // HPがマックス
                        continue;
                    }

                    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDir = (targetMember.GetWorldPosition() - member.GetWorldPosition()).normalized;

                    float unitShoulderHeight = 1.7f;
                    if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDir,
                        Vector3.Distance(unitWorldPosition, targetMember.GetWorldPosition()),
                        obstaclesLayerMask))
                    {
                        //障害物により、ブロック
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            targetMember = LevelGrid.Instance.GetMemberAtGridPosition(gridPosition);

            state = State.Aiming;
            float aimingStateTime = 1f;
            stateTimer = aimingStateTime;

            canHeal = true;

            ActionStart(onActionComplete);
        }

        public MemberCharacter GetTargetMember()
        {
            return targetMember;
        }

        public int GetMaxHealDistance()
        {
            return maxDistance;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            MemberCharacter targetMember = LevelGrid.Instance.GetMemberAtGridPosition(gridPosition);
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 200 + Mathf.RoundToInt((1 - targetMember.GetHealthNormalized()) * 100f),
            };
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }
    }
}
