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
        public static ISubject<Unit> OnEndAttack = new Subject<Unit>();

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
        private MemberCharacter targetMember;

        [SerializeField] private LayerMask obstaclesLayerMask;

        private int maxAttackDistance = 2;
        private bool canAttack;

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
                    Vector3 aimDir = (targetMember.GetWorldPosition() - member.GetWorldPosition()).normalized;
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
                    targetMember.Damage(damagePoint);
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
            MemberCharacter targetMember = LevelGrid.Instance.GetMemberAtGridPosition(gridPosition);

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - targetMember.GetHealthNormalized()) * 100f),
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

                    if (!LevelGrid.Instance.HasAnyMemberOnGridPosition(testGridPosition))
                    {
                        // その位置には空
                        continue;
                    }

                    MemberCharacter targetUnit = LevelGrid.Instance.GetMemberAtGridPosition(testGridPosition);

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
            targetMember = LevelGrid.Instance.GetMemberAtGridPosition(gridPosition);

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

        public List<GridPosition> GetValidActionGridPositionList(GridPosition memberGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
            {
                for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = memberGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxAttackDistance)
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyMemberOnGridPosition(testGridPosition))
                    {
                        // その位置には空
                        continue;
                    }

                    MemberCharacter targetMember = LevelGrid.Instance.GetMemberAtGridPosition(testGridPosition);

                    if (targetMember.IsEnemy() == member.IsEnemy())
                    {
                        // 敵の仲間
                        continue;
                    }

                    Vector3 memberWorldPosition = LevelGrid.Instance.GetWorldPosition(memberGridPosition);
                    Vector3 attackDir = (targetMember.GetWorldPosition() - member.GetWorldPosition()).normalized;

                    float memberShoulderHeight = 1.7f;
                    if (Physics.Raycast(
                        memberWorldPosition + Vector3.up * memberShoulderHeight,
                        attackDir,
                        Vector3.Distance(memberWorldPosition, targetMember.GetWorldPosition()),
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

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }
    }
}
