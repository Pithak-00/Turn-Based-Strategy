using System;
using UnityEngine;
using UniRx;
using Command;

namespace Member
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForEnemyTurn,
            TakingTurn,
            Busy,
        }

        private State state;
        private float timer;

        private void Awake()
        {
            state = State.WaitingForEnemyTurn;
        }

        private void Start()
        {
            TurnSystem.Instance.OnTurnChanged.Subscribe(_ => OnTurnChanged());
        }

        private void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn())
            {
                return;
            }

            switch (state)
            {
                case State.WaitingForEnemyTurn:
                    break;
                case State.TakingTurn:
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            state = State.Busy;
                        }
                        else
                        {
                            //敵のターン終了
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                    break;
                case State.Busy:
                    break;
            }
        }

        private void SetStateTakingTurn()
        {
            timer = 0.5f;
            state = State.TakingTurn;
        }

        private void OnTurnChanged()
        {
            if (!TurnSystem.Instance.IsPlayerTurn())
            {
                state = State.TakingTurn;
                timer = 2f;
            }
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
        {
            foreach (MemberCharacter enemyUnit in MemberManager.Instance.GetEnemyMemberList())
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryTakeEnemyAIAction(MemberCharacter enemyMember, Action onEnemyAIActionComplete)
        {
            EnemyAIAction bestEnemyAIAction = null;
            BaseCommand bestBaseCommand = null;

            foreach (BaseCommand baseCommand in enemyMember.GetBaseCommandArray())
            {
                if (!enemyMember.CanSpendActionPointsToTakeAction(baseCommand))
                {
                    //行動不可能
                    continue;
                }

                if (bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseCommand.GetBestEnemyAIAction();
                    bestBaseCommand = baseCommand;
                }
                else
                {
                    EnemyAIAction testEnemyAIAction = baseCommand.GetBestEnemyAIAction();
                    if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                    {
                        bestEnemyAIAction = testEnemyAIAction;
                        bestBaseCommand = baseCommand;
                    }
                }
            }

            if (bestEnemyAIAction != null && enemyMember.TrySpendActionPointsToTakeAction(bestBaseCommand))
            {
                bestBaseCommand.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}