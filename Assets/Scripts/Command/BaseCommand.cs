using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Member;
using Grid;

namespace Command
{
    public abstract class BaseCommand : MonoBehaviour
    {
        public ISubject<BaseCommand> OnStartCommand = new Subject<BaseCommand>(); //コマンド開始イベント
        public ISubject<BaseCommand> OnEndCommand = new Subject<BaseCommand>(); //コマンド終了イベント

        protected MemberCharacter member;
        protected bool isActive;
        protected Action onActionComplete;

        protected virtual void Awake()
        {
            member = GetComponent<MemberCharacter>();
        }

        //コマンド名取得
        public abstract string GetCommandName();

        //アクション実行
        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        //コマンド実行できるグリッドかどうか
        public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }

        //コマンド実行できるグリッドリスト
        public abstract List<GridPosition> GetValidActionGridPositionList();

        //アクションポイント消費値
        public virtual int GetActionPointsCost()
        {
            return 1;
        }

        //コマンド開始
        protected void ActionStart(Action onActionComplete)
        {
            isActive = true;
            this.onActionComplete = onActionComplete;

            OnStartCommand.OnNext(this);
        }

        //コマンド終了
        protected void ActionComplete()
        {
            isActive = false;
            onActionComplete();

            OnEndCommand.OnNext(this);
        }

        //メンバー
        public MemberCharacter GetMember()
        {
            return member;
        }

        //敵の優先アクション
        public EnemyAIAction GetBestEnemyAIAction()
        {
            List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

            List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (GridPosition gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)
            {
                enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
                return enemyAIActionList[0];
            }
            else
            {
                //可能なアクションがない
                return null;
            }
        }

        //敵のアクション
        public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    }
}